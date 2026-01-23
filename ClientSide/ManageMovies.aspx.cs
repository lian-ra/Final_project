using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ManageMovies : System.Web.UI.Page
{
    private localhost.Service myService = new localhost.Service();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Session["status"].ToString().Equals("2"))
        {
            string script = @"alert('You are not welcome!'); setTimeout(function() {window.location = 'login.aspx';}, 10); // 10 = 10/1000 seconds delay";
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script, true);
        }

        if (!IsPostBack)
        {
            // Only admins (status "2") can access this page
            string status = Session["status"] as string;
            if (status != "2")
            {
                Response.Redirect("Login.aspx");
            }

            BindMoviesGrid();
        }
    }

    protected void btnAddMovie_Click(object sender, EventArgs e)
    {
        try
        {
            string title = txtTitle.Text.Trim();
            string yearText = txtYear.Text.Trim();
            string genre = ddlGenre.SelectedValue;
            string ratingText = txtRating.Text.Trim();
            string poster = txtPoster.Text.Trim();
            string director = txtDirector.Text.Trim();
            string actors = txtActors.Text.Trim();
            string durationText = txtDuration.Text.Trim();
            string description = txtDescription.Text.Trim();

            if (string.IsNullOrEmpty(title))
            {
                ShowMessage("Title is required.", false);
                return;
            }

            int year;
            if (!int.TryParse(yearText, out year))
            {
                ShowMessage("Year must be a valid number.", false);
                return;
            }

            decimal rating = 0m;
            if (!string.IsNullOrEmpty(ratingText) && !decimal.TryParse(ratingText, out rating))
            {
                ShowMessage("Rating must be a valid decimal number.", false);
                return;
            }

            int duration = 0;
            if (!string.IsNullOrEmpty(durationText) && !int.TryParse(durationText, out duration))
            {
                ShowMessage("Duration must be a valid number.", false);
                return;
            }

            localhost.Movies movie = new localhost.Movies();
            movie.MovieId = 0;
            movie.Title = title;
            movie.Year = year;
            movie.Genre = genre;
            movie.Rating = rating;
            movie.Poster = string.IsNullOrEmpty(poster) ? "images/uploads/slider1.jpg" : poster;
            movie.Director = director;
            movie.Actors = actors;
            movie.Duration = duration;
            movie.Description = description;

            myService.AddMovie(movie);

            ShowMessage("Movie added successfully.", true);
            ClearForm();
            BindMoviesGrid();
        }
        catch (Exception ex)
        {
            ShowMessage("Error adding movie: " + ex.Message, false);
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearForm();
        lblManageMessage.Visible = false;
    }

    private void ClearForm()
    {
        txtTitle.Text = "";
        txtYear.Text = "";
        ddlGenre.SelectedIndex = 0;
        txtRating.Text = "";
        txtPoster.Text = "";
        txtDirector.Text = "";
        txtActors.Text = "";
        txtDuration.Text = "";
        txtDescription.Text = "";
    }

    private void ShowMessage(string message, bool isSuccess)
    {
        lblManageMessage.Text = message;
        lblManageMessage.Visible = true;
        if (isSuccess)
        {
            lblManageMessage.CssClass = "message-label message-success";
        }
        else
        {
            lblManageMessage.CssClass = "message-label message-error";
        }
    }

    private void BindMoviesGrid()
    {
        try
        {
            var dt = myService.GetAllMovies("");
            grdMovies.DataSource = dt;
            grdMovies.DataBind();
        }
        catch (Exception ex)
        {
            ShowMessage("Error loading movies list: " + ex.Message, false);
        }
    }

    protected void grdMovies_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdMovies.EditIndex = e.NewEditIndex;
        BindMoviesGrid();
    }

    protected void grdMovies_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdMovies.EditIndex = -1;
        BindMoviesGrid();
    }

    protected void grdMovies_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            int movieId = (int)grdMovies.DataKeys[e.RowIndex].Value;

            GridViewRow row = grdMovies.Rows[e.RowIndex];

            string title = ((TextBox)row.Cells[2].Controls[0]).Text.Trim();
            string yearText = ((TextBox)row.Cells[3].Controls[0]).Text.Trim();
            string genre = ((TextBox)row.Cells[4].Controls[0]).Text.Trim();
            string ratingText = ((TextBox)row.Cells[5].Controls[0]).Text.Trim();
            string poster = ((TextBox)row.Cells[6].Controls[0]).Text.Trim();
            string director = ((TextBox)row.Cells[7].Controls[0]).Text.Trim();
            string actors = ((TextBox)row.Cells[8].Controls[0]).Text.Trim();
            string durationText = ((TextBox)row.Cells[9].Controls[0]).Text.Trim();

            int year;
            if (!int.TryParse(yearText, out year))
            {
                ShowMessage("Year must be a valid number.", false);
                return;
            }

            decimal rating = 0m;
            if (!string.IsNullOrEmpty(ratingText) && !decimal.TryParse(ratingText, out rating))
            {
                ShowMessage("Rating must be a valid decimal number.", false);
                return;
            }

            int duration = 0;
            if (!string.IsNullOrEmpty(durationText) && !int.TryParse(durationText, out duration))
            {
                ShowMessage("Duration must be a valid number.", false);
                return;
            }

            localhost.Movies movie = new localhost.Movies();
            movie.MovieId = movieId;
            movie.Title = title;
            movie.Year = year;
            movie.Genre = genre;
            movie.Rating = rating;
            movie.Poster = poster;
            movie.Director = director;
            movie.Actors = actors;
            movie.Duration = duration;
            movie.Description = ""; // description is not in grid; keep as empty or could be fetched if needed

            myService.UpdateMovie(movie);

            grdMovies.EditIndex = -1;
            BindMoviesGrid();
            ShowMessage("Movie updated successfully.", true);
        }
        catch (Exception ex)
        {
            ShowMessage("Error updating movie: " + ex.Message, false);
        }
    }

    protected void grdMovies_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int movieId = (int)grdMovies.DataKeys[e.RowIndex].Value;
            myService.DeleteMovie(movieId);
            BindMoviesGrid();
            ShowMessage("Movie deleted successfully.", true);
        }
        catch (Exception ex)
        {
            ShowMessage("Error deleting movie: " + ex.Message, false);
        }
    }
}
