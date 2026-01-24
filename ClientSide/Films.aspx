 <%@ Page Title="Films" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="Films.aspx.cs" Inherits="Films" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .shop-wrapper {
            max-width: 1200px;
            margin: 0 auto;
            padding: 40px 20px;
            font-family: 'Segoe UI', sans-serif;
        }

        .cards-container {
            display: flex;
            flex-wrap: wrap;
            justify-content: center;
            gap: 25px;
        }

            .cards-container table {
                width: 260px;
                background: #ffffff;
                border-radius: 18px;
                padding: 0;
                box-shadow: 0 6px 20px rgba(0,0,0,0.1);
                transition: transform 0.25s, box-shadow 0.25s;
                overflow: hidden;
                text-align: center;
                position: relative;
            }

                .cards-container table:hover {
                    transform: translateY(-6px);
                    box-shadow: 0 12px 28px rgba(0,0,0,0.18);
                }

            .cards-container .image-container {
                position: relative;
                overflow: hidden;
                border-radius: 18px 18px 0 0;
            }

            .cards-container img {
                width: 100%;
                height: 280px;
                object-fit: cover;
                transition: transform 0.3s ease;
            }

            .cards-container table:hover img {
                transform: scale(1.05);
            }

            .cards-container td.info {
                padding: 10px;
            }

            .cards-container .name {
                font-size: 18px;
                font-weight: 700;
                color: #333;
                margin-bottom: 6px;
            }

            .cards-container .price {
                font-size: 16px;
                font-weight: 600;
                color: #ff4c3b;
            }

        .cart-btn {
            font-size: 22px;
            padding: 12px 0;
            width: 100%;
            color: white;
            background: #d3d3d3;
            border: none;
            border-radius: 0 0 18px 18px;
            cursor: pointer;
            transition: background 0.2s, transform 0.15s;
        }

            .cart-btn:hover {
                background: #6ecfff;
            }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="shop-wrapper">

    <h1 style="text-align:center">Shop</h1>

    <asp:DataList ID="dtlMovies" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" CssClass="cards-container">
        <ItemTemplate>
            <table>
                <tr>
                    <td>
                        <div class="image-container">
                            <asp:Image ID="Image1"
                                ImageUrl='<%# DataBinder.Eval(Container.DataItem,"pic","~/MyPics/{0}") %>'
                                runat="server" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center" class="info">
                        <div class="name">
                            <asp:Label ID="lblName" runat="server"
                                Text='<%# DataBinder.Eval(Container.DataItem,"name") %>'></asp:Label>
                        </div>
                        <div class="price">
                            <asp:Label ID="lblPrice" runat="server"
                                Text='<%# DataBinder.Eval(Container.DataItem,"price") %>'></asp:Label>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:LinkButton ID="btnAddCart" runat="server" CssClass="cart-btn" CommandName="AddToCart">
                            <i class="fa fa-cart-plus"></i>
                        </asp:LinkButton>
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:DataList>
</div>
</asp:Content>
