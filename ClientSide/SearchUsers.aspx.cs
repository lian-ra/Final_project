using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class SearchUsers : System.Web.UI.Page
{
    private localhost.Service myService = new localhost.Service();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["status"] == null || !Session["status"].ToString().Equals("2"))
        {
            string script = @"alert('You are not welcome!'); setTimeout(function() {window.location = 'Login.aspx';}, 10);";
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script, true);
            return;
        }

        if (!IsPostBack)
        {
            LoadAllUsers();
        }
    }

    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        string searchText = TxtSearch.Text.Trim();
        string searchOption = DrpSearch.SelectedValue;

        if (string.IsNullOrEmpty(searchText))
        {
            LoadAllUsers();
            return;
        }

        try
        {
            DataTable dt = myService.SearchUser(searchText, searchOption);
            GrdUsers.DataSource = dt;
            GrdUsers.DataBind();
            pnlModal.Visible = false;
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    protected void BtnReset_Click(object sender, EventArgs e)
    {
        TxtSearch.Text = "";
        DrpSearch.SelectedIndex = 0;
        LoadAllUsers();
        pnlModal.Visible = false;
        GrdUsers.SelectedIndex = -1;
    }

    private void LoadAllUsers()
    {
        try
        {
            DataTable dt = myService.SearchUser("", "");
            GrdUsers.DataSource = dt;
            GrdUsers.DataBind();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    protected void GrdUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (GrdUsers.SelectedRow != null)
        {
            string username = GrdUsers.DataKeys[GrdUsers.SelectedRow.RowIndex].Value.ToString();
            HiddenUsername.Value = username;

            try
            {
                DataTable dt = myService.SearchUser(username, "username");
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    TxtName.Text = row["FName"].ToString();
                    TxtLast.Text = row["LName"].ToString();
                    TxtEmail.Text = row["email"].ToString();
                    TxtAddress.Text = row["address"].ToString();

                    // Handle phone/gender column confusion logic just in case, but usually Service returns raw data
                    if (dt.Columns.Contains("phone"))
                        TxtPhone.Text = row["phone"].ToString();
                    else
                        TxtPhone.Text = "";
                }
            }
            catch { }

            // Show the Modal
            pnlModal.Visible = true;
        }
    }

    protected void BtnUpdateUser_Click(object sender, EventArgs e)
    {
        try
        {
            string username = HiddenUsername.Value;
            if (string.IsNullOrEmpty(username)) return;

            // 1. Fetch current user data to preserve other fields (like password, picture)
            DataTable dt = myService.SearchUser(username, "username");
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                localhost.Users user = new localhost.Users();
                user.UserN = username;

                // Update modified fields from TextBoxes
                user.NameF = TxtName.Text.Trim();
                user.LastN = TxtLast.Text.Trim();
                user.Email = TxtEmail.Text.Trim();
                user.Fulladdres = TxtAddress.Text.Trim();
                user.PhoneN = TxtPhone.Text.Trim();

                // Preserve existing fields (Pass, Pic)
                user.Pass = row["pass"].ToString();

                if (fileUploadPic.HasFile)
                {
                    try
                    {
                        string fileName = System.IO.Path.GetFileName(fileUploadPic.FileName);
                        string savePath = Server.MapPath("~/MyPics/") + fileName; //כדי לדעת איפה נמצא הקובץ- תיקייה
                        fileUploadPic.SaveAs(savePath); //שמירה בתיקייה
                        user.Pic = fileName; //שמירה בכתובת של הקובץ
                    }
                    catch { user.Pic = "Profile.jpg"; }
                }
                else
                {
                    if (row.Table.Columns.Contains("pic"))
                        user.Pic = row["pic"].ToString();
                    else if (row.ItemArray.Length > 9)
                        user.Pic = row[9].ToString();
                    else
                        user.Pic = "Profile.jpg";
                }

                // 2. Send update
                myService.UpdateUser(user);

                // 3. Refresh Grid and Close Modal
                LoadAllUsers();
                pnlModal.Visible = false;

                string script = "alert('User updated successfully!');";
                ClientScript.RegisterStartupScript(this.GetType(), "Success", script, true);
            }
        }
        catch (Exception ex)
        {
            ShowError("Update failed: " + ex.Message);
        }
    }

    protected void BtnDeleteUser_Click(object sender, EventArgs e)
    {
        string username = HiddenUsername.Value;
        if (!string.IsNullOrEmpty(username))
        {
            try
            {
                myService.DeleteUser(username);
                LoadAllUsers();
                pnlModal.Visible = false;
                ClientScript.RegisterStartupScript(this.GetType(), "Deleted", "alert('User deleted successfully.');", true);
            }
            catch (Exception ex)
            {
                ShowError("Delete failed: " + ex.Message);
            }
        }
    }

    protected void BtnClose_Click(object sender, EventArgs e)
    {
        pnlModal.Visible = false;
        GrdUsers.SelectedIndex = -1;
    }

    private void ShowError(string msg)
    {
        string message = "alert('Error: " + msg.Replace("'", "\\'") + "');";
        ClientScript.RegisterStartupScript(this.GetType(), "Error", message, true);
    }

    // --- Helper Methods ---

    protected string GetPhoneValue(object dataItem)
    {
        if (dataItem == null) return "";
        DataRowView row = dataItem as DataRowView;
        if (row == null) return "";

        if (row.Row.Table.Columns.Contains("phone"))
        {
            object objVal = row["phone"];
            if (objVal != null && objVal != DBNull.Value)
            {
                string val = objVal.ToString().Trim();
                if (!string.IsNullOrEmpty(val) && !IsGender(val)) return val;
            }
        }

        if (row.Row.Table.Columns.Contains("gender"))
        {
            object objVal = row["gender"];
            if (objVal != null && objVal != DBNull.Value)
            {
                string val = objVal.ToString().Trim();
                if (!string.IsNullOrEmpty(val) && !IsGender(val) && !IsDate(val)) return val;
            }
        }
        return "";
    }

    protected string GetGenderValue(object dataItem)
    {
        if (dataItem == null) return "";
        DataRowView row = dataItem as DataRowView;
        if (row == null) return "";

        if (row.Row.Table.Columns.Contains("gender"))
        {
            object objVal = row["gender"];
            if (objVal != null && objVal != DBNull.Value)
            {
                string val = objVal.ToString().Trim();
                if (IsGender(val)) return val;
            }
        }

        if (row.Row.Table.Columns.Contains("phone"))
        {
            object objVal = row["phone"];
            if (objVal != null && objVal != DBNull.Value)
            {
                string val = objVal.ToString().Trim();
                if (IsGender(val)) return val;
            }
        }
        return "";
    }

    protected string GetBirthdayValue(object dataItem)
    {
        if (dataItem == null) return "";
        DataRowView row = dataItem as DataRowView;
        if (row == null) return "";

        string[] cols = { "birth", "birthday", "Birth", "Birthday", "phone" };

        foreach (string col in cols)
        {
            if (row.Row.Table.Columns.Contains(col))
            {
                object val = row[col];
                if (val != null && val != DBNull.Value)
                {
                    DateTime date;
                    if (DateTime.TryParse(val.ToString(), out date))
                        return date.ToString("MM/dd/yyyy");
                }
            }
        }
        return "";
    }

    private bool IsGender(string s)
    {
        s = s.ToLower();
        return s == "male" || s == "female";
    }

    private bool IsDate(string s)
    {
        DateTime d;
        return DateTime.TryParse(s, out d);
    }
}