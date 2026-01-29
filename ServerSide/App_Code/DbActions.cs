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
        private static SqlConnection connect;
        //private static OleDbConnection connect;

        private static void Connect_Me(string path)
        {
            //string str_connection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Persist Security Info=False;";

            //string str = "@\"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Db.mdf;Integrated Security=True;";
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + path + "; Integrated Security=True";

            connect = new SqlConnection(connectionString);

            //connect = new OleDbConnection(str_connection);

        }

        public static DataTable Search(string sql,string path)
        {
            // General search in any sql table
            Connect_Me(path);

            DataSet ds = new DataSet();

            SqlCommand cmmd = new SqlCommand(sql, connect);
            
            SqlDataAdapter da = new SqlDataAdapter(cmmd);

            da.Fill(ds,"MyTable");

            if (ds.Tables.Count == 0 || ds.Tables["MyTable"] == null)
            {
                return new DataTable();
            }

            DataTable dt = ds.Tables[0];

           return dt;


        }

        public static DataTable SearchWithParameters(SqlCommand cmmd, string path)
        {
            // general search with any parameters
            Connect_Me(path);

            DataSet ds = new DataSet();

            cmmd.Connection = connect;
            
            SqlDataAdapter da = new SqlDataAdapter(cmmd);

            da.Fill(ds,"MyTable");

            if (ds.Tables.Count == 0 || ds.Tables["MyTable"] == null)
            {
                return new DataTable();
            }

            DataTable dt = ds.Tables[0];

            return dt;
        }

        //Insert,Update,Delete

        //public static void MyAction(string sql, string path)
        //{
        //    Connect_Me(path);

        //    SqlCommand myCommand = new SqlCommand(sql, connect);

        //    connect.Open();

        //    myCommand.ExecuteNonQuery();

        //    connect.Close();
        //}



        //הערה לבדיקה 
        public static void MyAction(SqlCommand cmmd, string path)
        {
            Connect_Me(path);

            //SqlCommand myCommand = new SqlCommand(sql, connect);

            cmmd.Connection = connect;
            connect.Open();

            cmmd.ExecuteNonQuery();

            connect.Close();
        }


    }
}