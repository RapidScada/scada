// Notifier control
var notifier = null;
// Current view date
var viewDate = null;
// Beginning of the time period
var timeFrom = null;
// End of the time period
var timeTo = null;

// jQuery cells that display current data
var curDataCells = null;
// Array of columns those display hourly data, and consist of jQuery cells
var hourDataCols = [];

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
    // TODO: ajax request here
    var d = new Date(dateStr);
    alert(d);
}

// Set current view date
function setViewDate(date) {
    viewDate = date;
    $("#txtDate").val(date.toLocaleDateString(locale, VIEW_DATE_OPTIONS));
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

// Select and prepare current data cells
function initCurDataCells() {
    // select cells
    curDataCells = $("#divTblWrapper td.cur");

    // copy data-cnl attributes from rows to the cells
    curDataCells.each(function () {
        $(this).data("cnl", $(this).closest("tr.item").data("cnl"));
    });
}

// Select and prepare hourly data columns
function initHourDataCols() {
    // init columns
    hourDataCols.length = timeTo - timeFrom + 1;
    for (var hour = timeFrom; hour <= timeTo; hour++) {
        hourDataCols[hour] = [];
    }

    // get and arrange cells
    $("#divTblWrapper tr.item").each(function () {
        var row = $(this);
        var cnlNum = row.data("cnl");
        row.find("td.hour").each(function () {
            var cell = $(this);
            cell.data("cnl", cnlNum);
            var hour = cell.data("hour");
            hourDataCols[hour].push(cell);
        });
    });
}

// Set visibility of the table view columns according to the time period
function updateTableViewHours() {
    var firstHour = $("#divTblWrapper tr:first td.hour:first").data("hour");
    var lastHour = $("#divTblWrapper tr:first td.hour:last").data("hour");

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
function showHint(imgIcon) {
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
function hideHint(imgIcon) {
    imgIcon.siblings("span.hint").removeClass("visible");
}

// Request and display current data.
// callback is function (success)
function updateCurrentData(callback) {
    scada.clientAPI.getCurCnlDataExtByView(viewID, function (success, cnlDataExtArr) {
        if (success) {
            var curCnlDataMap = scada.clientAPI.createCnlDataExtMap(cnlDataExtArr);

            if (curCnlDataMap) {
                curDataCells.each(function () {
                    var cell = $(this);
                    var cnlNum = cell.data("cnl");
                    if (cnlNum) {
                        // display current data
                        var curCnlDataExt = curCnlDataMap.get(cnlNum);
                        if (curCnlDataExt) {
                            cell.text(curCnlDataExt.Text);
                            cell.css("color", curCnlDataExt.Color);
                        } else {
                            cell.text("");
                            cell.css("color", "");
                        }
                    }
                });
            }

            callback(true);
        } else {
            callback(false);
        }
    });
}

// Start cyclic updating of current data
function startUpdatingCurData() {
    updateCurrentData(function (success) {
        if (!success) {
            notifier.addNotification(phrases.UpdateCurDataError, true, notifier.DEF_NOTIF_LIFETIME);
        }

        setTimeout(startUpdatingCurData, refrRate);
    });
}

// Start cyclic updating of hourly data
function startUpdatingHourData() {
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
    notifier = new scada.Notifier("#divNotif");
    notifier.startClearingNotifications();

    // update layout on window and table area resize
    $(window).on("resize " + scada.EventTypes.UPDATE_LAYOUT, function () {
        updateLayout();
    });

    // process the view date changing
    $(window).on(scada.EventTypes.VIEW_DATE_CHANGED, function (event, sender, extraParams) {
        setViewDate(extraParams);
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
        updateTableViewHours();
    });

    // show and hide hint on hover and click
    $("#divTblWrapper img.icon").hover(
        function () {
            showHint($(this));
        }, function () {
            hideHint($(this));
        }
    );

    $("#divTblWrapper span.hint").click(function () {
        $(this).css("display", "none");
    });

    if (DEBUG_MODE) {
        initDebugTools();
    } else {
        startUpdatingCurData();
        startUpdatingHourData();
    }
});
