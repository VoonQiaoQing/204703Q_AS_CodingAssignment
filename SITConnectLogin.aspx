<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SITConnectLogin.aspx.cs" Inherits="_204703Q_AS_CodingAssignment_Ver2.SITConnectLogin" %>

<!DOCTYPE html>
<link rel="stylesheet" href="css/style.css">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title></title>
        <script src="https://www.google.com/recaptcha/api.js?render=6Lda1kMeAAAAAGqMZQEK3toLQzak0tAx1duE75er"></script>
    </head>
    
    <body>
        <form id="form1" runat="server">
        <fieldset class="fieldset">
        <legend><strong>SITCONNECT Login</strong><br /></legend>

        <p>Email<br />
            <asp:TextBox class="tb" ID="tb_loginEmail" runat="server"></asp:TextBox>
        </p>

        <p>Password<br />
            <asp:TextBox class="tb" ID="tb_loginPassword" TextMode="Password" runat="server"></asp:TextBox>
        </p>

        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>

        <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6Lda1kMeAAAAAGqMZQEK3toLQzak0tAx1duE75er', { action: 'Login' }).then(function (token) {
            document.getElementById("g-recaptcha-response").value = token;
                });
            }); 
        </script>

            <strong>

        <asp:Label ID="loginFail" runat="server" Text=""></asp:Label>
            </strong>
        <br />        
        <br />
        <asp:Button class="button" ID="registerUser" runat="server" Text="Register New User" Width="146px" OnClick="registerUser_Click" />
            
        <asp:Button class="button" ID="loginSubmit" runat="server" Text="Confirm" Width="146px" OnClick="loginSubmit_Click" />

        <br />
        <br />        
    </fieldset>
                    <fieldset class="fieldset" id="OTPField" runat="server" visible="false">
                <legend><strong>Email Confirmation</strong></legend>
                <div>
                <br />
                    <strong>A 6-digit Verification Code has been sent to your email.</strong><br />
                <br />
                6 Digit Code <br />
                    <asp:TextBox class="tb" ID="tb_EmailOTP" runat="server"></asp:TextBox>
                <br />
                <br />
                    <strong>
                <asp:Label ID="EmailLabel" runat="server" Text=""></asp:Label>
                    </strong>
                <br />
                <br />
                <asp:Button class="button" ID="OTPSubmit" Width="146px" runat="server" Text="Submit" OnClick="OTPSubmit_Click"  />&nbsp;
                    <asp:Button class="button" ID="ResendCode" runat="server" Width="146px" Text="Resend Code" OnClick="ResendCode_Click" />
            </div>
    </fieldset>
    </form>

    </body>
</html>
