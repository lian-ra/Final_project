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
                min-height: 80px;
                padding: 8px;
                border-radius: 4px;
                border: 1px solid #333399;
                font-size: 13px;
                color: black;
            }

            .comment-actions {
                margin-top: 8px;
                display: flex;
                align-items: center;
                gap: 10px;
            }

            .comment-actions select {
                padding: 4px 6px;
                border-radius: 4px;
                border: 1px solid #333399;
                font-size: 13px;
            }

            .btn-comment {
                padding: 6px 14px;
                border-radius: 4px;
                border: none;
                background-color: #333399;
                color: white;
                font-size: 13px;
                cursor: pointer;
            }

            .comments-list {
                margin-top: 10px;
            }

            .comment-item {
                padding: 8px 10px;
                border-radius: 4px;
                background: rgba(0, 0, 0, 0.5);
                border: 1px solid rgba(255, 255, 255, 0.1);
                margin-bottom: 8px;
                font-size: 13px;
            }

            .comment-meta {
                opacity: 0.8;
                font-size: 11px;
                margin-bottom: 4px;
            }

            .comment-text {
                white-space: pre-wrap;
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
                    <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine"></asp:TextBox>
                    <div class="comment-actions">
                        <span>Rating:</span>
                        <asp:DropDownList ID="ddlRating" runat="server">
                            <asp:ListItem Value="5">5</asp:ListItem>
                            <asp:ListItem Value="4">4</asp:ListItem>
                            <asp:ListItem Value="3">3</asp:ListItem>
                            <asp:ListItem Value="2">2</asp:ListItem>
                            <asp:ListItem Value="1">1</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Button ID="btnAddComment" runat="server" Text="Add Comment" CssClass="btn-comment"
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
    </asp:Content>