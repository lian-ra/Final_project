using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Celebs : System.Web.UI.Page
{
    private localhost.Service backendService = new localhost.Service();

    //הפעולה טוענת את רשימת הסלבים בטעינה הראשונה של הדף.
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)//כדי להגיד למחשב:תטען את הנתונים מהדאטה-בייס רק כשהדף נפתח בפעם הראשונה
                        //  אם המשתמש לוחץ על כפתור והדף מתרענן, אני
                        //לא רוצה שהמחשב יטען את הכל מחדש, כי זה ימחוק
                        //את מה שהמשתמש כתב או שינה בתיבות הטקסט.
                        //כאן המשתמש רק עכשיו נכנס לדף, הכל נקי וחדש

        {
            LoadCelebs();
        }
    }

    //הפעולה מפעילה את טעינת רשימת הסלבים מחדש בעת לחיצה על כפתור החיפוש
    protected void btnSearchCelebs_Click(object sender, EventArgs e)
    {
        LoadCelebs();
    }

    //הפעולה מחפשת סלבים לפי טקסט ותפקיד, יוצרת עבור כל אחד "כרטיס" תצוגה
    //ומציגה אותו בטבלה, או מציגה הודעת שגיאה/חוסר תוצאות.
    private void LoadCelebs()
    {
        try
        {
            string searchText = txtSearchCelebs.Text.Trim();
            string role = ddlRole.SelectedValue;

            DataTable dt = backendService.SearchCelebs(searchText, role);

            celebsGrid.Controls.Clear();

            if (dt != null && dt.Rows.Count > 0)//מוודאת שהטבלה קיימת ושיש בה לפחות שורה אחת של נתונים
            {
                foreach (DataRow row in dt.Rows)
                {
                    LiteralControl celebCard = new LiteralControl(GenerateCelebCard(row));
                    celebsGrid.Controls.Add(celebCard);
                }
            }
            else
            {
                LiteralControl noResults = new LiteralControl("<div style='text-align: center; color: white;" +
                    " padding: 40px; grid-column: 1 / -1;'>No celebrities found.</div>");
                celebsGrid.Controls.Add(noResults);
            }
        }
        catch (Exception ex)
        {
            string message = "alert('Error loading celebs: " + ex.Message.Replace("'", "\\'") + "');";
            ClientScript.RegisterStartupScript(this.GetType(), "Error", message, true);
            LiteralControl errorMsg = new LiteralControl("<div style='text-align: center; color: white; " +
                "padding: 40px; grid-column: 1 / -1;'>Error loading celebrities. Please try again.</div>");
            celebsGrid.Controls.Clear();
            celebsGrid.Controls.Add(errorMsg);
        }
    }

    //HTML הפעולה מעצבת כרטיס תצוגה
    //עבור סלב, הכולל את תמונתו, שמו, תפקידו ותיאורו, ויוצרת קישור לדף הפרטים האישיים שלו.
    private string GenerateCelebCard(DataRow row)
    {
        string name = row["Name"] != DBNull.Value ? row["Name"].ToString() : "Unknown";
        string role = row["Role"] != DBNull.Value ? row["Role"].ToString() : "";
        string photo = row["Photo"] != DBNull.Value ? row["Photo"].ToString() : "images/uploads/ava1.jpg";
        string bio = row["Bio"] != DBNull.Value ? row["Bio"].ToString() : "";

        if (!photo.StartsWith("http") && !photo.StartsWith("/") && !photo.StartsWith("~/"))
        {
            photo = "~/" + photo;
        }

        string urlName = HttpUtility.UrlEncode(name);
        string cardHtml = string.Format(@"
            <div class='celeb-card'>
                <a href='CelebDetails.aspx?name={4}' style='text-decoration:none; color:inherit;'>
                    <img src='{0}' alt='{1}' class='celeb-photo' />
                    <div class='celeb-info'>
                        <div class='celeb-name'>{1}</div>
                        <div class='celeb-role'>{2}</div>
                        <div class='celeb-bio'>{3}</div>
                    </div>
                </a>
            </div>",
            ResolveUrl(photo),
            HttpUtility.HtmlEncode(name),
            HttpUtility.HtmlEncode(role),
            HttpUtility.HtmlEncode(bio),
            urlName
        );

        return cardHtml;
    }
}
