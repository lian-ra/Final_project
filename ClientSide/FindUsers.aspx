<%@ Page Title="Find Users" Language="C#" MasterPageFile="~/Design.master"
    AutoEventWireup="true" CodeFile="FindUsers.aspx.cs" Inherits="FindUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .community-wrapper {
            padding: 50px 20px;
            max-width: 1200px;
            margin: 0 auto;
        }

        .page-header {
            text-align: center;
            margin-bottom: 50px;
        }

        .page-header h1 {
            color: white;
            font-size: 42px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 2px;
            margin-bottom: 10px;
        }

        .page-header p {
            color: #aaa;
            font-size: 16px;
        }

        .search-container {
            display: flex;
            justify-content: center;
            gap: 10px;
            margin-bottom: 50px;
        }

        .search-input {
            width: 300px;
            padding: 12px 20px;
            border-radius: 30px;
            border: 1px solid #444;
            background: rgba(255, 255, 255, 0.1);
            color: white;
            outline: none;
            transition: all 0.3s;
        }

        .search-input:focus {
            background: rgba(255, 255, 255, 0.2);
            border-color: #ff4c3b;
        }

        .btn-search {
            padding: 12px 30px;
            border-radius: 30px;
            background: #ff4c3b;
            color: white;
            border: none;
            font-weight: bold;
            cursor: pointer;
            transition: transform 0.2s;
        }

        .btn-search:hover {
            transform: scale(1.05);
            background: #e04332;
        }

        .profiles-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
            gap: 30px;
        }

        .profile-card {
            background: rgba(30, 30, 30, 0.8);
            border: 1px solid rgba(255, 255, 255, 0.05);
            border-radius: 15px;
            padding: 30px 20px;
            text-align: center;
            transition: transform 0.3s, box-shadow 0.3s;
            position: relative;
            overflow: hidden;
        }

        .profile-card:hover {
            transform: translateY(-10px);
            box-shadow: 0 15px 30px rgba(0, 0, 0, 0.5);
            border-color: #ff4c3b;
        }

        .profile-img-container {
            width: 100px;
            height: 100px;
            margin: 0 auto 20px;
            border-radius: 50%;
            overflow: hidden;
            border: 3px solid #333;
            transition: border-color 0.3s;
        }

        .profile-card:hover .profile-img-container {
            border-color: #ff4c3b;
        }

        .profile-img {
            width: 100%;
            height: 100%;
            object-fit: cover;
        }

        .profile-name {
            color: white;
            font-size: 18px;
            font-weight: 700;
            margin-bottom: 5px;
        }

        .profile-username {
            color: #ff4c3b;
            font-size: 14px;
            font-weight: 600;
            margin-bottom: 15px;
            display: block;
        }

        .profile-detail {
            color: #bbb;
            font-size: 13px;
            margin-bottom: 5px;
        }

        .no-results {
            text-align: center;
            color: #aaa;
            grid-column: 1 / -1;
            padding: 50px;
            font-size: 18px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  
    <div class="community-wrapper">
        <div class="page-header">
            <h1>Community</h1>
            <p>Discover other movie lovers</p>
        </div>

        <div class="search-container">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="search-input" 
                placeholder="Search by name or username..."></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="Find" OnClick="btnSearch_Click"
                CssClass="btn-search" />
        </div>

        <div class="profiles-grid">
            <asp:Repeater ID="rptUsers" runat="server">
                <ItemTemplate>

                    <a class="profile-card" href='UserProfile.aspx?username=<%# Eval("User") %>'
                        class="profile-card" style="width: 100%; text-decoration:none;">
                        <div class="profile-img-container">
                            <img src='<%# ResolveUrl("~/MyPics/" + (string.IsNullOrEmpty(Eval("pic").ToString()) ?
                            "Profile.jpg" : Eval("pic"))) %>' class="profile-img" alt="User Pic" />
                        </div>
                        
                        <div class="profile-name">
                            <%# Eval("FName") %> <%# Eval("LName") %>
                        </div>
                        
                        <span class="profile-username">@<%# Eval("User") %></span>
                        
                        <div class="profile-detail">
                            <i class="fa fa-map-marker"></i> <%# string.IsNullOrEmpty(Eval("address").ToString()) ? 
                                                                     "Unknown" : Eval("address") %>
                        </div>
                        
                        <div class="profile-detail">
                            <%# Eval("email") %>
                        </div>
                     </a>
                </ItemTemplate>
            </asp:Repeater>
            
            <asp:Label ID="lblNoResults" runat="server" CssClass="no-results" 
                Text="No users found." Visible="false"></asp:Label>
        </div>
    </div>
   

</asp:Content>