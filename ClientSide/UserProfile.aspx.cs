using System;
using System.Data;
using System.Web.UI;

public partial class UserProfile : System.Web.UI.Page
{
    private localhost.Service myService = new localhost.Service();
    private string currentUser; // The person logged in
    private string profileUser; // The person being viewed

    protected void Page_Load(object sender, EventArgs e)
    {
        // 1. Get Logged In User
        if (Session["status"] == null || (Session["status"].ToString() != "1" && Session["status"].ToString() != "2"))
        {
            Response.Redirect("Login.aspx");
            return;
        }

        DataTable dtSession = Session["data"] as DataTable;
        if (dtSession != null && dtSession.Rows.Count > 0)
        {
            if (dtSession.Columns.Contains("User"))
                currentUser = dtSession.Rows[0]["User"].ToString();
            else
                currentUser = dtSession.Rows[0][0].ToString();
        }

        // 2. Determine Profile User
        profileUser = Request.QueryString["username"];
        if (string.IsNullOrEmpty(profileUser))
        {
            profileUser = currentUser; // Viewing own profile
        }

        if (!IsPostBack)
        {
            LoadUserProfile(profileUser);
            LoadUserWishlist(profileUser);
            LoadFollowData();
        }
    }

    private void LoadFollowData()
    {
        try
        {
            int followers = myService.GetFollowersCount(profileUser);
            int following = myService.GetFollowingCount(profileUser);

            lblFollowersCount.Text = followers.ToString();
            lblFollowingCount.Text = following.ToString();

            if (currentUser == profileUser)
            {
                btnFollow.Visible = false;
            }
            else
            {
                btnFollow.Visible = true;
                bool isFollowing = myService.IsFollowing(currentUser, profileUser);
                if (isFollowing)
                {
                    btnFollow.Text = "Unfollow";
                    btnFollow.CssClass = "btn-follow btn-unfollow";
                }
                else
                {
                    btnFollow.Text = "Follow";
                    btnFollow.CssClass = "btn-follow btn-follow-action";
                }
            }
        }
        catch { }
    }

    protected void btnFollow_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnFollow.Text == "Follow")
            {
                myService.FollowUser(currentUser, profileUser);
            }
            else
            {
                myService.UnfollowUser(currentUser, profileUser);
            }
            LoadFollowData();
        }
        catch { }
    }

    private void LoadUserProfile(string username)
    {
        DataTable dt = myService.SearchUser(username, "username");
        if (dt != null && dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            lblUsername.Text = row["User"].ToString();
            lblName.Text = row["FName"] + " " + row["LName"];
            lblEmail.Text = row["email"].ToString();

            string pic = "Profile.jpg";
            if (dt.Columns.Contains("pic")) pic = row["pic"].ToString();
            else if (row.ItemArray.Length > 9) pic = row[9].ToString();

            if (string.IsNullOrEmpty(pic)) pic = "Profile.jpg";
            imgProfile.ImageUrl = "~/MyPics/" + pic;
        }
    }

    private void LoadUserWishlist(string username)
    {
        try
        {
            DataTable dtWishlist = myService.GetWishlistMovies(username);

            if (dtWishlist != null && dtWishlist.Rows.Count > 0)
            {
                rptWishlist.DataSource = dtWishlist;
                rptWishlist.DataBind();
                lblEmptyWishlist.Visible = false;
            }
            else
            {
                lblEmptyWishlist.Visible = true;
            }
        }
        catch
        {
            lblEmptyWishlist.Text = "Error loading wishlist.";
            lblEmptyWishlist.Visible = true;
        }
    }
}