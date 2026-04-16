using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MovieDetails : System.Web.UI.Page
{
    private localhost.Service backendService = new localhost.Service();

    //הפעולה מוודאת שהמשתמש מחובר, וטוענת את פרטי הסרט,
    //התגובות ורשימות המשתמשים שצפו בו או הוסיפו אותו למשאלוֹת.
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
            HandleActions();
            LoadMovie();
            LoadComments();
            
            // Load Watched By Use
            int movieId = GetCurrentMovieId();
            if (movieId > 0)
            {
               DataTable dtWatched = backendService.GetUsersWhoWatchedMovie(movieId);
               if (dtWatched != null && dtWatched.Rows.Count > 0)
               {
                   rptWatchedUsers.DataSource = dtWatched;
                   rptWatchedUsers.DataBind();
               }
               else
               {
                   lblNoWatched.Visible = true;
               }

               // Load Wishlisted By Users
               DataTable dtWishlist = backendService.GetUsersWhoWishlistedMovie(movieId);
               if (dtWishlist != null && dtWishlist.Rows.Count > 0)
               {
                   rptWishlistUsers.DataSource = dtWishlist;
                   rptWishlistUsers.DataBind();
               }
               else
               {
                   lblNoWishlist.Visible = true;
               }
            }
            
            SetupCommentFormVisibility();
        }
        else
        {
            // Also handle actions on postback if needed, or primarily GET actions
             HandleActions();
        }
    }

    //הפעולה בודקת איזה "אקשן" המשתמש ביקש לעשות- כמו הוספה או הסרה מרשימת הסרטים שראה,
    //מבצעת את השינוי במסד הנתונים ומעבירה אותו חזרה לדף המתאים.
    private void HandleActions()
    {
        string username = GetLoggedInUsername();
        string action = Request.QueryString["action"];
        string movieIdStr = Request.QueryString["movieId"];
        string ret = Request.QueryString["ret"]; //כדי לדעת לאיזה דף להחזיר את המשתמש אחרי שהוא מבצע פעולה
        int movieId;

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(action) &&
            int.TryParse(movieIdStr, out movieId) && movieId > 0)
        {
            string redirectUrl = ret == "home" ? "Home.aspx" : "MovieDetails.aspx?movieId=" + movieId;

             if (action == "addToWatched")
             {
                 backendService.AddToWatched(username, movieId);
                 Response.Redirect(redirectUrl);
             }
             else if (action == "removeFromWatched")
             {
                 backendService.RemoveFromWatched(username, movieId);
                 Response.Redirect(redirectUrl);
             }
        }
    }

    //הפעולה בודקת אם המשתמש מחובר- סטטוס 1
    //Sessionואז שולפת ומחזירה את שם המשתמש שלו מתוך הנתונים שנשמרו ב
    private string GetLoggedInUsername()
    {
        try
        {
            string status = Session["status"] as string;
            if (status != "1")
            {
                return null;
            }

            DataTable dt = Session["data"] as DataTable;
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Columns.Contains("User"))
                    return dt.Rows[0]["User"].ToString();
                return dt.Rows[0][0].ToString();
            }
        }
        catch
        {
        }

        return null;
    }

    //הפעולה בודקת אם סרט מסוים נמצא ברשימת המשאלות של המשתמש
    //על ידי מעבר על כל הרשימה שלו והשוואת המספר מזהה
    private bool IsMovieInWishlist(int movieId)
    {
        try
        {
            string username = GetLoggedInUsername();
            if (string.IsNullOrEmpty(username) || movieId <= 0)
            {
                return false;
            }

            DataTable dt = backendService.GetWishlistMovies(username);
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["MovieId"] != DBNull.Value && Convert.ToInt32(row["MovieId"]) == movieId)
                    {
                        return true;
                    }
                }
            }
        }
        catch
        {
        }
        return false;
    }

    //Backend הפעולה בודקת מול השירות
    //האם המשתמש המחובר כבר צפה בסרט ספציפי ומחזירה אמת או שקר
    private bool IsMovieWatched(int movieId)
    {
         try
        {
            string username = GetLoggedInUsername();
            if (string.IsNullOrEmpty(username) || movieId <= 0) return false;
            return backendService.IsWatched(username, movieId);
        }
        catch { return false; }
    }

    //הפעולה שולפת את המספר מזהה לסרט הנוכחי מתוך כתובת הדף ומחזירה אותו כמספר
    private int GetCurrentMovieId()
    {
        int movieId;
        if (!int.TryParse(Request.QueryString["movieId"], out movieId) || movieId <= 0)
        {
            return 0;
        }
        return movieId;
    }

    //HTMLהפעולה הופכת דירוג מספרי מתוך 10 לתצוגה חזותית של כוכבים ב
    //עם חישוב של כמה כוכבים מלאים, חצויים או ריקים להציג מתוך החמישה.
    private string GenerateStarsHtml(string ratingStr)
    {
        double rating = 0;
        double.TryParse(ratingStr, out rating);

        //to 5 stars
        double stars = rating / 2.0;
        int fullStars = (int)Math.Floor(stars);
        bool halfStar = (stars - fullStars) >= 0.25 && (stars - fullStars) < 0.75;
        // If 0.75 it round up to a full star
        if ((stars - fullStars) >= 0.75) fullStars++;
        int emptyStars = 5 - fullStars - (halfStar ? 1 : 0);

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<div class='star-rating'>");
        for (int i = 0; i < fullStars; i++)
            sb.Append("<span class='star star-full'>&#9733;</span>");
        if (halfStar)
            sb.Append("<span class='star star-half'>&#9733;</span>");
        for (int i = 0; i < emptyStars; i++)
            sb.Append("<span class='star star-empty'>&#9733;</span>");
        sb.AppendFormat("<span class='star-score'>{0}<span style='opacity:0.5;" +
            "font-size:13px;'>/10</span></span>", ratingStr);
        sb.Append("</div>");
        return sb.ToString();
    }

    //HTML הפעולה שולפת את כל פרטי הסרט מהמסד (כמו כותרת, תקציר, במאי ושחקנים), ובונה מהם מבנה של דף
    //phDetails ומציגה אותו למשתמש בתוך הפקד
    private void LoadMovie()
    {
        int movieId;
        if (!int.TryParse(Request.QueryString["movieId"], out movieId) || movieId <= 0)
        {
            phDetails.Controls.Clear();
            phDetails.Controls.Add(new LiteralControl("<div class='details-error'>Invalid movie.</div>"));
            return;
        }
        try
        {
            DataTable dt = backendService.GetMovieById(movieId);
            phDetails.Controls.Clear();

            if (dt == null || dt.Rows.Count == 0)
            {
                phDetails.Controls.Add(new LiteralControl("<div class='details-error'>Movie not found.</div>"));
                return;
            }
            DataRow row = dt.Rows[0];
            string title = row["Title"] != DBNull.Value ? row["Title"].ToString() : "Unknown";
            string description = row["Description"] != DBNull.Value ? row["Description"].ToString() :
                "No description available.";
            string genre = row["Genre"] != DBNull.Value ? row["Genre"].ToString() : "";
            string director = row["Director"] != DBNull.Value ? row["Director"].ToString() : "";
            string actors = row["Actors"] != DBNull.Value ? row["Actors"].ToString() : "";
            string poster = row["Poster"] != DBNull.Value ? row["Poster"].ToString() : "images/uploads/slider1.jpg";
            string year = row["Year"] != DBNull.Value ? row["Year"].ToString() : "";
            string rating = row["Rating"] != DBNull.Value ? row["Rating"].ToString() : "0.0";

            if (!poster.StartsWith("http") && !poster.StartsWith("/") && !poster.StartsWith("~/"))
            {
                poster = "~/" + poster;
            }
            string actorsHtml = "";
            if (!string.IsNullOrWhiteSpace(actors))
            {
                string[] parts = actors.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string part in parts)
                {
                    string name = part.Trim();
                    if (name.Length > 0)
                    {
                        string urlName = HttpUtility.UrlEncode(name);
                        actorsHtml += "<a href='CelebDetails.aspx?name=" + urlName + "' " +
                            "class='actor-badge'>" + HttpUtility.HtmlEncode(name) + "</a>";
                    }
                }
            }
            if (string.IsNullOrEmpty(actorsHtml))
            {
                actorsHtml = HttpUtility.HtmlEncode(actors);
            }

            bool inWishlist = IsMovieInWishlist(movieId);
            string actionHtml;
            if (inWishlist)
            {
                actionHtml = "<span class='actor-badge' style='background:#555;border-color:" +
                    "#555;color:#ddd;'>In wishlist</span>";
            }
            else
            {
                actionHtml = "<a href='Wishlist.aspx?action=add&movieId=" + 
                    movieId + "' class='btn-wishlist'>Add to wishlist</a>";
            }
            bool isWatched = IsMovieWatched(movieId);
            string watchedHtml;
            if (isWatched)
            {
                watchedHtml = "<a href='MovieDetails.aspx?action=removeFromWatched&movieId=" 
                    + movieId + "' class='btn-watched applied'>Watched</a>";
            }
            else
            {
                watchedHtml = "<a href='MovieDetails.aspx?action=addToWatched&movieId="
                    + movieId + "' class='btn-watched'>Mark as Watched</a>";
            }

            string html = string.Format(@"<div class='details-layout'>
                    <div class='details-poster'>
                        <img src='{0}' alt='{1}' />
                    </div>
                    <div class='details-main'>
                        <div class='details-title'>{1}</div>
                        <div class='details-meta'>Year: {2}{3}</div>
                        {10}
                        <div class='details-section-title'>Synopsis</div>
                        <div class='details-description'>{5}</div>
                        <div class='details-section-title'>Director</div>
                        <div class='details-description'>{6}</div>
                        <div class='details-section-title'>Actors</div>
                        <div class='details-actors'>{7}</div>
                        <div class='details-actions'>
                            <a href='Home.aspx' class='btn-back'>Back to films</a>
                            {8}
                            {9}
                        </div>
                    </div>
                </div>",
                ResolveUrl(poster),
                HttpUtility.HtmlEncode(title),
                HttpUtility.HtmlEncode(year),
                string.IsNullOrEmpty(genre) ? "" : " | Genre: " + HttpUtility.HtmlEncode(genre),
                HttpUtility.HtmlEncode(rating),
                HttpUtility.HtmlEncode(description),
                HttpUtility.HtmlEncode(director),
                actorsHtml,
                actionHtml,
                watchedHtml,
                GenerateStarsHtml(rating)
            );

            phDetails.Controls.Add(new LiteralControl(html));
        }
        catch (Exception ex)
        {
            string msg = "alert('Error loading movie: " + ex.Message.Replace("'", "\\'") + "');";
            ClientScript.RegisterStartupScript(this.GetType(), "MovieDetailsError", msg, true);
            phDetails.Controls.Clear();
            phDetails.Controls.Add(new LiteralControl("<div class='details-error'>Error loading movie details.</div>"));
        }
    }

    //HTML הפעולה מייצרת קוד
    //שמציג 5 כוכבים עבור ביקורת, כאשר הכוכבים המלאים הם לפי הדירוג שהתקבל והשאר נשארים ריקים.
    private string GenerateReviewStarsHtml(int rating)
    {
        // Review rating is 1-5
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<span class='review-stars'>");
        for (int i = 1; i <= 5; i++)
        {
            if (i <= rating)
                sb.Append("<span class='rs rs-full'>&#9733;</span>");
            else
                sb.Append("<span class='rs rs-empty'>&#9733;</span>");
        }
        sb.Append("</span>");
        return sb.ToString();
    }

    //הפעולה שולפת את כל התגובות והדירוגים שנכתבו על הסרט,
    //הופכת אותם למבנה של רשימה מעוצבת ומציגה אותם בדף.
    private void LoadComments()
    {
        try
        {
            int movieId = GetCurrentMovieId();
            phComments.Controls.Clear();

            if (movieId <= 0)
            {
                return;
            }

            DataTable dt = backendService.GetMovieComments(movieId);
            if (dt == null || dt.Rows.Count == 0)
            {
                phComments.Controls.Add(new LiteralControl("<div class='comment-item'>" +
                    "No comments yet. Be the first to comment!</div>"));
                return;
            }

            foreach (DataRow row in dt.Rows)
            {
                string username = row["Username"] != DBNull.Value ? row["Username"].ToString() : "Unknown";
                string text = row["CommentText"] != DBNull.Value ? row["CommentText"].ToString() : "";
                int rating = 0;
                if (row["Rating"] != DBNull.Value)
                {
                    int.TryParse(row["Rating"].ToString(), out rating);
                }
                string created = row["CreatedAt"] != DBNull.Value ?
                    Convert.ToDateTime(row["CreatedAt"]).ToString("yyyy-MM-dd HH:mm") : "";

                string ratingStars = rating > 0 ? GenerateReviewStarsHtml(rating) : "";

                string html = string.Format(
                    "<div class='comment-item'><div class='comment-meta'><strong>{0}</strong>{1}" +
                    " &mdash; <span style='opacity:0.6'>{2}</span></div><div class='comment-text'>{3}</div></div>",
                    HttpUtility.HtmlEncode(username),
                    ratingStars,
                    HttpUtility.HtmlEncode(created),
                    HttpUtility.HtmlEncode(text)
                );

                phComments.Controls.Add(new LiteralControl(html));
            }
        }
        catch
        {
        }
    }

    //הפעולה בודקת אם המשתמש מחובר
    //אם כן היא מציגה לו את הטופס להוספת תגובה, ואם לא
    //היא מסתירה את הטופס ומציגה הודעה שצריך להתחבר כדי להגיב.
    private void SetupCommentFormVisibility()
    {
        string username = GetLoggedInUsername();
        if (string.IsNullOrEmpty(username))
        {
            pnlCommentForm.Visible = false;
            phComments.Controls.Add(new LiteralControl("<div class='comment-item'>" +
                "You must be logged in to add comments.</div>"));
        }
        else
        {
            pnlCommentForm.Visible = true;
        }
    }


    //הפעולה שומרת את התגובה והדירוג שהמשתמש הזין במסד הנתונים,
    //ולאחר מכן מנקה את הטופס ומעדכנת את רשימת התגובות המוצגת בדף.
    protected void btnAddComment_Click(object sender, EventArgs e)
    {
        try
        {
            string username = GetLoggedInUsername();
            if (string.IsNullOrEmpty(username))
            {
                lblCommentMessage.Text = "You must be logged in to add a comment.";
                lblCommentMessage.Visible = true;
                return;
            }

            int movieId = GetCurrentMovieId();
            if (movieId <= 0)
            {
                lblCommentMessage.Text = "Invalid movie.";
                lblCommentMessage.Visible = true;
                return;
            }

            string text = txtComment.Text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                lblCommentMessage.Text = "Please enter a comment.";
                lblCommentMessage.Visible = true;
                return;
            }

            int rating = 0;
            int.TryParse(ddlRating.SelectedValue, out rating);

            backendService.AddMovieComment(username, movieId, rating, text);

            txtComment.Text = string.Empty;
            lblCommentMessage.Text = "Comment added.";
            lblCommentMessage.Visible = true;

            LoadComments();
        }
        catch (Exception ex)
        {
            lblCommentMessage.Text = "Error adding comment: " + ex.Message;
            lblCommentMessage.Visible = true;
        }
    }
}
