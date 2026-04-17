using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EventOrders : System.Web.UI.Page
{
    localhost.Service srv = new localhost.Service();

    public class EventOrderDisplay
    {
        public int OrderId { get; set; }
        public string Buyer { get; set; }
        public DateTime DatePurchased { get; set; }
        public decimal Total { get; set; }
        public string ItemsSummary { get; set; }
    }

    //הפעולה בודקת אם המשתמש מחובר למערכת לפי הסטטוס
    //שלו ושולפת את שם המשתמש 
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

    //הפעולה מוודאת שהמשתמש מחובר ושהוא בעל האירוע, ואם הכל תקין
    //היא מציגה את שם האירוע וטוענת את ההזמנות שלו.
    protected void Page_Load(object sender, EventArgs e)
    {
        string username = GetLoggedInUsername();
        if (username == null)
        {
            Response.Redirect("Login.aspx");
            return;
        }

        string eventIdStr = Request.QueryString["eventId"];// הדרך של הדף לקרוא נתונים מהכתובת של האתר

        int eventId = 0;
        if (string.IsNullOrEmpty(eventIdStr) || !int.TryParse(eventIdStr, out eventId))
        {
            Response.Redirect("Events.aspx");
            return;
        }

        // Verify Owner
        DataTable dtEvent = srv.GetEventById(eventId);//מבקשת את כל המידע על אירוע ספציפי לפי ה-מספר מזהה שלו.
        if (dtEvent == null || dtEvent.Rows.Count == 0 ||
            !dtEvent.Rows[0]["Username"].ToString().Equals(username, StringComparison.OrdinalIgnoreCase))// התעלמות מהבדל באותיות קטנות או גדולות
            //השורה בודקת אם השם של בעל האירוע ששמור במערכת זהה לשם של המשתמש שמחובר כרגע.
            //השתמשתי בהשוואה שמתעלמת מהבדלים בין אותיות גדולות
            //לקטנות כדי למנוע שגיאות בזיהוי המשתמש ולאפשר לו גישה בטוחה לניהול האירוע של
        {
            Response.Redirect("EventDetails.aspx?eventId=" + eventId);
            return;
        }

        if (!IsPostBack)//כדי להגיד למחשב:תטען את הנתונים מהדאטה-בייס רק כשהדף נפתח בפעם הראשונה
                        //  אם המשתמש לוחץ על כפתור והדף מתרענן, אני
                        //לא רוצה שהמחשב יטען את הכל מחדש, כי זה ימחוק
                        //את מה שהמשתמש כתב או שינה בתיבות הטקסט.
                        //כאן המשתמש רק עכשיו נכנס לדף, הכל נקי וחדש

        {
            lblEventName.Text = dtEvent.Rows[0]["Title"].ToString();
            LoadOrders(eventId); //אחראית לשלוף ממסד הנתונים את כל ההזמנות שקשורות
                                 //לאירוע הספציפי הזה ולהציג אותן בתוך הטבלה או הרשימה המתאימה בדף
        }
    }

    //הפעולה טוענת את כל ההזמנות של אירוע ספציפי, מחשבת את סך
    //ההכנסות ומציגה את רשימת הפריטים בדף או הודעה במידה ואין הזמנות.
    private void LoadOrders(int eventId)
    {
        DataTable dtOrders = srv.GetEventOrders(eventId);
        List<EventOrderDisplay> ordersList = new List<EventOrderDisplay>();

        //EventOrderDisplay- זהו אובייקט (קלאס) עזר שיצרתי. הוא משמש כטיפוס נתונים שמרכז
        //את כל המידע שצריך להציג עבור הזמנה בודדת.
        //השורה הזו מגדירה רשימה של האובייקטים האלו,
        //כדי שנוכל לאסוף את כל ההזמנות מהדאטה-בייס ולהעביר אותן בצורה מסודרת לתצוגה בדף."

        decimal totalRev = 0;

        if (dtOrders != null && dtOrders.Rows.Count > 0)
        {
            foreach (DataRow row in dtOrders.Rows)
            {
                int orderId = Convert.ToInt32(row["OrderId"]);
                
                EventOrderDisplay order = new EventOrderDisplay();
                order.OrderId = orderId;
                order.Buyer = row["Username"].ToString();
                order.DatePurchased = Convert.ToDateTime(row["DatePurchased"]);
                order.Total = Convert.ToDecimal(row["Total"]);
                totalRev += order.Total;
                
                // Get Items
                DataTable dtItems = srv.GetOrderItems(orderId);
                List<string> itemStrs = new List<string>();
                if (dtItems != null)
                {
                    foreach(DataRow iRow in dtItems.Rows)
                    {
                        itemStrs.Add(iRow["Quantity"].ToString() + "x " + iRow["ProductName"].ToString());
                    }
                }
                
                order.ItemsSummary = string.Join("<br/>", itemStrs); //מאוסף מחרוזות למחרוזת אחת
                //השורה הזו לוקחת את כל המילים  ומדביקה אותן אחת לשנייה כדי ליצור טקסט אחד ארוך
                //כל פריט מתחת לשני

                ordersList.Add(order);
            }
        }
        
        if (ordersList.Count == 0) //אם הרשימה ריקה
        {
            lblEmpty.Visible = true; //מציג הודעה למשתמש (למשל: "אין עדיין הזמנות לאירוע זה").
            rptOrders.Visible = false; //מסתיר את הטבלה (כדי שלא תופיע טבלה ריקה
            pnlStats.Visible = false;//מסתיר את אזור הסטטיסטיקות (כי אין טעם להראות שרווחת 0 שקלים).
        }
        else
        {
            lblEmpty.Visible = false; //מסתירה את ההודעה שאומרת שהרשימה ריקה, כי הפעם מצאנו נתונים להציג.
            rptOrders.DataSource = ordersList; //מחברת את רשימת ההזמנות שהכנתי לפקד
                                               //התצוגה, כדי שהוא ידע מאיפה לקחת את המידע.

            rptOrders.DataBind();//זו הפקודה שמבצעת את החיבור בפועל ומציירת
                                 //את כל הנתונים בתוך הטבלה באתר.

            rptOrders.Visible = true;//גורמת לטבלת ההזמנות להופיע על המסך כדי שהמשתמש יוכל לראות אותה.

            pnlStats.Visible = true;//מציגה את האזור של הסיכומים  בתחתית הדף.
            lblTotalOrders.Text = ordersList.Count.ToString();//סופרת כמה הזמנות יש ברשימה ומציגה את
                                                              //המספר הזה בתוך התווית המתאימה.

            lblTotalRevenue.Text = totalRev.ToString("0.00") + " ILS";//ציגה את סכום הרווח הכולל, מעגלת
        }                                                               //אותו לשתי ספרות אחרי הנקודה ומוסיפה את סמל השקלים.
    }
}
