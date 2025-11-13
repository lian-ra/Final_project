<%@ Page Title="" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 469px;
        }
        .auto-style2 {
            width: 960px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

   <center>
<h1 style="color: #FFFFFF"white">Login</h1>

<table>
<tr>
<td class="auto-style1">
<asp:Label ID="Label1" runat="server" Text="Username" ForeColor="White" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
<td class="auto-style2">
<asp:TextBox ID="txtuser" runat="server" Width="337px" ></asp:TextBox></td>
</tr>
<tr>
<td class="auto-style1">
<asp:Label ID="Label2" runat="server" Text="Password" ForeColor="White" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
<td class="auto-style2">
<asp:TextBox ID="txtpass" runat="server" Width="337px"></asp:TextBox></td>
</tr>
    <tr>
        <td class="auto-style1">
            <asp:Label ID="Label3" runat="server" Text="Choose"  ForeColor="white" Font-Bold="True" Font-Size="X-Large"> </asp:Label></td>
        <td class="auto-style2">
            <asp:DropDownList ID="drpChoice" runat="server" Width="366px">
                <asp:ListItem>user</asp:ListItem>
                <asp:ListItem>admin</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
<tr>
  <td <%--class="auto-style1"--%>>
      <asp:Button ID="btnsi" runat="server" Text="Sign In" OnClick="btnsi_Click" Width="109px"   /></td>
  <td <%--class="auto-style2"--%>>
      <asp:Button ID="btnsu2" runat="server" Text="Sign Up" Width="109px" OnClick="btnsu2_Click" /></td>
</tr>
</table>

</center>

</asp:Content>

