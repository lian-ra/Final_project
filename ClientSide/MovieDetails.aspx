<%@ Page Title="Movie Details" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true"
    CodeFile="MovieDetails.aspx.cs" Inherits="MovieDetails" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <style type="text/css">
            .details-wrapper {
                padding: 40px 20px;
                color: white;
            }

            .details-layout {
                display: flex;
                flex-wrap: wrap;
                gap: 30px;
            }

            .details-poster {
                flex: 0 0 260px;
            }

            .details-poster img {
                width: 100%;
                max-width: 260px;
                border-radius: 8px;
                object-fit: cover;
            }

            .details-main {
                flex: 1 1 300px;
            }

            .details-title {
                font-size: 32px;
                margin-bottom: 8px;
            }

            .details-meta {
                font-size: 14px;
                opacity: 0.8;
                margin-bottom: 15px;
            }

            .details-rating {
                font-size: 16px;
                color: #ffd700;
                margin-bottom: 15px;
            }

            /* Star rating display */
            .star-rating {
                display: flex;
                align-items: center;
                gap: 3px;
                margin-bottom: 15px;
            }

            .star {
                font-size: 26px;
                line-height: 1;
                transition: transform 0.15s ease;
            }

            .star-full {
                color: #ffd700;
                text-shadow: 0 0 6px rgba(255, 215, 0, 0.5);
            }

            .star-half {
                background: linear-gradient(90deg, #ffd700 50%, #444 50%);
                -webkit-background-clip: text;
                -webkit-text-fill-color: transparent;
                background-clip: text;
            }

            .star-empty {
                color: #444;
            }

            .star-score {
                margin-left: 8px;
                font-size: 20px;
                font-weight: 700;
                color: #ffd700;
                letter-spacing: 0.5px;
            }

            .details-section-title {
                font-size: 18px;
                margin-top: 20px;
                margin-bottom: 8px;
                border-bottom: 1px solid rgba(255, 255, 255, 0.2);
                padding-bottom: 4px;
            }

            .details-description {
                font-size: 14px;
                line-height: 1.6;
            }

            .details-actors {
                font-size: 14px;
                line-height: 1.5;
            }

            .actor-badge {
                display: inline-block;
                margin: 3px 6px 3px 0;
                padding: 4px 10px;
                border-radius: 15px;
                background: rgba(255, 255, 255, 0.1);
                border: 1px solid rgba(255, 255, 255, 0.2);
                font-size: 13px;
                color: white;
                text-decoration: none;
                cursor: pointer;
                transition: all 0.3s ease;
            }

            .actor-badge:hover {
                background: rgba(255, 255, 255, 0.2);
                border-color: rgba(255, 255, 255, 0.4);
                transform: translateY(-2px);
            }

            .details-actions {
                margin-top: 25px;
            }

            .details-actions a {
                display: inline-block;
                margin-right: 10px;
                padding: 8px 18px;
                border-radius: 20px;
                text-decoration: none;
                font-size: 13px;
            }

            .btn-back {
                background: transparent;
                border: 1px solid #fff;
                color: #fff;
            }

            .btn-wishlist {
                background: #ff6b6b;
                border: none;
                color: #fff;
            }

            .btn-watched {
                background: #4ecdc4;
                border: none;
                color: #fff;
            }

            .btn-watched.applied {
                background: #333;
                color: #aaa;
                border: 1px solid #555;
            }

            .details-error {
                text-align: center;
                padding: 40px 0;
                color: white;
            }

            .comments-wrapper {
                margin-top: 40px;
            }

            .comments-title {
                font-size: 20px;
                margin-bottom: 10px;
                border-bottom: 1px solid rgba(255, 255, 255, 0.3);
                padding-bottom: 5px;
            }

            .comment-form {
                margin-bottom: 20px;
            }

            .comment-form textarea {
                width: 100%;
                min-height: 100px;
                padding: 12px;
                border-radius: 8px;
                border: 1px solid rgba(51, 51, 153, 0.6);
                background: rgba(255, 255, 255, 0.07);
                font-size: 14px;
                color: white;
                resize: vertical;
                transition: border-color 0.2s;
            }

            .comment-form textarea:focus {
                outline: none;
                border-color: #333399;
                background: rgba(255, 255, 255, 0.1);
            }

            .comment-form textarea::placeholder {
                color: rgba(255, 255, 255, 0.4);
            }

            .comment-actions {
                margin-top: 12px;
                display: flex;
                align-items: center;
                gap: 16px;
                flex-wrap: wrap;
            }

            /* ---- Interactive star input (review form) ---- */
            .star-input-label {
                color: rgba(255,255,255,0.7);
                font-size: 14px;
                font-weight: 500;
            }

            .star-input-group {
                display: flex;
                flex-direction: row-reverse;
                gap: 4px;
            }

            .star-input-group input[type="radio"] {
                display: none;
            }

            .star-input-group label {
                font-size: 28px;
                color: #444;
                cursor: pointer;
                transition: color 0.15s, transform 0.15s;
                line-height: 1;
            }

            /* Hover: highlight hovered star and all siblings after it (row-reverse trick) */
            .star-input-group label:hover,
            .star-input-group label:hover ~ label {
                color: #ffd700;
                transform: scale(1.15);
            }

            /* Checked: keep selected stars gold */
            .star-input-group input:checked ~ label {
                color: #ffd700;
            }

            .btn-comment {
                padding: 8px 20px;
                border-radius: 20px;
                border: none;
                background: linear-gradient(135deg, #333399, #6633cc);
                color: white;
                font-size: 14px;
                font-weight: 600;
                cursor: pointer;
                letter-spacing: 0.5px;
                transition: opacity 0.2s, transform 0.2s;
                margin-left: auto;
            }

            .btn-comment:hover {
                opacity: 0.9;
                transform: translateY(-1px);
            }

            /* ---- Review star display (in comment cards) ---- */
            .review-stars {
                display: inline-flex;
                gap: 2px;
                vertical-align: middle;
                margin-left: 4px;
            }

            .review-stars .rs {
                font-size: 14px;
            }

            .review-stars .rs-full  { color: #ffd700; }
            .review-stars .rs-empty { color: #444; }

            .comment-actions select {
                display: none; /* keep hidden; updated by JS */
            }

            .comments-message {
                margin-top: 6px;
                font-size: 12px;
            }
        </style>
    </asp:Content>

    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="details-wrapper">
            <asp:PlaceHolder ID="phDetails" runat="server"></asp:PlaceHolder>

            <div class="comments-wrapper" style="margin-top: 20px;">
                <div class="comments-title">Watched By</div>
                <div style="display: flex; gap: 10px; flex-wrap: wrap; margin-top: 10px;">
                    <asp:Repeater ID="rptWatchedUsers" runat="server">
                        <ItemTemplate>
                            <div title='<%# Eval("Username") %>' style="text-align: center;">
                                <a href='UserProfile.aspx?username=<%# Eval("Username") %>'>
                                    <asp:Image ID="imgUserWatched" runat="server"
                                        ImageUrl='<%# ResolveUrl("~/MyPics/" + Eval("pic")) %>' CssClass="user-avatar"
                                        Style="width: 40px; height: 40px; border-radius: 50%; border: 2px solid #555; object-fit: cover;" />
                                </a>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Label ID="lblNoWatched" runat="server" Text="No one has watched this yet." Visible="false"
                        ForeColor="#ffffff" Font-Size="13px"></asp:Label>
                </div>
            </div>

            <div class="comments-wrapper" style="margin-top: 20px;">
                <div class="comments-title">In Wishlist of</div>
                <div style="display: flex; gap: 10px; flex-wrap: wrap; margin-top: 10px;">
                    <asp:Repeater ID="rptWishlistUsers" runat="server">
                        <ItemTemplate>
                            <div title='<%# Eval("Username") %>' style="text-align: center;">
                                <a href='UserProfile.aspx?username=<%# Eval("Username") %>'>
                                    <asp:Image ID="imgUserWishlist" runat="server"
                                        ImageUrl='<%# ResolveUrl("~/MyPics/" + Eval("pic")) %>' CssClass="user-avatar"
                                        Style="width: 40px; height: 40px; border-radius: 50%; border: 2px solid #555; object-fit: cover;" />
                                </a>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Label ID="lblNoWishlist" runat="server" Text="No one has this in their wishlist."
                        Visible="false" ForeColor="#ffffff" Font-Size="13px"></asp:Label>
                </div>
            </div>

            <div class="comments-wrapper">
                <div class="comments-title">Comments</div>
                <asp:Panel ID="pnlCommentForm" runat="server" CssClass="comment-form">
                    <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" placeholder="Share your thoughts about this movie..."></asp:TextBox>
                    <div class="comment-actions">
                        <span class="star-input-label">Your Rating:</span>
                        <div class="star-input-group" id="starInputGroup">
                            <input type="radio" id="star5" name="reviewRating" value="5"/><label for="star5" title="5 stars">&#9733;</label>
                            <input type="radio" id="star4" name="reviewRating" value="4"/><label for="star4" title="4 stars">&#9733;</label>
                            <input type="radio" id="star3" name="reviewRating" value="3" checked="checked"/><label for="star3" title="3 stars">&#9733;</label>
                            <input type="radio" id="star2" name="reviewRating" value="2"/><label for="star2" title="2 stars">&#9733;</label>
                            <input type="radio" id="star1" name="reviewRating" value="1"/><label for="star1" title="1 star">&#9733;</label>
                        </div>
                        <!-- Hidden ASP.NET dropdown still read by server-side code -->
                        <asp:DropDownList ID="ddlRating" runat="server">
                            <asp:ListItem Value="5">5</asp:ListItem>
                            <asp:ListItem Value="4">4</asp:ListItem>
                            <asp:ListItem Value="3" Selected="True">3</asp:ListItem>
                            <asp:ListItem Value="2">2</asp:ListItem>
                            <asp:ListItem Value="1">1</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Button ID="btnAddComment" runat="server" Text="Post Review" CssClass="btn-comment"
                            OnClick="btnAddComment_Click" />
                    </div>
                    <asp:Label ID="lblCommentMessage" runat="server" CssClass="comments-message" Visible="false">
                    </asp:Label>
                </asp:Panel>
                <div class="comments-list">
                    <asp:PlaceHolder ID="phComments" runat="server"></asp:PlaceHolder>
                </div>
            </div>
        </div>

        <script type="text/javascript">
            // Sync star radio buttons -> hidden ASP.NET DropDownList
            (function () {
                var radios = document.querySelectorAll('#starInputGroup input[type="radio"]');
                var ddl = document.getElementById('<%= ddlRating.ClientID %>');

                function syncToDdl() {
                    for (var i = 0; i < radios.length; i++) {
                        if (radios[i].checked) {
                            for (var j = 0; j < ddl.options.length; j++) {
                                if (ddl.options[j].value === radios[i].value) {
                                    ddl.selectedIndex = j;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }

                for (var i = 0; i < radios.length; i++) {
                    radios[i].addEventListener('change', syncToDdl);
                }

                // Set initial sync (star3 checked by default = value 3)
                syncToDdl();
            })();
        </script>
    </asp:Content>