using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UpdateMyUser : System.Web.UI.Page
{
    private DataTable dtUser;
    private localhost.Users user = new localhost.Users(); 
    private localhost.Service backendService = new localhost.Service();
    private string pic;

    //הפעולה בודקת אם המשתמש מחובר עם הרשאות מתאימות, אם כן היא טוענת את פרטיו האישיים מהזיכרון
    //ומציגה אותם בטפסים ובתמונת הפרופיל עם טעינת הדף.
    protected void Page_Load(object sender, EventArgs e)
    {
        string status = Session["status"] as string;
        if (status != "1" && status != "2")
        {
            string script = @"alert('You are not welcome!'); setTimeout(function() 
                            {window.location = 'Login.aspx';}, 10);";
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script, true);
            return; 
        }

        if (!Page.IsPostBack)
        {
            if (Session["data"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            dtUser = (DataTable)Session["data"];

            if (dtUser == null || dtUser.Rows.Count == 0)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            txtUsername.Text = dtUser.Rows[0]["User"].ToString();
            txtPassword.Text = dtUser.Rows[0]["pass"].ToString();
            txtFirstName.Text = dtUser.Rows[0]["FName"].ToString();
            txtLastName.Text = dtUser.Rows[0]["LName"].ToString();
            txtAddress.Text = dtUser.Rows[0]["address"].ToString();
            txtEmail.Text = dtUser.Rows[0]["email"].ToString();

            string phoneValue = dtUser.Rows[0]["phone"].ToString();
            txtPhone.Text = phoneValue;

            if (dtUser.Columns.Contains("pic"))
            {
                pic = dtUser.Rows[0]["pic"].ToString();
            }
            else if (dtUser.Rows[0].ItemArray.Length > 9)
            {
                pic = dtUser.Rows[0][9].ToString();
            }

            //הגדרה מאיזה כתובת להביא את התמונה
            img.ImageUrl = "~/MyPics/" + (string.IsNullOrEmpty(pic) ? "Profile.jpg" : pic);
        }
    }

    //הפעולה מעדכנת את פרטי המשתמש במסד הנתונים
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (Session["data"] != null)
        {
            dtUser = (DataTable)Session["data"];
        }
        user.UserN = txtUsername.Text;
        user.Pass = txtPassword.Text;
        user.Fulladdres = txtAddress.Text;
        user.Email = txtEmail.Text;
        user.PhoneN = txtPhone.Text;
        user.NameF = txtFirstName.Text;
        user.LastN = txtLastName.Text;
        if (dtUser != null && dtUser.Rows.Count > 0)
        {
            if (dtUser.Columns.Contains("pic")) pic = dtUser.Rows[0]["pic"].ToString();
            else if (dtUser.Rows[0].ItemArray.Length > 9) pic = dtUser.Rows[0][9].ToString();
        }
        if (string.IsNullOrEmpty(pic))
            pic = "Profile.jpg";

        if (FileUpload1.HasFile)
        {
            pic = FileUpload1.FileName;
            FileUpload1.SaveAs(Server.MapPath("~/MyPics/") + pic);
        }
        user.Pic = pic;

        dtUser = backendService.UpdateUser(user);
        Session["data"] = dtUser;

        string script = @"alert('Profile Updated Successfully!'); 
                    setTimeout(function() { window.location = 'ClientArea.aspx'; }, 100);";
        ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script, true);
    }
}
