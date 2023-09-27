using Microsoft.Data.SqlClient;
using PRUV_WebApp.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Net.NetworkInformation;

namespace PRUV_WebApp.Controllers
{
    public static class DBCall
    {
        private static string connectionString = "Server=localhost\\SQLEXPRESS;Database=PRUV;Trusted_Connection=True;";
        public static int GetDBId(string name, string table)
        {
            SqlConnection sqlconn2 = new SqlConnection(connectionString);
            string sqlquery2 = $"select * from {table} where Name = '{name}'";
            System.Diagnostics.Debug.WriteLine(sqlquery2);
            SqlCommand sqlcomm2 = new SqlCommand(sqlquery2, sqlconn2);
            sqlconn2.Open();
            SqlDataAdapter adapter2 = new SqlDataAdapter(sqlcomm2);
            DataTable dt2 = new DataTable();
            adapter2.Fill(dt2);
            sqlconn2.Close();
            return int.Parse(dt2.Rows[0][0].ToString()!);
            
        }

        public static List<JoinedRequest> GetJoinedRequestsIndex()
        {
            var list = new List<JoinedRequest>();
            SqlConnection sqlconn = new SqlConnection(connectionString);
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
            sqlconn.Close();
            return list;
        }

        public static JoinedRequest GetJoinedRequestDetails(int? id)
        {
            
            // joint query to gather item data from related tables
            SqlConnection sqlconn = new SqlConnection(connectionString);
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
                
            }
            sqlconn.Close();
            jr.images = GetRequestImages(jr.RequestID);
            return jr;
        }

        public static List<byte[]>? GetRequestImages(int requestId)
        {
            List<byte[]> imageList = new List<byte[]>();

            SqlConnection sqlconn = new SqlConnection(connectionString);
            string sqlquery = $"select RequestImage from RequestImage where RequestId = {requestId};";
            JoinedRequest jr = new JoinedRequest();
            SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
            sqlconn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            for(
                int i = 0; i < dt.Rows.Count; i++)
            {
                imageList.Add((byte[])dt.Rows[i][0]);
            }


            return imageList;
        }

        public static JoinedRequest GetEmailInfo(int id)
        {
            // joint query to gather item data from related tables
            SqlConnection sqlconn = new SqlConnection(connectionString);
            string sqlquery = "select UserRequest.Id, UserRequest.RequestId, StoreID, RequestYear, Brand.Name, RequestModel, CaseOption.Name, CONVERT(VARCHAR(19),Created,100), FirstName, StoreUser.Email" +
                "\r\nfrom UserRequest" +
                "\r\nJoin Brand on UserRequest.BrandId = Brand.Id" +
                "\r\nLeft Join CaseOption on UserRequest.CaseId = CaseOption.Id" +
                "\r\nLeft Join StoreUser on UserRequest.UserID = StoreUser.empId" +
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
                jr.UserName = dt.Rows[i][8].ToString();
                jr.Email = dt.Rows[i][9].ToString();
            }
            sqlconn.Close();
            return jr;

        }

        public static List<string> PopulateDropDown(string table, int column)
        {
            // new list that will be retunred for drop down menu
            List<string> list = new List<string>();

            // query table for values to populate list using column and table parameters
            SqlConnection sqlconn = new SqlConnection(connectionString);
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
            sqlconn.Close();
            return list;
        }

        public static string SetDefaultValue(int id, string table, string columnName, int column)
        {

            SqlConnection sqlconn = new SqlConnection(connectionString);
            string sqlquery = $"select * from {table} where {columnName} = '{id}'";
            SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
            sqlconn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            sqlconn.Close();
            return dt.Rows[0][column].ToString()!;
           
        }

        public static void AddImageToDB(int? requestId, byte[]? image)
        {

            SqlConnection sqlconn = new SqlConnection(connectionString);
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
            SqlCommand sqlcomm = new SqlCommand(sql_insert_string, sqlconn, transaction);
            sqlcomm.Parameters.Add(byteParam);
            sqlcomm.Parameters.Add(imageIdParam);
            sqlconn.Open();
            sqlcomm.ExecuteNonQuery();
            sqlconn.Close();
        }

        public static void InsertNewBrand(string newBrand)
        {

            SqlConnection sqlconn = new SqlConnection(connectionString);
            string sqlquery = $"insert into Brand (Name) values ('{newBrand}')";
            SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
            sqlconn.Open();
            sqlcomm.ExecuteNonQuery();
            sqlconn.Close();

        }

        public static int CreateRequestID(int loc)
        {
            // query gathers the count of requests for given locoation to assign a new RequestId unique to
            // the location
            int id = 0;
            SqlConnection sqlconn = new SqlConnection(connectionString);
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
            sqlconn.Close();
            id += loc * 100000;
            return id;
        }

    }

    
}
