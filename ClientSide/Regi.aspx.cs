using localhost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Regi : System.Web.UI.Page
{
    private localhost.Service backendService= new localhost.Service();
    private localhost.Users user = new localhost.Users();

    //הפעולה מסדרת את כל התיבות והרשימות בדף רגע לפני שהמשתמש רואה אותם
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            for (int i = DateTime.Today.Year; i >= 1900; i--)
            {
                ddlYear.Items.Add(i.ToString());
            }

            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo(); //מילון של תאריכים
            for (int i = 1; i <= 12; i++)
            {
                ddlMonth.Items.Add(new ListItem(mfi.GetMonthName(i), i.ToString()));
            }

            DateTime defaultDate = DateTime.Today.AddYears(-18);
            ddlYear.SelectedValue = defaultDate.Year.ToString();
            ddlMonth.SelectedValue = defaultDate.Month.ToString();
            Calendar1.VisibleDate = new DateTime(defaultDate.Year, defaultDate.Month, 1); //18 שנה קודם ישר
        }
    }

    //הפעולה מבצעת עדכון אוטומטי של לוח השנה לפי הבחירה של המשתמש.
    protected void DateDropdown_SelectedIndexChanged(object sender, EventArgs e)
    {
        int year = int.Parse(ddlYear.SelectedValue);
        int month = int.Parse(ddlMonth.SelectedValue);
        Calendar1.VisibleDate = new DateTime(year, month, 1);
    }

    //הפעולה מבצעת חסימה של תאריכים עתידיים בלוח השנה.
    protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
    {
        if (e.Day.Date > DateTime.Today)
        {
            e.Day.IsSelectable = false; //לא ניתן לבחור
            e.Cell.ForeColor = System.Drawing.Color.LightGray;
            e.Cell.ToolTip = "Cannot select future dates";
        }
    }

    //הפעולה מבצעת סנכרון מלוח השנה לרשימות הבחירה.
    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
        ddlYear.SelectedValue = Calendar1.SelectedDate.Year.ToString();
        ddlMonth.SelectedValue = Calendar1.SelectedDate.Month.ToString();
    }

    //הפעולה מבצעת עדכון של רשימות הבחירה בזמן דפדוף בלוח השנה.
    protected void Calendar1_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        ddlYear.SelectedValue = e.NewDate.Year.ToString();
        ddlMonth.SelectedValue = e.NewDate.Month.ToString();
    }

    //הפעולה  מבצעת רישום של משתמש חדש למערכת.
    protected void RegisterNewUser(object sender, EventArgs e)
    {
        try
        {
            if (Calendar1.SelectedDate == DateTime.MinValue) //לאם המשתמש בחר תאריך שבערך המינימלי בלוח שנה 
                throw new Exception("Please explicitly select your birthdate on the calendar.");
            if (Calendar1.SelectedDate > DateTime.Today)
                throw new Exception("Your birthdate cannot be a future date.");

            Users newUser = new Users(); //אובייקט חדש של יוזר       

            newUser.UserN = txtUName.Text;
            newUser.Pass = txtPass.Text;
            newUser.NameF = txtFirstNameName.Text;
            newUser.LastN = txtLastNameName.Text;
            newUser.Fulladdres = txtAddress.Text;
            newUser.Email = txtEmailail.Text;
            newUser.Gender = dpdphone.SelectedValue;
            newUser.Birthday = Calendar1.SelectedDate.ToString("yyyy-MM-dd");
            newUser.PhoneN = txtPhoneone.Text;

            string Pic_Name = "Profile.jpg";
            if (FileUpload1.HasFile)
            {
                Pic_Name = FileUpload1.FileName;
                FileUpload1.SaveAs(Server.MapPath("~/MyPics/") + Pic_Name);
            }
            newUser.Pic = Pic_Name;
            backendService.Regi(newUser);

            string script1 = @"
                alert('You are registered !!!!!!!!!!');
                setTimeout(function() {
                    window.location = 'Login.aspx';
                }, 10); // 10 = 10/1000 seconds delay
            ";
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script1, true);
        } 
        catch (Exception ex)
        {
            string msg = "alert('Error: " + ex.Message.Replace("'", "\\'") + "');";                                                                
            ClientScript.RegisterStartupScript(this.GetType(), "Error", msg, true);
        }
    }
}

