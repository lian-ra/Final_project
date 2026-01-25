using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class UserProfile : System.Web.UI.Page
{
    private localhost.Service myService = new localhost.Service();

    protected void Page_Load(object sender, EventArgs e)
    {
        // Require login to view any profile (optional security choice)
        if (Session["status"] == null || (Session["status"].ToString() != "1" && Session["status"].ToString() != "2"))
        {
            Response.Redirect("Login.aspx");
            return;
        }

        if (!IsPostBack)
        {
            string targetUsername = Request.QueryString["username"];

            // If no username in URL, show logged-in user's profile
            if (string.IsNullOrEmpty(targetUsername))
            {
                DataTable dtSession = Session["data"] as DataTable;
                if (dtSession != null && dtSession.Rows.Count > 0)
                {
                    // Safe column check
                    if (dtSession.Columns.Contains("User"))
                        targetUsername = dtSession.Rows[0]["User"].ToString();
                    else if (dtSession.Columns.Contains("Usern"))
                        targetUsername = dtSession.Rows[0]["Usern"].ToString();
                    else
                        targetUsername = dtSession.Rows[0][0].ToString();
                }
            }

            if (!string.IsNullOrEmpty(targetUsername))
            {
                LoadUserProfile(targetUsername);
                LoadUserWishlist(targetUsername);
            }
            else
            {
                Response.Redirect("Home.aspx");
            }
        }
    }

    private void LoadUserProfile(string username)
    {
        try
        {
            // Re-using SearchUser to fetch details. "username" is the search type.
            DataTable dt = myService.SearchUser(username, "username");

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                lblUsername.Text = row["User"].ToString();
                lblName.Text = row["FName"] + " " + row["LName"];
                lblEmail.Text = row["email"].ToString();

                string address = row["address"].ToString();
                lblAddress.Text = string.IsNullOrEmpty(address) ? "No address provided" : address;

                // Handle Phone (optional display)
                if (dt.Columns.Contains("phone"))
                {
                    string phone = row["phone"].ToString();
                    if (!string.IsNullOrEmpty(phone))
                    {
                        lblPhone.Text = phone;
                        divPhone.Visible = true;
                    }
                }

                // Handle Profile Pic
                string pic = "Profile.jpg"; // Default
                if (dt.Columns.Contains("pic"))
                    pic = row["pic"].ToString();
                else if (row.ItemArray.Length > 9)
                    pic = row[9].ToString();

                if (string.IsNullOrEmpty(pic)) pic = "Profile.jpg";

                imgProfile.ImageUrl = "~/MyPics/" + pic;
            }
            else
            {
                // User not found
                Response.Redirect("Home.aspx");
            }
        }
        catch
        {
            // Handle error silently or redirect
        }
    }

    private void LoadUserWishlist(string username)
    {
        try
        {
            // Use existing service method to get wishlist
            DataTable dtWishlist = myService.GetWishlistMovies(username);

            // Filter out rows that might be null/empty if the join failed
            // Note: Depending on your Service implementation, GetWishlistMovies usually joins Users+Wishlist+Movies
            // If it returns raw Movie data, we can bind it directly.

            if (dtWishlist != null && dtWishlist.Rows.Count > 0)
            {
                // Ensure the Poster path is correct
                // Sometimes DB stores just "slider1.jpg", sometimes "images/uploads/..."
                // The ASPX ResolveUrl handles the "~/" part, but we might need to prepend folder if missing.
                // We'll assume the DB has relative paths like "images/uploads/x.jpg" or we handle it in ASPX.

                rptWishlist.DataSource = dtWishlist;
                rptWishlist.DataBind();
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