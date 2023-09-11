using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRUV_WebApp.Data;
using PRUV_WebApp.Models;

namespace PRUV_WebApp.Controllers
{
    public class CaseOptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CaseOptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CaseOptions
        public async Task<IActionResult> Index()
        {
              return _context.CaseOption != null ? 
                          View(await _context.CaseOption.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.CaseOption'  is null.");
        }

        // GET: CaseOptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CaseOption == null)
            {
                return NotFound();
            }

            var caseOption = await _context.CaseOption
                .FirstOrDefaultAsync(m => m.Id == id);
            if (caseOption == null)
            {
                return NotFound();
            }

            return View(caseOption);
        }

        // GET: CaseOptions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CaseOptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] CaseOption caseOption)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caseOption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(caseOption);
        }

        // GET: CaseOptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CaseOption == null)
            {
                return NotFound();
            }

            var caseOption = await _context.CaseOption.FindAsync(id);
            if (caseOption == null)
            {
                return NotFound();
            }
            return View(caseOption);
        }

        // POST: CaseOptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] CaseOption caseOption)
        {
            if (id != caseOption.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseOption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseOptionExists(caseOption.Id))
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
            return View(caseOption);
        }

        // GET: CaseOptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CaseOption == null)
            {
                return NotFound();
            }

            var caseOption = await _context.CaseOption
                .FirstOrDefaultAsync(m => m.Id == id);
            if (caseOption == null)
            {
                return NotFound();
            }

            return View(caseOption);
        }

        // POST: CaseOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CaseOption == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CaseOption'  is null.");
            }
            var caseOption = await _context.CaseOption.FindAsync(id);
            if (caseOption != null)
            {
                _context.CaseOption.Remove(caseOption);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseOptionExists(int id)
        {
          return (_context.CaseOption?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
