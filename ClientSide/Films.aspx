<%@ Page Title="Films" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="Films.aspx.cs" Inherits="Films" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .films-container {
            padding: 40px 20px;
            color: white;
        }
        .films-header {
            text-align: center;
            margin-bottom: 40px;
        }
        .films-header h1 {
            font-size: 48px;
            margin-bottom: 10px;
            color: #ff6b6b;
        }
        .films-header p {
            font-size: 18px;
            opacity: 0.8;
        }
        .films-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
            gap: 30px;
            margin-top: 40px;
        }
        .film-card {
            background: rgba(255, 255, 255, 0.1);
            border-radius: 10px;
            padding: 15px;
            text-align: center;
            transition: transform 0.3s, box-shadow 0.3s;
            cursor: pointer;
        }
        .film-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.5);
            background: rgba(255, 255, 255, 0.15);
        }
        .film-poster {
            width: 100%;
            height: 300px;
            object-fit: cover;
            border-radius: 8px;
            margin-bottom: 15px;
        }
        .film-title {
            font-size: 18px;
            font-weight: bold;
            margin-bottom: 8px;
            color: white;
        }
        .film-rating {
            color: #ffd700;
            font-size: 16px;
            margin-bottom: 5px;
        }
        .film-year {
            color: #ccc;
            font-size: 14px;
        }
        .search-section {
            text-align: center;
            margin-bottom: 40px;
        }
        .search-box {
            padding: 15px 30px;
            font-size: 16px;
            width: 400px;
            max-width: 90%;
            border-radius: 25px;
            border: 2px solid #333399;
            background: rgba(255, 255, 255, 0.1);
            color: white;
        }
        .search-box::placeholder {
            color: rgba(255, 255, 255, 0.6);
        }
        .genre-filters {
            display: flex;
            justify-content: center;
            flex-wrap: wrap;
            gap: 10px;
            margin-top: 20px;
        }
        .genre-btn {
            padding: 8px 20px;
            background: rgba(255, 255, 255, 0.1);
            border: 1px solid rgba(255, 255, 255, 0.3);
            border-radius: 20px;
            color: white;
            cursor: pointer;
            transition: all 0.3s;
        }
        .genre-btn:hover {
            background: rgba(255, 255, 255, 0.2);
            border-color: #ff6b6b;
        }
        .genre-btn.active {
            background: #ff6b6b;
            border-color: #ff6b6b;
        }

        /* Card layout styles for the DataList */
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

        /* Add to Cart icon button with hover + bounce animation */
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
                animation: hoverBounce 0.3s;
            }

            .cart-btn:active {
                animation: bounce 0.3s;
            }

        /* Bounce animation keyframes on click */
        @keyframes bounce {
            0% {
                transform: scale(1);
            }

            30% {
                transform: scale(1.2) translateY(-5px);
            }

            50% {
                transform: scale(0.95) translateY(0);
            }

            70% {
                transform: scale(1.05) translateY(-3px);
            }

            100% {
                transform: scale(1) translateY(0);
            }
        }

        /* Small bounce on hover */
        @keyframes hoverBounce {
            0% {
                transform: scale(1);
            }

            50% {
                transform: scale(1.05) translateY(-2px);
            }

            100% {
                transform: scale(1);
            }
        }

        @media screen and (max-width: 900px) {
            .cards-container table {
                width: 45%;
            }
        }

        @media screen and (max-width: 600px) {
            .cards-container table {
                width: 90%;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    <asp:Label ID="Label1" runat="server" Text="Username"></asp:Label>
    <asp:Label ID="LblUser" runat="server" Text="Label"></asp:Label>

    <div class="shop-wrapper">

    <h1 style="text-align:center">Shop</h1>

    <asp:DataList ID="dtlMovies" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" CssClass="cards-container" OnEditCommand="dtlMovies_EditCommand">
        <itemtemplate>
            <table>

                <tr>
                    <td>
                        <div class="image-container">
                            <asp:Image ID="Image1"
                                ImageUrl='<%# DataBinder.Eval(Container.DataItem, "pic", "MyPics/{0}") %>'
                                runat="server" />
                        </div>
                    </td>
                </tr>

                <tr>
                    <td style="text-align: center" class="info">
                        <div class="name">
                            <asp:Label ID="lblName" runat="server"
                                Text='<%# DataBinder.Eval(Container.DataItem, "name") %>'></asp:Label>
                        </div>
                        <div class="price">
                            <asp:Label ID="lblPrice" runat="server"
                                Text='<%# DataBinder.Eval(Container.DataItem, "price") %>'></asp:Label>
                        </div>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:LinkButton ID="btnAddCart" runat="server" Text="AddToWishlist" CssClass="cart-btn" CommandName="AddToCart">

                            <i class="fa fa-cart-plus"></i>
                        </asp:LinkButton>
                    </td>
                </tr>
                <center>
                    <asp:Button ID="BtnChoose" CommandName="Select" runat="server" Text="ViewDetails" />
                    <asp:Button ID="BtnWish" CommandName="Edit" runat="server" Text="ViewDetails" />
                </center>
            </table>
        </itemtemplate>
    </asp:DataList>
    </div>
</asp:Content>


