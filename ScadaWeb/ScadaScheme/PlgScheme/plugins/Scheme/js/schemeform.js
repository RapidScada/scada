// Scheme object
var scheme = new scada.scheme.Scheme();
// Notifier control
var notifier = null;
// Possible scale values
var scaleVals = [0.1, 0.25, 0.5, 0.75, 1, 1.25, 1.5, 2, 2.5, 3, 4, 5];

// The variables below must be defined in Scheme.aspx
// View ID
var viewID = viewID || 0;
// Scheme refresh rate
var refrRate = refrRate || 1000;
// Localized phrases
var phrases = phrases || {};
// View control right
var controlRight = controlRight || false;

// Load the scheme
function loadScheme(viewID) {
    scheme.load(viewID, function (success) {
        if (success) {
            if (!DEBUG_MODE) {
                // show errors
                if (Array.isArray(scheme.loadErrors) && scheme.loadErrors.length > 0) {
                    notifier.addNotification(scheme.loadErrors.join("<br/>"),
                        scada.NotifTypes.ERROR, notifier.INFINITE_NOTIF_LIFETIME);
                }

                // show scheme
                scheme.createDom(controlRight);
                loadScale();
                displayScale();
                startUpdatingScheme();
            }
        } else {
            notifier.addNotification(phrases.LoadSchemeError +
                " <input type='button' value='" + phrases.ReloadButton + "' onclick='reloadScheme()' />",
                scada.NotifTypes.ERROR, notifier.INFINITE_NOTIF_LIFETIME);
        }
    });
}

// Reload the scheme
function reloadScheme() {
    location.reload(true);
}

// Start cyclic scheme updating process
function startUpdatingScheme() {
    scheme.updateData(scada.clientAPI, function (success) {
        if (!success) {
            notifier.addNotification(phrases.UpdateError, scada.NotifTypes.ERROR, notifier.DEF_NOTIF_LIFETIME);
        }

        setTimeout(startUpdatingScheme, refrRate);
    });
}

// Bind handlers of the toolbar buttons
function initToolbar() {
    var Scales = scada.scheme.Scales;

    $("#lblFitScreenBtn").click(function () {
        scheme.setScale(Scales.FIT_SCREEN);
        displayScale();
        saveScale(Scales.FIT_SCREEN);
    });

    $("#lblFitWidthBtn").click(function () {
        scheme.setScale(Scales.FIT_WIDTH);
        displayScale();
        saveScale(Scales.FIT_WIDTH);
    });

    $("#lblZoomInBtn").click(function (event) {
        scheme.setScale(getNextScale());
        displayScale();
        saveScale();
    });

    $("#lblZoomOutBtn").click(function (event) {
        scheme.setScale(getPrevScale());
        displayScale();
        saveScale();
    });
}

// Get the previous scale value from the possible values array
function getPrevScale() {
    var curScale = scheme.scale;
    for (var i = scaleVals.length - 1; i >= 0; i--) {
        var prevScale = scaleVals[i];
        if (curScale > prevScale)
            return prevScale;
    }
    return curScale;
}

// Get the next scale value from the possible values array
function getNextScale() {
    var curScale = scheme.scale;
    for (var i = 0, len = scaleVals.length; i < len; i++) {
        var nextScale = scaleVals[i];
        if (curScale < nextScale)
            return nextScale;
    }
    return curScale;
}

// Display the scheme scale
function displayScale() {
    $("#spanCurScale").text(Math.round(scheme.scale * 100) + "%");
}

// Load the scheme scale from the local storage
function loadScale() {
    var scale = localStorage.getItem("Scheme.SchemeScale");
    if (scale) {
        scheme.setScale(scale);
    }
}

// Save the scheme scale in the local storage
function saveScale(opt_scale) {
    localStorage.setItem("Scheme.SchemeScale", opt_scale ? opt_scale : scheme.scale);
}

// Update layout of the top level div elements
function updateLayout() {
    var divNotif = $("#divNotif");
    var divSchWrapper = $("#divSchWrapper");
    var divToolbar = $("#divToolbar");
    var notifHeight = divNotif.css("display") == "block" ? divNotif.outerHeight() : 0;
    var windowWidth = $(window).width();

    $("body").css("padding-top", notifHeight);
    divNotif.outerWidth(windowWidth);
    divSchWrapper
        .outerWidth(windowWidth)
        .outerHeight($(window).height() - notifHeight);
    divToolbar.css("top", notifHeight);
}

// Initialize debug tools
function initDebugTools() {
    $("#divDebugTools").css("display", "inline-block");

    $("#spanLoadSchemeBtn").click(function () {
        loadScheme(viewID);
    });

    $("#spanCreateDomBtn").click(function () {
        scheme.createDom();
    });

    $("#spanStartUpdBtn").click(function () {
        startUpdatingScheme();
        $(this).prop("disabled", true);
    });

    $("#spanAddNotifBtn").click(function () {
        notifier.addNotification(scada.utils.getCurTime() + " Test notification",
            scada.NotifTypes.INFO, notifier.DEF_NOTIF_LIFETIME);
    });
}

$(document).ready(function () {
    scada.clientAPI.rootPath = "../../";
    var divSchWrapper = $("#divSchWrapper");
    scheme.parentDomElem = divSchWrapper;
    initToolbar();
    scada.utils.styleIOS(divSchWrapper);
    updateLayout();
    notifier = new scada.Notifier("#divNotif");
    notifier.startClearingNotifications();

    $(window).on("resize " + scada.EventTypes.UPDATE_LAYOUT, function () {
        updateLayout();
    });

    if (DEBUG_MODE) {
        initDebugTools();
    } else {
        loadScheme(viewID);
    }
});