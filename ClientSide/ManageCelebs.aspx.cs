using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class ManageCelebs : System.Web.UI.Page
{
    private localhost.Service backendService = new localhost.Service();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["status"] == null || !Session["status"].ToString().Equals("2"))
        {
            string script = @"alert('You are not welcome!'); setTimeout(function() {window.location = 'login.aspx';}, 10);";
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script, true);
            return;
        }

        if (!IsPostBack)
        {
            BindCelebsGrid();
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

            backendService.AddCeleb(celeb);

            ShowMessage("Celebrity added successfully.", true);
            ClearForm();
            BindCelebsGrid();
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindCelebsGrid();
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
        lblManageMessage.CssClass = isSuccess ? "message-label message-success" : "message-label message-error";
    }

    private void BindCelebsGrid()
    {
        try
        {
            string searchTerm = txtSearch.Text.Trim();
            // If there's a search term, use SearchCelebs, otherwise GetAllCelebs
            DataTable dt;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                dt = backendService.SearchCelebs(searchTerm, "all");
            }
            else
            {
                dt = backendService.GetAllCelebs();
            }

            grdCelebs.DataSource = dt;
            grdCelebs.DataBind();
        }
        catch (Exception ex)
        {
            ShowMessage("Error loading celebrities list: " + ex.Message, false);
        }
    }

    protected void grdCelebs_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdCelebs.EditIndex = e.NewEditIndex;
        BindCelebsGrid();
    }

    protected void grdCelebs_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdCelebs.EditIndex = -1;
        BindCelebsGrid();
    }

    protected void grdCelebs_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            int celebId = (int)grdCelebs.DataKeys[e.RowIndex].Value;
            GridViewRow row = grdCelebs.Rows[e.RowIndex];

            // Safely get values. Cells indices: 0=Actions, 1=ID, 2=Name, 3=Role, 4=Photo, 5=Bio
            // Note: If you have AutoGenerateColumns=False, the column order matches your ASPX exactly.
            // But usually the first visible column is index 0. 
            // In your code:
            // Col 0: ID
            // Col 1: Name
            // Col 2: Role
            // Col 3: Photo
            // Col 4: Bio
            // Col 5: Actions (CommandField)

            // Wait, looking at your ASPX:
            // <Columns>
            // 0: ID (ReadOnly)
            // 1: Name
            // 2: Role
            // 3: Photo
            // 4: Bio
            // 5: Actions

            // When Editing, BoundFields become TextBoxes.
            // We use Cells[index].Controls[0] to access the TextBox.

            string name = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();
            string role = ((TextBox)row.Cells[2].Controls[0]).Text.Trim();
            string photo = ((TextBox)row.Cells[3].Controls[0]).Text.Trim();
            string bio = ((TextBox)row.Cells[4].Controls[0]).Text.Trim();

            localhost.Celeb celeb = new localhost.Celeb();
            celeb.CelebId = celebId;
            celeb.Name = name;
            celeb.Role = role;
            celeb.Photo = photo;
            celeb.Bio = bio;

            backendService.UpdateCeleb(celeb);

            grdCelebs.EditIndex = -1;
            BindCelebsGrid();
            ShowMessage("Celebrity updated successfully.", true);
        }
        catch (Exception ex)
        {
            ShowMessage("Error updating celebrity: " + ex.Message, false);
        }
    }

    protected void grdCelebs_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int celebId = (int)grdCelebs.DataKeys[e.RowIndex].Value;
            backendService.DeleteCeleb(celebId);
            BindCelebsGrid();
            ShowMessage("Celebrity deleted successfully.", true);
        }
        catch (Exception ex)
        {
            ShowMessage("Error deleting celebrity: " + ex.Message, false);
        }
    }
}
