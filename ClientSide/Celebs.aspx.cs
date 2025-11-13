using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Celebs : System.Web.UI.Page
{
    private localhost.Service myService = new localhost.Service();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCelebs();
        }
    }

    protected void btnSearchCelebs_Click(object sender, EventArgs e)
    {
        LoadCelebs();
    }

    private void LoadCelebs()
    {
        try
        {
            string searchText = txtSearchCelebs.Text.Trim();
            string role = ddlRole.SelectedValue;

            DataTable dt = myService.SearchCelebs(searchText, role);

            celebsGrid.Controls.Clear();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    LiteralControl celebCard = new LiteralControl(GenerateCelebCard(row));
                    celebsGrid.Controls.Add(celebCard);
                }
            }
            else
            {
                LiteralControl noResults = new LiteralControl("<div style='text-align: center; color: white; padding: 40px; grid-column: 1 / -1;'>No celebrities found.</div>");
                celebsGrid.Controls.Add(noResults);
            }
        }
        catch (Exception ex)
        {
            string message = "alert('Error loading celebs: " + ex.Message.Replace("'", "\\'") + "');";
            ClientScript.RegisterStartupScript(this.GetType(), "Error", message, true);
            LiteralControl errorMsg = new LiteralControl("<div style='text-align: center; color: white; padding: 40px; grid-column: 1 / -1;'>Error loading celebrities. Please try again.</div>");
            celebsGrid.Controls.Clear();
            celebsGrid.Controls.Add(errorMsg);
        }
    }

    private string GenerateCelebCard(DataRow row)
    {
        string name = row["Name"] != DBNull.Value ? row["Name"].ToString() : "Unknown";
        string role = row["Role"] != DBNull.Value ? row["Role"].ToString() : "";
        string photo = row["Photo"] != DBNull.Value ? row["Photo"].ToString() : "images/uploads/ava1.jpg";
        string bio = row["Bio"] != DBNull.Value ? row["Bio"].ToString() : "";

        if (!photo.StartsWith("http") && !photo.StartsWith("/") && !photo.StartsWith("~/"))
        {
            photo = "~/" + photo;
        }

        string cardHtml = string.Format(@"
            <div class='celeb-card'>
                <img src='{0}' alt='{1}' class='celeb-photo' />
                <div class='celeb-info'>
                    <div class='celeb-name'>{1}</div>
                    <div class='celeb-role'>{2}</div>
                    <div class='celeb-bio'>{3}</div>
                </div>
            </div>",
            ResolveUrl(photo),
            HttpUtility.HtmlEncode(name),
            HttpUtility.HtmlEncode(role),
            HttpUtility.HtmlEncode(bio)
        );

        return cardHtml;
    }
}
