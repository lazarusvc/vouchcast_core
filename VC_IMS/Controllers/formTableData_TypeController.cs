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
    public class formTableData_TypeController : Controller
    {
        private readonly VC_IMSDb_moreContext _context;

        public formTableData_TypeController(VC_IMSDb_moreContext context)
        {
            _context = context;
        }

        // GET: formTableData_Type
        public async Task<IActionResult> Index()
        {
            var VC_IMSDb_moreContext = _context.VC_formTableData_Types.Include(s => s.VC_forms);
            return View(await VC_IMSDb_moreContext.ToListAsync());
        }

        // GET: formTableData_Type/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var VC_formTableData_Type = await _context.VC_formTableData_Types
                .Include(s => s.VC_forms)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (VC_formTableData_Type == null)
            {
                return NotFound();
            }

            return View(VC_formTableData_Type);
        }

        // GET: formTableData_Type/Create
        public IActionResult Create()
        {
            ViewData["VC_formsId"] = new SelectList(_context.VC_forms, "Id", "name");
            return View();
        }

        // POST: formTableData_Type/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,type,field,VC_formsId")] VC_formTableData_Type VC_formTableData_Type)
        {
            if (ModelState.IsValid)
            {
                _context.Add(VC_formTableData_Type);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VC_formsId"] = new SelectList(_context.VC_forms, "Id", "name", VC_formTableData_Type.VC_formsId);
            return View(VC_formTableData_Type);
        }

        // GET: formTableData_Type/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var VC_formTableData_Type = await _context.VC_formTableData_Types.FindAsync(id);
            if (VC_formTableData_Type == null)
            {
                return NotFound();
            }
            ViewData["VC_formsId"] = new SelectList(_context.VC_forms, "Id", "name", VC_formTableData_Type.VC_formsId);
            return View(VC_formTableData_Type);
        }

        // POST: formTableData_Type/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,type,field,VC_formsId")] VC_formTableData_Type VC_formTableData_Type)
        {
            if (id != VC_formTableData_Type.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(VC_formTableData_Type);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VC_formTableData_TypeExists(VC_formTableData_Type.Id))
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
            ViewData["VC_formsId"] = new SelectList(_context.VC_forms, "Id", "name", VC_formTableData_Type.VC_formsId);
            return View(VC_formTableData_Type);
        }

        // GET: formTableData_Type/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var VC_formTableData_Type = await _context.VC_formTableData_Types
                .Include(s => s.VC_forms)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (VC_formTableData_Type == null)
            {
                return NotFound();
            }

            return View(VC_formTableData_Type);
        }

        // POST: formTableData_Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var VC_formTableData_Type = await _context.VC_formTableData_Types.FindAsync(id);
            if (VC_formTableData_Type != null)
            {
                _context.VC_formTableData_Types.Remove(VC_formTableData_Type);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VC_formTableData_TypeExists(int id)
        {
            return _context.VC_formTableData_Types.Any(e => e.Id == id);
        }
    }
}
