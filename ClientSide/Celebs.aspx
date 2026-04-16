<%@ Page Title="Celebs" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true"
    CodeFile="Celebs.aspx.cs" Inherits="Celebs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .celebs-wrapper {
            padding: 40px 20px;
        }
        .celebs-title {
            color: white;
            font-size: 32px;
            margin-bottom: 20px;
            text-align: center;
        }
        .celebs-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
            gap: 24px;
        }
        .celeb-card {
            background: rgba(0, 0, 0, 0.6);
            border-radius: 8px;
            overflow: hidden;
            color: white;
            border: 1px solid rgba(255, 255, 255, 0.1);
        }
        .celeb-photo {
            width: 100%;
            height: 260px;
            object-fit: cover;
            display: block;
        }
        .celeb-info {
            padding: 12px 16px 16px 16px;
        }
        .celeb-name {
            font-size: 18px;
            font-weight: bold;
            margin-bottom: 4px;
        }
        .celeb-role {
            font-size: 14px;
            opacity: 0.8;
            margin-bottom: 8px;
        }
        .celeb-bio {
            font-size: 13px;
            line-height: 1.4;
            max-height: 70px;
            overflow: hidden;
            text-overflow: ellipsis;
        }
        .celebs-toolbar {
            margin: 0 auto 25px auto;
            max-width: 700px;
            display: flex;
            gap: 10px;
            justify-content: center;
            flex-wrap: wrap;
        }
        .celebs-toolbar input[type=text],
        .celebs-toolbar select {
            padding: 6px 10px;
            border-radius: 4px;
            border: 1px solid #333399;
            font-size: 13px;
        }
        .celebs-toolbar button,
        .celebs-toolbar .asp-button {
            padding: 6px 14px;
            border-radius: 4px;
            border: none;
            background-color: #333399;
            color: white;
            font-size: 13px;
            cursor: pointer;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="celebs-wrapper">
        <h1 class="celebs-title">Celebrities</h1>
        <div class="celebs-toolbar">
            <asp:TextBox ID="txtSearchCelebs" runat="server" Width="220px" placeholder="Search by name or bio..."></asp:TextBox>
            <asp:DropDownList ID="ddlRole" runat="server" Width="140px">
                <asp:ListItem Value="all">All roles</asp:ListItem>
                <asp:ListItem Value="Actor">Actor</asp:ListItem>
                <asp:ListItem Value="Actress">Actress</asp:ListItem>
                <asp:ListItem Value="Director">Director</asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnSearchCelebs" runat="server" Text="Search" CssClass="asp-button" OnClick="btnSearchCelebs_Click" />
        </div>
        <div id="celebsGrid" runat="server" class="celebs-grid"></div>
    </div>
</asp:Content>
