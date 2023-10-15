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
using Microsoft.AspNetCore.Http;
using System.Web;
using System.Text;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PRUV_WebApp.Data.Migrations;

namespace PRUV_WebApp.Controllers
{
    public class UserRequestsController : Controller
    {
        //public Dictionary<string, int> Brands { get; set; } = new Dictionary<string, int>();
        public List<string> Brands { get; set; } = new List<string>();
        public List<int> Locations { get; set; } = new List<int>();


        private readonly ApplicationDbContext _context;

        public UserRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserRequests
        public async Task<IActionResult> Index()
        {
            // Refresh page to show new incoming Requests
            Response.Headers.Add("Refresh", "5");
            // custom class JoinedRequest holds joined table data form custom query
            return View(DBCall.GetJoinedRequestsIndex(0));
        }

        // GET: UserRequests
        public async Task<IActionResult> InitiatedIndex()
        {
            // Refresh page to show new incoming Requests
            Response.Headers.Add("Refresh", "5");
            // custom class JoinedRequest holds joined table data form custom query
            return View("Index",DBCall.GetJoinedRequestsIndex(1));
        }

        // GET: UserRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // get item data with joint table query, send item to view
            JoinedRequest item = DBCall.GetJoinedRequestDetails(id);
            return View(item);
        }


        // GET: UserRequests/Create
        public IActionResult Create()
        {
             
            ViewBag.BrandList = new SelectList(DBCall.PopulateDropDown("Brand", 1));
            ViewBag.Locations = new SelectList
                (DBCall.PopulateDropDown("Locations", 0), DBCall.SetDefaultValue(Global.empLoc, "Locations", "LocationID", 0)) ;
            ViewBag.Employees = new SelectList
                (DBCall.PopulateDropDown("StoreUser", 5), DBCall.SetDefaultValue(Global.empNum, "StoreUser", "EmpId", 5));

            ViewBag.Cases = new SelectList(DBCall.PopulateDropDown("CaseOption", 1));
            return View();
        }

        // POST: UserRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create
            ([Bind("Id,RequestID,RequestYear,RequestModel,Serial,UserID,Intiated,InitiatedBy,InitiatedAt,Details,AskingPrice,Cost,Retail,Created")] 
            UserRequest userRequest, string StoreID, string BrandId, string? newBrand, string? CaseId,List<IFormFile> imageFile)
        {

        
            userRequest.StoreID = int.Parse(StoreID);

            if (BrandId == "Other")
            {
                DBCall.InsertNewBrand(newBrand);
                userRequest.BrandId = DBCall.GetDBId(newBrand, "Brand");

            }
            else userRequest.BrandId = DBCall.GetDBId(BrandId, "Brand");

            if(CaseId != null) userRequest.CaseId = DBCall.GetDBId(CaseId, "CaseOption");
            
            userRequest.RequestID = DBCall.CreateRequestID(userRequest.StoreID);
            userRequest.Created = DateTime.Now;
            userRequest.Intiated = false;
            bool state = ModelState.IsValid;
            if (state)
            {
                foreach(var image in imageFile)
                {
                    DBCall.AddImageToDB(userRequest.RequestID, Global.ConvertImageFile(image));

                }
                
                _context.Add(userRequest);
                await _context.SaveChangesAsync();
                Global.SendNotification(DBCall.GetEmailInfo(userRequest.Id));
                return RedirectToAction("ThankYou", "Home");
            }


            ViewBag.BrandList = new SelectList(DBCall.PopulateDropDown("Brand", 1));
            ViewBag.Locations = new SelectList
                (DBCall.PopulateDropDown("Locations", 0), DBCall.SetDefaultValue(Global.empLoc, "Locations", "LocationID", 0));
            ViewBag.Employees = new SelectList
                (DBCall.PopulateDropDown("StoreUser", 5), DBCall.SetDefaultValue(Global.empNum, "StoreUser", "EmpId", 5));

            ViewBag.Cases = new SelectList(DBCall.PopulateDropDown("CaseOption", 1));
            return View(userRequest);
        }

        public async Task<IActionResult> AddImages(int? Id)
        {
            JoinedRequest userRequest = DBCall.GetJoinedRequestDetails(Id);
            
            return View(userRequest);
        }

        [HttpPost]
        public async Task<IActionResult> AddImages(int requestId, List<IFormFile> imageFile)
        {
            if(imageFile != null) 
            {
                foreach (var image in imageFile)
                {
                    DBCall.AddImageToDB(requestId, Global.ConvertImageFile(image));

                }
                return RedirectToAction("ThankYou", "Home");
            }
            return View();
        }

        public async Task<IActionResult> SearchRequests()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchRequests(int empId)
        {
            List<JoinedRequest> jr = DBCall.ManageRequestList(empId);
            return View("UserRequestList", jr);
        }

        public async Task<IActionResult> UserRequestList()
        {
            return View();
        }

        public async Task<IActionResult> IntakeForms()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IntakeForms(int empNum)
        {
            if (empNum != null)
            {
                List<JoinedRequest> jr = DBCall.ManageRequestList(empNum);
                return View(jr);
            }
            return View();
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
        /*[HttpPost]*/
        public async Task<IActionResult> Initiate(int id)
        {
            DBCall.Initiate(id);
            JoinedRequest item = DBCall.GetJoinedRequestDetails(id);
            return View("Details",item);
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
