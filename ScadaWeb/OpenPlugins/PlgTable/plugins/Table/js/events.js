// Set current view date and process the consequent changes
function changeViewDate(date, notify) {
    setViewDate(date);
    //updateHourDataColHdrText();
    //restartUpdatingHourData();

    if (notify) {
        sendViewDateNotification(date);
    }
}

$(document).ready(function () {
    scada.clientAPI.rootPath = "../../";
    styleIOS();
    updateLayout();
    initViewDate();
    notifier = new scada.Notifier("#divNotif");
    notifier.startClearingNotifications();

    if (DEBUG_MODE) {
        initDebugTools();
    }

    $(window).on("resize " + scada.EventTypes.UPDATE_LAYOUT, function () {
        updateLayout();
    });

    // process the view date changing
    $(window).on(scada.EventTypes.VIEW_DATE_CHANGED, function (event, sender, extraParams) {
        changeViewDate(extraParams, false);
    });

    // select view date on click the calendar icon
    $("#spanDate i").click(function (event) {
        selectViewDate(changeViewDate);
    });

    // parse manually entered view date
    $("#txtDate").change(function () {
        parseViewDate($(this).val(), changeViewDate);
    });

    // switch event filter
    $("#spanAllEventsBtn").click(function () {
        $("#spanAllEventsBtn").addClass("selected");
        $("#spanEventsByViewBtn").removeClass("selected");
    });

    $("#spanEventsByViewBtn").click(function () {
        $("#spanAllEventsBtn").removeClass("selected");
        $("#spanEventsByViewBtn").addClass("selected");
    });

    // export events on the button click
    $("#spanExportBtn").click(function () {
        alert("Export is not implemented yet.");
    });
});
