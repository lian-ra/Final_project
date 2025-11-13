<%@ Page Title="Add Movie" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="AddMovie.aspx.cs" Inherits="AddMovie" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .add-movie-container {
            padding: 40px 20px;
            color: white;
            max-width: 800px;
            margin: 0 auto;
        }
        .add-movie-header {
            text-align: center;
            margin-bottom: 30px;
        }
        .add-movie-header h1 {
            font-size: 36px;
            margin-bottom: 10px;
            color: #ff6b6b;
        }
        .form-table {
            width: 100%;
            margin: 0 auto;
        }
        .form-table td {
            padding: 15px;
            vertical-align: top;
        }
        .form-label {
            color: white;
            font-size: 14pt;
            text-align: right;
            padding-right: 20px;
            width: 150px;
        }
        .form-input {
            width: 100%;
            padding: 10px;
            border-radius: 5px;
            border: 1px solid #333399;
            background: rgba(255, 255, 255, 0.1);
            color: white;
            font-size: 14px;
        }
        .form-input::placeholder {
            color: rgba(255, 255, 255, 0.6);
        }
        .form-textarea {
            width: 100%;
            padding: 10px;
            border-radius: 5px;
            border: 1px solid #333399;
            background: rgba(255, 255, 255, 0.1);
            color: white;
            font-size: 14px;
            min-height: 100px;
            resize: vertical;
        }
        .form-dropdown {
            width: 100%;
            padding: 10px;
            border-radius: 5px;
            border: 1px solid #333399;
            background: rgba(255, 255, 255, 0.1);
            color: white;
            font-size: 14px;
        }
        .form-buttons {
            text-align: center;
            margin-top: 30px;
        }
        .btn-submit {
            padding: 12px 40px;
            background: #333399;
            color: white;
            border: none;
            border-radius: 25px;
            cursor: pointer;
            font-size: 16px;
            margin-right: 10px;
        }
        .btn-submit:hover {
            background: #4444aa;
        }
        .btn-reset {
            padding: 12px 40px;
            background: rgba(255, 255, 255, 0.2);
            color: white;
            border: 1px solid rgba(255, 255, 255, 0.3);
            border-radius: 25px;
            cursor: pointer;
            font-size: 16px;
        }
        .btn-reset:hover {
            background: rgba(255, 255, 255, 0.3);
        }
        .message {
            text-align: center;
            padding: 15px;
            margin: 20px 0;
            border-radius: 5px;
        }
        .message-success {
            background: rgba(0, 255, 0, 0.2);
            color: #00ff00;
            border: 1px solid #00ff00;
        }
        .message-error {
            background: rgba(255, 0, 0, 0.2);
            color: #ff6b6b;
            border: 1px solid #ff6b6b;
        }
        .file-upload {
            padding: 10px;
            border-radius: 5px;
            border: 1px solid #333399;
            background: rgba(255, 255, 255, 0.1);
            color: white;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="add-movie-container">
        <div class="add-movie-header">
            <h1>Add New Movie</h1>
        </div>

        <asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="message"></asp:Label>

        <center>
            <table class="form-table">
                <tr>
                    <td class="form-label">
                        <asp:Label ID="lblTitle" runat="server" Text="Title *" ForeColor="White" Font-Size="14pt"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="form-input" placeholder="Enter movie title"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle" 
                            ErrorMessage="Title is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>

                <tr>
                    <td class="form-label">
                        <asp:Label ID="lblDescription" runat="server" Text="Description" ForeColor="White" Font-Size="14pt"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-textarea" TextMode="MultiLine" 
                            placeholder="Enter movie description"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td class="form-label">
                        <asp:Label ID="lblYear" runat="server" Text="Year *" ForeColor="White" Font-Size="14pt"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtYear" runat="server" CssClass="form-input" placeholder="e.g., 2024" TextMode="Number"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvYear" runat="server" ControlToValidate="txtYear" 
                            ErrorMessage="Year is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rvYear" runat="server" ControlToValidate="txtYear" 
                            Type="Integer" MinimumValue="1900" MaximumValue="2100" 
                            ErrorMessage="Year must be between 1900 and 2100" ForeColor="Red" Display="Dynamic"></asp:RangeValidator>
                    </td>
                </tr>

                <tr>
                    <td class="form-label">
                        <asp:Label ID="lblGenre" runat="server" Text="Genre *" ForeColor="White" Font-Size="14pt"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlGenre" runat="server" CssClass="form-dropdown">
                            <asp:ListItem Value="" Text="-- Select Genre --"></asp:ListItem>
                            <asp:ListItem Value="Action" Text="Action"></asp:ListItem>
                            <asp:ListItem Value="Comedy" Text="Comedy"></asp:ListItem>
                            <asp:ListItem Value="Drama" Text="Drama"></asp:ListItem>
                            <asp:ListItem Value="Horror" Text="Horror"></asp:ListItem>
                            <asp:ListItem Value="Sci-Fi" Text="Sci-Fi"></asp:ListItem>
                            <asp:ListItem Value="Thriller" Text="Thriller"></asp:ListItem>
                            <asp:ListItem Value="Romance" Text="Romance"></asp:ListItem>
                            <asp:ListItem Value="Adventure" Text="Adventure"></asp:ListItem>
                            <asp:ListItem Value="Animation" Text="Animation"></asp:ListItem>
                            <asp:ListItem Value="Documentary" Text="Documentary"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvGenre" runat="server" ControlToValidate="ddlGenre" 
                            InitialValue="" ErrorMessage="Genre is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>

                <tr>
                    <td class="form-label">
                        <asp:Label ID="lblRating" runat="server" Text="Rating" ForeColor="White" Font-Size="14pt"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtRating" runat="server" CssClass="form-input" placeholder="e.g., 8.5" TextMode="Number" step="0.1"></asp:TextBox>
                        <asp:RangeValidator ID="rvRating" runat="server" ControlToValidate="txtRating" 
                            Type="Double" MinimumValue="0" MaximumValue="10" 
                            ErrorMessage="Rating must be between 0 and 10" ForeColor="Red" Display="Dynamic"></asp:RangeValidator>
                    </td>
                </tr>

                <tr>
                    <td class="form-label">
                        <asp:Label ID="lblDirector" runat="server" Text="Director" ForeColor="White" Font-Size="14pt"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDirector" runat="server" CssClass="form-input" placeholder="Enter director name"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td class="form-label">
                        <asp:Label ID="lblActors" runat="server" Text="Actors" ForeColor="White" Font-Size="14pt"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtActors" runat="server" CssClass="form-input" placeholder="Enter actor names separated by commas"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td class="form-label">
                        <asp:Label ID="lblDuration" runat="server" Text="Duration (min)" ForeColor="White" Font-Size="14pt"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDuration" runat="server" CssClass="form-input" placeholder="e.g., 120" TextMode="Number"></asp:TextBox>
                        <asp:RangeValidator ID="rvDuration" runat="server" ControlToValidate="txtDuration" 
                            Type="Integer" MinimumValue="1" MaximumValue="999" 
                            ErrorMessage="Duration must be between 1 and 999 minutes" ForeColor="Red" Display="Dynamic"></asp:RangeValidator>
                    </td>
                </tr>

                <tr>
                    <td class="form-label">
                        <asp:Label ID="lblPoster" runat="server" Text="Poster Image" ForeColor="White" Font-Size="14pt"></asp:Label>
                    </td>
                    <td>
                        <asp:FileUpload ID="filePoster" runat="server" CssClass="file-upload" />
                        <br />
                        <small style="color: rgba(255, 255, 255, 0.7);">Upload an image file (will be saved to images/uploads/)</small>
                        <br />
                        <asp:Label ID="lblPosterPath" runat="server" Visible="false" style="color: rgba(255, 255, 255, 0.7); font-size: 12px;"></asp:Label>
                    </td>
                </tr>
            </table>

            <div class="form-buttons">
                <asp:Button ID="btnSubmit" runat="server" Text="Add Movie" CssClass="btn-submit" OnClick="btnSubmit_Click" />
                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn-reset" OnClick="btnReset_Click" CausesValidation="false" />
            </div>
        </center>
    </div>
</asp:Content>

