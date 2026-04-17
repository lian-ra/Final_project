using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class ClientArea : System.Web.UI.Page
{
    //Master Page הפעולה מוודאת שהמשתמש מחובר, מעבירה משתמשים לא מורשים לדף ההתחברות, ומעדכנת את דף
    //בפרטי המשתמש המחובר כדי להציג לו תצוגה אישית

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["status"] == null || !Session["status"].ToString().Equals("1"))
        {
            ///אם המשתמש לא מחובר, אני מכינה קוד קטן בשפת גאווהסקריפט. הקוד הזה מקפיץ
            //הודעה (אלרט) שאומרת שחובה להתחבר, ואז מעביר אותו אוטומטית לדף ההתחברות
            string script = @"alert('You are not welcome!'); setTimeout(function()
                            {window.location = 'Login.aspx';}, 10);";

           //זו שגורמת להודעת ההתראה לקפוץ למשתמש על המסך מיד כשהדף נטען
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script, true);
            //this.GetType(): אומר למחשב באיזה דף אנחנו נמצאים כרגע.
            //"MessageBox": זה בסך הכל שם (מזהה) שנתנו לקוד הקטן הזה, כדי שהמחשב לא יריץ אותו פעמיים בטעות
            //script: זה המשתנה שבו שמרנו מקודם את פקודת ההודעה שרצינו להקפיץ.
            //true: אומר למחשב להוסיף באופן אוטומטי את התגיות של הסקריפט, כדי שלא נצטרך לכתוב אותן בעצמנו.

            return;
        }
        try
        {
            string status = Session["status"] as string;
            if (status == "1")
            {
                Design master = (Design)this.Master; //פעולת המרה שיוצרת קשר בין דף התוכן לבין דף העיצוב הראשי
                DataTable dt = Session["data"] as DataTable;//ניגשת לטבלה שנשמרה ב-סיזיון-נתונים בזמן הלוגין.
                                                            //הטבלה הזו מכילה את כל הפרטים של המשתמש מהדאטה-בייס
                if (dt != null && dt.Rows.Count > 0)//מוודאת שהטבלה קיימת ושיש בה לפחות שורה אחת של נתונים
                {
                    string username = dt.Columns.Contains("User") ?
                        dt.Rows[0]["User"].ToString() : dt.Rows[0][0].ToString(); //user אם יש בעמודות של הטבלה עמודה שקוראים לה
                                                                                  //אם כן, אני שולפת את מה שכתוב
                                                                                  //בשורה הראשונה בעמודה הזו ומחזירה את זה אחרת 
                                                                                  ////לוקחת את הערך שנמצא בתא הראשון בטבלה (שורה 0, עמודה 0), כי
                                                                                  // בדרך כלל שם נמצא שם המשתמש או המזהה
                    master.SetUserLoggedIn(username, status);                     
                    //השורה הזו היא הקשר בין הדף הנוכחי לבין דף האב (מאסטר פייג). היא מעדכנת את ה-ראש של האתר
                    //בזמן אמת כדי שהמשתמש יראה שהוא מחובר ושכל האפשרויות האישיות שלו יהיו זמינות לו בתפריט

                    //ה-Header הוא הרכיב העליון באתר שמשמש לניווט ולזיהוי המשתמש.
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
        catch
        {
            Response.Redirect("Login.aspx");
        }
    }

    //הפעולה מאפסת את טבלת רשימת המשאלות דרך השירות,
    //ומקפיצה הודעה למשתמש המאשרת שהאיפוס הצליח או הודעת שגיאה במקרה של תקלה
    protected void btnResetWishlist_Click(object sender, EventArgs e)
    {
        try
        {
            localhost.Service service = new localhost.Service(); //יצירת קשר עם השרת המקומי שמנהל את הנתונים- webservice
            service.ResetWishlistTable(); //הקריאה לפעולה שמוחקת/מנקה את כל הנתונים בטבלת ה-משאלות בדאטה-בייס.
            ClientScript.RegisterStartupScript(this.GetType(), "alert", 
                "alert('Wishlist table has been reset successfully.');", true);
            //פקודה ששולחת הודעת "קופצת" לדפדפן של המשתמש כדי לעדכן אותו שהפעולה הצליחה.
        }
        catch (Exception ex)//המשתנה ששומר את המידע על מה בדיוק השתבש.
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert",
                "alert('An error occurred: " + ex.Message.Replace("'", "\\'") + "');", true);
            //קוד לוקח את הודעת השגיאה ומנקה ממנה תווים שעלולים
            //לשבור את ה-אלרט (כמו גרש), כדי שהמשתמש יראה הודעה מסודרת על התקלה.
        }
    }
}