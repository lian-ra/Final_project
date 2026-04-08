<%@ Page Title="Event Store" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="EventStore.aspx.cs" Inherits="EventStore" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .store-wrapper {
            padding: 50px 20px;
            max-width: 1200px;
            margin: 0 auto;
            min-height: 80vh;
        }

        .store-header {
            text-align: center;
            margin-bottom: 50px;
        }

        .store-title {
            color: white;
            font-size: 42px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 2px;
            text-shadow: 0 4px 15px rgba(255, 76, 59, 0.4);
            margin-bottom: 10px;
        }

        .store-subtitle {
            color: #ccc;
            font-size: 18px;
        }

        .msg-box {
            text-align: center;
            margin-bottom: 20px;
            font-size: 16px;
            font-weight: bold;
        }

        .products-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
            gap: 30px;
            margin-bottom: 50px;
        }

        .product-card {
            background: rgba(30, 30, 30, 0.8);
            border: 1px solid rgba(255, 255, 255, 0.1);
            border-radius: 15px;
            padding: 20px;
            text-align: center;
            transition: transform 0.3s, box-shadow 0.3s, border-color 0.3s;
            backdrop-filter: blur(10px);
            position: relative;
            display: flex;
            flex-direction: column;
            justify-content: space-between;
        }

        .product-card:hover {
            transform: translateY(-10px);
            box-shadow: 0 15px 30px rgba(0, 0, 0, 0.6);
            border-color: #ff4c3b;
            background: rgba(40, 40, 40, 0.9);
        }

        .product-image {
            width: 100%;
            height: 200px;
            object-fit: contain;
            border-radius: 10px;
            margin-bottom: 15px;
            background: #111;
            padding: 10px;
        }

        .product-name {
            color: white;
            font-size: 20px;
            font-weight: 700;
            margin-bottom: 5px;
        }

        .product-desc {
            color: #aaa;
            font-size: 14px;
            margin-bottom: 15px;
            height: 40px;
            overflow: hidden;
        }
        
        .product-code {
            color: #ff4c3b;
            font-size: 12px;
            font-weight: bold;
            margin-bottom: 15px;
            letter-spacing: 1px;
        }

        .product-bottom {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-top: auto;
            border-top: 1px solid rgba(255, 255, 255, 0.1);
            padding-top: 15px;
        }

        .product-price {
            color: #fff;
            font-size: 22px;
            font-weight: bold;
        }
        
        .currency {
            color: #ff4c3b;
        }

        .qty-control {
            display: flex;
            align-items: center;
            background: #222;
            border-radius: 20px;
            padding: 5px;
            border: 1px solid #444;
        }

        .qty-btn {
            background: transparent;
            border: none;
            color: white;
            font-size: 18px;
            width: 30px;
            height: 30px;
            cursor: pointer;
            transition: color 0.3s;
        }

        .qty-btn:hover {
            color: #ff4c3b;
        }

        .qty-input {
            width: 40px;
            background: transparent;
            border: none;
            color: white;
            text-align: center;
            font-size: 16px;
            font-weight: bold;
        }

        .qty-input:focus {
            outline: none;
        }

        .checkout-bar {
            background: rgba(20, 20, 20, 0.95);
            border: 2px solid #ff4c3b;
            border-radius: 15px;
            padding: 20px 30px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            position: sticky;
            bottom: 20px;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.8);
            z-index: 100;
        }

        .total-text {
            color: white;
            font-size: 24px;
            font-weight: bold;
        }

        .total-amount {
            color: #ff4c3b;
            font-size: 28px;
            margin-left: 10px;
        }

        .btn-checkout {
            background: linear-gradient(135deg, #ff4c3b 0%, #d82b1f 100%);
            color: white;
            border: none;
            padding: 15px 40px;
            border-radius: 30px;
            font-size: 18px;
            font-weight: bold;
            cursor: pointer;
            text-transform: uppercase;
            letter-spacing: 1px;
            transition: all 0.3s ease;
            box-shadow: 0 5px 15px rgba(255, 76, 59, 0.4);
        }

        .btn-checkout:hover {
            transform: translateY(-2px);
            box-shadow: 0 8px 25px rgba(255, 76, 59, 0.6);
        }
    </style>
    <script type="text/javascript">
        function updateQty(btn, change, priceString) {
            var container = btn.parentElement;
            var input = container.querySelector('.qty-input');
            var currentQty = parseInt(input.value) || 0;
            var newQty = currentQty + change;
            if (newQty < 0) newQty = 0;
            input.value = newQty;
            
            recalculateTotal();
            return false; // prevent postback
        }

        function recalculateTotal() {
            var total = 0;
            // Iterate over all cards to sum price * qty
            var cards = document.querySelectorAll('.product-card');
            cards.forEach(function(card) {
                var priceEl = card.querySelector('input[id$="hfPrice"]');
                var qtyEl = card.querySelector('.qty-input');
                if(priceEl && qtyEl) {
                    var price = parseFloat(priceEl.value) || 0;
                    var qty = parseInt(qtyEl.value) || 0;
                    total += (price * qty);
                }
            });
            document.getElementById('lblTotalAmount').innerText = total.toFixed(2) + ' ILS';
        }

        // Run once on load
        window.onload = function() {
            recalculateTotal();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="store-wrapper">
        <div class="store-header">
            <h1 class="store-title">Event Store</h1>
            <div class="store-subtitle">Buy snacks and merch for <span style="color:white; font-weight:bold;"><asp:Label ID="lblEventName" runat="server"></asp:Label></span></div>
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="msg-box"></asp:Label>

        <asp:Repeater ID="rptProducts" runat="server">
            <HeaderTemplate>
                <div class="products-grid">
            </HeaderTemplate>
            <ItemTemplate>
                <div class="product-card">
                    <img src="<%# ResolveUrl(Eval("Picture").ToString().StartsWith("~/") ? Eval("Picture").ToString() : "~/" + Eval("Picture").ToString()) %>" class="product-image" onerror="this.src='/images/default-product.png';" />
                    <div class="product-name"><%# Eval("Name") %></div>
                    <div class="product-code">Item ID: <%# Eval("ProductId") %></div>
                    <div class="product-desc"><%# Eval("Description") %></div>
                    
                    <asp:HiddenField ID="hfProductId" runat="server" Value='<%# Eval("ProductId") %>' />
                    <asp:HiddenField ID="hfPrice" runat="server" Value='<%# Eval("Price") %>' />

                    <div class="product-bottom">
                        <div class="product-price">
                            <%# Convert.ToDecimal(Eval("Price")).ToString("0.00") %> <span class="currency">ILS</span>
                        </div>
                        <div class="qty-control">
                            <button type="button" class="qty-btn" onclick="updateQty(this, -1)">-</button>
                            <asp:TextBox ID="txtQty" runat="server" CssClass="qty-input" Text="0" onkeyup="recalculateTotal()"></asp:TextBox>
                            <button type="button" class="qty-btn" onclick="updateQty(this, 1)">+</button>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
            <FooterTemplate>
                </div>
            </FooterTemplate>
        </asp:Repeater>

        <div class="checkout-bar">
            <div class="total-text">
                Total: <span id="lblTotalAmount" class="total-amount">0.00 ILS</span>
            </div>
            <asp:Button ID="btnCheckout" runat="server" Text="Checkout & Place Order" CssClass="btn-checkout" OnClick="btnCheckout_Click" />
        </div>
    </div>
</asp:Content>
