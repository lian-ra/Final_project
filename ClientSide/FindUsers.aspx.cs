using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class FindUsers : System.Web.UI.Page
{
    private localhost.Service myService = new localhost.Service();

    protected void Page_Load(object sender, EventArgs e)
    {
        // Optional: Check if user is logged in (Status 1 or 2)
        // If you want this page to be public, remove this check.
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
                dt = myService.SearchUser("", "");
            }
            else
            {
                // Search by name (you can assume 'name' matches First or Last name logic in your service)
                // Or you can create a specific generic search in your service.
                // Here we try searching by 'username' first, if your service supports it.
                // Assuming SearchUser(value, column) format based on your previous code.

                dt = myService.SearchUser(searchTerm, "username");

                // Optional: If no results by username, try by name (if your logic allows multiple calls)
                if (dt == null || dt.Rows.Count == 0)
                {
                    dt = myService.SearchUser(searchTerm, "name");
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