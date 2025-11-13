using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class AdminArea : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string status = Session["status"] as string;
            if (status == "2")
            {
                // Admin is logged in
                Design master = (Design)this.Master;
                DataTable dt = Session["data"] as DataTable;
                if (dt != null && dt.Rows.Count > 0)
                {
                    string username = dt.Columns.Contains("Usern") ? dt.Rows[0]["Usern"].ToString() : dt.Rows[0][0].ToString();
                    master.SetUserLoggedIn(username);
                }
            }
            else
            {
                Response.Redirect("Home.aspx");
            }
        }
        catch
        {
            Response.Redirect("Home.aspx");
        }
    }
}
