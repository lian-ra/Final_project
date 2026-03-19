<%@ Page Title="Manage Platform Stock" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="ManageStock.aspx.cs" Inherits="ManageStock" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .manage-wrapper {
            padding: 40px 20px;
            max-width: 1200px;
            margin: 0 auto;
            color: white;
        }

        .manage-header { text-align: center; margin-bottom: 40px; }
        .manage-title { font-size: 36px; font-weight: 700; color: #ff4c3b; text-transform: uppercase; letter-spacing: 1px; }
        .manage-subtitle { font-size: 18px; color: #aaa; }

        .split-layout { display: flex; gap: 30px; flex-wrap: wrap; }
        .panel-half {
            flex: 1;
            min-width: 300px;
            background: rgba(30, 30, 30, 0.8);
            border-radius: 15px;
            padding: 25px;
            border: 1px solid rgba(255, 255, 255, 0.1);
        }

        .panel-title { font-size: 24px; font-weight: 600; margin-bottom: 20px; border-bottom: 2px solid #ff4c3b; padding-bottom: 10px; }

        .form-group { margin-bottom: 20px; }
        .form-group label { display: block; margin-bottom: 8px; color: #ccc; font-size: 14px; }
        .form-control { width: 100%; padding: 12px; background: rgba(255,255,255,0.05); border: 1px solid rgba(255,255,255,0.1); border-radius: 8px; color: white; box-sizing: border-box; }
        
        .btn-action { background: #ff4c3b; color: white; padding: 12px 25px; border-radius: 8px; border: none; font-weight: bold; cursor: pointer; text-transform: uppercase; width: 100%; transition: background 0.3s; }
        .btn-action:hover { background: #d82b1f; }

        .stock-list { display: grid; grid-template-columns: repeat(auto-fill, minmax(200px, 1fr)); gap: 15px; }
        .stock-item { background: rgba(255, 255, 255, 0.05); border-radius: 10px; padding: 15px; border: 1px solid rgba(255, 255, 255, 0.05); text-align: center; }
        
        .item-img { width: 100px; height: 100px; object-fit: contain; margin-bottom: 10px; }
        .item-title { font-weight: bold; margin-bottom: 5px; color: white; }
        .item-price { color: #4ecdc4; font-weight: bold; }
        
        .btn-delete { display: inline-block; background: rgba(255, 76, 59, 0.1); color: #ff4c3b; padding: 5px 10px; border-radius: 5px; text-decoration: none; margin-top: 10px; font-size: 12px; border: 1px solid #ff4c3b; transition: all 0.3s; }
        .btn-delete:hover { background: #ff4c3b; color: white; }
        .btn-edit { display: inline-block; background: rgba(52, 152, 219, 0.1); color: #3498db; padding: 5px 10px; border-radius: 5px; text-decoration: none; margin-top: 10px; margin-right: 5px; font-size: 12px; border: 1px solid #3498db; transition: all 0.3s; }
        .btn-edit:hover { background: #3498db; color: white; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="manage-wrapper">
        <a href="AdminArea.aspx" class="btn-back"><i class="fa fa-arrow-left"></i> Back to Admin Dashboard</a>
        
        <div class="manage-header">
            <h1 class="manage-title">Manage Platform Stock</h1>
            <div class="manage-subtitle">Add new merchandise or concessions to the global store</div>
        </div>

        <asp:Label ID="lblSuccess" runat="server" CssClass="success-msg" Visible="false"></asp:Label>
        <asp:Label ID="lblError" runat="server" Visible="false" style="color: #ff4c3b; background: rgba(255, 76, 59, 0.1); padding: 15px; border-radius: 5px; margin-bottom: 20px; text-align: center; font-weight: bold; display: block;"></asp:Label>

        <div class="split-layout">
            <div class="panel-half" style="flex: 0 0 400px;">
                <asp:Panel ID="pnlAdd" runat="server">
                    <h3 class="panel-title">Add New Product</h3>
                    <div class="form-group">
                        <label>Product Name</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" required="true"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Price (ILS)</label>
                        <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" TextMode="Number" step="0.01" required="true"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Description</label>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Picture Upload</label>
                        <asp:FileUpload ID="fuPicture" runat="server" CssClass="form-control" />
                    </div>
                    <asp:Button ID="btnAddProduct" runat="server" Text="Add Product to Stock" CssClass="btn-action" OnClick="btnAddProduct_Click" />
                </asp:Panel>

                <asp:Panel ID="pnlEdit" runat="server" Visible="false">
                    <h3 class="panel-title" style="border-bottom-color: #3498db; color: #3498db;">Edit Product</h3>
                    <asp:HiddenField ID="hfEditProductCode" runat="server" />
                    <div class="form-group">
                        <label>Product Name</label>
                        <asp:TextBox ID="txtEditName" runat="server" CssClass="form-control" ReadOnly="true" style="background: rgba(0,0,0,0.3); color:#888;"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Price (ILS)</label>
                        <asp:TextBox ID="txtEditPrice" runat="server" CssClass="form-control" TextMode="Number" step="0.01" required="true"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>New Picture Upload (Optional)</label>
                        <asp:FileUpload ID="fuEditPicture" runat="server" CssClass="form-control" />
                    </div>
                    <asp:Button ID="btnSaveEdit" runat="server" Text="Save Changes" CssClass="btn-action" style="background:#3498db;" OnClick="btnSaveEdit_Click" />
                    <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel Edit" CssClass="btn-action" style="background:#555; margin-top:10px;" OnClick="btnCancelEdit_Click" formnovalidate="formnovalidate" />
                </asp:Panel>
            </div>

            <div class="panel-half">
                <h3 class="panel-title">Current Global Stock</h3>
                
                <div style="display: flex; gap: 10px; margin-bottom: 20px;">
                    <asp:TextBox ID="txtSearchStock" runat="server" CssClass="form-control" placeholder="Search product name or code..." style="margin-bottom:0;"></asp:TextBox>
                    <asp:Button ID="btnSearchStock" runat="server" Text="Search" CssClass="btn-action" style="margin-top:0; padding:10px 20px; width:auto; border-radius:5px;" OnClick="btnSearchStock_Click" formnovalidate="formnovalidate" />
                    <asp:Button ID="btnClearSearch" runat="server" Text="Clear" CssClass="btn-action" style="margin-top:0; padding:10px 20px; width:auto; border-radius:5px; background: #555;" OnClick="btnClearSearch_Click" formnovalidate="formnovalidate" />
                </div>

                <div class="stock-list">
                    <asp:Repeater ID="rptStock" runat="server" OnItemCommand="rptStock_ItemCommand">
                        <ItemTemplate>
                            <div class="stock-item">
                                <img src='<%# ResolveUrl(Eval("Picture").ToString().StartsWith("~/") ? Eval("Picture").ToString() : "~/" + Eval("Picture").ToString()) %>' class="item-img" alt="Product" />
                                <div class="item-title"><%# Eval("Name") %></div>
                                <div class="item-price"><%# Eval("Price") %> ILS</div>
                                <div style="margin-top: 10px;">
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditProd" CommandArgument='<%# Eval("ProductCode") + "|" + Eval("Name") + "|" + Eval("Price") %>' CssClass="btn-edit" formnovalidate="formnovalidate"><i class="fa fa-pencil"></i> Edit</asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ProductCode") %>' CssClass="btn-delete" OnClientClick="return confirm('Are you sure you want to delete this item? It will be removed from the store, but permanently preserved in any past order receipts.');" formnovalidate="formnovalidate"><i class="fa fa-trash"></i> Delete</asp:LinkButton>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
