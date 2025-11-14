<%@ Page Title="" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="Regi.aspx.cs" Inherits="Regi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style1 {
            height: 265px;
        }
        .auto-style2 {
            height: 48px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     
    <center style="display: flex; flex-direction: column; justify-content: center; align-items: center; margin-top: 70px;">
        <h1 style="color: #FFFFFF; margin-bottom: 25px;" > Register </h1>
     <table>
         <tr>
             <td>
                 <asp:Label ID="label1" runat="server" Text="Username" ForeColor="White" Font-Size="14pt"></asp:Label>
             </td>
             <td>
                <asp:TextBox ID="txtUName" runat="server" Width="295px"></asp:TextBox>
             </td>
         </tr>

         <tr>
             <td>
                 <asp:Label ID="label2" runat="server" Text="Password" ForeColor="White" Font-Size="14pt"></asp:Label>
             </td>
             <td>
                  <asp:TextBox ID="txtPass" runat="server" Width="295px"></asp:TextBox>
             </td>
         </tr>

         <tr>
             <td>
                  <asp:Label ID="label3" runat="server" Text="Name" ForeColor="White" Font-Size="14pt"></asp:Label>
             </td>
             <td>
                  <asp:TextBox ID="txtFName" runat="server" Width="294px"></asp:TextBox>
             </td>
         </tr>

         <tr>
             <td>
                  <asp:Label ID="label4" runat="server" Text="Lastname" ForeColor="White" Font-Size="14pt"></asp:Label>
             </td>
             <td>
                 <asp:TextBox ID="txtLName" runat="server" Width="294px"></asp:TextBox>
             </td>
         </tr>

          <tr>
     <td>
          <asp:Label ID="label5" runat="server" Text="Adderss" ForeColor="White" Font-Size="14pt"></asp:Label>
     </td>
     <td>
         <asp:TextBox ID="txtAdd" runat="server" Width="294px"></asp:TextBox>
     </td>
 </tr>


       <tr>
          <td>
            <asp:Label ID="label6" runat="server" Text="Email" ForeColor="White" Font-Size="14pt"></asp:Label>
           </td>
            <td>
                <asp:TextBox ID="txtEmail" runat="server" Width="293px"></asp:TextBox>
           </td>
        </tr>

         <tr>
           <td>
              <asp:Label ID="label7" runat="server" Text="Phone" ForeColor="White" Font-Size="14pt"></asp:Label>
           </td>
           <td>
              <asp:TextBox ID="txtPhone" runat="server" Width="294px"></asp:TextBox>
          </td>
     </tr>

     <tr>
       <td class="auto-style2">
          <asp:Label ID="txtGender" runat="server" Text="Gender" ForeColor="White" Font-Size="14pt"></asp:Label>
      </td>
       <td class="auto-style2">
            <asp:DropDownList ID="dpdphone" runat="server" Width="366px">
       <asp:ListItem>Female</asp:ListItem>
       <asp:ListItem>Male</asp:ListItem>
   </asp:DropDownList>
      </td>
    </tr>

    <%-- <tr>
       <td>
         <asp:Label ID="txtBirth" runat="server" Text="Birthday" ForeColor="White" Font-Size="14pt"></asp:Label>
      </td>
       <td>
         <asp:Calendar ID="Calendar1" runat="server" Height="19px" Width="92px"></asp:Calendar>
       </td>
     </tr>--%>
         <tr>

    <td class="auto-style1">
      <asp:Label ID="Label12" runat="server" Text="Birthday" ForeColor="white"></asp:Label>
   </td>

   <td class="auto-style1">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Calendar ID="Calendar1" runat="server" 
                    SelectionMode="Day" 
                    ShowDayHeader="True" 
                    ShowGridLines="True"
                    ShowTitle="True"
                    DayNameFormat="Short"
                    FirstDayOfWeek="Sunday"
                    NextPrevFormat="ShortMonth"
                    BackColor="White"
                    ForeColor="Black"
                    BorderColor="Black"
                    BorderStyle="Solid"
                    BorderWidth="1px"
                    CellPadding="1"
                    Font-Names="Verdana"
                    Font-Size="9pt"
                    Height="200px"
                    Width="220px">
                    <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <SelectorStyle BackColor="#CCCCCC" />
                    <NextPrevStyle Font-Size="8pt" ForeColor="White" Font-Bold="True" />
                    <DayHeaderStyle Font-Size="8pt" Font-Bold="True" Height="8pt" />
                    <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                    <TitleStyle BackColor="#333399" Font-Bold="True" Font-Size="9pt" ForeColor="White" Height="9pt" />
                    <WeekendDayStyle BackColor="#FFFFCC" />
                </asp:Calendar>
            </ContentTemplate>
        </asp:UpdatePanel>
     <%--  <asp:TextBox ID="txtbirthday" runat="server"></asp:TextBox>--%>
   </td>

 </tr>
         <tr>
            <td>
                 <asp:Image ID="img" runat="server" Height="197px" ImageUrl="~/MyPics/Profile.jpg" Width="166px" />
                 
            </td>
             <td>
                 &nbsp;</td>
         </tr>

         <tr>
             <td>

                 <asp:FileUpload ID="FileUpload1" runat="server" Width="386px" />

             </td>
         </tr>

         <tr>
             <td>
                 <asp:Button runat="server" id="btnsave"  Text="Sign Up" OnClick="btnsave_Click" style="padding: 12px; border-radius: 20px; background: red; color: white; font-weight: 700; margin-top: 20px" />             </td>
         </tr>

     </table>

 </center>

</asp:Content>

