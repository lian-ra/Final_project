<%@ Page Title="All System Orders" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="AdminOrders.aspx.cs" Inherits="AdminOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .admin-orders-wrapper {
            padding: 50px 20px;
            max-width: 1200px;
            margin: 0 auto;
            min-height: 80vh;
        }

        .orders-title {
            color: #ffffff;
            font-size: 42px;
            margin-bottom: 5px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 2px;
            text-align: center;
        }

        .orders-subtitle {
            text-align: center;
            color: #aaa;
            font-size: 18px;
            margin-bottom: 40px;
        }

        .table-responsive {
            overflow-x: auto;
            background: rgba(30, 30, 30, 0.8);
            border-radius: 15px;
            border: 1px solid rgba(255, 255, 255, 0.1);
            box-shadow: 0 5px 15px rgba(0,0,0,0.5);
            padding: 20px;
        }

        table.admin-table {
            width: 100%;
            border-collapse: collapse;
            color: white;
            text-align: left;
        }

        table.admin-table th, table.admin-table td {
            padding: 15px;
            border-bottom: 1px solid rgba(255, 255, 255, 0.1);
        }

        table.admin-table th {
            background: rgba(255, 76, 59, 0.2);
            color: #ff4c3b;
            font-weight: 600;
            text-transform: uppercase;
            font-size: 14px;
            letter-spacing: 1px;
        }

        table.admin-table tr:hover td {
            background: rgba(255, 255, 255, 0.05);
        }

        .order-code {
            color: #4ecdc4;
            font-weight: bold;
        }

        .order-total {
            color: #2ecc71;
            font-weight: bold;
        }

        .btn-back {
            display: inline-block;
            margin-bottom: 20px;
            color: #ff4c3b;
            text-decoration: none;
            font-size: 16px;
            transition: color 0.3s;
        }
        
        .btn-back:hover {
            color: white;
        }
        
        .stats-bar {
            background: rgba(255, 76, 59, 0.1);
            border: 1px solid #ff4c3b;
            padding: 20px;
            border-radius: 10px;
            display: flex;
            justify-content: space-around;
            margin-bottom: 30px;
        }
        
        .stat-item {
            text-align: center;
        }
        
        .stat-value {
            font-size: 32px;
            font-weight: bold;
            color: #ff4c3b;
        }
        
        .stat-label {
            color: #aaa;
            text-transform: uppercase;
            font-size: 12px;
            letter-spacing: 1px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="admin-orders-wrapper">
        <a href="AdminArea.aspx" class="btn-back"><i class="fa fa-arrow-left"></i> Back to Admin Dashboard</a>
        
        <h1 class="orders-title">All System Orders</h1>
        <div class="orders-subtitle">Platform-wide events store revenue tracking</div>
        
        <asp:Panel ID="pnlStats" runat="server" CssClass="stats-bar" Visible="false">
            <div class="stat-item">
                <div class="stat-value"><asp:Label ID="lblTotalOrders" runat="server" Text="0"></asp:Label></div>
                <div class="stat-label">Total Orders</div>
            </div>
            <div class="stat-item">
                <div class="stat-value"><asp:Label ID="lblTotalRevenue" runat="server" Text="0.00 ILS"></asp:Label></div>
                <div class="stat-label">Platform Revenue</div>
            </div>
        </asp:Panel>

        <div class="table-responsive">
            <asp:Repeater ID="rptAllOrders" runat="server">
                <HeaderTemplate>
                    <table class="admin-table">
                        <thead>
                            <tr>
                                <th>Order Code</th>
                                <th>Buyer</th>
                                <th>Event Owner</th>
                                <th>Movie (Event)</th>
                                <th>Date Purchased</th>
                                <th>Items</th>
                                <th>Total</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                            <tr>
                                <td class="order-code"><%# Eval("OrderCode") %></td>
                                <td>@<a href="UserProfile.aspx?username=<%# Eval("Buyer") %>" style="color:white;"><%# Eval("Buyer") %></a></td>
                                <td>@<a href="UserProfile.aspx?username=<%# Eval("EventOwner") %>" style="color:white;"><%# Eval("EventOwner") %></a></td>
                                <td><b><%# Eval("MovieTitle") %></b><br /><small><%# Convert.ToDateTime(Eval("EventDate")).ToShortDateString() %></small></td>
                                <td><%# Convert.ToDateTime(Eval("DatePurchased")).ToString("MM/dd/yyyy HH:mm") %></td>
                                <td style="font-size:13px; color:#ccc;"><%# Eval("ItemsSummary") %></td>
                                <td class="order-total"><%# Convert.ToDecimal(Eval("Total")).ToString("0.00") %> ILS</td>
                            </tr>
                </ItemTemplate>
                <FooterTemplate>
                        </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            
            <asp:Label ID="lblEmpty" runat="server" Text="No orders found." Visible="false" style="color:#aaa; font-style:italic; display:block; text-align:center; padding:20px;"></asp:Label>
        </div>
    </div>
</asp:Content>
