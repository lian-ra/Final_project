<%@ Page Title="Event Orders" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="EventOrders.aspx.cs" Inherits="EventOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .orders-wrapper {
            padding: 50px 20px;
            max-width: 1000px;
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

        .order-card {
            background: rgba(30, 30, 30, 0.8);
            border: 1px solid rgba(255, 255, 255, 0.1);
            border-radius: 15px;
            padding: 25px;
            margin-bottom: 25px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.5);
            display: flex;
            flex-direction: column;
            gap: 15px;
        }

        .order-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            border-bottom: 1px solid rgba(255,255,255,0.1);
            padding-bottom: 15px;
        }

        .order-code {
            color: #ff4c3b;
            font-size: 20px;
            font-weight: bold;
            letter-spacing: 1px;
        }

        .order-buyer {
            color: #4ecdc4;
            font-size: 16px;
            font-weight: bold;
        }

        .order-date {
            color: #aaa;
            font-size: 14px;
        }

        .order-body {
            color: #e0e0e0;
            font-size: 16px;
            line-height: 1.6;
            background: rgba(0,0,0,0.2);
            padding: 15px;
            border-radius: 8px;
        }

        .order-footer {
            display: flex;
            justify-content: flex-end;
            align-items: center;
            border-top: 1px solid rgba(255,255,255,0.1);
            padding-top: 15px;
        }

        .order-total {
            font-size: 24px;
            font-weight: bold;
            color: white;
        }
        
        .order-total span {
            color: #ff4c3b;
        }

        .empty-orders {
            text-align: center;
            color: #888;
            font-size: 18px;
            font-style: italic;
            background: rgba(255, 255, 255, 0.05);
            padding: 40px;
            border-radius: 10px;
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
            background: rgba(78, 205, 196, 0.1);
            border: 1px solid #4ecdc4;
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
            color: #4ecdc4;
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
    <div class="orders-wrapper">
        <a href="EventDetails.aspx?eventId=<%= Request.QueryString["eventId"] %>" class="btn-back"><i class="fa fa-arrow-left"></i> Back to Event Details</a>
        
        <h1 class="orders-title">Event Orders</h1>
        <div class="orders-subtitle"><asp:Label ID="lblEventName" runat="server"></asp:Label></div>
        
        <asp:Panel ID="pnlStats" runat="server" CssClass="stats-bar" Visible="false">
            <div class="stat-item">
                <div class="stat-value"><asp:Label ID="lblTotalOrders" runat="server" Text="0"></asp:Label></div>
                <div class="stat-label">Total Orders</div>
            </div>
            <div class="stat-item">
                <div class="stat-value"><asp:Label ID="lblTotalRevenue" runat="server" Text="0.00 ILS"></asp:Label></div>
                <div class="stat-label">Revenue</div>
            </div>
        </asp:Panel>

        <asp:Label ID="lblEmpty" runat="server" CssClass="empty-orders" Visible="false" Text="No orders have been placed yet for this event."></asp:Label>

        <asp:Repeater ID="rptOrders" runat="server">
            <ItemTemplate>
                <div class="order-card">
                    <div class="order-header">
                        <div class="order-code">ORDER #<%# Eval("OrderId") %></div>
                        <div class="order-buyer">Buyer: @<a href="UserProfile.aspx?username=<%# Eval("Buyer") %>" style="color:inherit;"><%# Eval("Buyer") %></a></div>
                        <div class="order-date"><%# Convert.ToDateTime(Eval("DatePurchased")).ToString("MMMM dd, yyyy HH:mm") %></div>
                    </div>
                    <div class="order-body">
                        <b>Items Ordered:</b><br />
                        <%# Eval("ItemsSummary") %>
                    </div>
                    <div class="order-footer">
                        <div class="order-total">Total: <span><%# Convert.ToDecimal(Eval("Total")).ToString("0.00") %> ILS</span></div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
