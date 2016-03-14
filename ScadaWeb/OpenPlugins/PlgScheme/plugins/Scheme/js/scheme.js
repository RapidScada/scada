// Scheme object
var scheme = new scada.scheme.Scheme();

// Start scheme loading process
function startLoadingScheme(viewID) {
    scheme.load(viewID, function (success, complete) {
        
    });
}

$(document).ready(function () {
    scada.clientAPI.rootPath = "../../";
    scheme.dom = $("#divScheme");

    $("#btnLoadScheme").click(function (event) {
        event.preventDefault();
        startLoadingScheme(viewID); // viewID is defined in Scheme.aspx
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