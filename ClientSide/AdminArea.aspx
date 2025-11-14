<%@ Page Title="" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="AdminArea.aspx.cs" Inherits="AdminArea" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <center>
        <h1>Admin Area</h1>
      <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Login.aspx">log out</asp:HyperLink>   
      &nbsp;|
      <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/SearchUsers.aspx" >Search Users</asp:HyperLink>
      &nbsp;|
      <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/ManageCelebs.aspx" >Manage Celebs</asp:HyperLink>
      &nbsp;|
      <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/ManageMovies.aspx" >Manage Movies</asp:HyperLink>
<center>


</asp:Content>

