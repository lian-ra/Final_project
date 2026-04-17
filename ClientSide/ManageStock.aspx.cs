using System;
using System.Data;
using System.IO;
using System.Web.UI;

public partial class ManageStock : System.Web.UI.Page
{
    localhost.Service srv = new localhost.Service();

    //הפעולה בודקת אם המשתמש המחובר הוא מנהל, ואם כן
    //היא מחזירה את שם המשתמש שלו מתוך הנתונים שנשמרו בזיכרון המערכת
    private string GetLoggedInUsername()
    {
        string status = Session["status"] as string;
        if (status != "2") return null; 

        DataTable dt = Session["data"] as DataTable; //ניגשת לטבלה שנשמרה ב-סיזיון-נתונים בזמן הלוגין.
                                                     //הטבלה הזו מכילה את כל הפרטים של המשתמש מהדאטה-בייס
        if (dt != null && dt.Rows.Count > 0)//מוודאת שהטבלה קיימת ושיש בה לפחות שורה אחת של נתונים
        {
            if (dt.Columns.Contains("User"))
                return dt.Rows[0]["User"].ToString();
            return dt.Rows[0][0].ToString();
            //user אם יש בעמודות של הטבלה עמודה שקוראים לה
            //אם כן, אני שולפת את מה שכתוב
            //בשורה הראשונה בעמודה הזו ומחזירה את זה אחרת 
            ////לוקחת את הערך שנמצא בתא הראשון בטבלה (שורה 0, עמודה 0), כי
            // בדרך כלל שם נמצא שם המשתמש או המזהה
        }
        return null;
    }

    //הפעולה בודקת אם המשתמש המחובר הוא מנהל: אם לא היא
    //מעבירה אותו לדף ההתחברות ואם כן היא טוענת עבורו
    //את רשימת מלאי המוצרים כשהדף עולה לראשונה
    protected void Page_Load(object sender, EventArgs e)
    {
        if (GetLoggedInUsername() == null)
        {
            Response.Redirect("Login.aspx");
            return;
        }
        if (!IsPostBack)//כדי להגיד למחשב:תטען את הנתונים מהדאטה-בייס רק כשהדף נפתח בפעם הראשונה
                        //  אם המשתמש לוחץ על כפתור והדף מתרענן, אני
                        //לא רוצה שהמחשב יטען את הכל מחדש, כי זה ימחוק
                        //את מה שהמשתמש כתב או שינה בתיבות הטקסט.
                        //כאן המשתמש רק עכשיו נכנס לדף, הכל נקי וחדש

        {
            LoadStock();
        }
    }

    //הפעולה טוענת את רשימת כל המוצרים מהמלאי ומציגה אותם בדף, תוך
    //אפשרות לסינון התוצאות לפי שם המוצר או הקוד שלו במידה והוזן טקסט בחיפוש
    private void LoadStock(string searchQuery = "")
    {
        DataTable dt = srv.GetProducts(); 
        
        if (!string.IsNullOrWhiteSpace(searchQuery)) 
        {
            DataTable filteredDt = dt.Clone(); //טבלה ריקה משוכפלת
            string escaped = searchQuery.Replace("'", "''"); //השורה הזו מבצעת 'ניקוי' לטקסט שהמשתמש הקליד. אני מחליפה כל
                                                //גרש בודד בשני גרשיים כדי למנוע שגיאות בשאילתת ה-אסקיואל ולהגן על האתר

            DataRow [] results = dt.Select("Name LIKE '%" + escaped +
                "%' OR Convert(ProductId, 'System.String') LIKE '%" + escaped + "%'"); //סינון
            //השורה הזו מבצעת סינון על הטבלה. היא מחפשת את המחרוזת שהמשתמש
            //הקליד גם בעמודת השם וגם בעמודת קוד המוצר (אחרי המרה לטקסט), ומחזירה
            //את כל השורות שמתאימות לחיפוש

            foreach (DataRow row in results)
            {
                filteredDt.ImportRow(row);
                //השורה הזו מעתיקה את השורה שנמצאה בסינון אל תוך הטבלה החדשה, כך
                //שהיא תשמור על המבנה המקורי של הנתונים ותהיה מוכנה לתצוגה
            }
            dt = filteredDt; //כדי להציג רק את התוצאות שנמצאו בחיפוש
        }

        rptStock.DataSource = dt; //מחברת את המידע (הטבלה) לרכיב התצוגה באתר.
        rptStock.DataBind(); //נותנת פקודה למחשב להציג את המידע בפועל על המסך.
    } 

    //הפעולה מפעילה את טעינת המלאי מחדש, תוך סינון
    //המוצרים לפי הטקסט שהמנהל הקליד בתיבת החיפוש
    protected void btnSearchStock_Click(object sender, EventArgs e)
    {
        LoadStock(txtSearchStock.Text.Trim());
    }

    //הפעולה מנקה את טקסט החיפוש ומציגה מחדש
    //את רשימת המלאי המלאה ללא סינון
    protected void btnClearSearch_Click(object sender, EventArgs e)
    {
        txtSearchStock.Text = "";//מרוקנת את תיבת הטקסט של החיפוש. אני שמה
                                 //שם מחרוזת ריקה כדי שהמשתמש יראה שהטקסט שהוא כתב נמחק.

        LoadStock();//אני קוראת שוב לפעולה שטוענת את ההזמנות, אבל
    }               //הפעם אני שולחת לה גרשיים ריקים. זה גורם לה להבין שאין יותר
                    //סינון, והיא פשוט מציגה מחדש את כל ההזמנות של המשתמש

