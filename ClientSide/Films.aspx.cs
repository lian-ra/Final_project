using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Films : System.Web.UI.Page
{
    private localhost.Service myService = new localhost.Service();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadShopMovies();
        }
    }

    private void LoadShopMovies()
    {
        // This function loads movies into the DataList 'dtlMovies' found in Films.aspx
        try
        {
            // Assuming you want to show all movies initially
            DataTable dt = myService.SearchMovies("", "");

            // Map the DataTable columns to match what the DataList expects (pic, name, price)
            // You might need to adjust column names if your database is different
            if (dt != null)
            {
                // Adding dummy columns if they don't exist to prevent binding errors
                if (!dt.Columns.Contains("pic")) dt.Columns.Add("pic", typeof(string), "Poster");
                if (!dt.Columns.Contains("name")) dt.Columns.Add("name", typeof(string), "Title");
                if (!dt.Columns.Contains("price")) dt.Columns.Add("price", typeof(string), "'$10.00'"); // Dummy price

                dtlMovies.DataSource = dt;
                dtlMovies.DataBind();
            }
        }
        catch
        {
            // Handle error
        }
    }
}