<%@ Page Title="Manage Celebs" Language="C#" MasterPageFile="~/Design.master" 
    AutoEventWireup="true" CodeFile="ManageCelebs.aspx.cs" Inherits="ManageCelebs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .manage-wrapper {
            padding: 40px 20px;
            max-width: 95%;
            margin: 0 auto;
            color: #ddd;
        }

        .manage-title {
            color: #ffffff;
            font-size: 36px;
            margin-bottom: 30px;
            text-align: center;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 1px;
        }

        .manage-form {
            background: rgba(30, 30, 30, 0.9);
            border-radius: 12px;
            padding: 30px;
            border: 1px solid rgba(255, 255, 255, 0.1);
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.3);
            margin-bottom: 40px;
            max-width: 800px;
            margin-left: auto;
            margin-right: auto;
        }

        .manage-form label {
            display: block;
            margin-bottom: 8px;
            color: #ccc;
            font-size: 14px;
            font-weight: 500;
        }

        .form-control {
            width: 100%;
            padding: 12px 15px;
            border-radius: 6px;
            border: 1px solid #444;
            background-color: #2a2a2a;
            color: white;
            font-size: 14px;
            box-sizing: border-box;
            transition: border-color 0.3s, box-shadow 0.3s;
        }

        .form-control:focus {
            border-color: #ff4c3b;
            outline: none;
            box-shadow: 0 0 8px rgba(255, 76, 59, 0.2);
        }

        .form-row {
            display: flex;
            gap: 20px;
            margin-bottom: 20px;
        }

        .form-group {
            flex: 1;
        }

        .manage-buttons {
            display: flex;
            justify-content: flex-end;
            gap: 15px;
            margin-top: 20px;
        }

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

        .search-container {
            margin-bottom: 20px;
            display: flex;
            justify-content: flex-end;
            gap: 10px;
        }

        .search-box {
            width: 250px;
            padding: 10px 15px;
            border-radius: 25px;
            border: 1px solid #444;
            background-color: #2a2a2a;
            color: white;
        }

        .grid-container {
            overflow-x: auto;
        }

        .manage-grid {
            width: 100%;
            border-collapse: collapse;
            background-color: #222;
            border-radius: 8px;
            overflow: hidden;
            border: 1px solid #444;
            table-layout: fixed;
        }

        .manage-grid th {
            background-color: #333;
            color: #ff4c3b;
            padding: 15px;
            text-align: left;
            font-weight: 600;
            border-bottom: 2px solid #444;
        }

        .manage-grid td {
            padding: 12px 10px;
            border-bottom: 1px solid #333;
            color: #ddd;
            word-wrap: break-word;
            word-break: break-word; 
            white-space: normal;
            vertical-align: top;
        }

        .manage-grid tr:last-child td {
            border-bottom: none;
        }

        .manage-grid tr:hover {
            background-color: #2a2a2a;
        }

        .manage-grid a {
            color: #ff4c3b;
            text-decoration: none;
            margin-right: 10px;
            font-weight: 500;
        }

        .manage-grid a:hover {
            text-decoration: underline;
        }

        .message-label {
            display: block;
            margin-top: 15px;
            font-size: 14px;
            padding: 10px;
            border-radius: 4px;
            text-align: center;
        }
        
        .message-success { background: rgba(0, 128, 0, 0.2); border: 1px solid green; color: #8fdd8f; }
        .message-error { background: rgba(128, 0, 0, 0.2); border: 1px solid red; color: #ff9090; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="manage-wrapper">
        <h1 class="manage-title">Manage Celebrities</h1>
        
        <div class="manage-form">
            <div class="form-row">
                <div class="form-group">
                    <label for="txtCelebName">Name</label>
                    <asp:TextBox ID="txtCelebName" runat="server" CssClass="form-control" 
                        placeholder="Enter celebrity name" />
                </div>
                <div class="form-group">
                    <label for="ddlCelebRole">Role</label>
                    <asp:DropDownList ID="ddlCelebRole" runat="server" CssClass="form-control">
                        <asp:ListItem Value="Actor">Actor</asp:ListItem>
                        <asp:ListItem Value="Actress">Actress</asp:ListItem>
                        <asp:ListItem Value="Director">Director</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="form-group" style="margin-bottom: 20px;">
                <label for="txtPhotoUrl">Photo URL</label>
                <asp:TextBox ID="txtPhotoUrl" runat="server" CssClass="form-control" 
                    placeholder="images/uploads/ava1.jpg" />
            </div>

            <div class="form-group" style="margin-bottom: 20px;">
                <label for="txtBio">Bio</label>
                <asp:TextBox ID="txtBio" runat="server" TextMode="MultiLine" Rows="4"
                    CssClass="form-control" placeholder="Enter short bio..." />
            </div>

            <div class="manage-buttons">
                <asp:Button ID="btnClear" runat="server" Text="Clear Form" 
                    CssClass="btn-custom btn-secondary" OnClick="btnClear_Click" />
                <asp:Button ID="btnAddCeleb" runat="server" Text="Add Celebrity"
                    CssClass="btn-custom btn-primary" OnClick="btnAddCeleb_Click" />
            </div>


            <asp:Label ID="lblManageMessage" runat="server" Visible="false"
                CssClass="message-label"></asp:Label>
        </div>

        <div class="search-container">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="search-box" 
                placeholder="Search celebrities..."></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn-custom btn-primary"
                OnClick="btnSearch_Click" />
        </div>

        <div class="grid-container">
            <asp:GridView ID="grdCelebs" runat="server" AutoGenerateColumns="False"
                CssClass="manage-grid"
                DataKeyNames="CelebId"
                OnRowEditing="grdCelebs_RowEditing"
                OnRowCancelingEdit="grdCelebs_RowCancelingEdit"
                OnRowUpdating="grdCelebs_RowUpdating"
                OnRowDeleting="grdCelebs_RowDeleting"
                GridLines="None">
                <Columns>
                    <asp:BoundField DataField="CelebId" HeaderText="ID" ReadOnly="True" ItemStyle-Width="5%" />
                    <asp:BoundField DataField="Name" HeaderText="Name" ControlStyle-CssClass="form-control" ItemStyle-Width="15%" />
                    <asp:BoundField DataField="Role" HeaderText="Role" ControlStyle-CssClass="form-control" ItemStyle-Width="10%" />
                    <asp:BoundField DataField="Photo" HeaderText="Photo" ControlStyle-CssClass="form-control" ItemStyle-Width="20%" />
                    <asp:BoundField DataField="Bio" HeaderText="Bio" ControlStyle-CssClass="form-control" ItemStyle-Width="35%" />
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" HeaderText="Actions" ItemStyle-Width="15%" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>