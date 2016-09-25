// Close the modal with successful result
function closeModal(cnlNums, viewIDs) {
    var popup = scada.popupLocator.getPopup();
    if (popup) {
        popup.closeModal(window, true, { cnlNums: cnlNums, viewIDs: viewIDs });
    }
}

$(document).ready(function () {
    // initialize Bootstrap popovers
    $('[data-toggle="popover"]').popover({ html: true });

    // submit the form on OK button click
    $(window).on(scada.EventTypes.MODAL_BTN_CLICK, function (event, result) {
        if (result == scada.ModalButtons.OK) {
            $("#btnSubmit").click();
        }
    });

    // show "loading" message on change a view
    /*$("#ddlView").change(function () {
        $("#pnlCnlsByView").addClass("hidden");
        $(".list-msg").addClass("hidden");
        $("#lblLoading").removeClass("hidden");
    });*/
});