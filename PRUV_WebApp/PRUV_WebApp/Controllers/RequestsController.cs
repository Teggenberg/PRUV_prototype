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
        public List<string> Brands { get; set; } = new List<string>() ;
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

        public void PopulateBrandDropDown()
        {

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

                Brands.Add(dt.Rows[i][1].ToString()!);
                
            }

            ViewBag.Roles = new SelectList(Brands);

        }

        // GET: Requests/Create
        public async Task <IActionResult> Create()
        {
            
           

            System.Diagnostics.Debug.WriteLine("hello");
            PopulateBrandDropDown();
            
            
            return View();

           

        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestID,RequestYear,RequestBrand,RequestModel")] Request request, string RequestBrand, string NewBrand)
        {
            
            if(RequestBrand == "Other")
            {
                RequestBrand = NewBrand;
                request.RequestBrand = RequestBrand;
                try
                {
                    InsertNewBrand(NewBrand);

                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Duplicate Entry");
                }

            }
            
            
            int f ;
            string mainconn2 = "Server=localhost\\SQLEXPRESS;Database=PRUV;Trusted_Connection=True;";
            
            SqlConnection sqlconn2 = new SqlConnection(mainconn2);
            
            
            
            
            string sqlquery2 = $"select * from Brand where Name = '{request.RequestBrand}'";
            System.Diagnostics.Debug.WriteLine(sqlquery2);
            SqlCommand sqlcomm2 = new SqlCommand(sqlquery2, sqlconn2);

            sqlconn2.Open();
            SqlDataAdapter adapter2 = new SqlDataAdapter(sqlcomm2);
            DataTable dt2 = new DataTable();
            adapter2.Fill(dt2);
            int.TryParse(dt2.Rows[0][0].ToString(), out f);
            request.BrandId = f;
            System.Diagnostics.Debug.WriteLine(RequestBrand);
            System.Diagnostics.Debug.WriteLine(request.RequestID);
            System.Diagnostics.Debug.WriteLine(request.RequestModel);
            System.Diagnostics.Debug.WriteLine(request.RequestBrand);

            if (true)
            {
                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateBrandDropDown();
            return View(request);
        }

        public void InsertNewBrand(string newBrand)
        {
            string mainconn2 = "Server=localhost\\SQLEXPRESS;Database=PRUV;Trusted_Connection=True;";
            SqlConnection sqlconn = new SqlConnection(mainconn2);
           
            string sqlquery = $"insert into Brand (Name) values ('{newBrand}')";
            
            SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
            //SqlDataAdapter adapter = new SqlDataAdapter(sqlcomm);
            sqlconn.Open();
            sqlcomm.ExecuteNonQuery();

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
        public async Task<IActionResult> Edit(int id, [Bind("RequestID,RequestYear,NewBrand,RequestBrand,RequestModel")] Request request)
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
