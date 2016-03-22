// Scheme object
var scheme = new scada.scheme.Scheme();

// View ID. Must be defined in Scheme.aspx
var viewID = viewID || 0;

// Start scheme loading process
function startLoadingScheme(viewID) {
    scheme.clear();
    continueLoadingScheme(viewID);
}

// Continue scheme loading process
function continueLoadingScheme(viewID) {
    scheme.load(viewID, function (success, complete) {
        if (success) {
            if (complete) {
                console.info("Loading complete successfully");
            } else {
                setTimeout(continueLoadingScheme, 0, viewID);
            }
        } else {
            console.error("Loading failed. Try again");
        }
    });
}

$(document).ready(function () {
    scada.clientAPI.rootPath = "../../";
    scheme.parentDomElem = $("#divSchemeParent");

    $("#btnLoadScheme").click(function (event) {
        event.preventDefault();
        startLoadingScheme(viewID);
    });

    $("#btnCreateDom").click(function (event) {
        event.preventDefault();
        scheme.createDom();
    });

    $("#btnUpdate").click(function (event) {
        event.preventDefault();
        scheme.update(scada.clientAPI);
    });
});