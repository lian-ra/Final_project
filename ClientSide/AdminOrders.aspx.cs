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

        DataTable dt = Session["data"] as DataTable;
        if (dt != null && dt.Rows.Count > 0)
        {
            if (dt.Columns.Contains("User")) return dt.Rows[0]["User"].ToString();
            return dt.Rows[0][0].ToString();
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

        if (!IsPostBack)
        {
            LoadAllOrders();
        }
    }

    //הפעולה מפעילה את טעינת כל ההזמנות מחדש, תוך סינון התוצאות לפי הטקסט שהמנהל הקליד בתיבת החיפוש
    protected void btnSearchOrders_Click(object sender, EventArgs e)
    {
        LoadAllOrders(txtSearch.Text.Trim());
    }

    //הפעולה מנקה את מה שכתוב בתיבת החיפוש ומציגה מחדש את רשימת כל ההזמנות ללא סינון.
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        LoadAllOrders("");
    }

    //הפעולה מציגה למנהל את כל ההזמנות באתר, כולל סינון לפי חיפוש,
    //פירוט מוצרים לכל הזמנה וחישוב סך כל הרווחים וההזמנות שבוצעו
    private void LoadAllOrders(string searchTerm = "")
    {
        DataTable dtOrders = srv.GetAllOrders();
        List<AdminOrder> ordersList = new List<AdminOrder>();
        decimal totalRev = 0;

        if (dtOrders != null && dtOrders.Rows.Count > 0)
        {
            foreach (DataRow row in dtOrders.Rows)
            {
                int orderId = Convert.ToInt32(row["OrderId"]);
                string buyer = row["Username"].ToString();
                string eventOwner = row["EventOwner"].ToString();
                string movieTitle = row["MovieTitle"].ToString();
                
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    string s = searchTerm.ToLower();
                    if (!orderId.ToString().Contains(s) && 
                        !buyer.ToLower().Contains(s) && 
                        !eventOwner.ToLower().Contains(s) && 
                        !movieTitle.ToLower().Contains(s))
                    {
                        continue; //בודק אם מה שהמשתמש חיפש לא נמצא באף אחד מהשדות- הוא פשוט מדלג על השורה הזו.
                    }
                }
                
                AdminOrder order = new AdminOrder();
                order.OrderId = orderId;
                order.Buyer = buyer;
                order.EventOwner = eventOwner;
                order.MovieTitle = movieTitle;
                order.EventDate = Convert.ToDateTime(row["EventDate"]);
                order.DatePurchased = Convert.ToDateTime(row["DatePurchased"]);
                order.Total = Convert.ToDecimal(row["Total"]);
                totalRev += order.Total;
                
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
            rptAllOrders.Visible = false;
            pnlStats.Visible = false;
        }
        else
        {
            lblEmpty.Visible = false;
            rptAllOrders.DataSource = ordersList;
            rptAllOrders.DataBind();
            rptAllOrders.Visible = true;
            
            pnlStats.Visible = true;
            lblTotalOrders.Text = ordersList.Count.ToString();
            lblTotalRevenue.Text = totalRev.ToString("0.00") + " ILS";
        }
    }
}
