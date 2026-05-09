using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Login : System.Web.UI.Page
{
    private localhost.Service backendService = new localhost.Service();

    //הפעולה מבצעת ניתוק משתמש ברגע שהדף נטען לראשונה
    protected void Page_Load(object sender, EventArgs e)
    {      
        if (!IsPostBack)
        {
            string message = "alert('You are Loged out');"; 
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", message, true); 
            Session["status"] = "-1";
            Session["data"] = null;
        }
       

    }

    //הפעולה מבצעת אימות כניסה למערכת
    protected void AttemptLogin(object sender, EventArgs e)
    {
        string user = txtUsername.Text;
        string pass = txtPassword.Text;
        bool choice = true; // האם המשתמש רגיל

        if (drpChoice.Text.Equals("admin")) 
            choice = false;

        DataTable dt = backendService.Login(user, pass, choice); 

        if (dt.Rows.Count > 0) 
        {
            Session["data"] = dt;
            if (drpChoice.Text.Equals("admin"))
            {
                Session["status"] = "2";
                Response.Redirect("AdminArea.aspx");

            }
            else
            {
                Session["status"] = "1";
                Response.Redirect("ClientArea.aspx");
            }
        }
        else
        {
            string message = "alert('You are not welcome');";
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", message, true);
            txtPassword.Text = String.Empty;
            txtUsername.Text = String.Empty;
            txtUsername.Focus(); //להקליד מיד במקום ללחוץ על התיבה
        }
    }

    //הפעולה מבצעת מעבר לדף ההרשמה
    protected void MoveToRegister_Click(object sender, EventArgs e)
    {
        Response.Redirect("Regi.aspx");
    }
}
