<%@ Page Title="Movie Details" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="MovieDetails.aspx.cs" Inherits="MovieDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .details-wrapper {
            padding: 40px 20px;
            color: white;
        }
        .details-layout {
            display: flex;
            flex-wrap: wrap;
            gap: 30px;
        }
        .details-poster {
            flex: 0 0 260px;
        }
        .details-poster img {
            width: 100%;
            max-width: 260px;
            border-radius: 8px;
            object-fit: cover;
        }
        .details-main {
            flex: 1 1 300px;
        }
        .details-title {
            font-size: 32px;
            margin-bottom: 8px;
        }
        .details-meta {
            font-size: 14px;
            opacity: 0.8;
            margin-bottom: 15px;
        }
        .details-rating {
            font-size: 16px;
            color: #ffd700;
            margin-bottom: 15px;
        }
        .details-section-title {
            font-size: 18px;
            margin-top: 20px;
            margin-bottom: 8px;
            border-bottom: 1px solid rgba(255,255,255,0.2);
            padding-bottom: 4px;
        }
        .details-description {
            font-size: 14px;
            line-height: 1.6;
        }
        .details-actors {
            font-size: 14px;
            line-height: 1.5;
        }
        .actor-badge {
            display: inline-block;
            margin: 3px 6px 3px 0;
            padding: 4px 10px;
            border-radius: 15px;
            background: rgba(255,255,255,0.1);
            border: 1px solid rgba(255,255,255,0.2);
            font-size: 13px;
        }
        .details-actions {
            margin-top: 25px;
        }
        .details-actions a {
            display: inline-block;
            margin-right: 10px;
            padding: 8px 18px;
            border-radius: 20px;
            text-decoration: none;
            font-size: 13px;
        }
        .btn-back {
            background: transparent;
            border: 1px solid #fff;
            color: #fff;
        }
        .btn-wishlist {
            background: #ff6b6b;
            border: none;
            color: #fff;
        }
        .details-error {
            text-align: center;
            padding: 40px 0;
            color: white;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="details-wrapper">
        <asp:PlaceHolder ID="phDetails" runat="server"></asp:PlaceHolder>
    </div>
</asp:Content>
