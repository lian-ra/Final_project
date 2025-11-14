<%@ Page Title="Manage Celebs" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="ManageCelebs.aspx.cs" Inherits="ManageCelebs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .manage-wrapper {
            padding: 40px 20px;
            max-width: 800px;
            margin: 0 auto;
        }
        .manage-title {
            color: white;
            font-size: 32px;
            margin-bottom: 20px;
            text-align: center;
        }
        .manage-form {
            background: rgba(0,0,0,0.6);
            border-radius: 8px;
            padding: 20px;
            border: 1px solid rgba(255,255,255,0.1);
            color: white;
        }
        .manage-form table {
            width: 100%;
        }
        .manage-form td {
            padding: 6px 4px;
            vertical-align: top;
        }
        .manage-form label {
            font-weight: bold;
        }
        .manage-form input[type=text],
        .manage-form textarea {
            width: 100%;
            padding: 6px 8px;
            border-radius: 4px;
            border: 1px solid #333399;
            font-size: 13px;
            color: black;
        }
        .manage-buttons {
            text-align: right;
            margin-top: 10px;
        }
        .manage-buttons .asp-button {
            padding: 6px 14px;
            border-radius: 4px;
            border: none;
            background-color: #333399;
            color: white;
            font-size: 13px;
            cursor: pointer;
            margin-left: 8px;
        }
        .message-label {
            display: block;
            margin-top: 10px;
            font-size: 13px;
        }
        .message-success {
            color: #8fdd8f;
        }
        .message-error {
            color: #ff9090;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="manage-wrapper">
        <h1 class="manage-title">Manage Celebrities</h1>
        <div class="manage-form">
            <table>
                <tr>
                    <td style="width: 120px;"><label for="txtCelebName">Name</label></td>
                    <td><asp:TextBox ID="txtCelebName" runat="server" /></td>
                </tr>
                <tr>
                    <td><label for="ddlCelebRole">Role</label></td>
                    <td>
                        <asp:DropDownList ID="ddlCelebRole" runat="server">
                            <asp:ListItem Value="Actor">Actor</asp:ListItem>
                            <asp:ListItem Value="Actress">Actress</asp:ListItem>
                            <asp:ListItem Value="Director">Director</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td><label for="txtPhotoUrl">Photo URL</label></td>
                    <td>
                        <asp:TextBox ID="txtPhotoUrl" runat="server" />
                        <span style="font-size: 11px; opacity: 0.8;">(e.g. images/uploads/ava1.jpg)</span>
                    </td>
                </tr>
                <tr>
                    <td><label for="txtBio">Bio</label></td>
                    <td><asp:TextBox ID="txtBio" runat="server" TextMode="MultiLine" Rows="4" /></td>
                </tr>
            </table>
            <div class="manage-buttons">
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="asp-button" OnClick="btnClear_Click" />
                <asp:Button ID="btnAddCeleb" runat="server" Text="Add Celeb" CssClass="asp-button" OnClick="btnAddCeleb_Click" />
            </div>
            <asp:Label ID="lblManageMessage" runat="server" Visible="false" CssClass="message-label"></asp:Label>
        </div>

        <br />
        <asp:GridView ID="grdCelebs" runat="server" AutoGenerateColumns="False" CssClass="manage-grid"
            DataKeyNames="CelebId"
            OnRowEditing="grdCelebs_RowEditing"
            OnRowCancelingEdit="grdCelebs_RowCancelingEdit"
            OnRowUpdating="grdCelebs_RowUpdating"
            OnRowDeleting="grdCelebs_RowDeleting">
            <Columns>
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                <asp:BoundField DataField="CelebId" HeaderText="ID" ReadOnly="True" />
                <asp:BoundField DataField="Name" HeaderText="Name" />
                <asp:BoundField DataField="Role" HeaderText="Role" />
                <asp:BoundField DataField="Photo" HeaderText="Photo" />
                <asp:BoundField DataField="Bio" HeaderText="Bio" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
