using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class ClientArea : System.Web.UI.Page
{
    //Master Page הפעולה מוודאת שהמשתמש מחובר, מעבירה משתמשים לא מורשים לדף ההתחברות, ומעדכנת את דף
    //בפרטי המשתמש המחובר כדי להציג לו תצוגה אישית

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["status"] == null || !Session["status"].ToString().Equals("1"))
        {
            string script = @"alert('You are not welcome!'); setTimeout(function()
                            {window.location = 'Login.aspx';}, 10);";
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script, true);
            return;
        }
        try
        {
            string status = Session["status"] as string;
            if (status == "1")
            {
                Design master = (Design)this.Master; //פעולת המרה שיוצרת קשר בין דף התוכן לבין דף העיצוב הראשי
                DataTable dt = Session["data"] as DataTable;
                if (dt != null && dt.Rows.Count > 0)
                {
                    string username = dt.Columns.Contains("User") ?
                        dt.Rows[0]["User"].ToString() : dt.Rows[0][0].ToString();
                    master.SetUserLoggedIn(username, status);
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
        catch
        {
            Response.Redirect("Login.aspx");
        }
    }

    //הפעולה מאפסת את טבלת רשימת המשאלות דרך השירות,
    //ומקפיצה הודעה למשתמש המאשרת שהאיפוס הצליח או הודעת שגיאה במקרה של תקלה
    protected void btnResetWishlist_Click(object sender, EventArgs e)
    {
        try
        {
            localhost.Service service = new localhost.Service();
            service.ResetWishlistTable();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", 
                "alert('Wishlist table has been reset successfully.');", true);
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert",
                "alert('An error occurred: " + ex.Message.Replace("'", "\\'") + "');", true);
        }
    }
}