    //הפעולה מוסיפה מוצר חדש למערכת עם הפרטים
    //שהוזנו (שם, תיאור ומחיר) ומעדכנת את תצוגת המלאי
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
                string serverPath = Server.MapPath("~/MyPics/") + filename;//כדי לדעת איפה נמצא הקובץ- תיקייה
                fuPicture.SaveAs(serverPath); //שמירה בתיקייה
                picturePath = "~/MyPics/" + filename; //שמירה בכתובת של קובץ
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

    //הפעולה מנהלת את הכפתורים ברשימת המוצרים: מחיקה(בתנאי שהמוצר לא הוזמן בעבר) או
    //פתיחת טופס עריכה עם פרטי המוצר הקיים.
    protected void rptStock_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
                                                         //מכיל את כל הנתונים על הכפתור שנלחץ בתוך הרשימה
    {
        if (e.CommandName == "Delete")//משתנה ששומר את שם הפעולה שהגדרנו לכפתור
        {
            int productId = Convert.ToInt32(e.CommandArgument); //תעודת זהות של המוצר הספציפי שלחצו עליו.
            try
            {
                srv.DeleteProduct(productId);
                lblSuccess.Text = "Item deleted successfully.";
                lblSuccess.Visible = true;
                lblError.Visible = false;
                LoadStock(txtSearchStock.Text.Trim());
                //היא מפעילה את הפעולה שטוענת את רשימת המלאי, ומתחשבת במה שהמשתמש כתב בתיבת החיפוש.
            }
            catch (Exception)
            {
                lblError.Text = "Cannot delete this stock item because it has been included in past user orders." +
                    " To preserve order receipts permanently, products cannot be deleted if already purchased.";
                lblError.Visible = true;
                lblSuccess.Visible = false;
            }
        }
        else if (e.CommandName == "EditProd")//משתנה ששומר את שם הפעולה שהגדרנו לכפתור
        {
            try 
            {
                //השורה הזו לוקחת את המידע שנשלח מהכפתור ומפרקת אותו
                //למערך של מחרוזות לפי התו המפריד '|', כדי שאוכל להשתמש בכל נתון בנפרד.
                string[] args = e.CommandArgument.ToString().Split('|');
                               //פירוק מחרוזת לרשימה של נתונים נפרדים
                if (args.Length >= 3)
                {
                    hfEditProductId.Value = args[0];
                    txtEditName.Text = args[1];
                    txtEditPrice.Text = args[2];
                    
                    pnlAdd.Visible = false;
                    pnlEdit.Visible = true; //מצב עריכה
                    lblSuccess.Visible = false;
                    lblError.Visible = false;
                }
            } 
            catch { }
        }
    }

    //הפעולה מעדכנת את המחיר והתמונה של מוצר קיים במסד הנתונים, שומרת
    //את התמונה החדשה בשרת (אם הועלתה) ומרעננת את תצוגת המלאי.
    protected void btnSaveEdit_Click(object sender, EventArgs e)
    {
        try
        {
            int productId = Convert.ToInt32(hfEditProductId.Value);
            decimal price = 0;
            decimal.TryParse(txtEditPrice.Text.Replace(",", "."), 
         System.Globalization.NumberStyles.Any, //מקבל גם אם הוסיף בטעות רווחים
         System.Globalization.CultureInfo.InvariantCulture, out price);//כדי למנוע קריסה
                                                             //של האתר בשל הבדלי פורמטים ומחירים בין מדינות

            string picture = "";
            if (fuEditPicture.HasFile)
            {
                string filename = Path.GetFileName(fuEditPicture.FileName);
                string newName = Guid.NewGuid().ToString("N").Substring(0, 8) + "_" + filename;
                //השורה הזו מייצרת שם ייחודי לקובץ על ידי יצירת קוד אקראי וחיבורו
                //לשם המקורי. זה מבטיח שלא יהיו כפילויות בשמות הקבצים בשרת

                string serverPath = Server.MapPath("~/MyPics/") + newName; //כדי לדעת איפה נמצא הקובץ- תיקייה
                fuEditPicture.SaveAs(serverPath); //שמירה בתיקייה
                picture = "~/MyPics/" + newName; //שמירה בכתובת של קובץ
            }
            //.Substring(0, 8): אומר למחשב: "אל תקח את כל הקוד הארוך, קח רק את 8 התווים הראשונים".
            //Guid.NewGuid(): פקודה שמייצרת קוד ייחודי וארוך של אותיות ומספרים
            //(כדי שאף פעם לא יהיו שני קבצים עם אותו שם).

            srv.UpdateProduct(productId, price, picture);

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

    //הפעולה מבטלת את מצב העריכה על ידי הסתרת טופס
    //העריכה והודעות המערכת, והחזרת טופס הוספת המוצר לתצוגה
    protected void btnCancelEdit_Click(object sender, EventArgs e)
    {
        pnlAdd.Visible = true;
        pnlEdit.Visible = false;
        lblSuccess.Visible = false;
        lblError.Visible = false;
    }
}
