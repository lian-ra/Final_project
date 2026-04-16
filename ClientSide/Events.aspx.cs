using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Events : System.Web.UI.Page
{
    private localhost.Service backendService = new localhost.Service();

    //הפעולה מוודאת שהמשתמש מחובר, חוסמת גישה למי שלא,
    //ומגדירה ערכי מינימום לתאריך (מהיום והלאה) ולמחיר כדי למנוע טעויות בהזנת אירועים חדשים
    protected void Page_Load(object sender, EventArgs e)
    {
        string status = Session["status"] as string;
        if (status != "1" && status != "2")
        {
            string script = @"alert('You must be logged in to view this page.'); 
            setTimeout(function() {window.location = 'login.aspx';}, 10);";
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script, true);
            return;
        }

        if (!IsPostBack)
        {
            //HTMLהוספת הגבלות ישירות על פקדי ה
            txtEventDate.Attributes["min"] = DateTime.Today.ToString("yyyy-MM-dd");
            txtPrice.Attributes["min"] = "0";
            LoadMoviesDropdown();
            LoadEvents();
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

            DataTable dt = Session["data"] as DataTable;
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Columns.Contains("User"))
                    return dt.Rows[0]["User"].ToString();
                return dt.Rows[0][0].ToString();
            }
        }
        catch { }
        return null;
    }

    //הפעולה שולפת את רשימת כל הסרטים ממסד הנתונים וממלאת איתם תפריט בחירה
    //כך שכל אפשרות מציגה את שם הסרט ושנת היציאה שלו
    private void LoadMoviesDropdown()
    {
        try
        {
            DataTable dt = backendService.GetAllMovies();
            ddlMovie.Items.Clear();
            ddlMovie.Items.Add(new ListItem("-- Select a Movie --", "0"));

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    int movieId = Convert.ToInt32(row["MovieId"]);
                    string title = row["Title"].ToString();
                    string year = row["Year"] != DBNull.Value ? row["Year"].ToString() : "";
                    string displayText = title + (string.IsNullOrEmpty(year) ? "" : " (" + year + ")");//עיצוב טקסט
                    ddlMovie.Items.Add(new ListItem(displayText, movieId.ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            lblModalMessage.Text = "Error loading movies: " + ex.Message;
            lblModalMessage.Visible = true;
        }
    }

    //הפעולה שולפת אירועים ממסד הנתונים לפי סינון שנבחר כמו אירועים שלי או חיפוש,
    //בונה עבורם כרטיסי תצוגה מעוצבים עם כל הפרטים (תמונה, תאריך ומחיר), ומציגה אותם בדף.
    private void LoadEvents()
    {
        try
        {
            backendService.CreateEventsTable();
            string filter = Request.QueryString["filter"];
            string username = GetLoggedInUsername();
            DataTable dt = null;

            if (filter == "upcoming")
            {
                dt = backendService.GetUpcomingEvents();
            }
            else if (filter == "myevents" && !string.IsNullOrEmpty(username))
            {
                dt = backendService.GetEventsByUser(username);
            }
            else if (filter == "subscribed" && !string.IsNullOrEmpty(username))
            {
                dt = backendService.GetUserSubscriptions(username);
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["search"]))
            {
                string location = Request.QueryString["search"];
                dt = backendService.GetEventsByLocation(location);
                txtSearchLocation.Text = location;
            }
            else
            {
                dt = backendService.GetAllEvents();
            }

            phEvents.Controls.Clear(); //מרוקן את כל התוכן שנמצא בתוך המיקום בטבלה
                                       //מונע כפילויות על המסך והיפטרות מפקדים קודמים

            if (dt == null || dt.Rows.Count == 0)
            {
                lblEmpty.Visible = true;
                return;
            }

            lblEmpty.Visible = false;

            string html = "";

            foreach (DataRow row in dt.Rows)
            {
                int eventId = Convert.ToInt32(row["EventId"]);
                string eventUsername = row["Username"].ToString();
                string movieTitle = row["Title"].ToString();
                string poster = row["Poster"] != DBNull.Value ? row["Poster"].ToString() : "images/uploads/slider1.jpg";
                string eventDate = Convert.ToDateTime(row["EventDate"]).ToString("MMM dd, yyyy");
                string startTime = row["StartTime"] != DBNull.Value ? Convert.ToDateTime(row["StartTime"].ToString()).ToString("HH:mm") : "";
                decimal price = row["Price"] != DBNull.Value ? Convert.ToDecimal(row["Price"]) : 0;
                string eventStatus = row["Status"].ToString();
                string genre = row["Genre"] != DBNull.Value ? row["Genre"].ToString() : "";

                if (!poster.StartsWith("http") && !poster.StartsWith("/") && !poster.StartsWith("~/"))
                {
                    poster = "~/" + poster;
                }

                // Get subscriber count
                DataTable subscribers = backendService.GetEventSubscribers(eventId);
                int subscriberCount = subscribers != null ? subscribers.Rows.Count : 0;

                string statusClass = "status-" + eventStatus.ToLower();

                // יצירת הכרטיס תצוגה
                html += string.Format(@"
                    <div class='event-card'>
                        <div style='position: relative;'>
                            <img src='{0}' alt='{1}' class='event-poster' />
                            <span class='event-status-badge {2}'>{3}</span>
                        </div>
                        <div class='event-content'>
                            <div class='event-movie-title'>{1}</div>
                            <div class='event-host'>Hosted by <a href='UserProfile.aspx?username={4}'>@{4}</a></div>
                            <div class='event-details'>
                                <div class='event-detail-row'>
                                    <i class='fa fa-calendar'></i>
                                    <span>{5}</span>
                                </div>
                                <div class='event-detail-row'>
                                    <i class='fa fa-clock-o'></i>
                                    <span>{6}</span>
                                </div>
                                <div class='event-detail-row'>
                                    <i class='fa fa-film'></i>
                                    <span>{7}</span>
                                </div>
                                <div class='event-detail-row'>
                                    <i class='fa fa-map-marker'></i>
                                    <span>{11}</span>
                                </div>
                                <div class='event-detail-row'>
                                    <i class='fa fa-users'></i>
                                    <span>{8} subscribers</span>
                                </div>
                            </div>
                            <div class='event-price'>{9} ILS</div>
                            <div class='event-actions'>
                                <a href='EventDetails.aspx?eventId={10}' class='btn-view-event'>View Details</a>
                            </div>
                        </div>
                    </div>",
                    ResolveUrl(poster),
                    Server.HtmlEncode(movieTitle), 
                    statusClass,
                    Server.HtmlEncode(eventStatus),
                    Server.HtmlEncode(eventUsername),
                    Server.HtmlEncode(eventDate),
                    Server.HtmlEncode(startTime),
                    Server.HtmlEncode(genre),
                    subscriberCount,
                    price.ToString("F2"),
                    eventId,
                    row.Table.Columns.Contains("Location") && row["Location"] != DBNull.Value ? Server.HtmlEncode(row["Location"].ToString()) : "TBD" //יקבע בהמשך
                                                                                      //חשש מכתובת מסוכנת- שומר על הקוד מפני פריצות
                );
            }

            phEvents.Controls.Add(new LiteralControl(html));
        }
        catch (Exception ex)
        {
            phEvents.Controls.Add(new LiteralControl("<div class='empty-state'>Error loading events: " + Server.HtmlEncode(ex.Message) + "</div>"));
                                                                                                         
                                                                                                           
        }
    }


    //פעולה שמציגה למשתמש את הטופס למילוי פרטי האירוע
    //על ידי הפעלת סקריפט שפותח את חלון המודל
    protected void btnCreateEvent_Click(object sender, EventArgs e)
    {
        pnlCreateModal.Visible = true;
        ClientScript.RegisterStartupScript(this.GetType(), "ShowModal", "showModal();", true);
    }

    //הפעולה סוגרת את חלונית יצירת האירוע
    //ומסתירה את הודעת העדכון שבתוכה
    protected void btnCancelModal_Click(object sender, EventArgs e)
    {
        pnlCreateModal.Visible = false;
        lblModalMessage.Visible = false;
    }


    //הפעולה בודקת שכל פרטי האירוע (משתמש, סרט, תאריך, שעה ומחיר) תקינים ומלאים.
    //אם הכל בסדר, היא שומרת את האירוע החדש במסד הנתונים
    //ומעבירה את המשתמש לדף הפרטים של האירוע שנוצר
    protected void btnSubmitEvent_Click(object sender, EventArgs e)
    {
        try
        {
            string username = GetLoggedInUsername();
            if (string.IsNullOrEmpty(username))
            {
                lblModalMessage.Text = "You must be logged in to create an event.";
                lblModalMessage.Visible = true;
                return;
            }

            int movieId = Convert.ToInt32(ddlMovie.SelectedValue);
            if (movieId <= 0)
            {
                lblModalMessage.Text = "Please select a movie.";
                lblModalMessage.Visible = true;
                return;
            }

            if (string.IsNullOrEmpty(txtEventDate.Text))
            {
                lblModalMessage.Text = "Please select an event date.";
                lblModalMessage.Visible = true;
                return;
            }
            DateTime selectedEventDate; //שומר תאריך ושעה
            if (DateTime.TryParse(txtEventDate.Text, out selectedEventDate))
            {
                if (selectedEventDate.Date < DateTime.Today)
                {
                    lblModalMessage.Text = "Event date cannot be in the past.";
                    lblModalMessage.Visible = true;
                    return;
                }
            }

            if (string.IsNullOrEmpty(txtStartTime.Text))
            {
                lblModalMessage.Text = "Please select a start time.";
                lblModalMessage.Visible = true;
                return;
            }

            DateTime eventDate = DateTime.Parse(txtEventDate.Text);
            if (eventDate < DateTime.Today)
            {
                lblModalMessage.Text = "Event date cannot be in the past.";
                lblModalMessage.Visible = true;
                return;
            }

            decimal price = 0;
            if (!string.IsNullOrEmpty(txtPrice.Text))
            {
                if (!decimal.TryParse(txtPrice.Text, out price) || price < 0)
                {
                    lblModalMessage.Text = "Please enter a valid price.";
                    lblModalMessage.Visible = true;
                    return;
                }
            }

            string status = ddlStatus.SelectedValue;
            string location = txtLocation.Text;

            int eventId = backendService.CreateEvent(username, movieId, txtEventDate.Text, txtStartTime.Text, price, location, status);

            if (eventId > 0) //אם יש אירועים
            {
                Response.Redirect("EventDetails.aspx?eventId=" + eventId);
            }
            else
            {
                lblModalMessage.Text = "Failed to create event.";
                lblModalMessage.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblModalMessage.Visible = true;
            lblModalMessage.Text = "Error: " + ex.Message;
        }
    }

    //הפעולה בודקת אם הוכנס טקסט בתיבת החיפוש. אם כן,
    //היא מעבירה את המשתמש לדף האירועים עם מילת החיפוש- המיקום,
    //ואם התיבה ריקה, היא פשוט מרעננת את דף האירועים ומציגה את כולם
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string location = txtSearchLocation.Text.Trim();
        if (!string.IsNullOrEmpty(location))
        {
            Response.Redirect("Events.aspx?search=" + Server.UrlEncode(location));
                       //פקודה הממירה תווים מיוחדים ורווחים לפורמט תקין של כתובת כדי שהקישור בדפדפן לא יישבר.
        }
        else
        {
            Response.Redirect("Events.aspx");
        }
    }
}
