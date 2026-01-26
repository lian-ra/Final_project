using System;
using System.Data;
using System.Web.UI;

public partial class MyNetwork : System.Web.UI.Page
{
    private localhost.Service myService = new localhost.Service();

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

    private void LoadNetwork()
    {
        try
        {
            string currentUser = "";
            DataTable dtSession = Session["data"] as DataTable;
            if (dtSession != null && dtSession.Rows.Count > 0)
            {
                if (dtSession.Columns.Contains("User"))
                    currentUser = dtSession.Rows[0]["User"].ToString();
                else
                    currentUser = dtSession.Rows[0][0].ToString();
            }

            // Load Following
            DataTable dtFollowing = myService.GetFollowingList(currentUser);
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
            DataTable dtFollowers = myService.GetFollowersList(currentUser);
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