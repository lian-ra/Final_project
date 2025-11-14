<%@ Page Title="Manage Movies" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="ManageMovies.aspx.cs" Inherits="ManageMovies" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .manage-wrapper {
            padding: 40px 20px;
            max-width: 1000px;
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
        .manage-grid {
            margin-top: 20px;
            background: rgba(0,0,0,0.6);
            color: white;
            border: 1px solid rgba(255,255,255,0.1);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="manage-wrapper">
        <h1 class="manage-title">Manage Movies</h1>
        <div class="manage-form">
            <table>
                <tr>
                    <td style="width: 130px;"><label for="txtTitle">Title</label></td>
                    <td><asp:TextBox ID="txtTitle" runat="server" /></td>
                </tr>
                <tr>
                    <td><label for="txtYear">Year</label></td>
                    <td><asp:TextBox ID="txtYear" runat="server" /></td>
                </tr>
                <tr>
                    <td><label for="ddlGenre">Genre</label></td>
                    <td>
                        <asp:DropDownList ID="ddlGenre" runat="server">
                            <asp:ListItem Value="Action">Action</asp:ListItem>
                            <asp:ListItem Value="Drama">Drama</asp:ListItem>
                            <asp:ListItem Value="Comedy">Comedy</asp:ListItem>
                            <asp:ListItem Value="Horror">Horror</asp:ListItem>
                            <asp:ListItem Value="Sci-Fi">Sci-Fi</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td><label for="txtRating">Rating</label></td>
                    <td><asp:TextBox ID="txtRating" runat="server" /></td>
                </tr>
                <tr>
                    <td><label for="txtPoster">Poster URL</label></td>
                    <td>
                        <asp:TextBox ID="txtPoster" runat="server" />
                        <span style="font-size: 11px; opacity: 0.8;">(e.g. images/uploads/slider1.jpg)</span>
                    </td>
                </tr>
                <tr>
                    <td><label for="txtDirector">Director</label></td>
                    <td><asp:TextBox ID="txtDirector" runat="server" /></td>
                </tr>
                <tr>
                    <td><label for="txtActors">Actors</label></td>
                    <td><asp:TextBox ID="txtActors" runat="server" TextMode="MultiLine" Rows="2" /></td>
                </tr>
                <tr>
                    <td><label for="txtDuration">Duration (minutes)</label></td>
                    <td><asp:TextBox ID="txtDuration" runat="server" /></td>
                </tr>
                <tr>
                    <td><label for="txtDescription">Description</label></td>
                    <td><asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="4" /></td>
                </tr>
            </table>
            <div class="manage-buttons">
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="asp-button" OnClick="btnClear_Click" />
                <asp:Button ID="btnAddMovie" runat="server" Text="Add Movie" CssClass="asp-button" OnClick="btnAddMovie_Click" />
            </div>
            <asp:Label ID="lblManageMessage" runat="server" Visible="false" CssClass="message-label"></asp:Label>
        </div>

        <br />
        <asp:GridView ID="grdMovies" runat="server" AutoGenerateColumns="False" CssClass="manage-grid"
            DataKeyNames="MovieId"
            OnRowEditing="grdMovies_RowEditing"
            OnRowCancelingEdit="grdMovies_RowCancelingEdit"
            OnRowUpdating="grdMovies_RowUpdating"
            OnRowDeleting="grdMovies_RowDeleting">
            <Columns>
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                <asp:BoundField DataField="MovieId" HeaderText="ID" ReadOnly="True" />
                <asp:BoundField DataField="Title" HeaderText="Title" />
                <asp:BoundField DataField="Year" HeaderText="Year" />
                <asp:BoundField DataField="Genre" HeaderText="Genre" />
                <asp:BoundField DataField="Rating" HeaderText="Rating" />
                <asp:BoundField DataField="Poster" HeaderText="Poster" />
                <asp:BoundField DataField="Director" HeaderText="Director" />
                <asp:BoundField DataField="Actors" HeaderText="Actors" />
                <asp:BoundField DataField="Duration" HeaderText="Duration" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
