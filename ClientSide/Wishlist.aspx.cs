using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Wishlist : System.Web.UI.Page
{
    private localhost.Service myService = new localhost.Service();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HandleActions();
            LoadWishlist();
        }
    }

    private string GetTargetUsername()
    {
        string queryUser = Request.QueryString["username"];
        if (!string.IsNullOrEmpty(queryUser))
        {
            return queryUser;
        }
        return GetLoggedInUsername();
    }

    private string GetLoggedInUsername()
    {
        try
        {
            string status = Session["status"] as string;
            if (status != "1" && status != "2")
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

    private void HandleActions()
    {
        string username = GetLoggedInUsername();
        if (string.IsNullOrEmpty(username))
        {
            // Only redirect if trying to perform an action or viewing own wishlist without login
            if (string.IsNullOrEmpty(Request.QueryString["username"])) 
            {
                 Response.Redirect("Login.aspx");
            }
            return;
        }

        string action = Request.QueryString["action"];
        string movieIdStr = Request.QueryString["movieId"];

        // Prevent modifying others' wishlists
        string targetUser = GetTargetUsername();
        if (targetUser != username && !string.IsNullOrEmpty(action))
        {
             return; 
        }

        int movieId;
        if (!string.IsNullOrEmpty(action) && int.TryParse(movieIdStr, out movieId))
        {
            try
            {
                if (action == "add")
                {
                    myService.AddToWishlist(username, movieId);
                }
                else if (action == "remove")
                {
                    myService.RemoveFromWishlist(username, movieId);
                }
            }
            catch (Exception ex)
            {
                string msg = "alert('Wishlist action failed: " + ex.Message.Replace("'", "\\'") + "');";
                ClientScript.RegisterStartupScript(this.GetType(), "WishlistActionError", msg, true);
            }

            Response.Redirect("Wishlist.aspx");
        }
    }

    private void LoadWishlist()
    {
        string username = GetTargetUsername();
        if (string.IsNullOrEmpty(username))
        {
            return;
        }

        try
        {
            DataTable dt = myService.GetWishlistMovies(username);
            phWishlist.Controls.Clear();

            if (dt != null && dt.Rows.Count > 0)
            {
                bool isOwner = (username == GetLoggedInUsername());
                foreach (DataRow row in dt.Rows)
                {
                    phWishlist.Controls.Add(new LiteralControl(GenerateWishlistCard(row, isOwner)));
                }
            }
            else
            {
                string msg = (username == GetLoggedInUsername()) ? "Your wishlist is empty. Browse films and add some!" : "This user's wishlist is empty.";
                phWishlist.Controls.Add(new LiteralControl("<div class='wishlist-empty'>" + msg + "</div>"));
            }
        }
        catch (Exception ex)
        {
            string msg = "alert('Error loading wishlist: " + ex.Message.Replace("'", "\\'") + "');";
            ClientScript.RegisterStartupScript(this.GetType(), "WishlistLoadError", msg, true);
            phWishlist.Controls.Clear();
            phWishlist.Controls.Add(new LiteralControl("<div class='wishlist-empty'>Error loading wishlist.</div>"));
        }
    }

    private string GenerateWishlistCard(DataRow row, bool isOwner)
    {
        string title = row["Title"] != DBNull.Value ? row["Title"].ToString() : "Unknown";
        string poster = row["Poster"] != DBNull.Value ? row["Poster"].ToString() : "images/uploads/slider1.jpg";
        string year = row["Year"] != DBNull.Value ? row["Year"].ToString() : "";
        string rating = row["Rating"] != DBNull.Value ? row["Rating"].ToString() : "0.0";
        int movieId = row["MovieId"] != DBNull.Value ? Convert.ToInt32(row["MovieId"]) : 0;

        if (!poster.StartsWith("http") && !poster.StartsWith("/") && !poster.StartsWith("~/"))
        {
            poster = "~/" + poster;
        }

        string removeBtn = "";
        if (isOwner)
        {
            removeBtn = "<a href='Wishlist.aspx?action=remove&movieId=" + movieId + "' class='wishlist-remove'>Remove</a>";
        }

        string html = string.Format(@"<div class='wishlist-card'>
                <img src='{0}' alt='{1}' class='wishlist-poster' />
                <div class='wishlist-title'>{1}</div>
                <div class='wishlist-meta'>Year: {2} | Rating: {3}</div>
                {5}
            </div>",
            ResolveUrl(poster),
            HttpUtility.HtmlEncode(title),
            HttpUtility.HtmlEncode(year),
            HttpUtility.HtmlEncode(rating),
            movieId,
            removeBtn);

        return html;
    }
}
