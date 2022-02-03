<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="_204703Q_AS_CodingAssignment_Ver2.ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <strong>Change Password<br />
            <br />
            </strong>New Password:<strong>
            <asp:TextBox ID="tb_ChangePassword" runat="server"></asp:TextBox>
            <br />
            <br />
            </strong>Confirm Password:<strong>
            <asp:TextBox ID="tb_ConfirmPassword" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="comparePassword" runat="server" Text=""></asp:Label>
            <br />
            <br />
            <asp:Button ID="btn_ConfirmPassword" runat="server" Text="Submit" OnClick="btn_ConfirmPassword_Click"/>
            </strong></div>
    </form>
</body>
</html>
