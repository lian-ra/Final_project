using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ManageMovies : System.Web.UI.Page
{
    private localhost.Service backendService = new localhost.Service();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["status"] == null || !Session["status"].ToString().Equals("2"))
        {
            string script = @"alert('You are not welcome!'); setTimeout(function() {window.location = 'login.aspx';}, 10);";
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script, true);
            return;
        }

        if (!IsPostBack)
        {
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

            backendService.AddMovie(movie);

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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindMoviesGrid();
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
        lblManageMessage.CssClass = isSuccess ? "message-label message-success" : "message-label message-error";
    }

    private void BindMoviesGrid()
    {
        try
        {
            string searchTerm = txtSearch.Text.Trim();
            DataTable dt;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                dt = backendService.SearchMovies(searchTerm, "all");
            }
            else
            {
                dt = backendService.GetAllMovies();
            }

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

            // Accessing cells by index since we have known columns
            // Cell 0: ID (ReadOnly) - we can't edit it
            // Cell 1: Title
            // Cell 2: Year
            // Cell 3: Genre
            // Cell 4: Rating
            // Cell 5: Director
            // Cell 6: Actors
            // Cell 7: Duration
            // Cell 8: Actions

            // Note: The index shifts based on AutoGenerateColumns=False. 
            // In the ASPX above:
            // 0: ID
            // 1: Title
            // 2: Year
            // 3: Genre
            // 4: Rating
            // 5: Director
            // 6: Actors
            // 7: Duration
            // 8: Actions (CommandField)

            // TextBoxes are the first control in the cell during edit mode
            string title = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();
            string yearText = ((TextBox)row.Cells[2].Controls[0]).Text.Trim();
            string genre = ((TextBox)row.Cells[3].Controls[0]).Text.Trim();
            string ratingText = ((TextBox)row.Cells[4].Controls[0]).Text.Trim();
            string director = ((TextBox)row.Cells[5].Controls[0]).Text.Trim();
            string actors = ((TextBox)row.Cells[6].Controls[0]).Text.Trim();
            string durationText = ((TextBox)row.Cells[7].Controls[0]).Text.Trim();
            string poster = ((TextBox)row.Cells[8].Controls[0]).Text.Trim();
            string description = ((TextBox)row.Cells[9].Controls[0]).Text.Trim();

            int year;
            int.TryParse(yearText, out year);

            decimal rating;
            decimal.TryParse(ratingText, out rating);

            int duration;
            int.TryParse(durationText, out duration);

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
            movie.Description = description;



            backendService.UpdateMovie(movie);

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
            backendService.DeleteMovie(movieId);
            BindMoviesGrid();
            ShowMessage("Movie deleted successfully.", true);
        }
        catch (Exception ex)
        {
            ShowMessage("Error deleting movie: " + ex.Message, false);
        }
    }
}
