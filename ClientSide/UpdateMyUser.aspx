<%@ Page Title="Update Profile" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="UpdateMyUser.aspx.cs" Inherits="UpdateMyUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .update-wrapper {
            padding: 40px 20px;
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 80vh;
        }

        .update-card {
            background: rgba(30, 30, 30, 0.9);
            border-radius: 15px;
            padding: 40px;
            border: 1px solid rgba(255, 255, 255, 0.1);
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.5);
            width: 100%;
            max-width: 600px;
            text-align: center;
        }

        .update-card h1 {
            color: white;
            margin-bottom: 30px;
            text-transform: uppercase;
            font-size: 32px;
        }

        .form-row {
            display: flex;
            gap: 15px;
            margin-bottom: 15px;
        }

        .form-group {
            margin-bottom: 15px;
            text-align: left;
            flex: 1;
        }

        .form-label {
            display: block;
            color: #ccc;
            font-size: 14px;
            margin-bottom: 5px;
        }

        .form-control {
            width: 100%;
            padding: 10px;
            border-radius: 25px;
            border: 1px solid #444;
            background: #222;
            color: white;
            font-size: 14px;
            box-sizing: border-box;
        }

        .form-control:focus {
            border-color: #ff4c3b;
            outline: none;
        }

        .profile-img {
            width: 100px;
            height: 100px;
            border-radius: 50%;
            object-fit: cover;
            border: 3px solid #ff4c3b;
            margin-bottom: 10px;
        }

        .file-upload {
            color: white;
            margin-top: 5px;
        }

        .btn-update {
            background: #ff4c3b;
            color: white;
            border: none;
            padding: 12px 0;
            width: 100%;
            border-radius: 25px;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
            margin-top: 20px;
            transition: transform 0.2s;
        }

        .btn-update:hover {
            background: #e04332;
            transform: translateY(-2px);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="update-wrapper">
        <div class="update-card">
            <h1>Update Profile</h1>

            <div style="text-align: center; margin-bottom: 20px;">
                <asp:Image ID="img" runat="server" CssClass="profile-img" ImageUrl="~/MyPics/Profile.jpg" />
                <br />
                <asp:FileUpload ID="FileUpload1" runat="server" CssClass="file-upload" />
            </div>

            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Username (ReadOnly)" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" ReadOnly="true" style="background:#333; cursor:not-allowed;"></asp:TextBox>
            </div>

            <div class="form-row">
                <div class="form-group">
                    <asp:Label ID="Label3" runat="server" Text="First Name" CssClass="form-label"></asp:Label>
                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="Label4" runat="server" Text="Last Name" CssClass="form-label"></asp:Label>
                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Password" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="form-group">
                <asp:Label ID="Label5" runat="server" Text="Address" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="form-row">
                <div class="form-group">
                    <asp:Label ID="Label6" runat="server" Text="Email" CssClass="form-label"></asp:Label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="Label7" runat="server" Text="Phone" CssClass="form-label"></asp:Label>
                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>

            <asp:Button ID="btnUpdate" runat="server" Text="Update Profile" OnClick="btnUpdate_Click" CssClass="btn-update" />
        </div>
    </div>
</asp:Content>
