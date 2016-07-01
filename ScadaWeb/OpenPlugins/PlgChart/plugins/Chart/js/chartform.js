$(document).ready(function () {
    // chart parameters must be defined in Chart.aspx
    var chartData = new scada.chart.ChartData();
    chartData.TimePoints = timePoints;
    chartData.Trends = [trend];
    chartData.QuantityName = quantityName;

    var chart = new scada.chart.Chart($("#cnvChart"));
    chart.displaySettings = displaySettings;
    chart.timeRange = timeRange;
    chart.chartData = chartData;
    chart.draw();

    $(window).resize(function () {
        chart.draw();
    });

    $(window).mousemove(function (event) {
        chart.showHint(event.pageX, event.pageY);
    })
});