<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailConfirmation.aspx.cs" Inherits="_204703Q_AS_CodingAssignment_Ver2.EmailConfirmation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <strong>Email Confirmation<br />
            <br />
            A 6 digit code has been send to your email. <asp:Label ID="EmailLabel" runat="server" Text=""></asp:Label><br />
            <br />
            </strong>6 Digit Code:

        <asp:TextBox ID="EmailOTP" runat="server"></asp:TextBox>
            <br />
        </div>

        <asp:Button ID="Submit" runat="server" Text="Submit" Width="321px" OnClick="Submit_Click" />

        <br />

        <br />
        <asp:Button ID="Back" runat="server" Text="Back" Width="151px" OnClick="Back_Click" />

        &nbsp;&nbsp;&nbsp;

        <asp:Button ID="ResendCode" runat="server" Text="Resend Code" Width="143px" OnClick="ResendCode_Click" />
    </form>
</body>
</html>
