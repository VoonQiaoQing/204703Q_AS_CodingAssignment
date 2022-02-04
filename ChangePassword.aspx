<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="_204703Q_AS_CodingAssignment_Ver2.ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Change Password</title>

        <script type="text/javascript">
            function validate() {
            var str = document.getElementById('<%=tb_ChangePassword.ClientID %>').value;
            if (str.length < 12) {
                document.getElementById("lbl_PasswordStrength").innerHTML = " Password length must be atleast 8 characters.";
                document.getElementById("lbl_PasswordStrength").style.color = "Red";
                return ("too_short");
            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("lbl_PasswordStrength").innerHTML = " Password require atleast 1 number";
                document.getElementById("lbl_PasswordStrength").style.color = "Red";
                return ("no_number");
            }
            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("lbl_PasswordStrength").innerHTML = " Password require atleast 1 uppercased letter";
                document.getElementById("lbl_PasswordStrength").style.color = "Red";
                return ("no_uppercase");
            }
            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = " Password require atleast 1 lowercased letter";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_lowercase");
            }
            else if (str.search(/[^a-zA-Z0-9]/) == -1) {
                document.getElementById("lbl_PasswordStrength").innerHTML = " Password require atleast 1 special character";
                document.getElementById("lbl_PasswordStrength").style.color = "Red";
                return ("no_lowercase");
            }
                document.getElementById("lbl_PasswordStrength").innerHTML = " Excellent!";
                document.getElementById("lbl_PasswordStrength").style.color = "Blue";
        }
        </script>
</head>
<body>
    <form id="form1" runat="server">
        <fieldset>
            <legend><strong>Change Password</strong></legend>
            <br />
            <div>
                New Password:<strong>
            <asp:TextBox ID="tb_ChangePassword" runat="server" onkeyup="javascript:validate()"></asp:TextBox><asp:Label ID="lbl_PasswordStrength" runat="server" Text=""> ...</asp:Label>
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
        </fieldset>
    </form>
</body>
</html>
