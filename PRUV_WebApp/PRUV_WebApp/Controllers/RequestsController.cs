using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRUV_WebApp.Data;
using PRUV_WebApp.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Data.SqlClient;

namespace PRUV_WebApp.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Requests
        public async Task<IActionResult> Index()
        {
              return _context.Request != null ? 
                          View(await _context.Request.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Request'  is null.");
        }
        // Get: search requests
        public async Task<IActionResult> ShowSearchForm()
        {
            return _context.Request != null ?
                        View() :
                        Problem("Entity set 'ApplicationDbContext.Request'  is null.");
        }

        //PoST: Requests/ShowSEarchResults
        public async Task<IActionResult> ShowSearchResults(String SearchForm)
        {
            return _context.Request != null ?
                        View("Index",await _context.Request.Where( j => j.RequestBrand.Contains(SearchForm)).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Request'  is null.");

        }

        // GET: Requests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Request == null)
            {
                return NotFound();
            }

            var request = await _context.Request
                .FirstOrDefaultAsync(m => m.RequestID == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // GET: Requests/Create
        public async Task <IActionResult> Create()
        {
            List<string> brands = new List<string>();
           

            System.Diagnostics.Debug.WriteLine("hello");
            string mainconn = "Server=localhost\\SQLEXPRESS;Database=PRUV;Trusted_Connection=True;";
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string sqlquery = "select * from Brand";
            SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
            sqlconn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {





                brands.Add(dt.Rows[i][1].ToString());
                //System.Diagnostics.Debug.WriteLine(i);
            }

            ViewBag.Roles = new SelectList(brands);

            return View();

           

        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestID,RequestYear,RequestBrand,RequestModel")] Request request, string RequestBrand)
        {
            int f ;
            string mainconn2 = "Server=localhost\\SQLEXPRESS;Database=PRUV;Trusted_Connection=True;";
            SqlConnection sqlconn = new SqlConnection(mainconn2);
            string sqlquery2 = $"select * from Brand where Name = '{RequestBrand}'";
            System.Diagnostics.Debug.WriteLine(sqlquery2);
            SqlCommand sqlcomm2 = new SqlCommand(sqlquery2, sqlconn);
            sqlconn.Open();
            SqlDataAdapter adapter2 = new SqlDataAdapter(sqlcomm2);
            DataTable dt2 = new DataTable();
            adapter2.Fill(dt2);
            int.TryParse(dt2.Rows[0][0].ToString(), out f);
            request.BrandId = f;
            System.Diagnostics.Debug.WriteLine(RequestBrand);
            if (ModelState.IsValid)
            {
                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        private static List<SelectListItem> PopulateBrands()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string constr = "Server=localhost\\SQLEXPRESS;Database=PRUV;Trusted_Connection=True;";
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = " SELECT Name, Id FROM Brand";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["Name"].ToString(),
                                Value = sdr["Id"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }

            return items;
        }

        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Request == null)
            {
                return NotFound();
            }

            var request = await _context.Request.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequestID,RequestYear,RequestBrand,RequestModel")] Request request)
        {
            if (id != request.RequestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestExists(request.RequestID))
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
            return View(request);
        }

        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Request == null)
            {
                return NotFound();
            }

            var request = await _context.Request
                .FirstOrDefaultAsync(m => m.RequestID == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Request == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Request'  is null.");
            }
            var request = await _context.Request.FindAsync(id);
            if (request != null)
            {
                _context.Request.Remove(request);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestExists(int id)
        {
          return (_context.Request?.Any(e => e.RequestID == id)).GetValueOrDefault();
        }
    }
}
