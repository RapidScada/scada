<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chart.aspx.cs" Inherits="Scada.Web.Plugins.Chart.WFrmChart" EnableViewState="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Chart</title>
    <link href="~/images/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="~/lib/open-sans/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="~/plugins/Chart/css/chart.min.css" rel="stylesheet" type="text/css" />
    <link href="~/plugins/Chart/css/chartform.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../lib/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../js/api/utils.js"></script>
    <script type="text/javascript" src="js/chart.js"></script>
    <script type="text/javascript">
        <%= sbClientScript.ToString() %>
        var phrases = <%= Scada.Web.WebUtils.DictionaryToJs("Scada.Web.Plugins.Chart.WFrmChart.Js") %>;
    </script>
    <script type="text/javascript" src="js/chartform.js"></script>
</head>
<body>
    <form id="frmChart" runat="server">
        <div id="divChart" class="chart"></div>
    </form>
</body>
</html>
