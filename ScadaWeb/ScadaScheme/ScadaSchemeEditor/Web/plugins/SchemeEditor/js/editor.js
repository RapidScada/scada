// Data exchange rate, ms
var GET_CHANGES_RATE = 300;
// Timeout to dismiss alert, ms
var ALERT_DISMISS_TIMEOUT = 10000;

// Scheme object
var scheme = new scada.scheme.EditableScheme();
// The ID of alert dismissing timer
var alertTimeoutID = 0;

// The variables below must be defined in editor.html
// Editor session ID
var editorID = editorID || "";
// Localized phrases
var phrases = phrases || {};
// WCF-service URL
var serviceUrl = serviceUrl || "http://localhost:10001/ScadaSchemeEditor/SchemeEditorSvc/";

// Load the scheme
function loadScheme(editorID) {
    scheme.load(editorID, function (success) {
        if (success) {
            scheme.createDom(true);
            startGettingChanges();
        } else {
            showAlert(phrases.UnableLoadScheme);
        }
    });
}

// Reload the scheme
function reloadScheme() {
    location.reload(true);
}

// Start cyclic process of getting scheme changes
function startGettingChanges() {
    var GetChangesResults = scada.scheme.GetChangesResults;

    scheme.getChanges(function (result) {
        var continueProcess = true;

        switch (result) {
            case GetChangesResults.RELOAD_SCHEME:
                reloadScheme();
                continueProcess = false;
                break;
            case GetChangesResults.EDITOR_UNKNOWN:
                showAlert(phrases.PageNotValid);
                continueProcess = false;
                break;
            case GetChangesResults.DATA_ERROR:
                showAlert(phrases.DataError, true);
                break;
            case GetChangesResults.COMM_ERROR:
                showAlert(phrases.EditorClosed, true);
                break;
        }

        if (continueProcess) {
            setTimeout(startGettingChanges, GET_CHANGES_RATE);
        }
    });
}

// Show alert message
function showAlert(message, opt_auto_dismiss) {
    // prevent dismissing alert
    if (alertTimeoutID) {
        clearTimeout(alertTimeoutID);
        alertTimeoutID = 0;
    }

    // create or replace alert message
    var divAlert = $("#divAlertWrapper div.alert");

    if (divAlert.length > 0) {
        divAlert.text(message);
    } else {
        $("<div class='alert'></div>").text(message).appendTo("#divAlertWrapper");
    }

    // start alert dismissing timer
    if (opt_auto_dismiss) {
        alertTimeoutID = setTimeout(function () { divAlert.remove(); }, ALERT_DISMISS_TIMEOUT);
    }
}

// Update layout of the top level div elements
function updateLayout() {
    $("#divSchWrapper")
        .outerWidth($(window).width())
        .outerHeight($(window).height());
}

$(document).ready(function () {
    var divSchWrapper = $("#divSchWrapper");
    scheme.parentDomElem = divSchWrapper;
    scheme.serviceUrl = serviceUrl;
    updateLayout();

    $(window).on("resize", function () {
        updateLayout();
    });

    $(document).on("keydown", function (event) {
        return scheme.processKey(event.key, event.which, event.ctrlKey);
    });

    loadScheme(editorID);
});