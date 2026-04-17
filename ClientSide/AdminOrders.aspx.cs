using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminOrders : System.Web.UI.Page
{
    localhost.Service srv = new localhost.Service();

    public class AdminOrder
    {
        public int OrderId { get; set; }
        public string Buyer { get; set; }
        public string EventOwner { get; set; }
        public string MovieTitle { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime DatePurchased { get; set; }
        public decimal Total { get; set; }
        public string ItemsSummary { get; set; }
    }

    //הפעולה בודקת אם המשתמש המחובר הוא מנהל ואם כן היא מחזירה את שם המשתמש
    //שלו מתוך הנתונים שנשמרו במערכת. אם הוא לא מנהל, היא לא מחזירה כלום
    private string GetLoggedInUsername()
    {
        string status = Session["status"] as string;
        if (status != "2") return null; 

        DataTable dt = Session["data"] as DataTable; //ניגשת לטבלה שנשמרה ב-סיזיון-נתונים בזמן הלוגין.
                                                     //הטבלה הזו מכילה את כל הפרטים של המשתמש מהדאטה-בייס
        if (dt != null && dt.Rows.Count > 0) //מוודאת שהטבלה קיימת ושיש בה לפחות שורה אחת של נתונים
        {
            if (dt.Columns.Contains("User")) //user אם יש בעמודות של הטבלה עמודה שקוראים לה
                return dt.Rows[0]["User"].ToString();//אם כן, אני שולפת את מה שכתוב
                                                     //בשורה הראשונה בעמודה הזו ומחזירה את זה

            return dt.Rows[0][0].ToString();//לוקחת את הערך שנמצא בתא הראשון בטבלה (שורה 0, עמודה 0), כי
                                            // בדרך כלל שם נמצא שם המשתמש או המזהה
        }
        return null;
    }

    //הפעולה בודקת אם המשתמש מחובר (מנהל): אם לא, היא מעבירה אותו
    //לדף ההתחברות. אם כן, היא טוענת עבורו את רשימת כל ההזמנות הקיימות במערכת.
    protected void Page_Load(object sender, EventArgs e)
    {
        string username = GetLoggedInUsername();
        if (username == null) 
        {
            Response.Redirect("Login.aspx");
            return;
        }

        if (!IsPostBack)//כדי להגיד למחשב:תטען את הנתונים מהדאטה-בייס רק כשהדף נפתח בפעם הראשונה
        {               //  אם המשתמש לוחץ על כפתור והדף מתרענן, אני
                        //לא רוצה שהמחשב יטען את הכל מחדש, כי זה ימחוק
                        //את מה שהמשתמש כתב או שינה בתיבות הטקסט.
                        //כאן המשתמש רק עכשיו נכנס לדף, הכל נקי וחדש

            LoadAllOrders();//קוראת לפעולה שטוענת את כל ההזמנות
                            //שלו מהדאטה-בייס ומציגה אותן על המסך
        }
    }

    //הפעולה מפעילה את טעינת כל ההזמנות מחדש, תוך סינון התוצאות לפי הטקסט שהמנהל הקליד בתיבת החיפוש
    protected void btnSearchOrders_Click(object sender, EventArgs e)
    {
        LoadAllOrders(txtSearch.Text.Trim());
        //קוראת לפעולה שטוענת את ההזמנות, אבל הפעם
        //אני שולחת לה את המילה שהמשתמש חיפש. ככה היא תדע
        //להציג רק את ההזמנות שמתאימות לחיפוש ולא את כולן
    }

    //הפעולה מנקה את מה שכתוב בתיבת החיפוש ומציגה מחדש את רשימת כל ההזמנות ללא סינון.
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSearch.Text = ""; //מרוקנת את תיבת הטקסט של החיפוש. אני שמה
                             //שם מחרוזת ריקה כדי שהמשתמש יראה שהטקסט שהוא כתב נמחק.

        LoadAllOrders(""); //אני קוראת שוב לפעולה שטוענת את ההזמנות, אבל
                           //הפעם אני שולחת לה גרשיים ריקים. זה גורם לה להבין שאין יותר
                           //סינון, והיא פשוט מציגה מחדש את כל ההזמנות של המשתמש
    }

    //הפעולה מציגה למנהל את כל ההזמנות באתר, כולל סינון לפי חיפוש,
    //פירוט מוצרים לכל הזמנה וחישוב סך כל הרווחים וההזמנות שבוצעו
    private void LoadAllOrders(string searchTerm = "")
    {                        //מקבלת מילת חיפוש

        DataTable dtOrders = srv.GetAllOrders();//פונה ל-סרוויס ומבקשת ממנו להביא לי
                                                //את כל הטבלה של ההזמנות מהדאטה-בייס

        List<AdminOrder> ordersList = new List<AdminOrder>();//וצרת רשימה ריקה של אובייקטים מסוג אדמין
                                                    //אורדרס. כאן אני אשמור את ההזמנות אחרי שאעבד אותן

        decimal totalRev = 0; //משתנה שיסכום את סך כל ההכנסות מכל ההזמנות שיוצגו

        if (dtOrders != null && dtOrders.Rows.Count > 0)
        {
            foreach (DataRow row in dtOrders.Rows)//עוברת שורה-שורה על הטבלה שחזרה מה-אסקיואל
                                                  //כדי לטפל בכל הזמנה בנפרד
            {
                int orderId = Convert.ToInt32(row["OrderId"]); 
                string buyer = row["Username"].ToString();
                string eventOwner = row["EventOwner"].ToString();
                string movieTitle = row["MovieTitle"].ToString();
                
                if (!string.IsNullOrEmpty(searchTerm))
                { //בודקת אם מילת החיפוש לא נמצאת ב...
                    string s = searchTerm.ToLower();
                    if (!orderId.ToString().Contains(s) && 
                        !buyer.ToLower().Contains(s) && 
                        !eventOwner.ToLower().Contains(s) && 
                        !movieTitle.ToLower().Contains(s))
                    {
                        continue; //אם היא לא נמצאת באף אחד מהם נדלג על ההזמנה הזו ולא להוסיף אותה לרשימה
                    }
                }
                
                AdminOrder order = new AdminOrder();//אובייקט חדש של הזמנה וממלאת אותו בנתונים מהשורה בטבלה
                order.OrderId = orderId;
                order.Buyer = buyer;
                order.EventOwner = eventOwner;
                order.MovieTitle = movieTitle;
                order.EventDate = Convert.ToDateTime(row["EventDate"]);
                order.DatePurchased = Convert.ToDateTime(row["DatePurchased"]);
                order.Total = Convert.ToDecimal(row["Total"]);
                totalRev += order.Total;
                
                DataTable dtItems = srv.GetOrderItems(orderId);//לכל הזמנה יש מוצרים (כמו פופקורן או שתייה). אני פונה שוב
                                                               //ל-סרוויס כדי להביא את רשימת המוצרים הספציפית להזמנה הזו.
                List<string> itemStrs = new List<string>();
                if (dtItems != null)
                {
                    foreach(DataRow iRow in dtItems.Rows)
                    {
                        itemStrs.Add(iRow["Quantity"].ToString() + "x " + iRow["ProductName"].ToString());
                        //בונה מחרוזת יפה לכל מוצר
                    }
                }     //סיכום פריטים
                order.ItemsSummary = string.Join("<br/>", itemStrs); //מאוסף מחרוזות למחרוזת אחת
                //מחברת את כל המוצרים למחרוזת אחת ארוכה עם ירידת שורה (<ביאר/>), כדי שזה יוצג יפה בטבלה באתר

                ordersList.Add(order);//מוסיפה אותה לרשימה הסופית שתועבר לתצוגה
            }
        }
        
        if (ordersList.Count == 0)//אם הרשימה ריקה
        {
            //מציגה הודעה למשתמש ('לא נמצאו הזמנות') ומסתירה
            //את הטבלה ואת הסטטיסטיקות, כי אין טעם להציג טבלה ריקה.
            lblEmpty.Visible = true;

            //מסתירה את פקד התצוגה של ההזמנות. אם הרשימה ריקה, אין טעם להציג
            //את כותרות הטבלה או מסגרת ריקה, אז אני פשוט מעלימה אותה כדי שהדף ייראה נקי
            rptAllOrders.Visible = false;

            //מסתירה זה שמראה סך הכל רווחים ומספר הזמנות
            //אם אין תוצאות עדיף להסתיר את כל האזור הזה כדי לא לבלבל את המשתמש
            pnlStats.Visible = false;
        }
        else
        {
            lblEmpty.Visible = false;//מסתירה את ההודעה שאומרת שהרשימה ריקה, כי הפעם מצאנו נתונים להציג.

            rptAllOrders.DataSource = ordersList;//מחברת את רשימת ההזמנות שהכנתי לפקד
                                                 //התצוגה, כדי שהוא ידע מאיפה לקחת את המידע.

            rptAllOrders.DataBind();//זו הפקודה שמבצעת את החיבור בפועל ומציירת
                                    //את כל הנתונים בתוך הטבלה באתר.

            rptAllOrders.Visible = true;//גורמת לטבלת ההזמנות להופיע על המסך כדי שהמשתמש יוכל לראות אותה.

            pnlStats.Visible = true;//מציגה את האזור של הסיכומים והסטטיסטיקות בתחתית הדף.
            lblTotalOrders.Text = ordersList.Count.ToString();//סופרת כמה הזמנות יש ברשימה ומציגה את
                                                              //המספר הזה בתוך התווית המתאימה.
            lblTotalRevenue.Text = totalRev.ToString("0.00") + " ILS"; //ציגה את סכום הרווח הכולל, מעגלת
                                                                         //אותו לשתי ספרות אחרי הנקודה ומוסיפה את סמל השקלים.
        }
    }
}
