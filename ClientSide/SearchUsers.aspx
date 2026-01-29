<%@ Page Title="Search Users" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true"
    CodeFile="SearchUsers.aspx.cs" Inherits="SearchUsers" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <style type="text/css">
            .search-wrapper {
                padding: 40px 20px;
                max-width: 95%;
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

            .search-input {
                width: 250px;
            }

            .search-dropdown {
                width: 160px;
                height: 42px;
            }

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

            .btn-primary {
                background-color: #ff4c3b;
                color: white;
            }

            .btn-primary:hover {
                background-color: #e04332;
                transform: translateY(-2px);
            }

            .btn-secondary {
                background-color: #444;
                color: white;
            }

            .btn-secondary:hover {
                background-color: #555;
            }

            /* Grid Styling */
            .grid-container {
                overflow-x: auto;
                border-radius: 8px;
                box-shadow: 0 5px 15px rgba(0, 0, 0, 0.2);
            }

            .users-grid {
                width: 100%;
                border-collapse: collapse;
                background-color: #222;
                font-size: 13px;
                border: 1px solid #444;
                table-layout: fixed;
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
                word-wrap: break-word;
                word-break: break-word;
                white-space: normal;
            }

            .users-grid tr:hover {
                background-color: #2a2a2a;
            }

            .users-grid tr.selected-row {
                background-color: rgba(255, 76, 59, 0.15) !important;
                border-left: 4px solid #ff4c3b;
            }

            .user-avatar {
                border-radius: 50%;
                object-fit: cover;
                border: 2px solid #555;
            }

            /* --- MODAL STYLES --- */
            .modal-overlay {
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background: rgba(0, 0, 0, 0.7);
                z-index: 1000;
                display: flex;
                align-items: center;
                justify-content: center;
                backdrop-filter: blur(5px);
            }

            .modal-content {
                background: #2a2a2a;
                border: 1px solid #444;
                border-radius: 15px;
                padding: 30px;
                width: 90%;
                max-width: 500px;
                box-shadow: 0 15px 50px rgba(0, 0, 0, 0.5);
                animation: fadeIn 0.3s ease-out;
            }

            @keyframes fadeIn {
                from {
                    opacity: 0;
                    transform: translateY(-20px);
                }

                to {
                    opacity: 1;
                    transform: translateY(0);
                }
            }

            .modal-header {
                font-size: 24px;
                font-weight: bold;
                color: #ff4c3b;
                margin-bottom: 20px;
                text-align: center;
                text-transform: uppercase;
            }

            .modal-body {
                display: flex;
                flex-direction: column;
                gap: 15px;
            }

            .modal-footer {
                margin-top: 25px;
                display: flex;
                justify-content: space-between;
            }

            .input-group label {
                display: block;
                color: #ccc;
                margin-bottom: 5px;
                font-size: 13px;
            }

            .modal-control {
                width: 100%;
                padding: 10px;
                border-radius: 5px;
                border: 1px solid #555;
                background: #1a1a1a;
                color: white;
                box-sizing: border-box;
            }
        </style>
    </asp:Content>

    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="search-wrapper">
            <h1 class="page-title">Search Users</h1>

            <div class="search-card">
                <asp:Label ID="Label1" runat="server" Text="Search:" CssClass="search-label"></asp:Label>
                <asp:TextBox ID="TxtSearch" runat="server" CssClass="form-control search-input"
                    placeholder="Type here..."></asp:TextBox>
                <asp:DropDownList ID="DrpSearch" runat="server" CssClass="form-control search-dropdown">
                    <asp:ListItem Value="name">By Name</asp:ListItem>
                    <asp:ListItem Value="address">By Address</asp:ListItem>
                    <asp:ListItem Value="username">By Username</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="BtnSearch" runat="server" Text="Search" OnClick="BtnSearch_Click"
                    CssClass="btn-custom btn-primary" />
                <asp:Button ID="BtnReset" runat="server" Text="Show All" OnClick="BtnReset_Click"
                    CssClass="btn-custom btn-secondary" />
            </div>

            <div class="grid-container">
                <asp:GridView ID="GrdUsers" runat="server" AutoGenerateColumns="False" CssClass="users-grid"
                    GridLines="None" OnSelectedIndexChanged="GrdUsers_SelectedIndexChanged" DataKeyNames="User">
                    <Columns>
                        <asp:CommandField ButtonType="Button" SelectText="Edit" ShowSelectButton="True"
                            ControlStyle-CssClass="btn-custom btn-secondary" ItemStyle-Width="8%" />
                        <asp:BoundField DataField="User" HeaderText="Username" ItemStyle-Width="10%" />
                        <asp:BoundField DataField="FName" HeaderText="First Name" ItemStyle-Width="10%" />
                        <asp:BoundField DataField="LName" HeaderText="Last Name" ItemStyle-Width="10%" />
                        <asp:BoundField DataField="address" HeaderText="Address" ItemStyle-Width="15%" />
                        <asp:BoundField DataField="email" HeaderText="Email" ItemStyle-Width="17%" />

                        <asp:TemplateField HeaderText="Phone" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="lblPhone" runat="server"
                                    Text='<%# GetPhoneValue(Container.DataItem) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Gender" ItemStyle-Width="6%">
                            <ItemTemplate>
                                <asp:Label ID="lblGender" runat="server"
                                    Text='<%# GetGenderValue(Container.DataItem) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Birthday" ItemStyle-Width="8%">
                            <ItemTemplate>
                                <asp:Label ID="lblBirthday" runat="server"
                                    Text='<%# GetBirthdayValue(Container.DataItem) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Pic" ItemStyle-Width="6%">
                            <ItemTemplate>
                                <asp:Image ID="imgUser" runat="server"
                                    ImageUrl='<%# ResolveUrl("~/MyPics/" + Eval("pic")) %>' CssClass="user-avatar"
                                    Height="40px" Width="40px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <SelectedRowStyle CssClass="selected-row" />
                </asp:GridView>
            </div>

            <asp:Panel ID="pnlModal" runat="server" Visible="false" CssClass="modal-overlay">
                <div class="modal-content">
                    <div class="modal-header">Edit User</div>
                    <div class="modal-body">
                        <asp:HiddenField ID="HiddenUsername" runat="server" />

                        <div class="input-group">
                            <label>First Name</label>
                            <asp:TextBox ID="TxtName" runat="server" CssClass="modal-control"></asp:TextBox>
                        </div>
                        <div class="input-group">
                            <label>Last Name</label>
                            <asp:TextBox ID="TxtLast" runat="server" CssClass="modal-control"></asp:TextBox>
                        </div>
                        <div class="input-group">
                            <label>Email</label>
                            <asp:TextBox ID="TxtEmail" runat="server" CssClass="modal-control"></asp:TextBox>
                        </div>
                        <div class="input-group">
                            <label>Address</label>
                            <asp:TextBox ID="TxtAddress" runat="server" CssClass="modal-control"></asp:TextBox>
                        </div>
                        <div class="input-group">
                            <label>Phone</label>
                            <asp:TextBox ID="TxtPhone" runat="server" CssClass="modal-control"></asp:TextBox>
                        </div>
                        <div class="input-group">
                            <label>Profile Picture</label>
                            <asp:FileUpload ID="fileUploadPic" runat="server" CssClass="modal-control" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div>
                            <asp:Button ID="BtnDeleteUser" runat="server" Text="Delete User"
                                OnClick="BtnDeleteUser_Click" CssClass="btn-custom" BackColor="#d9534f"
                                ForeColor="White"
                                OnClientClick="return confirm('Are you sure you want to delete this user? This cannot be undone.');" />
                        </div>
                        <div>
                            <asp:Button ID="BtnClose" runat="server" Text="Cancel" OnClick="BtnClose_Click"
                                CssClass="btn-custom btn-secondary" />
                            <asp:Button ID="BtnUpdateUser" runat="server" Text="Save Changes"
                                OnClick="BtnUpdateUser_Click" CssClass="btn-custom btn-primary" />
                        </div>
                    </div>
                </div>
            </asp:Panel>

        </div>
    </asp:Content>