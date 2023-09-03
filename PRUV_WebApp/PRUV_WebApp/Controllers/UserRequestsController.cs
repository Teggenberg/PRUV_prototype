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
    public class UserRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserRequests
        public async Task<IActionResult> Index()
        {
              return _context.UserRequest != null ? 
                          View(await _context.UserRequest.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.UserRequest'  is null.");
        }

        // GET: UserRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserRequest == null)
            {
                return NotFound();
            }

            var userRequest = await _context.UserRequest
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRequest == null)
            {
                return NotFound();
            }

            return View(userRequest);
        }

        // GET: UserRequests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RequestID,RequestYear,BrandId,RequestModel,Serial,UserID,Intiated,InitiatedBy,InitiatedAt,Details,AskingPrice,Cost,Retail,Case,Created")] UserRequest userRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userRequest);
        }

        // GET: UserRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserRequest == null)
            {
                return NotFound();
            }

            var userRequest = await _context.UserRequest.FindAsync(id);
            if (userRequest == null)
            {
                return NotFound();
            }
            return View(userRequest);
        }

        // POST: UserRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RequestID,RequestYear,BrandId,RequestModel,Serial,UserID,Intiated,InitiatedBy,InitiatedAt,Details,AskingPrice,Cost,Retail,Case,Created")] UserRequest userRequest)
        {
            if (id != userRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRequestExists(userRequest.Id))
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
            return View(userRequest);
        }

        // GET: UserRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserRequest == null)
            {
                return NotFound();
            }

            var userRequest = await _context.UserRequest
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRequest == null)
            {
                return NotFound();
            }

            return View(userRequest);
        }

        // POST: UserRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserRequest == null)
            {
                return Problem("Entity set 'ApplicationDbContext.UserRequest'  is null.");
            }
            var userRequest = await _context.UserRequest.FindAsync(id);
            if (userRequest != null)
            {
                _context.UserRequest.Remove(userRequest);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRequestExists(int id)
        {
          return (_context.UserRequest?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
