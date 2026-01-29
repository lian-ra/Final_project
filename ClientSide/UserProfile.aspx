<%@ Page Title="User Profile" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true"
    CodeFile="UserProfile.aspx.cs" Inherits="UserProfile" %>

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
                top: 100px;
            }

            .user-avatar-large {
                width: 150px;
                height: 150px;
                border-radius: 50%;
                object-fit: cover;
                border: 4px solid #ff4c3b;
                margin-bottom: 20px;
                box-shadow: 0 5px 15px rgba(0, 0, 0, 0.5);
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

            .profile-stats {
                display: flex;
                justify-content: center;
                gap: 20px;
                margin: 15px 0;
                padding: 10px 0;
                border-top: 1px solid rgba(255, 255, 255, 0.1);
                border-bottom: 1px solid rgba(255, 255, 255, 0.1);
            }

            .stat-item {
                text-align: center;
            }

            .stat-value {
                display: block;
                color: white;
                font-size: 18px;
                font-weight: bold;
            }

            .stat-label {
                display: block;
                color: #aaa;
                font-size: 12px;
                text-transform: uppercase;
            }

            .btn-follow {
                width: 100%;
                padding: 10px;
                border-radius: 25px;
                border: none;
                font-weight: bold;
                cursor: pointer;
                margin-top: 10px;
                transition: all 0.3s;
            }

            .btn-follow-action {
                background-color: #ff4c3b;
                color: white;
            }

            .btn-follow-action:hover {
                background-color: #e04332;
            }

            .btn-unfollow {
                background-color: transparent;
                border: 2px solid #555;
                color: #ccc;
            }

            .btn-unfollow:hover {
                border-color: #ff4c3b;
                color: #ff4c3b;
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
                grid-template-columns: repeat(auto-fill, minmax(140px, 1fr));
                gap: 15px;
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
                box-shadow: 0 10px 20px rgba(0, 0, 0, 0.5);
                background: rgba(255, 255, 255, 0.1);
            }

            .movie-poster {
                width: 100%;
                height: 210px;
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

            .rating-star {
                color: #ffd700;
            }

            .empty-state {
                color: #888;
                font-size: 16px;
                font-style: italic;
                background: rgba(255, 255, 255, 0.05);
                padding: 30px;
                border-radius: 10px;
                text-align: center;
                display: block;
            }

            @media (max-width: 768px) {
                .profile-wrapper {
                    flex-direction: column;
                }

                .user-card {
                    width: 100%;
                    max-width: none;
                    position: static;
                }
            }

            /* --- Modal Styles --- */
            .modal-overlay {
                display: none;
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background: rgba(0, 0, 0, 0.7);
                z-index: 1000;
                justify-content: center;
                align-items: center;
            }

            .modal-content {
                background: #1e1e1e;
                padding: 30px;
                border-radius: 10px;
                width: 400px;
                max-width: 90%;
                box-shadow: 0 5px 20px rgba(0, 0, 0, 0.5);
                border: 1px solid rgba(255, 255, 255, 0.1);
            }

            .modal-title {
                margin-top: 0;
                color: white;
                margin-bottom: 20px;
                font-size: 20px;
                border-bottom: 1px solid #333;
                padding-bottom: 10px;
            }

            .form-group {
                margin-bottom: 15px;
            }

            .form-group label {
                display: block;
                color: #ccc;
                margin-bottom: 5px;
                font-size: 14px;
            }

            .form-control {
                width: 100%;
                padding: 8px;
                border-radius: 4px;
                border: 1px solid #444;
                background: #2a2a2a;
                color: white;
            }

            .modal-footer {
                margin-top: 20px;
                display: flex;
                justify-content: flex-end;
                gap: 10px;
            }
        </style>
    </asp:Content>

    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

        <div class="profile-wrapper">

            <div class="user-card">
                <asp:Image ID="imgProfile" runat="server" CssClass="user-avatar-large"
                    ImageUrl="~/MyPics/Profile.jpg" />

                <div class="user-name">
                    <asp:Label ID="lblName" runat="server"></asp:Label>
                </div>
                <span class="user-handle">@<asp:Label ID="lblUsername" runat="server"></asp:Label></span>

                <asp:Button ID="btnEditProfile" runat="server" Text="Edit Profile" OnClick="btnEditProfile_Click"
                    CssClass="btn-follow" BackColor="#444" ForeColor="white" Visible="false" />

                <div class="profile-stats">
                    <div class="stat-item">
                        <a href='MyFollowers.aspx?username=<% = Request.QueryString["username"]  %>'
                            style="text-decoration:none;">
                            <span class="stat-value">
                                <asp:Label ID="lblFollowersCount" runat="server" Text="0"></asp:Label>
                            </span>
                            <span class="stat-label">Followers</span>
                        </a>
                    </div>
                    <div class="stat-item">
                        <a href='MyFollowers.aspx?username=<% = Request.QueryString["username"] %>'
                            style="text-decoration:none;">
                            <span class="stat-value">
                                <asp:Label ID="lblFollowingCount" runat="server" Text="0"></asp:Label>
                            </span>
                            <span class="stat-label">Following</span>
                        </a>
                    </div>
                </div>

                <asp:Button ID="btnFollow" runat="server" Text="Follow" OnClick="btnFollow_Click"
                    CssClass="btn-follow btn-follow-action" Visible="false" />

                <hr style="border-color: rgba(255,255,255,0.1); margin: 20px 0;" />

                <div class="user-info-row">
                    <i class="fa fa-envelope"></i>
                    <asp:Label ID="lblEmail" runat="server"></asp:Label>
                </div>
            </div>

            <div class="profile-content-right" style="flex: 2; min-width: 300px;">

                <div class="wishlist-section">
                    <div style="display:flex; justify-content:space-between; align-items:center; margin-bottom: 25px;">
                        <h2 class="section-title" style="margin-bottom: 0;">Movies In Wishlist</h2>
                        <a href='Wishlist.aspx?username=<% = Request.QueryString["username"] ?? GetLoggedInUsername() %>'
                            style="color: white; background: rgba(255,255,255,0.1); padding: 5px 15px; border-radius: 15px; text-decoration: none; font-size: 13px; transition: background 0.3s;">
                            See All <i class="fa fa-arrow-right"></i>
                        </a>
                    </div>

                    <asp:Repeater ID="rptWishlist" runat="server">
                        <HeaderTemplate>
                            <div class="movies-grid">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <a href='MovieDetails.aspx?movieId=<%# Eval("MovieId") %>' class="movie-card">
                                <img src='<%# ResolveUrl(Eval("Poster").ToString()) %>' alt='<%# Eval("Title") %>'
                                    class="movie-poster" />
                                <div class="movie-info">
                                    <span class="movie-title">
                                        <%# Eval("Title") %>
                                    </span>
                                    <div class="movie-meta">
                                        <span>
                                            <%# Eval("Year") %>
                                        </span>
                                        <span>
                                            <%# GetStarRatingHtml(Eval("Rating")) %>
                                        </span>
                                    </div>
                                </div>
                            </a>
                        </ItemTemplate>
                        <FooterTemplate>
                </div>
                </FooterTemplate>
                </asp:Repeater>

                <asp:Label ID="lblEmptyWishlist" runat="server" CssClass="empty-state" Visible="false"
                    Text="This user hasn't added any movies to their wishlist yet."></asp:Label>
            </div>

            <div class="wishlist-section"
                style="margin-top: 40px; border-top: 1px solid rgba(255,255,255,0.1); padding-top: 40px; width: 100%;">
                <h2 class="section-title" style="border-left-color: #4ecdc4;">Watched Movies</h2>

                <asp:Repeater ID="rptWatched" runat="server">
                    <HeaderTemplate>
                        <div class="movies-grid">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <a href='MovieDetails.aspx?movieId=<%# Eval("MovieId") %>' class="movie-card">
                            <img src='<%# ResolveUrl(Eval("Poster").ToString()) %>' alt='<%# Eval("Title") %>'
                                class="movie-poster" />
                            <div class="movie-info">
                                <span class="movie-title">
                                    <%# Eval("Title") %>
                                </span>
                                <div class="movie-meta">
                                    <span>
                                        <%# Eval("Year") %>
                                    </span>
                                    <span>
                                        <%# GetStarRatingHtml(Eval("Rating")) %>
                                    </span>
                                </div>
                            </div>
                        </a>
                    </ItemTemplate>
                    <FooterTemplate>
            </div>
            </FooterTemplate>
            </asp:Repeater>

            <asp:Label ID="lblEmptyWatched" runat="server" CssClass="empty-state" Visible="false"
                Text="No movies watched yet."></asp:Label>
        </div>

        </div> <!-- End profile-content-right -->
        </div> <!-- End profile-wrapper -->

        <!-- Edit Profile Modal -->
        <asp:Panel ID="pnlEditModal" runat="server" CssClass="modal-overlay" style="display: flex;" Visible="false">
            <div class="modal-content">
                <div class="modal-title">Edit Profile</div>

                <div class="form-group">
                    <label>First Name</label>
                    <asp:TextBox ID="txtEditFName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Last Name</label>
                    <asp:TextBox ID="txtEditLName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Email</label>
                    <asp:TextBox ID="txtEditEmail" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Password</label>
                    <asp:TextBox ID="txtEditPass" runat="server" CssClass="form-control" TextMode="Password">
                    </asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Profile Picture</label>
                    <asp:FileUpload ID="fuProfilePic" runat="server" CssClass="form-control" />
                </div>

                <div class="modal-footer">
                    <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel" OnClick="btnCancelEdit_Click"
                        CssClass="btn-follow btn-unfollow" style="width:auto; padding: 8px 15px; margin-top:0;" />
                    <asp:Button ID="btnSaveProfile" runat="server" Text="Save Changes" OnClick="btnSaveProfile_Click"
                        CssClass="btn-follow btn-follow-action" style="width:auto; padding: 8px 15px; margin-top:0;" />
                </div>
            </div>
        </asp:Panel>

    </asp:Content>