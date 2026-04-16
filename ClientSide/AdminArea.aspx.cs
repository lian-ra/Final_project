using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class AdminArea : System.Web.UI.Page
{
    //הפעולה בודקת אם המשתמש המחובר הוא מנהל
    //אם לא, היא חוסמת אותו ומעבירה אותו לדף התחברות ואם כן, היא מציגה את השם שלו באתר.
    //במקרה של תקלה, היא מחזירה אותו לדף הבית
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Session["status"].ToString().Equals("2"))
        {
            string script = @"alert('You are not welcome!'); 
            setTimeout(function() {window.location = 'login.aspx';}, 10);
               // 10 = 10/1000 seconds delay";
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script, true);
        }
        try
        {
            string status = Session["status"] as string;
            if (status == "2")
            {
                Design master = (Design)this.Master; //פעולת המרה שיוצרת קשר בין דף התוכן לבין דף העיצוב הראשי
                DataTable dt = Session["data"] as DataTable;
                if (dt != null && dt.Rows.Count > 0)
                {
                    string username = dt.Columns.Contains("Usern") ? 
                        dt.Rows[0]["Usern"].ToString() : dt.Rows[0][0].ToString();
                    master.SetUserLoggedIn(username, status); 
                }
            }
            else
            {
                Response.Redirect("Home.aspx");
            }
        }
        catch
        {
            Response.Redirect("Home.aspx");
        }
    }
}
