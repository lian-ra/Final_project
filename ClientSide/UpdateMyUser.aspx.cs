using localhost;//
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using localhost;

public partial class UpdateMyUser : System.Web.UI.Page
{
    private DataTable dtUser;
    private localhost.Users user = new localhost.Users();
    private localhost.Service my_service = new localhost.Service(); //my_

    //private static string pic;
    private string pic;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["data"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            dtUser = (DataTable)Session["data"];//מקבל את כל הנתונים שהכניסו 

            if (dtUser == null || dtUser.Rows.Count == 0)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // Access by column name instead of index for safety
            txtUName.Text = dtUser.Rows[0]["User"].ToString();//שם משתמש 
            txtPass.Text = dtUser.Rows[0]["pass"].ToString();//סיסמא     
            txtFName.Text = dtUser.Rows[0]["FName"].ToString();     
            txtLName.Text = dtUser.Rows[0]["LName"].ToString();    
            txtAdd.Text = dtUser.Rows[0]["address"].ToString();//כתובת  
            txtEmail.Text = dtUser.Rows[0]["email"].ToString();
            string phoneValue = dtUser.Rows[0]["phone"].ToString();
            if (phoneValue.Length >= 10)
            {
                txtPhone.Text = phoneValue.Substring(3, 7);
            }
            else
            {
                txtPhone.Text = phoneValue;
            }
            if (dtUser.Columns.Contains("pic"))
            {
                pic = dtUser.Rows[0]["pic"].ToString();
            }
            else if (dtUser.Rows[0].ItemArray.Length > 9)
            {
                pic = dtUser.Rows[0][9].ToString();
            }
            //img.ImageUrl = "~/MyPics/Profile.jpg" + pic;
        }
    }



    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        // Initialize dtUser from Session if not already set
        if (dtUser == null && Session["data"] != null)
        {
            dtUser = (DataTable)Session["data"];
        }

        user.UserN = txtUName.Text;
        user.Pass = txtPass.Text;
        user.Fulladdres = txtAdd.Text;
        user.Email = txtEmail.Text;
        user.PhoneN = txtPhone.Text;
        user.NameF = txtFName.Text;
        user.LastN = txtLName.Text;

         //user.Pic = pic;
        if (dtUser != null && dtUser.Rows.Count > 0)
        {
            if (dtUser.Columns.Contains("pic"))
            {
                pic = dtUser.Rows[0]["pic"].ToString(); //--
            }
            else if (dtUser.Rows[0].ItemArray.Length > 9)
            {
                pic = dtUser.Rows[0][9].ToString();
            }
        }
        else if (string.IsNullOrEmpty(pic))
        {
            pic = "Profile.jpg"; // Default value
        }

        if (FileUpload1.HasFile == true)
        {
            pic = FileUpload1.FileName;
            FileUpload1.SaveAs(Server.MapPath("~/MyPics/") + pic);
        }
        user.Pic = pic;

        dtUser = my_service.UpdateUser(user);
        Session["data"] = dtUser;




        //string message = "alert('You are updated!!!!!!!');";
        //ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", message, true);

        string script = @"
    alert('You are Update !!!!!!!!!!');
    setTimeout(function() {
        window.location = 'Login.aspx';
    }, 10); // 10 = 10/1000 seconds delay
";
        ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script, true);

    }


}