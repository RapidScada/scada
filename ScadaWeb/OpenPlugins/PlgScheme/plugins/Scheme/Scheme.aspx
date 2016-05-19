<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Scheme.aspx.cs" Inherits="Scada.Web.Plugins.Scheme.WFrmScheme" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Scheme - Rapid SCADA</title>
    <link href="~/lib/open-sans/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="~/plugins/Scheme/css/scheme.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../lib/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../js/api/utils.js"></script>
    <script type="text/javascript" src="../../js/api/clientapi.js"></script>
    <script type="text/javascript" src="../../js/api/eventtypes.js"></script>
    <script type="text/javascript" src="../../js/api/viewhub.js"></script>
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
    <form id="frmScheme" runat="server">
        <div id="divDebugTools"><input 
            id="btnLoadScheme" type="button" value="Load scheme" /><input 
            id="btnCreateDom" type="button" value="Create DOM" /><input 
            id="btnStartUpd" type="button" value="Start updating" /><input 
            id="btnAddNotif" type="button" value="Add notification" />
        </div>
        <div id="divNotif">
        </div>
        <div id="divSchParent">
        </div>
    </form>
</body>
</html>
