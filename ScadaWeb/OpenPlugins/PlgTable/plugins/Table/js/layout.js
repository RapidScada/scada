// View date format options
var VIEW_DATE_OPTIONS = { year: "numeric", month: "long", day: "2-digit" };

// The view hub object
var viewHub = scada.viewHubLocator.getViewHub();

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
}

// Initialize debug tools
function initDebugTools() {
    $("#divDebugTools").css("display", "inline-block");

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
