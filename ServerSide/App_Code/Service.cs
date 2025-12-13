//using MDb.App_Code;
//using System; 
//using System.Data;
//using System.Web.Services;
//using System.Web.UI.WebControls;
//using System.Data.SqlClient;
//using System.Data.OleDb;
using MDb.App_Code;
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

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]

public class Service : System.Web.Services.WebService
{
    public Service()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 

    }
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
        catch
        {
            // Swallow any logging errors to avoid masking the original exception
        }
    }

    //[WebMethod]
    //public void Check()
    //{
    //    DbActions.Search("", GetPath());
    //}

    //======================================================

    [WebMethod]
    public DataTable Login(string username, string password, bool Choice)
    {

        string Sql = "Select * from ";

        if (Choice == true) //user
            Sql += "[Users] where [User]='" + username.Replace("'", "''") + "' and [pass]='" + password.Replace("'", "''") + "'";
        else
            Sql += "[Admin] where [Usern]='" + username.Replace("'", "''") + "' and [Pass]='" + password.Replace("'", "''") + "'";

        DataTable dt= DbActions.Search(Sql, GetPath());

        return dt;
    }

    //======================================================

    [WebMethod]


    public void Regi(Users users)
    {
        string sql = "INSERT INTO [Users] values(@p1 , @p2 , @p3 , @p4 ,@p5 , @p6 ,@p7 ,@p8 ,@p9 ,@p10)";


        SqlCommand cmmd = new SqlCommand(sql);

        cmmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.VarChar));
        cmmd.Parameters["@p1"].Value = users.UserN;

        cmmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.VarChar));
        cmmd.Parameters["@p2"].Value = users.Pass;

        cmmd.Parameters.Add(new SqlParameter("@p3", SqlDbType.VarChar));
        cmmd.Parameters["@p3"].Value = users.LastN;

        cmmd.Parameters.Add(new SqlParameter("@p4", SqlDbType.VarChar));
        cmmd.Parameters["@p4"].Value = users.NameF;

        cmmd.Parameters.Add(new SqlParameter("@p5", SqlDbType.VarChar));
        cmmd.Parameters["@p5"].Value = users.Fulladdres;

        cmmd.Parameters.Add(new SqlParameter("@p6", SqlDbType.VarChar));
        cmmd.Parameters["@p6"].Value = users.Email;

        cmmd.Parameters.Add(new SqlParameter("@p7", SqlDbType.VarChar));
        cmmd.Parameters["@p7"].Value = users.PhoneN;

        cmmd.Parameters.Add(new SqlParameter("@p8", SqlDbType.VarChar));
        cmmd.Parameters["@p8"].Value = users.Gender;

        cmmd.Parameters.Add(new SqlParameter("@p9", SqlDbType.DateTime));
        DateTime birthdayDate;
        if (DateTime.TryParse(users.Birthday, out birthdayDate))
        {
            cmmd.Parameters["@p9"].Value = birthdayDate;
        }
        else
        {
            cmmd.Parameters["@p9"].Value = DBNull.Value;
        }

        cmmd.Parameters.Add(new SqlParameter("@p10", SqlDbType.VarChar));
        cmmd.Parameters["@p10"].Value = users.Pic;

        DbActions.MyAction(cmmd, GetPath());



    }
    //-----------------------------------------------------

    [WebMethod]
    public DataTable UpdateUser(Users user)
    {
        string sql = "Update [Users] SET [User]=@p1, [pass]=@p2, [FName]=@p3, [LName]=@p4, [address]=@p5,[email]=@p6,[phone]=@p7, [pic]=@p8 where [User]=@p9"; //שם הטבלה
        SqlCommand cmmd = new SqlCommand(sql);

        cmmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.VarChar));
        cmmd.Parameters["@p1"].Value = user.UserN;

        cmmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.VarChar));
        cmmd.Parameters["@p2"].Value = user.Pass;

        cmmd.Parameters.Add(new SqlParameter("@p3", SqlDbType.VarChar));
        cmmd.Parameters["@p3"].Value = user.NameF;

        cmmd.Parameters.Add(new SqlParameter("@p4", SqlDbType.VarChar));
        cmmd.Parameters["@p4"].Value = user.LastN;

        cmmd.Parameters.Add(new SqlParameter("@p5", SqlDbType.VarChar));
        cmmd.Parameters["@p5"].Value = user.Fulladdres;

        cmmd.Parameters.Add(new SqlParameter("@p6", SqlDbType.VarChar));
        cmmd.Parameters["@p6"].Value = user.Email;

        cmmd.Parameters.Add(new SqlParameter("@p7", SqlDbType.VarChar));
        cmmd.Parameters["@p7"].Value = user.PhoneN;

        cmmd.Parameters.Add(new SqlParameter("@p8", SqlDbType.VarChar));
        cmmd.Parameters["@p8"].Value = user.Pic;

        
       
        


        cmmd.Parameters.Add(new SqlParameter("@p9", SqlDbType.VarChar));
        cmmd.Parameters["@p9"].Value = user.UserN;

        DbActions.MyAction(cmmd, GetPath());
        sql = "Select * from [Users] where [User]='" + user.UserN.Replace("'", "''") + "'";
        return DbActions.Search(sql, GetPath());
    }

    //---------------------------------------------------------------------------------
    [WebMethod]
    public DataTable SearchUser(string data, string option)
    {
        string sql = "Select * from [Users] ";

        // If data is empty or null, return all users
        if (!string.IsNullOrEmpty(data) && !string.IsNullOrEmpty(option))
        {
            if (option.Equals("name"))
                sql += "WHERE [FName]='" + data.Replace("'", "''") + "'";
            else if (option.Equals("address"))
                sql += "WHERE [address]='" + data.Replace("'", "''") + "'";
            else if (option.Equals("username"))
                sql += "WHERE [User]='" + data.Replace("'", "''") + "'";
        }

        return DbActions.Search(sql, GetPath());
    }

    //---------------------------------------------------------------------------------
    // Movies Methods
    //---------------------------------------------------------------------------------

    [WebMethod]
    public void CreateMoviesTable()
    {
        // Check if table exists, if not create it
        string checkTableSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Movies'";
        DataTable dt = DbActions.Search(checkTableSql, GetPath());
        
        int tableExists = 0;
        if (dt != null && dt.Rows.Count > 0)
        {
            tableExists = Convert.ToInt32(dt.Rows[0][0]);
        }

        if (tableExists == 0)
        {
            // Create Movies table
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

            // Insert sample movies
            InsertSampleMovies();
        }
    }

    private void InsertSampleMovies()
    {
        Movies[] sampleMovies = new Movies[]
        {
            new Movies { Title = "Interstellar", Description = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.", Year = 2014, Genre = "Sci-Fi", Rating = 8.6m, Poster = "images/uploads/slider1.jpg", Director = "Christopher Nolan", Actors = "Matthew McConaughey, Anne Hathaway, Jessica Chastain", Duration = 169 },
            new Movies { Title = "The Revenant", Description = "A frontiersman on a fur trading expedition in the 1820s fights for survival after being mauled by a bear.", Year = 2015, Genre = "Drama", Rating = 8.0m, Poster = "images/uploads/slider2.jpg", Director = "Alejandro G. Iñárritu", Actors = "Leonardo DiCaprio, Tom Hardy, Will Poulter", Duration = 156 },
            new Movies { Title = "Die Hard", Description = "An NYPD officer tries to save his wife and several others taken hostage by German terrorists during a Christmas party.", Year = 1988, Genre = "Action", Rating = 8.2m, Poster = "images/uploads/slider3.jpg", Director = "John McTiernan", Actors = "Bruce Willis, Alan Rickman, Bonnie Bedelia", Duration = 132 },
            new Movies { Title = "The Walk", Description = "In 1974, high-wire artist Philippe Petit recruits a team of people to help him realize his dream: to walk the immense void between the World Trade Center towers.", Year = 2015, Genre = "Drama", Rating = 7.3m, Poster = "images/uploads/slider4.jpg", Director = "Robert Zemeckis", Actors = "Joseph Gordon-Levitt, Charlotte Le Bon, Guillaume Baillargeon", Duration = 123 }
        };

        foreach (Movies movie in sampleMovies)
        {
            AddMovieInternal(movie);
        }
    }

    // Internal method to add movies without admin check (for initialization only)
    private void AddMovieInternal(Movies movie)
    {
        string sql = "INSERT INTO [Movies] ([Title], [Description], [Year], [Genre], [Rating], [Poster], [Director], [Actors], [Duration]) VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9)";
        
        SqlCommand cmmd = new SqlCommand(sql);
        
        cmmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar));
        cmmd.Parameters["@p1"].Value = movie.Title ?? (object)DBNull.Value;
        
        cmmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar));
        cmmd.Parameters["@p2"].Value = movie.Description ?? (object)DBNull.Value;
        
        cmmd.Parameters.Add(new SqlParameter("@p3", SqlDbType.Int));
        cmmd.Parameters["@p3"].Value = movie.Year;
        
        cmmd.Parameters.Add(new SqlParameter("@p4", SqlDbType.NVarChar));
        cmmd.Parameters["@p4"].Value = movie.Genre ?? (object)DBNull.Value;
        
        cmmd.Parameters.Add(new SqlParameter("@p5", SqlDbType.Decimal));
        cmmd.Parameters["@p5"].Value = movie.Rating;
        
        cmmd.Parameters.Add(new SqlParameter("@p6", SqlDbType.NVarChar));
        cmmd.Parameters["@p6"].Value = movie.Poster ?? (object)DBNull.Value;
        
        cmmd.Parameters.Add(new SqlParameter("@p7", SqlDbType.NVarChar));
        cmmd.Parameters["@p7"].Value = movie.Director ?? (object)DBNull.Value;
        
        cmmd.Parameters.Add(new SqlParameter("@p8", SqlDbType.NVarChar));
        cmmd.Parameters["@p8"].Value = movie.Actors ?? (object)DBNull.Value;
        
        cmmd.Parameters.Add(new SqlParameter("@p9", SqlDbType.Int));
        cmmd.Parameters["@p9"].Value = movie.Duration;
        
        DbActions.MyAction(cmmd, GetPath());
    }

    [WebMethod]
    public DataTable GetAllMovies()
    {
        CreateMoviesTable(); // Ensure table exists
        string sql = "SELECT * FROM [Movies] ORDER BY [Year] DESC, [Title]";
        return DbActions.Search(sql, GetPath());
    }

    [WebMethod]
    public DataTable GetLatestMovies()
    {
        CreateMoviesTable(); // Ensure table exists
        string sql = "SELECT TOP 5 * FROM [Movies] ORDER BY [MovieId] DESC";
        return DbActions.Search(sql, GetPath());
    }

    [WebMethod]
    public DataTable SearchMovies(string searchTerm, string genre)
    {
        CreateMoviesTable(); // Ensure table exists
        string sql = "SELECT * FROM [Movies] WHERE 1=1";
        
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string escapedSearchTerm = searchTerm.Replace("'", "''");
            sql += " AND ([Title] LIKE '%" + escapedSearchTerm + "%' OR [Description] LIKE '%" + escapedSearchTerm + "%' OR [Director] LIKE '%" + escapedSearchTerm + "%' OR [Actors] LIKE '%" + escapedSearchTerm + "%')";
        }
        
        if (!string.IsNullOrWhiteSpace(genre) && genre.ToLower() != "all")
        {
            sql += " AND [Genre] = '" + genre.Replace("'", "''") + "'";
        }
        
        sql += " ORDER BY [Rating] DESC, [Year] DESC";
        
        return DbActions.Search(sql, GetPath());
    }

    [WebMethod]
    public DataTable GetMovieById(int movieId)
    {
        CreateMoviesTable(); // Ensure table exists
        string sql = "SELECT * FROM [Movies] WHERE [MovieId] = " + movieId;
        return DbActions.Search(sql, GetPath());
    }

    [WebMethod]
    public void AddMovie(Movies movie)
    {
        try
        {
            // Note: Admin authorization is performed on the client side before calling this service.
            // In a production environment, use Windows authentication or ASP.NET roles for better security.
            if (movie == null)
            {
                throw new Exception("Movie data is required.");
            }

            CreateMoviesTable(); // Ensure table exists
            
            string sql = "INSERT INTO [Movies] ([Title], [Description], [Year], [Genre], [Rating], [Poster], [Director], [Actors], [Duration]) VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9)";
            
            SqlCommand cmmd = new SqlCommand(sql);
            
            cmmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar));
            cmmd.Parameters["@p1"].Value = movie.Title ?? (object)DBNull.Value;
            
            cmmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar));
            cmmd.Parameters["@p2"].Value = movie.Description ?? (object)DBNull.Value;
            
            cmmd.Parameters.Add(new SqlParameter("@p3", SqlDbType.Int));
            cmmd.Parameters["@p3"].Value = movie.Year;
            
            cmmd.Parameters.Add(new SqlParameter("@p4", SqlDbType.NVarChar));
            cmmd.Parameters["@p4"].Value = movie.Genre ?? (object)DBNull.Value;
            
            cmmd.Parameters.Add(new SqlParameter("@p5", SqlDbType.Decimal));
            cmmd.Parameters["@p5"].Value = movie.Rating;
            
            cmmd.Parameters.Add(new SqlParameter("@p6", SqlDbType.NVarChar));
            cmmd.Parameters["@p6"].Value = movie.Poster ?? (object)DBNull.Value;
            
            cmmd.Parameters.Add(new SqlParameter("@p7", SqlDbType.NVarChar));
            cmmd.Parameters["@p7"].Value = movie.Director ?? (object)DBNull.Value;
            
            cmmd.Parameters.Add(new SqlParameter("@p8", SqlDbType.NVarChar));
            cmmd.Parameters["@p8"].Value = movie.Actors ?? (object)DBNull.Value;
            
            cmmd.Parameters.Add(new SqlParameter("@p9", SqlDbType.Int));
            cmmd.Parameters["@p9"].Value = movie.Duration;
            
            DbActions.MyAction(cmmd, GetPath());
        }
        catch (Exception ex)
        {
            LogError(ex);
            throw;
        }
    }

    [WebMethod]
    public void UpdateMovie(Movies movie)
    {
        try
        {
            if (movie == null || movie.MovieId <= 0)
            {
                throw new Exception("Valid movie data (with MovieId) is required for update.");
            }

            CreateMoviesTable(); // Ensure table exists

            string sql = @"UPDATE [Movies]
                           SET [Title]=@p1, [Description]=@p2, [Year]=@p3, [Genre]=@p4,
                               [Rating]=@p5, [Poster]=@p6, [Director]=@p7, [Actors]=@p8,
                               [Duration]=@p9
                           WHERE [MovieId]=@p10";

            SqlCommand cmmd = new SqlCommand(sql);

            cmmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar));
            cmmd.Parameters["@p1"].Value = movie.Title ?? (object)DBNull.Value;

            cmmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar));
            cmmd.Parameters["@p2"].Value = movie.Description ?? (object)DBNull.Value;

            cmmd.Parameters.Add(new SqlParameter("@p3", SqlDbType.Int));
            cmmd.Parameters["@p3"].Value = movie.Year;

            cmmd.Parameters.Add(new SqlParameter("@p4", SqlDbType.NVarChar));
            cmmd.Parameters["@p4"].Value = movie.Genre ?? (object)DBNull.Value;

            cmmd.Parameters.Add(new SqlParameter("@p5", SqlDbType.Decimal));
            cmmd.Parameters["@p5"].Value = movie.Rating;

            cmmd.Parameters.Add(new SqlParameter("@p6", SqlDbType.NVarChar));
            cmmd.Parameters["@p6"].Value = movie.Poster ?? (object)DBNull.Value;

            cmmd.Parameters.Add(new SqlParameter("@p7", SqlDbType.NVarChar));
            cmmd.Parameters["@p7"].Value = movie.Director ?? (object)DBNull.Value;

            cmmd.Parameters.Add(new SqlParameter("@p8", SqlDbType.NVarChar));
            cmmd.Parameters["@p8"].Value = movie.Actors ?? (object)DBNull.Value;

            cmmd.Parameters.Add(new SqlParameter("@p9", SqlDbType.Int));
            cmmd.Parameters["@p9"].Value = movie.Duration;

            cmmd.Parameters.Add(new SqlParameter("@p10", SqlDbType.Int));
            cmmd.Parameters["@p10"].Value = movie.MovieId;

            DbActions.MyAction(cmmd, GetPath());
        }
        catch (Exception ex)
        {
            LogError(ex);
            throw;
        }
    }

    [WebMethod]
    public void DeleteMovie(int movieId)
    {
        try
        {
            if (movieId <= 0)
            {
                throw new Exception("Valid MovieId is required for delete.");
            }

            CreateMoviesTable(); // Ensure table exists

            string sql = "DELETE FROM [Movies] WHERE [MovieId]=@p1";
            SqlCommand cmmd = new SqlCommand(sql);
            cmmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int));
            cmmd.Parameters["@p1"].Value = movieId;

            DbActions.MyAction(cmmd, GetPath());
        }
        catch (Exception ex)
        {
            LogError(ex);
            throw;
        }
    }

    [WebMethod]
    public DataTable GetMoviesByGenre(string genre)
    {
        CreateMoviesTable(); // Ensure table exists
        string sql = "SELECT * FROM [Movies] WHERE [Genre] = '" + genre.Replace("'", "''") + "' ORDER BY [Rating] DESC, [Year] DESC";
        return DbActions.Search(sql, GetPath());
    }

    //---------------------------------------------------------------------------------
    // Wishlist Methods
    //---------------------------------------------------------------------------------

    private void CreateWishlistTable()
    {
        string checkTableSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Wishlist'";
        DataTable dt = DbActions.Search(checkTableSql, GetPath());

        int tableExists = 0;
        if (dt != null && dt.Rows.Count > 0)
        {
            tableExists = Convert.ToInt32(dt.Rows[0][0]);
        }

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

    private void AddToWishlistInternal(string username, int movieId)
    {
        CreateMoviesTable();
        CreateWishlistTable();

        string checkSql = "SELECT COUNT(*) FROM [Wishlist] WHERE [Username]=@u AND [MovieId]=@m";
        SqlCommand checkCmd = new SqlCommand(checkSql);
        checkCmd.Parameters.Add(new SqlParameter("@u", SqlDbType.NVarChar));
        checkCmd.Parameters["@u"].Value = username ?? string.Empty;
        checkCmd.Parameters.Add(new SqlParameter("@m", SqlDbType.Int));
        checkCmd.Parameters["@m"].Value = movieId;

        DataTable dt = DbActions.Search(checkSql.Replace("@u", "'" + username.Replace("'", "''") + "'").Replace("@m", movieId.ToString()), GetPath());
        int exists = 0;
        if (dt != null && dt.Rows.Count > 0)
        {
            exists = Convert.ToInt32(dt.Rows[0][0]);
        }

        if (exists == 0)
        {
            string insertSql = "INSERT INTO [Wishlist] ([Username], [MovieId]) VALUES (@p1, @p2)";
            SqlCommand insertCmd = new SqlCommand(insertSql);
            insertCmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar));
            insertCmd.Parameters["@p1"].Value = username ?? string.Empty;
            insertCmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.Int));
            insertCmd.Parameters["@p2"].Value = movieId;

            DbActions.MyAction(insertCmd, GetPath());
        }
    }

    private void RemoveFromWishlistInternal(string username, int movieId)
    {
        CreateWishlistTable();

        string deleteSql = "DELETE FROM [Wishlist] WHERE [Username]=@p1 AND [MovieId]=@p2";
        SqlCommand cmd = new SqlCommand(deleteSql);
        cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar));
        cmd.Parameters["@p1"].Value = username ?? string.Empty;
        cmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.Int));
        cmd.Parameters["@p2"].Value = movieId;

        DbActions.MyAction(cmd, GetPath());
    }

    [WebMethod]
    public void AddToWishlist(string username, int movieId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(username) || movieId <= 0)
            {
                throw new Exception("Username and valid MovieId are required.");
            }

            AddToWishlistInternal(username, movieId);
        }
        catch (Exception ex)
        {
            LogError(ex);
            throw;
        }
    }

    [WebMethod]
    public void RemoveFromWishlist(string username, int movieId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(username) || movieId <= 0)
            {
                throw new Exception("Username and valid MovieId are required.");
            }

            RemoveFromWishlistInternal(username, movieId);
        }
        catch (Exception ex)
        {
            LogError(ex);
            throw;
        }
    }

    [WebMethod]
    public DataTable GetWishlistMovies(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new Exception("Username is required.");
        }

        CreateMoviesTable();
        CreateWishlistTable();

        string safeUsername = username.Replace("'", "''");
        string sql = @"SELECT m.* FROM [Movies] m
                        INNER JOIN [Wishlist] w ON m.[MovieId] = w.[MovieId]
                        WHERE w.[Username] = '" + safeUsername + "'";

        return DbActions.Search(sql, GetPath());
    }

    //---------------------------------------------------------------------------------
    // Movie Comments Methods
    //---------------------------------------------------------------------------------

    private void CreateMovieCommentsTable()
    {
        string checkTableSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'MovieComments'";
        DataTable dt = DbActions.Search(checkTableSql, GetPath());

        int tableExists = 0;
        if (dt != null && dt.Rows.Count > 0)
        {
            tableExists = Convert.ToInt32(dt.Rows[0][0]);
        }

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
        try
        {
            if (string.IsNullOrWhiteSpace(username) || movieId <= 0 || string.IsNullOrWhiteSpace(commentText))
            {
                throw new Exception("Username, movie and comment text are required.");
            }

            if (rating < 1 || rating > 5)
            {
                rating = 0;
            }

            CreateMoviesTable();
            CreateMovieCommentsTable();

            string sql = @"INSERT INTO [MovieComments] ([MovieId], [Username], [Rating], [CommentText], [CreatedAt])
                           VALUES (@p1, @p2, @p3, @p4, @p5)";

            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int));
            cmd.Parameters["@p1"].Value = movieId;
            cmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar));
            cmd.Parameters["@p2"].Value = username;
            cmd.Parameters.Add(new SqlParameter("@p3", SqlDbType.Int));
            cmd.Parameters["@p3"].Value = rating;
            cmd.Parameters.Add(new SqlParameter("@p4", SqlDbType.NVarChar));
            cmd.Parameters["@p4"].Value = commentText;
            cmd.Parameters.Add(new SqlParameter("@p5", SqlDbType.DateTime));
            cmd.Parameters["@p5"].Value = DateTime.Now;

            DbActions.MyAction(cmd, GetPath());
        }
        catch (Exception ex)
        {
            LogError(ex);
            throw;
        }
    }

    [WebMethod]
    public DataTable GetMovieComments(int movieId)
    {
        if (movieId <= 0)
        {
            throw new Exception("Valid MovieId is required.");
        }

        CreateMovieCommentsTable();

        string sql = @"SELECT [CommentId], [MovieId], [Username], [Rating], [CommentText], [CreatedAt]
                       FROM [MovieComments]
                       WHERE [MovieId] = " + movieId + " ORDER BY [CreatedAt] DESC";

        return DbActions.Search(sql, GetPath());
    }

    //---------------------------------------------------------------------------------
    // Celebs Methods
    //---------------------------------------------------------------------------------

    [WebMethod]
    public void CreateCelebsTable()
    {
        string checkTableSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Celebs'";
        DataTable dt = DbActions.Search(checkTableSql, GetPath());

        int tableExists = 0;
        if (dt != null && dt.Rows.Count > 0)
        {
            tableExists = Convert.ToInt32(dt.Rows[0][0]);
        }

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
        Celeb[] sampleCelebs = new Celeb[]
        {
            new Celeb { Name = "Leonardo DiCaprio", Role = "Actor", Photo = "images/uploads/ava1.jpg", Bio = "Academy Award-winning actor known for films such as Inception, The Revenant, and Titanic." },
            new Celeb { Name = "Anne Hathaway", Role = "Actress", Photo = "images/uploads/ava2.jpg", Bio = "American actress known for roles in The Devil Wears Prada, Les Misérables, and Interstellar." },
            new Celeb { Name = "Christopher Nolan", Role = "Director", Photo = "images/uploads/ava3.jpg", Bio = "British-American film director, producer, and screenwriter known for The Dark Knight trilogy and Inception." },
            new Celeb { Name = "Tom Hardy", Role = "Actor", Photo = "images/uploads/ava4.jpg", Bio = "English actor known for roles in Inception, Mad Max: Fury Road, and The Revenant." }
        };

        foreach (Celeb celeb in sampleCelebs)
        {
            AddCelebInternal(celeb);
        }
    }

    private void AddCelebInternal(Celeb celeb)
    {
        string sql = "INSERT INTO [Celebs] ([Name], [Role], [Photo], [Bio]) VALUES (@p1, @p2, @p3, @p4)";

        SqlCommand cmmd = new SqlCommand(sql);

        cmmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar));
        cmmd.Parameters["@p1"].Value = celeb.Name ?? (object)DBNull.Value;

        cmmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar));
        cmmd.Parameters["@p2"].Value = celeb.Role ?? (object)DBNull.Value;

        cmmd.Parameters.Add(new SqlParameter("@p3", SqlDbType.NVarChar));
        cmmd.Parameters["@p3"].Value = celeb.Photo ?? (object)DBNull.Value;

        cmmd.Parameters.Add(new SqlParameter("@p4", SqlDbType.NVarChar));
        cmmd.Parameters["@p4"].Value = celeb.Bio ?? (object)DBNull.Value;

        DbActions.MyAction(cmmd, GetPath());
    }

    [WebMethod]
    public DataTable GetAllCelebs()
    {
        CreateCelebsTable(); // Ensure table exists
        string sql = "SELECT * FROM [Celebs] ORDER BY [Name]";
        return DbActions.Search(sql, GetPath());
    }

    [WebMethod]
    public DataTable GetLatestCelebs()
    {
        CreateCelebsTable(); // Ensure table exists
        string sql = "SELECT TOP 5 * FROM [Celebs] ORDER BY [CelebId] DESC";
        return DbActions.Search(sql, GetPath());
    }

    [WebMethod]
    public DataTable SearchCelebs(string searchText, string role)
    {
        CreateCelebsTable(); // Ensure table exists

        string sql = "SELECT * FROM [Celebs] WHERE 1=1";

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            string escaped = searchText.Replace("'", "''");
            sql += " AND ([Name] LIKE '%" + escaped + "%' OR [Bio] LIKE '%" + escaped + "%')";
        }

        if (!string.IsNullOrWhiteSpace(role) && role.ToLower() != "all")
        {
            sql += " AND [Role] = '" + role.Replace("'", "''") + "'";
        }

        sql += " ORDER BY [Name]";

        return DbActions.Search(sql, GetPath());
    }

    [WebMethod]
    public void AddCeleb(Celeb celeb)
    {
        try
        {
            if (celeb == null)
            {
                throw new Exception("Celeb data is required.");
            }

            CreateCelebsTable(); // Ensure table exists

            string sql = "INSERT INTO [Celebs] ([Name], [Role], [Photo], [Bio]) VALUES (@p1, @p2, @p3, @p4)";

            SqlCommand cmmd = new SqlCommand(sql);

            cmmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar));
            cmmd.Parameters["@p1"].Value = celeb.Name ?? (object)DBNull.Value;

            cmmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar));
            cmmd.Parameters["@p2"].Value = celeb.Role ?? (object)DBNull.Value;

            cmmd.Parameters.Add(new SqlParameter("@p3", SqlDbType.NVarChar));
            cmmd.Parameters["@p3"].Value = celeb.Photo ?? (object)DBNull.Value;

            cmmd.Parameters.Add(new SqlParameter("@p4", SqlDbType.NVarChar));
            cmmd.Parameters["@p4"].Value = celeb.Bio ?? (object)DBNull.Value;

            DbActions.MyAction(cmmd, GetPath());
        }
        catch (Exception ex)
        {
            LogError(ex);
            throw;
        }
    }

    [WebMethod]
    public void UpdateCeleb(Celeb celeb)
    {
        try
        {
            if (celeb == null || celeb.CelebId <= 0)
            {
                throw new Exception("Valid celeb data (with CelebId) is required for update.");
            }

            CreateCelebsTable(); // Ensure table exists

            string sql = "UPDATE [Celebs] SET [Name]=@p1, [Role]=@p2, [Photo]=@p3, [Bio]=@p4 WHERE [CelebId]=@p5";

            SqlCommand cmmd = new SqlCommand(sql);

            cmmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar));
            cmmd.Parameters["@p1"].Value = celeb.Name ?? (object)DBNull.Value;

            cmmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.NVarChar));
            cmmd.Parameters["@p2"].Value = celeb.Role ?? (object)DBNull.Value;

            cmmd.Parameters.Add(new SqlParameter("@p3", SqlDbType.NVarChar));
            cmmd.Parameters["@p3"].Value = celeb.Photo ?? (object)DBNull.Value;

            cmmd.Parameters.Add(new SqlParameter("@p4", SqlDbType.NVarChar));
            cmmd.Parameters["@p4"].Value = celeb.Bio ?? (object)DBNull.Value;

            cmmd.Parameters.Add(new SqlParameter("@p5", SqlDbType.Int));
            cmmd.Parameters["@p5"].Value = celeb.CelebId;

            DbActions.MyAction(cmmd, GetPath());
        }
        catch (Exception ex)
        {
            LogError(ex);
            throw;
        }
    }

    [WebMethod]
    public void DeleteCeleb(int celebId)
    {
        try
        {
            CreateCelebsTable(); // Ensure table exists

            string sql = "DELETE FROM [Celebs] WHERE [CelebId]=@p1";
            SqlCommand cmmd = new SqlCommand(sql);
            cmmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.Int));
            cmmd.Parameters["@p1"].Value = celebId;

            DbActions.MyAction(cmmd, GetPath());
        }
        catch (Exception ex)
        {
            LogError(ex);
            throw;
        }
    }




    [WebMethod]
    public DataTable GetMovies()
    {
        string sql = "select * from [Movies]";
        return DbActions.Search(sql, GetPath());
    }
}
