using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NotFound : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Set the status code to 404
        Response.StatusCode = 404;
        Response.StatusDescription = "Not Found";
        
        // Log the 404 error if needed (optional)
        string requestedUrl = Request.RawUrl;
        string referrer = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : "Direct";
        
        // You can log this information to a database or file if needed
        // System.Diagnostics.Debug.WriteLine($"404 Error: {requestedUrl} - Referrer: {referrer}");
    }
}

