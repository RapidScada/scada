// Localized phrases. Must be defined in Login.aspx
var phrases = phrases || {};

// Show alert before login form
function showAlert(message) {
    var divAlert = $('<div class="alert alert-danger alert-dismissible">' +
        '<button type="button" class="close" data-dismiss="alert">' +
        '<span>&times;</span></button>' + message + '</div>');

    divAlert.first().outerWidth($("#divLogin").outerWidth());
    $("#divAlertsInner").append(divAlert);
}

// Check that the browser is supported
function checkBrowserSupport() {
    if (!checkBrowser()) {
        showAlert(phrases.BrowserOutdated);
    } else if (!navigator.cookieEnabled) {
        showAlert(phrases.CookiesDisabled);
    }
}