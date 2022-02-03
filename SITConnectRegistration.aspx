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
            <asp:TextBox ID="firstName" runat="server" Width="231px"></asp:TextBox>
        </p>
        <p>
            Last Name:
            <asp:TextBox ID="lastName" runat="server" Width="230px"></asp:TextBox>
        </p>
        <p>
            Credit Card Info:
            <asp:TextBox ID="CreditCardInfo" runat="server"></asp:TextBox>
        </p>
        <p>
            Email Address: <asp:TextBox ID="Email" runat="server" Width="194px"></asp:TextBox>

        </p>

        
         <p> Password:
            <asp:TextBox ID="Password" runat="server" Width="241px" onkeyup="javascript:validate()"></asp:TextBox>
            <asp:Label ID="lbl_pwdchecker" runat="server" Text=""> PasswordChecker </asp:Label>
        </p>

        <p>
            Date of Birth:
            <asp:TextBox ID="DateofBirth" runat="server" type="date" Width="207px"></asp:TextBox>
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

        <asp:Button CausesValidation="true" ID="confirmPassword" runat="server" Text="Submit" OnClick="confirmPassword_Click" Width="241px" />
          
    </form>
    </body>
</html>
