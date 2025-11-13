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

    }
    //    protected void btnsave_Click(object sender, EventArgs e)
    //    {
    //        Service service = new Service();
    //        Users newUser = new Users();

    //        newUser.UserN = txtUName.Text;
    //        newUser.Pass = txtPass.Text;
    //        newUser.NameF = txtFName.Text;
    //        newUser.LastN = txtLName.Text;
    //        newUser.Fulladdres = txtAdd.Text;
    //        newUser.Email = txtEmail.Text;
    //        newUser.Gender = txtGender.Text;
    //        newUser.Birthday = Calendar1.SelectedDate.ToString();
    //        newUser.PhoneN = txtPhone.Text;

    //        string Pic_Name = "Profile.jpg";
    //        //if (FileUpload1.HasFile == true)
    //        //{
    //        //    Pic_Name = FileUpload1.FileName;
    //        //    FileUpload1.SaveAs(Server.MapPath("") + Pic_Name);

    //        //    //FileUpload1.SaveAs(Server.MapPath("MyPics/") + Pic_Name);
    //        //}
    //        if (FileUpload1.HasFile)
    //        {
    //            Pic_Name = FileUpload1.FileName;
    //            FileUpload1.SaveAs(Server.MapPath("~/MyPics/") + Pic_Name);
    //        }
    //        newUser.Pic = Pic_Name;

    //        service.Regi(user);


    //        //string message = "alert('You are Registered');";
    //        //ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", message, true);

    //    //    string script = @"
    //    //alert('You are Registerd');
    //    //setTimeout(function() {
    //    //    window.location = 'Login.aspx';
    //    //}, 10); // 10 = 10/1000 seconds delay
    //    //        ";
    //    //    ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script, true);

    //    }
    //    //string message = "alert('You are welcome');";
    //    //ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", message, true);
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Users newUser = new Users();         

        newUser.UserN = txtUName.Text;
        newUser.Pass = txtPass.Text;
        newUser.NameF = txtFName.Text;
        newUser.LastN = txtLName.Text;
        newUser.Fulladdres = txtAdd.Text;
        newUser.Email = txtEmail.Text;
        newUser.Gender = dpdphone.SelectedValue;
        newUser.Birthday = Calendar1.SelectedDate.ToString();
        newUser.PhoneN = txtPhone.Text;

        string Pic_Name = "Profile.jpg";
        if (FileUpload1.HasFile)
        {
            Pic_Name = FileUpload1.FileName;
            FileUpload1.SaveAs(Server.MapPath("~/MyPics/") + Pic_Name);
        }
        newUser.Pic = Pic_Name;


        // try to register
        try
        {
            my_service.Regi(newUser);
            //string script = "alert('You are registered!');";
            //ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script, true);


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
            string msg = "alert('Error: " + ex.Message.Replace("'", "\\'") + "');";
            ClientScript.RegisterStartupScript(this.GetType(), "Error", msg, true);
        }

    }


}


//להוסיף דף בclient
//בשם updateuserd
//להעתיק את כל השדות שברצונך לעדכן