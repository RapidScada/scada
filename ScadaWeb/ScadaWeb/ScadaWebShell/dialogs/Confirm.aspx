<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Confirm.aspx.cs" Inherits="Scada.Web.Dialogs.WFrmConfirm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Confirmation</title>
    <link href="~/css/dialogs/confirm.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../lib/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../js/api/utils.js"></script>
    <script type="text/javascript" src="../js/api/eventtypes.js"></script>
    <script type="text/javascript" src="../js/controls/popup.js"></script>
    <script type="text/javascript" src="../js/dialogs/confirm.js"></script>
</head>
<body>
    <form id="frmConfirm" runat="server">
        <asp:Label ID="lblConfirm" runat="server" Text="Are you sure?"></asp:Label>
    </form>
</body>
</html>
