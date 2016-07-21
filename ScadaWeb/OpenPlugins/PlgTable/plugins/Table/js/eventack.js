// Popup dialogs manipulation object
var popup = scada.popupLocator.getPopup();

// Close the modal with successful result
function closeModal() {
    if (popup) {
        popup.closeModal(window, true);
    }
}

$(document).ready(function () {
    // hide or disable OK button according to the submit button state
    if (popup) {
        var btnSubmit = $("#btnSubmit");
        if (btnSubmit.length == 0) {
            popup.setButtonVisible(window, scada.ModalButtons.OK, false);
        } else if (btnSubmit.is(":disabled")) {
            popup.setButtonEnabled(window, scada.ModalButtons.OK, false);
        }
    }

    // submit the form on OK button click
    $(window).on(scada.EventTypes.MODAL_BTN_CLICK, function (event, result) {
        if (result == scada.ModalButtons.OK) {
            $("#btnSubmit").click();
        }
    });
});