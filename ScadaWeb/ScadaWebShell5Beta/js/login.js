// Show alert before login form
function showAlert(message) {
    var divAlert = $('<div class="alert alert-danger alert-dismissible" role="alert">' +
        '<button type="button" class="close" data-dismiss="alert" aria-label="Close">' +
        '<span aria-hidden="true">&times;</span></button>' + message + '</div>');

    divAlert.first().outerWidth($("#divLogin").outerWidth());
    $("#divAlertContainer").append(divAlert);
}