// Scheme object
var scheme = new scada.scheme.Scheme(true);

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
        } else {
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

    loadScheme(editorID);
});