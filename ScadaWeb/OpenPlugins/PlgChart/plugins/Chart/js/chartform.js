// Set the chart width and height
function updateLayout() {
    var BODY_PADDING = 20;
    $("#cnvChart")
        .outerWidth($(window).width() - BODY_PADDING)
        .outerHeight($(window).height() - $("#divHeader").outerHeight() - BODY_PADDING);
}

$(document).ready(function () {
    // chart parameters must be defined in Chart.aspx
    var chart = new scada.chart.Chart($("#cnvChart"));
    chart.displaySettings = displaySettings;
    chart.timeRange = timeRange;
    chart.chartData = chartData;

    updateLayout();
    chart.draw();

    $(window).resize(function () {
        updateLayout();
        chart.draw();
    });

    $(document).on("mousemove touchstart touchmove", function (event) {
        var touch = false;
        if (event.type == "touchstart") {
            event = event.originalEvent.touches[0];
            touch = true;
        }
        else if (event.type == "touchmove") {
            $(this).off("mousemove");
            event = event.originalEvent.touches[0];
            touch = true;
        }

        chart.showHint(event.pageX, event.pageY, touch);
        return false;
    })
});