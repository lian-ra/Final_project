using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MyOrders : System.Web.UI.Page
{
    localhost.Service srv = new localhost.Service();

    public class StoreOrder
    {
        public int OrderId { get; set; } //הקוד של ההזמנה
        public DateTime DatePurchased { get; set; } //התאריך שבה בוצעה ההזמנה
        public decimal Total { get; set; } //המחיר הכולל של ההזמנה
        public string ItemsSummary { get; set; } //תיאור הפריטים 
    }

    //הפעולה מחלצת את שם המשתמש מתוך נתוני הזיכרון במידה והוא מחובר למערכת
    private string GetLoggedInUsername()
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
        return null;
    }

    //הפעולה מוודאת שהמשתמש מחובר למערכת
    //ולאחר מכן מציגה הודעת אישור אם בוצעה הזמנה בהצלחה וטוענת את רשימת ההזמנות של המשתמש
    protected void Page_Load(object sender, EventArgs e)
    {
        if (GetLoggedInUsername() == null) 
        {
            Response.Redirect("Login.aspx");
            return;
        }

        if (!IsPostBack)
        {
            if (Request.QueryString["success"] != null)
            {
                lblSuccess.Text = "Order " + Request.QueryString["success"] + " placed successfully!";
                lblSuccess.Visible = true;
            }
            LoadOrders();
        }
    }

    //הפעולה שולפת את כל ההזמנות של המשתמש ממסד הנתונים,
    //והופכת אותן לרשימה מסודרת הכוללת את פרטי האירוע
    //והמוצרים שנרכשו, ומציגה אותן בטבלה בדף
    private void LoadOrders(string searchQuery = "")
    {
        string username = GetLoggedInUsername();
        if (username == null) return;

        DataTable dtOrders = srv.GetMyOrders(username); 
        
        List<StoreOrder> ordersList = new List<StoreOrder>();
        searchQuery = searchQuery.ToLower();

        if (dtOrders != null && dtOrders.Rows.Count > 0)
        {
            foreach (DataRow row in dtOrders.Rows)
            {
                int orderId = Convert.ToInt32(row["OrderId"]);
                
                StoreOrder order = new StoreOrder(); 
                order.OrderId = orderId;
                order.DatePurchased = Convert.ToDateTime(row["DatePurchased"]);
                order.Total = Convert.ToDecimal(row["Total"]);
                
                string movieTitle = row["MovieTitle"].ToString();
                DateTime eventDate = Convert.ToDateTime(row["EventDate"]);
                
                DataTable dtItems = srv.GetOrderItems(orderId); 
                List<string> itemStrs = new List<string>(); 
                if (dtItems != null)
                {
                    foreach(DataRow iRow in dtItems.Rows)
                    {
                        itemStrs.Add(iRow["Quantity"].ToString() + "x "
                            + iRow["ProductName"].ToString());
                    }
                }
                
                string itemsText = string.Join(", ", itemStrs);//אוסף של מחרוזות למחרוזת אחת ארוכה
                order.ItemsSummary = "<b>Event:</b> " + movieTitle + 
                    " (" + eventDate.ToShortDateString() + ") " + //תאריך מסורבל לידידותי
                    "<br/> <b>Items:</b> " + itemsText;
                
                if (!string.IsNullOrWhiteSpace(searchQuery)) 
                {
                    bool match = orderId.ToString().Contains(searchQuery) ||
                                 movieTitle.ToLower().Contains(searchQuery) ||
                                 itemsText.ToLower().Contains(searchQuery);
                    if (!match) 
                        continue; //דילוג על המוצר שלא עומד בתנאי החיפוש- סינון
                }
                ordersList.Add(order);
            }
        }
        
        if (ordersList.Count == 0) 
        {
            lblEmpty.Text = string.IsNullOrWhiteSpace(searchQuery)
                ? "You haven't placed any orders yet." : "No orders found matching your search.";
            lblEmpty.Visible = true;
            rptOrders.Visible = false; 
        }
        else
        {
            lblEmpty.Visible = false;
            rptOrders.DataSource = ordersList;
            rptOrders.DataBind();                            
            rptOrders.Visible = true;
        }
    }

    //הפעולה מפעילה את טעינת ההזמנות מחדש תוך שימוש בטקסט שהמשתמש הקליד
    //בתיבת החיפוש, כדי להציג רק את ההזמנות המתאימות לחיפוש
    protected void btnSearchOrders_Click(object sender, EventArgs e)
    {
        LoadOrders(txtSearchOrders.Text.Trim());
    }

    //הפעולה מרוקנת את תיבת החיפוש וטוענת מחדש את כל רשימת ההזמנות ללא סינון.
    protected void btnClearSearch_Click(object sender, EventArgs e)
    {
        txtSearchOrders.Text = "";
        LoadOrders();
    }
}
