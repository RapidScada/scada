// Notifier control
var notifier = null;
// View ID. Must be defined in Table.aspx
var viewID = viewID || 0;
// Current data refresh rate
var refrRate = refrRate || 1000;
// Localized phrases
var phrases = phrases || {};
// Beginning of the time period
var timeFrom = timeFrom || 0;
// End of the time period
var timeTo = timeTo || 23;

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


$(document).ready(function () {
    scada.clientAPI.rootPath = "../../";
    styleIOS();
    updateLayout();
    setItemLinkWidths();
    notifier = new scada.Notifier("#divNotif");
    notifier.startClearingNotifications();

    // update layout on window and table area resize
    $(window).on("resize " + scada.EventTypes.UPDATE_LAYOUT, function () {
        updateLayout();
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

    $("span.hint").click(function () {
        $(this).css("display", "none");
    });

    // TODO
    var dialogs = viewHub ? viewHub.dialogs : null;
    if (dialogs) {
        $("#spanDate *").click(function (event) {
            dialogs.showCalendar($("#txtDate"), "09/06/2016", function (dialogResult, extraParams) {
                //alert("dialogResult = " + dialogResult + ", extraParams = " + extraParams);
            });
        });
    }

    if (DEBUG_MODE) {
        initDebugTools();
    } else {
        // TODO: start updating
    }
});
