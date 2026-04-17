using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EventDetails : System.Web.UI.Page
{
    private localhost.Service backendService = new localhost.Service();
    private int currentEventId = 0;

    //הפעולה בודקת שהמשתמש מחובר למערכת
    //אם הוא מחובר היא טוענת את פרטי האירוע ואת רשימת הנרשמים אליו.
    protected void Page_Load(object sender, EventArgs e)
    {
        string status = Session["status"] as string;
        if (status != "1" && status != "2")
        {
            string script = @"alert('You must be logged in to view this page.'); 
             setTimeout(function() {window.location = 'login.aspx';}, 10);";
            //אם המשתמש לא מחובר, אני מכינה קוד קטן בשפת גאווהסקריפט. הקוד הזה מקפיץ
            //הודעה (אלרט) שאומרת שחובה להתחבר, ואז מעביר אותו אוטומטית לדף ההתחברות

            //זו שגורמת להודעת ההתראה לקפוץ למשתמש על המסך מיד כשהדף נטען
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script, true);
            //this.GetType(): אומר למחשב באיזה דף אנחנו נמצאים כרגע.
            //"MessageBox": זה בסך הכל שם (מזהה) שנתנו לקוד הקטן הזה, כדי שהמחשב לא יריץ אותו פעמיים בטעות.
            //script: זה המשתנה שבו שמרנו מקודם את פקודת ההודעה שרצינו להקפיץ.
            //true: אומר למחשב להוסיף באופן אוטומטי את התגיות של הסקריפט, כדי שלא נצטרך לכתוב אותן בעצמנו.

            return;
        }

        if (!IsPostBack)//  //אם המשתמש לוחץ על כפתור והדף מתרענן, אני
                        //לא רוצה שהמחשב יטען את הכל מחדש, כי זה ימחוק
                        //את מה שהמשתמש כתב או שינה בתיבות הטקסט.
                        //כאן המשתמש רק עכשיו נכנס לדף, הכל נקי וחדש


        {
            LoadEventDetails();//פעולה ששולפת מהדאטה-בייס את כל הפרטים של האירוע
                               //כמו שם הסרט, תאריך ושעה ומציגה אותם בתוך תיבות הטקסט בדף.

            LoadSubscribers();//מפעילה פעולה שטוענת את רשימת האנשים
                              //שנרשמו לאירוע ומציגה אותם בתוך הטבלה, כדי שהמשתמש
                              //יוכל לראות מי כבר הזמין כרטיס.
        }
    }

    //הפעולה מחלצת את שם המשתמש המחובר מתוך הנתונים השמורים
    //בתנאי שהמשתמש מזוהה במערכת כמשתמש רשום או כמנהל
    private string GetLoggedInUsername()
    {
        try
        {
            string status = Session["status"] as string;
            if (status != "1" && status != "2") return null;

            DataTable dt = Session["data"] as DataTable; //ניגשת לטבלה שנשמרה ב-סיזיון-נתונים בזמן הלוגין.
                                                         //הטבלה הזו מכילה את כל הפרטים של המשתמש מהדאטה-בייס
            if (dt != null && dt.Rows.Count > 0)//מוודאת שהטבלה קיימת ושיש בה לפחות שורה אחת של נתונים
            {
                if (dt.Columns.Contains("User"))//user אם יש בעמודות של הטבלה עמודה שקוראים לה
                    return dt.Rows[0]["User"].ToString();//אם כן, אני שולפת את מה שכתוב
                                                         //בשורה הראשונה בעמודה הזו ומחזירה את זה

                return dt.Rows[0][0].ToString(); //לוקחת את הערך שנמצא בתא הראשון בטבלה (שורה 0, עמודה 0), כי
            }                                    // בדרך כלל שם נמצא שם המשתמש או המזהה
        }
        catch { }
        return null;
    }

    //הפעולה טוענת את כל פרטי האירוע והסרט ממסד הנתונים ומציגה אותם בדף בתוך
    //כרטיס מעוצב הכולל אפשרויות הרשמה למשתמשים או כלי ניהול ליוצר האירוע.
    private void LoadEventDetails()
    {
        //בודקת את הכתובת של הדף (יואראל) כדי לראות איזה מספר אירוע המשתמש ביקש לראות. אם המספר
        //לא תקין או חסר, אני מציגה הודעת שגיאה ועוצרת.
        if (!int.TryParse(Request.QueryString["eventId"], out currentEventId) || currentEventId <= 0)
        {
            phEventDetails.Controls.Add(new LiteralControl
                ("<div class='empty-subscribers'>Invalid event.</div>")); //במידה והבדיקה נכשלה, אני
                                                                          //מוסיפה הודעת שגיאה מעוצבת ישירות למקום המיועד
                                                                          //בדף, שאומרת למשתמש: "האירוע אינו תקין".
             //phEventDetails- פקד מסוג פלייס אורדר שהגדרת בדף ה-אספק. הוא
             //משמש כ"מקום שמור" או מכולה ריקה
             //בדף, שתוכלי להכניס לתוכה תוכן דינמי דרך הקוד.

            //.Controls.Add()-ו פקודה שאומרת למחשב: "קח את מה שאני נותנת לך עכשיו, ותוסיף
            //אותו לרשימת הרכיבים שנמצאים בתוך המקום השמור

            //new LiteralControl(...)- את יוצרת רכיב חדש מסוג "פקד טקסט חופשי". בניגוד
            //לכפתור או תיבת טקסט רגילה, הפקד הזה פשוט לוקח את מה
            //שתכתבי בתוכו ומתייחס אליו כאל קוד אייץ טי אמ אל

            //"<div class='empty-subscribers'>Invalid event.</div>" -
            //זהו תוכן ה-אייץ טי אמ אל שיוצג למשתמש. את משתמשת בתגית דיב עם מחלקה (קלאס) של
            //עיצוב שכבר כתבת ב-סי אס אס, כדי שהודעת השגיאה תיראה יפה ולא סתם טקסט פשוט.

            return;
        }

        try
        {
            DataTable dt = backendService.GetEventById(currentEventId); //תביא לי מהשירות את
                                                                        //כל המידע על האירוע הזה ושמור אותו בטבלה dt
            if (dt == null || dt.Rows.Count == 0)
            {
                phEventDetails.Controls.Add(new LiteralControl("<div class='empty-subscribers'>Event not found.</div>"));

                //phEventDetails- פקד מסוג פלייס אורדר שהגדרת בדף ה-אספק. הוא
                //משמש כ"מקום שמור" או מכולה ריקה
                //בדף, שתוכלי להכניס לתוכה תוכן דינמי דרך הקוד.

                //.Controls.Add()-ו פקודה שאומרת למחשב: "קח את מה שאני נותנת לך עכשיו, ותוסיף
                //אותו לרשימת הרכיבים שנמצאים בתוך המקום השמור

                //new LiteralControl(...)- את יוצרת רכיב חדש מסוג "פקד טקסט חופשי". בניגוד
                //לכפתור או תיבת טקסט רגילה, הפקד הזה פשוט לוקח את מה
                //שתכתבי בתוכו ומתייחס אליו כאל קוד אייץ טי אמ אל

                //"<div class='empty-subscribers'>Invalid event.</div>" -
                //זהו תוכן ה-אייץ טי אמ אל שיוצג למשתמש. את משתמשת בתגית דיב עם מחלקה (קלאס) של
                //עיצוב שכבר כתבת ב-סי אס אס, כדי שהודעת השגיאה תיראה יפה ולא סתם טקסט פשוט.

                return;
            }

            DataRow row = dt.Rows[0];
            string username = GetLoggedInUsername();
            string eventOwner = row["Username"].ToString();
            string movieTitle = row["Title"].ToString();
            string poster = row["Poster"] != DBNull.Value ?
                row["Poster"].ToString() : "images/uploads/slider1.jpg";
            string eventDate = Convert.ToDateTime(row["EventDate"]).ToString("MMMM dd, yyyy");
            string startTime = row["StartTime"] != DBNull.Value ? 
                Convert.ToDateTime(row["StartTime"].ToString()).ToString("HH:mm") : "";
            decimal price = row["Price"] != DBNull.Value ? Convert.ToDecimal(row["Price"]) : 0;
            string eventStatus = row["Status"].ToString();
            string genre = row["Genre"] != DBNull.Value ? 
                row["Genre"].ToString() : "";
            string director = row["Director"] != DBNull.Value ? row["Director"].ToString() : "";
            string actors = row["Actors"] != DBNull.Value ? 
                row["Actors"].ToString() : "";
            int movieId = Convert.ToInt32(row["MovieId"]);
            string location = row.Table.Columns.Contains("Location") && 
                row["Location"] != DBNull.Value ? row["Location"].ToString() : "TBD"; //קיצור של ייקבע בהמשך

            // DBNull.Value זהו אובייקט שמייצג ערך חסר ואנחנו בודקים אותו כדי לוודא שלא ננסה להשתמש בנתון שלא קיים.

            if (!poster.StartsWith("http") && !poster.StartsWith("/") && !poster.StartsWith("~/"))
            {
                poster = "~/" + poster;
            }

            //השורה הזו מחברת בין הנתונים מהדאטה-בייס לבין העיצוב - סי אס אס.
            //אני יוצרת שם של מחלקה באופן דינמי לפי
            //מצב האירוע, וככה האתר יודע לצבוע את הסטטוס בצבע הנכון באופן אוטומטי.
            string statusClass = "status-" + eventStatus.ToLower();

            //השורה הזו בודקת סטטוס הרשמה בצורה בטוחה. קודם
            //כל היא מוודאת שיש בכלל משתמש מחובר, ורק אז היא פונה לדאטה-בייס כדי
            //לבדוק אם הוא רשום לאירוע הספציפי הזה. התוצאה נשמרת
            //במשתנה בוליאני שמשמש אותי להצגת הכפתורים הנכונים.
            bool isSubscribed = !string.IsNullOrEmpty(username) &&
                backendService.IsUserSubscribed(currentEventId, username);

            //השורה הזו בודקת אם המשתמש המחובר הוא יוצר האירוע.
            //אני משתמשת בהשוואה שמתעלמת מאותיות גדולות וקטנות כדי למנוע
            //טעויות זיהוי, וזה עוזר לי להחליט אם להציג למשתמש את אפשרויות הניהול של האירוע
            bool isOwner = !string.IsNullOrEmpty(username) && 
                username.Equals(eventOwner, StringComparison.OrdinalIgnoreCase);//מתעלם מאותיות גדולות/קטנות

            if (isOwner)
            {
                pnlOwnerControls.Visible = true;//הצגת אפשרויות ניהול
                //מכיל אפשרויות שרק המארח אמור לראות, כמו "עריכת אירוע" או "מחיקת אירוע".
            }

            string subscriptionButton = "";
            if (!string.IsNullOrEmpty(username) && !isOwner)
            {
                if (isSubscribed)
                {
                    //החלק הזה בונה את ממשק המשתמש הדינמי. הקוד מחליט בזמן אמת איזה כפתור
                    //להציג לגולש לפי המצב שלו: האם הוא כבר רשום, האם הוא המארח,
                    //והאם האירוע עדיין פתוח להרשמה. השתמשתי בסטרינג.פורמט כדי לוודא שכל כפתור יוביל בדיוק לאירוע הנכון
                    subscriptionButton = string.Format("<a href='EventDetails.aspx?eventId={0}&action=unsubscribe" +
                        "' class='btn-action btn-unsubscribe'>Unsubscribe</a>", currentEventId);
                    subscriptionButton += string.Format(" <a href='EventStore.aspx?eventId={0}' class='btn-action " +
                        "btn-subscribe' " +
                        "style='background: linear-gradient(135deg, #ff4c3b 0%, #d82b1f 100%); margin-left:10px;'><i " +
                        "class='fa fa-shopping-cart'></i> Event Store</a>"
                        , currentEventId);

                    //string.Format(...)- זו פקודה שעוזרת לי לבנות את הקישור בצורה חכמה. במקום
                    //שבו כתוב {0}, המחשב שותל באופן אוטומטי את ה-מספר מזהה
                    //של האירוע הנוכחי, כדי שהכפתור ידע בדיוק לאיזה אירוע להירשם.
                }
                else if (eventStatus.Equals("Open", StringComparison.OrdinalIgnoreCase)) //תשווה את המילים, אבל
                                                                                         //תתעלם מההבדל בין אותיות גדולות לקטנות
                {
                                         //מבטיח שהכפתורים יובילו בדיוק לאירוע הנכון שהמשתמש נמצא בו כרגע. 
                    subscriptionButton = string.Format("<a href='EventDetails.aspx?eventId={0}&action=subscribe'" +
                        " class='btn-action btn-subscribe'>Subscribe to Event</a>", currentEventId);
                    //השתמשתי בבדיקת תנאי כדי לוודא שאירוע מאפשר הרשמה רק אם הסטטוס שלו הוא פתוח.
                    //השתמשתי ב-איגנור קייס כדי להבטיח שהשוואת הטקסט תהיה גמישה ותמנע תקלות
                    //במקרה של הבדלים באותיות גדולות או קטנות שנשלחות ממסד הנתונים."
                }
            }

            // יצירת הכרטיס תצוגה

            //הקוד הזה בונה את "תעודת הזהות" של האירוע שהמשתמש רואה על המסך.

            //יוצרת משתנה טקסט ארוך שמכיל את כל מבנה ה-אייץ טי אמ אל של הדף. סימן ה-@ מאפשר
            //לי לכתוב את הקוד על פני כמה שורות כדי שיהיה סדר .

            string html = string.Format(@"
                <div class='event-header'>
                    <div class='event-layout'>
                        <div class='event-poster-section'>
                            <img src='{0}' alt='{1}' />
                        </div>
                        <div class='event-info-section'>
                            <div class='event-movie-title'>{1}</div>
                            <div class='event-host-info'>Hosted by <a href='UserProfile.aspx?username={2}'>@{2}</a></div>
                            <span class='event-status-badge {3}'>{4}</span>
                            
                            <div class='event-details-grid'>
                                <div class='event-detail-card'>
                                    <div class='event-detail-label'>Date</div>
                                    <div class='event-detail-value'><i class='fa fa-calendar'></i>{5}</div>
                                </div>
                                <div class='event-detail-card'>
                                    <div class='event-detail-label'>Time</div>
                                    <div class='event-detail-value'><i class='fa fa-clock-o'></i>{6}</div>
                                </div>
                                <div class='event-detail-card'>
                                    <div class='event-detail-value'><i class='fa fa-film'></i>{7}</div>
                                </div>
                                <div class='event-detail-card'>
                                    <div class='event-detail-label'>Location</div>
                                    <div class='event-detail-value'><i class='fa fa-map-marker'></i>{12}</div>
                                </div>
                                <div class='event-detail-card'>
                                    <div class='event-detail-label'>Price</div>
                                    <div class='event-price-highlight'>{8} ILS</div>
                                </div>
                            </div>

                            <div class='event-detail-card' style='margin-bottom: 20px;'>
                                <div class='event-detail-label'>Director</div>
                                <div class='event-detail-value'>{9}</div>
                            </div>

                            <div class='event-actions'>
                                <a href='Events.aspx' class='btn-action btn-back'>Back to Events</a>
                                <a href='MovieDetails.aspx?movieId={10}' class='btn-action btn-back'>View Movie</a>
                                {11}
                            </div>
                        </div>
                    </div>
                </div>",
                ResolveUrl(poster),
                Server.HtmlEncode(movieTitle),
                Server.HtmlEncode(eventOwner),
                statusClass,
                Server.HtmlEncode(eventStatus),
                Server.HtmlEncode(eventDate),
                Server.HtmlEncode(startTime),
                Server.HtmlEncode(genre),
                price.ToString("F2"),
                Server.HtmlEncode(director),
                movieId,
                subscriptionButton,
                Server.HtmlEncode(location)
            );
            //חשש מכתובת מסוכנת- שומר על הקוד מפני פריצות HtmlEncode

            //יצירת הכרטיס תצוגה פה ולא באספק:
            //אנחנו לא יודעים כמה אירועים יהיו
            //שינוי הנתונים בזמן אמת
            //סדר

            //בחרתי לבנות את ה-אייץ טי אמ אל ב-קוד ביאיינד כדי לשמור על תצוגה דינמית. מכיוון
            //שהדף משתנה משמעותית בהתאם לסוג המשתמש (מארח או אורח) ובהתאם לנתוני האירוע,
            //הרבה יותר נוח ובטוח לנהל את מבנה הדף בתוך ה-סי שארפ מאשר להעמיס על ה-אספק פקדים מוסתרים."

            phEventDetails.Controls.Add(new LiteralControl(html)); //היא לוקחת את כל ה הטמל שנבנה קודם ומציגה אותו בפועל למשתמש.

            string action = Request.QueryString["action"];
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(action))
            {
                if (action == "subscribe" && !isOwner)
                {
                    backendService.SubscribeToEvent(currentEventId, username);
                    Response.Redirect("EventDetails.aspx?eventId=" + currentEventId);
                }
                else if (action == "unsubscribe")
                {
                    backendService.UnsubscribeFromEvent(currentEventId, username);
                    Response.Redirect("EventDetails.aspx?eventId=" + currentEventId);
                }
            }
        }
        catch (Exception ex)
        {
            phEventDetails.Controls.Add(new LiteralControl("<div class='empty-subscribers'>Error loading event: " +
                "" + Server.HtmlEncode(ex.Message) + "</div>"));
        }
    }

    //הפעולה שולפת את רשימת המשתמשים שנרשמו לאירוע ומציגה אותם בתוך גריד
    //של כרטיסים מעוצבים הכוללים את תמונת הפרופיל ושמם המלא עם קישור לדף הפרופיל האישי שלהם.
    private void LoadSubscribers()
    {
        if (!int.TryParse(Request.QueryString["eventId"], out currentEventId) || currentEventId <= 0)
        {
            return;
        }

        try
        {
            DataTable dt = backendService.GetEventSubscribers(currentEventId);
            lblSubscriberCount.Text = dt != null ? dt.Rows.Count.ToString() : "0";

            phSubscribers.Controls.Clear();

            if (dt == null || dt.Rows.Count == 0)
            {
                phSubscribers.Controls.Add(new LiteralControl("<div class='empty-subscribers'>No " +
                    "subscribers yet. Be the first to join!</div>"));
                return;
            }

            string html = "<div class='subscribers-grid'>";

            foreach (DataRow row in dt.Rows)
            {
                string subscriberUsername = row["Username"].ToString();
                string fname = row["FName"] != DBNull.Value ? row["FName"].ToString() : "";
                string lname = row["LName"] != DBNull.Value ? row["LName"].ToString() : "";
                string fullName = (fname + " " + lname).Trim();
                if (string.IsNullOrEmpty(fullName)) fullName = subscriberUsername;

                string pic = row["pic"] != DBNull.Value ? row["pic"].ToString() : "Profile.jpg";
                if (!pic.StartsWith("http") && !pic.StartsWith("/") && !pic.StartsWith("~/"))
                {
                    pic = "~/MyPics/" + pic;
                }

                html += string.Format(@"
                    <div class='subscriber-card'>
                        <a href='UserProfile.aspx?username={0}'>
                            <img src='{1}' alt='{2}' class='subscriber-avatar' />
                        </a>
                        <div class='subscriber-name'>
                            <a href='UserProfile.aspx?username={0}'>{2}</a>
                        </div>
                    </div>",
                    Server.HtmlEncode(subscriberUsername),
                    ResolveUrl(pic),
                    Server.HtmlEncode(fullName)
                );
            }

            html += "</div>";
            phSubscribers.Controls.Add(new LiteralControl(html));
        }
        catch (Exception ex)
        {
            phSubscribers.Controls.Add(new LiteralControl("<div class='empty-subscribers'>Error loading subscribers: " +
                "" + Server.HtmlEncode(ex.Message) + "</div>"));
        }
    }

    //open הפעולה מעדכנת את סטטוס האירוע למצב
    protected void btnSetOpen_Click(object sender, EventArgs e)
    {
        UpdateEventStatus("Open");
    }

    //Closed הפעולה משנה את סטטוס האירוע למצב
    protected void btnSetClosed_Click(object sender, EventArgs e)
    {
        UpdateEventStatus("Closed");
    }

    //Canceled הפעולה משנה את סטטוס האירוע למצב 
    protected void btnSetCanceled_Click(object sender, EventArgs e)
    {
        UpdateEventStatus("Canceled");
    }

    //הפעולה מקבלת סטטוס חדש כטקסט, מעדכנת אותו במסד הנתונים עבור האירוע הנוכחי,
    //ולאחר מכן מרעננת את הדף כדי להציג את השינוי
    private void UpdateEventStatus(string status)
    {
        if (!int.TryParse(Request.QueryString["eventId"], out currentEventId) || currentEventId <= 0)
        {
            return;
        }
        try
        {
            backendService.UpdateEventStatus(currentEventId, status);
            Response.Redirect("EventDetails.aspx?eventId=" + currentEventId);
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Error", 
                "alert('Error updating status: " + ex.Message.Replace("'", "\\'") + "');", true);
        }
    }

    //הפעולה שולפת את פרטי האירוע הנוכחי ממסד הנתונים וממלאת אותם אוטומטית בתוך שדות
    //(Date, Time, Price, Location) הטקסט של חלונית העריכה
    //כדי שהמשתמש יוכל לעדכן אותם
    protected void btnUpdateEvent_Click(object sender, EventArgs e)
    {
        if (!int.TryParse(Request.QueryString["eventId"], out currentEventId) || currentEventId <= 0)
            return;

        DataTable dt = backendService.GetEventById(currentEventId);
        if (dt != null && dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            txtUpdateDate.Text = Convert.ToDateTime(row["EventDate"]).ToString("yyyy-MM-dd");
            txtUpdateTime.Text = row["StartTime"] != DBNull.Value ? 
                Convert.ToDateTime(row["StartTime"].ToString()).ToString("HH:mm") : "";
            txtUpdatePrice.Text = row["Price"] != DBNull.Value ? 
                row["Price"].ToString() : "0";
            txtUpdateLocation.Text = row.Table.Columns.Contains("Location") &&
                row["Location"] != DBNull.Value ? row["Location"].ToString() : "";
            
            pnlUpdateModal.Visible = true;
        }
    }

    //הפעולה מסתירה את חלונית העדכון וסוגרת אותה מהתצוגה של המשתמש
    protected void btnCloseModal_Click(object sender, EventArgs e)
    {
        pnlUpdateModal.Visible = false;
    }

    //הפעולה אוספת את הנתונים החדשים שהמשתמש הזין- תאריך, שעה, מיקום ומחיר,
    //שולחת אותם לעדכון במסד הנתונים עבור האירוע הספציפי,
    //ומעבירה את המשתמש חזרה לדף פרטי האירוע המעודכן
    protected void btnSaveUpdate_Click(object sender, EventArgs e)
    {
        if (!int.TryParse(Request.QueryString["eventId"], out currentEventId) || currentEventId <= 0) return;

        string newDate = txtUpdateDate.Text;
        string newTime = txtUpdateTime.Text;
        string newLocation = txtUpdateLocation.Text;
        decimal newPrice = 0;
        decimal.TryParse(txtUpdatePrice.Text, out newPrice);

        try
        {
            backendService.UpdateEvent(currentEventId, newDate, newTime, newPrice, newLocation);
            Response.Redirect("EventDetails.aspx?eventId=" + currentEventId);
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Error",
                "alert('Error updating event: " + ex.Message.Replace("'", "\\'") + "');", true);
        }
    }
}
