using System;
using System.Data;
using System.Web.UI;

public partial class MyNetwork : System.Web.UI.Page
{
    private localhost.Service backendService = new localhost.Service();

    //הפעולה מוודאת הרשאות מנהל בכניסה לדף, מפנה משתמשים לא מורשים
    //להתחברות וטוענת את נתוני הרשת בביקור הראשון.
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["status"] == null || Session["status"].ToString() != "1")
        {
            Response.Redirect("Login.aspx");
            return;
        }
        if (!IsPostBack)
        {
            LoadNetwork();
        }
    }

    //הפעולה מזהה את המשתמש הרלוונטי (מכתובת הדף או מהזיכרון), שולפת ומציגה
    //את רשימות הנעקבים והעוקבים שלו, ומציגה הודעה אם הרשימות ריקות
    private void LoadNetwork()
    {
        try
        {
            string currentUser = "";     
            string queryUser = Request.QueryString["username"];
            if (!string.IsNullOrEmpty(queryUser))
            {
                currentUser = queryUser;
            }
            else
            {
                DataTable dtSession = Session["data"] as DataTable;
                if (dtSession != null && dtSession.Rows.Count > 0)
                {
                    if (dtSession.Columns.Contains("User"))
                        currentUser = dtSession.Rows[0]["User"].ToString();
                    else
                        currentUser = dtSession.Rows[0][0].ToString();
                }
            }

            if (string.IsNullOrEmpty(currentUser)) return;

            // Load Following
            DataTable dtFollowing = backendService.GetFollowingList(currentUser);
            if (dtFollowing != null && dtFollowing.Rows.Count > 0)
            {
                rptFollowing.DataSource = dtFollowing;
                rptFollowing.DataBind();
            }
            else
            {
                lblNoFollowing.Visible = true;
            }

            // Load Followers
            DataTable dtFollowers = backendService.GetFollowersList(currentUser);
            if (dtFollowers != null && dtFollowers.Rows.Count > 0)
            {
                rptFollowers.DataSource = dtFollowers;
                rptFollowers.DataBind();
            }
            else
            {
                lblNoFollowers.Visible = true;
            }
        }
        catch { }
    }
}
