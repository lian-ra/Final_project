using localhost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Regi : System.Web.UI.Page
{
    private localhost.Service my_service= new localhost.Service();
    private localhost.Users user = new localhost.Users();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Populate Years (from today back to 1900)
            for (int i = DateTime.Today.Year; i >= 1900; i--)
            {
                ddlYear.Items.Add(i.ToString());
            }

            // Populate Months (1 to 12)
            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
            for (int i = 1; i <= 12; i++)
            {
                ddlMonth.Items.Add(new ListItem(mfi.GetMonthName(i), i.ToString()));
            }

            DateTime defaultDate = DateTime.Today.AddYears(-18);
            ddlYear.SelectedValue = defaultDate.Year.ToString();
            ddlMonth.SelectedValue = defaultDate.Month.ToString();
            Calendar1.VisibleDate = new DateTime(defaultDate.Year, defaultDate.Month, 1);
        }
    }

    protected void DateDropdown_SelectedIndexChanged(object sender, EventArgs e)
    {
        int year = int.Parse(ddlYear.SelectedValue);
        int month = int.Parse(ddlMonth.SelectedValue);
        // Switch the calendar view
        Calendar1.VisibleDate = new DateTime(year, month, 1);
    }

    protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
    {
        // Block choosing any date after today
        if (e.Day.Date > DateTime.Today)
        {
            e.Day.IsSelectable = false;
            e.Cell.ForeColor = System.Drawing.Color.LightGray;
            e.Cell.ToolTip = "Cannot select future dates";
        }
    }

    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
        // Sync the dropdowns if user picks a date natively on the calendar across months
        ddlYear.SelectedValue = Calendar1.SelectedDate.Year.ToString();
        ddlMonth.SelectedValue = Calendar1.SelectedDate.Month.ToString();
    }

    protected void Calendar1_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        // Sync the dropdowns if user clicks the [<] or [>] arrows on the calendar
        ddlYear.SelectedValue = e.NewDate.Year.ToString();
        ddlMonth.SelectedValue = e.NewDate.Month.ToString();
    }

    protected void RegisterNewUser(object sender, EventArgs e)
    {
        try
        {
            // Hard Backend Validation for Calendar
            if (Calendar1.SelectedDate == DateTime.MinValue)
                throw new Exception("Please explicitly select your birthdate on the calendar.");
            if (Calendar1.SelectedDate > DateTime.Today)
                throw new Exception("Your birthdate cannot be a future date.");

            Users newUser = new Users();//אובייקט חדש       

            newUser.UserN = txtUName.Text;
            newUser.Pass = txtPass.Text;
            newUser.NameF = txtFName.Text;
            newUser.LastN = txtLName.Text;
            newUser.Fulladdres = txtAdd.Text;
            newUser.Email = txtEmail.Text;
            newUser.Gender = dpdphone.SelectedValue;
            newUser.Birthday = Calendar1.SelectedDate.ToString("yyyy-MM-dd");
            newUser.PhoneN = txtPhone.Text;

            string Pic_Name = "Profile.jpg";
            if (FileUpload1.HasFile)
            {
                Pic_Name = FileUpload1.FileName;
                FileUpload1.SaveAs(Server.MapPath("~/MyPics/") + Pic_Name);
            }
            newUser.Pic = Pic_Name;

            my_service.Regi(newUser);

            string script1 = @"
                alert('You are registered !!!!!!!!!!');
                setTimeout(function() {
                    window.location = 'Login.aspx';
                }, 10); // 10 = 10/1000 seconds delay
            ";
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script1, true);

        } // if fails - print correct message to user
        catch (Exception ex)
        {
            string msg = "alert('Error: " + ex.Message.Replace("'", "\\'") + "');"; //הסיבה
                                                                       //למניעת שגיאה בגלל הגרשיים
            ClientScript.RegisterStartupScript(this.GetType(), "Error", msg, true);
        }
    }



 
}

