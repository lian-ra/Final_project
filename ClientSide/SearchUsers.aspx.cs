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
        if (!Session["status"].ToString().Equals("2"))
        {
            string script = @"alert('You are not welcome!'); setTimeout(function() {window.location = 'login.aspx';}, 10); // 10 = 10/1000 seconds delay";
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script, true);
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
            // If search text is empty, show all users
            LoadAllUsers();
            return;
        }

        try
        {
            DataTable dt = myService.SearchUser(searchText, searchOption);
            GrdUsers.DataSource = dt;
            GrdUsers.DataBind();
        }
        catch (Exception ex)
        {
            string message = "alert('Error searching users: " + ex.Message.Replace("'", "\\'") + "');";
            ClientScript.RegisterStartupScript(this.GetType(), "Error", message, true);
        }
    }

    protected void BtnReset_Click(object sender, EventArgs e)
    {
        TxtSearch.Text = "";
        DrpSearch.SelectedIndex = 0;
        LoadAllUsers();
    }

    private void LoadAllUsers()
    {
        try
        {
            DataTable dt = myService.SearchUser("", "");
            
            // Debug: Check column names (remove this after testing)
            if (dt != null && dt.Rows.Count > 0 && dt.Columns.Count > 0)
            {
                System.Diagnostics.Debug.WriteLine("Columns in DataTable:");
                foreach (DataColumn col in dt.Columns)
                {
                    System.Diagnostics.Debug.WriteLine("Column " + col.Ordinal + ": " + col.ColumnName + " (Type: " + col.DataType + ")");
                }
            }
            
            GrdUsers.DataSource = dt;
            GrdUsers.DataBind();
        }
        catch (Exception ex)
        {
            string message = "alert('Error loading users: " + ex.Message.Replace("'", "\\'") + "');";
            ClientScript.RegisterStartupScript(this.GetType(), "Error", message, true);
        }
    }

    protected void GrdUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Handle row selection if needed

        TxtName.Text = GrdUsers.SelectedRow.Cells[3].Text;
        TxtLast.Text = GrdUsers.SelectedRow.Cells[4].Text;

    }

    protected string GetPhoneValue(object dataItem)
    {
        if (dataItem == null) return "";
        
        DataRowView row = dataItem as DataRowView;
        if (row == null) return "";
        
        // Try phone column first
        if (row.Row.Table.Columns.Contains("phone"))
        {
            object val = row["phone"];
            if (val != null && val != DBNull.Value)
            {
                string str = val.ToString().Trim();
                // If it looks like a date, it's probably the wrong column - try gender column
                DateTime testDate;
                if (DateTime.TryParse(str, out testDate))
                {
                    // Phone column has date, so phone might be in gender column
                    if (row.Row.Table.Columns.Contains("gender"))
                    {
                        object genderVal = row["gender"];
                        if (genderVal != null && genderVal != DBNull.Value)
                        {
                            string genderStr = genderVal.ToString().Trim();
                            // If gender column doesn't look like a date and isn't Male/Female, it might be phone
                            if (!DateTime.TryParse(genderStr, out testDate) && 
                                genderStr != "Male" && genderStr != "Female" && 
                                genderStr != "male" && genderStr != "female" &&
                                genderStr != "MALE" && genderStr != "FEMALE")
                            {
                                return genderStr; // This is probably the phone number
                            }
                        }
                    }
                    return ""; // Wrong column, skip
                }
                // If it's not Male/Female, return it as phone
                if (str != "Male" && str != "Female" && str != "male" && str != "female" &&
                    str != "MALE" && str != "FEMALE")
                {
                    return str; // This looks like a phone number
                }
            }
        }
        
        // If phone column is empty or has gender value, check gender column
        if (row.Row.Table.Columns.Contains("gender"))
        {
            object val = row["gender"];
            if (val != null && val != DBNull.Value)
            {
                string str = val.ToString().Trim();
                DateTime testDate;
                // If it doesn't look like a date and isn't Male/Female, it might be phone
                if (!DateTime.TryParse(str, out testDate) && 
                    str != "Male" && str != "Female" && 
                    str != "male" && str != "female" &&
                    str != "MALE" && str != "FEMALE")
                {
                    return str; // This might be the phone number
                }
            }
        }
        
        return "";
    }
    
    protected string GetBirthdayValue(object dataItem)
    {
        if (dataItem == null) return "";
        
        DataRowView row = dataItem as DataRowView;
        if (row == null) return "";
        
        // Try different possible column names for birthday
        string[] possibleNames = { "birth", "birthday", "Birth", "Birthday" };
        
        foreach (string colName in possibleNames)
        {
            if (row.Row.Table.Columns.Contains(colName))
            {
                object val = row[colName];
                if (val != null && val != DBNull.Value)
                {
                    try
                    {
                        DateTime date = Convert.ToDateTime(val);
                        return date.ToString("MM/dd/yyyy");
                    }
                    catch
                    {
                        // Not a date, try next column
                    }
                }
            }
        }
        
        // Also check if phone column actually contains birthday data
        if (row.Row.Table.Columns.Contains("phone"))
        {
            object val = row["phone"];
            if (val != null && val != DBNull.Value)
            {
                try
                {
                    DateTime date = Convert.ToDateTime(val);
                    return date.ToString("MM/dd/yyyy");
                }
                catch
                {
                    // Not a date
                }
            }
        }
        
        return "";
    }
    
    protected string GetGenderValue(object dataItem)
    {
        if (dataItem == null) return "";
        
        DataRowView row = dataItem as DataRowView;
        if (row == null) return "";
        
        // Check gender column first
        if (row.Row.Table.Columns.Contains("gender"))
        {
            object val = row["gender"];
            if (val != null && val != DBNull.Value)
            {
                string str = val.ToString().Trim();
                // If it's Male/Female, return it
                if (str == "Male" || str == "Female" || str == "male" || str == "female" || 
                    str == "MALE" || str == "FEMALE")
                {
                    return str;
                }
            }
        }
        
        // Check phone column as fallback (in case columns are swapped)
        if (row.Row.Table.Columns.Contains("phone"))
        {
            object val = row["phone"];
            if (val != null && val != DBNull.Value)
            {
                string str = val.ToString().Trim();
                // Only return if it's actually Male/Female, not a phone number
                if (str == "Male" || str == "Female" || str == "male" || str == "female" || 
                    str == "MALE" || str == "FEMALE")
                {
                    return str;
                }
            }
        }
        
        return "";
    }

    protected void TxtLast_TextChanged(object sender, EventArgs e)
    {

    }
}