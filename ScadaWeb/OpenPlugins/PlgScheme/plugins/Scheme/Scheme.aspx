<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Scheme.aspx.cs" Inherits="Scada.Web.Plugins.Scheme.WFrmScheme" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Scheme - Rapid SCADA</title>
    <link href="~/plugins/Scheme/css/scheme.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../js/jquery.min.js"></script>
    <script type="text/javascript">
        var viewID = <%= viewID %>;
    </script>
    <script type="text/javascript" src="js/scheme.js"></script>
</head>
<body>
    <form id="frmScheme" runat="server">
        <div id="divToolbar">
            <input id="btnLoadScheme" type="button" value="Load scheme" />
        </div>
        <div id="divScheme">
    
        </div>
    </form>
</body>
</html>
