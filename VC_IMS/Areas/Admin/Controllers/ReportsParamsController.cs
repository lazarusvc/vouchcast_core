using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VC_IMS.Models;
using System.Linq;

namespace VC_IMS.Areas.Admin.Controllers
{
    // Add a route template so POST cannot miss your action because of conventional routing quirks
[Area("Admin")]
[Authorize(Policy = "ReportsAdmin")]
[Route("Admin/[controller]/[action]")]
public class ReportParamsController : Controller
{
    private readonly VC_IMSDb_moreContext _db;
        private readonly ILogger<ReportParamsController> _logger;

        public ReportParamsController(VC_IMSDb_moreContext db, ILogger<ReportParamsController> logger)
        {
            _db = db;
            _logger = logger;
        }

    // GET: /Admin/ReportParams/Index?reportId=123
    [HttpGet]
    public async Task<IActionResult> Index(int reportId)
    {
        var report = await _db.VC_reports.FindAsync(reportId);
        if (report == null) return NotFound();
        ViewBag.Report = report;

        var items = await _db.VC_reportParams
            .Where(p => p.VCReportId == reportId)
            .OrderBy(p => p.ParamKey)
            .ToListAsync();

        return View(items);
    }

        // GET: /Admin/ReportParams/Create?reportId=123
        [HttpGet]
    public IActionResult Create(int reportId)
    {
        return View(new VC_reportParam { VCReportId = reportId });
    }
        

        // POST: /Admin/ReportParams/Create
        // Force binding from form; log and display any model errors
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] VC_reportParam m, int? reportId)
    {
            // Safety: if hidden field didn't bind for any reason, fall back to route/query param
            if (m.VCReportId == 0 && reportId.HasValue) m.VCReportId = reportId.Value;

            // ignore the navigation property during validation
            ModelState.Remove(nameof(VC_reportParam.VCReportId));

            if (!ModelState.IsValid)
            {
                var errs = string.Join(" | ",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                if (!string.IsNullOrWhiteSpace(errs))
                {
                    _logger.LogWarning("Create ReportParam ModelState errors: {Errors}", errs);
                    TempData["Err"] = errs;
                }
                return View(m);
            }

            m.ParamKey = m.ParamKey?.Trim() ?? "";
            m.ParamValue = m.ParamValue?.Trim() ?? "";

        _db.Add(m);
        await _db.SaveChangesAsync();
        TempData["Ok"] = "Parameter added.";
        return RedirectToAction(nameof(Index), new { reportId = m.VCReportId });
    }

        // GET: /Admin/ReportParams/Edit/5
        [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var m = await _db.VC_reportParams.FindAsync(id);
        if (m == null) return NotFound();
        return View(m);
    }

        // POST: /Admin/ReportParams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] VC_reportParam m)
    {
        if (id != m.Id) return BadRequest();

            // ignore the navigation property during validation
            ModelState.Remove(nameof(VC_reportParam.VCReportId));

            if (!ModelState.IsValid)
            {
                var errs = string.Join(" | ",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                if (!string.IsNullOrWhiteSpace(errs))
                {
                    _logger.LogWarning("Edit ReportParam ModelState errors: {Errors}", errs);
                    TempData["Err"] = errs;
                }
                return View(m);
            }

            m.ParamKey = m.ParamKey?.Trim() ?? "";
            m.ParamValue = m.ParamValue?.Trim() ?? "";

        _db.Update(m);
        await _db.SaveChangesAsync();
        TempData["Ok"] = "Parameter saved.";
        return RedirectToAction(nameof(Index), new { reportId = m.VCReportId });
    }

        // POST: /Admin/ReportParams/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var m = await _db.VC_reportParams.FindAsync(id);
        if (m == null) return NotFound();
        var rid = m.VCReportId;
        _db.Remove(m);
        await _db.SaveChangesAsync();
        TempData["Ok"] = "Parameter deleted.";
        return RedirectToAction(nameof(Index), new { reportId = rid });
    }
    }
}
