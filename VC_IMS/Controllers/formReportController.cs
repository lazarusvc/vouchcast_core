using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VC_IMS.Models;

namespace VC_IMS.Controllers
{
    public class formReportController : Controller
    {
        private readonly VC_IMSDb_moreContext _context;

        public formReportController(VC_IMSDb_moreContext context)
        {
            _context = context;
        }

        // GET: formReport
        public async Task<IActionResult> Index()
        {
            var VC_IMSDb_moreContext = _context.VC_formReports.Include(s => s.VC_forms);
            return View(await VC_IMSDb_moreContext.ToListAsync());
        }

        // GET: formReport/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var VC_formReport = await _context.VC_formReports
                .Include(s => s.VC_forms)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (VC_formReport == null)
            {
                return NotFound();
            }

            return View(VC_formReport);
        }

        // GET: formReport/Create
        public IActionResult Create()
        {
            ViewData["VC_formsId"] = new SelectList(_context.VC_forms, "Id", "name");
            return View();
        }

        // POST: formReport/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,url,name,VC_formsId")] VC_formReport VC_formReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(VC_formReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VC_formsId"] = new SelectList(_context.VC_forms, "Id", "name", VC_formReport.VC_formsId);
            return View(VC_formReport);
        }

        // GET: formReport/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var VC_formReport = await _context.VC_formReports.FindAsync(id);
            if (VC_formReport == null)
            {
                return NotFound();
            }
            ViewData["VC_formsId"] = new SelectList(_context.VC_forms, "Id", "name", VC_formReport.VC_formsId);
            return View(VC_formReport);
        }

        // POST: formReport/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,url,name,VC_formsId")] VC_formReport VC_formReport)
        {
            if (id != VC_formReport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(VC_formReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VC_formReportExists(VC_formReport.Id))
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
            ViewData["VC_formsId"] = new SelectList(_context.VC_forms, "Id", "name", VC_formReport.VC_formsId);
            return View(VC_formReport);
        }

        // GET: formReport/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var VC_formReport = await _context.VC_formReports
                .Include(s => s.VC_forms)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (VC_formReport == null)
            {
                return NotFound();
            }

            return View(VC_formReport);
        }

        // POST: formReport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var VC_formReport = await _context.VC_formReports.FindAsync(id);
            if (VC_formReport != null)
            {
                _context.VC_formReports.Remove(VC_formReport);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VC_formReportExists(int id)
        {
            return _context.VC_formReports.Any(e => e.Id == id);
        }
    }
}
