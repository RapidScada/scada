<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoView.aspx.cs" Inherits="Scada.Web.WFrmNoView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>No View</title>
    <link href="~/lib/open-sans/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="~/css/noview.min.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmNoView" runat="server">
    <div id="divViewNotExist">
        <asp:Label ID="lblViewNotExist" runat="server" Text="The requested view does not exist or you have insufficient rights to access it."></asp:Label>
    </div>
    </form>
</body>
</html>
