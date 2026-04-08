using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Movies
/// </summary>
public class Movies
{
    public Movies()
    {
   
    }

    private int movieId;
    public int MovieId
    {
        get { return movieId; }
        set { movieId = value; }
    }

    private string title;
    public string Title
    {
        get { return title; }
        set { title = value; }
    }

    private string description;
    public string Description
    {
        get { return description; }
        set { description = value; }
    }

    private int year;
    public int Year
    {
        get { return year; }
        set { year = value; }
    }

    private string genre;
    public string Genre
    {
        get { return genre; }
        set { genre = value; }
    }

    private decimal rating;
    public decimal Rating
    {
        get { return rating; }
        set { rating = value; }
    }

    private string poster;
    public string Poster
    {
        get { return poster; }
        set { poster = value; }
    }

    private string director;
    public string Director
    {
        get { return director; }
        set { director = value; }
    }

    private string actors;
    public string Actors
    {
        get { return actors; }
        set { actors = value; }
    }

    private int duration;
    public int Duration
    {
        get { return duration; }
        set { duration = value; }
    }
}

