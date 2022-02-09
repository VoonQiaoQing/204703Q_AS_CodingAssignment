<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SITConnectHomePage.aspx.cs" Inherits="_204703Q_AS_CodingAssignment_Ver2.SITConnectHomePage" %>
<link rel="stylesheet" href="css/style.css">

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            float: left;
        }
    </style>
    </head>
<body>
    <form id="form1" runat="server">
        <fieldset>
            <legend><strong>SIT Connect Home Page</strong></legend>
            <div>
            <div style="padding-left: 900px;">
            <asp:Image id="DisplayImage" runat="server" Height="85px" Width="85px" CssClass="auto-style1"></asp:Image>
            <asp:Button ID="changePassword" class="button"  runat="server" Text="Change Password" OnClick="changePassword_Click" Width="225px" Height="35px"/>
            <br />
            <asp:Button ID="checkLogs" class="button"  runat="server" Text="Check Logs" Visible="false" OnClick="checkLogs_Click" Height="35px" Width="110px"/>
            <asp:Button ID="logOut" class="button" runat="server" Text="Log Out" OnClick="logOut_Click" Height="35px" Width="110px"/>
            </div>
             <asp:Label ID="MustChangePassword" runat="server" Text=""></asp:Label>
                <br />
                <br />
                Welcome to SITConnect! SITConnect is a stationary store that provide allow staff and students to purchase stationaries. More upcoming tools for your convenience!<br />
                <br />
            </div>
            <strong>Black Friday Stationary Haul!</strong><br />
            <asp:Image ID="StationaryImage1" runat="server" Height="130px" Width="138px" />
            <br />
            <br />
            <strong>Qoqo Fountain Pens back in Stock!</strong><br />
            <asp:Image ID="StationaryImage2" runat="server" Height="130px" Width="138px" />
        </fieldset>
    </form>
</body>
</html>