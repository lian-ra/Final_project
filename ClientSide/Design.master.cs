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
        try
        {
            string status = Session["status"] as string;

            DataTable dt = Session["data"] as DataTable;

            if ((status == "1" || status == "2") && dt != null && dt.Rows.Count > 0)
            {
                string username = null;

                if (dt.Columns.Contains("User"))
                {
                    username = dt.Rows[0]["User"].ToString();
                }
                else if (dt.Columns.Contains("Usern"))
                {
                    username = dt.Rows[0]["Usern"].ToString();
                }
                else
                {
                    username = dt.Rows[0][0].ToString();
                }

                if (!string.IsNullOrEmpty(username))
                {
                    SetUserLoggedIn(username, status);
                }
                else
                {
                    SetUserLoggedOut();
                }
            }
            else
            {
                SetUserLoggedOut();
            }
        }
        catch
        {
            // In case of any unexpected issue with Session/DataTable, default to logged-out UI
            SetUserLoggedOut();
        }
    }

    public void SetUserLoggedIn(string username, string status)
    {
        try
        {
            if (liLoggedOut != null) liLoggedOut.Visible = false;
            if (liSignUp != null) liSignUp.Visible = false;
            if (liLoggedIn != null) liLoggedIn.Visible = true;
            if (liLogout != null) liLogout.Visible = true;
            if (lblUsername != null) lblUsername.Text = username;
            if (liAdminArea != null) liAdminArea.Visible = (status == "2");
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
            if (liAdminArea != null) liAdminArea.Visible = false;
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
