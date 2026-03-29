IF OBJECT_ID('UserMovieInteractions', 'U') IS NULL
BEGIN
    CREATE TABLE [UserMovieInteractions] (
        [InteractionId] INT IDENTITY(1,1) PRIMARY KEY,
        [Username] NVARCHAR(255) NOT NULL,
        [MovieId] INT NOT NULL,
        [IsWatched] BIT DEFAULT 0,
        [Rating] INT NULL,
        [CommentText] NVARCHAR(MAX) NULL,
        [CreatedAt] DATETIME DEFAULT GETDATE(),
        CONSTRAINT UQ_User_Movie UNIQUE ([Username], [MovieId])
    );
END

IF OBJECT_ID('Watched', 'U') IS NOT NULL
BEGIN
    INSERT INTO [UserMovieInteractions] ([Username], [MovieId], [IsWatched])
    SELECT [Username], [MovieId], 1
    FROM [Watched] w
    WHERE NOT EXISTS (
        SELECT 1 FROM [UserMovieInteractions] umi 
        WHERE umi.[Username] = w.[Username] AND umi.[MovieId] = w.[MovieId]
    );
    DROP TABLE [Watched];
END

IF OBJECT_ID('MovieComments', 'U') IS NOT NULL
BEGIN
    MERGE INTO [UserMovieInteractions] AS target
    USING (
        SELECT [Username], [MovieId], [Rating], [CommentText], [CreatedAt]
        FROM (
            SELECT [Username], [MovieId], [Rating], [CommentText], [CreatedAt],
                   ROW_NUMBER() OVER(PARTITION BY [Username], [MovieId] ORDER BY [CreatedAt] DESC) as rn
            FROM [MovieComments]
        ) t WHERE t.rn = 1
    ) AS source (Username, MovieId, Rating, CommentText, CreatedAt)
    ON (target.Username = source.Username AND target.MovieId = source.MovieId)
    WHEN MATCHED THEN
        UPDATE SET 
            Rating = source.Rating, 
            CommentText = source.CommentText, 
            CreatedAt = source.CreatedAt
    WHEN NOT MATCHED THEN
        INSERT ([Username], [MovieId], [Rating], [CommentText], [CreatedAt])
        VALUES (source.Username, source.MovieId, source.Rating, source.CommentText, source.CreatedAt);

    DROP TABLE [MovieComments];
END
