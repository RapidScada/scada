<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chart.aspx.cs" Inherits="Scada.Web.Plugins.Chart.WFrmChart" EnableViewState="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Chart - Rapid SCADA</title>
    <link href="~/lib/open-sans/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="~/plugins/Chart/css/chart.min.css" rel="stylesheet" type="text/css" />
    <link href="~/plugins/Chart/css/chartform.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../lib/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../js/api/utils.js"></script>
    <script type="text/javascript" src="js/chart.js"></script>
    <script type="text/javascript">
        var displaySettings = new scada.chart.DisplaySettings();
        displaySettings.locale = "<%= Scada.Localization.Culture.Name %>";
        displaySettings.chartGap = <%= chartGap %> / scada.chart.const.SEC_PER_DAY;

        var timeRange = new scada.chart.TimeRange();
        timeRange.startDate = Date.UTC(<%= string.Format("{0}, {1}, {2}", startDate.Year, startDate.Month - 1, startDate.Day) %>);
        timeRange.startTime = 0;
        timeRange.endTime = 1;

        var trend = new scada.chart.TrendExt();
        trend.CnlNum = <%= cnlNum %>;
        trend.CnlName = "<%= cnlName %>";
        trend.TrendPoints = <%= trendPoints %>;

        var timePoints = <%= timePoints %>;
        var quantityName = "<%= quantityName %>";
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
        <canvas id="cnvChart" class="chart">Upgrade the browser to display chart.</canvas>
    </form>
</body>
</html>
