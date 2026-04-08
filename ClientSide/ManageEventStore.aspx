<%@ Page Title="Manage Event Store" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="ManageEventStore.aspx.cs" Inherits="ManageEventStore" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .manage-wrapper {
            padding: 40px 20px;
            max-width: 1200px;
            margin: 0 auto;
            color: white;
        }

        .manage-header {
            text-align: center;
            margin-bottom: 40px;
        }

        .manage-title {
            font-size: 36px;
            font-weight: 700;
            color: #ff4c3b;
            text-transform: uppercase;
            letter-spacing: 1px;
        }

        .manage-subtitle {
            font-size: 18px;
            color: #aaa;
        }

        .split-layout {
            display: flex;
            gap: 30px;
            flex-wrap: wrap;
        }

        .panel-half {
            flex: 1;
            min-width: 300px;
            background: rgba(30, 30, 30, 0.8);
            border-radius: 15px;
            padding: 25px;
            border: 1px solid rgba(255, 255, 255, 0.1);
        }

        .panel-title {
            font-size: 24px;
            font-weight: 600;
            margin-bottom: 20px;
            border-bottom: 2px solid #ff4c3b;
            padding-bottom: 10px;
            color: #fff;
        }

        .stock-list {
            display: flex;
            flex-direction: column;
            gap: 15px;
        }

        .stock-item {
            background: rgba(255, 255, 255, 0.05);
            border-radius: 10px;
            padding: 15px;
            display: flex;
            align-items: center;
            justify-content: space-between;
            border: 1px solid rgba(255, 255, 255, 0.05);
            transition: all 0.3s;
        }

        .stock-item:hover {
            background: rgba(255, 255, 255, 0.1);
            transform: translateY(-2px);
        }

        .item-info {
            display: flex;
            align-items: center;
            gap: 15px;
        }

        .item-img {
            width: 60px;
            height: 60px;
            object-fit: contain;
            border-radius: 8px;
            background: rgba(255,255,255,0.1);
            padding: 5px;
        }

        .item-details h4 {
            margin: 0 0 5px 0;
            font-size: 16px;
            color: white;
        }

        .item-details p {
            margin: 0;
            color: #4ecdc4;
            font-weight: bold;
        }

        .btn-add, .btn-remove {
            padding: 8px 15px;
            border-radius: 20px;
            border: none;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s;
            font-size: 12px;
            text-transform: uppercase;
        }

        .btn-add {
            background: #4ecdc4;
            color: #111;
        }

        .btn-add:hover {
            background: #45b7af;
            box-shadow: 0 2px 10px rgba(78, 205, 196, 0.4);
        }

        .btn-remove {
            background: #ff4c3b;
            color: white;
        }

        .btn-remove:hover {
            background: #e63e2e;
            box-shadow: 0 2px 10px rgba(255, 76, 59, 0.4);
        }
        
        .empty-message {
            color: #aaa;
            font-style: italic;
            text-align: center;
            padding: 20px;
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
    <div class="manage-wrapper">
        <a href="EventDetails.aspx?eventId=<%= Request.QueryString["eventId"] %>" class="btn-back"><i class="fa fa-arrow-left"></i> Back to Event Details</a>
        
        <div class="manage-header">
            <h1 class="manage-title">Manage Event Store</h1>
            <div class="manage-subtitle">Select items from the global stock to sell at <asp:Label ID="lblEventName" runat="server" Font-Bold="true" ForeColor="White"></asp:Label></div>
        </div>

        <div class="split-layout">
            <div class="panel-half">
                <h3 class="panel-title">Global Stock</h3>
                <asp:Label ID="lblGlobalEmpty" runat="server" CssClass="empty-message" Text="No more items to add." Visible="false"></asp:Label>
                
                <div class="stock-list">
                    <asp:Repeater ID="rptGlobalStock" runat="server" OnItemCommand="rptGlobalStock_ItemCommand">
                        <ItemTemplate>
                            <div class="stock-item">
                                <div class="item-info">
                                    <img src='<%# ResolveUrl(Eval("Picture").ToString().StartsWith("~/") ? Eval("Picture").ToString() : "~/" + Eval("Picture").ToString()) %>' alt='<%# Eval("Name") %>' class="item-img" />
                                    <div class="item-details">
                                        <h4><%# Eval("Name") %></h4>
                                        <p><%# Eval("Price") %> ILS</p>
                                    </div>
                                </div>
                                <asp:Button ID="btnAdd" runat="server" Text="Add to Event" CommandName="Add" CommandArgument='<%# Eval("ProductId") %>' CssClass="btn-add" />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

            <div class="panel-half">
                <h3 class="panel-title">Event Store (Items for Sale)</h3>
                <asp:Label ID="lblEventEmpty" runat="server" CssClass="empty-message" Text="No items in your store yet. Add some from the Global Stock!" Visible="false"></asp:Label>

                <div class="stock-list">
                    <asp:Repeater ID="rptEventStock" runat="server" OnItemCommand="rptEventStock_ItemCommand">
                        <ItemTemplate>
                            <div class="stock-item">
                                <div class="item-info">
                                    <img src='<%# ResolveUrl(Eval("Picture").ToString().StartsWith("~/") ? Eval("Picture").ToString() : "~/" + Eval("Picture").ToString()) %>' alt='<%# Eval("Name") %>' class="item-img" />
                                    <div class="item-details">
                                        <h4><%# Eval("Name") %></h4>
                                        <p><%# Eval("Price") %> ILS</p>
                                    </div>
                                </div>
                                <asp:Button ID="btnRemove" runat="server" Text="Remove" CommandName="Remove" CommandArgument='<%# Eval("ProductId") %>' CssClass="btn-remove" />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
