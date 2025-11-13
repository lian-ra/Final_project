<%@ Page Title="Page Not Found" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="NotFound.aspx.cs" Inherits="NotFound" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .error-container {
            text-align: center;
            padding: 100px 20px;
            color: white;
        }
        .error-code {
            font-size: 120px;
            font-weight: bold;
            margin: 20px 0;
            color: #ff6b6b;
        }
        .error-message {
            font-size: 24px;
            margin: 20px 0;
        }
        .error-description {
            font-size: 16px;
            margin: 20px 0;
            opacity: 0.8;
        }
        .home-button {
            display: inline-block;
            padding: 12px 30px;
            background-color: #333399;
            color: white;
            text-decoration: none;
            border-radius: 5px;
            margin-top: 30px;
            font-size: 16px;
        }
        .home-button:hover {
            background-color: #4444aa;
            color: white;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="error-container">
        <div class="error-code">404</div>
        <div class="error-message">Page Not Found</div>
        <div class="error-description">
            The page you are looking for might have been removed, had its name changed, or is temporarily unavailable.
        </div>
        <a href="Login.aspx" class="home-button">Go to Home Page</a>
    </div>
</asp:Content>

