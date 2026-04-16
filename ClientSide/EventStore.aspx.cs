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

    //הפעולה בודקת אם המשתמש מחובר ומחזירה
    //את שם המשתמש שלו מתוך הנתונים שנשמרו בזיכרון
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

    //הפעולה מוודאת שהמשתמש מחובר ושרשום לו מזהה אירוע תקין, בודקת
    //שהוא רשום לאותו אירוע, ואז טוענת את פרטי האירוע והמוצרים הרלוונטיים.
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

    //הפעולה שולפת את פרטי האירוע ממסד הנתונים לפי המספר
    //המזהה שלו ומציגה את שם האירוע בתווית שבדף
    private void LoadEventDetails(int eventId)
    {
        DataTable dt = srv.GetEventById(eventId);
        if (dt != null && dt.Rows.Count > 0)
        {
            lblEventName.Text = dt.Rows[0]["Title"].ToString();
        }
    }

    //הפעולה שולפת את רשימת המוצרים ששייכים לאירוע ספציפי
    //ומציגה אותם בדף, ואם אין מוצרים היא מציגה הודעה שהחנות ריקה כרגע
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

    //הפעולה אוספת את כל המוצרים שהמשתמש בחר לקנות, בודקת
    //שהכמויות תקינות, יוצרת הזמנה חדשה במסד הנתונים ומעבירה את המשתמש
    //לדף "ההזמנות שלי" עם אישור על הצלחת הרכישה
    protected void btnCheckout_Click(object sender, EventArgs e)
    {
        try
        {
            string username = GetLoggedInUsername();
            if (username == null) return;

            int eventId = Convert.ToInt32(Request.QueryString["eventId"]);            
            List<localhost.OrderItem> cart = new List<localhost.OrderItem>();
            
            foreach (RepeaterItem item in rptProducts.Items)
            {
                HiddenField hfProductId = (HiddenField)item.FindControl("hfProductId");
                HiddenField hfPrice = (HiddenField)item.FindControl("hfPrice");
                TextBox txtQty = (TextBox)item.FindControl("txtQty");
                
                int qty = 0;
                int.TryParse(txtQty.Text, out qty);

                //הפיכת בחירה של משתמש באתר לפריט אמיתי בתוך סל הקניות

                if (qty > 0) //הכמות שהמשתמש בחר
                {
                    decimal price = 0;                                //מאפשר לקוד לקבל את המספר בכל מיני צורות
                    decimal.TryParse(hfPrice.Value.Replace(",", "."), System.Globalization.NumberStyles.Any, 
                        System.Globalization.CultureInfo.InvariantCulture, out price); //המספר הסופי יישמר בתוך המשתנה שנקרא מחיר
                        //מבטיח שהקוד שלך יעבוד אותו דבר על כל מחשב בעולם
                    cart.Add(new localhost.OrderItem
                    {
                        ProductId = Convert.ToInt32(hfProductId.Value),
                        Quantity = qty,
                        Price = price
                    }); //מוסיפה פריט חדש לסל הקניות
                }
            }
            if (cart.Count == 0)
            {
                lblMessage.Text = "Please select at least one item to checkout.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }
            int orderId = srv.PlaceOrder(username, eventId, cart.ToArray());       
            if (orderId > 0)
            {
                Response.Redirect("MyOrders.aspx?success=" + orderId);
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
