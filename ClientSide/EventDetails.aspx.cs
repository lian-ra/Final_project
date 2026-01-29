using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EventDetails : System.Web.UI.Page
{
    private localhost.Service myService = new localhost.Service();
    private int currentEventId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        string status = Session["status"] as string;
        if (status != "1" && status != "2")
        {
            string script = @"alert('You must be logged in to view this page.'); setTimeout(function() {window.location = 'login.aspx';}, 10);";
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script, true);
            return;
        }

        if (!IsPostBack)
        {
            LoadEventDetails();
            LoadSubscribers();
        }
    }

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

    private void LoadEventDetails()
    {
        if (!int.TryParse(Request.QueryString["eventId"], out currentEventId) || currentEventId <= 0)
        {
            phEventDetails.Controls.Add(new LiteralControl("<div class='empty-subscribers'>Invalid event.</div>"));
            return;
        }

        try
        {
            DataTable dt = myService.GetEventById(currentEventId);
            if (dt == null || dt.Rows.Count == 0)
            {
                phEventDetails.Controls.Add(new LiteralControl("<div class='empty-subscribers'>Event not found.</div>"));
                return;
            }

            DataRow row = dt.Rows[0];
            string username = GetLoggedInUsername();
            string eventOwner = row["Username"].ToString();
            string movieTitle = row["Title"].ToString();
            string poster = row["Poster"] != DBNull.Value ? row["Poster"].ToString() : "images/uploads/slider1.jpg";
            string eventDate = Convert.ToDateTime(row["EventDate"]).ToString("MMMM dd, yyyy");
            string startTime = row["StartTime"] != DBNull.Value ? Convert.ToDateTime(row["StartTime"].ToString()).ToString("HH:mm") : "";
            decimal price = row["Price"] != DBNull.Value ? Convert.ToDecimal(row["Price"]) : 0;
            string eventStatus = row["Status"].ToString();
            string genre = row["Genre"] != DBNull.Value ? row["Genre"].ToString() : "";
            string director = row["Director"] != DBNull.Value ? row["Director"].ToString() : "";
            string actors = row["Actors"] != DBNull.Value ? row["Actors"].ToString() : "";
            int movieId = Convert.ToInt32(row["MovieId"]);

            if (!poster.StartsWith("http") && !poster.StartsWith("/") && !poster.StartsWith("~/"))
            {
                poster = "~/" + poster;
            }

            string statusClass = "status-" + eventStatus.ToLower();

            // Check if user is subscribed
            bool isSubscribed = !string.IsNullOrEmpty(username) && myService.IsUserSubscribed(currentEventId, username);
            bool isOwner = !string.IsNullOrEmpty(username) && username.Equals(eventOwner, StringComparison.OrdinalIgnoreCase);

            // Show owner controls if user is the event owner
            if (isOwner)
            {
                pnlOwnerControls.Visible = true;
            }

            // Build subscription button
            string subscriptionButton = "";
            if (!string.IsNullOrEmpty(username) && !isOwner)
            {
                if (isSubscribed)
                {
                    subscriptionButton = string.Format("<a href='EventDetails.aspx?eventId={0}&action=unsubscribe' class='btn-action btn-unsubscribe'>Unsubscribe</a>", currentEventId);
                }
                else if (eventStatus.Equals("Open", StringComparison.OrdinalIgnoreCase))
                {
                    subscriptionButton = string.Format("<a href='EventDetails.aspx?eventId={0}&action=subscribe' class='btn-action btn-subscribe'>Subscribe to Event</a>", currentEventId);
                }
            }

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
                                    <div class='event-detail-label'>Genre</div>
                                    <div class='event-detail-value'><i class='fa fa-film'></i>{7}</div>
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
                subscriptionButton
            );

            phEventDetails.Controls.Add(new LiteralControl(html));

            // Handle subscription actions
            string action = Request.QueryString["action"];
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(action))
            {
                if (action == "subscribe" && !isOwner)
                {
                    myService.SubscribeToEvent(currentEventId, username);
                    Response.Redirect("EventDetails.aspx?eventId=" + currentEventId);
                }
                else if (action == "unsubscribe")
                {
                    myService.UnsubscribeFromEvent(currentEventId, username);
                    Response.Redirect("EventDetails.aspx?eventId=" + currentEventId);
                }
            }
        }
        catch (Exception ex)
        {
            phEventDetails.Controls.Add(new LiteralControl("<div class='empty-subscribers'>Error loading event: " + Server.HtmlEncode(ex.Message) + "</div>"));
        }
    }

    private void LoadSubscribers()
    {
        if (!int.TryParse(Request.QueryString["eventId"], out currentEventId) || currentEventId <= 0)
        {
            return;
        }

        try
        {
            DataTable dt = myService.GetEventSubscribers(currentEventId);
            lblSubscriberCount.Text = dt != null ? dt.Rows.Count.ToString() : "0";

            phSubscribers.Controls.Clear();

            if (dt == null || dt.Rows.Count == 0)
            {
                phSubscribers.Controls.Add(new LiteralControl("<div class='empty-subscribers'>No subscribers yet. Be the first to join!</div>"));
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
            phSubscribers.Controls.Add(new LiteralControl("<div class='empty-subscribers'>Error loading subscribers: " + Server.HtmlEncode(ex.Message) + "</div>"));
        }
    }

    protected void btnSetOpen_Click(object sender, EventArgs e)
    {
        UpdateEventStatus("Open");
    }

    protected void btnSetClosed_Click(object sender, EventArgs e)
    {
        UpdateEventStatus("Closed");
    }

    protected void btnSetCanceled_Click(object sender, EventArgs e)
    {
        UpdateEventStatus("Canceled");
    }

    private void UpdateEventStatus(string status)
    {
        if (!int.TryParse(Request.QueryString["eventId"], out currentEventId) || currentEventId <= 0)
        {
            return;
        }

        try
        {
            myService.UpdateEventStatus(currentEventId, status);
            Response.Redirect("EventDetails.aspx?eventId=" + currentEventId);
        }
        catch (Exception ex)
        {
            // Handle error
            ClientScript.RegisterStartupScript(this.GetType(), "Error", "alert('Error updating status: " + ex.Message.Replace("'", "\\'") + "');", true);
        }
    }
}
