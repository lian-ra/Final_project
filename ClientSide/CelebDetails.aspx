<%@ Page Title="Celeb Details" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="CelebDetails.aspx.cs" Inherits="CelebDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .celeb-details-wrapper {
            padding: 40px 20px;
            color: white;
        }
        .celeb-details-layout {
            display: flex;
            flex-wrap: wrap;
            gap: 30px;
        }
        .celeb-details-photo {
            flex: 0 0 260px;
        }
        .celeb-details-photo img {
            width: 100%;
            max-width: 260px;
            border-radius: 8px;
            object-fit: cover;
        }
        .celeb-details-main {
            flex: 1 1 300px;
        }
        .celeb-details-name {
            font-size: 32px;
            margin-bottom: 8px;
        }
        .celeb-details-role {
            font-size: 16px;
            opacity: 0.8;
            margin-bottom: 15px;
        }
        .celeb-details-bio {
            font-size: 14px;
            line-height: 1.6;
        }
        .celeb-details-actions {
            margin-top: 20px;
        }
        .celeb-details-actions a {
            display: inline-block;
            margin-right: 10px;
            padding: 8px 18px;
            border-radius: 20px;
            text-decoration: none;
            font-size: 13px;
        }
        .btn-back-celebs {
            background: transparent;
            border: 1px solid #fff;
            color: #fff;
        }
        .celeb-details-error {
            text-align: center;
            padding: 40px 0;
            color: white;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="celeb-details-wrapper">
        <asp:PlaceHolder ID="phCeleb" runat="server"></asp:PlaceHolder>
    </div>
</asp:Content>
