using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data.OleDb;
using MDb.App_Code;
// using MDb.App_Code; // Not needed if in same App_Code folder

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// [System.Web.Script.Services.ScriptService]

public class Service : System.Web.Services.WebService
{
    public Service() { }

    private string GetPath()
    {
        return Server.MapPath("App_Data/Db.mdf");
    }

    //    הפעולה מתעדת את פרטי השגיאה והזמן הנוכחי לקובץ טקסט בשרת(service_errors.log),
    //    ומונעת מהתוכנה לקרוס גם אם הכתיבה לקובץ נכשלת.
    private void LogError(Exception ex)
    {
        try
        {
            string logPath = Server.MapPath("App_Data/service_errors.log");
            string text = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + ex.ToString() + Environment.NewLine;
            System.IO.File.AppendAllText(logPath, text);
        }
        catch { }
    }

    //======================================================
    // USERS
    //======================================================


  //  פעולה מבצעת אימות כניסה(Login) על ידי הרצת שאילתת SQL הבודקת אם קיים משתמש או מנהל עם שם המשתמש
  //  והסיסמה שהוזנו בטבלאות המתאימות במסד הנתונים.
    [WebMethod]
    public DataTable Login(string username, string password, bool Choice)
    {
        string Sql = "Select * from ";
        if (Choice == true) //user
            Sql += "[Users] where [User]='" + username.Replace("'", "''") + "' and [pass]='" + password.Replace("'", "''") + "'";
        else
            Sql += "[Admin] where [Usern]='" + username.Replace("'", "''") + "' and [Pass]='" + password.Replace("'", "''") + "'";

        SqlCommand cmd = new SqlCommand(Sql); // מריץ פקודה על הDB
        return DbActions.SearchWithParameters(cmd, GetPath());
    }


