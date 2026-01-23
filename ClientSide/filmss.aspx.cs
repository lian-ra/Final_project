using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

public partial class filmss : System.Web.UI.Page
{
    private localhost.Service myService = new localhost.Service();
    private HashSet<int> watchListMovieIds = new HashSet<int>();

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!IsPostBack)
        {
            LoadFilms();
        }
    }

    protected void btnSearchFilms_Click(object sender, EventArgs e)
    {
        LoadFilms();
    }

    private void LoadFilms()
    {
        string searchTerm = txtSearchFilms.Text.Trim();
        DataTable dt = myService.SearchMovies(searchTerm, "");

        dtlMovies.DataSource = dt;
        dtlMovies.DataBind();
    }

    protected void dtlMovies_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "AddToWatch")
        {
            int movieId = Convert.ToInt32(e.CommandArgument);
            if (!watchListMovieIds.Contains(movieId))
            {
                watchListMovieIds.Add(movieId);
                // כאן ניתן להוסיף שמירה ב־Session או DB
                ClientScript.RegisterStartupScript(GetType(), "added", "alert('סרט נוסף לרשימת הצפייה!');", true);
            }
        }
    }
}


