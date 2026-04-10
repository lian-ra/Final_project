<%@ Page Title="Home" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="Home.aspx.cs"
    Inherits="Home" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
        <style type="text/css">
            .films-container {
                padding: 40px 20px;
                color: white;
                min-height: 600px;
            }

            .films-header {
                text-align: center;
                margin-bottom: 40px;
            }

            .films-header h1 {
                font-size: 48px;
                margin-bottom: 10px;
                color: #ff6b6b;
            }

            .films-header p {
                font-size: 18px;
                opacity: 0.8;
            }

            .films-grid {
                display: grid;
                grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
                gap: 30px;
                margin-top: 40px;
            }

            .film-card {
                background: rgba(255, 255, 255, 0.1);
                border-radius: 10px;
                padding: 15px;
                text-align: center;
                transition: transform 0.3s, box-shadow 0.3s;
                cursor: pointer;
                backdrop-filter: blur(5px);
                border: 1px solid rgba(255, 255, 255, 0.1);
            }

            .film-card:hover {
                transform: translateY(-5px);
                box-shadow: 0 10px 30px rgba(0, 0, 0, 0.5);
                background: rgba(255, 255, 255, 0.15);
            }

            .film-poster {
                width: 100%;
                height: 320px;
                object-fit: cover;
                border-radius: 8px;
                margin-bottom: 15px;
            }

            .film-title {
                font-size: 18px;
                font-weight: bold;
                margin-bottom: 8px;
                color: white;
                white-space: nowrap;
                overflow: hidden;
                text-overflow: ellipsis;
            }

            .film-rating {
                color: #ffd700;
                font-size: 16px;
                margin-bottom: 5px;
            }

            .film-year {
                color: #ccc;
                font-size: 14px;
            }

            .search-section {
                text-align: center;
                margin-bottom: 40px;
                display: flex;
                flex-direction: column;
                align-items: center;
            }

            .search-box {
                padding: 12px 30px;
                font-size: 16px;
                width: 400px;
                max-width: 90%;
                border-radius: 25px;
                border: 2px solid #333399;
                background: rgba(255, 255, 255, 0.1);
                color: white;
            }

            .search-box::placeholder {
                color: rgba(255, 255, 255, 0.6);
            }

            .filters-row {
                display: flex;
                flex-wrap: wrap;
                justify-content: center;
                gap: 20px;
                margin-top: 20px;
                width: 100%;
            }

            .genre-filters {
                display: flex;
                justify-content: center;
                flex-wrap: wrap;
                gap: 10px;
            }

            .genre-btn {
                padding: 8px 20px;
                background: rgba(255, 255, 255, 0.1);
                border: 1px solid rgba(255, 255, 255, 0.3);
                border-radius: 20px;
                color: white;
                cursor: pointer;
                text-decoration: none;
                transition: all 0.3s;
            }

            .genre-btn:hover {
                background: rgba(255, 255, 255, 0.2);
                border-color: #ff6b6b;
                text-decoration: none;
                color: white;
            }

            .genre-btn.active {
                background: #ff6b6b;
                border-color: #ff6b6b;
            }

            /* Card action buttons row */
            .card-actions {
                display: flex;
                flex-wrap: wrap;
                gap: 6px;
                margin-top: 10px;
                justify-content: center;
            }

            .card-btn {
                display: inline-block;
                padding: 5px 12px;
                border-radius: 20px;
                font-size: 12px;
                font-weight: 600;
                text-decoration: none;
                cursor: pointer;
                transition: all 0.25s ease;
                border: none;
                white-space: nowrap;
            }

            /* Wishlist - not yet added */
            .card-btn-wishlist {
                background: rgba(255, 107, 107, 0.15);
                color: #ff6b6b;
                border: 1px solid #ff6b6b;
            }
            .card-btn-wishlist:hover {
                background: #ff6b6b;
                color: white;
                text-decoration: none;
                transform: translateY(-1px);
            }

            /* Wishlist - already added */
            .card-btn-wishlisted {
                background: #555;
                color: #bbb;
                border: 1px solid #555;
                cursor: default;
            }

            /* Watched - not yet watched */
            .card-btn-watch {
                background: rgba(78, 205, 196, 0.15);
                color: #4ecdc4;
                border: 1px solid #4ecdc4;
            }
            .card-btn-watch:hover {
                background: #4ecdc4;
                color: #111;
                text-decoration: none;
                transform: translateY(-1px);
            }

            /* Watched - already watched (click to undo) */
            .card-btn-watched-done {
                background: rgba(78, 205, 196, 0.3);
                color: #4ecdc4;
                border: 1px solid #4ecdc4;
            }
            .card-btn-watched-done:hover {
                background: #333;
                color: #aaa;
                border-color: #555;
                text-decoration: none;
            }

            /* View Details arrow button */
            .card-btn-details {
                background: rgba(51, 51, 153, 0.3);
                color: #aaaaff;
                border: 1px solid #333399;
            }
            .card-btn-details:hover {
                background: #333399;
                color: white;
                text-decoration: none;
                transform: translateY(-1px);
            }
        </style>
    </asp:Content>

    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

        <div class="films-container">
            <div class="films-header">
                <h1>Films</h1>
                <p>Discover and explore movies</p>
            </div>

            <div class="search-section">
                <div
                    style="display: flex; gap: 15px; justify-content: center; align-items: center; flex-wrap: wrap; margin-bottom: 20px;">
                    <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearchFilms"
                        style="display: flex; gap: 10px; align-items: center;">
                        <asp:TextBox ID="txtSearchFilms" runat="server" CssClass="search-box"
                            placeholder="Search for movies..." style="padding: 20px 30px; margin: 0; width: 300px;" />
                        <asp:Button ID="btnSearchFilms" runat="server" Text="Search" OnClick="btnSearchFilms_Click"
                            style="padding: 5px 30px; background: #333399; color: white; border: none; border-radius: 25px; cursor: pointer; font-size: 16px; margin: 0; min-height: 48px;" />
                        <asp:DropDownList ID="ddlSort" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlSort_SelectedIndexChanged"
                            style="padding: 8px 14px; border-radius: 25px; border: 1px solid #333399; background: white; color: black; height: 48px; font-size: 16px; margin: 0; cursor: pointer;">
                            <asp:ListItem Value="default" Selected="True">Sort: Best (rating)</asp:ListItem>
                            <asp:ListItem Value="year_desc">Year: Newest first</asp:ListItem>
                            <asp:ListItem Value="year_asc">Year: Oldest first</asp:ListItem>
                            <asp:ListItem Value="rating_desc">Rating: High to low</asp:ListItem>
                            <asp:ListItem Value="rating_asc">Rating: Low to high</asp:ListItem>
                        </asp:DropDownList>
                    </asp:Panel>


                </div>

                <div class="filters-row" style="margin-top: 5px;">
                    <div class="genre-filters">
                        <asp:LinkButton ID="btnAll" runat="server" CssClass="genre-btn active" Text="All"
                            OnClick="btnFilter_Click" CommandArgument="all" />
                        <asp:LinkButton ID="btnAction" runat="server" CssClass="genre-btn" Text="Action"
                            OnClick="btnFilter_Click" CommandArgument="action" />
                        <asp:LinkButton ID="btnComedy" runat="server" CssClass="genre-btn" Text="Comedy"
                            OnClick="btnFilter_Click" CommandArgument="comedy" />
                        <asp:LinkButton ID="btnDrama" runat="server" CssClass="genre-btn" Text="Drama"
                            OnClick="btnFilter_Click" CommandArgument="drama" />
                        <asp:LinkButton ID="btnHorror" runat="server" CssClass="genre-btn" Text="Horror"
                            OnClick="btnFilter_Click" CommandArgument="horror" />
                        <asp:LinkButton ID="btnSciFi" runat="server" CssClass="genre-btn" Text="Sci-Fi"
                            OnClick="btnFilter_Click" CommandArgument="scifi" />
                    </div>
                </div>
            </div>

            <div class="films-grid" id="filmsGrid" runat="server">
            </div>

        </div>
    </asp:Content>