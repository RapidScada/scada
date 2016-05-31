<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Events.aspx.cs" Inherits="Scada.Web.Plugins.Table.WFrmEvents" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Events - Rapid SCADA</title>
    <link href="~/lib/open-sans/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="~/css/controls/notifier.min.css" rel="stylesheet" type="text/css" />
    <link href="~/plugins/Table/css/layout.min.css" rel="stylesheet" type="text/css" />
    <link href="~/plugins/Table/css/events.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../lib/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../js/api/utils.js"></script>
    <script type="text/javascript" src="../../js/api/clientapi.js"></script>
    <script type="text/javascript" src="../../js/api/eventtypes.js"></script>
    <script type="text/javascript" src="../../js/api/viewhub.js"></script>
    <script type="text/javascript" src="../../js/controls/notifier.js"></script>
    <script type="text/javascript" src="js/layout.js"></script>
    <script type="text/javascript" src="js/events.js"></script>
</head>
<body>
    <form id="frmEvents" runat="server">
        <div id="divNotif" class="notifier">
        </div>
        <div id="divToolbar"><asp:TextBox ID="txtDate" runat="server"></asp:TextBox><div id="divDebugTools"><span 
                id="spanTitleChangedBtn" class="tool-btn">TitleChanged</span><span 
                id="spanNavigateBtn" class="tool-btn">Navigate</span><span 
                id="spanDateChangedBtn" class="tool-btn">DateChanged</span>
            </div>
        </div>
        <div id="divTblWrapper">
        </div>
    </form>
</body>
</html>
