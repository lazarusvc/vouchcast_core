using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using VC_IMS.Data;
using VC_IMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VC_IMS.Controllers
{
    public class formProcessController : Controller
    {
        private readonly VC_IMSDb_moreContext _context;

        private static int? TryExtractProcIdFromRunUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url)) return null;
            var m = Regex.Match(url, @"/StoredProcesses/Run/(\d+)", RegexOptions.IgnoreCase);
            return m.Success ? int.Parse(m.Groups[1].Value) : (int?)null;
        }

        public formProcessController(VC_IMSDb_moreContext context)
        {
            _context = context;
        }

        // GET: formProcess
        public async Task<IActionResult> Index()
        {
            return View(await _context.VC_formProcesses.ToListAsync());
        }

        // GET: formProcess/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var VC_formProcess = await _context.VC_formProcesses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (VC_formProcess == null)
            {
                return NotFound();
            }

            return View(VC_formProcess);
        }

        // GET: formProcess/Create
        public IActionResult Create()
        {
            ViewBag.processes = _context.VC_storedProcesses
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = "StoredProcesses/Run/" + Convert.ToString(c.Id)
                })
                .ToList();

            ViewData["VC_formsId"] = new SelectList(_context.VC_forms, "Id", "name");
            return View();
        }

        // POST: formProcess/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,url,name,VC_formsId")] VC_formProcess VC_formProcess)
        {
            if (ModelState.IsValid)
            {
                // --- Auto-fill name from Stored Procedure when blank ---
                if (string.IsNullOrWhiteSpace(VC_formProcess.name))
                {
                    var procId = TryExtractProcIdFromRunUrl(VC_formProcess.url);
                    if (procId.HasValue)
                    {
                        var spName = await _context.VC_storedProcesses
                            .Where(x => x.Id == procId.Value)
                            .Select(x => x.Name)
                            .FirstOrDefaultAsync();

                        if (!string.IsNullOrWhiteSpace(spName))
                            VC_formProcess.name = spName;
                    }
                }
                // -------------------------------------------------------

                _context.Add(VC_formProcess);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(VC_formProcess);
        }

        // GET: formProcess/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var VC_formProcess = await _context.VC_formProcesses.FindAsync(id);
            if (VC_formProcess == null)
            {
                return NotFound();
            }
            return View(VC_formProcess);
        }

        // POST: formProcess/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,url,name,VC_formsId")] VC_formProcess VC_formProcess)
        {
            if (id != VC_formProcess.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // --- Auto-fill name from Stored Procedure when blank (on edit too) ---
                    if (string.IsNullOrWhiteSpace(VC_formProcess.name))
                    {
                        var procId = TryExtractProcIdFromRunUrl(VC_formProcess.url);
                        if (procId.HasValue)
                        {
                            var spName = await _context.VC_storedProcesses
                                .Where(x => x.Id == procId.Value)
                                .Select(x => x.Name)
                                .FirstOrDefaultAsync();

                            if (!string.IsNullOrWhiteSpace(spName))
                                VC_formProcess.name = spName;
                        }
                    }
                    // ---------------------------------------------------------------------

                    _context.Update(VC_formProcess);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VC_formProcessExists(VC_formProcess.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(VC_formProcess);
        }

        // GET: formProcess/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var VC_formProcess = await _context.VC_formProcesses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (VC_formProcess == null)
            {
                return NotFound();
            }

            return View(VC_formProcess);
        }

        // POST: formProcess/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var VC_formProcess = await _context.VC_formProcesses.FindAsync(id);
            if (VC_formProcess != null)
            {
                _context.VC_formProcesses.Remove(VC_formProcess);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VC_formProcessExists(int id)
        {
            return _context.VC_formProcesses.Any(e => e.Id == id);
        }
    }
}