using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ManageCelebs : System.Web.UI.Page
{
    private localhost.Service myService = new localhost.Service();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Only admins (status "2") can access this page
            string status = Session["status"] as string;
            if (status != "2")
            {
                Response.Redirect("Login.aspx");
            }
        }
    }

    protected void btnAddCeleb_Click(object sender, EventArgs e)
    {
        try
        {
            string name = txtCelebName.Text.Trim();
            string role = ddlCelebRole.SelectedValue;
            string photo = txtPhotoUrl.Text.Trim();
            string bio = txtBio.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                ShowMessage("Name is required.", false);
                return;
            }

            localhost.Celeb celeb = new localhost.Celeb();
            celeb.CelebId = 0;
            celeb.Name = name;
            celeb.Role = role;
            celeb.Photo = string.IsNullOrEmpty(photo) ? "images/uploads/ava1.jpg" : photo;
            celeb.Bio = bio;

            myService.AddCeleb(celeb);

            ShowMessage("Celebrity added successfully.", true);
            ClearForm();
        }
        catch (Exception ex)
        {
            ShowMessage("Error adding celebrity: " + ex.Message, false);
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearForm();
        lblManageMessage.Visible = false;
    }

    private void ClearForm()
    {
        txtCelebName.Text = "";
        ddlCelebRole.SelectedIndex = 0;
        txtPhotoUrl.Text = "";
        txtBio.Text = "";
    }

    private void ShowMessage(string message, bool isSuccess)
    {
        lblManageMessage.Text = message;
        lblManageMessage.Visible = true;
        if (isSuccess)
        {
            lblManageMessage.CssClass = "message-label message-success";
        }
        else
        {
            lblManageMessage.CssClass = "message-label message-error";
        }
    }
}
