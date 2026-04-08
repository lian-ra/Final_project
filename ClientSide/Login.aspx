<%@ Page Title="Login" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        /* Renamed class to avoid conflict with template styles */
        .custom-login-wrapper {
            margin-top: 40px;
            min-height: 80vh;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 20px;
            /* Ensure it sits on top if there are z-index issues */
            position: relative; 
            z-index: 10;
        }

        /* The main card container */
        .login-card {
            background: rgba(20, 20, 20, 0.8); /* Dark semi-transparent background */
            padding: 40px;
            border-radius: 15px;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.5); /* Soft shadow */
            border: 1px solid rgba(255, 255, 255, 0.1);
            width: 100%;
            max-width: 400px;
            text-align: center;
        }

        .login-card h1 {
            color: #ffffff;
            font-size: 32px;
            margin-bottom: 30px;
            font-weight: 600;
            letter-spacing: 1px;
            text-transform: uppercase;
        }

        /* Input group styling */
        .form-group {
            margin-bottom: 20px;
            text-align: left;
        }

        .form-label {
            color: #ccc;
            font-size: 14px;
            margin-bottom: 8px;
            display: block;
            font-weight: 500;
        }

        /* Styled TextBoxes */
        .form-control-custom {
            width: 100%;
            padding: 12px 15px;
            border-radius: 25px;
            border: 1px solid rgba(255, 255, 255, 0.2);
            background: rgba(255, 255, 255, 0.1);
            color: white;
            font-size: 16px;
            outline: none;
            transition: all 0.3s ease;
            box-sizing: border-box; 
        }

        .form-control-custom:focus {
            background: rgba(255, 255, 255, 0.2);
            border-color: #ff4c3b; /* Accent color */
            box-shadow: 0 0 10px rgba(255, 76, 59, 0.3);
        }

        /* Dropdown styling */
        .dropdown-custom {
            width: 100%;
            padding: 10px 15px;
            border-radius: 25px;
            border: 1px solid rgba(255, 255, 255, 0.2);
            background: #222; 
            color: white;
            font-size: 16px;
            height: 45px;
        }

        /* Button Container */
        .button-group {
            margin-top: 30px;
            display: flex;
            gap: 15px;
            justify-content: center;
        }

        /* Button Styling */
        .btn-custom {
            border: none;
            padding: 12px 0;
            border-radius: 25px;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
            width: 100%;
            transition: transform 0.2s, box-shadow 0.2s;
        }

        .btn-signin {
            background: #ffffff;
            color: #333;
        }

        .btn-signin:hover {
            background: #f0f0f0;
            transform: translateY(-2px);
            box-shadow: 0 5px 15px rgba(255, 255, 255, 0.2);
        }

        .btn-signup {
            background: #ff4c3b;
            color: white;
        }

        .btn-signup:hover {
            background: #e04332;
            transform: translateY(-2px);
            box-shadow: 0 5px 15px rgba(255, 76, 59, 0.4);
        }

    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="custom-login-wrapper">
        <div class="login-card">
            <h1>Login</h1>

            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Username" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtuser" runat="server" CssClass="form-control-custom" placeholder="Enter username"></asp:TextBox>
            </div>

            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Password" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtpass" runat="server" CssClass="form-control-custom" TextMode="Password" placeholder="Enter password"></asp:TextBox>
            </div>

            <div class="form-group">
                <asp:Label ID="Label3" runat="server" Text="Login As" CssClass="form-label"></asp:Label>
                <asp:DropDownList ID="drpChoice" runat="server" CssClass="dropdown-custom">
                    <asp:ListItem>user</asp:ListItem>
                    <asp:ListItem>admin</asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="button-group">
                <asp:Button ID="btnsi" runat="server" Text="Sign In" OnClick="ValidateUserLogin" CssClass="btn-custom btn-signin" />
                <asp:Button ID="btnsu2" runat="server" Text="Sign Up" OnClick="MoveToRegister_Click" CssClass="btn-custom btn-signup" />
            </div>
        </div>
    </div>

</asp:Content>
