// Scheme changes results enumeration
var GetChangesResults = {
    SUCCESS: 0,
    RELOAD_SCHEME: 1,
    EDITOR_UNKNOWN: 2,
    ERROR: 3
};

// Types of scheme changes enumeration
var SchemeChangeTypes = {
    NONE: 0,
    SCHEME_DOC_CHANGED: 1,
    COMPONENT_ADDED: 2,
    COMPONENT_CHANGED: 3,
    COMPONENT_DELETED: 4
};

// Data exchange rate, ms
var GET_CHANGES_RATE = 300;

// Scheme object
var scheme = new scada.scheme.Scheme(true);
// Stamp of the last processed change
var lastChangeStamp = 0;

// The variables below must be defined in editor.html
// Editor session ID
var editorID = editorID || "";
// WCF-service URL
var serviceUrl = "http://localhost:10001/ScadaSchemeEditor/SchemeEditorSvc/";

// Load the scheme
function loadScheme(editorID) {
    scheme.load(editorID, function (success) {
        if (success) {
            scheme.createDom(true/*controlRight*/);
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
    getChanges(function (result) {
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

// Iteration of getting scheme changes
// callback is a function (result)
function getChanges(callback) {
    var operation = serviceUrl + "GetChanges";

    $.ajax({
        url: operation +
            "?editorID=" + editorID +
            "&viewStamp=" + scheme.viewStamp +
            "&changeStamp=" + lastChangeStamp,
        method: "GET",
        dataType: "json",
        cache: false
    })
    .done(function (data, textStatus, jqXHR) {
        try {
            var parsedData = $.parseJSON(data.d);
            if (parsedData.Success) {
                scada.utils.logSuccessfulRequest(operation);

                if (parsedData.EditorUnknown) {
                    console.error(scada.utils.getCurTime() + " Editor is unknown. Normal operation is impossible.");
                    callback(GetChangesResults.EDITOR_UNKNOWN);
                } else if (scheme.viewStamp && parsedData.ViewStamp) {
                    if (scheme.viewStamp == parsedData.ViewStamp) {
                        processChanges(parsedData.Changes);
                        // TODO: highlight selected objects
                        callback(GetChangesResults.SUCCESS);
                    } else {
                        console.log(scada.utils.getCurTime() + " View stamps are different. Need to reload scheme.");
                        callback(GetChangesResults.RELOAD_SCHEME);
                    }
                } else {
                    console.error(scada.utils.getCurTime() + " View stamp is undefined on client or server side.");
                    callback(GetChangesResults.ERROR);
                }
            } else {
                scada.utils.logServiceError(operation, parsedData.ErrorMessage);
                callback(GetChangesResults.ERROR);
            }
        }
        catch (ex) {
            scada.utils.logProcessingError(operation, ex.message);
            callback(GetChangesResults.ERROR);
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        scada.utils.logFailedRequest(operation, jqXHR);
        callback(GetChangesResults.ERROR);
    });
}

// Apply the received scheme changes
function processChanges(changes) {
    for (var change of changes) {
        switch (change.ChangeType) {
            case SchemeChangeTypes.COMPONENT_CHANGED:
                break;
        }

        lastChangeStamp = change.Stamp;
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

    loadScheme(editorID);
});