// Scheme object
var scheme = new scada.scheme.Scheme();
// View ID. Must be defined in Scheme.aspx
var viewID = viewID || 0;
// Scheme refresh rate
var refrRate = refrRate || 1000;
// Localized phrases
var phrases = phrases || {};
// Possible scale values
var scaleVals = [0.1, 0.25, 0.5, 0.75, 1, 1.25, 1.5, 2, 2.5, 3, 4, 5];

// Default notification message lifetime, ms
var DEF_NOTIF_LIFETIME = 10000;
// Infinite notification message lifetime
var INFINITE_NOTIF_LIFETIME = 0;

// Start scheme loading process
function startLoadingScheme(viewID) {
    console.info(scada.utils.getCurTime() + " Start loading scheme");
    $("body").addClass("loading");
    scheme.clear();
    continueLoadingScheme(viewID);
}

// Continue scheme loading process
function continueLoadingScheme(viewID) {
    var getCurTime = scada.utils.getCurTime;

    scheme.load(viewID, function (success, complete) {
        if (success) {
            if (complete) {
                console.info(getCurTime() + " Scheme loading completed successfully");
                $("body").removeClass("loading");

                if (!DEBUG_MODE) {
                    scheme.createDom();
                    loadScale();
                    displayScale();
                    startUpdatingScheme();
                }
            } else {
                setTimeout(continueLoadingScheme, 0, viewID);
            }
        } else {
            console.error(getCurTime() + " Scheme loading failed");
            $("body").removeClass("loading");
            addNotification(phrases.LoadSchemeError +
                " <input type='button' value='" + phrases.ReloadButton + "' onclick='reloadScheme()' />",
                true, INFINITE_NOTIF_LIFETIME);
        }
    });
}

// Start cyclic scheme updating process
function startUpdatingScheme() {
    scheme.update(scada.clientAPI, function (success) {
        if (!success) {
            addNotification(phrases.UpdateError, true, DEF_NOTIF_LIFETIME);
        }

        setTimeout(startUpdatingScheme, refrRate);
    });
}

// Add notification to the notification panel
function addNotification(messageHtml, error, lifetime) {
    // remove the same previous message
    var divNotif = $("#divNotif");
    var divPrevMessage = divNotif.children(".message:last");

    if (divPrevMessage.html() == messageHtml) {
        divPrevMessage.remove();
    }

    // add the new message
    var divMessage = $("<div class='message'></div>").html(messageHtml);

    if (error) {
        divMessage.addClass("error");
    }

    if (lifetime) {
        divMessage.attr("data-expires", Date.now() + lifetime);
    }

    divNotif
        .css("display", "block")
        .append(divMessage)
        .scrollTop(divNotif.prop("scrollHeight"));

    updateLayout();
}

// Clear the notifications which lifetime is expired
function clearOutdatedNotifications() {
    var messages = $("#divNotif .message");

    if (messages.length > 0) {
        var nowMs = Date.now();

        $.each(messages, function () {
            var expires = $(this).attr("data-expires");
            if (expires < nowMs) {
                $(this).remove();
            }
        });

        if ($("#divNotif .message").length == 0) {
            $("#divNotif").css("display", "none");
        }

        updateLayout();
    }
}

// Clear all the notifications
function clearAllNotifications() {
    $("#divNotif .message").remove();
    updateLayout();
}

// Start outdated notifications clearing process
function startClearingNotifications() {
    setInterval(clearOutdatedNotifications, 1000);
}

// Reload scheme
function reloadScheme() {
    location = location;
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

// Load the scheme scale from the cookies
function loadScale() {
    var scale = scada.utils.getCookie("SchemeScale");
    if (scale) {
        scheme.setScale(scale);
    }
}

// Save the scheme scale in the cookies
function saveScale(opt_scale) {
    scada.utils.setCookie("SchemeScale", opt_scale ? opt_scale : scheme.scale);
}

// Initialize debug tools
function initDebugTools() {
    $("#divDebugTools").css("display", "inline-block");

    $("#spanLoadSchemeBtn").click(function () {
        startLoadingScheme(viewID);
    });

    $("#spanCreateDomBtn").click(function () {
        scheme.createDom();
    });

    $("#spanStartUpdBtn").click(function () {
        startUpdatingScheme();
        $(this).prop("disabled", true);
    });

    $("#spanAddNotifBtn").click(function () {
        addNotification(scada.utils.getCurTime() + " Test notification", false, DEF_NOTIF_LIFETIME);
    });
}

// Update layout of the top level div elements
function updateLayout() {
    var divNotif = $("#divNotif");
    var notifHeight = divNotif.css("display") == "block" ? divNotif.outerHeight() : 0;
    var divSchWrapper = $("#divSchWrapper");
    var divToolbar = $("#divToolbar");

    $("body").css("padding-top", notifHeight);
    divNotif.outerWidth($(window).width());
    divSchWrapper.height($(window).height() - notifHeight);
    divToolbar.css("top", notifHeight);
}


$(document).ready(function () {
    scada.clientAPI.rootPath = "../../";
    scheme.parentDomElem = $("#divSchWrapper");
    initToolbar();

    if (DEBUG_MODE) {
        initDebugTools();
    } else {
        startLoadingScheme(viewID);
    }

    $(window).resize(function () {
        updateLayout();
    });

    updateLayout();
    startClearingNotifications();
});