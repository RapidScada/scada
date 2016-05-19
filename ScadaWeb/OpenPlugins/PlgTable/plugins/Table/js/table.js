$(document).ready(function () {
    /*$(window).on(scada.eventTypes.viewNavigate, function (event) {
        console.log(event.type);
    });*/

    $(window).on(
        scada.eventTypes.viewTitleChanged + " " +
        scada.eventTypes.viewNavigate + " " +
        scada.eventTypes.viewDateChanged, function (event, extraParams) {
        var divLog = $("#divLog");
        divLog.html(divLog.html() + event.type + " - " + extraParams + "<br/>")
    });

    var viewHub = scada.viewHubLocator.getViewHub();

    if (viewHub) {
        $("#btn1").click(function () {
            viewHub.notify(window, scada.eventTypes.viewTitleChanged, "new title " + (new Date()));
        });

        $("#btn2").click(function () {
            viewHub.notify(window, scada.eventTypes.viewNavigate, 100);
        });

        $("#btn3").click(function () {
            viewHub.notify(window, scada.eventTypes.viewDateChanged, new Date());
        });
    }
});
