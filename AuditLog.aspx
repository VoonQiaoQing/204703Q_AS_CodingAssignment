<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditLog.aspx.cs" Inherits="_204703Q_AS_CodingAssignment_Ver2.AuditLog" %>

<!DOCTYPE html>
<link rel="stylesheet" href="css/style.css">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <title>Audit Logs</title>
</head>
<body>
 <form id="form1" runat="server">

     <fieldset class="fieldset" style="margin: 50px; width: 1100px;">
         <legend>Audit Logs</legend>
        <asp:Button ID="headBack" class="button" runat="server" Text="Head Back to Home Page"/>
         <br />
         <br />
        <table style="text-align: center; width: 1000px; ">  
            <tr>  
                <td>  
                    <asp:PlaceHolder ID="DBDataPlaceHolder" runat="server"></asp:PlaceHolder>  
                </td>  
            </tr>  
        </table> 
      </fieldset>
 </form>
</body>
</html>
