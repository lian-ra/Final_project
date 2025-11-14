<%@ Page Title="News" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="News.aspx.cs" Inherits="News" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .news-wrapper {
            padding: 40px 20px;
            color: white;
        }
        .news-title {
            text-align: center;
            font-size: 36px;
            margin-bottom: 10px;
        }
        .news-subtitle {
            text-align: center;
            font-size: 16px;
            opacity: 0.8;
            margin-bottom: 30px;
        }
        .news-section-title {
            font-size: 24px;
            margin: 25px 0 15px 0;
            border-bottom: 1px solid rgba(255,255,255,0.2);
            padding-bottom: 5px;
        }
        .news-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
            gap: 20px;
        }
        .news-card {
            background: rgba(0, 0, 0, 0.6);
            border-radius: 8px;
            overflow: hidden;
            border: 1px solid rgba(255, 255, 255, 0.1);
        }
        .news-card img {
            width: 100%;
            height: 220px;
            object-fit: cover;
            display: block;
        }
        .news-card-body {
            padding: 10px 14px 14px 14px;
            color: white;
        }
        .news-card-title {
            font-size: 16px;
            font-weight: bold;
            margin-bottom: 4px;
        }
        .news-card-meta {
            font-size: 13px;
            opacity: 0.8;
            margin-bottom: 6px;
        }
        .news-card-text {
            font-size: 13px;
            line-height: 1.4;
            max-height: 60px;
            overflow: hidden;
            text-overflow: ellipsis;
        }
        .news-empty {
            text-align: center;
            color: white;
            padding: 25px 0;
            opacity: 0.8;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="news-wrapper">
        <h1 class="news-title">Latest News</h1>
        <div class="news-subtitle">See the newest movies and celebrities added to the site</div>

        <h2 class="news-section-title">Newest Movies</h2>
        <div class="news-grid">
            <asp:PlaceHolder ID="phLatestMovies" runat="server"></asp:PlaceHolder>
        </div>

        <h2 class="news-section-title">Newest Celebs</h2>
        <div class="news-grid">
            <asp:PlaceHolder ID="phLatestCelebs" runat="server"></asp:PlaceHolder>
        </div>
    </div>
</asp:Content>
