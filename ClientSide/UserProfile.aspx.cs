using System;
using System.Data;
using System.Web.UI;

public partial class UserProfile : System.Web.UI.Page
{
    private localhost.Service backendService = new localhost.Service();
    private string currentUser; // The person logged in
    private string profileUser; // The person being viewed

    //הקוד מאמת את חיבור המשתמש ומפנה אותו לדף ההתחברות במידה ואינו מורשה.
    protected void Page_Load(object sender, EventArgs e)
    {
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

        profileUser = Request.QueryString["username"];
        if (string.IsNullOrEmpty(profileUser))
        {
            profileUser = currentUser; // Viewing own profile
        }

        if (!IsPostBack)
        {
            LoadUserProfile(profileUser);
            LoadUserWishlist(profileUser);
            LoadUserWatched(profileUser);
            LoadFollowData();
        }
    }

    //הפעולה מחזירה את שם המשתמש הנוכחי שנשמר בזיכרון.
    public string GetLoggedInUsername()
    {
         return currentUser;
    }

    //הפעולה טוענת את מספר העוקבים והנעקבים
    //ומציגה את כפתור העריכה עבור הפרופיל
    //שלך או את כפתור המעקב  עבור פרופילים של משתמשים אחרים
    private void LoadFollowData()
    {
        try
        {
            int followers = backendService.GetFollowersCount(profileUser);
            int following = backendService.GetFollowingCount(profileUser);

            lblFollowersCount.Text = followers.ToString();
            lblFollowingCount.Text = following.ToString();

            if (currentUser == profileUser) //הפרופיל שלנו והפרופיל שמבקרים
            {
                btnFollow.Visible = false;
                btnEditProfile.Visible = true;
            }
            else
            {
                btnEditProfile.Visible = false;
                btnFollow.Visible = true;
                bool isFollowing = backendService.IsFollowing(currentUser, profileUser);
                if (isFollowing)
                {
                    btnFollow.Text = "Unfollow";
                    btnFollow.CssClass = "btn-follow btn-unfollow"; //עיצוב משתנה
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

    //הפעולה מעדכנת את סטטוס המעקב של המשתמש
    protected void btnFollow_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnFollow.Text == "Follow")
            {
                backendService.FollowUser(currentUser, profileUser);
            }
            else
            {
                backendService.UnfollowUser(currentUser, profileUser);
            }
            LoadFollowData();
        }
        catch { }
    }

    //הפעולה שולפת את פרטי המשתמש מהמסד,
    //ממלאת את תיבות הטקסט בנתונים אלו, ומציגה את חלון העריכה
    protected void btnEditProfile_Click(object sender, EventArgs e)
    {
        DataTable dt = backendService.SearchUser(currentUser, "username");
        if (dt != null && dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];//מכילה את כל המידע על המשתמש הספציפי.
            txtEditFirstName.Text = row["FName"].ToString();
            txtEditLastName.Text = row["LName"].ToString();
            txtEditEmail.Text = row["email"].ToString();
            txtEditPass.Text = row["pass"].ToString();
            
            pnlEditModal.Visible = true;
        }
    }

    //הפעולה סוגרת (מסתירה) את חלון העריכה.
    protected void btnCancelEdit_Click(object sender, EventArgs e)
    {
        pnlEditModal.Visible = false;
    }

    //הפעולה שומרת את פרטי הפרופיל המעודכנים ומעדכנת את הנתונים במסד הנתונים
    protected void btnSaveProfile_Click(object sender, EventArgs e)
    {
        try
        {      
            // Handle Pic
            string picName = "Profile.jpg"; // default
            DataTable dt = backendService.SearchUser(currentUser, "username");
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Columns.Contains("pic")) picName = dt.Rows[0]["pic"].ToString();
            }

            if (fuProfilePic.HasFile)
            {
                string fileName = System.IO.Path.GetFileName(fuProfilePic.FileName);
                string savePath = Server.MapPath("~/MyPics/") + fileName; //כדי לדעת איפה נמצא הקובץ- תיקייה
                fuProfilePic.SaveAs(savePath); //שמירה בתיקייה
                picName = fileName;//שמירה בכתובת של הקובץ
            }

            // Create User object
            localhost.Users user = new localhost.Users();
            user.UserN = currentUser;
            user.NameF = txtEditFirstName.Text;
            user.LastN = txtEditLastName.Text;
            user.Email = txtEditEmail.Text;
            user.Pass = txtEditPass.Text;
            user.Pic = picName;
            
        
            user.Fulladdres = ""; 
            user.PhoneN = "";
            
