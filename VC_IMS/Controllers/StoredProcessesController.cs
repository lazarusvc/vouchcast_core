using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using OfficeOpenXml;
using System.Data;
using System.Security.Claims;
using System.Text.Json;
using VC_IMS.Data;
using VC_IMS.Models;
using VC_IMS.Models.ViewModels;
using VC_IMS.Services;

namespace VC_IMS.Controllers
{
    public class StoredProcessesController : Controller
    {
        private readonly VC_IMSDb_moreContext _db;
        private readonly StoredProcedureRunner _runner;

        public StoredProcessesController(
            VC_IMSDb_moreContext db,
            StoredProcedureRunner runner)
        {
            _db = db;
            _runner = runner;
        }

        // GET: /StoredProcesses
        public async Task<IActionResult> Index()
        {
            var procs = await _db.VC_storedProcesses
                                 .AsNoTracking()
                                 .OrderBy(x => x.Name)
                                 .ToListAsync();
            return View(procs);
        }

        // GET: /StoredProcesses/Run/5
        [HttpGet]
        public async Task<IActionResult> Run(int id, int? formId = null, int? orgId = null)
        {
            var sp = await _db.VC_storedProcesses
                              .Include(x => x.Params)
                              .FirstOrDefaultAsync(x => x.Id == id);
            if (sp is null) return NotFound();

            var vm = new RunStoredProcessViewModel
            {
                ProcessId = sp.Id,
                Name = sp.Name,
                Description = sp.Description,
                ConnectionDisplay = !string.IsNullOrWhiteSpace(sp.ConnectionKey)
                    ? $"Connection: {sp.ConnectionKey}"
                    : $"{sp.DataSource}/{sp.Database}",
                Params = sp.Params
                           .OrderBy(p => p.Key)
                           .Select(p => new RunParamViewModel
                           {
                               Id = p.Id,
                               Key = p.Key,
                               DataType = p.DataType,
                               Value = p.Value
                           })
                           .ToList()
            };
            return View(vm);
        }

        // POST: /StoredProcesses/Run/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Run(int id, RunStoredProcessViewModel model, int? formId = null, int? orgId = null)
        {
            var sp = await _db.VC_storedProcesses.Include(x => x.Params).FirstOrDefaultAsync(x => x.Id == id);
            if (sp is null) return NotFound();

            // persist edited values
            var map = sp.Params.ToDictionary(p => p.Id);
            foreach (var p in model.Params)
                if (map.TryGetValue(p.Id, out var row)) row.Value = p.Value;
            await _db.SaveChangesAsync();

            // --- uuid-aware tokenization (unchanged) ---
            var uid = Request.Query["uid"].FirstOrDefault()
                   ?? Request.Form["uid"].FirstOrDefault()
                   ?? Request.Query["uuid"].FirstOrDefault()
                   ?? Request.Query["UID"].FirstOrDefault();

            var ctx = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["FormId"] = formId?.ToString() ?? string.Empty,
                ["OrganizationId"] = orgId?.ToString() ?? string.Empty,
                ["FormUUID"] = uid ?? string.Empty,
                ["UserName"] = User?.Identity?.Name ?? "system"
            };
            var tokenizedParams = ApplyTokens(sp.Params, ctx);

            // ✅ Stash context so Export (GET) doesn’t read Request.Form
            TempData["uid"] = uid ?? string.Empty;
            TempData["formId"] = formId?.ToString() ?? string.Empty;
            TempData["orgId"] = orgId?.ToString() ?? string.Empty;
            TempData.Keep();

            var (table, error) = await _runner.ExecuteAsync(sp, tokenizedParams);

            var hasError = !string.IsNullOrWhiteSpace(error);
            string subject;
            string body;
            string? errorSummary = null;

            if (!hasError)
            {
                subject = "Stored procedure executed successfully";
                body = $"Stored process '{sp.Name}' executed successfully.";
            }
            else
            {
                // Take only the first line / prefix of the error, so we don't spam the notif.
                var firstLine = error
                    .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault() ?? "Unknown error";

                // Trim to a reasonable length for the notification.
                errorSummary = firstLine.Length > 160 ? firstLine[..160] + "…" : firstLine;

                subject = "Stored procedure execution failed";
                body = $"Stored process '{sp.Name}' failed: {errorSummary}";
            }

