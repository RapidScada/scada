// Scheme object
var scheme = new scada.scheme.Scheme();

// View ID. Must be defined in Scheme.aspx
var viewID = viewID || 0;

// Start scheme loading process
function startLoadingScheme(viewID) {
    console.info(scada.utils.getCurTime() + " Start loading scheme");
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
            } else {
                setTimeout(continueLoadingScheme, 0, viewID);
            }
        } else {
            console.error(getCurTime() + " Scheme loading failed");
        }
    });
}

// Start cyclic scheme updating process
function startUpdatingScheme() {
    scheme.update(scada.clientAPI, function (success) {
        setTimeout(startUpdatingScheme, 1000);
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
        startUpdatingScheme();
    });
});