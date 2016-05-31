<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Table.aspx.cs" Inherits="Scada.Web.Plugins.Table.WFrmTable" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Table - Rapid SCADA</title>
    <link href="~/lib/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="~/lib/open-sans/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="~/css/controls/notifier.min.css" rel="stylesheet" type="text/css" />
    <link href="~/plugins/Table/css/layout.min.css" rel="stylesheet" type="text/css" />
    <link href="~/plugins/Table/css/table.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../lib/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../js/api/utils.js"></script>
    <script type="text/javascript" src="../../js/api/clientapi.js"></script>
    <script type="text/javascript" src="../../js/api/eventtypes.js"></script>
    <script type="text/javascript" src="../../js/api/viewhub.js"></script>
    <script type="text/javascript" src="../../js/controls/notifier.js"></script>
    <script type="text/javascript" src="js/layout.js"></script>
    <script type="text/javascript">
        var DEBUG_MODE = <%= debugMode ? "true" : "false" %>;
        var viewID = <%= viewID %>;
        var refrRate = <%= refrRate %>;
        var phrases = <%= phrases %>;
    </script>
    <script type="text/javascript" src="js/table.js"></script>
</head>
<body>
    <form id="frmTable" runat="server">
        <div id="divNotif" class="notifier">
        </div>
        <div id="divToolbar"><span class="tool-ctrl"><asp:TextBox ID="txtDate" runat="server"></asp:TextBox></span><span class="tool-ctrl"><asp:DropDownList 
            ID="ddlTimeFrom" runat="server"></asp:DropDownList> - <asp:DropDownList 
            ID="ddlTimeTo" runat="server"></asp:DropDownList></span><div id="divDebugTools"><span 
                id="spanTitleChangedBtn" class="tool-btn">TitleChanged</span><span 
                id="spanNavigateBtn" class="tool-btn">Navigate</span><span 
                id="spanDateChangedBtn" class="tool-btn">DateChanged</span>
            </div>
        </div>
        <div id="divTblWrapper">
            <%= tableViewHtml %>
        </div>
    </form>
</body>
</html>
