<%@ Page Title="" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<center style="display: flex; flex-direction: column; justify-content: center; align-items: center; width: 100vw; height: 100vh;">
<h1 style="color: #FFFFFF; margin-bottom: 50px;">Login</h1>

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
<tr style="display: flex; flex-direction: row; margin-top: 40px; align-items: center; justify-content: center;">
  <td <%--class="auto-style1"--%>>
      <asp:Button ID="btnsi" runat="server" Text="Sign In" OnClick="btnsi_Click" Width="109px" style="padding: 12px; border-radius: 20px; background: white; font-weight: 800;"   /></td>
  <td <%--class="auto-style2"--%>>
      <asp:Button ID="btnsu2" runat="server" Text="Sign Up" Width="109px" OnClick="btnsu2_Click" style="padding: 12px; border-radius: 20px; background: red; color: white; font-weight: 700;" /></td>
</tr>
</table>

</center>

</asp:Content>

