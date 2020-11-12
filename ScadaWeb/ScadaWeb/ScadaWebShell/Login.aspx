<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Scada.Web.WFrmLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <!-- The above 3 meta tags are required by Bootstrap -->
    <title>Login</title>
    <link href="~/images/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="~/lib/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="~/lib/open-sans/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="~/css/login.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="lib/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="lib/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript">
        var phrases = <%= Scada.Web.WebUtils.DictionaryToJs("Scada.Web.WFrmLogin.Js") %>;
    </script>
    <script type="text/javascript" src="js/api/checkbrowser.js"></script>
    <script type="text/javascript" src="js/login.js"></script>
</head>
<body>
    <form id="LoginForm" runat="server">
        <div id="divLoginContainer">
            <div id="divLogo">
                <asp:HyperLink ID="hlRapidScada" runat="server" NavigateUrl="http://rapidscada.org" Target="_blank"><img src="images/gear.png" alt="Logo" /></asp:HyperLink></div>
            <div id="divTitle">
                <asp:Label ID="lblProductName" runat="server" Text="Rapid SCADA"></asp:Label>
            </div>
            <div id="divAlertsOuter"><div id="divAlertsInner"></div></div>
            <div id="divLogin">
                <div>
                    <asp:Label ID="lblUsername" runat="server" Text="Username" AssociatedControlID="txtUsername"></asp:Label></div>
                <div>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" autofocus="autofocus"></asp:TextBox></div>
                <div>
                    <asp:Label ID="lblPassword" runat="server" Text="Password" AssociatedControlID="txtPassword"></asp:Label></div>
                <div>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox></div>
                <div>
                    <asp:Panel ID="pnlRememberMe" runat="server" CssClass="checkbox pull-left">
                        <label>
                          <asp:CheckBox ID="chkRememberMe" runat="server" /><asp:Label ID="lblRememberMe" runat="server" Text="Remember me"></asp:Label>
                        </label>
                    </asp:Panel>
                    <div id="divLoginBtn" class="pull-right">
                        <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary" OnClick="btnLogin_Click" /></div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
