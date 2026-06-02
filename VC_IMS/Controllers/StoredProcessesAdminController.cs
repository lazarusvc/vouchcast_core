using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VC_IMS.Data;
using VC_IMS.Models;
using VC_IMS.Models.Security;
using VC_IMS.Models.ViewModels;

namespace VC_IMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StoredProcessesAdminController : Controller
    {
        private readonly VC_IMSDb_moreContext _db;
        private readonly IDataProtector? _protector;

        public StoredProcessesAdminController(VC_IMSDb_moreContext db, IDataProtectionProvider dp)
        {
            _db = db;
            _protector = dp.CreateProtector(DataProtectionPurposes.StoredProcedures);
        }

        // GET: /StoredProcessesAdmin
        public async Task<IActionResult> Index()
        {
            var rows = await _db.VC_storedProcesses.AsNoTracking().OrderBy(x => x.Name).ToListAsync();
            return View(rows);
        }

        // GET: /StoredProcessesAdmin/Create
        public IActionResult Create() => View(new StoredProcessEditViewModel());

        // POST: /StoredProcessesAdmin/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StoredProcessEditViewModel vm, IFormCollection frm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (string.IsNullOrWhiteSpace(vm.ConnectionKey) &&
                (string.IsNullOrWhiteSpace(vm.DataSource) || string.IsNullOrWhiteSpace(vm.Database)))
            {
                ModelState.AddModelError(string.Empty, "Provide either a ConnectionKey or a DataSource + Database.");
                return View(vm);
            }

            var row = new VC_storedProcess
            {
                Name = vm.Name.Trim(),
                Description = vm.Description?.Trim(),
                ConnectionKey = string.IsNullOrWhiteSpace(vm.ConnectionKey) ? null : vm.ConnectionKey!.Trim(),
                DataSource = string.IsNullOrWhiteSpace(vm.DataSource) ? null : vm.DataSource!.Trim(),
                Database = string.IsNullOrWhiteSpace(vm.Database) ? null : vm.Database!.Trim(),
                DbUserEncrypted = string.IsNullOrWhiteSpace(vm.DbUser) ? null : Protect(vm.DbUser!),
                DbPasswordEncrypted = string.IsNullOrWhiteSpace(vm.DbPassword) ? null : Protect(vm.DbPassword!),
                ExcludeHeadersOnExport = vm.ExcludeHeadersOnExport
            };

            _db.VC_storedProcesses.Add(row);
            await _db.SaveChangesAsync();      

            // CREATE Stored Procedure in DB if not exists
            // ___________________________________________
            await _db.Database.ExecuteSqlRawAsync(String.Format(@"CREATE PROCEDURE dbo.usp_{0} {1}", vm.Name.ToString(), frm["editorVal"].ToString()));


            return RedirectToAction(nameof(Index));
        }

        // GET: /StoredProcessesAdmin/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var row = await _db.VC_storedProcesses.FindAsync(id);
            if (row == null) return NotFound();

            var vm = new StoredProcessEditViewModel
            {
                Id = row.Id,
                Name = row.Name,
                Description = row.Description,
                ConnectionKey = row.ConnectionKey,
                DataSource = row.DataSource,
                Database = row.Database,
                ExcludeHeadersOnExport = row.ExcludeHeadersOnExport
                // Do NOT echo creds
            };
            return View(vm);
        }

        // POST: /StoredProcessesAdmin/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StoredProcessEditViewModel vm)
        {
            if (id != vm.Id) return BadRequest();
            if (!ModelState.IsValid) return View(vm);

            if (string.IsNullOrWhiteSpace(vm.ConnectionKey) &&
                (string.IsNullOrWhiteSpace(vm.DataSource) || string.IsNullOrWhiteSpace(vm.Database)))
            {
                ModelState.AddModelError(string.Empty, "Provide either a ConnectionKey or a DataSource + Database.");
                return View(vm);
            }

            var row = await _db.VC_storedProcesses.FirstOrDefaultAsync(x => x.Id == id);
            if (row == null) return NotFound();

            row.Name = vm.Name.Trim();
            row.Description = vm.Description?.Trim();
            row.ConnectionKey = string.IsNullOrWhiteSpace(vm.ConnectionKey) ? null : vm.ConnectionKey!.Trim();
            row.DataSource = string.IsNullOrWhiteSpace(vm.DataSource) ? null : vm.DataSource!.Trim();
            row.Database = string.IsNullOrWhiteSpace(vm.Database) ? null : vm.Database!.Trim();
            row.ExcludeHeadersOnExport = vm.ExcludeHeadersOnExport;

            if (!string.IsNullOrWhiteSpace(vm.DbUser)) row.DbUserEncrypted = Protect(vm.DbUser!);
            if (!string.IsNullOrWhiteSpace(vm.DbPassword)) row.DbPasswordEncrypted = Protect(vm.DbPassword!);

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /StoredProcessesAdmin/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var row = await _db.VC_storedProcesses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (row == null) return NotFound();
            return View(row);
        }

        // POST: /StoredProcessesAdmin/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var row = await _db.VC_storedProcesses.Include(x => x.Params).FirstOrDefaultAsync(x => x.Id == id);
            if (row != null)
            {
                _db.VC_storedProcesses.Remove(row); // cascade deletes params
                await _db.SaveChangesAsync();

                // DELETE Stored Procedure in DB if not exists
                // ___________________________________________
                await _db.Database.ExecuteSqlRawAsync(String.Format(@"DROP PROCEDURE IF EXISTS dbo.usp_{0};", row.Name.ToString()));
            }
            return RedirectToAction(nameof(Index));
        }

        private string Protect(string plaintext) =>
            _protector == null ? plaintext : _protector.Protect(plaintext);
    }
}
