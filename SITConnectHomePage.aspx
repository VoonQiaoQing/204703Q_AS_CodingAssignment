<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SITConnectHomePage.aspx.cs" Inherits="_204703Q_AS_CodingAssignment_Ver2.SITConnectHomePage" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <strong>SIT Connect Home Page<br />
            <br />
            <asp:Label ID="MustChangePassword" runat="server" Text=""></asp:Label>
            <br />
            <br />
                <asp:Button ID="changePassword" runat="server" Text="Change Password" OnClick="changePassword_Click"/>
            </strong>
            <br />
            <br />
        <asp:Button ID="logOut" runat="server" Text="Log Out" OnClick="logOut_Click" Width="237px" />
            </div>
    </form>
</body>
</html>