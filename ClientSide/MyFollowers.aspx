<%@ Page Title="My Followers" Language="C#" MasterPageFile="~/Design.master" 
    AutoEventWireup="true" CodeFile="MyFollowers.aspx.cs" Inherits="MyNetwork" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .network-wrapper {
            padding: 50px 20px;
            max-width: 1000px;
            margin: 0 auto;
        }

        .network-header {
            text-align: center;
            margin-bottom: 40px;
        }

        .network-header h1 {
            color: white;
            font-size: 36px;
            font-weight: 700;
            text-transform: uppercase;
        }

        .network-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 40px;
        }

        .network-column {
            background: rgba(30, 30, 30, 0.8);
            border: 1px solid rgba(255, 255, 255, 0.1);
            border-radius: 15px;
            padding: 20px;
            min-height: 400px;
        }

        .column-title {
            color: #ff4c3b;
            font-size: 20px;
            font-weight: 600;
            margin-bottom: 20px;
            text-align: center;
            text-transform: uppercase;
            border-bottom: 1px solid #444;
            padding-bottom: 10px;
        }

        .user-list-item {
            display: flex;
            align-items: center;
            padding: 10px;
            border-bottom: 1px solid #444;
            transition: background 0.2s;
            text-decoration: none;
        }

        .user-list-item:hover {
            background: rgba(255, 255, 255, 0.05);
        }

        .user-pic {
            width: 40px;
            height: 40px;
            border-radius: 50%;
            object-fit: cover;
            margin-right: 15px;
            border: 2px solid #555;
        }

        .user-info {
            display: flex;
            flex-direction: column;
        }

        .username {
            color: white;
            font-weight: bold;
            font-size: 14px;
        }

        .empty-msg {
            color: #888;
            text-align: center;
            font-style: italic;
            padding: 20px;
        }

        @media (max-width: 768px) {
            .network-grid { grid-template-columns: 1fr; }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="network-wrapper">
        <div class="network-header">
            <h1>My Network</h1>
        </div>

        <div class="network-grid">
            <div class="network-column">
                <div class="column-title">Following</div>
                <asp:Repeater ID="rptFollowing" runat="server">
                    <ItemTemplate>
                        <a href='UserProfile.aspx?username=<%# Eval("FollowingUser") %>'
                            class="user-list-item">
                            <img src="MyPics/Profile.jpg" class="user-pic" />
                            <div class="user-info">
                                <span class="username">@<%# Eval("FollowingUser") %></span>
                            </div>
                        </a>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Label ID="lblNoFollowing" runat="server" CssClass="empty-msg"
                    Text="You are not following anyone." Visible="false"></asp:Label>
            </div>

            <div class="network-column">
                <div class="column-title">Followers</div>
                <asp:Repeater ID="rptFollowers" runat="server">
                    <ItemTemplate>
                        <a href='UserProfile.aspx?username=<%# Eval("FollowerUser") %>'
                            class="user-list-item">
                            <img src="MyPics/Profile.jpg" class="user-pic" />
                            <div class="user-info">
                                <span class="username">@<%# Eval("FollowerUser") %></span>
                            </div>
                        </a>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Label ID="lblNoFollowers" runat="server" CssClass="empty-msg" 
                    Text="You have no followers yet." Visible="false"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>