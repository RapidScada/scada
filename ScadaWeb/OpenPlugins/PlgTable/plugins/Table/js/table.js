// View hub. Must be defined in the parent window
var viewHub = scada.viewHubLocator.getViewHub();

$(document).ready(function () {
    /*$(window).on(scada.EventTypes.VIEW_NAVIGATE, function (event) {
        console.log(event.type);
    });*/

    $(window).on(
        scada.EventTypes.VIEW_TITLE_CHANGED + " " +
        scada.EventTypes.VIEW_NAVIGATE + " " +
        scada.EventTypes.VIEW_DATE_CHANGED, function (event, extraParams) {
        var divLog = $("#divLog");
        divLog.html(divLog.html() + event.type + " - " + extraParams + "<br/>")
    });

    if (viewHub) {
        $("#btn1").click(function () {
            viewHub.notify(window, scada.EventTypes.VIEW_TITLE_CHANGED, "new title " + (new Date()));
        });

        $("#btn2").click(function () {
            viewHub.notify(window, scada.EventTypes.VIEW_NAVIGATE, 100);
        });

        $("#btn3").click(function () {
            viewHub.notify(window, scada.EventTypes.VIEW_DATE_CHANGED, new Date());
        });
    }
});
