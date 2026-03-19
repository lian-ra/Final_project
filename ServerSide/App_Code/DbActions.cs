using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
namespace MDb.App_Code
{
    public class DbActions
    {
        private static SqlConnection GetConnection(string path)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + path + "; Integrated Security=True";
            return new SqlConnection(connectionString);
        }

        public static DataTable Search(string sql, string path)
        {
            using (SqlConnection connect = GetConnection(path))
            {
                DataSet ds = new DataSet();
                using (SqlCommand cmmd = new SqlCommand(sql, connect))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmmd))
                    {
                        da.Fill(ds, "MyTable");
                    }
                }

                if (ds.Tables.Count == 0 || ds.Tables["MyTable"] == null)
                    return new DataTable();

                return ds.Tables[0];
            }
        }

        public static DataTable SearchWithParameters(SqlCommand cmmd, string path)
        {
            using (SqlConnection connect = GetConnection(path))
            {
                DataSet ds = new DataSet();
                cmmd.Connection = connect;
                using (SqlDataAdapter da = new SqlDataAdapter(cmmd))
                {
                    da.Fill(ds, "MyTable");
                }

                if (ds.Tables.Count == 0 || ds.Tables["MyTable"] == null)
                    return new DataTable();

                return ds.Tables[0];
            }
        }

        public static void MyAction(SqlCommand cmmd, string path)
        {
            using (SqlConnection connect = GetConnection(path))
            {
                cmmd.Connection = connect;
                connect.Open();
                cmmd.ExecuteNonQuery();
                connect.Close();
            }
        }
    }
}