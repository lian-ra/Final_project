<%@ Page Title="" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="UpdateMyUser.aspx.cs" Inherits="UpdateMyUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
    <center>
        <h1 style="color: #FFFFFF; margin-top: 60px;"> Update </h1>
     <table>
         <tr>
             <td>
                 <asp:Label ID="TextBox3" runat="server" Text="Username" ForeColor="White" Font-Size="14pt"></asp:Label>
             </td>
             <td>
                <asp:TextBox ID="txtUName" Readonly="true" runat="server" Width="295px"></asp:TextBox>
             </td
         </tr>

         <tr>
             <td>
                 <asp:Label ID="TextBox4" runat="server" Text="Password" ForeColor="White" Font-Size="14pt"></asp:Label>
             </td>
             <td>
                  <asp:TextBox  ID="txtPass"  runat="server" Width="295px"></asp:TextBox>
             </td>
         </tr>

         <tr>
             <td>
                  <asp:Label  ID="TextBox5" runat="server" Text="Name" ForeColor="White" Font-Size="14pt"></asp:Label>
             </td>
             <td>
                  <asp:TextBox ID="txtFName" runat="server" Width="294px"></asp:TextBox>
             </td>
         </tr>

         <tr>
             <td>
                  <asp:Label ID="TextBox6" runat="server" Text="Lastname" ForeColor="White" Font-Size="14pt"></asp:Label>
             </td>
             <td>
                 <asp:TextBox  ID="txtLName" runat="server" Width="294px"></asp:TextBox>
             </td>
         </tr>

          <tr>
     <td>
          <asp:Label ID="TextBox1" runat="server" Text="Adderss" ForeColor="White" Font-Size="14pt"></asp:Label>
     </td>
     <td>
         <asp:TextBox  ID="txtAdd"  runat="server" Width="294px"></asp:TextBox>
     </td>
 </tr>


       <tr>
          <td>
            <asp:Label ID="TextBox7" runat="server" Text="Email" ForeColor="White" Font-Size="14pt"></asp:Label>
           </td>
            <td>
                <asp:TextBox  ID="txtEmail" runat="server" Width="293px"></asp:TextBox>
           </td>
        </tr>

         <tr>
           <td>
              <asp:Label  ID="TextBox8" runat="server" Text="Phone" ForeColor="White" Font-Size="14pt"></asp:Label>
           </td>
           <td>
              <asp:TextBox ID="txtPhone" runat="server" Width="294px"></asp:TextBox>
          </td>
     </tr>

  <%--   <tr>
       <td>
          <asp:Label ID="Label1" runat="server" Text="Gender" Font-Bold="True" Font-Size="Large" ForeColor="White"></asp:Label>
          </td>
       <td>
            <asp:DropDownList ID="drpGender" runat="server" Width="319px"  Height="30px">
       <asp:ListItem>Female</asp:ListItem>
       <asp:ListItem>Male</asp:ListItem>
   </asp:DropDownList>
      </td>
    </tr>--%>

     <%--<tr>
       <td>
         <asp:Label ID="TextBox10" runat="server" Text="Birthday" ForeColor="White" Font-Size="14pt"></asp:Label>
      </td>
       <td>
          <asp:TextBox ID="txtBirth"  runat="server" Width="295px"></asp:TextBox>
       </td>
     </tr>--%>

         <tr>
            <td>
                 <asp:Image ID="img" runat="server" Height="197px" ImageUrl="~/MyPics/Profile.jpg" Width="166px" />
                 
            </td>
             <td>
                 &nbsp;</td>
         </tr>

         <tr>
             <td>

                 <asp:FileUpload ID="FileUpload1" runat="server" Width="546px" />

             </td>
         </tr>
         <tr>
             <td>  <asp:Button runat="server"  Text="Update me" ID="btnUpdate" OnClick="btnUpdate_Click" Width="248px"   /></td>
         </tr>

        

     </table>

 </center>
</asp:Content>

