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
    private HashSet<int> wishlistMovieIds = new HashSet<int>();

    protected void Page_Load(object sender, EventArgs e)
    {
        LoadWishlistForCurrentUser();
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

    private void LoadWishlistForCurrentUser()
    {
        try
        {
            string status = Session["status"] as string;
            if (status != "1")
            {
                return;
            }

            DataTable dtUser = Session["data"] as DataTable;
            if (dtUser == null || dtUser.Rows.Count == 0)
            {
                return;
            }

            string username;
            if (dtUser.Columns.Contains("User"))
                username = dtUser.Rows[0]["User"].ToString();
            else
                username = dtUser.Rows[0][0].ToString();

            DataTable dtWishlist = myService.GetWishlistMovies(username);
            wishlistMovieIds.Clear();
            if (dtWishlist != null)
            {
                foreach (DataRow row in dtWishlist.Rows)
                {
                    if (row["MovieId"] != DBNull.Value)
                    {
                        int id;
                        if (int.TryParse(row["MovieId"].ToString(), out id))
                        {
                            wishlistMovieIds.Add(id);
                        }
                    }
                }
            }
        }
        catch
        {
            // ignore wishlist load errors on films page
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

    protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;
        if (ddl != null)
        {
            ViewState["SelectedSort"] = ddl.SelectedValue;
        }
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
            // Apply sorting based on selected option
            string sortOption = ViewState["SelectedSort"] as string ?? "default";
            if (dt != null && dt.Rows.Count > 0)
            {
                DataView dv = dt.DefaultView;
                switch (sortOption)
                {
                    case "year_desc":
                        dv.Sort = "Year DESC, Rating DESC";
                        break;
                    case "year_asc":
                        dv.Sort = "Year ASC, Rating DESC";
                        break;
                    case "rating_asc":
                        dv.Sort = "Rating ASC, Year DESC";
                        break;
                    case "rating_desc":
                        dv.Sort = "Rating DESC, Year DESC";
                        break;
                    case "default":
                    default:
                        dv.Sort = "Rating DESC, Year DESC";
                        break;
                }
                dt = dv.ToTable();
            }
            
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
        int movieId = row["MovieId"] != DBNull.Value ? Convert.ToInt32(row["MovieId"]) : 0;
        
        // Build poster path - check if it starts with ~/ or is relative
        if (!poster.StartsWith("http") && !poster.StartsWith("/") && !poster.StartsWith("~/"))
        {
            poster = "~/" + poster;
        }
        
        string actionHtml;
        if (wishlistMovieIds.Contains(movieId))
        {
            actionHtml = "<span style='display:inline-block;margin-top:8px;padding:6px 14px;border-radius:20px;background:#555;color:#ddd;font-size:13px;'>In wishlist</span>";
        }
        else
        {
            actionHtml = string.Format("<a href='Wishlist.aspx?action=add&movieId={0}' style='display:inline-block;margin-top:8px;padding:6px 14px;border-radius:20px;background:#ff6b6b;color:white;text-decoration:none;font-size:13px;'>Add to wishlist</a>", movieId);
        }

        string cardHtml = string.Format(@"
            <div class='film-card'>
                <img src='{0}' alt='{1}' class='film-poster' />
                <div class='film-title'>{1}</div>
                <div class='film-rating'>{2}</div>
                <div class='film-year'>{3}</div>
                {4}
            </div>",
            ResolveUrl(poster),
            HttpUtility.HtmlEncode(title),
            rating,
            year,
            actionHtml
        );
        
        return cardHtml;
    }
}

