<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Events.aspx.cs" Inherits="Scada.Web.Plugins.Table.WFrmEvents" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Events - Rapid SCADA</title>
    <script type="text/javascript" src="../../lib/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../js/api/eventtypes.js"></script>
    <script type="text/javascript" src="../../js/api/viewhub.js"></script>
    <script type="text/javascript" src="js/table.js"></script>
</head>
<body>
    <form id="frmEvents" runat="server">
        <div>
            <input id="btn1" type="button" value="TitleChanged" />
            <input id="btn2" type="button" value="Navigate" />
            <input id="btn3" type="button" value="DateChanged" />
        </div>
        <div id="divLog"></div>
    </form>
</body>
</html>
