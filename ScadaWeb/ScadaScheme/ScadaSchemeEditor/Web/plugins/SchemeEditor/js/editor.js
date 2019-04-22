// Data exchange rate, ms
var GET_CHANGES_RATE = 300;

// Ajax queue
var ajaxQueue = new scada.AjaxQueue();
// Scheme object
var scheme = new scada.scheme.EditableScheme();

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
            // show errors
            if (Array.isArray(scheme.loadErrors) && scheme.loadErrors.length > 0) {
                showAlert(scheme.loadErrors.join("<br/>"));
            }

            // show scheme
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