            //// 🔔 Notify: Stored procedure executed (with success/failure info)
            //await NotifyStoredProcAsync(
            //    subject: subject,
            //    body: body,
            //    metadata: new
            //    {
            //        action = "StoredProcessRun",
            //        processId = sp.Id,
            //        processName = sp.Name,
            //        formId,
            //        orgId,
            //        uid,
            //        hasError,
            //        errorSummary
            //    },
            //    ct: HttpContext.RequestAborted);

            // make context available to the RunResult view so Export can include it
            ViewBag.uid = uid;
            ViewBag.formId = formId;
            ViewBag.orgId = orgId;

            return View("RunResult", new RunStoredProcessResultViewModel
            {
                ProcessId = id,
                Name = sp.Name,
                Description = sp.Description,
                Error = error,
                Table = table
            });

        }

        [HttpGet]
        public async Task<IActionResult> Export(int id, string? format, int? formId = null, int? orgId = null)
        {
            var sp = await _db.VC_storedProcesses.Include(x => x.Params).FirstOrDefaultAsync(x => x.Id == id);
            if (sp is null) return NotFound();

            // ✅ Only read Query on GET; fall back to TempData.Peek
            var uidQ = Request.Query["uid"].FirstOrDefault()
                    ?? Request.Query["uuid"].FirstOrDefault()
                    ?? Request.Query["UID"].FirstOrDefault()
                    ?? (TempData.Peek("uid") as string);

            var formIdStr = formId?.ToString() ?? (TempData.Peek("formId") as string ?? string.Empty);
            var orgIdStr = orgId?.ToString() ?? (TempData.Peek("orgId") as string ?? string.Empty);

            var ctx = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["FormId"] = formIdStr,
                ["OrganizationId"] = orgIdStr,
                ["FormUUID"] = uidQ ?? string.Empty,
                ["UserName"] = User?.Identity?.Name ?? "system"
            };
            var tokenizedParams = ApplyTokens(sp.Params, ctx);

            var (table, error) = await _runner.ExecuteAsync(sp, tokenizedParams);
            if (!string.IsNullOrWhiteSpace(error) || table is null)
            {
                TempData["Error"] = error ?? "No data returned.";
                return RedirectToAction(nameof(Run), new { id, formId = formIdStr, orgId = orgIdStr, uid = uidQ });
            }

            // 🔔 Notify: Stored procedure export
            //await NotifyStoredProcAsync(
            //    subject: "Stored procedure exported",
            //    body: $"Data from stored process '{sp.Name}' was exported as {format.ToUpperInvariant()}.",
            //    metadata: new
            //    {
            //        action = "StoredProcessExport",
            //        processId = sp.Id,
            //        processName = sp.Name,
            //        formId = formIdStr,
            //        orgId = orgIdStr,
            //        uid = uidQ,
            //        format
            //    },
            //    ct: HttpContext.RequestAborted);

            switch (format)
            {
                case "xlsx":
                    var xlsx = DataTableToXlxs(table, sp.ExcludeHeadersOnExport);
                    var excelName = $"{sp.Name.Replace(':', '_').Replace('/', '_')}_{DateTime.UtcNow:yyyyMMdd}.xlsx";
                    return File(xlsx.ToArray(), "application/octet-stream", excelName);
                case "csv":
                    var csv = DataTableToCsv(table, includeHeaders: !sp.ExcludeHeadersOnExport);
                    var csvName = $"{sp.Name.Replace(':', '_').Replace('/', '_')}_{DateTime.UtcNow:yyyyMMdd}.csv";
                    return File(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv", csvName);
                case "txt":
                    var txt = DataTableToTxt(table, includeHeaders: !sp.ExcludeHeadersOnExport);
                    var txtName = $"{sp.Name.Replace(':', '_').Replace('/', '_')}_{DateTime.UtcNow:yyyyMMdd}.txt";
                    return File(System.Text.Encoding.UTF8.GetBytes(txt), "application/octet-stream", txtName);
            }

            return BadRequest("Unsupported format.");
        }

