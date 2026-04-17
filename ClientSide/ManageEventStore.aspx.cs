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

        DataTable dt = Session["data"] as DataTable; //ניגשת לטבלה שנשמרה ב-סיזיון-נתונים בזמן הלוגין.
                                                     //הטבלה הזו מכילה את כל הפרטים של המשתמש מהדאטה-בייס
        if (dt != null && dt.Rows.Count > 0)//מוודאת שהטבלה קיימת ושיש בה לפחות שורה אחת של נתונים
        {
            if (dt.Columns.Contains("User")) return dt.Rows[0]["User"].ToString();
            return dt.Rows[0][0].ToString();
            //user אם יש בעמודות של הטבלה עמודה שקוראים לה
            //אם כן, אני שולפת את מה שכתוב
            //בשורה הראשונה בעמודה הזו ומחזירה את זה אחרת 
            ////לוקחת את הערך שנמצא בתא הראשון בטבלה (שורה 0, עמודה 0), כי
            // בדרך כלל שם נמצא שם המשתמש או המזהה
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
        if (!IsPostBack)//כדי להגיד למחשב:תטען את הנתונים מהדאטה-בייס רק כשהדף נפתח בפעם הראשונה
                        //  אם המשתמש לוחץ על כפתור והדף מתרענן, אני
                        //לא רוצה שהמחשב יטען את הכל מחדש, כי זה ימחוק
        {               //את מה שהמשתמש כתב או שינה בתיבות הטקסט.
                        //כאן המשתמש רק עכשיו נכנס לדף, הכל נקי וחדש

            lblEventName.Text = dtEvent.Rows[0]["Title"].ToString();
            LoadStoreData(eventId);
        }
    }

    //הפעולה טוענת ומציגה שתי רשימות מוצרים עבור אירוע ספציפי: מוצרים
    //שעוד לא צורפו אליו ומוצרים שכבר קיימים בו, ומציגה הודעה מתאימה אם אחת הרשימות ריקה
    private void LoadStoreData(int eventId)
    {
        DataTable dtNotInEvent = srv.GetProductsNotInEvent(eventId);
        rptGlobalStock.DataSource = dtNotInEvent; //בשורה הזו אני מגדירה לרכיב התצוגה את המקור
           //שממנו הוא שואב את הנתונים – במקרה הזה, הטבלה שמכילה את המוצרים שאינם משויכים לאירוע

        rptGlobalStock.DataBind();
        //זו הפקודה שמבצעת את הקישור הסופי. היא גורמת לרכיב התצוגה
        //להתעדכן ולהציג את כל המוצרים מהטבלה ישירות בדף האינטרנט

        lblGlobalEmpty.Visible = (dtNotInEvent == null || dtNotInEvent.Rows.Count == 0);
        //שדה טקסט () שמופיע רק כשאין מוצרים ברשימה, כדי להודיע למשתמש שהרשימה ריקה.

        DataTable dtInEvent = srv.GetEventProducts(eventId);
        rptEventStock.DataSource = dtInEvent; // המוצרים ששויכו לאירוע הספציפי שבו אנחנו נמצאים
        rptEventStock.DataBind();
        lblEventEmpty.Visible = (dtInEvent == null || dtInEvent.Rows.Count == 0);
    }

    //הפעולה מזהה מתי נלחץ כפתור ה"הוספה" ברשימת המלאי
    //הכללית, מוסיפה את המוצר הנבחר לאירוע הנוכחי ומעדכנת את תצוגת הרשימות בדף
    protected void rptGlobalStock_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Add")//משתנה ששומר את שם הפעולה שהגדרנו לכפתור
        {
            int productId = Convert.ToInt32(e.CommandArgument);//תעודת זהות של המוצר הספציפי שלחצו עליו.
            int eventId = Convert.ToInt32(Request.QueryString["eventId"]);
            srv.AddProductToEvent(eventId, productId);
            LoadStoreData(eventId);
        }
    }

    //הפעולה מזהה מתי נלחץ כפתור ה"הסרה" ברשימת מוצרי האירוע, מסירה את
    //המוצר הנבחר מהאירוע הנוכחי ומעדכנת את תצוגת הרשימות בדף
    protected void rptEventStock_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Remove")//משתנה ששומר את שם הפעולה שהגדרנו לכפתור
        {
            int productId = Convert.ToInt32(e.CommandArgument);//תעודת זהות של המוצר הספציפי שלחצו עליו.
            int eventId = Convert.ToInt32(Request.QueryString["eventId"]);
            srv.RemoveProductFromEvent(eventId, productId);
            LoadStoreData(eventId);
        }
    }
}
