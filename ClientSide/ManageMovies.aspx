<%@ Page Title="Manage Movies" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="ManageMovies.aspx.cs" Inherits="ManageMovies" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .manage-wrapper {
            padding: 40px 20px;
            max-width: 95%; /* Wider to accommodate many columns */
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

        /* Form Card Styling */
        .manage-form {
            background: rgba(30, 30, 30, 0.9);
            border-radius: 12px;
            padding: 30px;
            border: 1px solid rgba(255, 255, 255, 0.1);
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.3);
            margin-bottom: 40px;
            max-width: 900px;
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

        /* Input Fields */
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

        /* Layout for form rows */
        .form-row {
            display: flex;
            gap: 20px;
            margin-bottom: 20px;
            flex-wrap: wrap;
        }

        .form-group {
            flex: 1;
            min-width: 200px;
        }

        /* Buttons */
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

        .btn-primary { background-color: #ff4c3b; color: white; }
        .btn-primary:hover { background-color: #e04332; transform: translateY(-2px); }

        .btn-secondary { background-color: #444; color: white; }
        .btn-secondary:hover { background-color: #555; }

        /* Search Bar */
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

        /* GridView Styling */
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
            table-layout: fixed; /* IMPORTANT */
            font-size: 13px;
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
            
            /* IMPORTANT: Text wrapping */
            word-wrap: break-word;
            word-break: break-word; 
            white-space: normal;
            vertical-align: top;
        }

        .manage-grid tr:last-child td { border-bottom: none; }
        .manage-grid tr:hover { background-color: #2a2a2a; }

        .manage-grid a {
            color: #ff4c3b;
            text-decoration: none;
            margin-right: 10px;
            font-weight: 500;
        }
        .manage-grid a:hover { text-decoration: underline; }

        /* Messages */
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
        <h1 class="manage-title">Manage Movies</h1>
        
        <div class="manage-form">
            <div class="form-row">
                <div class="form-group">
                    <label for="txtTitle">Title</label>
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" placeholder="Movie title" />
                </div>
                <div class="form-group">
                    <label for="txtYear">Year</label>
                    <asp:TextBox ID="txtYear" runat="server" CssClass="form-control" placeholder="e.g. 2023" />
                </div>
            </div>

            <div class="form-row">
                <div class="form-group">
                    <label for="ddlGenre">Genre</label>
                    <asp:DropDownList ID="ddlGenre" runat="server" CssClass="form-control">
                        <asp:ListItem Value="Action">Action</asp:ListItem>
                        <asp:ListItem Value="Drama">Drama</asp:ListItem>
                        <asp:ListItem Value="Comedy">Comedy</asp:ListItem>
                        <asp:ListItem Value="Horror">Horror</asp:ListItem>
                        <asp:ListItem Value="Sci-Fi">Sci-Fi</asp:ListItem>
                        <asp:ListItem Value="Romance">Romance</asp:ListItem>
                        <asp:ListItem Value="Adventure">Adventure</asp:ListItem>
                        <asp:ListItem Value="Fantasy">Fantasy</asp:ListItem>
                        <asp:ListItem Value="Thriller">Thriller</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label for="txtRating">Rating (0-10)</label>
                    <asp:TextBox ID="txtRating" runat="server" CssClass="form-control" placeholder="e.g. 8.5" />
                </div>
                <div class="form-group">
                    <label for="txtDuration">Duration (min)</label>
                    <asp:TextBox ID="txtDuration" runat="server" CssClass="form-control" placeholder="e.g. 120" />
                </div>
            </div>

            <div class="form-row">
                <div class="form-group">
                    <label for="txtDirector">Director</label>
                    <asp:TextBox ID="txtDirector" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label for="txtPoster">Poster URL</label>
                    <asp:TextBox ID="txtPoster" runat="server" CssClass="form-control" placeholder="images/uploads/..." />
                </div>
            </div>

            <div class="form-group" style="margin-bottom: 20px;">
                <label for="txtActors">Actors</label>
                <asp:TextBox ID="txtActors" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
            </div>

            <div class="form-group" style="margin-bottom: 20px;">
                <label for="txtDescription">Description</label>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
            </div>

            <div class="manage-buttons">
                <asp:Button ID="btnClear" runat="server" Text="Clear Form" CssClass="btn-custom btn-secondary" OnClick="btnClear_Click" />
                <asp:Button ID="btnAddMovie" runat="server" Text="Add Movie" CssClass="btn-custom btn-primary" OnClick="btnAddMovie_Click" />
            </div>

            <asp:Label ID="lblManageMessage" runat="server" Visible="false" CssClass="message-label"></asp:Label>
        </div>

        <div class="search-container">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="search-box" placeholder="Search movies..."></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn-custom btn-primary" OnClick="btnSearch_Click" />
        </div>

        <div class="grid-container">
            <asp:GridView ID="grdMovies" runat="server" AutoGenerateColumns="False" CssClass="manage-grid"
                DataKeyNames="MovieId"
                OnRowEditing="grdMovies_RowEditing"
                OnRowCancelingEdit="grdMovies_RowCancelingEdit"
                OnRowUpdating="grdMovies_RowUpdating"
                OnRowDeleting="grdMovies_RowDeleting"
                GridLines="None">
                <Columns>
                    <asp:BoundField DataField="MovieId" HeaderText="ID" ReadOnly="True" ItemStyle-Width="5%" />
                    <asp:BoundField DataField="Title" HeaderText="Title" ControlStyle-CssClass="form-control" ItemStyle-Width="15%" />
                    <asp:BoundField DataField="Year" HeaderText="Year" ControlStyle-CssClass="form-control" ItemStyle-Width="6%" />
                    <asp:BoundField DataField="Genre" HeaderText="Genre" ControlStyle-CssClass="form-control" ItemStyle-Width="8%" />
                    <asp:BoundField DataField="Rating" HeaderText="Rating" ControlStyle-CssClass="form-control" ItemStyle-Width="6%" />
                    <asp:BoundField DataField="Director" HeaderText="Director" ControlStyle-CssClass="form-control" ItemStyle-Width="12%" />
                    <asp:BoundField DataField="Actors" HeaderText="Actors" ControlStyle-CssClass="form-control" ItemStyle-Width="20%" />
                    <asp:BoundField DataField="Duration" HeaderText="Min" ControlStyle-CssClass="form-control" ItemStyle-Width="6%" />
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" HeaderText="Actions" ItemStyle-Width="12%" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>