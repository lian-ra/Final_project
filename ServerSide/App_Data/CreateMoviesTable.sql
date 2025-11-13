-- SQL Script to create Movies table
-- Run this script in SQL Server Management Studio or execute via Service method

CREATE TABLE [Movies] (
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
);

-- Insert sample movies
INSERT INTO [Movies] ([Title], [Description], [Year], [Genre], [Rating], [Poster], [Director], [Actors], [Duration])
VALUES 
('Interstellar', 'A team of explorers travel through a wormhole in space in an attempt to ensure humanity''s survival.', 2014, 'Sci-Fi', 8.6, 'images/uploads/slider1.jpg', 'Christopher Nolan', 'Matthew McConaughey, Anne Hathaway, Jessica Chastain', 169),
('The Revenant', 'A frontiersman on a fur trading expedition in the 1820s fights for survival after being mauled by a bear.', 2015, 'Drama', 8.0, 'images/uploads/slider2.jpg', 'Alejandro G. Iñárritu', 'Leonardo DiCaprio, Tom Hardy, Will Poulter', 156),
('Die Hard', 'An NYPD officer tries to save his wife and several others taken hostage by German terrorists during a Christmas party.', 1988, 'Action', 8.2, 'images/uploads/slider3.jpg', 'John McTiernan', 'Bruce Willis, Alan Rickman, Bonnie Bedelia', 132),
('The Walk', 'In 1974, high-wire artist Philippe Petit recruits a team of people to help him realize his dream: to walk the immense void between the World Trade Center towers.', 2015, 'Drama', 7.3, 'images/uploads/slider4.jpg', 'Robert Zemeckis', 'Joseph Gordon-Levitt, Charlotte Le Bon, Guillaume Baillargeon', 123),
('The Dark Knight', 'When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests.', 2008, 'Action', 9.0, 'images/uploads/slider1.jpg', 'Christopher Nolan', 'Christian Bale, Heath Ledger, Aaron Eckhart', 152),
('Inception', 'A thief who steals corporate secrets through dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.', 2010, 'Sci-Fi', 8.8, 'images/uploads/slider2.jpg', 'Christopher Nolan', 'Leonardo DiCaprio, Marion Cotillard, Tom Hardy', 148),
('The Shawshank Redemption', 'Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.', 1994, 'Drama', 9.3, 'images/uploads/slider3.jpg', 'Frank Darabont', 'Tim Robbins, Morgan Freeman, Bob Gunton', 142),
('Pulp Fiction', 'The lives of two mob hitmen, a boxer, a gangster and his wife, and a pair of diner bandits intertwine in four tales of violence and redemption.', 1994, 'Drama', 8.9, 'images/uploads/slider4.jpg', 'Quentin Tarantino', 'John Travolta, Uma Thurman, Samuel L. Jackson', 154);

