<%@ Page Title="" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="ClientArea.aspx.cs" Inherits="ClientArea" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <center style=" margin-top: 70px;">
        <h1>
            User
        </h1>

        <table>
            <tr><td><a href="UpdateMyUser.aspx">
                <asp:Image ID="imgUpdateUser" runat="server" Height="172px" ImageUrl="~/MyPics/update_user.jpg" Width="181px" />  </a>

                </td></tr>

            <tr><td>  <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/UpdateMyUser.aspx">Update User</asp:HyperLink></td></tr>

            <tr> 
                <td>
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Login.aspx">Log out</asp:HyperLink></td></tr>
                </td>
            </tr>

        </table>

      
       
    </center>
</asp:Content>

