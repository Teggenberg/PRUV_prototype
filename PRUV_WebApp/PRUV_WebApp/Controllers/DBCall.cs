using Microsoft.Data.SqlClient;
using PRUV_WebApp.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace PRUV_WebApp.Controllers
{
    public static class DBCall
    {
        private static string connectionString = "Server=localhost\\SQLEXPRESS;Database=PRUV;Trusted_Connection=True;";
        public static int GetDBId(string name, string table)
        {
            
            //string mainconn2 = "Server=localhost\\SQLEXPRESS;Database=PRUV;Trusted_Connection=True;";
            SqlConnection sqlconn2 = new SqlConnection(connectionString);
            string sqlquery2 = $"select * from {table} where Name = '{name}'";
            System.Diagnostics.Debug.WriteLine(sqlquery2);
            SqlCommand sqlcomm2 = new SqlCommand(sqlquery2, sqlconn2);
            sqlconn2.Open();
            SqlDataAdapter adapter2 = new SqlDataAdapter(sqlcomm2);
            DataTable dt2 = new DataTable();
            adapter2.Fill(dt2);
            return int.Parse(dt2.Rows[0][0].ToString()!);
        }

        public static List<JoinedRequest> GetJoinedRequests()
        {
            var list = new List<JoinedRequest>();
            //string mainconn = "Server=localhost\\SQLEXPRESS;Database=PRUV;Trusted_Connection=True;";
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
            return list;
        }

        public static JoinedRequest GetItemData(int? id)
        {
            
            // joint query to gather item data from related tables
            //string mainconn = "Server=localhost\\SQLEXPRESS;Database=PRUV;Trusted_Connection=True;";
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
                jr.image = (byte[])dt.Rows[i][8];
            }

            return jr;
        }

        public static List<string> PopulateDropDown(string table, int column)
        {
            // new list that will be retunred for drop down menu
            List<string> list = new List<string>();

            // query table for values to populate list using column and table parameters
            //string mainconn = "Server=localhost\\SQLEXPRESS;Database=PRUV;Trusted_Connection=True;";
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

            return list;
        }

    }

    
}