   // פעולה רושמת משתמש חדש למסד הנתונים על ידי קבלת אובייקט Users והכנסת פרטיו(כמו שם, סיסמה, אימייל ותאריך לידה)
   // לטבלת Users תוך שימוש בפרמטרים מאובטחים למניעת הזרקות SQL.
    [WebMethod]
    public void Regi(Users users)
    {
        string sql = "INSERT INTO [Users] values(@p1 , @p2 , @p3 , @p4 ,@p5 , @p6 ,@p7 ,@p8 ,@p9 ,@p10)";
        SqlCommand cmmd = new SqlCommand(sql);

        cmmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.VarChar)).Value = users.UserN;
        cmmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.VarChar)).Value = users.Pass;
        cmmd.Parameters.Add(new SqlParameter("@p3", SqlDbType.VarChar)).Value = users.LastN;
        cmmd.Parameters.Add(new SqlParameter("@p4", SqlDbType.VarChar)).Value = users.NameF;
        cmmd.Parameters.Add(new SqlParameter("@p5", SqlDbType.VarChar)).Value = users.Fulladdres;
        cmmd.Parameters.Add(new SqlParameter("@p6", SqlDbType.VarChar)).Value = users.Email;
        cmmd.Parameters.Add(new SqlParameter("@p7", SqlDbType.VarChar)).Value = users.PhoneN;
        cmmd.Parameters.Add(new SqlParameter("@p8", SqlDbType.VarChar)).Value = users.Gender;

        DateTime birthdayDate;
        object birthValue = DBNull.Value;
        if (DateTime.TryParse(users.Birthday, out birthdayDate)) birthValue = birthdayDate;
        cmmd.Parameters.Add(new SqlParameter("@p9", SqlDbType.DateTime)).Value = birthValue;
        cmmd.Parameters.Add(new SqlParameter("@p10", SqlDbType.VarChar)).Value = users.Pic;

        DbActions.MyAction(cmmd, GetPath());
    }


   // הפעולה מעדכנת את פרטי המשתמש במסד הנתונים
   // לפי שם המשתמש שלו, ולאחר מכן שולפת ומחזירה את הנתונים המעודכנים כטבלה(DataTable).
    [WebMethod]
    public DataTable UpdateUser(Users user)
    {
        string sql = "Update [Users] SET [User]=@p1, [pass]=@p2, [FName]=@p3, [LName]=@p4, [address]=@p5,[email]=@p6,[phone]=@p7, [pic]=@p8 where [User]=@p9";
        SqlCommand cmmd = new SqlCommand(sql);

        cmmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.VarChar)).Value = user.UserN;
        cmmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.VarChar)).Value = user.Pass;
        cmmd.Parameters.Add(new SqlParameter("@p3", SqlDbType.VarChar)).Value = user.NameF;
        cmmd.Parameters.Add(new SqlParameter("@p4", SqlDbType.VarChar)).Value = user.LastN;
        cmmd.Parameters.Add(new SqlParameter("@p5", SqlDbType.VarChar)).Value = user.Fulladdres;
        cmmd.Parameters.Add(new SqlParameter("@p6", SqlDbType.VarChar)).Value = user.Email;
        cmmd.Parameters.Add(new SqlParameter("@p7", SqlDbType.VarChar)).Value = user.PhoneN;
        cmmd.Parameters.Add(new SqlParameter("@p8", SqlDbType.VarChar)).Value = user.Pic;
        cmmd.Parameters.Add(new SqlParameter("@p9", SqlDbType.VarChar)).Value = user.UserN;

        DbActions.MyAction(cmmd, GetPath());
        sql = "Select * from [Users] where [User]='" + user.UserN.Replace("'", "''") + "'";
        SqlCommand cmd = new SqlCommand(sql);
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public void DeleteUser(string username)
    {
        if (string.IsNullOrEmpty(username)) throw new Exception("Username is required");
        string sql = "DELETE FROM [Users] WHERE [User] = @p1";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.VarChar)).Value = username;
        DbActions.MyAction(cmd, GetPath());
    }

    //הפעולה מחפשת ושולפת משתמשים מטבלת Users על פי קריטריון שנבחר (שם, כתובת או שם משתמש),
    //ומחזירה את התוצאות כטבלה (DataTable).
    [WebMethod]
    public DataTable SearchUser(string data, string option)
    {
        string sql = "Select * from [Users] ";
        if (!string.IsNullOrEmpty(data) && !string.IsNullOrEmpty(option))
        {
            if (option.Equals("name")) sql += "WHERE [FName]='" + data.Replace("'", "''") + "'";
            else if (option.Equals("address")) sql += "WHERE [address]='" + data.Replace("'", "''") + "'";
            else if (option.Equals("username")) sql += "WHERE [User]='" + data.Replace("'", "''") + "'";
        }
        SqlCommand cmd = new SqlCommand(sql);
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    //======================================================
    // MOVIES
    //======================================================

    //הפעולה בודקת אם טבלת הסרטים (Movies) קיימת במסד הנתונים, ואם לא – היא יוצרת אותה עם
    //מבנה עמודות מפורט (כמו כותרת, ז'אנר ודירוג) ומכניסה אליה נתוני דוגמה ראשוניים.
    [WebMethod]
    public void CreateMoviesTable()
    {
        string checkTableSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Movies'";
        SqlCommand cmd = new SqlCommand(checkTableSql);
        DataTable dt = DbActions.SearchWithParameters(cmd, GetPath());
        int tableExists = (dt != null && dt.Rows.Count > 0) ? Convert.ToInt32(dt.Rows[0][0]) : 0;

        if (tableExists == 0)
        {
            string createTableSql = @"CREATE TABLE [Movies] (
                [MovieId] INT IDENTITY(1,1) PRIMARY KEY,
                [Title] NVARCHAR(255) NOT NULL,
                [Description] NVARCHAR(MAX),
                [Year] INT NOT NULL,
                [Genre] NVARCHAR(100),
                [Rating] DECIMAL(3,1) DEFAULT 0.0,
                [Poster] NVARCHAR(500),
                [Director] NVARCHAR(255),
                [Actors] NVARCHAR(MAX),
                [Duration] INT DEFAULT 0
            )";
            SqlCommand cmmd = new SqlCommand(createTableSql);
            DbActions.MyAction(cmmd, GetPath());
            InsertSampleMovies();
        }
    }
    //הפעולה יוצרת מערך של אובייקטי סרטים המכילים נתוני דוגמה
    //(כמו "בין כוכבים" ו"מת לחיות"), ומכניסה אותם בזה אחר זה למסד הנתונים באמצעות קריאה לפעולה AddMovieInternal.
    private void InsertSampleMovies()
    {
        Movies[] sampleMovies = new Movies[] {
            new Movies { Title = "Interstellar", Description = "Explorers travel through a wormhole...", Year = 2014, Genre = "Sci-Fi", Rating = 8.6m, Poster = "images/uploads/slider1.jpg", Director = "Christopher Nolan", Actors = "Matthew McConaughey", Duration = 169 },
            new Movies { Title = "The Revenant", Description = "A frontiersman fights for survival...", Year = 2015, Genre = "Drama", Rating = 8.0m, Poster = "images/uploads/slider2.jpg", Director = "Alejandro G. I??rritu", Actors = "Leonardo DiCaprio", Duration = 156 },
            new Movies { Title = "Die Hard", Description = "NYPD officer saves wife...", Year = 1988, Genre = "Action", Rating = 8.2m, Poster = "images/uploads/slider3.jpg", Director = "John McTiernan", Actors = "Bruce Willis", Duration = 132 },
            new Movies { Title = "The Walk", Description = "High-wire artist...", Year = 2015, Genre = "Drama", Rating = 7.3m, Poster = "images/uploads/slider4.jpg", Director = "Robert Zemeckis", Actors = "Joseph Gordon-Levitt", Duration = 123 }
        };
        foreach (Movies movie in sampleMovies) AddMovieInternal(movie);
    }

    //הפעולה מקבלת אובייקט של סרט ומכניסה את כל פרטיו לטבלת מוביס
    // במסד הנתונים תוך שימוש בפרמטרים מאובטחים ובדיקה האם
    private void AddMovieInternal(Movies movie)
    {
        string sql = "INSERT INTO [Movies] ([Title], [Description], [Year], [Genre], [Rating], [Poster], [Director], [Actors], [Duration]) VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9)";
        SqlCommand cmmd = new SqlCommand(sql);
        cmmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = movie.Title ?? (object)DBNull.Value; // ?? => if first value exist use it, else use the value after ??
        cmmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar)).Value = movie.Description ?? (object)DBNull.Value;
        cmmd.Parameters.Add(new SqlParameter("@p3", SqlDbType.Int)).Value = movie.Year;
        cmmd.Parameters.Add(new SqlParameter("@p4", SqlDbType.NVarChar)).Value = movie.Genre ?? (object)DBNull.Value;
        cmmd.Parameters.Add(new SqlParameter("@p5", SqlDbType.Decimal)).Value = movie.Rating;
        cmmd.Parameters.Add(new SqlParameter("@p6", SqlDbType.NVarChar)).Value = movie.Poster ?? (object)DBNull.Value;
        cmmd.Parameters.Add(new SqlParameter("@p7", SqlDbType.NVarChar)).Value = movie.Director ?? (object)DBNull.Value;
        cmmd.Parameters.Add(new SqlParameter("@p8", SqlDbType.NVarChar)).Value = movie.Actors ?? (object)DBNull.Value;
        cmmd.Parameters.Add(new SqlParameter("@p9", SqlDbType.Int)).Value = movie.Duration;
        DbActions.MyAction(cmmd, GetPath());
    }

    [WebMethod]
    public DataTable GetAllMovies()
    {
        // מחזיר את כל הסרטים מהטבלה של הסרטים לפי סדר הוצאה של השנה
        CreateMoviesTable();  // תיצור את הטבלה אם לא קיימת כבר
        string sql = "SELECT * FROM [Movies] ORDER BY [Year] DESC, [Title]"; //שאילתא
        SqlCommand cmd = new SqlCommand(sql); // מכין אובייקט של השאילתא לסוג הטבלה - sql
        return DbActions.SearchWithParameters(cmd, GetPath()); // תריץ את השאילתא מול הטבלה
    }

    [WebMethod]
    public DataTable GetMovies()
    {
        // להחזיר את כל הסרטים בלי מיון
        CreateMoviesTable();
        string sql = "SELECT * FROM [Movies]";
        SqlCommand cmd = new SqlCommand(sql);
        return DbActions.SearchWithParameters(cmd, GetPath());
    }



    [WebMethod]
    public DataTable SearchMovies(string searchTerm, string genre)
    {
        // חיפוש סרטים בטבלה
        CreateMoviesTable();
        string sql = "SELECT * FROM [Movies] WHERE 1=1";
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string escaped = searchTerm.Replace("'", "''");
            sql += " AND ([Title] LIKE '%" + escaped + "%' OR [Description] LIKE '%" + escaped + "%' OR [Director] LIKE '%" + escaped + "%' OR [Actors] LIKE '%" + escaped + "%')";
        }
        if (!string.IsNullOrWhiteSpace(genre) && genre.ToLower() != "all")
        {
            sql += " AND [Genre] = '" + genre.Replace("'", "''") + "'";
        }
        sql += " ORDER BY [Rating] DESC, [Year] DESC";
        SqlCommand cmd = new SqlCommand(sql);
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public DataTable GetMovieById(int movieId)
    {
        // קבלת סרט ספציפי לפי המספר המזהה שלו
        CreateMoviesTable();
        string sql = "SELECT * FROM [Movies] WHERE [MovieId] = @p1";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int)).Value = movieId;
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public DataTable GetMoviesByGenre(string genre)
    {
        CreateMoviesTable();
        string sql = "SELECT * FROM [Movies] WHERE [Genre] = @p1 ORDER BY [Rating] DESC, [Year] DESC";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = genre;
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public void AddMovie(Movies movie)
    {
        if (movie == null) throw new Exception("Movie data is required.");
        CreateMoviesTable();
        AddMovieInternal(movie);
    }

    [WebMethod]
    public void UpdateMovie(Movies movie)
    {
        if (movie == null || movie.MovieId <= 0) throw new Exception("Valid movie data required.");
        CreateMoviesTable();
        string sql = @"UPDATE [Movies] SET [Title]=@p1, [Description]=@p2, [Year]=@p3, [Genre]=@p4, [Rating]=@p5, [Poster]=@p6, [Director]=@p7, [Actors]=@p8, [Duration]=@p9 WHERE [MovieId]=@p10";
        SqlCommand cmmd = new SqlCommand(sql);
        // under explained : @p1 = movie.title 
        cmmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = movie.Title ?? (object)DBNull.Value;
        cmmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar)).Value = movie.Description ?? (object)DBNull.Value;
        cmmd.Parameters.Add(new SqlParameter("@p3", SqlDbType.Int)).Value = movie.Year;
        cmmd.Parameters.Add(new SqlParameter("@p4", SqlDbType.NVarChar)).Value = movie.Genre ?? (object)DBNull.Value;
        cmmd.Parameters.Add(new SqlParameter("@p5", SqlDbType.Decimal)).Value = movie.Rating;
        cmmd.Parameters.Add(new SqlParameter("@p6", SqlDbType.NVarChar)).Value = movie.Poster ?? (object)DBNull.Value;
        cmmd.Parameters.Add(new SqlParameter("@p7", SqlDbType.NVarChar)).Value = movie.Director ?? (object)DBNull.Value;
        cmmd.Parameters.Add(new SqlParameter("@p8", SqlDbType.NVarChar)).Value = movie.Actors ?? (object)DBNull.Value;
        cmmd.Parameters.Add(new SqlParameter("@p9", SqlDbType.Int)).Value = movie.Duration;
        cmmd.Parameters.Add(new SqlParameter("@p10", SqlDbType.Int)).Value = movie.MovieId;
        DbActions.MyAction(cmmd, GetPath());
    }

    [WebMethod]
    public void DeleteMovie(int movieId)
    {
        if (movieId <= 0) throw new Exception("Valid MovieId is required.");
        CreateMoviesTable();
        string sql = "DELETE FROM [Movies] WHERE [MovieId]=@p1";
        SqlCommand cmmd = new SqlCommand(sql);
        cmmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int)).Value = movieId;
        DbActions.MyAction(cmmd, GetPath());
    }

    //======================================================
    // WISHLIST
    //======================================================

    private void CreateWishlistTable()
    {
        string checkTableSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Wishlist'";
        SqlCommand cmd = new SqlCommand(checkTableSql);
        DataTable dt = DbActions.SearchWithParameters(cmd, GetPath());
        int tableExists = (dt != null && dt.Rows.Count > 0) ? Convert.ToInt32(dt.Rows[0][0]) : 0;

        if (tableExists == 0)
        {
            string createTableSql = @"CREATE TABLE [Wishlist] (
                    [Id] INT IDENTITY(1,1) PRIMARY KEY,
                    [Username] NVARCHAR(255) NOT NULL,
                    [MovieId] INT NOT NULL
                )";
            SqlCommand cmmd = new SqlCommand(createTableSql);
            DbActions.MyAction(cmmd, GetPath());
        }
    }

    [WebMethod]
    public void AddToWishlist(string username, int movieId)
    {
        if (string.IsNullOrWhiteSpace(username) || movieId <= 0) throw new Exception("Data required.");
        CreateMoviesTable();
        CreateWishlistTable();

        string checkSql = "SELECT COUNT(*) FROM [Wishlist] WHERE [Username]=@p1 AND [MovieId]=@p2";
        SqlCommand checkCmd = new SqlCommand(checkSql);
        checkCmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = username;
        checkCmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.Int)).Value = movieId;
        DataTable dt = DbActions.SearchWithParameters(checkCmd, GetPath());
        int exists = (dt != null && dt.Rows.Count > 0) ? Convert.ToInt32(dt.Rows[0][0]) : 0;

        if (exists == 0)
        {
            string insertSql = "INSERT INTO [Wishlist] ([Username], [MovieId]) VALUES (@p1, @p2)";
            SqlCommand insertCmd = new SqlCommand(insertSql);
            insertCmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = username;
            insertCmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.Int)).Value = movieId;
            DbActions.MyAction(insertCmd, GetPath());
        }
    }

    [WebMethod]
    public void RemoveFromWishlist(string username, int movieId)
    {
        if (string.IsNullOrWhiteSpace(username)) throw new Exception("Data required.");
        CreateWishlistTable();
        string deleteSql = "DELETE FROM [Wishlist] WHERE [Username]=@p1 AND [MovieId]=@p2";
        SqlCommand cmd = new SqlCommand(deleteSql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = username;
        cmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.Int)).Value = movieId;
        DbActions.MyAction(cmd, GetPath());
    }

    [WebMethod]
    public void ResetWishlistTable()
    {
        string dropSql = "IF OBJECT_ID('dbo.Wishlist', 'U') IS NOT NULL DROP TABLE dbo.Wishlist";
        SqlCommand cmd = new SqlCommand(dropSql);
        DbActions.MyAction(cmd, GetPath()); // Executing the command to drop the table
        CreateWishlistTable(); // Re-create it immediately
    }

    [WebMethod]
    public DataTable GetWishlistMovies(string username)
    {
        if (string.IsNullOrWhiteSpace(username)) throw new Exception("Username is required.");
        CreateMoviesTable();
        CreateWishlistTable();
        string sql = @"SELECT m.* FROM [Movies] m INNER JOIN [Wishlist] w ON m.[MovieId] = w.[MovieId] WHERE w.[Username] = @p1";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = username;
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public DataTable GetUsersWhoWishlistedMovie(int movieId)
    {
        // Join Wishlist with Users to get user details (pic)
        CreateWishlistTable();
        string sql = @"
            SELECT u.[User] as Username, u.[pic] 
            FROM [Wishlist] w 
            INNER JOIN [Users] u ON w.[Username] = u.[User] 
            WHERE w.[MovieId] = @p1";
        
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int)).Value = movieId;
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    //======================================================
    // USER MOVIE INTERACTIONS (WATCHED & COMMENTS)
    //======================================================

    private void CreateMovieReviewsTable()
    {
        string checkTableSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'MovieReviews'";
        SqlCommand cmd = new SqlCommand(checkTableSql);
        DataTable dt = DbActions.SearchWithParameters(cmd, GetPath());
        int tableExists = (dt != null && dt.Rows.Count > 0) ? Convert.ToInt32(dt.Rows[0][0]) : 0;

        if (tableExists == 0)
        {
            string createTableSql = @"CREATE TABLE [MovieReviews] (
                [InteractionId] INT IDENTITY(1,1) PRIMARY KEY,
                [Username] NVARCHAR(255) NOT NULL,
                [MovieId] INT NOT NULL,
                [IsWatched] BIT DEFAULT 0,
                [Rating] INT NULL,
                [CommentText] NVARCHAR(MAX) NULL,
                [CreatedAt] DATETIME DEFAULT GETDATE(),
                CONSTRAINT UQ_User_Movie UNIQUE ([Username], [MovieId])
            )";
            SqlCommand cmmd = new SqlCommand(createTableSql);
            DbActions.MyAction(cmmd, GetPath());
        }
    }

    [WebMethod]
    public void AddToWatched(string username, int movieId)
    {
        if (string.IsNullOrWhiteSpace(username) || movieId <= 0) throw new Exception("Data required.");
        CreateMoviesTable();
        CreateMovieReviewsTable();

        string sql = @"
            IF EXISTS (SELECT 1 FROM [MovieReviews] WHERE [Username]=@p1 AND [MovieId]=@p2)
                UPDATE [MovieReviews] SET [IsWatched]=1 WHERE [Username]=@p1 AND [MovieId]=@p2
            ELSE
                INSERT INTO [MovieReviews] ([Username], [MovieId], [IsWatched]) VALUES (@p1, @p2, 1)";
        
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = username;
        cmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.Int)).Value = movieId;
        DbActions.MyAction(cmd, GetPath());

        // Remove from wishlist if it exists there
        try { RemoveFromWishlist(username, movieId); } catch { }
    }

    [WebMethod]
    public void RemoveFromWatched(string username, int movieId)
    {
        if (string.IsNullOrWhiteSpace(username)) throw new Exception("Data required.");
        CreateMovieReviewsTable();
        string sql = "UPDATE [MovieReviews] SET [IsWatched]=0 WHERE [Username]=@p1 AND [MovieId]=@p2";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = username;
        cmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.Int)).Value = movieId;
        DbActions.MyAction(cmd, GetPath());
    }

    [WebMethod]
    public DataTable GetWatchedMovies(string username)
    {
        if (string.IsNullOrWhiteSpace(username)) throw new Exception("Username is required.");
        CreateMoviesTable();
        CreateMovieReviewsTable();
        string sql = @"SELECT m.* FROM [Movies] m INNER JOIN [MovieReviews] w ON m.[MovieId] = w.[MovieId] WHERE w.[Username] = @p1 AND w.[IsWatched] = 1";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = username;
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public bool IsWatched(string username, int movieId)
    {
        if (string.IsNullOrWhiteSpace(username) || movieId <= 0) return false;
        CreateMovieReviewsTable();
        string sql = "SELECT COUNT(*) FROM [MovieReviews] WHERE [Username]=@p1 AND [MovieId]=@p2 AND [IsWatched]=1";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = username;
        cmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.Int)).Value = movieId;
        DataTable dt = DbActions.SearchWithParameters(cmd, GetPath());
        int count = (dt != null && dt.Rows.Count > 0) ? Convert.ToInt32(dt.Rows[0][0]) : 0;
        return count > 0;
    }

    [WebMethod]
    public DataTable GetUsersWhoWatchedMovie(int movieId)
    {
        // Join Interactions with Users to get user details (pic)
        CreateMovieReviewsTable();
        string sql = @"
            SELECT u.[User] as Username, u.[pic] 
            FROM [MovieReviews] w 
            INNER JOIN [Users] u ON w.[Username] = u.[User] 
            WHERE w.[MovieId] = @p1 AND w.[IsWatched] = 1";
        
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int)).Value = movieId;
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    //======================================================
    // COMMENTS
    //======================================================

    [WebMethod]
    public void AddMovieComment(string username, int movieId, int rating, string commentText)
    {
        if (string.IsNullOrWhiteSpace(username) || movieId <= 0 || string.IsNullOrWhiteSpace(commentText)) throw new Exception("Data required.");
        CreateMoviesTable();
        CreateMovieReviewsTable();

        string sql = @"
            IF EXISTS (SELECT 1 FROM [MovieReviews] WHERE [Username]=@p1 AND [MovieId]=@p2)
                UPDATE [MovieReviews] SET [Rating]=@p3, [CommentText]=@p4, [CreatedAt]=@p5 WHERE [Username]=@p1 AND [MovieId]=@p2
            ELSE
                INSERT INTO [MovieReviews] ([Username], [MovieId], [Rating], [CommentText], [CreatedAt]) VALUES (@p1, @p2, @p3, @p4, @p5)";
        
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = username;
        cmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.Int)).Value = movieId;
        cmd.Parameters.Add(new SqlParameter("@p3", SqlDbType.Int)).Value = rating;
        cmd.Parameters.Add(new SqlParameter("@p4", SqlDbType.NVarChar)).Value = commentText;
        cmd.Parameters.Add(new SqlParameter("@p5", SqlDbType.DateTime)).Value = DateTime.Now;
        DbActions.MyAction(cmd, GetPath());
    }

    [WebMethod]
    public DataTable GetMovieComments(int movieId)
    {
        CreateMovieReviewsTable();
        string sql = "SELECT * FROM [MovieReviews] WHERE [MovieId] = @p1 AND [CommentText] IS NOT NULL ORDER BY [CreatedAt] DESC";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int)).Value = movieId;
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    //======================================================
    // CELEBS
    //======================================================

    [WebMethod]
    public void CreateCelebsTable()
    {
        string checkTableSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Celebs'";
        SqlCommand cmd = new SqlCommand(checkTableSql);
        DataTable dt = DbActions.SearchWithParameters(cmd, GetPath());
        int tableExists = (dt != null && dt.Rows.Count > 0) ? Convert.ToInt32(dt.Rows[0][0]) : 0;

        if (tableExists == 0)
        {
            string createTableSql = @"CREATE TABLE [Celebs] (
                [CelebId] INT IDENTITY(1,1) PRIMARY KEY,
                [Name] NVARCHAR(255) NOT NULL,
                [Role] NVARCHAR(255),
                [Photo] NVARCHAR(500),
                [Bio] NVARCHAR(MAX)
            )";
            SqlCommand cmmd = new SqlCommand(createTableSql);
            DbActions.MyAction(cmmd, GetPath());
            InsertSampleCelebs();
        }
    }

    private void InsertSampleCelebs()
    {
        Celeb[] sampleCelebs = new Celeb[] {
            new Celeb { Name = "Leonardo DiCaprio", Role = "Actor", Photo = "images/uploads/ava1.jpg", Bio = "Academy Award-winning actor..." },
            new Celeb { Name = "Anne Hathaway", Role = "Actress", Photo = "images/uploads/ava2.jpg", Bio = "American actress..." },
            new Celeb { Name = "Christopher Nolan", Role = "Director", Photo = "images/uploads/ava3.jpg", Bio = "British-American film director..." },
            new Celeb { Name = "Tom Hardy", Role = "Actor", Photo = "images/uploads/ava4.jpg", Bio = "English actor..." }
        };
        foreach (Celeb celeb in sampleCelebs) AddCelebInternal(celeb);
    }

    private void AddCelebInternal(Celeb celeb)
    {
        string sql = "INSERT INTO [Celebs] ([Name], [Role], [Photo], [Bio]) VALUES (@p1, @p2, @p3, @p4)";
        SqlCommand cmmd = new SqlCommand(sql);
        cmmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = celeb.Name ?? (object)DBNull.Value;
        cmmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar)).Value = celeb.Role ?? (object)DBNull.Value;
        cmmd.Parameters.Add(new SqlParameter("@p3", SqlDbType.NVarChar)).Value = celeb.Photo ?? (object)DBNull.Value;
        cmmd.Parameters.Add(new SqlParameter("@p4", SqlDbType.NVarChar)).Value = celeb.Bio ?? (object)DBNull.Value;
        DbActions.MyAction(cmmd, GetPath());
    }

    [WebMethod]
    public DataTable GetAllCelebs()
    {
        CreateCelebsTable();
        string sql = "SELECT * FROM [Celebs] ORDER BY [Name]";
        SqlCommand cmd = new SqlCommand(sql);
        return DbActions.SearchWithParameters(cmd, GetPath());
    }



    [WebMethod]
    public DataTable SearchCelebs(string searchText, string role)
    {
        CreateCelebsTable();
        string sql = "SELECT * FROM [Celebs] WHERE 1=1";
        SqlCommand cmd = new SqlCommand();

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            sql += " AND ([Name] LIKE @searchText OR [Bio] LIKE @searchText)";
            cmd.Parameters.Add(new SqlParameter("@searchText", SqlDbType.NVarChar)).Value = "%" + searchText + "%";
        }
        if (!string.IsNullOrWhiteSpace(role) && role.ToLower() != "all")
        {
            sql += " AND [Role] = @role";
            cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.NVarChar)).Value = role;
        }
        sql += " ORDER BY [Name]";
        cmd.CommandText = sql;
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public void AddCeleb(Celeb celeb)
    {
        if (celeb == null) throw new Exception("Celeb data is required.");
        CreateCelebsTable();
        AddCelebInternal(celeb);
    }

    [WebMethod]
    public void UpdateCeleb(Celeb celeb)
    {
        if (celeb == null || celeb.CelebId <= 0) throw new Exception("Valid celeb data required.");
        CreateCelebsTable();
        string sql = "UPDATE [Celebs] SET [Name]=@p1, [Role]=@p2, [Photo]=@p3, [Bio]=@p4 WHERE [CelebId]=@p5";
        SqlCommand cmmd = new SqlCommand(sql);
        cmmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = celeb.Name ?? (object)DBNull.Value;
        cmmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar)).Value = celeb.Role ?? (object)DBNull.Value;
        cmmd.Parameters.Add(new SqlParameter("@p3", SqlDbType.NVarChar)).Value = celeb.Photo ?? (object)DBNull.Value;
        cmmd.Parameters.Add(new SqlParameter("@p4", SqlDbType.NVarChar)).Value = celeb.Bio ?? (object)DBNull.Value;
        cmmd.Parameters.Add(new SqlParameter("@p5", SqlDbType.Int)).Value = celeb.CelebId;
        DbActions.MyAction(cmmd, GetPath());
    }

    [WebMethod]
    public void DeleteCeleb(int celebId)
    {
        CreateCelebsTable();
        string sql = "DELETE FROM [Celebs] WHERE [CelebId]=@p1";
        SqlCommand cmmd = new SqlCommand(sql);
        cmmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int)).Value = celebId;
        DbActions.MyAction(cmmd, GetPath());
    }


    //======================================================
    // FOLLOWERS
    //======================================================

    [WebMethod]
    public bool IsFollowing(string follower, string following)
    {
        // Checks if 'follower' is already following 'following'
        string sql = "SELECT COUNT(*) FROM [Followers] WHERE [FollowerUser]=@p1 AND [FollowingUser]=@p2";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = follower;
        cmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar)).Value = following;

        DataTable dt = DbActions.SearchWithParameters(cmd, GetPath());
        int count = (dt != null && dt.Rows.Count > 0) ? Convert.ToInt32(dt.Rows[0][0]) : 0;
        return count > 0;
    }

    [WebMethod]
    public void FollowUser(string follower, string following)
    {
        // Prevent following yourself
        if (follower == following) return;

        if (!IsFollowing(follower, following))
        {
            string sql = "INSERT INTO [Followers] ([FollowerUser], [FollowingUser]) VALUES (@p1, @p2)";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = follower;
            cmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar)).Value = following;
            DbActions.MyAction(cmd, GetPath());
        }
    }

    [WebMethod]
    public void UnfollowUser(string follower, string following)
    {
        string sql = "DELETE FROM [Followers] WHERE [FollowerUser]=@p1 AND [FollowingUser]=@p2";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = follower;
        cmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar)).Value = following;
        DbActions.MyAction(cmd, GetPath());
    }

    [WebMethod]
    public int GetFollowersCount(string username)
    {
        // Count how many people follow this user
        string sql = "SELECT COUNT(*) FROM [Followers] WHERE [FollowingUser]=@p1";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = username;

        DataTable dt = DbActions.SearchWithParameters(cmd, GetPath());
        return (dt != null && dt.Rows.Count > 0) ? Convert.ToInt32(dt.Rows[0][0]) : 0;
    }

    [WebMethod]
    public int GetFollowingCount(string username)
    {
        // Count how many people this user follows
        string sql = "SELECT COUNT(*) FROM [Followers] WHERE [FollowerUser]=@p1";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = username;

        DataTable dt = DbActions.SearchWithParameters(cmd, GetPath());
        return (dt != null && dt.Rows.Count > 0) ? Convert.ToInt32(dt.Rows[0][0]) : 0;
    }

    [WebMethod]
    public DataTable GetFollowersList(string username)
    {
        // Gets list of people following 'username'
        string sql = "SELECT * FROM [Followers] WHERE [FollowingUser] = @p1";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = username;
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public DataTable GetFollowingList(string username)
    {
        // Gets list of people 'username' is following
        string sql = "SELECT * FROM [Followers] WHERE [FollowerUser] = @p1";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = username;
        return DbActions.SearchWithParameters(cmd, GetPath());
    }


    //======================================================
    // EVENTS - ADD THESE METHODS TO Service.cs BEFORE THE CLOSING }
    //======================================================

    [WebMethod]
    public void CreateEventsTable()
    {
        string checkEventsSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Events'";
        SqlCommand cmd = new SqlCommand(checkEventsSql);
        DataTable dt = DbActions.SearchWithParameters(cmd, GetPath());
        int eventsExists = (dt != null && dt.Rows.Count > 0) ? Convert.ToInt32(dt.Rows[0][0]) : 0;

        if (eventsExists == 0)
        {
            string createEventsSql = @"CREATE TABLE [Events] (
                [EventId] INT IDENTITY(1,1) PRIMARY KEY,
                [Username] NVARCHAR(50) NOT NULL,
                [MovieId] INT NOT NULL,
                [EventDate] DATE NOT NULL,
                [StartTime] TIME NOT NULL,
                [Price] DECIMAL(10,2) DEFAULT 0.0,
                [Location] NVARCHAR(100) DEFAULT '',
                [Status] NVARCHAR(20) DEFAULT 'Open',
                [CreatedAt] DATETIME DEFAULT GETDATE(),
                FOREIGN KEY ([MovieId]) REFERENCES [Movies]([MovieId])
            )";
            SqlCommand cmmd = new SqlCommand(createEventsSql);
            DbActions.MyAction(cmmd, GetPath());
        }
        else
        {
            // Check if Location column exists, if not add it
            string checkColSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Events' AND COLUMN_NAME = 'Location'";
            SqlCommand colCmd = new SqlCommand(checkColSql);
            DataTable dtCol = DbActions.SearchWithParameters(colCmd, GetPath());
            int colExists = (dtCol != null && dtCol.Rows.Count > 0) ? Convert.ToInt32(dtCol.Rows[0][0]) : 0;
            
            if (colExists == 0)
            {
                string addColSql = "ALTER TABLE [Events] ADD [Location] NVARCHAR(100) DEFAULT ''";
                SqlCommand addCmd = new SqlCommand(addColSql);
                DbActions.MyAction(addCmd, GetPath());
            }
        }

        string checkSubsSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'EventSubscriptions'";
        SqlCommand cmd2 = new SqlCommand(checkSubsSql);
        DataTable dt2 = DbActions.SearchWithParameters(cmd2, GetPath());
        int subsExists = (dt2 != null && dt2.Rows.Count > 0) ? Convert.ToInt32(dt2.Rows[0][0]) : 0;

        if (subsExists == 0)
        {
            string createSubsSql = @"CREATE TABLE [EventSubscriptions] (
                [SubscriptionId] INT IDENTITY(1,1) PRIMARY KEY,
                [EventId] INT NOT NULL,
                [Username] NVARCHAR(50) NOT NULL,
                [SubscribedAt] DATETIME DEFAULT GETDATE(),
                FOREIGN KEY ([EventId]) REFERENCES [Events]([EventId]) ON DELETE CASCADE,
                UNIQUE([EventId], [Username])
            )";
            SqlCommand cmmd2 = new SqlCommand(createSubsSql);
            DbActions.MyAction(cmmd2, GetPath());
        }
    }

    [WebMethod]
    public int CreateEvent(string username, int movieId, string eventDate, string startTime, decimal price, string location, string status)
    {
        if (string.IsNullOrWhiteSpace(username) || movieId <= 0) throw new Exception("Username and MovieId are required.");
        CreateMoviesTable();
        CreateEventsTable();

        string sql = @"INSERT INTO [Events] ([Username], [MovieId], [EventDate], [StartTime], [Price], [Location], [Status], [CreatedAt]) 
                       VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8); SELECT SCOPE_IDENTITY();";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = username;
        cmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.Int)).Value = movieId;
        cmd.Parameters.Add(new SqlParameter("@p3", SqlDbType.Date)).Value = DateTime.Parse(eventDate);
        cmd.Parameters.Add(new SqlParameter("@p4", SqlDbType.Time)).Value = TimeSpan.Parse(startTime);
        cmd.Parameters.Add(new SqlParameter("@p5", SqlDbType.Decimal)).Value = price;
        cmd.Parameters.Add(new SqlParameter("@p6", SqlDbType.NVarChar)).Value = location ?? "";
        cmd.Parameters.Add(new SqlParameter("@p7", SqlDbType.NVarChar)).Value = status ?? "Open";
        cmd.Parameters.Add(new SqlParameter("@p8", SqlDbType.DateTime)).Value = DateTime.Now;

        DataTable result = DbActions.SearchWithParameters(cmd, GetPath());
        if (result != null && result.Rows.Count > 0)
        {
            return Convert.ToInt32(result.Rows[0][0]);
        }
        return 0;
    }

    [WebMethod]
    public DataTable GetEventById(int eventId)
    {
        CreateEventsTable();
        string sql = @"SELECT e.*, m.Title, m.Poster, m.Genre, m.Rating, m.Year, m.Director, m.Actors
                       FROM [Events] e
                       INNER JOIN [Movies] m ON e.[MovieId] = m.[MovieId]
                       WHERE e.[EventId] = @p1";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int)).Value = eventId;
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public DataTable GetAllEvents()
    {
        CreateEventsTable();
        string sql = @"SELECT e.*, m.Title, m.Poster, m.Genre, m.Rating
                       FROM [Events] e
                       INNER JOIN [Movies] m ON e.[MovieId] = m.[MovieId]
                       ORDER BY e.[EventDate] DESC, e.[StartTime] DESC";
        SqlCommand cmd = new SqlCommand(sql);
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public DataTable GetEventsByUser(string username)
    {
        if (string.IsNullOrWhiteSpace(username)) throw new Exception("Username is required.");
        CreateEventsTable();
        string sql = @"SELECT e.*, m.Title, m.Poster, m.Genre, m.Rating
                       FROM [Events] e
                       INNER JOIN [Movies] m ON e.[MovieId] = m.[MovieId]
                       WHERE e.[Username] = @p1
                       ORDER BY e.[EventDate] DESC, e.[StartTime] DESC";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = username;
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public DataTable GetUpcomingEvents()
    {
        CreateEventsTable();
        string sql = @"SELECT e.*, m.Title, m.Poster, m.Genre, m.Rating
                       FROM [Events] e
                       INNER JOIN [Movies] m ON e.[MovieId] = m.[MovieId]
                       WHERE e.[Status] = 'Open' AND e.[EventDate] >= CAST(GETDATE() AS DATE)
                       ORDER BY e.[EventDate] ASC, e.[StartTime] ASC";
        SqlCommand cmd = new SqlCommand(sql);
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public void UpdateEventStatus(int eventId, string status)
    {
        if (eventId <= 0 || string.IsNullOrWhiteSpace(status)) throw new Exception("EventId and Status are required.");
        CreateEventsTable();
        string sql = "UPDATE [Events] SET [Status] = @p1 WHERE [EventId] = @p2";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = status;
        cmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.Int)).Value = eventId;
        DbActions.MyAction(cmd, GetPath());
    }

    [WebMethod]
    public void UpdateEvent(int eventId, string eventDate, string startTime, decimal price, string location)
    {
        if (eventId <= 0) throw new Exception("Valid EventId is required.");
        CreateEventsTable();
        string sql = @"UPDATE [Events] 
                       SET [EventDate] = @p1, [StartTime] = @p2, [Price] = @p3, [Location] = @p4 
                       WHERE [EventId] = @p5";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Date)).Value = DateTime.Parse(eventDate);
        cmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.Time)).Value = TimeSpan.Parse(startTime);
        cmd.Parameters.Add(new SqlParameter("@p3", SqlDbType.Decimal)).Value = price;
        cmd.Parameters.Add(new SqlParameter("@p4", SqlDbType.NVarChar)).Value = location ?? "";
        cmd.Parameters.Add(new SqlParameter("@p5", SqlDbType.Int)).Value = eventId;
        DbActions.MyAction(cmd, GetPath());
    }

    [WebMethod]
    public void DeleteEvent(int eventId)
    {
        if (eventId <= 0) throw new Exception("Valid EventId is required.");
        CreateEventsTable();
        string sql = "DELETE FROM [Events] WHERE [EventId] = @p1";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int)).Value = eventId;
        DbActions.MyAction(cmd, GetPath());
    }

    [WebMethod]
    public void SubscribeToEvent(int eventId, string username)
    {
        if (eventId <= 0 || string.IsNullOrWhiteSpace(username)) throw new Exception("EventId and Username are required.");
        CreateEventsTable();

        string checkSql = "SELECT COUNT(*) FROM [EventSubscriptions] WHERE [EventId]=@p1 AND [Username]=@p2";
        SqlCommand checkCmd = new SqlCommand(checkSql);
        checkCmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int)).Value = eventId;
        checkCmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar)).Value = username;
        DataTable dt = DbActions.SearchWithParameters(checkCmd, GetPath());
        int exists = (dt != null && dt.Rows.Count > 0) ? Convert.ToInt32(dt.Rows[0][0]) : 0;

        if (exists == 0)
        {
            string insertSql = "INSERT INTO [EventSubscriptions] ([EventId], [Username], [SubscribedAt]) VALUES (@p1, @p2, @p3)";
            SqlCommand insertCmd = new SqlCommand(insertSql);
            insertCmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int)).Value = eventId;
            insertCmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar)).Value = username;
            insertCmd.Parameters.Add(new SqlParameter("@p3", SqlDbType.DateTime)).Value = DateTime.Now;
            DbActions.MyAction(insertCmd, GetPath());
        }
    }

    [WebMethod]
    public void UnsubscribeFromEvent(int eventId, string username)
    {
        if (eventId <= 0 || string.IsNullOrWhiteSpace(username)) throw new Exception("EventId and Username are required.");
        CreateEventsTable();
        string sql = "DELETE FROM [EventSubscriptions] WHERE [EventId]=@p1 AND [Username]=@p2";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int)).Value = eventId;
        cmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar)).Value = username;
        DbActions.MyAction(cmd, GetPath());
    }

    [WebMethod]
    public DataTable GetEventSubscribers(int eventId)
    {
        CreateEventsTable();
        string sql = @"SELECT u.[User] as Username, u.[FName], u.[LName], u.[pic], es.[SubscribedAt]
                       FROM [EventSubscriptions] es
                       INNER JOIN [Users] u ON es.[Username] = u.[User]
                       WHERE es.[EventId] = @p1
                       ORDER BY es.[SubscribedAt] ASC";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int)).Value = eventId;
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public bool IsUserSubscribed(int eventId, string username)
    {
        if (eventId <= 0 || string.IsNullOrWhiteSpace(username)) return false;
        CreateEventsTable();
        string sql = "SELECT COUNT(*) FROM [EventSubscriptions] WHERE [EventId]=@p1 AND [Username]=@p2";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int)).Value = eventId;
        cmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar)).Value = username;
        DataTable dt = DbActions.SearchWithParameters(cmd, GetPath());
        int count = (dt != null && dt.Rows.Count > 0) ? Convert.ToInt32(dt.Rows[0][0]) : 0;
        return count > 0;
    }

    [WebMethod]
    public DataTable GetUserSubscriptions(string username)
    {
        if (string.IsNullOrWhiteSpace(username)) throw new Exception("Username is required.");
        CreateEventsTable();
        string sql = @"SELECT e.*, m.Title, m.Poster, m.Genre, m.Rating, es.[SubscribedAt]
                       FROM [EventSubscriptions] es
                       INNER JOIN [Events] e ON es.[EventId] = e.[EventId]
                       INNER JOIN [Movies] m ON e.[MovieId] = m.[MovieId]
                       WHERE es.[Username] = @p1
                       ORDER BY e.[EventDate] DESC, e.[StartTime] DESC";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = username;
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public DataTable GetEventsByLocation(string location)
    {
        CreateEventsTable();
        string sql = @"SELECT e.*, m.Title, m.Poster, m.Genre, m.Rating
                       FROM [Events] e
                       INNER JOIN [Movies] m ON e.[MovieId] = m.[MovieId]
                       WHERE e.[Location] LIKE @p1
                       ORDER BY e.[EventDate] DESC, e.[StartTime] DESC";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = "%" + location + "%";
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    //======================================================
    // STORE AND ORDERS
    //======================================================

    [WebMethod]
    public void CreateStoreTables()
    {
        // Products
        string checkProductsSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Products'";
        SqlCommand cmd1 = new SqlCommand(checkProductsSql);
        DataTable dt1 = DbActions.SearchWithParameters(cmd1, GetPath());
        int prodExists = (dt1 != null && dt1.Rows.Count > 0) ? Convert.ToInt32(dt1.Rows[0][0]) : 0;

        if (prodExists == 0)
        {
            string createProductsSql = @"CREATE TABLE [Products] (
                [ProductId] INT IDENTITY(1,1) PRIMARY KEY,
                [ProductCode] NVARCHAR(50) NOT NULL UNIQUE,
                [Name] NVARCHAR(100) NOT NULL,
                [Price] DECIMAL(10,2) NOT NULL,
                [Description] NVARCHAR(MAX),
                [Picture] NVARCHAR(500)
            )";
            SqlCommand cmmd1 = new SqlCommand(createProductsSql);
            DbActions.MyAction(cmmd1, GetPath());
            
            // Seed data
            string[] names = { "Popcorn", "Nachos", "Soda", "Water", "Slushee", "Event Merch T-Shirt" };
            decimal[] prices = { 15.00m, 20.00m, 10.00m, 5.00m, 12.00m, 50.00m };
            string[] descs = { "Large butter popcorn", "Crispy nachos with cheese", "Refreshing soda 500ml", "Mineral water 500ml", "Icy fruit slushee", "Exclusive event t-shirt" };
            string[] pics = { "images/popcorn.jpg", "images/nachos.jpg", "images/soda.jpg", "images/water.jpg", "images/slushee.jpg", "images/shirt.jpg" };
            
            for(int i = 0; i < names.Length; i++)
            {
                string code = "PRD-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                string sqlIns = "INSERT INTO [Products] ([ProductCode], [Name], [Price], [Description], [Picture]) VALUES (@p1, @p2, @p3, @p4, @p5)";
                SqlCommand cmdIns = new SqlCommand(sqlIns);
                cmdIns.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = code;
                cmdIns.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar)).Value = names[i];
                cmdIns.Parameters.Add(new SqlParameter("@p3", SqlDbType.Decimal)).Value = prices[i];
                cmdIns.Parameters.Add(new SqlParameter("@p4", SqlDbType.NVarChar)).Value = descs[i];
                cmdIns.Parameters.Add(new SqlParameter("@p5", SqlDbType.NVarChar)).Value = pics[i];
                DbActions.MyAction(cmdIns, GetPath());
            }
        }

        // Orders
        string checkOrdersSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Orders'";
        SqlCommand cmd2 = new SqlCommand(checkOrdersSql);
        DataTable dt2 = DbActions.SearchWithParameters(cmd2, GetPath());
        int ordExists = (dt2 != null && dt2.Rows.Count > 0) ? Convert.ToInt32(dt2.Rows[0][0]) : 0;

        if (ordExists == 0)
        {
            string createOrdersSql = @"CREATE TABLE [Orders] (
                [OrderId] INT IDENTITY(1,1) PRIMARY KEY,
                [OrderCode] NVARCHAR(50) NOT NULL UNIQUE,
                [Username] NVARCHAR(50) NOT NULL,
                [EventId] INT NOT NULL,
                [DatePurchased] DATETIME DEFAULT GETDATE(),
                [Total] DECIMAL(10,2) NOT NULL
            )";
            SqlCommand cmmd2 = new SqlCommand(createOrdersSql);
            DbActions.MyAction(cmmd2, GetPath());
        }

        // OrderItems
        string checkItemsSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'OrderItems'";
        SqlCommand cmd3 = new SqlCommand(checkItemsSql);
        DataTable dt3 = DbActions.SearchWithParameters(cmd3, GetPath());
        int itemsExists = (dt3 != null && dt3.Rows.Count > 0) ? Convert.ToInt32(dt3.Rows[0][0]) : 0;

        if (itemsExists == 0)
        {
            string createItemsSql = @"CREATE TABLE [OrderItems] (
                [OrderItemId] INT IDENTITY(1,1) PRIMARY KEY,
                [OrderCode] NVARCHAR(50) NOT NULL,
                [ProductCode] NVARCHAR(50) NOT NULL,
                [Quantity] INT NOT NULL,
                FOREIGN KEY ([OrderCode]) REFERENCES [Orders]([OrderCode]) ON DELETE CASCADE,
                FOREIGN KEY ([ProductCode]) REFERENCES [Products]([ProductCode])
            )";
            SqlCommand cmmd3 = new SqlCommand(createItemsSql);
            DbActions.MyAction(cmmd3, GetPath());
        }

        // Adjust existing product list if needed
        try
        {
            try {
                string alterSql = "ALTER TABLE [Products] ADD [IsActive] BIT DEFAULT 1 WITH VALUES";
                DbActions.MyAction(new SqlCommand(alterSql), GetPath());
            } catch { }

            // Fix Slushie spelling
            string updateSql = "UPDATE [Products] SET [Name] = 'Slushie' WHERE [Name] = 'Slushi' OR [Name] = 'Slushee'";
            DbActions.MyAction(new SqlCommand(updateSql), GetPath());

            // Add S, M, L popcorn if not added yet
            string checkPopcorn = "SELECT COUNT(*) FROM [Products] WHERE [Name] LIKE 'Popcorn (%)'";
            DataTable dtPopcorn = DbActions.SearchWithParameters(new SqlCommand(checkPopcorn), GetPath());
            if (dtPopcorn != null && dtPopcorn.Rows.Count > 0 && Convert.ToInt32(dtPopcorn.Rows[0][0]) == 0)
            {
                string codeS = "PRD-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                string sql1 = "INSERT INTO [Products] ([ProductCode], [Name], [Price], [Description], [Picture]) VALUES ('" + codeS + "', 'Popcorn (S)', 10.00, 'Small size popcorn.', 'images/popcorn.jpg')";
                DbActions.MyAction(new SqlCommand(sql1), GetPath());

                string codeM = "PRD-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                string sql2 = "INSERT INTO [Products] ([ProductCode], [Name], [Price], [Description], [Picture]) VALUES ('" + codeM + "', 'Popcorn (M)', 15.00, 'Medium size popcorn.', 'images/popcorn.jpg')";
                DbActions.MyAction(new SqlCommand(sql2), GetPath());

                string codeL = "PRD-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                string sql3 = "INSERT INTO [Products] ([ProductCode], [Name], [Price], [Description], [Picture]) VALUES ('" + codeL + "', 'Popcorn (L)', 20.00, 'Large size popcorn.', 'images/popcorn.jpg')";
                DbActions.MyAction(new SqlCommand(sql3), GetPath());
            }
        }
        catch { }
    }

    [WebMethod]
    public DataTable GetProducts()
    {
        CreateStoreTables();
        string sql = "SELECT * FROM [Products] WHERE ISNULL([IsActive], 1) = 1";
        SqlCommand cmd = new SqlCommand(sql);
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public void DeleteProduct(string productCode)
    {
        try {
            string alterSql = "ALTER TABLE [Products] ADD [IsActive] BIT DEFAULT 1 WITH VALUES";
            DbActions.MyAction(new SqlCommand(alterSql), GetPath());
        } catch { }

        string sql = "UPDATE [Products] SET [IsActive] = 0 WHERE [ProductCode]=@p1";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = productCode;
        DbActions.MyAction(cmd, GetPath());
    }

    [WebMethod]
    public void UpdateProduct(string productCode, decimal price, string picture)
    {
        if (string.IsNullOrEmpty(picture))
        {
            string sql = "UPDATE [Products] SET [Price]=@p1 WHERE [ProductCode]=@p2";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Decimal)).Value = price;
            cmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar)).Value = productCode;
            DbActions.MyAction(cmd, GetPath());
        }
        else
        {
            string sql = "UPDATE [Products] SET [Price]=@p1, [Picture]=@p2 WHERE [ProductCode]=@p3";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Decimal)).Value = price;
            cmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar)).Value = picture;
            cmd.Parameters.Add(new SqlParameter("@p3", SqlDbType.NVarChar)).Value = productCode;
            DbActions.MyAction(cmd, GetPath());
        }
    }

    [WebMethod]
    public string PlaceOrder(string username, int eventId, OrderItem[] items)
    {
        if (string.IsNullOrWhiteSpace(username) || eventId <= 0 || items == null || items.Length == 0)
            throw new Exception("Invalid order data.");
            
        CreateStoreTables();

        string orderCode = "ORD-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        decimal total = 0;
        
        foreach(var item in items)
        {
            total += item.Price * item.Quantity; // Based on client side price passed in
        }

        string sqlOrder = "INSERT INTO [Orders] ([OrderCode], [Username], [EventId], [DatePurchased], [Total]) VALUES (@p1, @p2, @p3, @p4, @p5)";
        SqlCommand cmdOrd = new SqlCommand(sqlOrder);
        cmdOrd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = orderCode;
        cmdOrd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar)).Value = username;
        cmdOrd.Parameters.Add(new SqlParameter("@p3", SqlDbType.Int)).Value = eventId;
        cmdOrd.Parameters.Add(new SqlParameter("@p4", SqlDbType.DateTime)).Value = DateTime.Now;
        cmdOrd.Parameters.Add(new SqlParameter("@p5", SqlDbType.Decimal)).Value = total;
        DbActions.MyAction(cmdOrd, GetPath());

        foreach(var item in items)
        {
            string sqlItem = "INSERT INTO [OrderItems] ([OrderCode], [ProductCode], [Quantity]) VALUES (@p1, @p2, @p3)";
            SqlCommand cmdItem = new SqlCommand(sqlItem);
            cmdItem.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = orderCode;
            cmdItem.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar)).Value = item.ProductCode;
            cmdItem.Parameters.Add(new SqlParameter("@p3", SqlDbType.Int)).Value = item.Quantity;
            DbActions.MyAction(cmdItem, GetPath());
        }

        return orderCode;
    }

    [WebMethod]
    public DataTable GetMyOrders(string username)
    {
        CreateStoreTables();
        string sql = @"
            SELECT o.[OrderCode], o.[EventId], o.[DatePurchased], o.[Total], e.[EventDate], m.[Title] as MovieTitle
            FROM [Orders] o
            INNER JOIN [Events] e ON o.[EventId] = e.[EventId]
            INNER JOIN [Movies] m ON e.[MovieId] = m.[MovieId]
            WHERE o.[Username] = @p1
            ORDER BY o.[DatePurchased] DESC";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = username;
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public DataTable GetOrderItems(string orderCode)
    {
        string sql = @"
            SELECT oi.[Quantity], p.[Name] as ProductName, p.[Price]
            FROM [OrderItems] oi
            INNER JOIN [Products] p ON oi.[ProductCode] = p.[ProductCode]
            WHERE oi.[OrderCode] = @p1";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = orderCode;
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    //======================================================
    // EVENT STORE MANAGEMENT & ADMIN CONTROLS
    //======================================================
    [WebMethod]
    public void CreateEventProductsTable()
    {
        CreateStoreTables();
        string checkSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'EventProducts'";
        SqlCommand cmd = new SqlCommand(checkSql);
        DataTable dt = DbActions.SearchWithParameters(cmd, GetPath());
        int exists = (dt != null && dt.Rows.Count > 0) ? Convert.ToInt32(dt.Rows[0][0]) : 0;

        if (exists == 0)
        {
            string createSql = @"CREATE TABLE [EventProducts] (
                [Id] INT IDENTITY(1,1) PRIMARY KEY,
                [EventId] INT NOT NULL,
                [ProductCode] NVARCHAR(50) NOT NULL,
                FOREIGN KEY ([EventId]) REFERENCES [Events]([EventId]) ON DELETE CASCADE,
                FOREIGN KEY ([ProductCode]) REFERENCES [Products]([ProductCode]) ON DELETE CASCADE,
                UNIQUE([EventId], [ProductCode])
            )";
            SqlCommand cmmd = new SqlCommand(createSql);
            DbActions.MyAction(cmmd, GetPath());
        }
    }

    [WebMethod]
    public void AddProductToEvent(int eventId, string productCode)
    {
        CreateEventProductsTable();
        // check if already exists
        string checkSql = "SELECT COUNT(*) FROM [EventProducts] WHERE [EventId]=@p1 AND [ProductCode]=@p2";
        SqlCommand chkCmd = new SqlCommand(checkSql);
        chkCmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int)).Value = eventId;
        chkCmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar)).Value = productCode;
        DataTable dt = DbActions.SearchWithParameters(chkCmd, GetPath());
        if(dt != null && dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) > 0) return; // already exists

        string sql = "INSERT INTO [EventProducts] ([EventId], [ProductCode]) VALUES (@p1, @p2)";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int)).Value = eventId;
        cmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar)).Value = productCode;
        DbActions.MyAction(cmd, GetPath());
    }

    [WebMethod]
    public void RemoveProductFromEvent(int eventId, string productCode)
    {
        CreateEventProductsTable();
        string sql = "DELETE FROM [EventProducts] WHERE [EventId]=@p1 AND [ProductCode]=@p2";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int)).Value = eventId;
        cmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar)).Value = productCode;
        DbActions.MyAction(cmd, GetPath());
    }

    [WebMethod]
    public DataTable GetEventProducts(int eventId)
    {
        CreateEventProductsTable();
        string sql = @"
            SELECT p.* 
            FROM [Products] p 
            INNER JOIN [EventProducts] ep ON p.[ProductCode] = ep.[ProductCode] 
            WHERE ep.[EventId] = @p1 AND ISNULL(p.[IsActive], 1) = 1";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int)).Value = eventId;
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public DataTable GetProductsNotInEvent(int eventId)
    {
        CreateEventProductsTable();
        string sql = @"
            SELECT p.* 
            FROM [Products] p 
            WHERE ISNULL(p.[IsActive], 1) = 1 AND p.[ProductCode] NOT IN (
                SELECT [ProductCode] FROM [EventProducts] WHERE [EventId] = @p1
            )";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int)).Value = eventId;
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public DataTable GetEventOrders(int eventId)
    {
        CreateStoreTables();
        string sql = @"
            SELECT o.[OrderCode], o.[Username], o.[DatePurchased], o.[Total]
            FROM [Orders] o
            WHERE o.[EventId] = @p1
            ORDER BY o.[DatePurchased] DESC";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int)).Value = eventId;
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public DataTable GetAllOrders()
    {
        CreateStoreTables();
        string sql = @"
            SELECT o.[OrderCode], o.[EventId], o.[Username], o.[DatePurchased], o.[Total], 
                   e.[EventDate], m.[Title] as MovieTitle, e.[Username] as EventOwner
            FROM [Orders] o
            INNER JOIN [Events] e ON o.[EventId] = e.[EventId]
            INNER JOIN [Movies] m ON e.[MovieId] = m.[MovieId]
            ORDER BY o.[DatePurchased] DESC";
        SqlCommand cmd = new SqlCommand(sql);
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public void AddProduct(string name, decimal price, string description, string picture)
    {
        CreateStoreTables();
        string code = "PRD-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        string sqlIns = "INSERT INTO [Products] ([ProductCode], [Name], [Price], [Description], [Picture]) VALUES (@p1, @p2, @p3, @p4, @p5)";
        SqlCommand cmdIns = new SqlCommand(sqlIns);
        cmdIns.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = code;
        cmdIns.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar)).Value = name;
        cmdIns.Parameters.Add(new SqlParameter("@p3", SqlDbType.Decimal)).Value = price;
        cmdIns.Parameters.Add(new SqlParameter("@p4", SqlDbType.NVarChar)).Value = description;
        cmdIns.Parameters.Add(new SqlParameter("@p5", SqlDbType.NVarChar)).Value = picture;
        DbActions.MyAction(cmdIns, GetPath());
    }
}