        private static string DataTableToCsv(System.Data.DataTable dt, bool includeHeaders = true)
        {
            var sb = new System.Text.StringBuilder();

            // headers
            if (includeHeaders)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i > 0) sb.Append(',');
                    sb.Append(EscapeCsv(dt.Columns[i].ColumnName));
                }
                sb.AppendLine();
            }

            // rows
            foreach (System.Data.DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i > 0) sb.Append(',');
                    var val = row[i]?.ToString() ?? string.Empty;
                    sb.Append(EscapeCsv(val));
                }
                sb.AppendLine();
            }

            return sb.ToString();

            static string EscapeCsv(string s)
            {
                // wrap in quotes if it contains comma, quote, or newline; double the quotes inside
                var needsQuotes = s.Contains(',') || s.Contains('"') || s.Contains('\n') || s.Contains('\r');
                if (needsQuotes)
                    return $"\"{s.Replace("\"", "\"\"")}\"";
                return s;
            }
        }

        private static MemoryStream DataTableToXlxs(System.Data.DataTable dt, bool includeHeaders)
        {
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells["A1"].LoadFromDataTable(dt, true);

                // headers
                if (includeHeaders)
                {
                    workSheet.DeleteRow(1);
                }
                package.Save();
            }
            stream.Position = 0;

            return stream;
        }

        private static string DataTableToTxt(System.Data.DataTable dt, bool includeHeaders = true)
        {
            var sb = new System.Text.StringBuilder();

            // headers
            if (includeHeaders)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i > 0) sb.Append(',');
                    sb.Append(EscapeCsv(dt.Columns[i].ColumnName));
                }
                sb.AppendLine();
            }

            // rows
            foreach (System.Data.DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i > 0) sb.Append(',');
                    var val = row[i]?.ToString() ?? string.Empty;
                    sb.Append(EscapeCsv(val));
                }
                sb.AppendLine();
            }

            return sb.ToString();

            static string EscapeCsv(string s)
            {
                // wrap in quotes if it contains comma, quote, or newline; double the quotes inside
                var needsQuotes = s.Contains(',') || s.Contains('"') || s.Contains('\n') || s.Contains('\r');
                if (needsQuotes)
                    return $"\"{s.Replace("\"", "\"\"")}\"";
                return s;
            }
        }


        // ------------------- TOKEN HELPERS (tiny, self-contained) -------------------
        private static string ReplaceTokens(string? value, IDictionary<string, string> ctx)
        {
            if (string.IsNullOrEmpty(value)) return value ?? string.Empty;
            foreach (var kv in ctx)
                value = value.Replace("{" + kv.Key + "}", kv.Value, StringComparison.OrdinalIgnoreCase);
            return value;
        }

        private static IEnumerable<VC_IMS.Models.VC_storedProcessParam> ApplyTokens(
            IEnumerable<VC_IMS.Models.VC_storedProcessParam> src,
            IDictionary<string, string> ctx)
        {
            foreach (var p in src)
            {
                // return a transient copy — DB values remain unchanged
                yield return new VC_IMS.Models.VC_storedProcessParam
                {
                    Id = p.Id,
                    StoredProcessId = p.StoredProcessId,
                    Key = p.Key,
                    DataType = p.DataType,
                    Value = ReplaceTokens(p.Value, ctx)
                };
            }
        }
        // ---------------------------------------------------------------------------

        // ---------------------------------------------------------------------------
        // Generic notification helper for stored procedure operations.
        // ---------------------------------------------------------------------------
        private async Task NotifyStoredProcAsync(
            string subject,
            string body,
            object? metadata = null,
            CancellationToken ct = default)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var recipient = !string.IsNullOrWhiteSpace(userIdClaim)
                ? userIdClaim
                : User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(recipient))
                return;

            var payload = new
            {
                Recipient = recipient,
                Channel = "InApp",
                Subject = subject,
                Body = body,
                MetadataJson = metadata == null ? null : JsonSerializer.Serialize(metadata)
            };

            try
            {
                // 🔔 Notify: Stored procedure event
                // await _elsa.ExecuteByNameAsync("Swims.Notifications.DirectInApp", payload, ct);
            }
            catch
            {
                // Never block execution if Elsa is unavailable.
            }
        }


    }
}
