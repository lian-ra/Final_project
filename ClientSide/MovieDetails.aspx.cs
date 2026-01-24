using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MovieDetails : System.Web.UI.Page
{
    private localhost.Service myService = new localhost.Service();

    protected void Page_Load(object sender, EventArgs e)
    {
                string status = Session["status"] as string;
        if (status != "1" && status != "2")
        {
            string script = @"alert('You must be logged in to view this page.'); setTimeout(function() {window.location = 'login.aspx';}, 10);";
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script, true);
            return; // Stop further execution
        }

        if (!IsPostBack)
        {
            LoadMovie();
            LoadComments();
            SetupCommentFormVisibility();
        }
    }

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

    private bool IsMovieInWishlist(int movieId)
    {
        try
        {
            string username = GetLoggedInUsername();
            if (string.IsNullOrEmpty(username) || movieId <= 0)
            {
                return false;
            }

            DataTable dt = myService.GetWishlistMovies(username);
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

    private int GetCurrentMovieId()
    {
        int movieId;
        if (!int.TryParse(Request.QueryString["movieId"], out movieId) || movieId <= 0)
        {
            return 0;
        }

        return movieId;
    }

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
            DataTable dt = myService.GetMovieById(movieId);
            phDetails.Controls.Clear();

            if (dt == null || dt.Rows.Count == 0)
            {
                phDetails.Controls.Add(new LiteralControl("<div class='details-error'>Movie not found.</div>"));
                return;
            }

            DataRow row = dt.Rows[0];

            string title = row["Title"] != DBNull.Value ? row["Title"].ToString() : "Unknown";
            string description = row["Description"] != DBNull.Value ? row["Description"].ToString() : "No description available.";
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

            // Format actors as badges if comma-separated
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
                        actorsHtml += "<a href='CelebDetails.aspx?name=" + urlName + "' class='actor-badge'>" + HttpUtility.HtmlEncode(name) + "</a>";
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
                actionHtml = "<span class='actor-badge' style='background:#555;border-color:#555;color:#ddd;'>In wishlist</span>";
            }
            else
            {
                actionHtml = "<a href='Wishlist.aspx?action=add&movieId=" + movieId + "' class='btn-wishlist'>Add to wishlist</a>";
            }

            string html = string.Format(@"<div class='details-layout'>
                    <div class='details-poster'>
                        <img src='{0}' alt='{1}' />
                    </div>
                    <div class='details-main'>
                        <div class='details-title'>{1}</div>
                        <div class='details-meta'>Year: {2}{3}</div>
                        <div class='details-rating'>Rating: {4}/10</div>
                        <div class='details-section-title'>Synopsis</div>
                        <div class='details-description'>{5}</div>
                        <div class='details-section-title'>Director</div>
                        <div class='details-description'>{6}</div>
                        <div class='details-section-title'>Actors</div>
                        <div class='details-actors'>{7}</div>
                        <div class='details-actions'>
                            <a href='Films.aspx' class='btn-back'>Back to films</a>
                            {8}
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
                actionHtml
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

            DataTable dt = myService.GetMovieComments(movieId);
            if (dt == null || dt.Rows.Count == 0)
            {
                phComments.Controls.Add(new LiteralControl("<div class='comment-item'>No comments yet. Be the first to comment!</div>"));
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
                string created = row["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(row["CreatedAt"]).ToString("yyyy-MM-dd HH:mm") : "";

                string ratingText = rating > 0 ? " | Rating: " + rating + "/5" : string.Empty;

                string html = string.Format(
                    "<div class='comment-item'><div class='comment-meta'>{0}{1} - {2}</div><div class='comment-text'>{3}</div></div>",
                    HttpUtility.HtmlEncode(username),
                    ratingText,
                    HttpUtility.HtmlEncode(created),
                    HttpUtility.HtmlEncode(text)
                );

                phComments.Controls.Add(new LiteralControl(html));
            }
        }
        catch
        {
            // Swallow comment loading errors to avoid breaking the page
        }
    }

    private void SetupCommentFormVisibility()
    {
        string username = GetLoggedInUsername();
        if (string.IsNullOrEmpty(username))
        {
            pnlCommentForm.Visible = false;
            phComments.Controls.Add(new LiteralControl("<div class='comment-item'>You must be logged in to add comments.</div>"));
        }
        else
        {
            pnlCommentForm.Visible = true;
        }
    }

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

            myService.AddMovieComment(username, movieId, rating, text);

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
