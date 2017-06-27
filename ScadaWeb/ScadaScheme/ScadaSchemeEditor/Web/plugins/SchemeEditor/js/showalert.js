// Note: Needed to create a separate file showalert.js because editor.js is not supported by outdated browsers

// Timeout to dismiss alert, ms
var ALERT_DISMISS_TIMEOUT = 10000;

// The ID of alert dismissing timer
var alertTimeoutID = 0;

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
