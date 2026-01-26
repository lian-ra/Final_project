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

    [WebMethod]
    public DataTable Login(string username, string password, bool Choice)
    {
        string Sql = "Select * from ";
        if (Choice == true) //user
            Sql += "[Users] where [User]='" + username.Replace("'", "''") + "' and [pass]='" + password.Replace("'", "''") + "'";
        else
            Sql += "[Admin] where [Usern]='" + username.Replace("'", "''") + "' and [Pass]='" + password.Replace("'", "''") + "'";

        SqlCommand cmd = new SqlCommand(Sql);
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

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

    private void InsertSampleMovies()
    {
        Movies[] sampleMovies = new Movies[] {
            new Movies { Title = "Interstellar", Description = "Explorers travel through a wormhole...", Year = 2014, Genre = "Sci-Fi", Rating = 8.6m, Poster = "images/uploads/slider1.jpg", Director = "Christopher Nolan", Actors = "Matthew McConaughey", Duration = 169 },
            new Movies { Title = "The Revenant", Description = "A frontiersman fights for survival...", Year = 2015, Genre = "Drama", Rating = 8.0m, Poster = "images/uploads/slider2.jpg", Director = "Alejandro G. Iñárritu", Actors = "Leonardo DiCaprio", Duration = 156 },
            new Movies { Title = "Die Hard", Description = "NYPD officer saves wife...", Year = 1988, Genre = "Action", Rating = 8.2m, Poster = "images/uploads/slider3.jpg", Director = "John McTiernan", Actors = "Bruce Willis", Duration = 132 },
            new Movies { Title = "The Walk", Description = "High-wire artist...", Year = 2015, Genre = "Drama", Rating = 7.3m, Poster = "images/uploads/slider4.jpg", Director = "Robert Zemeckis", Actors = "Joseph Gordon-Levitt", Duration = 123 }
        };
        foreach (Movies movie in sampleMovies) AddMovieInternal(movie);
    }

    private void AddMovieInternal(Movies movie)
    {
        string sql = "INSERT INTO [Movies] ([Title], [Description], [Year], [Genre], [Rating], [Poster], [Director], [Actors], [Duration]) VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9)";
        SqlCommand cmmd = new SqlCommand(sql);
        cmmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = movie.Title ?? (object)DBNull.Value;
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
        CreateMoviesTable();
        string sql = "SELECT * FROM [Movies] ORDER BY [Year] DESC, [Title]";
        SqlCommand cmd = new SqlCommand(sql);
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public DataTable GetMovies()
    {
        CreateMoviesTable();
        string sql = "SELECT * FROM [Movies]";
        SqlCommand cmd = new SqlCommand(sql);
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public DataTable GetLatestMovies()
    {
        CreateMoviesTable();
        string sql = "SELECT TOP 5 * FROM [Movies] ORDER BY [MovieId] DESC";
        SqlCommand cmd = new SqlCommand(sql);
        return DbActions.SearchWithParameters(cmd, GetPath());
    }

    [WebMethod]
    public DataTable SearchMovies(string searchTerm, string genre)
    {
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

    //======================================================
    // COMMENTS
    //======================================================

    private void CreateMovieCommentsTable()
    {
        string checkTableSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'MovieComments'";
        SqlCommand cmd = new SqlCommand(checkTableSql);
        DataTable dt = DbActions.SearchWithParameters(cmd, GetPath());
        int tableExists = (dt != null && dt.Rows.Count > 0) ? Convert.ToInt32(dt.Rows[0][0]) : 0;

        if (tableExists == 0)
        {
            string createTableSql = @"CREATE TABLE [MovieComments] (
                [CommentId] INT IDENTITY(1,1) PRIMARY KEY,
                [MovieId] INT NOT NULL,
                [Username] NVARCHAR(255) NOT NULL,
                [Rating] INT NOT NULL,
                [CommentText] NVARCHAR(MAX) NOT NULL,
                [CreatedAt] DATETIME NOT NULL
            )";
            SqlCommand cmmd = new SqlCommand(createTableSql);
            DbActions.MyAction(cmmd, GetPath());
        }
    }

    [WebMethod]
    public void AddMovieComment(string username, int movieId, int rating, string commentText)
    {
        if (string.IsNullOrWhiteSpace(username) || movieId <= 0 || string.IsNullOrWhiteSpace(commentText)) throw new Exception("Data required.");
        CreateMoviesTable();
        CreateMovieCommentsTable();

        string sql = "INSERT INTO [MovieComments] ([MovieId], [Username], [Rating], [CommentText], [CreatedAt]) VALUES (@p1, @p2, @p3, @p4, @p5)";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int)).Value = movieId;
        cmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar)).Value = username;
        cmd.Parameters.Add(new SqlParameter("@p3", SqlDbType.Int)).Value = rating;
        cmd.Parameters.Add(new SqlParameter("@p4", SqlDbType.NVarChar)).Value = commentText;
        cmd.Parameters.Add(new SqlParameter("@p5", SqlDbType.DateTime)).Value = DateTime.Now;
        DbActions.MyAction(cmd, GetPath());
    }

    [WebMethod]
    public DataTable GetMovieComments(int movieId)
    {
        CreateMovieCommentsTable();
        string sql = "SELECT * FROM [MovieComments] WHERE [MovieId] = @p1 ORDER BY [CreatedAt] DESC";
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
    public DataTable GetLatestCelebs()
    {
        CreateCelebsTable();
        string sql = "SELECT TOP 5 * FROM [Celebs] ORDER BY [CelebId] DESC";
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

    // Helper to fix the "my_db.GetOneField" error you saw. 
    // Now we use DbActions.SearchWithParameters just like Login() method.

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

}