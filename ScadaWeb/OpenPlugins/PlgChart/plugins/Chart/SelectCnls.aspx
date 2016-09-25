<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectCnls.aspx.cs" Inherits="Scada.Web.Plugins.Chart.WFrmSelectCnls" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <!-- The above 3 meta tags are required by Bootstrap -->
    <title>Select Channels - Rapid SCADA</title>
    <link href="~/lib/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="~/lib/open-sans/css/open-sans.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../js/controls/popup.js"></script>
    <script type="text/javascript" src="../../lib/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../lib/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../../js/api/utils.js"></script>
    <script type="text/javascript" src="../../js/api/eventtypes.js"></script>
    <script type="text/javascript" src="js/selectcnls.js"></script>
</head>
<body>
    <form id="frmSelectCnls" runat="server">
        <asp:Button ID="btnSubmit" runat="server" CssClass="hidden" Text="Submit" OnClick="btnSubmit_Click" />
        <asp:TextBox ID="txtCnls" runat="server" Text="101,102"></asp:TextBox>
        <asp:TextBox ID="txtViewIDs" runat="server" Text="2,2"></asp:TextBox>
    </form>
</body>
</html>
