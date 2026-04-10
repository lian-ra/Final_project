using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class FindUsers : System.Web.UI.Page
{
    private localhost.Service backendService = new localhost.Service();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["status"] == null || (Session["status"].ToString() != "1" && Session["status"].ToString() != "2"))
        {
            Response.Redirect("Login.aspx");
            return;
        }

        if (!IsPostBack)
        {
            LoadUsers("");
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadUsers(txtSearch.Text.Trim());
    }

    private void LoadUsers(string searchTerm)
    {
        try
        {
            DataTable dt;

            if (string.IsNullOrEmpty(searchTerm))
            {
                // Load all users (using SearchUser with empty strings usually returns all)
                dt = backendService.SearchUser("", "");
            }
            else
            {
                dt = backendService.SearchUser(searchTerm, "username");

                if (dt == null || dt.Rows.Count == 0)
                {
                    dt = backendService.SearchUser(searchTerm, "name"); //חיפוש לפי שם פרטי
                }
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                rptUsers.DataSource = dt;
                rptUsers.DataBind();
                lblNoResults.Visible = false;
            }
            else
            {
                rptUsers.DataSource = null;
                rptUsers.DataBind();
                lblNoResults.Visible = true;
            }
        }
        catch (Exception)
        {
            // Ideally log error
            lblNoResults.Text = "Error loading users.";
            lblNoResults.Visible = true;
        }
    }
}
