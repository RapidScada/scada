<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Scheme.aspx.cs" Inherits="Scada.Web.Plugins.Scheme.WFrmScheme" EnableViewState="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Scheme - Rapid SCADA</title>
    <link href="~/lib/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="~/lib/open-sans/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="~/css/controls/notifier.min.css" rel="stylesheet" type="text/css" />
    <link href="~/plugins/Scheme/css/scheme.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../lib/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../js/api/utils.js"></script>
    <script type="text/javascript" src="../../js/api/clientapi.js"></script>
    <script type="text/javascript" src="../../js/api/eventtypes.js"></script>
    <script type="text/javascript" src="../../js/api/viewhub.js"></script>
    <script type="text/javascript" src="../../js/controls/notifier.js"></script>
    <script type="text/javascript" src="js/schemecommon.js"></script>
    <script type="text/javascript" src="js/schememodel.js"></script>
    <script type="text/javascript" src="js/schemerender.js"></script>
    <script type="text/javascript">
        var DEBUG_MODE = <%= debugMode ? "true" : "false" %>;
        var viewID = <%= viewID %>;
        var refrRate = <%= refrRate %>;
        var phrases = <%= phrases %>;
    </script>
    <script type="text/javascript" src="js/scheme.js"></script>
</head>
<body>
    <div id="divNotif" class="notifier">
    </div>
    <div id="divSchWrapper">
    </div>
    <div id="divToolbar"><asp:Label 
        ID="lblFitScreenBtn" runat="server" CssClass="tool-btn" ToolTip="Fit to Screen"><i class="fa fa-arrows" aria-hidden="true"></i></asp:Label><asp:Label 
        ID="lblFitWidthBtn" runat="server" CssClass="tool-btn" ToolTip="Fit to Width"><i class="fa fa-arrows-h" aria-hidden="true"></i></asp:Label><asp:Label 
        ID="lblZoomOutBtn" runat="server" CssClass="tool-btn" ToolTip="Zoom Out"><i class="fa fa-search-minus" aria-hidden="true"></i></asp:Label><asp:Label 
        ID="lblZoomInBtn" runat="server" CssClass="tool-btn" ToolTip="Zoom In"><i class="fa fa-search-plus" aria-hidden="true"></i></asp:Label><span id="spanCurScale">100%</span><div id="divDebugTools"><span 
            id="spanLoadSchemeBtn" class="tool-btn">Load Scheme</span><span 
            id="spanCreateDomBtn" class="tool-btn">Create DOM</span><span 
            id="spanStartUpdBtn" class="tool-btn">Start Updating</span><span 
            id="spanAddNotifBtn" class="tool-btn">Add Notification</span>
        </div>
    </div>
</body>
</html>
