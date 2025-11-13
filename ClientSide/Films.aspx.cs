using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Films : System.Web.UI.Page
{
    private localhost.Service myService = new localhost.Service();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadFilms();
        }
        else
        {
            // Reload films on postback to maintain state
            LoadFilms();
        }
    }

    protected void btnSearchFilms_Click(object sender, EventArgs e)
    {
        LoadFilms();
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        LinkButton btn = sender as LinkButton;
        string genre = btn.CommandArgument;
        
        // Update active button
        btnAll.CssClass = "genre-btn";
        btnAction.CssClass = "genre-btn";
        btnComedy.CssClass = "genre-btn";
        btnDrama.CssClass = "genre-btn";
        btnHorror.CssClass = "genre-btn";
        btnSciFi.CssClass = "genre-btn";
        
        btn.CssClass = "genre-btn active";
        
        // Store selected genre in ViewState
        ViewState["SelectedGenre"] = genre;
        
        LoadFilms();
    }

    private void LoadFilms()
    {
        try
        {
            string searchTerm = txtSearchFilms.Text.Trim();
            string genre = ViewState["SelectedGenre"] != null ? ViewState["SelectedGenre"].ToString() : "all";

            // Map genre filter values
            if (genre == "all")
                genre = "";
            else if (genre == "scifi")
                genre = "Sci-Fi";
            else if (genre == "action")
                genre = "Action";
            else if (genre == "comedy")
                genre = "Comedy";
            else if (genre == "drama")
                genre = "Drama";
            else if (genre == "horror")
                genre = "Horror";

            DataTable dt = myService.SearchMovies(searchTerm, genre);
            
            // Clear existing content
            filmsGrid.Controls.Clear();
            
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    LiteralControl movieCard = new LiteralControl(GenerateMovieCard(row));
                    filmsGrid.Controls.Add(movieCard);
                }
            }
            else
            {
                LiteralControl noResults = new LiteralControl("<div style='text-align: center; color: white; padding: 40px; grid-column: 1 / -1;'>No movies found. Try a different search or filter.</div>");
                filmsGrid.Controls.Add(noResults);
            }
        }
        catch (Exception ex)
        {
            string message = "alert('Error loading films: " + ex.Message.Replace("'", "\\'") + "');";
            ClientScript.RegisterStartupScript(this.GetType(), "Error", message, true);
            LiteralControl errorMsg = new LiteralControl("<div style='text-align: center; color: white; padding: 40px; grid-column: 1 / -1;'>Error loading films. Please try again.</div>");
            filmsGrid.Controls.Clear();
            filmsGrid.Controls.Add(errorMsg);
        }
    }

    private string GenerateMovieCard(DataRow row)
    {
        string title = row["Title"] != DBNull.Value ? row["Title"].ToString() : "Unknown";
        string poster = row["Poster"] != DBNull.Value ? row["Poster"].ToString() : "images/uploads/slider1.jpg";
        string rating = row["Rating"] != DBNull.Value ? row["Rating"].ToString() : "0.0";
        string year = row["Year"] != DBNull.Value ? row["Year"].ToString() : "";
        
        // Build poster path - check if it starts with ~/ or is relative
        if (!poster.StartsWith("http") && !poster.StartsWith("/") && !poster.StartsWith("~/"))
        {
            poster = "~/" + poster;
        }
        
        string cardHtml = string.Format(@"
            <div class='film-card'>
                <img src='{0}' alt='{1}' class='film-poster' />
                <div class='film-title'>{1}</div>
                <div class='film-rating'>{2}</div>
                <div class='film-year'>{3}</div>
            </div>",
            ResolveUrl(poster),
            HttpUtility.HtmlEncode(title),
            rating,
            year
        );
        
        return cardHtml;
    }
}

