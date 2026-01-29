-- SQL Script to create Events and EventSubscriptions tables
-- Run this script in SQL Server Management Studio or execute via Service method

-- Events table
CREATE TABLE [Events] (
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
);

-- Event Subscriptions table
CREATE TABLE [EventSubscriptions] (
    [SubscriptionId] INT IDENTITY(1,1) PRIMARY KEY,
    [EventId] INT NOT NULL,
    [Username] NVARCHAR(50) NOT NULL,
    [SubscribedAt] DATETIME DEFAULT GETDATE(),
    FOREIGN KEY ([EventId]) REFERENCES [Events]([EventId]) ON DELETE CASCADE,
    UNIQUE([EventId], [Username])
);
