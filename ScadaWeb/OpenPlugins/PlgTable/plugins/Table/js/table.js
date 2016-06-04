// Notifier control
var notifier = null;
// View ID. Must be defined in Table.aspx
var viewID = viewID || 0;
// Current data refresh rate
var refrRate = refrRate || 1000;
// Localized phrases
var phrases = phrases || {};

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

    hint.css({
        "display": "inline-block",
        "left": iconOffset.left + imgIcon.outerWidth(true),
        "top": hintTop
    });
}

// Hide hint associated with the icon
function hideHint(imgIcon) {
    imgIcon.siblings("span.hint").css("display", "none");
}


$(document).ready(function () {
    scada.clientAPI.rootPath = "../../";
    styleIOS();
    updateLayout();
    setItemLinkWidths();
    notifier = new scada.Notifier("#divNotif");
    notifier.startClearingNotifications();

    if (DEBUG_MODE) {
        initDebugTools();
    } else {
        // TODO: start updating
    }

    // update layout on window and table area resize
    $(window).on("resize " + scada.EventTypes.UPDATE_LAYOUT, function () {
        updateLayout();
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
            dialogs.showCalendar($("#txtDate"), null);
        });
    }
});
