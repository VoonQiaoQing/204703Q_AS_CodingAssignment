<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SITConnectRegistration.aspx.cs" Inherits="_204703Q_AS_CodingAssignment_Ver2.SITConnectRegistration" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>Registration Form</title>
    <script src="//code.jquery.com/jquery-1.11.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function validate() {
            var str = document.getElementById('<%=Password.ClientID %>').value;
            if (str.length < 12) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password length must be atleast 8 characters.";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("too_short");
            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require atleast 1 number";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_number");
            }
            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require atleast 1 uppercased letter";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_uppercase");
            }
            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require atleast 1 lowercased letter";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_lowercase");
            }
            else if (str.search(/[^a-zA-Z0-9]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require atleast 1 special character";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_lowercase");
            }
            document.getElementById("lbl_pwdchecker").innerHTML = "Excellent!";
            document.getElementById("lbl_pwdchecker").style.color = "Blue";
        }

        function ShowImagePreview(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#<%=ImageDisplay.ClientID%>').prop('src', e.target.result);
                    };
                        reader.readAsDataURL(input.files[0]);
                    }
                }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <strong>Registration Form</strong>
        <p>
            First Name:
            <asp:TextBox ID="firstName" runat="server" Width="266px"></asp:TextBox><asp:Label ID="firstNameMsg" runat="server" Text=""> </asp:Label>
        </p>
        <p>
            Last Name:
            <asp:TextBox ID="lastName" runat="server" Width="262px"></asp:TextBox><asp:Label ID="lastNameMsg" runat="server" Text=""> </asp:Label>
        </p>
        <p>
            Credit Card Name:
            <asp:TextBox ID="CreditCardName" runat="server" Width="192px"></asp:TextBox><asp:Label ID="creditNameMsg" runat="server" Text=""> </asp:Label>
        </p>
        <p>
            Credit Card Number:
            <asp:TextBox ID="CreditCardNumber" runat="server"></asp:TextBox><asp:Label ID="creditNumberMsg" runat="server" Text=""> </asp:Label>
        </p>
        <p>
            Expiry Date:
            <asp:TextBox ID="CreditCardExpireMonth" runat="server" Width="99px"></asp:TextBox><asp:Label ID="creditExpireMonthMsg" runat="server" Text=""> </asp:Label>
            /
            <asp:TextBox ID="CreditCardExpireYear" runat="server" Width="118px"></asp:TextBox><asp:Label ID="creditExpireYearMsg" runat="server" Text=""> </asp:Label>
        </p>
        <p>
            CCV:
            <asp:TextBox ID="CreditCardCCV" runat="server"></asp:TextBox><asp:Label ID="creditCCVMsg" runat="server" Text=""> </asp:Label>
        </p>
        <p>
            Email Address: <asp:TextBox ID="Email" runat="server" Width="194px"></asp:TextBox>
            <asp:Label ID="emailMsg" runat="server" Text=""> </asp:Label>
        </p>

        
         <p> Password:
            <asp:TextBox ID="Password" runat="server" Width="241px" onkeyup="javascript:validate()"></asp:TextBox>
            <asp:Label ID="lbl_pwdchecker" runat="server" Text=""> PasswordChecker </asp:Label>
        </p>

        <p>
            Date of Birth:
            <asp:TextBox ID="DateofBirth" runat="server" type="date" Width="207px"></asp:TextBox>
            <asp:Label ID="DateofBirthMsg" runat="server" Text=""> </asp:Label>
        </p>

        <p>
            Photo:
            <br />
            <asp:Image ID="ImageDisplay" runat="server" Height="150px" Width="150px" />
            <br />

            <asp:FileUpload ID="ImageUpload" runat="server" onchange="ShowImagePreview(this);"/>
            <br />
            <asp:RegularExpressionValidator ID="rev1" runat="server" ErrorMessage="Only jpeg,gif,png extensions allowed." ControlToValidate="ImageUpload" ForeColor="Red" ValidationExpression=".*\.([gG][iI][fF]|[jJ][pP][gG]|[jJ][pP][eE][gG]|[bB][mM][pP])$"></asp:RegularExpressionValidator>
        </p>

        <asp:Label ID="AccountCreated" runat="server" Text=""> </asp:Label><br />

        <asp:Button CausesValidation="true" ID="confirmPassword" runat="server" Text="Submit" OnClick="confirmPassword_Click" Width="241px" />
          
    </form>
    </body>
</html>
