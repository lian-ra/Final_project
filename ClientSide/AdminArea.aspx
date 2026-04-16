<%@ Page Title="Admin Area" Language="C#" MasterPageFile="~/Design.master" 
    AutoEventWireup="true" CodeFile="AdminArea.aspx.cs" Inherits="AdminArea" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .admin-wrapper {
            padding: 50px 20px;
            min-height: 80vh;
            max-width: 1000px;
            margin: 0 auto;
            display: flex;
            flex-direction: column;
            align-items: center;
        }

        .admin-title {
            color: #ffffff;
            font-size: 42px;
            margin-bottom: 50px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 2px;
            text-shadow: 0 2px 10px rgba(0,0,0,0.5);
        }

        .dashboard-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
            gap: 30px;
            width: 100%;
            margin-bottom: 50px;
        }

        .dash-card {
            background: rgba(30, 30, 30, 0.8);
            border: 1px solid rgba(255, 255, 255, 0.1);
            border-radius: 15px;
            padding: 40px 20px;
            text-align: center;
            text-decoration: none;
            transition: transform 0.3s, box-shadow 0.3s, border-color 0.3s;
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            backdrop-filter: blur(10px);
        }

        .dash-card:hover {
            transform: translateY(-10px);
            box-shadow: 0 15px 30px rgba(0, 0, 0, 0.5);
            border-color: #ff4c3b;
            background: rgba(40, 40, 40, 0.9);
        }

        .dash-icon {
            font-size: 48px;
            color: #ff4c3b;
            margin-bottom: 20px;
            transition: color 0.3s;
        }

        .dash-card:hover .dash-icon {
            color: #fff;
        }

        .dash-text {
            color: white;
            font-size: 20px;
            font-weight: 600;
            text-transform: uppercase;
        }

        .logout-container {
            margin-top: auto;
        }

        .btn-logout {
            padding: 12px 40px;
            background-color: transparent;
            border: 2px solid #ff4c3b;
            color: #ff4c3b;
            font-size: 16px;
            font-weight: bold;
            border-radius: 30px;
            text-decoration: none;
            transition: all 0.3s;
            display: inline-block;
        }

        .btn-logout:hover {
            background-color: #ff4c3b;
            color: white;
            box-shadow: 0 0 15px rgba(255, 76, 59, 0.4);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="admin-wrapper">
        <h1 class="admin-title">Admin Dashboard</h1>

        <div class="dashboard-grid">
            <asp:HyperLink ID="lnkSearchUsers" runat="server" NavigateUrl="~/SearchUsers.aspx"
                CssClass="dash-card">
                <i class="fa fa-users dash-icon"></i>
                <span class="dash-text">Search Users</span>
            </asp:HyperLink>

            <asp:HyperLink ID="lnkManageCelebs" runat="server" NavigateUrl="~/ManageCelebs.aspx"
                CssClass="dash-card">
                <i class="fa fa-star dash-icon"></i>
                <span class="dash-text">Manage Celebs</span>
            </asp:HyperLink>

            <asp:HyperLink ID="lnkManageMovies" runat="server" NavigateUrl="~/ManageMovies.aspx" 
                CssClass="dash-card">
                <i class="fa fa-film dash-icon"></i>
                <span class="dash-text">Manage Movies</span>
            </asp:HyperLink>

            <asp:HyperLink ID="lnkManageStock" runat="server" NavigateUrl="~/ManageStock.aspx" 
                CssClass="dash-card">
                <i class="fa fa-shopping-bag dash-icon"></i>
                <span class="dash-text">Manage Stock</span>
            </asp:HyperLink>

            <asp:HyperLink ID="lnkAdminOrders" runat="server" NavigateUrl="~/AdminOrders.aspx" 
                CssClass="dash-card">
                <i class="fa fa-list-alt dash-icon"></i>
                <span class="dash-text">All Orders</span>
            </asp:HyperLink>
        </div>

        <div class="logout-container">
            <asp:HyperLink ID="lnkLogout" runat="server" NavigateUrl="~/Login.aspx"
                CssClass="btn-logout">
                <i class="fa fa-sign-out"></i> Log Out
            </asp:HyperLink>
        </div>
    </div>

</asp:Content>