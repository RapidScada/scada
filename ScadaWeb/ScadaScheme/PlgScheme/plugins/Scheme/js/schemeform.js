// Rapid SCADA namespace
var scada = scada || {};
// Scheme namespace
scada.scheme = scada.scheme || {};

// Scheme object
var scheme = null;
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
// Scheme options
var schemeOptions = schemeOptions || scada.scheme.defaultOptions;

// Scheme environment object accessible by the scheme and its components
scada.scheme.env = {
    // Localized phrases
    phrases: phrases,
    // The view hub object
    viewHub: scada.viewHubLocator ? scada.viewHubLocator.getViewHub() : null,

    // Send telecommand
    sendCommand: function (ctrlCnlNum, cmdVal, viewID, componentID) {
        scheme.sendCommand(ctrlCnlNum, cmdVal, viewID, componentID, function (success) {
            if (!success) {
                notifier.addNotification(phrases.UnableSendCommand, scada.NotifTypes.ERROR, notifier.DEF_NOTIF_LIFETIME);
            }
        });
    }
};

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
    var ScaleTypes = scada.scheme.ScaleTypes;

    $("#lblFitScreenBtn").click(function () {
        scheme.setScale(ScaleTypes.FIT_SCREEN, 0);
        displayScale();
        saveScale();
    });

    $("#lblFitWidthBtn").click(function () {
        scheme.setScale(ScaleTypes.FIT_WIDTH, 0);
        displayScale();
        saveScale();
    });

    $("#lblZoomInBtn").click(function (event) {
        scheme.setScale(ScaleTypes.NUMERIC, getNextScale());
        displayScale();
        saveScale();
    });

    $("#lblZoomOutBtn").click(function (event) {
        scheme.setScale(ScaleTypes.NUMERIC, getPrevScale());
        displayScale();
        saveScale();
    });
}

// Get the previous scale value from the possible values array
function getPrevScale() {
    var curScale = scheme.scaleValue;
    for (var i = scaleVals.length - 1; i >= 0; i--) {
        var prevScale = scaleVals[i];
        if (curScale > prevScale) {
            return prevScale;
        }
    }
    return curScale;
}

// Get the next scale value from the possible values array
function getNextScale() {
    var curScale = scheme.scaleValue;
    for (var i = 0, len = scaleVals.length; i < len; i++) {
        var nextScale = scaleVals[i];
        if (curScale < nextScale) {
            return nextScale;
        }
    }
    return curScale;
}

// Display the scheme scale
function displayScale() {
    $("#spanCurScale").text(Math.round(scheme.scaleValue * 100) + "%");
}

// Load the scheme scale from the local storage
function loadScale() {
    var scaleType = NaN;
    var scaleValue = NaN;

    if (schemeOptions.rememberScale) {
        scaleType = parseInt(localStorage.getItem("Scheme.ScaleType"));
        scaleValue = parseFloat(localStorage.getItem("Scheme.ScaleValue"));
    }

    if (isNaN(scaleType) || isNaN(scaleValue)) {
        scheme.setScale(schemeOptions.scaleType, schemeOptions.scaleValue);
    } else {
        scheme.setScale(scaleType, scaleValue);
    }
}

// Save the scheme scale in the local storage
function saveScale() {
    localStorage.setItem("Scheme.ScaleType", scheme.scaleType);
    localStorage.setItem("Scheme.ScaleValue", scheme.scaleValue);
}

// Update the scheme scale if the scheme should fit size
function updateScale() {
    if (scheme.scaleType !== scada.scheme.ScaleTypes.NUMERIC) {
        scheme.setScale(scheme.scaleType, scheme.scaleValue);
        displayScale();
    }
}

// Update layout of the top level div elements
function updateLayout() {
    var divNotif = $("#divNotif");
    var divSchWrapper = $("#divSchWrapper");
    var divToolbar = $("#divToolbar");
    var notifHeight = divNotif.css("display") === "block" ? divNotif.outerHeight() : 0;
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
    // setup client API
    scada.clientAPI.rootPath = "../../";
    scada.clientAPI.ajaxQueue = scada.ajaxQueueLocator.getAjaxQueue();

    // create scheme
    var divSchWrapper = $("#divSchWrapper");
    scheme = new scada.scheme.Scheme();
    scheme.schemeEnv = scada.scheme.env;
    scheme.parentDomElem = divSchWrapper;

    // setup user interface
    initToolbar();
    scada.utils.styleIOS(divSchWrapper);
    updateLayout();
    notifier = new scada.Notifier("#divNotif");
    notifier.startClearingNotifications();

    $(window).on("resize " + scada.EventTypes.UPDATE_LAYOUT, function () {
        updateLayout();
        updateScale();
    });

    if (DEBUG_MODE) {
        initDebugTools();
    } else {
        loadScheme(viewID);
    }
});