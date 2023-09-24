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
            return View(GetJoinedRequests());
        }

        // GET: UserRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // get item data with joint table query, send item to view
            JoinedRequest item = GetItemData(id);
            return View(item);
        }

        public JoinedRequest GetItemData(int? id)
        {
            // joint query to gather item data from related tables
            string mainconn = "Server=localhost\\SQLEXPRESS;Database=PRUV;Trusted_Connection=True;";
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string sqlquery = "select UserRequest.Id, UserRequest.RequestId, StoreID, RequestYear, Brand.Name, RequestModel, CaseOption.Name, CONVERT(VARCHAR(19),Created,100), RequestImage " +
                "\r\nfrom UserRequest" +
                "\r\nJoin Brand on UserRequest.BrandId = Brand.Id" +
                "\r\nLeft Join CaseOption on UserRequest.CaseId = CaseOption.Id" +
                "\r\nLeft Join RequestImage on UserRequest.RequestId = RequestImage.RequestId" +
                $"\r\nWhere UserRequest.Id = {id};";
            JoinedRequest jr = new JoinedRequest();
            SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
            sqlconn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                // populate object with data table data
                jr.Id = int.Parse(dt.Rows[i][0].ToString()!);
                jr.RequestID = int.Parse(dt.Rows[i][1].ToString()!);
                jr.Store = int.Parse(dt.Rows[i][2].ToString()!);
                jr.Year = dt.Rows[i][3].ToString()!;
                jr.Brand = dt.Rows[i][4].ToString()!;
                jr.Model = dt.Rows[i][5].ToString()!;
                jr.Case = dt.Rows[i][6].ToString()!;
                jr.Created = dt.Rows[i][7].ToString();
                jr.image = (byte[])dt.Rows[i][8];
            }

            return jr;
        }



        public List<string> PopulateDropDown(string table, int column, int selectedItem = 0) 
        {
            // new list that will be retunred for drop down menu
            List<string> list = new List<string>();

            // query table for values to populate list using column and table parameters
            string mainconn = "Server=localhost\\SQLEXPRESS;Database=PRUV;Trusted_Connection=True;";
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string sqlquery = $"select * from {table}";
            SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
            sqlconn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                // add value to list
                list.Add(dt.Rows[i][column].ToString()!);

            }

            return list; 
        }



        public List<JoinedRequest> GetJoinedRequests()
        {
            var list = new List<JoinedRequest>();
            string mainconn = "Server=localhost\\SQLEXPRESS;Database=PRUV;Trusted_Connection=True;";
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string sqlquery = "select UserRequest.Id, RequestId, StoreID, RequestYear, Brand.Name, RequestModel, CaseOption.Name, datediff(minute, Created, GetDate()) " +
                "\r\nfrom UserRequest" +
                "\r\nJoin Brand on UserRequest.BrandId = Brand.Id" +
                "\r\nLeft Join CaseOption on UserRequest.CaseId = CaseOption.Id;";
            SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
            sqlconn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                JoinedRequest jr = new JoinedRequest();
                jr.Id = int.Parse(dt.Rows[i][0].ToString()!);
                jr.RequestID = int.Parse(dt.Rows[i][1].ToString()!);
                jr.Store = int.Parse(dt.Rows[i][2].ToString()!);
                jr.Year = dt.Rows[i][3].ToString()!;
                jr.Brand = dt.Rows[i][4].ToString()!;
                jr.Model = dt.Rows[i][5].ToString()!;
                jr.Case = dt.Rows[i][6].ToString()!;
                jr.Created = dt.Rows[i][7].ToString() + " minutes ago";
                System.Diagnostics.Debug.WriteLine(dt.Rows[i][7]);
                System.Diagnostics.Debug.WriteLine(jr.Created);
                list.Add(jr);

            }
            return list;
        }

        public int CreateRequestID(int loc)
        {
            // query gathers the count of requests for given locoation to assign a new RequestId unique to
            // the location
            int id = 0;
            string mainconn = "Server=localhost\\SQLEXPRESS;Database=PRUV;Trusted_Connection=True;";
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string sqlquery = $"select count(*) from UserRequest where StoreID = {loc}";
            SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
            sqlconn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                id = (int.Parse(dt.Rows[i][0].ToString()!));

            }
            id += loc * 100000;
            return id;
        }

        public int GetDBId(string name, string table)
        {
           
            string mainconn2 = "Server=localhost\\SQLEXPRESS;Database=PRUV;Trusted_Connection=True;";
            SqlConnection sqlconn2 = new SqlConnection(mainconn2);
            string sqlquery2 = $"select * from {table} where Name = '{name}'";
            System.Diagnostics.Debug.WriteLine(sqlquery2);
            SqlCommand sqlcomm2 = new SqlCommand(sqlquery2, sqlconn2);
            sqlconn2.Open();
            SqlDataAdapter adapter2 = new SqlDataAdapter(sqlcomm2);
            DataTable dt2 = new DataTable();
            adapter2.Fill(dt2);
            return int.Parse(dt2.Rows[0][0].ToString()!);
        }

        public string DefaultValue(int id, string table, string columnName, int column)
        {

            string mainconn2 = "Server=localhost\\SQLEXPRESS;Database=PRUV;Trusted_Connection=True;";
            SqlConnection sqlconn2 = new SqlConnection(mainconn2);
            string sqlquery2 = $"select * from {table} where {columnName} = '{id}'";
            System.Diagnostics.Debug.WriteLine(sqlquery2);
            SqlCommand sqlcomm2 = new SqlCommand(sqlquery2, sqlconn2);
            sqlconn2.Open();
            SqlDataAdapter adapter2 = new SqlDataAdapter(sqlcomm2);
            DataTable dt2 = new DataTable();
            adapter2.Fill(dt2);
            return dt2.Rows[0][column].ToString()!;
        }

        public void AddImage(int? requestId, byte[]? image)
        {
            string bytes = image.ToString();

            string mainconn = "Server=localhost\\SQLEXPRESS;Database=PRUV;Trusted_Connection=True;";
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string sqlquery = $"insert into RequestImage (RequestId,RequestImage) values ({requestId},CAST('{bytes}' AS VARBINARY(MAX)))";
            const string sql_insert_string =
                "Insert into RequestImage (RequestId,RequestImage) values (@image_id, @image_byte_array)";
            var byteParam = new SqlParameter("@image_byte_array", SqlDbType.VarBinary)
            {
                Direction = ParameterDirection.Input,
                Size = image.Length,
                Value = image
            };

            var imageIdParam = new SqlParameter("@image_id", SqlDbType.Int, 4)
            {
                Direction = ParameterDirection.Input,
                Value = requestId
            };
            SqlTransaction transaction = null;
            System.Diagnostics.Debug.WriteLine(sqlquery);
            SqlCommand sqlcomm = new SqlCommand(sql_insert_string, sqlconn, transaction);
            sqlcomm.Parameters.Add(byteParam);
            sqlcomm.Parameters.Add(imageIdParam);
            sqlconn.Open();
            sqlcomm.ExecuteNonQuery();

            

        }


        public void InsertNewBrand(string newBrand)
        {
            string mainconn2 = "Server=localhost\\SQLEXPRESS;Database=PRUV;Trusted_Connection=True;";
            SqlConnection sqlconn = new SqlConnection(mainconn2);
            string sqlquery = $"insert into Brand (Name) values ('{newBrand}')";
            SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
            sqlconn.Open();
            sqlcomm.ExecuteNonQuery();

        }

        // GET: UserRequests/Create
        public IActionResult Create()
        {
             
            ViewBag.BrandList = new SelectList(PopulateDropDown("Brand", 1));
            ViewBag.Locations = new SelectList(PopulateDropDown("Locations", 1), DefaultValue(Global.empLoc, "Locations", "LocationID", 1)) ;
            ViewBag.Employees = new SelectList(PopulateDropDown("StoreUser", 5), DefaultValue(Global.empNum, "StoreUser", "EmpId", 5));

            ViewBag.Cases = new SelectList(PopulateDropDown("CaseOption", 1));
            return View();
        }

        // POST: UserRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RequestID,RequestYear,RequestModel,Serial,UserID,Intiated,InitiatedBy,InitiatedAt,Details,AskingPrice,Cost,Retail,Created")] UserRequest userRequest, string StoreID, string BrandId, string? newBrand, string? CaseId,IFormFile imageFile)
        {
            if(Global.empNum != 0)
            {
                userRequest.UserID = Global.empNum;
            }

            if(Global.empLoc != 0)
            {
                userRequest.StoreID = Global.empLoc;
            }
            else userRequest.StoreID = int.Parse(StoreID);

            if (BrandId == "Other")
            {
                InsertNewBrand(newBrand);
                userRequest.BrandId = GetDBId(newBrand, "Brand");

            }
            else userRequest.BrandId = GetDBId(BrandId, "Brand");

            if(CaseId != null) userRequest.CaseId = GetDBId(CaseId, "CaseOption");
            
            userRequest.RequestID = CreateRequestID(userRequest.StoreID);
            userRequest.Created = DateTime.Now;
            bool state = ModelState.IsValid;
            if (state)
            {
                AddImage(userRequest.RequestID, Global.ConvertImageFile(imageFile));
                _context.Add(userRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            
            ViewBag.Cases = new SelectList(PopulateDropDown("CaseOption", 1));
            ViewBag.BrandList = new SelectList(PopulateDropDown("Brand", 1));
            ViewBag.Locations = new SelectList(PopulateDropDown("Locations", 1));
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
