using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CelebDetails : System.Web.UI.Page
{
    private localhost.Service backendService = new localhost.Service();

    //הפעולה בודקת אם המשתמש מחובר למערכת
    //ואם לא היא מקפיצה הודעה ומעבירה אותו לדף ההתחברות
    //אם הוא מחובר, היא טוענת את נתוני הסלב בטעינה הראשונה של הדף
    protected void Page_Load(object sender, EventArgs e)
    {
        string status = Session["status"] as string;
        if (status != "1" && status != "2")
        {
            string script = @"alert('You must be logged in to view this page.');
            setTimeout(function() {window.location = 'login.aspx';}, 10);";
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script, true);
            return; 
        }
        if (!IsPostBack)
        {
            LoadCeleb();
        }
    }

    //הפעולה שולפת את שם הסלב מהכתובת מבקשת מהשירות את הנתונים שלו, ומציגה אותם בדף
    private void LoadCeleb()
    {
        string name = Request.QueryString["name"];
        if (string.IsNullOrWhiteSpace(name))
        {
            phCeleb.Controls.Clear();
            phCeleb.Controls.Add(new LiteralControl("<div class='celeb-details-error'>" +
                "No celebrity specified.</div>"));
            return;
        }
        try
        {
            DataTable dt = backendService.SearchCelebs(name, "all");
            phCeleb.Controls.Clear();

            if (dt == null || dt.Rows.Count == 0)
            {
                phCeleb.Controls.Add(new LiteralControl("<div class='celeb-details-error'>" +
                    "Celebrity not found.</div>"));
                return;
            }

            DataRow row = dt.Rows[0];

            string celebName = row["Name"] != DBNull.Value ? row["Name"].ToString() : "Unknown";
            string role = row["Role"] != DBNull.Value ? row["Role"].ToString() : "";
            string photo = row["Photo"] != DBNull.Value ? row["Photo"].ToString() : "images/uploads/ava1.jpg";
            string bio = row["Bio"] != DBNull.Value ? row["Bio"].ToString() : "";

            if (!photo.StartsWith("http") && !photo.StartsWith("/") && !photo.StartsWith("~/"))
            {
                photo = "~/" + photo;
            }

            DataTable moviesDt = backendService.SearchMovies(celebName, "");
            string moviesHtml = "";
            if (moviesDt != null && moviesDt.Rows.Count > 0)
            {
                moviesHtml += "<div class='celeb-details-movies-title'>Movies</div><div" +
                    " class='celeb-details-movies-list'><ul>";
                foreach (DataRow m in moviesDt.Rows)
                {
                    int movieId = m["MovieId"] != DBNull.Value ? Convert.ToInt32(m["MovieId"]) : 0;
                    string title = m["Title"] != DBNull.Value ? m["Title"].ToString() : "Unknown";
                    string year = m["Year"] != DBNull.Value ? m["Year"].ToString() : "";
                    string rating = m["Rating"] != DBNull.Value ? m["Rating"].ToString() : "";

                    moviesHtml += "<li><a href='MovieDetails.aspx?movieId=" + movieId + "'>" + 
                        HttpUtility.HtmlEncode(title) + "</a>";
                    if (!string.IsNullOrEmpty(year) || !string.IsNullOrEmpty(rating))
                    {
                        moviesHtml += " (";
                        if (!string.IsNullOrEmpty(year)) moviesHtml += HttpUtility.HtmlEncode(year);
                        if (!string.IsNullOrEmpty(year) && !string.IsNullOrEmpty(rating)) moviesHtml += ", ";
                        if (!string.IsNullOrEmpty(rating)) moviesHtml += HttpUtility.HtmlEncode(rating);
                        moviesHtml += ")";
                    }
                    moviesHtml += "</li>";
                }
                moviesHtml += "</ul></div>";
            }

            string html = string.Format(@"<div class='celeb-details-layout'>
                    <div class='celeb-details-photo'>
                        <img src='{0}' alt='{1}' />
                    </div>
                    <div class='celeb-details-main'>
                        <div class='celeb-details-name'>{1}</div>
                        <div class='celeb-details-role'>{2}</div>
                        <div class='celeb-details-bio'>{3}</div>
                        {4}
                        <div class='celeb-details-actions'>
                            <a href='Celebs.aspx' class='btn-back-celebs'>Back to celebs</a>
                        </div>
                    </div>
                </div>",
                ResolveUrl(photo),//ממירה נתיב לנתיב הנכון
                HttpUtility.HtmlEncode(celebName),
                HttpUtility.HtmlEncode(role),
                HttpUtility.HtmlEncode(bio),
                moviesHtml
            );

            phCeleb.Controls.Add(new LiteralControl(html));
        }
        catch (Exception ex)
        {
            string msg = "alert('Error loading celebrity: " + ex.Message.Replace("'", "\\'") + "');";
            ClientScript.RegisterStartupScript(this.GetType(), "CelebDetailsError", msg, true);
            phCeleb.Controls.Clear();
            phCeleb.Controls.Add(new LiteralControl("<div class='celeb-details-error'>" +
                "Error loading celebrity details.</div>"));
        }
    }
}
