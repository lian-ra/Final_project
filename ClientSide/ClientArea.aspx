<%@ Page Title="User Dashboard" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true"
    CodeFile="ClientArea.aspx.cs" Inherits="ClientArea" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
        <style type="text/css">
            .client-wrapper {
                padding: 50px 20px;
                min-height: 80vh;
                max-width: 1200px;
                margin: 0 auto;
                display: flex;
                flex-direction: column;
                align-items: center;
            }

            .client-title {
                color: #ffffff;
                font-size: 42px;
                margin-bottom: 50px;
                font-weight: 700;
                text-transform: uppercase;
                letter-spacing: 2px;
                text-shadow: 0 2px 10px rgba(0, 0, 0, 0.5);
            }

            .dashboard-grid {
                display: flex;
                justify-content: center;
                gap: 20px;
                width: 100%;
                margin-bottom: 50px;
                flex-wrap: wrap;
            }

            .dash-card {
                background: rgba(30, 30, 30, 0.8);
                border: 1px solid rgba(255, 255, 255, 0.1);
                border-radius: 20px;
                padding: 40px;
                text-align: center;
                text-decoration: none;
                transition: transform 0.3s, box-shadow 0.3s, border-color 0.3s;
                display: flex;
                flex-direction: column;
                align-items: center;
                justify-content: center;
                backdrop-filter: blur(10px);
                width: 220px;
            }

            .dash-card:hover {
                transform: translateY(-10px);
                box-shadow: 0 15px 30px rgba(0, 0, 0, 0.5);
                border-color: #ff4c3b;
                background: rgba(40, 40, 40, 0.9);
            }

            .card-image-container {
                width: 100px;
                height: 100px;
                border-radius: 50%;
                overflow: hidden;
                border: 3px solid #444;
                margin-bottom: 20px;
                transition: border-color 0.3s;
                display: flex;
                align-items: center;
                justify-content: center;
                background: #222;
            }

            .dash-card:hover .card-image-container {
                border-color: #ff4c3b;
            }

            .dash-card img {
                width: 100%;
                height: 100%;
                object-fit: cover;
            }

            .dash-icon {
                font-size: 40px;
                color: #ccc;
            }

            .dash-card:hover .dash-icon {
                color: #ff4c3b;
            }

            .dash-text {
                color: white;
                font-size: 18px;
                font-weight: 600;
                text-transform: uppercase;
            }

            .actions-container {
                display: flex;
                flex-direction: column;
                gap: 15px;
                align-items: center;
                width: 100%;
                max-width: 300px;
            }

            .btn-custom {
                width: 100%;
                padding: 12px 0;
                border-radius: 30px;
                font-size: 16px;
                font-weight: bold;
                text-align: center;
                text-decoration: none;
                cursor: pointer;
                transition: all 0.3s;
                display: block;
                border: none;
            }

            .btn-logout {
                background-color: transparent;
                border: 2px solid #aaa;
                color: #aaa;
            }

            .btn-logout:hover {
                border-color: white;
                color: white;
                background-color: rgba(255, 255, 255, 0.1);
            }

            .btn-danger {
                background-color: rgba(255, 68, 68, 0.1);
                border: 2px solid #ff4444;
                color: #ff4444;
            }

            .btn-danger:hover {
                background-color: #ff4444;
                color: white;
                box-shadow: 0 0 15px rgba(255, 68, 68, 0.4);
            }
        </style>
    </asp:Content>

    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

        <div class="client-wrapper">
            <h1 class="client-title">User Dashboard</h1>

            <div class="dashboard-grid">
                <a href="UpdateMyUser.aspx" class="dash-card">
                    <div class="card-image-container">
                        <asp:Image ID="imgUpdateUser" runat="server" 
                            ImageUrl="~/MyPics/update_user.jpg" />
                    </div>
                    <span class="dash-text">Update Profile</span>
                </a>

                <a href="MyFollowers.aspx" class="dash-card">
                    <div class="card-image-container">
                        <i class="fa fa-users dash-icon"></i>
                    </div>
                    <span class="dash-text">My Friends</span>
                </a>

                <a href="Wishlist.aspx" class="dash-card">
                    <div class="card-image-container">
                        <i class="fa fa-heart dash-icon"></i>
                    </div>
                    <span class="dash-text">My Wishlist</span>
                </a>

                <a href="UserProfile.aspx" class="dash-card">
                    <div class="card-image-container">
                        <i class="fa fa-user dash-icon"></i>
                    </div>
                    <span class="dash-text">View My Profile</span>
                </a>
                
                <a href="MyOrders.aspx" class="dash-card">
                    <div class="card-image-container">
                        <i class="fa fa-shopping-bag dash-icon"></i>
                    </div>
                    <span class="dash-text">My Orders</span>
                </a>
            </div>

            <div class="actions-container">
                <asp:Button ID="btnResetWishlist" runat="server" Text="Reset Wishlist"
                    OnClick="btnResetWishlist_Click"
                    OnClientClick="return confirm('Are you sure you want to delete 
                    the Wishlist table? This cannot be undone.');"
                    CssClass="btn-custom btn-danger" />

                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Login.aspx"
                    CssClass="btn-custom btn-logout">
                    <i class="fa fa-sign-out"></i> Log Out
                </asp:HyperLink>
            </div>
        </div>

    </asp:Content>