using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Home : System.Web.UI.Page
{
    private localhost.Service backendService = new localhost.Service();
    private HashSet<int> wishlistMovieIds = new HashSet<int>(); //Watchlist הסרטים שב
    private HashSet<int> watchedMovieIds = new HashSet<int>(); //Watched הסרטים שב

    //הפעולה מבצעת טעינת נתונים אישיים של המשתמש בעת פתיחת הדף.
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadWishlistForCurrentUser();
        LoadWatchedForCurrentUser();
        if (!IsPostBack)//כדי להגיד למחשב:תטען את הנתונים מהדאטה-בייס רק כשהדף נפתח בפעם הראשונה
                        //  אם המשתמש לוחץ על כפתור והדף מתרענן, אני
                        //לא רוצה שהמחשב יטען את הכל מחדש, כי זה ימחוק
                        //את מה שהמשתמש כתב או שינה בתיבות הטקסט.
                        //כאן המשתמש רק עכשיו נכנס לדף, הכל נקי וחדש

        {
            LoadFilms();
        }
    }

    //הפעולה מבצעת טעינה של רשימת המשאלות עבור המשתמש המחובר
    private void LoadWishlistForCurrentUser()
    {
        try
        {
            if (Session["status"] == null || Session["status"].ToString() != "1") return;

            DataTable dtUser = Session["data"] as DataTable;
            if (dtUser == null || dtUser.Rows.Count == 0) return;

            string username;
            if (dtUser.Columns.Contains("User"))
                username = dtUser.Rows[0]["User"].ToString();
            else
                username = dtUser.Rows[0][0].ToString();

            DataTable dtWishlist = backendService.GetWishlistMovies(username);
            wishlistMovieIds.Clear();
            if (dtWishlist != null)
            {
                foreach (DataRow row in dtWishlist.Rows)
                {
                    if (row["MovieId"] != DBNull.Value)
                    {
                        int id;
                        if (int.TryParse(row["MovieId"].ToString(), out id))
                            wishlistMovieIds.Add(id);
                    }
                }
            }
        }
        catch {}
    }

    //הפעולה מבצעת טעינה של רשימת הסרטים שהמשתמש כבר ראה.
    private void LoadWatchedForCurrentUser()
    {
        try
        {
            if (Session["status"] == null) return;
            string status = Session["status"].ToString();
            if (status != "1" && status != "2") return;

            DataTable dtUser = Session["data"] as DataTable;
            if (dtUser == null || dtUser.Rows.Count == 0) return;

            string username;
            if (dtUser.Columns.Contains("User"))
                username = dtUser.Rows[0]["User"].ToString();
            else
                username = dtUser.Rows[0][0].ToString();

            DataTable dtWatched = backendService.GetWatchedMovies(username);
            watchedMovieIds.Clear();
            if (dtWatched != null)
            {
                foreach (DataRow row in dtWatched.Rows)
                {
                    if (row["MovieId"] != DBNull.Value)
                    {
                        int id;
                        if (int.TryParse(row["MovieId"].ToString(), out id))
                            watchedMovieIds.Add(id);
                    }
                }
            }
        }
        catch {}
    }

    //הפעולה מראה למשתמש את תוצאות הסרטים שהוא חיפש בחיפוש
    protected void btnSearchFilms_Click(object sender, EventArgs e)
    {
        LoadFilms();
    }

    //הפעולה מבצעת סינון של סרטים לפי ז'אנר.
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        LinkButton btn = sender as LinkButton;
        string genre = btn.CommandArgument;

        btnAll.CssClass = "genre-btn";
        btnAction.CssClass = "genre-btn";
        btnComedy.CssClass = "genre-btn";
        btnDrama.CssClass = "genre-btn";
        btnHorror.CssClass = "genre-btn";
        btnSciFi.CssClass = "genre-btn";

        btn.CssClass = "genre-btn active";

        ViewState["SelectedGenre"] = genre;
        LoadFilms();
    }

    //הפעולה מבצעת שינוי של סדר הצגת הסרטים- מיון.
    protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;
        if (ddl != null) ViewState["SelectedSort"] = ddl.SelectedValue;
        LoadFilms();
    }

    //הפעולה מבצעת את הצגת הסרטים על המסך לפי כל הסינונים והמיונים שהמשתמש בחר.
    private void LoadFilms()
    {
        try
        {
            string searchTerm = txtSearchFilms.Text.Trim();
            string genre = ViewState["SelectedGenre"] != null ?
                ViewState["SelectedGenre"].ToString() : "all";

            if (genre == "all") genre = "";
            else if (genre == "scifi") genre = "Sci-Fi";
            else if (genre == "action") genre = "Action";
            else if (genre == "comedy") genre = "Comedy";
            else if (genre == "drama") genre = "Drama";
            else if (genre == "horror") genre = "Horror";

            DataTable dt = backendService.SearchMovies(searchTerm, genre);
            string sortOption = ViewState["SelectedSort"] as string ?? "default";

            if (dt != null && dt.Rows.Count > 0)
            {
                DataView dv = dt.DefaultView;
                switch (sortOption)
                {
                    case "year_desc": dv.Sort = "Year DESC, Rating DESC"; break;
                    case "year_asc": dv.Sort = "Year ASC, Rating DESC"; break;
                    case "rating_asc": dv.Sort = "Rating ASC, Year DESC"; break;
                    case "rating_desc": dv.Sort = "Rating DESC, Year DESC"; break;
                    default: dv.Sort = "Rating DESC, Year DESC"; break;
                }
                dt = dv.ToTable();
            }
            filmsGrid.Controls.Clear();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    filmsGrid.Controls.Add(new LiteralControl(GenerateMovieCard(row)));
                }
            }
            else
            {
                filmsGrid.Controls.Add(new LiteralControl("<div style='text-align: center; color: white; padding: 40px; grid-column: 1 / -1;'>No movies found.</div>"));
            }
        }
        catch (Exception ex)
        {
            filmsGrid.Controls.Add(new LiteralControl("<div style='color:white'>Error loading films: " + ex.Message + "</div>"));
        }
    }


    //הפעולה מבצעת את הבנייה הוויזואלית של כרטיס הסרט
    //HTML היא הופכת את הנתונים מהטבלה לקוד
    //שמוצג למשתמש
    private string GenerateMovieCard(DataRow row)
    {
        string title = row["Title"] != DBNull.Value ? row["Title"].ToString() : "Unknown";
        string poster = row["Poster"] != DBNull.Value ? row["Poster"].ToString() : "images/uploads/slider1.jpg";
        string rating = row["Rating"] != DBNull.Value ? row["Rating"].ToString() : "0.0";
        string year = row["Year"] != DBNull.Value ? row["Year"].ToString() : "";
        int movieId = row["MovieId"] != DBNull.Value ? Convert.ToInt32(row["MovieId"]) : 0;

        if (!poster.StartsWith("http") && !poster.StartsWith("/") && !poster.StartsWith("~/"))
        {
            poster = "~/" + poster;
        }

        // Wishlist button
        string wishlistHtml;
        if (wishlistMovieIds.Contains(movieId))
        {
            wishlistHtml = "<span class='card-btn card-btn-wishlisted'>&#10003; In Wishlist</span>";
        }
        else
        {
            wishlistHtml = string.Format("<a href='Wishlist.aspx?action=add&movieId={0}' " +
                "class='card-btn card-btn-wishlist'>&#9825; Wishlist</a>", movieId);
        }

        // Watched button
        string watchedHtml;
        if (watchedMovieIds.Contains(movieId))
        {
            watchedHtml = string.Format("<a href='MovieDetails.aspx?action=removeFromWatched&movieId={0}&ret=home' class='card-btn card-btn-watched-done'>&#10003; Watched</a>", movieId);
        }
        else
        {
            watchedHtml = string.Format("<a href='MovieDetails.aspx?action=addToWatched&movieId={0}&ret=home' class='card-btn card-btn-watch'>&#9654; Watched</a>", movieId);
        }

        // View details button
        string detailsHtml = string.Format("<a href='MovieDetails.aspx?movieId={0}'" +
            " class='card-btn card-btn-details'>View Details &#8594;</a>", movieId);

        return string.Format(@"
            <div class='film-card'>
                <a href='MovieDetails.aspx?movieId={5}' style='text-decoration:none; color:inherit;'>
                    <img src='{0}' alt='{1}' class='film-poster' />
                    <div class='film-title'>{1}</div>
                </a>
                <div class='film-rating'>&#11088; {2}</div>
                <div class='film-year'>{3}</div>
                <div class='card-actions'>
                    {4}
                    {6}
                    {7}
                </div>
            </div>",
            ResolveUrl(poster),
            HttpUtility.HtmlEncode(title),
            rating,
            year,
            wishlistHtml,
            movieId,
            watchedHtml,
            detailsHtml
        );
    }
}
