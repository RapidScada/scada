// Popup dialogs manipulation object
var popup = scada.popupLocator.getPopup();
// Time before closing, sec
var closeTimeout = 3;

// Start countdown before closing
function startCountdown() {
    var spanCountdown = $("#spanCountdown");

    var countdownFunc = function () {
        if (closeTimeout > 1) {
            closeTimeout--;
            spanCountdown.text(closeTimeout);
            setTimeout(countdownFunc, 1000);
        } else {
            spanCountdown.text("0");
            if (popup) {
                popup.closeModal(window, true);
            }
        }
    }

    if (popup) {
        popup.setModalResult(window, true);
    }

    spanCountdown.text(closeTimeout);
    setTimeout(countdownFunc, 1000);
}

$(document).ready(function () {
    // hide or disable execute button according to the submit button state
    if (popup) {
        var btnSubmit = $("#btnSubmit");
        if (btnSubmit.hasClass("hide-exec-btn")) {
            popup.setButtonVisible(window, scada.ModalButtons.EXEC, false);
        } else if (btnSubmit.is(":disabled")) {
            popup.setButtonEnabled(window, scada.ModalButtons.EXEC, false);
        }
    }

    // submit the form on execute button click
    $(window).on(scada.EventTypes.MODAL_BTN_CLICK, function (event, result) {
        if (result == scada.ModalButtons.EXEC) {
            $("#btnSubmit").click();
        }
    });

    // highlight password error
    if ($("#lblWrongPwd").length > 0) {
        $("#pnlPassword").addClass("has-error");
    }

    // highlight command value error
    if ($("#lblIncorrectCmdVal").length > 0) {
        $("#pnlRealValue").addClass("has-error");
    }

    // highlight command data error
    if ($("#lblIncorrectCmdData").length > 0) {
        $("#pnlData").addClass("has-error");
    }
});