            if (dt != null && dt.Rows.Count > 0)
            {
                user.Fulladdres = dt.Rows[0]["address"].ToString();
                user.PhoneN = dt.Rows[0]["phone"].ToString();
            }

            // Update
            DataTable dtNew = backendService.UpdateUser(user);
            
            // Update Session
            Session["data"] = dtNew;

            // Refresh Page
            Response.Redirect("UserProfile.aspx");
        }
        catch (Exception ex)
        {
            string msg = "alert('Update failed: " + ex.Message.Replace("'", "\\'") + "');";
            ClientScript.RegisterStartupScript(this.GetType(), "UpdateError", msg, true);
        }
    }

    //הפעולה שולפת את פרטי המשתמש ממסד הנתונים ומציגה אותם ברכיבי העמוד
    private void LoadUserProfile(string username)
    {
        DataTable dt = backendService.SearchUser(username, "username");
        if (dt != null && dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];  //מכילה את כל המידע על המשתמש הספציפי.
            lblUsername.Text = row["User"].ToString();
            lblName.Text = row["FName"] + " " + row["LName"];
            lblEmail.Text = row["email"].ToString();

            string pic = "Profile.jpg";
            if (dt.Columns.Contains("pic")) pic = row["pic"].ToString();
            else if (row.ItemArray.Length > 9) // בדיקת גיבוי
                                               //מסיקה שהתמונה נמצאת במיקום קבוע מראש.
                pic = row[9].ToString();

            if (string.IsNullOrEmpty(pic)) pic = "Profile.jpg";
            imgProfile.ImageUrl = "~/MyPics/" + pic; //שמירה בכתובת של הקובץ
        }
    }

    //הפעולה שולפת את רשימת המשאלות של המשתמש,
    //מגבילה את התצוגה לעד 5 הסרטים הראשונים (במידה ויש יותר), ומציגה אותם בעמוד.
    private void LoadUserWishlist(string username)
    {
        try
        {
            DataTable dtWishlist = backendService.GetWishlistMovies(username);

            if (dtWishlist != null && dtWishlist.Rows.Count > 0)
            {
                if (dtWishlist.Rows.Count > 5)
                {
                    DataTable dtTop5 = dtWishlist.Clone();//מעתיקה את המבנה של הטבלה לטבלה חדשה ריקה?
                    for (int i = 0; i < 5; i++)
                    {
                        dtTop5.ImportRow(dtWishlist.Rows[i]);//העתקת שורה בודדת מטבלה אחת לטבלה אחרת.
                    }
                    rptWishlist.DataSource = dtTop5;
                }
                else
                {
                    rptWishlist.DataSource = dtWishlist;
                }
                
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
        }
    }

    //הפעולה שולפת את רשימת הסרטים הנצפים של המשתמש, קושרת את הנתונים לרכיב התצוגה ומציגה אותם.
    private void LoadUserWatched(string username)
    {
        try
        {
            DataTable dtWatched = backendService.GetWatchedMovies(username);

            if (dtWatched != null && dtWatched.Rows.Count > 0)
            {
                rptWatched.DataSource = dtWatched;
                rptWatched.DataBind();
                lblEmptyWatched.Visible = false;
            }
            else
            {
                lblEmptyWatched.Visible = true;
            }
        }
        catch
        {
            lblEmptyWatched.Text = "Error loading watched movies.";
            lblEmptyWatched.Visible = true;
        }
    }

    //HTML הפעולה ממירה את הדירוג מסולם של 10 לסולם של 5, ומייצרת קוד
    //המכיל כוכבים מלאים, חצי כוכבים וכוכבים ריקים לייצוג חזותי של הציון.
    public string GetStarRatingHtml(object ratingObj)
    {
        double rating = 0;
        if (ratingObj != null && ratingObj != DBNull.Value)
        {
            double.TryParse(ratingObj.ToString(), out rating);
        }

        // Convert 10-scale to 5-scale
        double stars = rating / 2.0;
        int fullStars = (int)stars;
        bool halfStar = (stars - fullStars) >= 0.5;
        int emptyStars = 5 - fullStars - (halfStar ? 1 : 0);

        string html = "";
        for (int i = 0; i < fullStars; i++) html += "<i class='fa fa-star rating-star'></i>";
        if (halfStar) html += "<i class='fa fa-star-half-o rating-star'></i>";
        for (int i = 0; i < emptyStars; i++) html += "<i class='fa fa-star-o rating-star'></i>";

        return html;
    }
}
