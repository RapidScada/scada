// Popup dialogs manipulation object
var popup = scada.popupLocator.getPopup();

// Update the modal dialog height
function updateModalHeight() {
    if (popup) {
        setTimeout(popup.updateModalHeight, 0, window);
    }
}

// Set the modal dialog title
function setModalTitle(title) {
    if (popup) {
        popup.setModalTitle(window, title);
    }
}

// Close the modal dialog
function closeModal(dialogResult, opt_extraParams) {
    if (popup) {
        popup.closeModal(window, dialogResult, opt_extraParams);
    }
}

$(document).ready(function () {
    // disable OK button according to the submit button state
    if (popup) {
        var enabled = !$("input[type='submit']:first").is(":disabled");
        popup.setButtonEnabled(window, scada.ModalButtons.OK, enabled);
    }

    // submit the form on OK button click
    $(window).on(scada.EventTypes.MODAL_BTN_CLICK, function (event, result) {
        if (result == scada.ModalButtons.OK) {
            $("input[type='submit']:first").click();
        }
    });
});
