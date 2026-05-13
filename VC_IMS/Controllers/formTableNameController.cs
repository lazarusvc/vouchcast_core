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
    public class formTableNameController : Controller
    {
        private readonly VC_IMSDb_moreContext _context;

        public formTableNameController(VC_IMSDb_moreContext context)
        {
            _context = context;
        }

        // GET: formTableName
        public async Task<IActionResult> Index()
        {
            var VC_IMSDb_moreContext = _context.VC_formTableNames.Include(s => s.VC_forms);
            return View(await VC_IMSDb_moreContext.ToListAsync());
        }

        // GET: formTableName/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var VC_formTableName = await _context.VC_formTableNames
                .Include(s => s.VC_forms)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (VC_formTableName == null)
            {
                return NotFound();
            }

            return View(VC_formTableName);
        }

        // GET: formTableName/Create
        public IActionResult Create()
        {
            ViewData["VC_formsId"] = new SelectList(_context.VC_forms, "Id", "name");
            return View();
        }

        // POST: formTableName/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,field,VC_formsId")] VC_formTableName VC_formTableName)
        {
            if (ModelState.IsValid)
            {
                _context.Add(VC_formTableName);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VC_formsId"] = new SelectList(_context.VC_forms, "Id", "name", VC_formTableName.VC_formsId);
            return View(VC_formTableName);
        }

        // GET: formTableName/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var VC_formTableName = await _context.VC_formTableNames.FindAsync(id);
            if (VC_formTableName == null)
            {
                return NotFound();
            }
            ViewData["VC_formsId"] = new SelectList(_context.VC_forms, "Id", "name", VC_formTableName.VC_formsId);
            return View(VC_formTableName);
        }

        // POST: formTableName/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,field,VC_formsId")] VC_formTableName VC_formTableName)
        {
            if (id != VC_formTableName.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(VC_formTableName);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VC_formTableNameExists(VC_formTableName.Id))
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
            ViewData["VC_formsId"] = new SelectList(_context.VC_forms, "Id", "name", VC_formTableName.VC_formsId);
            return View(VC_formTableName);
        }

        // GET: formTableName/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var VC_formTableName = await _context.VC_formTableNames
                .Include(s => s.VC_forms)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (VC_formTableName == null)
            {
                return NotFound();
            }

            return View(VC_formTableName);
        }

        // POST: formTableName/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var VC_formTableName = await _context.VC_formTableNames.FindAsync(id);
            if (VC_formTableName != null)
            {
                _context.VC_formTableNames.Remove(VC_formTableName);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VC_formTableNameExists(int id)
        {
            return _context.VC_formTableNames.Any(e => e.Id == id);
        }
    }
}
