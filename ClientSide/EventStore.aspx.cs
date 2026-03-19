using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EventStore : System.Web.UI.Page
{
    localhost.Service srv = new localhost.Service();
    
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
        string username = GetLoggedInUsername();
        if (username == null)
        {
            Response.Redirect("Login.aspx");
            return;
        }

        string eventIdStr = Request.QueryString["eventId"];
        if (string.IsNullOrEmpty(eventIdStr))
        {
            Response.Redirect("Events.aspx");
            return;
        }

        int eventId = 0;
        if (!int.TryParse(eventIdStr, out eventId)) 
        {
            Response.Redirect("Events.aspx");
            return;
        }

        // Ensure user is subscribed
        if (!srv.IsUserSubscribed(eventId, username))
        {
            Response.Redirect("EventDetails.aspx?eventId=" + eventId);
            return;
        }

        if (!IsPostBack)
        {
            LoadEventDetails(eventId);
            LoadProducts();
        }
    }

    private void LoadEventDetails(int eventId)
    {
        DataTable dt = srv.GetEventById(eventId);
        if (dt != null && dt.Rows.Count > 0)
        {
            lblEventName.Text = dt.Rows[0]["Title"].ToString();
        }
    }

    private void LoadProducts()
    {
        int eventId = Convert.ToInt32(Request.QueryString["eventId"]);
        DataTable dt = srv.GetEventProducts(eventId);
        rptProducts.DataSource = dt;
        rptProducts.DataBind();
        
        if (dt == null || dt.Rows.Count == 0)
        {
            lblMessage.Text = "No items available in this event's store yet.";
        }
    }

    protected void btnCheckout_Click(object sender, EventArgs e)
    {
        try
        {
            string username = GetLoggedInUsername();
            if (username == null) return;

            int eventId = Convert.ToInt32(Request.QueryString["eventId"]);
            
            List<localhost.OrderItem> cart = new List<localhost.OrderItem>();
            
            // Loop through repeater items to build the cart
            foreach (RepeaterItem item in rptProducts.Items)
            {
                HiddenField hfProductCode = (HiddenField)item.FindControl("hfProductCode");
                HiddenField hfPrice = (HiddenField)item.FindControl("hfPrice");
                TextBox txtQty = (TextBox)item.FindControl("txtQty");
                
                int qty = 0;
                int.TryParse(txtQty.Text, out qty);
                
                if (qty > 0)
                {
                    decimal price = 0;
                    decimal.TryParse(hfPrice.Value.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out price);
                    cart.Add(new localhost.OrderItem
                    {
                        ProductCode = hfProductCode.Value,
                        Quantity = qty,
                        Price = price
                    });
                }
            }

            if (cart.Count == 0)
            {
                lblMessage.Text = "Please select at least one item to checkout.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // Call PlaceOrder expecting an array
            string orderCode = srv.PlaceOrder(username, eventId, cart.ToArray());
            
            if (!string.IsNullOrEmpty(orderCode))
            {
                Response.Redirect("MyOrders.aspx?success=" + orderCode);
            }
            else
            {
                lblMessage.Text = "Could not generate order code.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error placing order: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }
}
