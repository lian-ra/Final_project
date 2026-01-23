using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadFilms();
        }
    }

    protected void btnSearchFilms_Click(object sender, EventArgs e)
    {
        // Search functionality can be implemented here
        LoadFilms();
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        LinkButton btn = sender as LinkButton;
        string genre = btn.CommandArgument;
        
        // Update active button
        btnAll.CssClass = "genre-btn";
        btnAction.CssClass = "genre-btn";
        btnComedy.CssClass = "genre-btn";
        btnDrama.CssClass = "genre-btn";
        btnHorror.CssClass = "genre-btn";
        btnSciFi.CssClass = "genre-btn";
        
        btn.CssClass = "genre-btn active";
        
        // Filter films by genre (can be implemented with database)
        LoadFilms();
    }

    protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;
        if (ddl != null)
        {
            ViewState["SelectedSort"] = ddl.SelectedValue;
        }
        LoadFilms();
    }

    private void LoadFilms()
    {
        // Films are currently static in the ASPX file
        // Can be extended to load from database
    }
}

