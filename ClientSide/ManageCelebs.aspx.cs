using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class ManageCelebs : System.Web.UI.Page
{
    private localhost.Service backendService = new localhost.Service();

    //הפעולה מוודאת הרשאות מנהל בכניסה לדף, מפנה משתמשים לא מורשים
    //לדף ההתחברות, וטוענת את טבלת הסלבים בטעינה הראשונה
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["status"] == null || !Session["status"].ToString().Equals("2"))
        {
            string script = @"alert('You are not welcome!'); 
            setTimeout(function() {window.location = 'login.aspx';}, 10);";
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script, true);
            return;
        }

        if (!IsPostBack)//כדי להגיד למחשב:תטען את הנתונים מהדאטה-בייס רק כשהדף נפתח בפעם הראשונה
                        //  אם המשתמש לוחץ על כפתור והדף מתרענן, אני
                        //לא רוצה שהמחשב יטען את הכל מחדש, כי זה ימחוק
                        //את מה שהמשתמש כתב או שינה בתיבות הטקסט.
                        //כאן המשתמש רק עכשיו נכנס לדף, הכל נקי וחדש

        {
            BindCelebsGrid();
        }
    }

    //הפעולה אוספת את נתוני הסלב מהטופס, מוודאת שהוזן שם, מוסיפה אותו
    //למסד הנתונים דרך השירות, ומעדכנת את התצוגה עם הודעה.
    protected void btnAddCeleb_Click(object sender, EventArgs e)
    {
        try
        {
            string name = txtCelebName.Text.Trim();
            string role = ddlCelebRole.SelectedValue;
            string photo = txtPhotoUrl.Text.Trim();
            string bio = txtBio.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                ShowMessage("Name is required.", false);
                return;
            }

            localhost.Celeb celeb = new localhost.Celeb();
            celeb.CelebId = 0;
            celeb.Name = name;
            celeb.Role = role;
            celeb.Photo = string.IsNullOrEmpty(photo) ? "images/uploads/ava1.jpg" : photo;
            celeb.Bio = bio;

            backendService.AddCeleb(celeb);

            ShowMessage("Celebrity added successfully.", true);
            ClearForm();
            BindCelebsGrid();
        }
        catch (Exception ex)
        {
            ShowMessage("Error adding celebrity: " + ex.Message, false);
        }
    }

    //הפעולה מנקה את הטפסים בדף ומסתירה את הודעת העדכון/ניהול
    //כדי לאפס את מצב הדף
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearForm();
        lblManageMessage.Visible = false;
    }

    //הפעולה מפעילה מחדש את טעינת רשימת הסלבים ומעדכנת את הטבלה בהתאם לסינון החיפוש
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindCelebsGrid();
    }

    //הפעולה מנקה את כל שדות הטופס (תיבות טקסט ובחירה) ומחזירה אותם למצבם הריק או לברירת המחדל.
    private void ClearForm()
    {
        txtCelebName.Text = "";
        ddlCelebRole.SelectedIndex = 0;
        txtPhotoUrl.Text = "";
        txtBio.Text = "";
    }

    //הפעולה מציגה הודעה למשתמש, קובעת את הטקסט שלה ומשנה את העיצוב, צבע/סגנון
    //בהתאם לשאלה אם הפעולה הצליחה או נכשלה
    private void ShowMessage(string message, bool isSuccess)
    {
        lblManageMessage.Text = message;
        lblManageMessage.Visible = true;
        lblManageMessage.CssClass = isSuccess ? "message-label message-success" : "message-label message-error";
    }

    //הפעולה שולפת את רשימת הסלבים (לפי חיפוש או את כולם), מקשרת
    //אותם לטבלה שמוצגת בדף, ומציגה הודעת שגיאה אם הטעינה נכשלה.
    private void BindCelebsGrid()
    {
        try
        {
            string searchTerm = txtSearch.Text.Trim();
            DataTable dt;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                dt = backendService.SearchCelebs(searchTerm, "all");
            }
            else
            {
                dt = backendService.GetAllCelebs();
            }

            grdCelebs.DataSource = dt;
            grdCelebs.DataBind();
        }
        catch (Exception ex)
        {
            ShowMessage("Error loading celebrities list: " + ex.Message, false);
        }
    }

    //הפעולה מעבירה את השורה שנבחרה למצב עריכה בטבלה
    //וטוענת את הנתונים מחדש כדי להציג את השדות לעריכה
    protected void grdCelebs_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdCelebs.EditIndex = e.NewEditIndex;
        BindCelebsGrid();
    }

    //הפעולה מבטלת את מצב העריכה בטבלה ומחזירה את השורה לתצוגה רגילה מבלי לשמור שינויים
    protected void grdCelebs_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdCelebs.EditIndex = -1;
        BindCelebsGrid();
    }

    //הפעולה שולפת את הנתונים המעודכנים שנכתבו בשורה, שולחת אותם
    //לעדכון במסד הנתונים, ומחזירה את הטבלה לתצוגה רגילה עם הודעת הצלחה
    protected void grdCelebs_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            int celebId = (int)grdCelebs.DataKeys[e.RowIndex].Value;
            GridViewRow row = grdCelebs.Rows[e.RowIndex];

            string name = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();
            string role = ((TextBox)row.Cells[2].Controls[0]).Text.Trim();
            string photo = ((TextBox)row.Cells[3].Controls[0]).Text.Trim();
            string bio = ((TextBox)row.Cells[4].Controls[0]).Text.Trim();

            localhost.Celeb celeb = new localhost.Celeb();
            celeb.CelebId = celebId;
            celeb.Name = name;
            celeb.Role = role;
            celeb.Photo = photo;
            celeb.Bio = bio;

            backendService.UpdateCeleb(celeb);

            grdCelebs.EditIndex = -1;
            BindCelebsGrid();
            ShowMessage("Celebrity updated successfully.", true);
        }
        catch (Exception ex)
        {
            ShowMessage("Error updating celebrity: " + ex.Message, false);
        }
    }

    //הפעולה מזהה את הקוד הייחודי של הסלב בשורה שנבחרה, מוחקת אותו
    //ממסד הנתונים דרך השירות, ומעדכנת את הטבלה עם הודעה שהמחיקה הצליחה
    protected void grdCelebs_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int celebId = (int)grdCelebs.DataKeys[e.RowIndex].Value;
            backendService.DeleteCeleb(celebId);
            BindCelebsGrid();
            ShowMessage("Celebrity deleted successfully.", true);
        }
        catch (Exception ex)
        {
            ShowMessage("Error deleting celebrity: " + ex.Message, false);
        }
    }
}
