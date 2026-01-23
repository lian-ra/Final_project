using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class News : System.Web.UI.Page
{
    private localhost.Service myService = new localhost.Service();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadLatestMovies();
            LoadLatestCelebs();
        }
    }

    private void LoadLatestMovies()
    {
        try
        {
            DataTable dt = myService.GetLatestMovies("");
            phLatestMovies.Controls.Clear();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    phLatestMovies.Controls.Add(new LiteralControl(GenerateMovieCard(row)));
                }
            }
            else
            {
                phLatestMovies.Controls.Add(new LiteralControl("<div class='news-empty'>No movies have been added yet.</div>"));
            }
        }
        catch (Exception ex)
        {
            string msg = "alert('Error loading latest movies: " + ex.Message.Replace("'", "\\'") + "');";
            ClientScript.RegisterStartupScript(this.GetType(), "LatestMoviesError", msg, true);
            phLatestMovies.Controls.Clear();
            phLatestMovies.Controls.Add(new LiteralControl("<div class='news-empty'>Error loading latest movies.</div>"));
        }
    }

    private void LoadLatestCelebs()
    {
        try
        {
            DataTable dt = myService.GetLatestCelebs();
            phLatestCelebs.Controls.Clear();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    phLatestCelebs.Controls.Add(new LiteralControl(GenerateCelebCard(row)));
                }
            }
            else
            {
                phLatestCelebs.Controls.Add(new LiteralControl("<div class='news-empty'>No celebs have been added yet.</div>"));
            }
        }
        catch (Exception ex)
        {
            string msg = "alert('Error loading latest celebs: " + ex.Message.Replace("'", "\\'") + "');";
            ClientScript.RegisterStartupScript(this.GetType(), "LatestCelebsError", msg, true);
            phLatestCelebs.Controls.Clear();
            phLatestCelebs.Controls.Add(new LiteralControl("<div class='news-empty'>Error loading latest celebs.</div>"));
        }
    }

    private string GenerateMovieCard(DataRow row)
    {
        string title = row["Title"] != DBNull.Value ? row["Title"].ToString() : "Unknown";
        string poster = row["Poster"] != DBNull.Value ? row["Poster"].ToString() : "images/uploads/slider1.jpg";
        string year = row["Year"] != DBNull.Value ? row["Year"].ToString() : "";
        string rating = row["Rating"] != DBNull.Value ? row["Rating"].ToString() : "0.0";

        if (!poster.StartsWith("http") && !poster.StartsWith("/") && !poster.StartsWith("~/"))
        {
            poster = "~/" + poster;
        }

        string html = string.Format(@"<div class='news-card'>
                <img src='{0}' alt='{1}' />
                <div class='news-card-body'>
                    <div class='news-card-title'>{1}</div>
                    <div class='news-card-meta'>Year: {2} | Rating: {3}</div>
                </div>
            </div>",
            ResolveUrl(poster),
            HttpUtility.HtmlEncode(title),
            HttpUtility.HtmlEncode(year),
            HttpUtility.HtmlEncode(rating));

        return html;
    }

    private string GenerateCelebCard(DataRow row)
    {
        string name = row["Name"] != DBNull.Value ? row["Name"].ToString() : "Unknown";
        string role = row["Role"] != DBNull.Value ? row["Role"].ToString() : "";
        string photo = row["Photo"] != DBNull.Value ? row["Photo"].ToString() : "images/uploads/ava1.jpg";
        string bio = row["Bio"] != DBNull.Value ? row["Bio"].ToString() : "";

        if (!photo.StartsWith("http") && !photo.StartsWith("/") && !photo.StartsWith("~/"))
        {
            photo = "~/" + photo;
        }

        string html = string.Format(@"<div class='news-card'>
                <img src='{0}' alt='{1}' />
                <div class='news-card-body'>
                    <div class='news-card-title'>{1}</div>
                    <div class='news-card-meta'>{2}</div>
                    <div class='news-card-text'>{3}</div>
                </div>
            </div>",
            ResolveUrl(photo),
            HttpUtility.HtmlEncode(name),
            HttpUtility.HtmlEncode(role),
            HttpUtility.HtmlEncode(bio));

        return html;
    }
}
