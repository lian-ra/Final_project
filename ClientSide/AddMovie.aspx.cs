using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;

public partial class AddMovie : System.Web.UI.Page
{
    private localhost.Service backendService = new localhost.Service();

    //הפעולה מבצעת אבטחת גישה לדף מנהל.
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Session["status"].ToString().Equals("2"))
        {
            string script = @"alert('You are not welcome!');
          setTimeout(function() {window.location = 'login.aspx';}, 10); 
           // 10 = 10/1000 seconds delay";
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script, true);
        }

                         //כדי להגיד למחשב:תטען את הנתונים מהדאטה-בייס רק כשהדף נפתח בפעם הראשונה
        if (!IsPostBack) //  אם המשתמש לוחץ על כפתור והדף מתרענן, אני
                         //לא רוצה שהמחשב יטען את הכל מחדש, כי זה ימחוק
                         //את מה שהמשתמש כתב או שינה בתיבות הטקסט.
                         //כאן המשתמש רק עכשיו נכנס לדף, הכל נקי וחדש


        {
            string status = Session["status"] as string; 
            if (status != "2")
            {
                Response.Redirect("Home.aspx");
            }
        }
    }

    //הפעולה מבצעת הוספת סרט חדש למערכת
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                localhost.Movies newMovie = new localhost.Movies();  

                newMovie.MovieId = 0;

                newMovie.Title = txtTitle.Text.Trim();
                newMovie.Description = txtDescription.Text.Trim();
                
                int year;
                if (int.TryParse(txtYear.Text.Trim(), out year))
                {
                    newMovie.Year = year;
                }
                else
                {
                    ShowMessage("Invalid year format.", false);
                    return;
                }

                newMovie.Genre = ddlGenre.SelectedValue; //מתוך רשימת הבחירה

                decimal rating = 0;
                if (!string.IsNullOrEmpty(txtRating.Text.Trim()))
                {
                    if (decimal.TryParse(txtRating.Text.Trim(), out rating))
                    {
                        newMovie.Rating = rating;
                    }
                }

                newMovie.Director = txtDirector.Text.Trim();
                newMovie.Actors = txtActors.Text.Trim();

                
                int duration = 0; //משך הזמן
                if (!string.IsNullOrEmpty(txtDuration.Text.Trim()))
                {
                    if (int.TryParse(txtDuration.Text.Trim(), out duration))
                    {
                        newMovie.Duration = duration;
                    }
                }

                string posterPath = "";
                if (filePoster.HasFile)
                {
                    try
                    {
                        string fileName = Path.GetFileName(filePoster.FileName);
                        string extension = Path.GetExtension(fileName).ToLower();
                        
                 
                        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" }; 
                        if (!allowedExtensions.Contains(extension))
                        {
                            ShowMessage("Invalid file type. Please upload an image file (jpg, jpeg, png, gif, bmp).", false);
                            return;
                        }

                        string uploadDir = Server.MapPath("~/images/uploads/"); //הגדרת מיקום לשמירה
                        if (!Directory.Exists(uploadDir)) //תיקייה
                        {
                            Directory.CreateDirectory(uploadDir);
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + extension; //למניעת כפילויות
                        string filePath = Path.Combine(uploadDir, uniqueFileName); //של משתמשים עם אותה תמונה

                        filePoster.SaveAs(filePath);

                     
                        posterPath = "images/uploads/" + uniqueFileName; //המיקום בו שמרנו
                        newMovie.Poster = posterPath;
                        
                        lblPosterPath.Text = "File uploaded: " + uniqueFileName;
                        lblPosterPath.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        ShowMessage("Error uploading file: " + ex.Message, false);
                        return;
                    }
                }
                else
                {
                    newMovie.Poster = "images/uploads/slider1.jpg";
                }

                backendService.AddMovie(newMovie);

                ShowMessage("Movie added successfully!", true);
                ClearForm();
            }
            catch (Exception ex)
            {
                ShowMessage("Error adding movie: " + ex.Message, false);
            }
        }
    }

    //הפעולה מבצעת ניקוי של הטופס כאשר המשתמש לוחץ על כפתור האתחל
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ClearForm();
    }

    //הפעולה מבצעת איפוס של כל שדות הטופס
    private void ClearForm()
    {
        txtTitle.Text = "";
        txtDescription.Text = "";
        txtYear.Text = "";
        ddlGenre.SelectedIndex = 0;
        txtRating.Text = "";
        txtDirector.Text = "";
        txtActors.Text = "";
        txtDuration.Text = "";
        filePoster.Dispose();
        lblPosterPath.Visible = false;
        lblMessage.Visible = false;
    }

    //הפעולה מבצעת הודעת משוב למשתמש- סטטוס
    private void ShowMessage(string message, bool isSuccess)
    {
        lblMessage.Text = message;
        lblMessage.Visible = true;
        if (isSuccess)
        {
            lblMessage.CssClass = "message message-success";
        }
        else
        {
            lblMessage.CssClass = "message message-error";
        }
    }
}

