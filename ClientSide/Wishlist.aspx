<%@ Page Title="Wishlist" Language="C#" MasterPageFile="~/Design.master"
    AutoEventWireup="true" CodeFile="Wishlist.aspx.cs" Inherits="Wishlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .wishlist-container {
            padding: 40px 20px;
            color: white;
        }
        .wishlist-header {
            text-align: center;
            margin-bottom: 30px;
        }
        .wishlist-header h1 {
            font-size: 36px;
            margin-bottom: 10px;
        }
        .wishlist-header p {
            font-size: 16px;
            opacity: 0.8;
        }
        .wishlist-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
            gap: 24px;
            margin-top: 20px;
        }
        .wishlist-card {
            background: rgba(255, 255, 255, 0.1);
            border-radius: 10px;
            padding: 15px;
            text-align: center;
        }
        .wishlist-poster {
            width: 100%;
            height: 260px;
            object-fit: cover;
            border-radius: 8px;
            margin-bottom: 10px;
        }
        .wishlist-title {
            font-size: 18px;
            font-weight: bold;
            margin-bottom: 5px;
        }
        .wishlist-meta {
            font-size: 14px;
            margin-bottom: 8px;
            color: #ffd700;
        }
        .wishlist-remove {
            display: inline-block;
            margin-top: 5px;
            padding: 6px 14px;
            border-radius: 20px;
            background-color: #ff6b6b;
            color: white;
            text-decoration: none;
            font-size: 13px;
        }
        .wishlist-empty {
            text-align: center;
            padding: 40px 0;
            opacity: 0.8;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wishlist-container">
        <div class="wishlist-header">
            <h1>Your Wishlist</h1>
            <p>Movies you saved to watch later</p>
        </div>

        <div class="wishlist-grid">
            <asp:PlaceHolder ID="phWishlist" runat="server"></asp:PlaceHolder>
        </div>
    </div>
</asp:Content>
