// Popup dialogs manipulation object
var popup = scada.popupLocator.getPopup();

// Update the modal dialog height according to a frame height
function updateModalHeight() {
    if (popup) {
        setTimeout(popup.updateModalHeight, 0, window);
    }
}

// Close the modal with successful result
function closeModal(cnlNums, viewIDs) {
    if (popup) {
        popup.closeModal(window, true, { cnlNums: cnlNums, viewIDs: viewIDs });
    }
}

$(document).ready(function () {
    // initialize Bootstrap popovers
    $('[data-toggle="popover"]').popover({ html: true });

    // disable OK button according to the submit button state
    if (popup) {
        var enabled = !$("#btnSubmit").is(":disabled");
        popup.setButtonEnabled(window, scada.ModalButtons.OK, enabled);
    }

    // submit the form on OK button click
    $(window).on(scada.EventTypes.MODAL_BTN_CLICK, function (event, result) {
        if (result == scada.ModalButtons.OK) {
            $("#btnSubmit").click();
        }
    });

    // show "loading" message on change a view
    $("#ddlView").change(function () {
        $(".cnl-list").addClass("hidden");
        $(".cnl-list-msg").addClass("hidden");
        $(".cnl-list-msg.loading").removeClass("hidden");
    });
});