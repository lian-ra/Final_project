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
                </div>
            </asp:Panel>

            <div class="section-title">Subscribers (<asp:Label ID="lblSubscriberCount" runat="server" Text="0">
                </asp:Label>)
            </div>
            <asp:PlaceHolder ID="phSubscribers" runat="server"></asp:PlaceHolder>
        </div>
    </asp:Content>