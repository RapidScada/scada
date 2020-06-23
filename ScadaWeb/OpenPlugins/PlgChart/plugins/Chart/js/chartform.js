// The variables below must be defined in Chart.aspx
// object timeRange
// object chartData
// string locale
// int gapBetweenPoints
// string chartTitle
// string chartStatus
// object phrases

// Set the chart width and height
function updateLayout() {
    $("#divChart")
        .outerWidth($(window).width())
        .outerHeight($(window).height());
}

$(document).ready(function () {
    // chart parameters must be defined in Chart.aspx
    var chart = new scada.chart.Chart($("#divChart"));
    chart.displayOptions.locale = locale;
    chart.displayOptions.gapBetweenPoints = gapBetweenPoints;
    chart.timeRange = timeRange;
    chart.chartData = chartData;
    chart.buildDom();
    chart.showTitle(chartTitle);
    chart.showStatus(phrases.Generated + chartStatus);

    setTimeout(function () {
        updateLayout();
        chart.draw();
        chart.bindHintEvents();
    }, 0); // timeout is needed to open chart in a popup window in Firefox

    $(window).resize(function () {
        updateLayout();
        chart.draw();
    });
});
