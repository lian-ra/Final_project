using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class AdminArea : System.Web.UI.Page
{
    //הפעולה בודקת אם המשתמש המחובר הוא מנהל
    //אם לא, היא חוסמת אותו ומעבירה אותו לדף התחברות ואם כן, היא מציגה את השם שלו באתר.
    //במקרה של תקלה, היא מחזירה אותו לדף הבית
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Session["status"].ToString().Equals("2"))
        {

            string script = @"alert('You are not welcome!'); 
            setTimeout(function() {window.location = 'login.aspx';}, 10);
               // 10 = 10/1000 seconds delay";

            //זו שגורמת להודעת ההתראה לקפוץ למשתמש על המסך מיד כשהדף נטען
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script, true);
            //this.GetType(): אומר למחשב באיזה דף אנחנו נמצאים כרגע.
            //"MessageBox": זה בסך הכל שם (מזהה) שנתנו לקוד הקטן הזה, כדי שהמחשב לא יריץ אותו פעמיים בטעות.
            //script: זה המשתנה שבו שמרנו מקודם את פקודת ההודעה שרצינו להקפיץ.
            //true: אומר למחשב להוסיף באופן אוטומטי את התגיות של הסקריפט, כדי שלא נצטרך לכתוב אותן בעצמנו.

        }
        try
        {
            string status = Session["status"] as string; //ניגשת לטבלה שנשמרה ב-סיזיון-נתונים בזמן הלוגין.
            if (status == "2")                           //הטבלה הזו מכילה את כל הפרטים של המשתמש מהדאטה-בייס
            {
                Design master = (Design)this.Master; //פעולת המרה שיוצרת קשר בין דף התוכן לבין דף העיצוב הראשי
                DataTable dt = Session["data"] as DataTable;
                if (dt != null && dt.Rows.Count > 0)//מוודאת שהטבלה קיימת ושיש בה לפחות שורה אחת של נתונים
                {
                    string username = dt.Columns.Contains("Usern") ?               //user אם יש בעמודות של הטבלה עמודה שקוראים לה
                        dt.Rows[0]["Usern"].ToString() : dt.Rows[0][0].ToString(); //אם כן, אני שולפת את מה שכתוב
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
                Response.Redirect("Home.aspx");
            }
        }
        catch
        {
            Response.Redirect("Home.aspx");
        }
    }
}
