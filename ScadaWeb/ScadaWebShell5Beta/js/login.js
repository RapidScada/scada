// Show alert before login form
function showAlert(message) {
    var divAlert = $('<div class="alert alert-danger alert-dismissible" role="alert">' +
        '<button type="button" class="close" data-dismiss="alert" aria-label="Close">' +
        '<span aria-hidden="true">&times;</span></button>' + message + '</div>');

    divAlert.first().outerWidth($("#divLogin").outerWidth());
    $("#divAlertsInner").append(divAlert);
}

// Check that the browser is supported
function checkBrowser() {
    try {
        // supports for...of
        eval("var arr = []; for (var x of arr) {}");
        // supports Map object
        eval("var map = new Map(); map.set(1, 1); map.get(1); ");
    }
    catch (ex) {
        showAlert($("#hidBrowserOutdatedMsg").val());
    }
}