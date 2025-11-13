<%@ Page Title="" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="SearchUsers.aspx.cs" Inherits="SearchUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <center>
        <h1 style="color: #FFFFFF"> Search Users </h1>
        <br />

        <table>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Search:" ForeColor="White"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TxtSearch" runat="server" Width="200px"></asp:TextBox>
                </td>
                <td>
                    <asp:DropDownList ID="DrpSearch" runat="server" Width="150px">
                        <asp:ListItem Value="name">By Name</asp:ListItem>
                        <asp:ListItem Value="address">By Address</asp:ListItem>
                        <asp:ListItem Value="username">By Username</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>   
                    <asp:Button ID="BtnSearch" runat="server" Text="Search" OnClick="BtnSearch_Click" />
                </td>
                <td>
                    <asp:Button ID="BtnReset" runat="server" Text="Show All" OnClick="BtnReset_Click" />
                </td>
            </tr>
        </table>

        <br /> <br />
        <asp:GridView ID="GrdUsers" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" Height="262px" OnSelectedIndexChanged="GrdUsers_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="User" HeaderText="Username" />
                <asp:BoundField DataField="FName" HeaderText="First Name" />
                <asp:BoundField DataField="LName" HeaderText="Last Name" />
                <asp:BoundField DataField="address" HeaderText="Address" />
                <asp:BoundField DataField="email" HeaderText="Email" />
                <asp:TemplateField HeaderText="Phone">
                    <ItemTemplate>
                        <asp:Label ID="lblPhone" runat="server" Text='<%# GetPhoneValue(Container.DataItem) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Gender">
                    <ItemTemplate>
                        <asp:Label ID="lblGender" runat="server" Text='<%# GetGenderValue(Container.DataItem) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Birthday">
                    <ItemTemplate>
                        <asp:Label ID="lblBirthday" runat="server" Text='<%# GetBirthdayValue(Container.DataItem) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Picture">
                    <ItemTemplate>
                        <asp:Image ID="imgUser" runat="server" ImageUrl='<%# "~/MyPics/" + Eval("pic") %>' Height="50px" Width="50px" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
            <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
            <RowStyle BackColor="White" ForeColor="#003399" />
            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
            <SortedAscendingCellStyle BackColor="#EDF6F6" />
            <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
            <SortedDescendingCellStyle BackColor="#D6DFDF" />
            <SortedDescendingHeaderStyle BackColor="#002876" />
        </asp:GridView>

    </center>
   

</asp:Content>

