// Period of preventing auto scrolling the event table after a user scrolling, ms
var STOP_SCROLL_PERIOD = 30000;

// Filter events by view
var eventsByView = true;
// Input channel filter for event requests
var cnlFilter = null;

// Array of jQuery objects, where each element represents an event row
var eventRows = [];
// Initial events loading
var initialLoad = true;
// Events data age after full update
var fullDataAge = 0;
// Events data age after partial update
var partialDataAge = 0;
// Number of the last received event
var lastEvNum = 0;
// The last received event has the alternate style
var lastEvAlt = true;
// Date and time of recent user activity
var activityTime = 0;
// Event sound is enabled
var evSoundEnabled = false;
// Timeout ID of the full events updating timer
var fullUpdateTimeoutID = null;
// Timeout ID of the partial events updating timer
var partUpdateTimeoutID = null;

// The variables below must be defined in Events.aspx
// Displayed event count
var dispEventCnt = dispEventCnt || 0;
// Right to view all data
var viewAllRight = viewAllRight || false;

// Set current view date and process the consequent changes
function changeViewDate(date, notify) {
    setViewDate(date);
    resetEvents();

    if (notify) {
        sendViewDateNotification(date);
    }
}

// Initialize the page controls
function initControls() {
    if (!viewAllRight) {
        $("#lblAllEventsBtn").addClass("disabled");
    }
}

// Enable or disable events by view filter
function setEventsByVeiw(val) {
    eventsByView = val || !viewAllRight;
    cnlFilter = new scada.CnlFilter();
    cnlFilter.viewID = eventsByView ? viewID : 0;
    saveEventFilter();

    if (eventsByView) {
        $("#lblAllEventsBtn").removeClass("selected");
        $("#lblEventsByViewBtn").addClass("selected");
    } else {
        $("#lblAllEventsBtn").addClass("selected");
        $("#lblEventsByViewBtn").removeClass("selected");
    }
}

// Load the event filter from the local storage
function loadEventFilter() {
    var val = localStorage.getItem("Table.EventsByView");
    setEventsByVeiw(val != "false");
}

// Save the event filter in the local storage
function saveEventFilter() {
    localStorage.setItem("Table.EventsByView", eventsByView);
}

// Export events to Excel
function exportEvents() {
    var exportUrl = "EventsRepOut.aspx?viewID=" + (eventsByView ? viewID : "") +
        "&" + scada.utils.dateToQueryString(viewDate);
    window.location = exportUrl;
}

// Play a sound if a new event is received
function playEventBeep() {
    $("#audEvent")[0].play();
}

// Show event acknowledgement dialog
function showEventAck(evNum) {
    var dialogs = viewHub ? viewHub.dialogs : null;
    if (dialogs) {
        dialogs.showEventAck(viewDate, evNum, viewID, function (dialogResult) {
            if (dialogResult) {
                restartUpdatingEvents();
            }
        });
    } else {
        console.warn(DIALOGS_UNDEFINED);
    }
}

// Generate HTML of acknowledgement cell
function generateAckHtml(evNum, ack) {
    return "<a href='javascript:showEventAck(" + evNum + ");'>" + ack + "</a>";
}

// Create detached jQuery object that represents an event row
function createEventRow(event) {
    var eventRow = $("<tr class='event'>" +
        "<td class='num'>" + event.Num + "</td>" +
        "<td class='time'>" + event.Time + "</td>" +
        "<td class='obj'>" + event.Obj + "</td>" +
        "<td class='dev'>" + event.KP + "</td>" +
        "<td class='cnl'>" + event.Cnl + "</td>" +
        "<td class='text'>" + event.Text + "</td>" +
        "<td class='ack'>" + generateAckHtml(event.Num, event.Ack) + "</td>" +
        "</tr>");

    if (event.Color) {
        eventRow.css("color", event.Color);
    }

    eventRow.data("num", event.Num);
    return eventRow;
}

// Append new event to the event table
function appendEvent(tableElem, event) {
    var eventRow = createEventRow(event);

    lastEvAlt = !lastEvAlt;
    if (lastEvAlt) {
        eventRow.addClass("alt");
    }

    eventRows.push(eventRow);
    tableElem.append(eventRow);
}

