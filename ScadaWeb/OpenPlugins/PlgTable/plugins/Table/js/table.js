// Column header time format options
var HEADER_TIME_OPTIONS = { hour: "2-digit", minute: "2-digit" };
// Column header date and time format options
var HEADER_DATETIME_OPTIONS = { month: "short", day: "2-digit", hour: "2-digit", minute: "2-digit" };

// Notifier control
var notifier = null;
// Current view date
var viewDate = null;

// Time period: firstHour <= timeFrom <= timeTo <= lastHour
// Beginning of the displayed time period
var timeFrom = null;
// End of the displayed time period
var timeTo = null;
// First possible hour of the time period
var firstHour = null;
// Last possible hour of the time period
var lastHour = null

// jQuery cells that display current data
var curDataCells = null;
// Array of columns those display hourly data, and consist of jQuery cells
var hourDataCols = [];
// Timeout ID of the hourly data updating timer
var updateHourDataTimeoutID = null;

// Set current view date to the initial value
function initViewDate() {
    if (viewHub) {
        if (viewHub.currentViewDate) {
            setViewDate(viewHub.currentViewDate);
        } else {
            viewHub.currentViewDate = today;
            setViewDate(today);
        }
    } else {
        setViewDate(today);
    }
}

// Parse manually entered view date and apply it
function parseViewDate(dateStr) {
    scada.clientAPI.parseDateTime(dateStr, function (success, value) {
        if (Number.isInteger(value)) {
            var parsedDate = new Date(value);
            setViewDate(parsedDate);
            sendViewDateNotification(parsedDate);
            restartUpdatingHourData();
        } else {
            $("#spanDate i").addClass("error");
        }
    });
}

// Set current view date
function setViewDate(date) {
    viewDate = date;
    $("#txtDate").val(date.toLocaleDateString(locale, VIEW_DATE_OPTIONS));
    $("#spanDate i").removeClass("error");
    updateHourDataColHdrText();
}

// Send view date changed notification to data windows
function sendViewDateNotification(date) {
    if (viewHub) {
        viewHub.notify(scada.EventTypes.VIEW_DATE_CHANGED, window, date);
    }
}

// Retrieve the time period from the control values
function retrieveTimePeriod() {
    timeFrom = parseInt($("#selTimeFrom").val());
    timeTo = parseInt($("#selTimeTo").val());
    firstHour = $("#divTblWrapper tr:first td.hour:first").data("hour");
    lastHour = $("#divTblWrapper tr:first td.hour:last").data("hour");
}

// Correct the beginning of the time period
function correctTimeFrom() {
    if (timeFrom > timeTo) {
        timeFrom = timeTo;
        $("#selTimeFrom").val(timeFrom);
    }
}

// Correct the end of the time period
function correctTimeTo()
{
    if (timeTo < timeFrom) {
        timeTo = timeFrom;
        $("#selTimeTo").val(timeFrom);
    }
}

// Save the time period in the cookies
function saveTimePeriod() {
    scada.utils.setCookie("Table.TimeFrom", $("#selTimeFrom").val());
    scada.utils.setCookie("Table.TimeTo", $("#selTimeTo").val());
}

// Select and prepare the current data cells
function initCurDataCells() {
    // select cells
    curDataCells = $("#divTblWrapper td.cur");

    // copy data-cnl attributes from rows to the cells
    curDataCells.each(function () {
        $(this).data("cnl", $(this).closest("tr.item").data("cnl"));
    });
}

// Select and prepare the hourly data columns
function initHourDataCols() {
    // init columns
    hourDataCols.length = lastHour - firstHour + 1;
    for (var i = 0, len = hourDataCols.length; i < len; i++) {
        hourDataCols[i] = [];
    }

    // get and arrange cells
    $("#divTblWrapper tr.item").each(function () {
        var row = $(this);
        var cnlNum = row.data("cnl");
        row.find("td.hour").each(function () {
            var cell = $(this);
            cell.data("cnl", cnlNum);
            var hour = cell.data("hour");
            hourDataCols[hour - firstHour].push(cell);
        });
    });
}

