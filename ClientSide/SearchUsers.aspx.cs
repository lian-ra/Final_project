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
            detailsSection.Visible = false;
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
        detailsSection.Visible = false;
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
            // Note: Indices depend on visible columns. 
            // In ASPX: Col 0 is button, Col 1 is Username, Col 2 is FName, Col 3 is LName
            TxtName.Text = GrdUsers.SelectedRow.Cells[2].Text; // First Name
            TxtLast.Text = GrdUsers.SelectedRow.Cells[3].Text; // Last Name

            detailsSection.Visible = true;
        }
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