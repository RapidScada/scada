<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Scada.Web.WFrmLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <!-- The above 3 meta tags are required by Bootstrap -->
    <title>Login - Rapid SCADA</title>
    <link href="~/images/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="~/lib/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="~/lib/open-sans/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="~/css/login.min.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="LoginForm" runat="server">
        <div id="divLoginContent">
            <div id="divLogin">
                <div>
                    <asp:Label ID="lblUsername" runat="server" Text="Username"></asp:Label></div>
                <div>
                    <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox></div>
                <div>
                    <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label></div>
                <div>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox></div>
                <div>
                    <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" /></div>
            </div>
        </div>
    </form>
</body>
</html>
