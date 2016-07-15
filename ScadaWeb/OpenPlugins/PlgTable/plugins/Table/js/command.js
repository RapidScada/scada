// Popup dialogs manipulation object
var popup = scada.popupLocator.getPopup();
// Time before closing, sec
var closeTimeout = 3;


// Start downcount before closing
function startDowncount() {
    var spanDowncount = $("#spanDowncount");

    var downcountFunc = function () {
        if (closeTimeout) {
            closeTimeout--;
            spanDowncount.text(closeTimeout);
            setTimeout(downcountFunc, 1000);
        } else {
            if (popup) {
                popup.closeModal(window, true);
            }
        }
    }

    spanDowncount.text(closeTimeout);
    setTimeout(downcountFunc, 1000);
}

$(document).ready(function () {
    // hide execute button for discrete command
    if ($("#pnlDiscreteValue").length > 0) {
        if (popup) {
            popup.setButtonVisible(window, scada.ModalButtons.EXEC, false);
        }
    }

    // submit the form on modal execute button click
    $(window).on(scada.EventTypes.MODAL_BTN_CLICK, function (event, result) {
        if (result == scada.ModalButtons.EXEC) {
            $("#btnSubmit").click();
        }
    });
});