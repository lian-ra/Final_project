using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ManageEventStore : System.Web.UI.Page
{
    localhost.Service srv = new localhost.Service();

    //הפעולה מוודאת שהמשתמש מחובר ומחזירה את
    //שם המשתמש שלו מנתוני הזיכרון ההשמורים במערכת
    private string GetLoggedInUsername()
    {
        string status = Session["status"] as string;
        if (status != "1" && status != "2") return null;

        DataTable dt = Session["data"] as DataTable;
        if (dt != null && dt.Rows.Count > 0)
        {
            if (dt.Columns.Contains("User")) return dt.Rows[0]["User"].ToString();
            return dt.Rows[0][0].ToString();
        }
        return null;
    }

    //הפעולה מוודאת שהמשתמש מחובר ושמזהה האירוע תקין, בודקת
    //שהמשתמש הוא אכן בעל האירוע, ואם הכל תקין היא מציגה
    //את שם האירוע
    protected void Page_Load(object sender, EventArgs e)
    {
        string username = GetLoggedInUsername();
        if (username == null)
        {
            Response.Redirect("Login.aspx");
            return;
        }
        string eventIdStr = Request.QueryString["eventId"];
        int eventId = 0;
        if (string.IsNullOrEmpty(eventIdStr) || !int.TryParse(eventIdStr, out eventId))
        {
            Response.Redirect("Events.aspx");
            return;
        }
        DataTable dtEvent = srv.GetEventById(eventId);
        if (dtEvent == null || dtEvent.Rows.Count == 0 ||
            !dtEvent.Rows[0]["Username"].ToString().Equals(username, StringComparison.OrdinalIgnoreCase))
                                                   //זה שרשום והמשתמש שלך
        {
            Response.Redirect("EventDetails.aspx?eventId=" + eventId);
            return;
        }
        if (!IsPostBack)
        {
            lblEventName.Text = dtEvent.Rows[0]["Title"].ToString();
            LoadStoreData(eventId);
        }
    }

    //הפעולה טוענת ומציגה שתי רשימות מוצרים עבור אירוע ספציפי: מוצרים
    //שעוד לא צורפו אליו ומוצרים שכבר קיימים בו, ומציגה הודעה מתאימה אם אחת הרשימות ריקה
    private void LoadStoreData(int eventId)
    {
        DataTable dtNotInEvent = srv.GetProductsNotInEvent(eventId);
        rptGlobalStock.DataSource = dtNotInEvent; //כל המוצרים הכללית הקיימת 
        rptGlobalStock.DataBind();
        lblGlobalEmpty.Visible = (dtNotInEvent == null || dtNotInEvent.Rows.Count == 0); //שדה טקסט-  רק כשאין מוצרים ברשימה

        DataTable dtInEvent = srv.GetEventProducts(eventId);
        rptEventStock.DataSource = dtInEvent; // המוצרים ששויכו לאירוע הספציפי שבו אנחנו נמצאים
        rptEventStock.DataBind();
        lblEventEmpty.Visible = (dtInEvent == null || dtInEvent.Rows.Count == 0);
    }

    //הפעולה מזהה מתי נלחץ כפתור ה"הוספה" ברשימת המלאי
    //הכללית, מוסיפה את המוצר הנבחר לאירוע הנוכחי ומעדכנת את תצוגת הרשימות בדף
    protected void rptGlobalStock_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Add")
        {
            int productId = Convert.ToInt32(e.CommandArgument);
            int eventId = Convert.ToInt32(Request.QueryString["eventId"]);
            srv.AddProductToEvent(eventId, productId);
            LoadStoreData(eventId);
        }
    }

    //הפעולה מזהה מתי נלחץ כפתור ה"הסרה" ברשימת מוצרי האירוע, מסירה את
    //המוצר הנבחר מהאירוע הנוכחי ומעדכנת את תצוגת הרשימות בדף
    protected void rptEventStock_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            int productId = Convert.ToInt32(e.CommandArgument);
            int eventId = Convert.ToInt32(Request.QueryString["eventId"]);
            srv.RemoveProductFromEvent(eventId, productId);
            LoadStoreData(eventId);
        }
    }
}
