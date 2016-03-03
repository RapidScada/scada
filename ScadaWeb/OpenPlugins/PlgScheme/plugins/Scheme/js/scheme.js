// Start scheme loading process
function startLoadingScheme(viewID) {
    alert(viewID);
}

$(document).ready(function () {
    $("#btnLoadScheme").click(function (event) {
        event.preventDefault();
        startLoadingScheme(viewID); // viewID is defined in Scheme.aspx
    });
});