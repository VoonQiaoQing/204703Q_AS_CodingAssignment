<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SITConnectRegistration.aspx.cs" Inherits="_204703Q_AS_CodingAssignment_Ver2.SITConnectRegistration" %>


<!DOCTYPE html>
<link rel="stylesheet" href="css/style.css">
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
    <style type="text/css">
        .auto-style1 {
            margin-top: 0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <fieldset class="fieldset">
        <legend><strong>Registration Form</strong></legend>
        <p>
            First Name<br />
            <asp:TextBox class="tb" ID="firstName" runat="server" ></asp:TextBox>
            <br /><asp:Label ID="firstNameMsg" runat="server" Text=""> </asp:Label>
        </p>
        <p>
            Last Name<br />
            <asp:TextBox class="tb" ID="lastName" runat="server"></asp:TextBox>
            <br /><asp:Label ID="lastNameMsg" runat="server" Text=""> </asp:Label>
        </p>
        <p>
            Credit Card Name<br />
            <asp:TextBox class="tb" ID="CreditCardName" runat="server" ></asp:TextBox>
            <br /><asp:Label ID="creditNameMsg" runat="server" Text=""> </asp:Label>
        </p>
        <p>
            Credit Card Number<br />
            <asp:TextBox class="tb" ID="CreditCardNumber" runat="server" ></asp:TextBox>
            <br /><asp:Label ID="creditNumberMsg" runat="server" Text=""> </asp:Label>
        </p>
        <p>
            Expiry Date<br />
            <asp:TextBox class="tb" ID="CreditCardExpireMonth" runat="server" Width="145px"></asp:TextBox>
            /
            <asp:TextBox class="tb" ID="CreditCardExpireYear" runat="server" Width="145px"></asp:TextBox>
            <br /><asp:Label ID="creditExpireMonthMsg" runat="server" Text=""> </asp:Label>
            <asp:Label ID="creditExpireYearMsg" runat="server" Text=""> </asp:Label>
        </p>
        <p>
            CCV<br />
            <asp:TextBox class="tb" ID="CreditCardCCV" runat="server"></asp:TextBox>
            <br /><asp:Label ID="creditCCVMsg" runat="server" Text=""> </asp:Label>
        </p>
        <p>
            Email Address<br /><asp:TextBox class="tb" ID="Email" runat="server" ></asp:TextBox>
            <br />
            <asp:Label ID="emailMsg" runat="server" Text=""> </asp:Label>
            <asp:Label ID="AccountCreated" runat="server" Text=""> </asp:Label>
        </p>

        
         <p> Password<br />
            <asp:TextBox ID="Password" class="tb" TextMode="Password" runat="server" onkeyup="javascript:validate()"></asp:TextBox>
            <br /><asp:Label ID="lbl_pwdchecker" runat="server" Text=""> </asp:Label>
        </p>
            
        <p>
            Date of Birth<br />
            <asp:TextBox ID="DateofBirth" class="tb" runat="server" type="date"></asp:TextBox>
            <br /><asp:Label ID="DateofBirthMsg" runat="server" Text=""> </asp:Label>
        </p>

        <p>
            Photo:
            <br />
            <asp:Image ID="ImageDisplay" runat="server" Height="150px" Width="150px" />
            <br />

            <asp:FileUpload ID="ImageUpload" runat="server" onchange="ShowImagePreview(this);"/>
            <br />
            <br />
            <asp:RegularExpressionValidator ID="rev1" runat="server" ErrorMessage="Only jpeg,gif,png extensions allowed." ControlToValidate="ImageUpload" ForeColor="Red" ValidationExpression=".*\.([gG][iI][fF]|[jJ][pP][gG]|[jJ][pP][eE][gG]|[bB][mM][pP])$"></asp:RegularExpressionValidator>
            <br /><asp:Label ID="NoImageMsg" runat="server" Text=""> </asp:Label>
        </p>

        <asp:Button class="button" CausesValidation="true" ID="confirmPassword" runat="server" Text="Submit" OnClick="confirmPassword_Click"/>
      </fieldset>
    </form>
    </body>
</html>
