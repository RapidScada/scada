// Filter events by view
var eventsByView = true;
// Input channel filter for event requests
var cnlFilter = new scada.CnlFilter();
// Events data age after full update
var fullDataAge = 0;
// Events data age after partial update
var partialDataAge = 0;
// Number of the last received event
var lastEvNum = 0;

// Displayed event count. Must be defined in Events.aspx
var dispEventCnt = dispEventCnt || 0;

// Set current view date and process the consequent changes
function changeViewDate(date, notify) {
    setViewDate(date);
    //updateHourDataColHdrText();
    //restartUpdatingHourData();

    if (notify) {
        sendViewDateNotification(date);
    }
}

// Enable or disable events by view filter
function setEventsByVeiw(val) {
    eventsByView = val;
    cnlFilter.viewID = val ? viewID : 0;
    saveEventFilter();

    if (val) {
        $("#spanAllEventsBtn").removeClass("selected");
        $("#spanEventsByViewBtn").addClass("selected");
    } else {
        $("#spanAllEventsBtn").addClass("selected");
        $("#spanEventsByViewBtn").removeClass("selected");
    }
}

// Load the event filter from the cookies
function loadEventFilter() {
    var val = scada.utils.getCookie("Table.EventsByView");
    setEventsByVeiw(val != "false");
}

// Save the event filter in the cookies
function saveEventFilter() {
    scada.utils.setCookie("Table.EventsByView", eventsByView);
}

// Append new event to the event table
function appendEvent(tableElem, event) {
    var eventElem = $("<tr class='event'>" +
        "<td class='num'>" + event.Num + "</td>" +
        "<td class='time'>" + event.Time + "</td>" +
        "<td class='obj'>" + event.Obj + "</td>" +
        "<td class='dev'>" + event.KP + "</td>" +
        "<td class='cnl'>" + event.Cnl + "</td>" +
        "<td class='text'>" + event.Text + "</td>" +
        "<td class='ack'>" + event.Ack + "</td>" +
        "</tr>");

    if (event.Color) {
        eventElem.css("color", event.Color);
    }

    tableElem.append(eventElem);
}

// Request and display events.
// callback is a function (success)
function updateEvents(full, callback) {
    var startEvNum = full ? 0 : lastEvNum + 1;
    var reqDataAge = full ? fullDataAge : partialDataAge;

    scada.clientAPI.getEvents(viewDate, cnlFilter, dispEventCnt, startEvNum, reqDataAge,
        function (success, eventArr, dataAge) {
            if (success) {
                partialDataAge = dataAge;

                if (eventArr.length) {
                    lastEvNum = eventArr[eventArr.length - 1].Num;
                }

                var tableElem = $("#tblEvents");

                if (full) {
                    fullDataAge = dataAge;
                } else {
                    for (var event of eventArr) {
                        appendEvent(tableElem, event);
                    }
                }

                callback(true);
            } else {
                callback(false);
            }
        });
}

// Start cyclic updating of all displayed or newly added events
function startUpdatingEvents(full) {
    updateEvents(full, function (success) {
        if (!success) {
            notifier.addNotification(phrases.UpdateEventsError, true, notifier.DEF_NOTIF_LIFETIME);
        }

        setTimeout(startUpdatingEvents, full ? arcRefrRate : dataRefrRate, full);
    });
}

$(document).ready(function () {
    scada.clientAPI.rootPath = "../../";
    styleIOS();
    updateLayout();
    initViewDate();
    loadEventFilter();
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
        if (!$(this).hasClass("disabled")) {
            setEventsByVeiw(false);
        }
    });

    $("#spanEventsByViewBtn").click(function () {
        setEventsByVeiw(true);
    });

    // export events on the button click
    $("#spanExportBtn").click(function () {
        alert("Export is not implemented yet.");
    });

    // start updating events
    setTimeout(startUpdatingEvents, arcRefrRate, true);
    setTimeout(startUpdatingEvents, dataRefrRate, false);
});
