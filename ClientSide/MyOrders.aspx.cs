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
        public string OrderCode { get; set; }
        public DateTime DatePurchased { get; set; }
        public decimal Total { get; set; }
        public string ItemsSummary { get; set; }
    }

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
                string orderCode = row["OrderCode"].ToString();
                
                StoreOrder order = new StoreOrder();
                order.OrderCode = orderCode;
                order.DatePurchased = Convert.ToDateTime(row["DatePurchased"]);
                order.Total = Convert.ToDecimal(row["Total"]);
                
                string movieTitle = row["MovieTitle"].ToString();
                DateTime eventDate = Convert.ToDateTime(row["EventDate"]);
                
                DataTable dtItems = srv.GetOrderItems(orderCode);
                List<string> itemStrs = new List<string>();
                if (dtItems != null)
                {
                    foreach(DataRow iRow in dtItems.Rows)
                    {
                        itemStrs.Add(iRow["Quantity"].ToString() + "x " + iRow["ProductName"].ToString());
                    }
                }
                
                string itemsText = string.Join(", ", itemStrs);
                order.ItemsSummary = "<b>Event:</b> " + movieTitle + " (" + eventDate.ToShortDateString() + ") <br/> <b>Items:</b> " + itemsText;
                
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    bool match = orderCode.ToLower().Contains(searchQuery) ||
                                 movieTitle.ToLower().Contains(searchQuery) ||
                                 itemsText.ToLower().Contains(searchQuery);
                    if (!match) continue;
                }

                ordersList.Add(order);
            }
        }
        
        if (ordersList.Count == 0)
        {
            lblEmpty.Text = string.IsNullOrWhiteSpace(searchQuery) ? "You haven't placed any orders yet." : "No orders found matching your search.";
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

    protected void btnSearchOrders_Click(object sender, EventArgs e)
    {
        LoadOrders(txtSearchOrders.Text.Trim());
    }

    protected void btnClearSearch_Click(object sender, EventArgs e)
    {
        txtSearchOrders.Text = "";
        LoadOrders();
    }
}
