// Data exchange rate, ms
var GET_CHANGES_RATE = 300;

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
            scheme.createDom(true);
            startGettingChanges();
        } else {
            // TODO: show alert
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
                // TODO: show alert
                continueProcess = false;
                break;
            case GetChangesResults.ERROR:
                // TODO: show alert
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