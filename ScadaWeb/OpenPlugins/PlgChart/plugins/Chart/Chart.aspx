<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chart.aspx.cs" Inherits="Scada.Web.Plugins.Chart.WFrmChart" EnableViewState="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Chart - Rapid SCADA</title>
    <link href="~/images/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="~/lib/open-sans/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="~/plugins/Chart/css/chart.min.css" rel="stylesheet" type="text/css" />
    <link href="~/plugins/Chart/css/chartform.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../lib/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../js/api/utils.js"></script>
    <script type="text/javascript" src="js/chart.js"></script>
    <script type="text/javascript">
        <%= chartDataBuilder.ToJs() %>
    </script>
    <script type="text/javascript" src="js/chartform.js"></script>
</head>
<body>
    <form id="frmChart" runat="server">
        <div id="divHeader" class="chart-header">
            <div class="chart-title">
                <asp:Label ID="lblTitle" runat="server" Text="Title"></asp:Label><asp:Label 
                    ID="lblStartDate" runat="server" Text=""></asp:Label>
            </div>
            <div class="chart-status">
                <asp:Label ID="lblGenerated" runat="server" Text="Generated"></asp:Label>
                <asp:Label ID="lblGenDT" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <div class="chart-wrapper">
            <canvas id="cnvChart" class="chart-canvas">Upgrade the browser to display chart.</canvas>
        </div>
    </form>
</body>
</html>