// Rewrite event HTML
function rewriteEvent(eventRow, event) {
    if (eventRow.data("num") == event.Num) {
        eventRow.children("td.time").text(event.Time);
        eventRow.children("td.obj").text(event.Obj);
        eventRow.children("td.dev").text(event.KP);
        eventRow.children("td.cnl").text(event.Cnl);
        eventRow.children("td.text").text(event.Text);
        eventRow.children("td.ack").html(generateAckHtml(event.Num, event.Ack));
    } else {
        console.error(scada.utils.getCurTime() + " Event number mismatch");
    }
}

// Append new events to the event table starting from the specified index
function appendEvents(tableElem, eventArr, startIndex) {
    var len = eventArr.length ? eventArr.length : 0;
    var beep = false;

    for (var i = startIndex; i < len; i++) {
        var event = eventArr[i];
        if (event.Sound) {
            beep = true;
        }
        appendEvent(tableElem, event);
    }

    if (beep && evSoundEnabled) {
        playEventBeep();
    }
}

// Rewrite HTML of the events from the specified range not including the end index
function rewriteEvents(tableElem, eventArr, startIndex, endIndex) {
    for (var i = startIndex; i < endIndex; i++) {
        rewriteEvent(eventRows[i], eventArr[i]);
    }
}

// Remove events from the specified range not including the end index
function removeEvents(tableElem, startIndex, endIndex) {
    for (var i = startIndex; i < endIndex; i++) {
        eventRows[i].remove();
    }
    eventRows.splice(startIndex, endIndex - startIndex);
}

// Clear the event table
function clearEvents(tableElem) {
    tableElem.find("tr.event").remove();
    eventRows = [];
    lastEvAlt = true;
}

// Reset the event table to the default state and restart updating
function resetEvents() {
    $("#divTblWrapper").addClass("hidden");
    $("#divNoEvents").addClass("hidden");
    $("#divLoading").removeClass("hidden");

    clearEvents($("#tblEvents"));
    initialLoad = true;
    fullDataAge = 0;
    partialDataAge = 0;
    lastEvNum = 0;
    evSoundEnabled = false;

    restartUpdatingEvents();
}

// Set elements visibility after loading events
function afterLoading() {
    evSoundEnabled = true; // enable event sound starting from the second response
    $("#divLoading").addClass("hidden");

    if ($("#tblEvents tr.event:first").length > 0) {
        var divTblWrapper = $("#divTblWrapper");
        divTblWrapper.removeClass("hidden");
        setTimeout(scada.tableHeader.update.bind(scada.tableHeader), 0);

        // scroll down the event table on initial loading or if a user has been idle for the time
        if (initialLoad || (new Date()) - activityTime > STOP_SCROLL_PERIOD) {
            var scrollHeight = divTblWrapper[0].scrollHeight;
            var newScrollTop = scrollHeight - divTblWrapper.innerHeight();
            if (divTblWrapper.scrollTop() < newScrollTop) {
                if (initialLoad) {
                    divTblWrapper.scrollTop(newScrollTop);
                } else {
                    divTblWrapper.animate({ scrollTop: newScrollTop }, "slow");
                }
            }
        }
        initialLoad = false;
    } else {
        $("#divTblWrapper").addClass("hidden");
        $("#divNoEvents").removeClass("hidden");
    }
}

