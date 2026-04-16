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

        DataTable dt = Session["data"] as DataTable;
        if (dt != null && dt.Rows.Count > 0)
        {
            if (dt.Columns.Contains("User")) return dt.Rows[0]["User"].ToString();
            return dt.Rows[0][0].ToString();
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

        string eventIdStr = Request.QueryString["eventId"];
        int eventId = 0;
        if (string.IsNullOrEmpty(eventIdStr) || !int.TryParse(eventIdStr, out eventId))
        {
            Response.Redirect("Events.aspx");
            return;
        }

        // Verify Owner
        DataTable dtEvent = srv.GetEventById(eventId);
        if (dtEvent == null || dtEvent.Rows.Count == 0 || !dtEvent.Rows[0]["Username"].ToString().Equals(username, StringComparison.OrdinalIgnoreCase))// התעלמות מהבדל באותיות קטנות או גדולות

        {
            Response.Redirect("EventDetails.aspx?eventId=" + eventId);
            return;
        }

        if (!IsPostBack)
        {
            lblEventName.Text = dtEvent.Rows[0]["Title"].ToString();
            LoadOrders(eventId);
        }
    }

    //הפעולה טוענת את כל ההזמנות של אירוע ספציפי, מחשבת את סך
    //ההכנסות ומציגה את רשימת הפריטים בדף או הודעה במידה ואין הזמנות.
    private void LoadOrders(int eventId)
    {
        DataTable dtOrders = srv.GetEventOrders(eventId);
        List<EventOrderDisplay> ordersList = new List<EventOrderDisplay>();
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
                ordersList.Add(order);
            }
        }
        
        if (ordersList.Count == 0)
        {
            lblEmpty.Visible = true;
            rptOrders.Visible = false;
            pnlStats.Visible = false;
        }
        else
        {
            lblEmpty.Visible = false;
            rptOrders.DataSource = ordersList;
            rptOrders.DataBind();
            rptOrders.Visible = true;
            
            pnlStats.Visible = true;
            lblTotalOrders.Text = ordersList.Count.ToString();
            lblTotalRevenue.Text = totalRev.ToString("0.00") + " ILS";
        }
    }
}
