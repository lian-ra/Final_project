<%@ Page Language="C#" AutoEventWireup="true" CodeFile="filmss.aspx.cs" Inherits="filmss" MasterPageFile="~/Design.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .shop-wrapper {
            max-width: 1200px;
            margin: 0 auto;
            padding: 40px 20px;
            font-family: 'Segoe UI', sans-serif;
        }

        .cards-container {
            display: flex;
            flex-wrap: wrap;
            justify-content: center;
            gap: 25px;
        }

        .cards-container table {
            width: 260px;
            background: #ffffff;
            border-radius: 18px;
            padding: 0;
            box-shadow: 0 6px 20px rgba(0,0,0,0.1);
            transition: transform 0.25s, box-shadow 0.25s;
            overflow: hidden;
            text-align: center;
        }

        .cards-container table:hover {
            transform: translateY(-6px);
            box-shadow: 0 12px 28px rgba(0,0,0,0.18);
        }

        .cards-container .image-container {
            overflow: hidden;
            border-radius: 18px 18px 0 0;
        }

        .cards-container img {
            width: 100%;
            height: 280px;
            object-fit: cover;
            transition: transform 0.3s ease;
        }

        .cards-container table:hover img {
            transform: scale(1.05);
        }

        .cards-container td.info {
            padding: 10px;
        }

        .cards-container .name {
            font-size: 18px;
            font-weight: 700;
            color: #333;
            margin-bottom: 6px;
        }

        .cards-container .rating {
            font-size: 16px;
            color: #ff4c3b;
        }

        .watch-btn {
            font-size: 16px;
            padding: 12px 0;
            width: 100%;
            color: white;
            background: #6ecfff;
            border: none;
            border-radius: 0 0 18px 18px;
            cursor: pointer;
            text-decoration: none;
            display: inline-block;
        }

        .watch-btn:hover {
            background: #4285f4;
        }

        .search-container {
            text-align: center;
            margin-bottom: 20px;
        }

        .search-box {
            width: 250px;
            padding: 8px;
            border-radius: 6px;
            border: 1px solid #ccc;
        }

        .search-btn {
            padding: 8px 15px;
            border-radius: 6px;
            background: #4285f4;
            border: none;
            color: white;
            cursor: pointer;
        }

        @media screen and (max-width: 900px) {
            .cards-container table { width: 45%; }
        }
        @media screen and (max-width: 600px) {
            .cards-container table { width: 90%; }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="shop-wrapper">
        <h1 style="text-align:center">סרטים</h1>

        <!-- חיפוש סרטים -->
        <div class="search-container">
            <asp:TextBox ID="txtSearchFilms" runat="server" CssClass="search-box" placeholder="חפש סרטים..."></asp:TextBox>
            <asp:Button ID="btnSearchFilms" runat="server" CssClass="search-btn" Text="חפש" OnClick="btnSearchFilms_Click" />
        </div>

        <!-- DataList להצגת סרטים -->
        <asp:DataList ID="dtlMovies" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" CssClass="cards-container" OnItemCommand="dtlMovies_ItemCommand">
            <ItemTemplate>
                <table>
                    <tr>
                        <td>
                            <div class="image-container">
                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Poster") %>' />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:center" class="info">
                            <div class="name">
                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                            </div>
                            <div class="rating">
                                דירוג: <asp:Label ID="lblRating" runat="server" Text='<%# Eval("Rating") %>'></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinkButton ID="btnAddWatch" runat="server" CssClass="watch-btn"
                                            CommandName="AddToWatch" CommandArgument='<%# Eval("MovieId") %>'>
                                הוסף לצפייה
                            </asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:DataList>
    </div>
</asp:Content>