// Request and display events.
// callback is a function (success)
function updateEvents(full, callback) {
    var reqViewDate = viewDate;
    var reqCnlFilter = cnlFilter;
    var startEvNum = full ? 0 : lastEvNum + 1;
    var reqDataAge = full ? fullDataAge : partialDataAge;

    scada.clientAPI.getEvents(reqViewDate, reqCnlFilter, dispEventCnt, startEvNum, reqDataAge,
        function (success, eventArr, dataAge) {
            if (reqViewDate != viewDate || reqCnlFilter != cnlFilter) {
                // do nothing
            }
            else if (success) {
                var tableElem = $("#tblEvents");
                var eventArrLen = eventArr.length ? eventArr.length : 0;

                if (full) {
                    if (eventArrLen > 0) {
                        var firstEvNum = eventArr[0].Num;
                        var firstEvInd = 0;
                        var eventRowsCnt = eventRows.length;
                        while (firstEvInd < eventRowsCnt && eventRows[firstEvInd].data("num") < firstEvNum) {
                            firstEvInd++;
                        }

                        var eventsToMerge = eventRowsCnt - firstEvInd;
                        var evNumsMatched = eventsToMerge <= eventArrLen;
                        var eventRowInd = firstEvInd;
                        var eventArrInd = 0;
                        while (eventRowInd < eventRowsCnt && eventArrInd < eventArrLen && evNumsMatched) {
                            evNumsMatched = eventRows[eventRowInd].data("num") == eventArr[eventArrInd].Num;
                            eventRowInd++;
                            eventArrInd++;
                        }

                        if (evNumsMatched) {
                            // merge received events with the existing
                            removeEvents(tableElem, 0, firstEvInd);
                            rewriteEvents(tableElem, eventArr, 0, eventsToMerge);
                            appendEvents(tableElem, eventArr, eventsToMerge);
                        } else {
                            // clear and fill again the event table
                            clearEvents(tableElem);
                            appendEvents(tableElem, eventArr, 0);
                        }
                    } else if (fullDataAge != dataAge) {
                        // clear the event table
                        clearEvents(tableElem);
                    }
                } else if (eventArrLen > 0) {
                    // skip already loaded events
                    var startIndex = 0;
                    while (startIndex < eventArrLen && eventArr[startIndex].Num <= lastEvNum) {
                        startIndex++;
                    }

                    // append new events to the event table
                    appendEvents(tableElem, eventArr, startIndex);
                }

                partialDataAge = dataAge;

                if (full || startEvNum <= 1) {
                    fullDataAge = dataAge;
                }

                if (eventArrLen > 0) {
                    lastEvNum = eventArr[eventArrLen - 1].Num;
                }

                afterLoading();
                callback(true);
            } else {
                callback(false);
            }
        });
}

// Start cyclic updating all displayed events
function startFullUpdatingEvents() {
    updateEvents(true, function (success) {
        if (!success) {
            notifier.addNotification(phrases.UpdateEventsError, scada.NotifTypes.ERROR, notifier.DEF_NOTIF_LIFETIME);
        }

        fullUpdateTimeoutID = setTimeout(startFullUpdatingEvents, arcRefrRate);
    });
}

// Start cyclic updating newly added events
function startPartialUpdatingEvents() {
    updateEvents(false, function (success) {
        if (!success) {
            notifier.addNotification(phrases.UpdateEventsError, scada.NotifTypes.ERROR, notifier.DEF_NOTIF_LIFETIME);
        }

        partUpdateTimeoutID = setTimeout(startPartialUpdatingEvents, dataRefrRate);
    });
}

// Restart updating events immediately
function restartUpdatingEvents() {
    clearTimeout(fullUpdateTimeoutID);
    clearTimeout(partUpdateTimeoutID);

    startFullUpdatingEvents();
    partUpdateTimeoutID = setTimeout(startPartialUpdatingEvents, dataRefrRate);
}

$(document).ready(function () {
    scada.clientAPI.rootPath = "../../";
    scada.clientAPI.ajaxQueue = scada.ajaxQueueLocator.getAjaxQueue();
    styleIOS();
    updateLayout();
    initViewDate();
    initControls();
    loadEventFilter();
    scada.tableHeader.create();
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
    $("#lblAllEventsBtn").click(function () {
        if (viewAllRight) {
            setEventsByVeiw(false);
            resetEvents();
        }
    });

    $("#lblEventsByViewBtn").click(function () {
        setEventsByVeiw(true);
        resetEvents();
    });

    // export events on the button click
    $("#spanExportBtn").click(function () {
        exportEvents();
    });

    // register the activity time
    $("#divTblWrapper").on("mousemove wheel touchstart", function () {
        activityTime = new Date();
    });

    // start updating events
    restartUpdatingEvents();
});
