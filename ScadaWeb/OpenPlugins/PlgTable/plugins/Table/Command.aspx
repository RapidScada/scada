<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Command.aspx.cs" Inherits="Scada.Web.Plugins.Table.WFrmCommand" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <!-- The above 3 meta tags are required by Bootstrap -->
    <title>Command - Rapid SCADA</title>
    <link href="~/lib/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="~/lib/open-sans/css/open-sans.css" rel="stylesheet" type="text/css" />
    <!--<script type="text/javascript" src="../../lib/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../lib/bootstrap/js/bootstrap.min.js"></script>-->
</head>
<body>
    <form id="frmCommand" runat="server">
    <div>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Button" />
    </div>
    </form>
</body>
</html>