// Update header text of the hourly data columns
function updateHourDataColHdrText() {
    // converting date to string doesn't work properly on iOS 
    if (!scada.utils.iOS()) {
        $("#divTblWrapper tr.hdr td.hour").each(function () {
            var cell = $(this);
            var hour = cell.data("hour");
            var colDT = new Date(viewDate.getTime());
            colDT.setHours(hour);

            // replacing span is required for fixed table header calculations
            if (timeFrom >= 0) {
                // display time only
                cell.html("<span>" + colDT.toLocaleTimeString(locale, HEADER_TIME_OPTIONS) + "</span>");
            } else {
                // display date and time
                cell.html("<span>" + colDT.toLocaleString(locale, HEADER_DATETIME_OPTIONS) + "</span>");
            }
        });
    }
}

// Update visibility of the table view columns according to the time period
function updateHourDataColVisibility() {
    $("#divTblWrapper tr").each(function () {
        var row = $(this);
        var hourCells = row.find("td.hour");

        // show all the cells of the row
        hourCells.removeClass("hidden");

        // hide cells from the left
        var cellsToHide = timeFrom - firstHour;
        if (cellsToHide > 0) {
            var cells = hourCells.slice(0, cellsToHide);
            cells.addClass("hidden");
            if (row.hasClass("item")) {
                cells.text(""); // clear cell text
            }
        }

        // hide cells from the right
        cellsToHide = lastHour - timeTo;
        if (cellsToHide > 0) {
            cells = hourCells.slice(-cellsToHide);
            cells.addClass("hidden");
            if (row.hasClass("item")) {
                cells.text(""); // clear cell text
            }
        }
    });
}

// Set widths of the item links to fill cells
function setItemLinkWidths() {
    var cellWidth = $("#divTblWrapper td.cap:first").width();
    $("#divTblWrapper td.cap").each(function () {
        var cell = $(this);
        cell.children("a.lbl").outerWidth(cellWidth -
            cell.children("img.icon").outerWidth(true) -
            cell.children("span.cmd").outerWidth(true));
    });
}

// Show hint associated with the icon
function showHintByIcon(imgIcon) {
    var iconOffset = imgIcon.offset();
    var hint = imgIcon.siblings("span.hint");
    var hintTop = iconOffset.top + imgIcon.height();
    var hintHeight = hint.outerHeight();

    if (hintTop + hintHeight > $(document).height()) {
        hintTop = iconOffset.top - hintHeight;
        if (hintTop < 0) {
            hintTop = 0;
        }
    }

    hint
    .css({
        "left": iconOffset.left + imgIcon.outerWidth(true),
        "top": hintTop
    })
    .addClass("visible");
}

// Hide hint associated with the icon
function hideHintByIcon(imgIcon) {
    hideHint(imgIcon.siblings("span.hint"));
}

// Hide hint associated with the icon
function hideHint(spanHint) {
    spanHint.removeClass("visible");
}

// Display the given data in the cell
function displayCellData(cell, cnlDataMap) {
    var cnlNum = cell.data("cnl");
    if (cnlNum) {
        var cnlData = cnlDataMap.get(cnlNum);
        if (cnlData) {
            cell.text(cnlData.Text);
            cell.css("color", cnlData.Color);
        } else {
            cell.text("");
            cell.css("color", "");
        }
    }
}

// Request and display current data.
// callback is function (success)
function updateCurData(callback) {
    scada.clientAPI.getCurCnlDataExtByView(viewID, function (success, cnlDataExtArr) {
        if (success) {
            var cnlDataMap = scada.clientAPI.createCnlDataExtMap(cnlDataExtArr);

            curDataCells.each(function () {
                var cell = $(this);
                displayCellData(cell, cnlDataMap);
            });

            callback(true);
        } else {
            callback(false);
        }
    });
}

// Request and display hourly data.
// callback is function (success)
function updateHourData(callback) {
    scada.clientAPI.getHourCnlDataExtByView(viewDate, timeFrom, timeTo, viewID, scada.HourDataModes.INTEGER_HOURS,
        function (success, hourCnlDataExtArr) {
            if (success) {
                var hourDataMap = scada.clientAPI.createHourCnlDataExtMap(hourCnlDataExtArr);

                for (var hour = timeFrom; hour <= timeTo; hour++) {
                    var hourData = hourDataMap.get(hour);
                    var hourCol = hourDataCols[hour - firstHour];

                    if (hourData) {
                        var cnlDataMap = scada.clientAPI.createCnlDataExtMap(hourData.CnlDataExtArr);
                        $.each(hourCol, function () {
                            var cell = $(this);
                            displayCellData(cell, cnlDataMap);
                        });
                    } else {
                        $.each(hourCol, function () {
                            var cell = $(this);
                            cell.text("");
                            cell.css("color", "");
                        });
                    }
                }

                callback(true);
            } else {
                callback(false);
            }
        });
}

