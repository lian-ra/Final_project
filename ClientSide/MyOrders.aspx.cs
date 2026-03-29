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

    // Helper class for data binding
    public class StoreOrder
    {
        public string OrderCode { get; set; } //הקוד של ההזמנה
        public DateTime DatePurchased { get; set; } //התאריך שבה בוצעה ההזמנה
        public decimal Total { get; set; } //המחיר הכולל של ההזמנה
        public string ItemsSummary { get; set; } //תיאור הפריטים 
    }

    //הפעולה מוצאת ומחזירה את שם המשתמש המחובר מתוך זיכרון אם הוא עבר אימות  
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (GetLoggedInUsername() == null) //משתמש לא מחובר
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

    //הפעולה
    //מושכת את כל ההזמנות של המשתמש מהמסד והופכת כל שורה בטבלה לאובייקט מסודר ברשימה 
    private void LoadOrders(string searchQuery = "")
    {
        string username = GetLoggedInUsername();
        if (username == null) return;

        DataTable dtOrders = srv.GetMyOrders(username); //טבלת להזמנות
        
        List<StoreOrder> ordersList = new List<StoreOrder>();
        searchQuery = searchQuery.ToLower();

        if (dtOrders != null && dtOrders.Rows.Count > 0)
        {
            foreach (DataRow row in dtOrders.Rows)
            {
                string orderCode = row["OrderCode"].ToString();
                
                StoreOrder order = new StoreOrder(); // אובייקט 
                order.OrderCode = orderCode;
                order.DatePurchased = Convert.ToDateTime(row["DatePurchased"]);
                order.Total = Convert.ToDecimal(row["Total"]);
                
                string movieTitle = row["MovieTitle"].ToString();
                DateTime eventDate = Convert.ToDateTime(row["EventDate"]);
                
                DataTable dtItems = srv.GetOrderItems(orderCode); //רשימת הפרטים שנבחרו בהזמנה ספציפית בטבלה
                List<string> itemStrs = new List<string>(); // רשימת פרטים ספציפית
                if (dtItems != null)
                {
                    foreach(DataRow iRow in dtItems.Rows)
                    {
                        itemStrs.Add(iRow["Quantity"].ToString() + "x " + iRow["ProductName"].ToString());
                    }
                }
                
                string itemsText = string.Join(", ", itemStrs); //שנקנו
                order.ItemsSummary = "<b>Event:</b> " + movieTitle + " (" + eventDate.ToShortDateString() + ") " +
                    "<br/> <b>Items:</b> " + itemsText;
                
                if (!string.IsNullOrWhiteSpace(searchQuery)) //חיפוש
                {
                    bool match = orderCode.ToLower().Contains(searchQuery) ||
                                 movieTitle.ToLower().Contains(searchQuery) ||
                                 itemsText.ToLower().Contains(searchQuery);
                    //עובד גם במקרה והחיפוש באותיות גדולות או באותיות קטנות
                    if (!match) continue;
                }
                ordersList.Add(order);
            }
        }
        
        if (ordersList.Count == 0) //רשימת ההזמנות
        {
            lblEmpty.Text = string.IsNullOrWhiteSpace(searchQuery)
                ? "You haven't placed any orders yet." : "No orders found matching your search.";
            lblEmpty.Visible = true;
            rptOrders.Visible = false; //הצגת טבלת ההזמנות
        }
        else
        {
            lblEmpty.Visible = false;
            rptOrders.DataSource = ordersList;
            rptOrders.DataBind(); //מעבירה את הנתונים של הc# 
                                  //אל התצוגה של הhtml 
            rptOrders.Visible = true;
        }
    }
    protected void btnSearchOrders_Click(object sender, EventArgs e)//חיפוש
    {
        LoadOrders(txtSearchOrders.Text.Trim());
    }
    protected void btnClearSearch_Click(object sender, EventArgs e)//מוחק
    {
        txtSearchOrders.Text = "";
        LoadOrders();
    }
}
