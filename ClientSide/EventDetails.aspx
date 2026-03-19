<%@ Page Title="Event Details" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true"
    CodeFile="EventDetails.aspx.cs" Inherits="EventDetails" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
        <style type="text/css">
            .event-details-wrapper {
                padding: 40px 20px;
                max-width: 1200px;
                margin: 0 auto;
                color: white;
            }

            .event-header {
                background: linear-gradient(135deg, rgba(30, 30, 30, 0.95) 0%, rgba(50, 50, 50, 0.95) 100%);
                border-radius: 15px;
                padding: 30px;
                margin-bottom: 30px;
                border: 1px solid rgba(255, 255, 255, 0.1);
            }

            .event-layout {
                display: flex;
                gap: 30px;
                flex-wrap: wrap;
            }

            .event-poster-section {
                flex: 0 0 300px;
            }

            .event-poster-section img {
                width: 100%;
                border-radius: 10px;
                box-shadow: 0 10px 30px rgba(0, 0, 0, 0.5);
            }

            .event-info-section {
                flex: 1;
                min-width: 300px;
            }

            .event-movie-title {
                font-size: 32px;
                font-weight: 700;
                margin-bottom: 10px;
            }

            .event-host-info {
                font-size: 16px;
                color: #aaa;
                margin-bottom: 20px;
            }

            .event-host-info a {
                color: #ff4c3b;
                text-decoration: none;
                font-weight: 600;
            }

            .event-status-badge {
                display: inline-block;
                padding: 8px 20px;
                border-radius: 20px;
                font-size: 14px;
                font-weight: 600;
                text-transform: uppercase;
                margin-bottom: 20px;
            }

            .status-open {
                background: #4ecdc4;
                color: white;
            }

            .status-closed {
                background: #95a5a6;
                color: white;
            }

            .status-canceled {
                background: #e74c3c;
                color: white;
            }

            .event-details-grid {
                display: grid;
                grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
                gap: 20px;
                margin-bottom: 30px;
            }

            .event-detail-card {
                background: rgba(255, 255, 255, 0.05);
                padding: 15px;
                border-radius: 10px;
                border: 1px solid rgba(255, 255, 255, 0.1);
            }

            .event-detail-label {
                font-size: 12px;
                color: #aaa;
                text-transform: uppercase;
                margin-bottom: 5px;
            }

            .event-detail-value {
                font-size: 18px;
                font-weight: 600;
                color: white;
            }

            .event-detail-value i {
                color: #ff4c3b;
                margin-right: 8px;
            }

            .event-price-highlight {
                font-size: 28px;
                color: #4ecdc4;
                font-weight: 700;
            }

            .event-actions {
                display: flex;
                gap: 15px;
                margin-top: 20px;
                flex-wrap: wrap;
            }

            .btn-action {
                padding: 12px 30px;
                border-radius: 25px;
                border: none;
                font-size: 15px;
                font-weight: 600;
                cursor: pointer;
                transition: all 0.3s;
                text-decoration: none;
                display: inline-block;
            }

            .btn-subscribe {
                background: linear-gradient(135deg, #4ecdc4 0%, #44a3a3 100%);
                color: white;
            }

            .btn-subscribe:hover {
                transform: translateY(-2px);
                box-shadow: 0 6px 20px rgba(78, 205, 196, 0.4);
            }

            .btn-unsubscribe {
                background: rgba(255, 255, 255, 0.1);
                color: white;
                border: 1px solid rgba(255, 255, 255, 0.2);
            }

            .btn-unsubscribe:hover {
                background: rgba(255, 76, 59, 0.2);
                border-color: #ff4c3b;
            }

            .btn-back {
                background: rgba(255, 255, 255, 0.1);
                color: white;
                border: 1px solid rgba(255, 255, 255, 0.2);
            }

            .btn-back:hover {
                background: rgba(255, 255, 255, 0.2);
            }

            .section-title {
                font-size: 24px;
                font-weight: 600;
                margin-bottom: 20px;
                border-left: 4px solid #ff4c3b;
                padding-left: 15px;
            }

            .subscribers-grid {
                display: grid;
                grid-template-columns: repeat(auto-fill, minmax(150px, 1fr));
                gap: 20px;
                margin-bottom: 30px;
            }

            .subscriber-card {
                background: rgba(255, 255, 255, 0.05);
                padding: 15px;
                border-radius: 10px;
                text-align: center;
                border: 1px solid rgba(255, 255, 255, 0.1);
                transition: all 0.3s;
            }

            .subscriber-card:hover {
                background: rgba(255, 255, 255, 0.1);
                transform: translateY(-3px);
            }

            .subscriber-avatar {
                width: 60px;
                height: 60px;
                border-radius: 50%;
                object-fit: cover;
                border: 2px solid #ff4c3b;
                margin-bottom: 10px;
            }

            .subscriber-name {
                font-size: 14px;
                color: white;
                font-weight: 600;
            }

            .subscriber-name a {
                color: white;
                text-decoration: none;
            }

            .subscriber-name a:hover {
                color: #ff4c3b;
            }

            .empty-subscribers {
                text-align: center;
                padding: 40px;
                color: #888;
                font-style: italic;
            }

            .owner-controls {
                background: rgba(255, 76, 59, 0.1);
                padding: 20px;
                border-radius: 10px;
                border: 1px solid rgba(255, 76, 59, 0.3);
                margin-bottom: 30px;
            }

            .owner-controls-title {
                font-size: 18px;
                font-weight: 600;
                margin-bottom: 15px;
                color: #ff4c3b;
            }

            .status-buttons {
                display: flex;
                gap: 10px;
                flex-wrap: wrap;
            }

            .btn-status {
                padding: 8px 20px;
                border-radius: 20px;
                border: none;
                cursor: pointer;
                font-size: 13px;
                font-weight: 600;
                transition: all 0.3s;
            }

            .btn-status-open {
                background: #4ecdc4;
                color: white;
            }

            .btn-status-closed {
                background: #95a5a6;
                color: white;
            }

            .btn-status-canceled {
                background: #e74c3c;
                color: white;
            }

            .btn-status:hover {
                transform: translateY(-2px);
                box-shadow: 0 4px 10px rgba(0, 0, 0, 0.3);
            }

            /* Modal Styles */
            .modal-overlay {
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background: rgba(0, 0, 0, 0.8);
                z-index: 1000;
                display: flex;
                justify-content: center;
                align-items: center;
            }

            .modal-content {
                background: #1a1a1a;
                width: 90%;
                max-width: 500px;
                border-radius: 15px;
                padding: 30px;
                border: 1px solid rgba(255, 255, 255, 0.1);
                box-shadow: 0 10px 40px rgba(0, 0, 0, 0.5);
                color: white;
            }

            .modal-header {
                display: flex;
                justify-content: space-between;
                align-items: center;
                margin-bottom: 25px;
                border-bottom: 1px solid rgba(255, 255, 255, 0.1);
                padding-bottom: 15px;
            }

            .modal-header h2 {
                margin: 0;
                font-size: 24px;
                color: white;
            }

            .close-btn {
                font-size: 30px;
                color: #aaa;
                text-decoration: none;
                cursor: pointer;
                background: none;
                border: none;
            }

            .close-btn:hover {
                color: white;
            }

            .form-group {
                margin-bottom: 20px;
            }

            .form-group label {
                display: block;
                margin-bottom: 8px;
                color: #ccc;
                font-size: 14px;
            }

            .form-control {
                width: 100%;
                padding: 12px;
                background: rgba(255, 255, 255, 0.05);
                border: 1px solid rgba(255, 255, 255, 0.1);
                border-radius: 8px;
                color: white;
                font-size: 16px;
                box-sizing: border-box;
                /* Important for padding */
            }

            .form-control:focus {
                outline: none;
                border-color: #ff4c3b;
                background: rgba(255, 255, 255, 0.1);
            }

            .modal-footer {
                margin-top: 30px;
                display: flex;
                justify-content: flex-end;
            }
        </style>
    </asp:Content>

    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        <div class="event-details-wrapper">
            <asp:PlaceHolder ID="phEventDetails" runat="server"></asp:PlaceHolder>

            <asp:Panel ID="pnlOwnerControls" runat="server" CssClass="owner-controls" Visible="false">
                <div class="owner-controls-title">Event Management</div>
                <div class="status-buttons">
                    <asp:Button ID="btnSetOpen" runat="server" Text="Set Open" CssClass="btn-status btn-status-open"
                        OnClick="btnSetOpen_Click" />
                    <asp:Button ID="btnSetClosed" runat="server" Text="Set Closed"
                        CssClass="btn-status btn-status-closed" OnClick="btnSetClosed_Click" />
                    <asp:Button ID="btnSetCanceled" runat="server" Text="Cancel Event"
                        CssClass="btn-status btn-status-canceled" OnClick="btnSetCanceled_Click" />
                    <asp:Button ID="btnUpdateEvent" runat="server" Text="Update Details"
                        CssClass="btn-status btn-status-open" style="background: #e67e22;"
                        OnClick="btnUpdateEvent_Click" />
                    <a href='ManageEventStore.aspx?eventId=<%= Request.QueryString["eventId"] %>' class="btn-status" style="background: #9b59b6; color: white; text-decoration: none; display: inline-flex; align-items: center;"><i class="fa fa-shopping-basket" style="margin-right:5px;"></i>Manage Store</a>
                    <a href='EventOrders.aspx?eventId=<%= Request.QueryString["eventId"] %>' class="btn-status" style="background: #34495e; color: white; text-decoration: none; display: inline-flex; align-items: center;"><i class="fa fa-list-alt" style="margin-right:5px;"></i>View Orders</a>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlUpdateModal" runat="server" Visible="false" CssClass="modal-overlay">
                <div class="modal-content">
                    <div class="modal-header">
                        <h2>Update Event</h2>
                        <asp:LinkButton ID="btnCloseModal" runat="server" OnClick="btnCloseModal_Click"
                            CssClass="close-btn">&times;</asp:LinkButton>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label>Event Date</label>
                            <asp:TextBox ID="txtUpdateDate" runat="server" TextMode="Date" CssClass="form-control">
                            </asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Start Time</label>
                            <asp:TextBox ID="txtUpdateTime" runat="server" TextMode="Time" CssClass="form-control">
                            </asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Price (ILS)</label>
                            <asp:TextBox ID="txtUpdatePrice" runat="server" TextMode="Number" step="0.01"
                                CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Location</label>
                            <asp:TextBox ID="txtUpdateLocation" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSaveUpdate" runat="server" Text="Save Changes"
                            CssClass="btn-action btn-subscribe" OnClick="btnSaveUpdate_Click" />
                    </div>
                </div>
            </asp:Panel>

            <div class="section-title">Subscribers (<asp:Label ID="lblSubscriberCount" runat="server" Text="0">
                </asp:Label>)
            </div>
            <asp:PlaceHolder ID="phSubscribers" runat="server"></asp:PlaceHolder>
        </div>
    </asp:Content>