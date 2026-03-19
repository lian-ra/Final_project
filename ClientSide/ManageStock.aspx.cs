using System;
using System.Data;
using System.IO;
using System.Web.UI;

public partial class ManageStock : System.Web.UI.Page
{
    localhost.Service srv = new localhost.Service();

    private string GetLoggedInUsername()
    {
        string status = Session["status"] as string;
        if (status != "2") return null; // Admin only

        DataTable dt = Session["data"] as DataTable;
        if (dt != null && dt.Rows.Count > 0)
        {
            if (dt.Columns.Contains("User")) return dt.Rows[0]["User"].ToString();
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
            LoadStock();
        }
    }

    private void LoadStock(string searchQuery = "")
    {
        DataTable dt = srv.GetProducts(); // We use the existing GetProducts to list all active stock!
        
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            DataTable filteredDt = dt.Clone();
            string escaped = searchQuery.Replace("'", "''");
            DataRow[] results = dt.Select("Name LIKE '%" + escaped + "%' OR ProductCode LIKE '%" + escaped + "%'");
            foreach (DataRow row in results)
            {
                filteredDt.ImportRow(row);
            }
            dt = filteredDt;
        }

        rptStock.DataSource = dt;
        rptStock.DataBind();
    }

    protected void btnSearchStock_Click(object sender, EventArgs e)
    {
        LoadStock(txtSearchStock.Text.Trim());
    }

    protected void btnClearSearch_Click(object sender, EventArgs e)
    {
        txtSearchStock.Text = "";
        LoadStock();
    }

    protected void btnAddProduct_Click(object sender, EventArgs e)
    {
        string name = txtName.Text.Trim();
        string desc = txtDescription.Text.Trim();
        decimal price = 0;
        decimal.TryParse(txtPrice.Text, out price);

        string picturePath = "images/default-product.png";
        
        if (fuPicture.HasFile)
        {
            try
            {
                string filename = Path.GetFileName(fuPicture.FileName);
                string serverPath = Server.MapPath("~/MyPics/") + filename;
                fuPicture.SaveAs(serverPath);
                picturePath = "~/MyPics/" + filename;
            }
            catch { }
        }

        try
        {
            srv.AddProduct(name, price, desc, picturePath);
            lblSuccess.Text = "Product added successfully!";
            lblSuccess.Visible = true;
            
            // clear form
            txtName.Text = "";
            txtPrice.Text = "";
            txtDescription.Text = "";
            
            LoadStock(txtSearchStock.Text.Trim());
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: " + ex.Message.Replace("'", "\\'") + "');", true);
        }
    }

    protected void rptStock_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            string productCode = e.CommandArgument.ToString();
            try
            {
                srv.DeleteProduct(productCode);
                lblSuccess.Text = "Item deleted successfully.";
                lblSuccess.Visible = true;
                lblError.Visible = false;
                LoadStock(txtSearchStock.Text.Trim());
            }
            catch (Exception)
            {
                lblError.Text = "Cannot delete this stock item because it has been included in past user orders. To preserve order receipts permanently, products cannot be deleted if already purchased.";
                lblError.Visible = true;
                lblSuccess.Visible = false;
            }
        }
        else if (e.CommandName == "EditProd")
        {
            try 
            {
                string[] args = e.CommandArgument.ToString().Split('|');
                if (args.Length >= 3)
                {
                    hfEditProductCode.Value = args[0];
                    txtEditName.Text = args[1];
                    txtEditPrice.Text = args[2];
                    
                    pnlAdd.Visible = false;
                    pnlEdit.Visible = true;
                    lblSuccess.Visible = false;
                    lblError.Visible = false;
                }
            } 
            catch { }
        }
    }

    protected void btnSaveEdit_Click(object sender, EventArgs e)
    {
        try
        {
            string productCode = hfEditProductCode.Value;
            decimal price = 0;
            decimal.TryParse(txtEditPrice.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out price);
            
            string picture = "";
            if (fuEditPicture.HasFile)
            {
                string filename = Path.GetFileName(fuEditPicture.FileName);
                string newName = Guid.NewGuid().ToString("N").Substring(0, 8) + "_" + filename;
                string serverPath = Server.MapPath("~/MyPics/") + newName;
                fuEditPicture.SaveAs(serverPath);
                picture = "~/MyPics/" + newName;
            }

            srv.UpdateProduct(productCode, price, picture);

            lblSuccess.Text = "Product updated successfully.";
            lblSuccess.Visible = true;
            lblError.Visible = false;

            pnlAdd.Visible = true;
            pnlEdit.Visible = false;
            LoadStock(txtSearchStock.Text.Trim());
        }
        catch (Exception ex)
        {
            lblError.Text = "Error updating product: " + ex.Message;
            lblError.Visible = true;
            lblSuccess.Visible = false;
        }
    }

    protected void btnCancelEdit_Click(object sender, EventArgs e)
    {
        pnlAdd.Visible = true;
        pnlEdit.Visible = false;
        lblSuccess.Visible = false;
        lblError.Visible = false;
    }
}
