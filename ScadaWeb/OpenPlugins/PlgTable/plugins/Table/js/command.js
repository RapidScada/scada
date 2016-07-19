// Popup dialogs manipulation object
var popup = scada.popupLocator.getPopup();
// Time before closing, sec
var closeTimeout = 3;

// Start countdown before closing
function startCountdown() {
    var spanCountdown = $("#spanCountdown");

    var countdownFunc = function () {
        if (closeTimeout) {
            closeTimeout--;
            spanCountdown.text(closeTimeout);
            setTimeout(countdownFunc, 1000);
        } else {
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
    // hide or disable execute button if it is specified by the server code
    if (popup) {
        if ($("#hidHideExecuteBtn").val() == "true") {
            popup.setButtonVisible(window, scada.ModalButtons.EXEC, false);
        } else if ($("#hidDisableExecuteBtn").val() == "true") {
            popup.setButtonEnabled(window, scada.ModalButtons.EXEC, false);
        }
    }

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

    // submit the form on execute button click
    $(window).on(scada.EventTypes.MODAL_BTN_CLICK, function (event, result) {
        if (result == scada.ModalButtons.EXEC) {
            $("#btnSubmit").click();
        }
    });
});