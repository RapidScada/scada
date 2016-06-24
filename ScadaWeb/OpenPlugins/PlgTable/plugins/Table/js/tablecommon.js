// View date format options
var VIEW_DATE_OPTIONS = { year: "numeric", month: "long", day: "2-digit" };
// Error message that is shown if dialogs object is undefined
var DIALOGS_UNDEFINED = "Dialogs object is undefined";

// The view hub object
var viewHub = scada.viewHubLocator.getViewHub();
// Notifier control
var notifier = null;
// Current view date
var viewDate = null;

// The variables below must be defined in *.aspx
// View ID
var viewID = viewID || 0;
// Current data refresh rate
var dataRefrRate = dataRefrRate || 1000;
// Archive data refresh rate
var arcRefrRate = arcRefrRate || 10000;
// Localized phrases
var phrases = phrases || {};
// Current server date
var today = today || new Date();
// Application culture name
var locale = locale || "en-GB";

// Apply additional css styles in case of using iOS
function styleIOS() {
    if (scada.utils.iOS()) {
        $("#divTblWrapper").css({
            "overflow": "scroll",
            "-webkit-overflow-scrolling": "touch",
            "width": 0, // initial size
            "height": 0
        });
    }
}

// Update layout of the top level div elements
function updateLayout() {
    var divNotif = $("#divNotif");
    var divToolbar = $("#divToolbar");
    var divTblWrapper = $("#divTblWrapper");
    var notifHeight = divNotif.css("display") == "block" ? divNotif.outerHeight() : 0;
    var toolbarHeight = divToolbar.outerHeight();
    var windowWidth = $(window).width();

    $("body").css("padding-top", notifHeight + toolbarHeight);
    divNotif.outerWidth(windowWidth);
    divTblWrapper
        .outerWidth(windowWidth)
        .outerHeight($(window).height() - notifHeight - toolbarHeight);
    divToolbar.css("top", notifHeight);

    scada.tableHeader.update();
}

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

// Set current view date
function setViewDate(date) {
    viewDate = date;
    $("#txtDate").val(date.toLocaleDateString(locale, VIEW_DATE_OPTIONS));
    $("#spanDate i").removeClass("error");
}

// Send view date changed notification to data windows
function sendViewDateNotification(date) {
    if (viewHub) {
        viewHub.notify(scada.EventTypes.VIEW_DATE_CHANGED, window, date);
    }
}

// Select view date using a calendar popup.
// changeViewDateFunc is function (date, notify)
function selectViewDate(changeViewDateFunc) {
    var dialogs = viewHub ? viewHub.dialogs : null;
    if (dialogs) {
        var txtDate = $("#txtDate");
        dialogs.showCalendar(txtDate, txtDate.val(), function (dialogResult, extraParams) {
            if (dialogResult) {
                // copy the date to avoid toLocaleDateString() bug in Edge
                var date = new Date(extraParams.date.getTime());
                changeViewDateFunc(date, true);
            }
        });
    } else {
        console.warn(DIALOGS_UNDEFINED);
    }
}

// Parse manually entered view date and apply it
function parseViewDate(dateStr, changeViewDateFunc) {
    scada.clientAPI.parseDateTime(dateStr, function (success, value) {
        if (Number.isInteger(value)) {
            var parsedDate = new Date(value);
            changeViewDateFunc(parsedDate, true);
        } else {
            $("#spanDate i").addClass("error");
        }
    });
}

// Initialize debug tools
function initDebugTools() {
    $("#divDebugTools").addClass("show");

    $(window).on(
        scada.EventTypes.VIEW_TITLE_CHANGED + " " +
        scada.EventTypes.VIEW_NAVIGATE + " " +
        scada.EventTypes.VIEW_DATE_CHANGED,
        function (event, sender, extraParams) {
            notifier.addNotification(scada.utils.getCurTime() + " Receive: " +
                event.type + " - " + sender.document.title + " - " + extraParams);
    });

    if (viewHub) {
        $("#spanTitleChangedBtn").click(function () {
            notifier.addNotification(scada.utils.getCurTime() + " Send: VIEW_TITLE_CHANGED");
            viewHub.notify(scada.EventTypes.VIEW_TITLE_CHANGED, window, "Title " + Math.random());
        });

        $("#spanNavigateBtn").click(function () {
            notifier.addNotification(scada.utils.getCurTime() + " Send: VIEW_NAVIGATE");
            viewHub.notify(scada.EventTypes.VIEW_NAVIGATE, window, 100);
        });

        $("#spanDateChangedBtn").click(function () {
            notifier.addNotification(scada.utils.getCurTime() + " Send: VIEW_DATE_CHANGED");
            viewHub.notify(scada.EventTypes.VIEW_DATE_CHANGED, window, new Date());
        });
    }
}
