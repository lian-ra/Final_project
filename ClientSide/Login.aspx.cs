using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Login : System.Web.UI.Page
{
    private localhost.Service my_service = new localhost.Service();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string message = "alert('You are Loged out');"; //
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", message, true); //
            Session["status"] = "-1";
            Session["data"] = null;
        }      
    }

    protected void btnsi_Click(object sender, EventArgs e)
    {
        string user = txtuser.Text;
        string pass = txtpass.Text;
        bool choice = true;

        if (drpChoice.Text.Equals("admin"))
            choice = false;

        DataTable dt = my_service.Login(user, pass, choice);

        if (dt.Rows.Count > 0)
        {
            Session["data"] = dt;
            if (drpChoice.Text.Equals("admin"))
            {
                Session["status"] = "2";
                Response.Redirect("AdminArea.aspx");

            }
            else
            {
                Session["status"] = "1";
                Response.Redirect("ClientArea.aspx");
            }
        }
        else
        {
            string message = "alert('You are not welcome');";
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", message, true);
            txtpass.Text = String.Empty;
            txtuser.Text = String.Empty;
            txtuser.Focus();
        }
    }

    protected void btnsu2_Click(object sender, EventArgs e)
    {
        Response.Redirect("Regi.aspx");
    }
}