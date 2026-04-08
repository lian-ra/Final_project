<%@ Page Title="Register" Language="C#" MasterPageFile="~/Design.master" AutoEventWireup="true" CodeFile="Regi.aspx.cs" Inherits="Regi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        /* Center the registration box on the screen */
        .register-wrapper {
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 40px 20px;
        }

        /* The main card container */
        .register-card {
            background: rgba(20, 20, 20, 0.85); /* Dark semi-transparent background */
            padding: 40px;
            border-radius: 15px;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.5); /* Soft shadow */
            border: 1px solid rgba(255, 255, 255, 0.1);
            width: 100%;
            max-width: 500px; /* Slightly wider than login for more fields */
            text-align: center;
        }

        .register-card h1 {
            color: #ffffff;
            font-size: 32px;
            margin-bottom: 30px;
            font-weight: 600;
            letter-spacing: 1px;
            text-transform: uppercase;
        }

        /* Input group styling */
        .form-group {
            margin-bottom: 20px;
            text-align: left;
        }

        .form-label {
            color: #ccc;
            font-size: 14px;
            margin-bottom: 8px;
            display: block;
            font-weight: 500;
        }

        /* Styled TextBoxes */
        .form-control-custom {
            width: 100%;
            padding: 12px 15px;
            border-radius: 25px;
            border: 1px solid rgba(255, 255, 255, 0.2);
            background: rgba(255, 255, 255, 0.1);
            color: white;
            font-size: 16px;
            outline: none;
            transition: all 0.3s ease;
            box-sizing: border-box;
        }

        .form-control-custom:focus {
            background: rgba(255, 255, 255, 0.2);
            border-color: #ff4c3b; /* Accent color */
            box-shadow: 0 0 10px rgba(255, 76, 59, 0.3);
        }

        /* Dropdown styling */
        .dropdown-custom {
            width: 100%;
            padding: 10px 15px;
            border-radius: 25px;
            border: 1px solid rgba(255, 255, 255, 0.2);
            background: #222;
            color: white;
            font-size: 16px;
            height: 45px;
        }

        /* Calendar styling fix */
        .calendar-container {
            background: white;
            border-radius: 10px;
            padding: 10px;
            margin-top: 5px;
            color: black;
        }

        /* File Upload Styling */
        .file-upload-custom {
            color: white;
            margin-top: 10px;
        }

        /* Button Styling */
        .btn-custom {
            border: none;
            padding: 12px 0;
            border-radius: 25px;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
            width: 100%;
            transition: transform 0.2s, box-shadow 0.2s;
            margin-top: 20px;
        }

        .btn-signup {
            background: #ff4c3b;
            color: white;
        }

        .btn-signup:hover {
            background: #e04332;
            transform: translateY(-2px);
            box-shadow: 0 5px 15px rgba(255, 76, 59, 0.4);
        }

        /* Profile Image Preview */
        .profile-img-preview {
            border-radius: 50%;
            object-fit: cover;
            border: 3px solid #ff4c3b;
            margin-bottom: 20px;
            display: block;
            margin-left: auto;
            margin-right: auto;
        }

    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="register-wrapper">
        <div class="register-card">
            <h1>Register</h1>

            <div style="text-align: center;">
                <asp:Image ID="img" runat="server" CssClass="profile-img-preview" Height="100px" ImageUrl="~/MyPics/Profile.jpg" Width="100px" />
            </div>

            <div class="form-group">
                <asp:Label ID="label1" runat="server" Text="Username" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtUName" runat="server" CssClass="form-control-custom" placeholder="Choose a username"></asp:TextBox>
                
                        
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage=" Username required" ControlToValidate="txtFName"></asp:RequiredFieldValidator>


            </div>
          

            <div class="form-group">
                <asp:Label ID="label2" runat="server" Text="Password" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtPass" runat="server" CssClass="form-control-custom" TextMode="Password" placeholder="Create a password"></asp:TextBox>
            </div>

            <div class="form-group">
                <asp:Label ID="label3" runat="server" Text="First Name" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtFName" runat="server" CssClass="form-control-custom" placeholder="Enter first name"></asp:TextBox>
            </div>

            <div class="form-group">
                <asp:Label ID="label4" runat="server" Text="Last Name" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtLName" runat="server" CssClass="form-control-custom" placeholder="Enter last name"></asp:TextBox>
            </div>

            <div class="form-group">
                <asp:Label ID="label5" runat="server" Text="Address" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtAdd" runat="server" CssClass="form-control-custom" placeholder="Enter address"></asp:TextBox>
            </div>

            <div class="form-group">
                <asp:Label ID="label6" runat="server" Text="Email" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control-custom" TextMode="Email" placeholder="Enter email address"></asp:TextBox>
                
                    <asp:RegularExpressionValidator ID="re_Email" runat="server" ErrorMessage="RegularExpressionValidator" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            </div>
            
                                      

            <div class="form-group">
                <asp:Label ID="label7" runat="server" Text="Phone" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control-custom" placeholder="Enter phone number" EnableTheming="False" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ErrorMessage="Phone number is required." ControlToValidate="txtPhone" ForeColor="#ff4c3b" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Must be exactly 10 digits." ControlToValidate="txtPhone" ValidationExpression="^\d{10}$" ForeColor="#ff4c3b" Display="Dynamic"></asp:RegularExpressionValidator>
            </div>
         
       
        
            <div class="form-group">
                <asp:Label ID="txtGender" runat="server" Text="Gender" CssClass="form-label"></asp:Label>
                <asp:DropDownList ID="dpdphone" runat="server" CssClass="dropdown-custom">
                    <asp:ListItem>Female</asp:ListItem>
                    <asp:ListItem>Male</asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <asp:Label ID="Label12" runat="server" Text="Birthday" CssClass="form-label"></asp:Label>
                <div class="calendar-container">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div style="margin-bottom: 10px; display: flex; justify-content: space-between;">
                                <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DateDropdown_SelectedIndexChanged" CssClass="dropdown-custom" style="width: 48%;"></asp:DropDownList>
                                <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DateDropdown_SelectedIndexChanged" CssClass="dropdown-custom" style="width: 48%;"></asp:DropDownList>
                            </div>
                            <asp:Calendar ID="Calendar1" runat="server" 
                                OnDayRender="Calendar1_DayRender"
                                OnSelectionChanged="Calendar1_SelectionChanged"
                                OnVisibleMonthChanged="Calendar1_VisibleMonthChanged"
                                SelectionMode="Day" 
                                ShowDayHeader="True" 
                                ShowGridLines="False"
                                ShowTitle="True"
                                DayNameFormat="Short"
                                FirstDayOfWeek="Sunday"
                                NextPrevFormat="ShortMonth"
                                BackColor="White"
                                ForeColor="Black"
                                BorderColor="White"
                                Width="100%">
                                <TitleStyle BackColor="#ff4c3b" Font-Bold="True" ForeColor="White" Height="30px" />
                                <NextPrevStyle Font-Bold="True" ForeColor="White" />
                                <DayHeaderStyle Font-Bold="True" BackColor="#f0f0f0" />
                                <SelectedDayStyle BackColor="#ff4c3b" ForeColor="White" />
                                <TodayDayStyle BackColor="#eeeeee" ForeColor="Black" />
                                <OtherMonthDayStyle ForeColor="#999999" />
                            </asp:Calendar>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="form-group">
                <asp:Label runat="server" Text="Profile Picture" CssClass="form-label"></asp:Label>
                <asp:FileUpload ID="FileUpload1" runat="server" CssClass="file-upload-custom" />
            </div>

            <asp:Button runat="server" ID="btnsave" Text="Sign Up" OnClick="RegisterNewUser" CssClass="btn-custom btn-signup" />
        </div>
    </div>

</asp:Content>