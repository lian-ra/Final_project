<%@ Page Title="My Orders" Language="C#" MasterPageFile="~/Design.master"
    AutoEventWireup="true" CodeFile="MyOrders.aspx.cs" Inherits="MyOrders" %>

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
            margin-bottom: 40px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 2px;
            text-align: center;
        }

        .success-msg {
            background: rgba(40, 167, 69, 0.2);
            color: #28a745;
            border: 1px solid #28a745;
            padding: 15px;
            border-radius: 8px;
            text-align: center;
            margin-bottom: 30px;
            font-weight: bold;
            font-size: 16px;
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

        .order-date {
            color: #aaa;
            font-size: 14px;
        }

        .order-body {
            color: #e0e0e0;
            font-size: 16px;
            line-height: 1.6;
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
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="orders-wrapper">
        <a href="ClientArea.aspx" class="btn-back"><i class="fa fa-arrow-left"></i> Back to Dashboard</a>
        
        <h1 class="orders-title">My Orders</h1>
        
        <asp:Label ID="lblSuccess" runat="server" CssClass="success-msg" Visible="false"></asp:Label>

        <div style="display: flex; gap: 10px; margin-bottom: 25px; justify-content: center;">
            <asp:TextBox ID="txtSearchOrders" runat="server" placeholder="Search order code, 
                event name, or items..." style="padding: 10px; border-radius: 5px; border: 1px 
             solid #444; background: #222; color: white; flex-grow: 1; max-width: 500px;"></asp:TextBox>
            <asp:Button ID="btnSearchOrders" runat="server" Text="Search" style="padding: 10px 20px;
         border-radius: 5px; background: #ff4c3b; color: white; border: none; font-weight: bold; 
         cursor: pointer;" OnClick="btnSearchOrders_Click" formnovalidate="formnovalidate" />
            <asp:Button ID="btnClearSearch" runat="server" Text="Clear" style="padding: 10px 20px; 
       border-radius: 5px; background: #555; color: white; border: none; font-weight: bold;
       cursor: pointer;" OnClick="btnClearSearch_Click" formnovalidate="formnovalidate" />
        </div>

        <asp:Label ID="lblEmpty" runat="server" CssClass="empty-orders" Visible="false" 
            Text="You haven't placed any orders yet."></asp:Label>

        <asp:Repeater ID="rptOrders" runat="server">
            <ItemTemplate>
                <div class="order-card">
                    <div class="order-header">
                        <div class="order-code">ORDER #<%# Eval("OrderId") %></div>
                        <div class="order-date"><%# Convert.ToDateTime(Eval("DatePurchased")).ToString("MMMM dd, yyyy HH:mm") %></div>
                    </div>
                    <div class="order-body">
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
