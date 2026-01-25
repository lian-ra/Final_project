<%@ Page Title="User Profile" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="UserProfile.aspx.cs" Inherits="UserProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .profile-wrapper {
            padding: 50px 20px;
            max-width: 1200px;
            margin: 0 auto;
            display: flex;
            gap: 40px;
            align-items: flex-start;
            flex-wrap: wrap;
        }

        /* --- Left Side: User Card --- */
        .user-card {
            flex: 1;
            min-width: 300px;
            max-width: 350px;
            background: rgba(30, 30, 30, 0.9);
            border: 1px solid rgba(255, 255, 255, 0.1);
            border-radius: 15px;
            padding: 40px 20px;
            text-align: center;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.5);
            position: sticky;
            top: 100px; /* Sticks when scrolling */
        }

        .user-avatar-large {
            width: 150px;
            height: 150px;
            border-radius: 50%;
            object-fit: cover;
            border: 4px solid #ff4c3b;
            margin-bottom: 20px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.5);
        }

        .user-name {
            color: white;
            font-size: 28px;
            font-weight: 700;
            margin-bottom: 5px;
        }

        .user-handle {
            color: #ff4c3b;
            font-size: 16px;
            font-weight: 600;
            margin-bottom: 25px;
            display: block;
        }

        .user-info-row {
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 10px;
            color: #ccc;
            margin-bottom: 10px;
            font-size: 14px;
        }

        .user-info-row i {
            color: #ff4c3b;
            width: 20px;
        }

        /* --- Right Side: Wishlist --- */
        .wishlist-section {
            flex: 2;
            min-width: 300px;
        }

        .section-title {
            color: white;
            font-size: 24px;
            font-weight: 600;
            margin-bottom: 25px;
            border-left: 4px solid #ff4c3b;
            padding-left: 15px;
            text-transform: uppercase;
        }

        .movies-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
            gap: 20px;
        }

        .movie-card {
            background: rgba(255, 255, 255, 0.05);
            border-radius: 10px;
            overflow: hidden;
            transition: transform 0.3s, box-shadow 0.3s;
            position: relative;
            text-decoration: none;
            display: block;
        }

        .movie-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 10px 20px rgba(0,0,0,0.5);
            background: rgba(255, 255, 255, 0.1);
        }

        .movie-poster {
            width: 100%;
            height: 270px;
            object-fit: cover;
        }

        .movie-info {
            padding: 15px;
        }

        .movie-title {
            color: white;
            font-size: 15px;
            font-weight: 700;
            margin-bottom: 5px;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
            display: block;
        }

        .movie-meta {
            color: #aaa;
            font-size: 12px;
            display: flex;
            justify-content: space-between;
        }

        .rating-star { color: #ffd700; }

        .empty-state {
            color: #888;
            font-size: 16px;
            font-style: italic;
            background: rgba(255,255,255,0.05);
            padding: 30px;
            border-radius: 10px;
            text-align: center;
        }

        @media (max-width: 768px) {
            .profile-wrapper { flex-direction: column; }
            .user-card { width: 100%; max-width: none; position: static; }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <div class="profile-wrapper">
        
        <div class="user-card">
            <asp:Image ID="imgProfile" runat="server" CssClass="user-avatar-large" ImageUrl="~/MyPics/Profile.jpg" />
            
            <div class="user-name">
                <asp:Label ID="lblName" runat="server"></asp:Label>
            </div>
            <span class="user-handle">@<asp:Label ID="lblUsername" runat="server"></asp:Label></span>

            <hr style="border-color: rgba(255,255,255,0.1); margin: 20px 0;" />

            <div class="user-info-row">
                <i class="fa fa-envelope"></i>
                <asp:Label ID="lblEmail" runat="server"></asp:Label>
            </div>
            <div class="user-info-row">
                <i class="fa fa-map-marker"></i>
                <asp:Label ID="lblAddress" runat="server"></asp:Label>
            </div>
            
            <div id="divPhone" runat="server" class="user-info-row" visible="false">
                <i class="fa fa-phone"></i>
                <asp:Label ID="lblPhone" runat="server"></asp:Label>
            </div>
        </div>

        <div class="wishlist-section">
            <h2 class="section-title">Favorite Movies</h2>

            <asp:Repeater ID="rptWishlist" runat="server">
                <HeaderTemplate>
                    <div class="movies-grid">
                </HeaderTemplate>
                <ItemTemplate>
                    <a href='MovieDetails.aspx?movieId=<%# Eval("MovieId") %>' class="movie-card">
                        <img src='<%# ResolveUrl(Eval("Poster").ToString()) %>' alt='<%# Eval("Title") %>' class="movie-poster" />
                        <div class="movie-info">
                            <span class="movie-title"><%# Eval("Title") %></span>
                            <div class="movie-meta">
                                <span><%# Eval("Year") %></span>
                                <span><i class="fa fa-star rating-star"></i> <%# Eval("Rating") %></span>
                            </div>
                        </div>
                    </a>
                </ItemTemplate>
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>

            <asp:Label ID="lblEmptyWishlist" runat="server" CssClass="empty-state" Visible="false" Text="This user hasn't added any movies to their wishlist yet."></asp:Label>
        </div>

    </div>

</asp:Content>