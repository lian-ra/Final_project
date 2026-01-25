<%@ Page Title="Search Users" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="SearchUsers.aspx.cs" Inherits="SearchUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .search-wrapper {
            padding: 40px 20px;
            max-width: 95%; /* Made wider to fit data */
            margin: 0 auto;
            color: #ddd;
        }

        .page-title {
            color: #ffffff;
            font-size: 36px;
            margin-bottom: 30px;
            text-align: center;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 1px;
        }

        /* Search Card Styling */
        .search-card {
            background: rgba(30, 30, 30, 0.9);
            border-radius: 12px;
            padding: 25px;
            border: 1px solid rgba(255, 255, 255, 0.1);
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.3);
            margin-bottom: 30px;
            display: flex;
            flex-wrap: wrap;
            gap: 15px;
            align-items: center;
            justify-content: center;
            max-width: 900px;
            margin-left: auto;
            margin-right: auto;
        }

        .search-label {
            font-size: 16px;
            font-weight: 500;
            color: white;
        }

        .form-control {
            padding: 10px 15px;
            border-radius: 25px;
            border: 1px solid #444;
            background-color: #2a2a2a;
            color: white;
            font-size: 14px;
            outline: none;
            transition: all 0.3s;
        }

        .form-control:focus {
            border-color: #ff4c3b;
            box-shadow: 0 0 8px rgba(255, 76, 59, 0.2);
        }

        .search-input { width: 250px; }
        .search-dropdown { width: 160px; height: 42px; }

        /* Buttons */
        .btn-custom {
            padding: 10px 25px;
            border-radius: 25px;
            border: none;
            font-size: 14px;
            font-weight: bold;
            cursor: pointer;
            transition: all 0.2s ease;
        }

        .btn-primary { background-color: #ff4c3b; color: white; }
        .btn-primary:hover { background-color: #e04332; transform: translateY(-2px); }

        .btn-secondary { background-color: #444; color: white; }
        .btn-secondary:hover { background-color: #555; }

        /* Grid Styling */
        .grid-container {
            overflow-x: auto;
            border-radius: 8px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.2);
        }

        .users-grid {
            width: 100%;
            border-collapse: collapse;
            background-color: #222;
            font-size: 13px; /* Slightly smaller text to fit more */
            border: 1px solid #444;
            table-layout: fixed; /* IMPORTANT: Keeps columns from exploding */
        }

        .users-grid th {
            background-color: #333;
            color: #ff4c3b;
            padding: 15px;
            text-align: left;
            border-bottom: 2px solid #444;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .users-grid td {
            padding: 12px 10px;
            border-bottom: 1px solid #333;
            color: #ddd;
            vertical-align: top;
            
            /* CRITICAL: Wraps long text to prevent overlap */
            word-wrap: break-word;
            word-break: break-word; 
            white-space: normal; 
        }

        .users-grid tr:hover { background-color: #2a2a2a; }

        .users-grid tr.selected-row {
            background-color: rgba(255, 76, 59, 0.15) !important;
            border-left: 4px solid #ff4c3b;
        }

        .user-avatar {
            border-radius: 50%;
            object-fit: cover;
            border: 2px solid #555;
        }

        /* Selected Details Area */
        .details-card {
            margin-top: 30px;
            background: rgba(40, 40, 40, 0.9);
            padding: 20px;
            border-radius: 10px;
            border: 1px solid #444;
            display: flex;
            align-items: center;
            gap: 20px;
            max-width: 800px;
            margin-left: auto;
            margin-right: auto;
        }

        .details-title {
            color: #ff4c3b;
            font-weight: 600;
            margin-right: 15px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="search-wrapper">
        <h1 class="page-title">Search Users</h1>

        <div class="search-card">
            <asp:Label ID="Label1" runat="server" Text="Search:" CssClass="search-label"></asp:Label>
            <asp:TextBox ID="TxtSearch" runat="server" CssClass="form-control search-input" placeholder="Type here..."></asp:TextBox>
            <asp:DropDownList ID="DrpSearch" runat="server" CssClass="form-control search-dropdown">
                <asp:ListItem Value="name">By Name</asp:ListItem>
                <asp:ListItem Value="address">By Address</asp:ListItem>
                <asp:ListItem Value="username">By Username</asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="BtnSearch" runat="server" Text="Search" OnClick="BtnSearch_Click" CssClass="btn-custom btn-primary" />
            <asp:Button ID="BtnReset" runat="server" Text="Show All" OnClick="BtnReset_Click" CssClass="btn-custom btn-secondary" />
        </div>

        <div class="grid-container">
            <asp:GridView ID="GrdUsers" runat="server" AutoGenerateColumns="False" 
                CssClass="users-grid" GridLines="None"
                OnSelectedIndexChanged="GrdUsers_SelectedIndexChanged"
                DataKeyNames="User">
                <Columns>
                    <asp:CommandField ButtonType="Button" SelectText="Select" ShowSelectButton="True" ControlStyle-CssClass="btn-custom btn-secondary" ItemStyle-Width="8%" />
                    <asp:BoundField DataField="User" HeaderText="Username" ItemStyle-Width="10%" />
                    <asp:BoundField DataField="FName" HeaderText="First Name" ItemStyle-Width="10%" />
                    <asp:BoundField DataField="LName" HeaderText="Last Name" ItemStyle-Width="10%" />
                    <asp:BoundField DataField="address" HeaderText="Address" ItemStyle-Width="15%" />
                    <asp:BoundField DataField="email" HeaderText="Email" ItemStyle-Width="17%" />
                    
                    <asp:TemplateField HeaderText="Phone" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Label ID="lblPhone" runat="server" Text='<%# GetPhoneValue(Container.DataItem) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Gender" ItemStyle-Width="6%">
                        <ItemTemplate>
                            <asp:Label ID="lblGender" runat="server" Text='<%# GetGenderValue(Container.DataItem) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Birthday" ItemStyle-Width="8%">
                        <ItemTemplate>
                            <asp:Label ID="lblBirthday" runat="server" Text='<%# GetBirthdayValue(Container.DataItem) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Pic" ItemStyle-Width="6%">
                        <ItemTemplate>
                            <asp:Image ID="imgUser" runat="server" ImageUrl='<%# ResolveUrl("~/MyPics/" + Eval("pic")) %>' CssClass="user-avatar" Height="40px" Width="40px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <SelectedRowStyle CssClass="selected-row" />
            </asp:GridView>
        </div>

        <div class="details-card" id="detailsSection" runat="server" visible="false">
            <span class="details-title">Selected User:</span>
            
            <div style="display:flex; gap: 10px; align-items: center;">
                <label style="color: #ccc;">First Name:</label>
                <asp:TextBox ID="TxtName" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
            
            <div style="display:flex; gap: 10px; align-items: center;">
                <label style="color: #ccc;">Last Name:</label>
                <asp:TextBox ID="TxtLast" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
        </div>

    </div>
</asp:Content>