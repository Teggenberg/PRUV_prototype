using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PRUV_WebApp.Data;
using PRUV_WebApp.Models;

namespace PRUV_WebApp.Controllers
{
    public class StoreUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StoreUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StoreUsers
        public async Task<IActionResult> Index()
        {
              return _context.StoreUser != null ? 
                          View(await _context.StoreUser.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.StoreUser'  is null.");
        }

        // Get: search requests
        public async Task<IActionResult> Search()
        {
            return _context.Request != null ?
                        View() :
                        Problem("Entity set 'ApplicationDbContext.Request'  is null.");
        }

        //PoST: Requests/ShowSEarchResults
        public async Task<IActionResult> ShowSearchResults(int? id)
        {
            /*if (id == null || _context.StoreUser == null)
            {
                return NotFound();
            }*/

            /*var storeUser = await _context.StoreUser
                .FirstOrDefaultAsync(m => m.EmpId == id);
            if (storeUser == null)
            {
                return NotFound();
            }
            return View("Details", storeUser);*/
            //int? userId = GetStoreUserId(id);
            var storeUser = await _context.StoreUser
                .FirstOrDefaultAsync(m => m.EmpId == id);

            if (storeUser == null) return View("Create");
            else
            return _context.Request != null ?
                        View("Details", storeUser) :
                        Problem("Entity set 'ApplicationDbContext.Request'  is null.");


        }

        public int? GetStoreUserId(int? empId)
        {
            
            string mainconn = "Server=localhost\\SQLEXPRESS;Database=PRUV;Trusted_Connection=True;";

            SqlConnection sqlconn = new SqlConnection(mainconn);




            string sqlquery = $"select Id from StoreUSer where EmpId = '{empId}'";
            
            SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);

            sqlconn.Open();
            SqlDataAdapter adapter2 = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            adapter2.Fill(dt);
            return int.Parse(dt.Rows[0][0].ToString()!);
        }

        public StoreUser UserExists(int empNum)
        {
            StoreUser found = new StoreUser();
            return found;

        }

        // GET: StoreUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StoreUser == null)
            {
                return NotFound();
            }

            var storeUser = await _context.StoreUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (storeUser == null)
            {
                return NotFound();
            }

            return View(storeUser);
        }

        // GET: StoreUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StoreUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Store,FirstName,LastName,Email,EmpId")] StoreUser storeUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(storeUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(storeUser);
        }

        // GET: StoreUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StoreUser == null)
            {
                return NotFound();
            }

            var storeUser = await _context.StoreUser.FindAsync(id);
            if (storeUser == null)
            {
                return NotFound();
            }
            return View(storeUser);
        }

        // POST: StoreUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Store,FirstName,LastName,Email")] StoreUser storeUser)
        {
            if (id != storeUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(storeUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreUserExists(storeUser.Id))
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
            return View(storeUser);
        }

        // GET: StoreUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StoreUser == null)
            {
                return NotFound();
            }

            var storeUser = await _context.StoreUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (storeUser == null)
            {
                return NotFound();
            }

            return View(storeUser);
        }

        // POST: StoreUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StoreUser == null)
            {
                return Problem("Entity set 'ApplicationDbContext.StoreUser'  is null.");
            }
            var storeUser = await _context.StoreUser.FindAsync(id);
            if (storeUser != null)
            {
                _context.StoreUser.Remove(storeUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoreUserExists(int id)
        {
          return (_context.StoreUser?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
