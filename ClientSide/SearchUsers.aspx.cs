using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class SearchUsers : System.Web.UI.Page
{
    private localhost.Service backendService = new localhost.Service();

    //הפעולה בודקת האם המשתמש המחובר הוא מנהל
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["status"] == null || !Session["status"].ToString().Equals("2"))
        {
            string script = @"alert('You are not welcome!'); setTimeout(function() {window.location = 'Login.aspx';}, 10);";
            ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", script, true);
            return; //לדף ההתחברות
        }

        if (!IsPostBack)//כדי להגיד למחשב:תטען את הנתונים מהדאטה-בייס רק כשהדף נפתח בפעם הראשונה
                        //  אם המשתמש לוחץ על כפתור והדף מתרענן, אני
                        //לא רוצה שהמחשב יטען את הכל מחדש, כי זה ימחוק
                        //את מה שהמשתמש כתב או שינה בתיבות הטקסט.
                        //כאן המשתמש רק עכשיו נכנס לדף, הכל נקי וחדש

        {
            LoadAllUsers();
        }
    }

    //הפעולה מבצעת חיפוש של משתמשים לפי הטקסט והסינון שהוזנו, ומציגה את התוצאות בתוך טבלה
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        string searchText = TxtSearch.Text.Trim();
        string searchOption = DrpSearch.SelectedValue;

        if (string.IsNullOrEmpty(searchText))
        {
            LoadAllUsers();
            return;
        }

        try
        {
            DataTable dt = backendService.SearchUser(searchText, searchOption);
            GrdUsers.DataSource = dt;
            GrdUsers.DataBind();
            pnlModal.Visible = false;
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    //הפעולה מאפסת את שדות החיפוש והסינון, סוגרת חלונות קופצים
    //וטוענת מחדש את רשימת כל המשתמשים כדי להחזיר את התצוגה למצבה הרגיל.
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        TxtSearch.Text = "";
        DrpSearch.SelectedIndex = 0;
        LoadAllUsers();
        pnlModal.Visible = false;
        GrdUsers.SelectedIndex = -1;
    }

    //הפעולה פונה לשרת כדי לקבל את רשימת כל המשתמשים ללא סינון
    //Grid ומחברת את הנתונים לטבלה
    //כדי להציג אותם בדף המנהל.
    private void LoadAllUsers()
    {
        try
        {
            DataTable dt = backendService.SearchUser("", "");
            GrdUsers.DataSource = dt;
            GrdUsers.DataBind();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    //הפעולה מזהה בחירה של משתמש מהטבלה, שולפת את פרטיו האישיים
    //ממסד הנתונים וממלאת אותם בשדות הטקסט בתוך החלונית שקופצת כדי שיהיה אפשר לערוך אותם
    protected void GrdUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (GrdUsers.SelectedRow != null)
        {
            string username = GrdUsers.DataKeys[GrdUsers.SelectedRow.RowIndex].Value.ToString();
            HiddenUsername.Value = username;

            try
            {
                DataTable dt = backendService.SearchUser(username, "username");
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    TxtFirstName.Text = row["FName"].ToString();
                    TxtLastName.Text = row["LName"].ToString();
                    TxtEmail.Text = row["email"].ToString();
                    TxtAddress.Text = row["address"].ToString();

                    if (dt.Columns.Contains("phone"))
                        TxtPhone.Text = row["phone"].ToString();
                    else
                        TxtPhone.Text = "";
                }
            }
            catch { }
            pnlModal.Visible = true;
        }
    }

    //הפעולה אוספת את הנתונים המעודכנים מהטפסים
    //ושולחת אותם לעדכון במסד הנתונים
    protected void BtnUpdateUser_Click(object sender, EventArgs e)
    {
        try
        {
            string username = HiddenUsername.Value;
            if (string.IsNullOrEmpty(username)) return;

            DataTable dt = backendService.SearchUser(username, "username");
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                localhost.Users user = new localhost.Users();
                user.UserN = username;

                user.NameF = TxtFirstName.Text.Trim();
                user.LastN = TxtLastName.Text.Trim();
                user.Email = TxtEmail.Text.Trim();
                user.Fulladdres = TxtAddress.Text.Trim();
                user.PhoneN = TxtPhone.Text.Trim();
                user.Pass = row["pass"].ToString();

                if (fileUploadPic.HasFile)
                {
                    try
                    {
                        string fileName = System.IO.Path.GetFileName(fileUploadPic.FileName);
                        string savePath = Server.MapPath("~/MyPics/") + fileName; //כדי לדעת איפה נמצא הקובץ- תיקייה
                        fileUploadPic.SaveAs(savePath); //שמירה בתיקייה
                        user.Pic = fileName; //שמירה בכתובת של הקובץ
                    }
                    catch { user.Pic = "Profile.jpg"; }
                }
                else
                {
                    if (row.Table.Columns.Contains("pic"))
                        user.Pic = row["pic"].ToString();
                    else if (row.ItemArray.Length > 9)
                        user.Pic = row[9].ToString();
                    else
                        user.Pic = "Profile.jpg";
                }

                //update
                backendService.UpdateUser(user);

                //Refresh 
                LoadAllUsers();
                pnlModal.Visible = false;

                string script = "alert('User updated successfully!');";
                ClientScript.RegisterStartupScript(this.GetType(), "Success", script, true);
            }
        }
        catch (Exception ex)
        {
            ShowError("Update failed: " + ex.Message);
        }
    }

    //הפעולה מוחקת את המשתמש הנבחר ממסד הנתונים
    protected void BtnDeleteUser_Click(object sender, EventArgs e)
    {
        string username = HiddenUsername.Value;
        if (!string.IsNullOrEmpty(username))
        {
            try
            {
                backendService.DeleteUser(username);
                LoadAllUsers();
                pnlModal.Visible = false;
                ClientScript.RegisterStartupScript(this.GetType(), "Deleted", "alert('User deleted successfully.');", true);
            }
            catch (Exception ex)
            {
                ShowError("Delete failed: " + ex.Message);
            }
        }
    }

    //הפעולה מסתירה את החלונית הקופצת ומבטלת את הבחירה
    //של השורה בטבלה כדי להחזיר את ממשק המשתמש למצבו הרגיל.
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        pnlModal.Visible = false;
        GrdUsers.SelectedIndex = -1;
    }

    //הפעולה מקבלת הודעת שגיאה ומציגה אותה למשתמש
    //תוך טיפול בתווים מיוחדים כדי למנוע תקלות בהרצת הקוד.
    private void ShowError(string msg)
    {
        string message = "alert('Error: " + msg.Replace("'", "\\'") + "');";
        ClientScript.RegisterStartupScript(this.GetType(), "Error", message, true);
    }

    // --- Helper Methods ---

    //הפעולה מחלצת את מספר הטלפון מתוך שורת הנתונים
    protected string GetPhoneValue(object dataItem)
    {
        if (dataItem == null) return "";
        DataRowView row = dataItem as DataRowView;
        if (row == null) return "";

        if (row.Row.Table.Columns.Contains("phone"))
        {
            object objVal = row["phone"];
            if (objVal != null && objVal != DBNull.Value)
            {
                string val = objVal.ToString().Trim();
                if (!string.IsNullOrEmpty(val) && !IsGender(val)) return val;
            }
        }
         
        if (row.Row.Table.Columns.Contains("gender")) //בדיקה שהמספר אינו מכיל בטעות מידע ששייך לעמודות אחרות
        {
            object objVal = row["gender"];
            if (objVal != null && objVal != DBNull.Value)
            {
                string val = objVal.ToString().Trim();
                if (!string.IsNullOrEmpty(val) && !IsGender(val) && !IsDate(val)) return val;
            }
        }
        return "";
    }

    //הפעולה מחלצת את ערך המגדר מתוך שורת הנתונים על ידי בדיקת העמודות הרלוונטיות
    protected string GetGenderValue(object dataItem)
    {
        if (dataItem == null) return "";
        DataRowView row = dataItem as DataRowView;
        if (row == null) return "";

        if (row.Row.Table.Columns.Contains("gender"))
        {
            object objVal = row["gender"];
            if (objVal != null && objVal != DBNull.Value)
            {
                string val = objVal.ToString().Trim();
                if (IsGender(val)) return val;
            }
        }

        if (row.Row.Table.Columns.Contains("phone"))
        {
            object objVal = row["phone"];
            if (objVal != null && objVal != DBNull.Value)
            {
                string val = objVal.ToString().Trim();
                if (IsGender(val)) return val;
            }
        }
        return "";
    }

    //הפעולה סורקת רשימת עמודות אפשריות כדי למצוא תאריך לידה,
    //מנסה להמיר את הערך שנמצא לתאריך תקין ומחזירה אותו בפורמט של חודש/יום/שנה.
    protected string GetBirthdayValue(object dataItem)
    {
        if (dataItem == null) return "";
        DataRowView row = dataItem as DataRowView;
        if (row == null) return "";

        string[] cols = { "birth", "birthday", "Birth", "Birthday", "phone" };

        foreach (string col in cols)
        {
            if (row.Row.Table.Columns.Contains(col))
            {
                object val = row[col];
                if (val != null && val != DBNull.Value)
                {
                    DateTime date;
                    if (DateTime.TryParse(val.ToString(), out date))
                        return date.ToString("MM/dd/yyyy");
                }
            }
        }
        return "";
    }

    //"male" או "female" הפעולה בודקת האם טקסט מסוים הוא
    private bool IsGender(string s)
    {
        s = s.ToLower();
        return s == "male" || s == "female";
    }

    //הפעולה בודקת האם טקסט מסוים מייצג תאריך תקין ומחזירה אמת או שקר בהתאם.
    private bool IsDate(string s)
    {
        DateTime d;
        return DateTime.TryParse(s, out d);
    }
}
