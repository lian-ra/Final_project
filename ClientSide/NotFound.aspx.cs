using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NotFound : System.Web.UI.Page
{
    //הפעולה מבצעת זיוף של שגיאת "דף לא נמצא"- 404
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.StatusCode = 404;
        Response.StatusDescription = "Not Found";
        
        string requestedUrl = Request.RawUrl;
        string referrer = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : "Direct";
    }
}

