<%@ Page Title="Movie Events" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true"
    CodeFile="Events.aspx.cs" Inherits="Events" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
        <style type="text/css">
            .events-wrapper {
                padding: 40px 20px;
                max-width: 1400px;
                margin: 0 auto;
                min-height: 80vh;
            }

            .events-header {
                display: flex;
                justify-content: space-between;
                align-items: center;
                margin-bottom: 30px;
                flex-wrap: wrap;
                gap: 20px;
            }

            .events-title {
                font-size: 32px;
                color: white;
                font-weight: 700;
            }

            .btn-create-event {
                padding: 12px 25px;
                background: linear-gradient(135deg, #ff4c3b 0%, #ff6b6b 100%);
                color: white;
                border: none;
                border-radius: 25px;
                font-size: 15px;
                font-weight: 600;
                cursor: pointer;
                transition: all 0.3s;
                box-shadow: 0 4px 15px rgba(255, 76, 59, 0.3);
            }

            .btn-create-event:hover {
                transform: translateY(-2px);
                box-shadow: 0 6px 20px rgba(255, 76, 59, 0.4);
            }

            .events-filters {
                display: flex;
                gap: 15px;
                margin-bottom: 30px;
                flex-wrap: wrap;
            }

            .filter-btn {
                padding: 8px 20px;
                background: rgba(255, 255, 255, 0.1);
                color: white;
                border: 1px solid rgba(255, 255, 255, 0.2);
                border-radius: 20px;
                cursor: pointer;
                transition: all 0.3s;
                text-decoration: none;
                display: inline-block;
            }

            .filter-btn:hover,
            .filter-btn.active {
                background: rgba(255, 76, 59, 0.3);
                border-color: #ff4c3b;
            }

            .events-grid {
                display: grid;
                grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
                gap: 25px;
                padding-bottom: 40px;
            }

            .event-card {
                background: linear-gradient(135deg, rgba(30, 30, 30, 0.95) 0%, rgba(50, 50, 50, 0.95) 100%);
                border-radius: 15px;
                overflow: hidden;
                transition: all 0.3s;
                border: 1px solid rgba(255, 255, 255, 0.1);
                position: relative;
                display: flex;
                flex-direction: column;
                height: 100%;
            }

            .event-card:hover {
                transform: translateY(-5px);
                box-shadow: 0 10px 30px rgba(0, 0, 0, 0.5);
                border-color: rgba(255, 76, 59, 0.5);
            }

            .event-poster {
                width: 100%;
                height: 250px;
                object-fit: cover;
                position: relative;
                flex-shrink: 0;
            }

            .event-status-badge {
                position: absolute;
                top: 10px;
                right: 10px;
                padding: 5px 15px;
                border-radius: 15px;
                font-size: 12px;
                font-weight: 600;
                text-transform: uppercase;
                z-index: 2;
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

            .event-content {
                padding: 20px;
                display: flex;
                flex-direction: column;
                flex: 1;
            }

            .event-movie-title {
                font-size: 18px;
                font-weight: 700;
                color: white;
                margin-bottom: 10px;
                min-height: 44px;
                /* Ensure 2 lines of text */
                display: -webkit-box;
                -webkit-line-clamp: 2;
                -webkit-box-orient: vertical;
                overflow: hidden;
            }

            .event-host {
                font-size: 13px;
                color: #aaa;
                margin-bottom: 15px;
            }

            .event-host a {
                color: #ff4c3b;
                text-decoration: none;
            }

            .event-details {
                display: flex;
                flex-direction: column;
                gap: 8px;
                margin-bottom: 15px;
            }

            .event-detail-row {
                display: flex;
                align-items: center;
                gap: 10px;
                font-size: 14px;
                color: #ccc;
            }

            .event-detail-row i {
                color: #ff4c3b;
                width: 20px;
            }

            .event-price {
                font-size: 16px;
                font-weight: 700;
                color: #4ecdc4;
                margin-bottom: 15px;
            }

            .event-actions {
                display: flex;
                gap: 10px;
            }

            .btn-view-event {
                flex: 1;
                padding: 10px;
                background: rgba(255, 255, 255, 0.1);
                color: white;
                border: 1px solid rgba(255, 255, 255, 0.2);
                border-radius: 8px;
                text-decoration: none;
                text-align: center;
                transition: all 0.3s;
                cursor: pointer;
            }

            .btn-view-event:hover {
                background: rgba(255, 76, 59, 0.2);
                border-color: #ff4c3b;
            }

            .empty-state {
                text-align: center;
                padding: 60px 20px;
                color: #888;
            }

            .empty-state i {
                font-size: 64px;
                margin-bottom: 20px;
                opacity: 0.5;
            }

            /* Modal Styles */
            .modal-overlay {
                display: none;
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background: rgba(0, 0, 0, 0.8);
                z-index: 1000;
                justify-content: center;
                align-items: center;
            }

            .modal-content {
                background: linear-gradient(135deg, #1e1e1e 0%, #2a2a2a 100%);
                padding: 30px;
                border-radius: 15px;
                width: 500px;
                max-width: 90%;
                max-height: 90vh;
                overflow-y: auto;
                box-shadow: 0 10px 40px rgba(0, 0, 0, 0.5);
                border: 1px solid rgba(255, 255, 255, 0.1);
            }

            .modal-title {
                margin-top: 0;
                color: white;
                margin-bottom: 25px;
                font-size: 24px;
                border-bottom: 2px solid #ff4c3b;
                padding-bottom: 10px;
            }

            .form-group {
                margin-bottom: 20px;
            }

            .form-group label {
                display: block;
                color: #ccc;
                margin-bottom: 8px;
                font-size: 14px;
                font-weight: 600;
            }

            .form-control {
                width: 100%;
                padding: 10px;
                border-radius: 8px;
                border: 1px solid #444;
                background: #2a2a2a;
                color: white;
                font-size: 14px;
            }

            .form-control:focus {
                outline: none;
                border-color: #ff4c3b;
            }

            .modal-footer {
                margin-top: 25px;
                display: flex;
                justify-content: flex-end;
                gap: 10px;
            }

            .btn-modal {
                padding: 10px 20px;
                border-radius: 8px;
                border: none;
                cursor: pointer;
                font-size: 14px;
                font-weight: 600;
                transition: all 0.3s;
            }

            .btn-cancel {
                background: rgba(255, 255, 255, 0.1);
                color: white;
                border: 1px solid rgba(255, 255, 255, 0.2);
            }

            .btn-cancel:hover {
                background: rgba(255, 255, 255, 0.2);
            }

            .btn-submit {
                background: linear-gradient(135deg, #ff4c3b 0%, #ff6b6b 100%);
                color: white;
            }

            .btn-submit:hover {
                transform: translateY(-2px);
                box-shadow: 0 4px 15px rgba(255, 76, 59, 0.4);
            }

            .subscriber-count {
                display: inline-flex;
                align-items: center;
                gap: 5px;
                font-size: 13px;
                color: #aaa;
            }

            .search-container {
                display: flex;
                gap: 10px;
                align-items: center;
            }

            .search-box {
                padding: 10px 15px;
                border-radius: 25px;
                border: 1px solid rgba(255, 255, 255, 0.2);
                background: rgba(255, 255, 255, 0.1);
                color: white;
                width: 250px;
            }

            .btn-search {
                background: transparent;
                color: white;
                border: 1px solid rgba(255, 255, 255, 0.2);
                width: 40px;
                height: 40px;
                border-radius: 50%;
                cursor: pointer;
                display: flex;
                align-items: center;
                justify-content: center;
                transition: all 0.3s;
            }

            .btn-search:hover {
                background: rgba(255, 76, 59, 0.2);
                border-color: #ff4c3b;
            }
        </style>
    </asp:Content>

    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        <div class="events-wrapper">
            <div class="events-header">
                <h1 class="events-title">Movie Watching Events</h1>
                <div class="search-container">
                    <asp:TextBox ID="txtSearchLocation" runat="server" CssClass="search-box"
                        placeholder="Search by location..." />
                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn-search" OnClick="btnSearch_Click">
                        <i class="fa fa-search"></i>
                    </asp:LinkButton>
                    <asp:Button ID="btnCreateEvent" runat="server" Text="+ Create Event" CssClass="btn-create-event"
                        OnClick="btnCreateEvent_Click" />
                </div>
            </div>

            <div class="events-filters">
                <a href="Events.aspx" class="filter-btn active">All Events</a>
                <a href="Events.aspx?filter=upcoming" class="filter-btn">Upcoming</a>
                <a href="Events.aspx?filter=myevents" class="filter-btn">My Events</a>
                <a href="Events.aspx?filter=subscribed" class="filter-btn">My Subscriptions</a>
            </div>

            <asp:PlaceHolder ID="phEvents" runat="server"></asp:PlaceHolder>

            <asp:Label ID="lblEmpty" runat="server" CssClass="empty-state" Visible="false">
                <i class="fa fa-calendar"></i><br />
                No events found. Be the first to create one!
            </asp:Label>
        </div>

        <!-- Create Event Modal -->
        <asp:Panel ID="pnlCreateModal" runat="server" CssClass="modal-overlay">
            <div class="modal-content">
                <h2 class="modal-title">Create Movie Watching Event</h2>

                <div class="form-group">
                    <label>Select Movie</label>
                    <asp:DropDownList ID="ddlMovie" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>

                <div class="form-group">
                    <label>Event Date</label>
                    <asp:TextBox ID="txtEventDate" runat="server" TextMode="Date" CssClass="form-control">
                    </asp:TextBox>
                </div>

                <div class="form-group">
                    <label>Start Time</label>
                    <asp:TextBox ID="txtStartTime" runat="server" TextMode="Time" CssClass="form-control">
                    </asp:TextBox>
                </div>

                <div class="form-group">
                    <label>Price (ILS)</label>
                    <asp:TextBox ID="txtPrice" runat="server" TextMode="Number" CssClass="form-control"
                        placeholder="0.00"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label>Location</label>
                    <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control"
                        placeholder="e.g. Tel Aviv, My House, Cinema City..."></asp:TextBox>
                </div>

                <div class="form-group">
                    <label>Status</label>
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                        <asp:ListItem Value="Open" Selected="True">Open</asp:ListItem>
                        <asp:ListItem Value="Closed">Closed</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <asp:Label ID="lblModalMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>

                <div class="modal-footer">
                    <asp:Button ID="btnCancelModal" runat="server" Text="Cancel" CssClass="btn-modal btn-cancel"
                        OnClick="btnCancelModal_Click" />
                    <asp:Button ID="btnSubmitEvent" runat="server" Text="Create Event" CssClass="btn-modal btn-submit"
                        OnClick="btnSubmitEvent_Click" />
                </div>
            </div>
        </asp:Panel>

        <script type="text/javascript">
            // Show/hide modal
            function showModal() {
                document.querySelector('.modal-overlay').style.display = 'flex';
            }

            function hideModal() {
                document.querySelector('.modal-overlay').style.display = 'none';
            }

            // Close modal when clicking outside
            document.addEventListener('DOMContentLoaded', function () {
                var modal = document.querySelector('.modal-overlay');
                if (modal) {
                    modal.addEventListener('click', function (e) {
                        if (e.target === modal) {
                            hideModal();
                        }
                    });
                }
            });
        </script>
    </asp:Content>