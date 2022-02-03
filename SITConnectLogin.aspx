<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SITConnectLogin.aspx.cs" Inherits="_204703Q_AS_CodingAssignment_Ver2.SITConnectLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://www.google.com/recaptcha/api.js?render=6Lda1kMeAAAAAGqMZQEK3toLQzak0tAx1duE75er"></script>

</head>
<body>

    <form id="form1" runat="server">

        <div id="AccountLockdown"  runat="server" visible="false">
        <strong> Account Lockdown</strong>
        <br />
        <asp:Label ID="LockdownMessage" runat="server" Text=""></asp:Label>
        <br />
        <br />
        </div>
        
        <div id="LoginSection"  runat="server" visible="true">
        
        <strong>SITCONNECT Login</strong>

        <div id="sup" runat="server" visible="true">

        <p>Email:
            <asp:TextBox ID="loginEmail" runat="server"></asp:TextBox>
        </p>


        <p>Password:
            <asp:TextBox ID="loginPassword" runat="server" Width="141px"></asp:TextBox>
        </p>

        <!--<asp:LinkButton ID="LinkButton1" runat="server">Forgot Password?</asp:LinkButton>

        <div class="g-recaptcha" data-sitekey="6LfUt0IeAAAAADgdE6xzv0A3xhlhenlHc6krTSof"></div>-->
        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
        <!--<asp:label ID="lbl_gScore" runat="server" EnableViewState="false"> </asp:label> -->

        <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6Lda1kMeAAAAAGqMZQEK3toLQzak0tAx1duE75er', { action: 'Login' }).then(function (token) {
            document.getElementById("g-recaptcha-response").value = token;
                });
            }); 
        </script>

        <p>

        <asp:Button ID="loginSubmit" runat="server" Text="Confirm" Width="263px" OnClick="loginSubmit_Click" />
        </p>

        </div>
        <asp:label ID="lblMessage" runat="server" Text=""> </asp:label> 
        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>

        <br />

        <br />

        <div id="emailconfirm"  runat="server" visible="false">
            <strong>Email Confirmation<br />
            <br />
            A 6 digit code has been send to your email.<br />
            <br />
            </strong>
            6 Digit Code: <asp:TextBox ID="tb_EmailOTP" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="EmailLabel" runat="server" Text=""></asp:Label>
            <br />
            <br />
            <asp:Button ID="submitorfail" runat="server" Text="Submit" OnClick="submitorfail_Click" />

        <br />

        <br />

        <asp:Button ID="ResendCode" runat="server" Text="Resend Code" Width="319px" OnClick="ResendCode_Click" />
        </div>
        </div>

    </form>

    </body>
</html>
