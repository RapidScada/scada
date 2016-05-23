<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Table.aspx.cs" Inherits="Scada.Web.Plugins.Table.WFrmTable" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Table - Rapid SCADA</title>
    <script type="text/javascript" src="../../lib/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../js/api/eventtypes.js"></script>
    <script type="text/javascript" src="../../js/api/viewhub.js"></script>
    <script type="text/javascript" src="js/table.js"></script>
</head>
<body>
    <form id="frmTable" runat="server">
        <div>
            <input id="btn1" type="button" value="TitleChanged" />
            <input id="btn2" type="button" value="Navigate" />
            <input id="btn3" type="button" value="DateChanged" />
        </div>
        <div id="divLog"></div>
        <table><tr>
            <% for (int i = 0; i < 100; i++) { %>
            <td>Test</td>
            <% } %>
        </tr></table>
    </form>
</body>
</html>
