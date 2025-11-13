using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Design : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Empty - child pages will call SetUserLoggedIn or SetUserLoggedOut
    }

    public void SetUserLoggedIn(string username)
    {
        try
        {
            if (liLoggedOut != null) liLoggedOut.Visible = false;
            if (liSignUp != null) liSignUp.Visible = false;
            if (liLoggedIn != null) liLoggedIn.Visible = true;
            if (liLogout != null) liLogout.Visible = true;
            if (lblUsername != null) lblUsername.Text = username;
        }
        catch { }
    }

    public void SetUserLoggedOut()
    {
        try
        {
            if (liLoggedOut != null) liLoggedOut.Visible = true;
            if (liSignUp != null) liSignUp.Visible = true;
            if (liLoggedIn != null) liLoggedIn.Visible = false;
            if (liLogout != null) liLogout.Visible = false;
        }
        catch { }
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session["status"] = "-1";
        Session["data"] = null;
        Response.Redirect("Login.aspx");
    }
}