// Start cyclic updating of current data
function startUpdatingCurData() {
    updateCurData(function (success) {
        if (!success) {
            notifier.addNotification(phrases.UpdateCurDataError, true, notifier.DEF_NOTIF_LIFETIME);
        }

        setTimeout(startUpdatingCurData, dataRefrRate);
    });
}

// Start cyclic updating of hourly data
function startUpdatingHourData() {
    updateHourData(function (success) {
        if (!success) {
            notifier.addNotification(phrases.UpdateHourDataError, true, notifier.DEF_NOTIF_LIFETIME);
        }

        updateHourDataTimeoutID = setTimeout(startUpdatingHourData, arcRefrRate);
    });
}

// Restart updating of hourly data immediately
function restartUpdatingHourData() {
    clearTimeout(updateHourDataTimeoutID);
    startUpdatingHourData();
}

$(document).ready(function () {
    scada.clientAPI.rootPath = "../../";
    styleIOS();
    updateLayout();
    setItemLinkWidths();
    initViewDate();
    retrieveTimePeriod();
    initCurDataCells();
    initHourDataCols();
    updateHourDataColHdrText();
    scada.tableHeader.create();
    notifier = new scada.Notifier("#divNotif");
    notifier.startClearingNotifications();

    if (DEBUG_MODE) {
        initDebugTools();
    }

    // update layout on window and table area resize
    $(window).on("resize " + scada.EventTypes.UPDATE_LAYOUT, function () {
        updateLayout();
    });

    // process the view date changing
    $(window).on(scada.EventTypes.VIEW_DATE_CHANGED, function (event, sender, extraParams) {
        setViewDate(extraParams);
        restartUpdatingHourData();
    });

    // show calendar popup on click the calendar icon
    $("#spanDate i").click(function (event) {
        var dialogs = viewHub ? viewHub.dialogs : null;
        if (dialogs) {
            var txtDate = $("#txtDate");
            dialogs.showCalendar(txtDate, txtDate.val(), function (dialogResult, extraParams) {
                if (dialogResult) {
                    setViewDate(extraParams.date);
                    sendViewDateNotification(extraParams.date);
                    restartUpdatingHourData();
                }
            });
        } else {
            console.warn("Unable to show calendar because dialogs object is undefined");
        }
    });

    // parse manually entered view date
    $("#txtDate").change(function () {
        parseViewDate($(this).val());
    });

    // process the time period changing
    $("#selTimeFrom, #selTimeTo").change(function () {
        retrieveTimePeriod();

        if ($(this).attr("id") == "selTimeFrom") {
            correctTimeTo();
        } else {
            correctTimeFrom();
        }

        saveTimePeriod();
        updateHourDataColHdrText();
        updateHourDataColVisibility();
        scada.tableHeader.update();
        restartUpdatingHourData();
    });

    // export the table view on button click
    $("#spanExportBtn").click(function () {
        alert("Export is not implemented yet.");
    });

    // show and hide hint on hover or touch events
    $("#divTblWrapper img.icon").hover(
        function () {
            showHintByIcon($(this));
        }, function () {
            // timeout prevents label click on tablets
            var icon = $(this);
            setTimeout(function () { hideHintByIcon(icon); }, 100);
        }
    );

    $("#divTblWrapper img.icon").on("touchstart", function () {
        showHintByIcon($(this));
    });

    $("#divTblWrapper span.hint").on("touchend touchcancel", function () {
        var hint = $(this);
        setTimeout(function () { hideHint(hint); }, 100);
    });

    // show chart on label click
    $("#divTblWrapper a.lbl").click(function () {
        alert("Charts are not implemented yet.");
    });

    // send command on command icon click
    $("#divTblWrapper span.cmd").click(function () {
        alert("Commands are not implemented yet.");
    });

    // start updating data
    startUpdatingCurData();
    startUpdatingHourData();
});
