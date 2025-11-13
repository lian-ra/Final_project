using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class ClientArea : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string status = Session["status"] as string;
            if (status == "1")
            {
                // User is logged in
                Design master = (Design)this.Master;
                DataTable dt = Session["data"] as DataTable;
                if (dt != null && dt.Rows.Count > 0)
                {
                    string username = dt.Columns.Contains("User") ? dt.Rows[0]["User"].ToString() : dt.Rows[0][0].ToString();
                    master.SetUserLoggedIn(username);
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
        catch
        {
            Response.Redirect("Login.aspx");
        }
    }
}
