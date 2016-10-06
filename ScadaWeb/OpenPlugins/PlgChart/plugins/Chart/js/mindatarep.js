// Duration of locking the generate report button, ms
var GEN_BTN_LOCK_DURATION = 3000;

// Select begin or end date using a calendar popup
function selectDate(inputElem, buttonElem) {
    scada.dialogs.showCalendar(buttonElem, inputElem.val(), function (dialogResult, extraParams) {
        if (dialogResult) {
            inputElem.val(extraParams.dateStr);
        }
    });
}

// Show modal dialog to select the channels
function showSelectCnlsModal() {
    popup.showModal("SelectCnls.aspx",
        new scada.ModalOptions([scada.ModalButtons.OK, scada.ModalButtons.CANCEL]),
        function (dialogResult, extraParams) {
            if (dialogResult) {
                // perform adding channels
                if (extraParams.cnlNums && extraParams.viewIDs) {
                    $("#hidAddedCnlNums").val(extraParams.cnlNums);
                    $("#hidAddedViewIDs").val(extraParams.viewIDs);
                    $("#btnApplyAddedCnls").click();
                }
            }
        });
}

// Start report generation
function generateReport(cnlNums, viewIDs, year, month, day, period) {
    lockGenerateButton();
    window.location = "MinDataRepOut.aspx?cnlNums=" + cnlNums + "&viewIDs=" + viewIDs + 
        "&year=" + year + "&month=" + month + "&day=" + day + "&period=" + period;
}

// Lock the generate report button after click
function lockGenerateButton() {
    $("#btnGenReport").prop("disabled", true);
    $("#lblGenStarted").removeClass("hidden");

    setTimeout(function () {
        $("#btnGenReport").prop("disabled", false);
        $("#lblGenStarted").addClass("hidden");
    }, GEN_BTN_LOCK_DURATION)
}

// Initialize Bootstrap popovers
function initPopovers() {
    $('[data-toggle="popover"]').popover({ html: true });
}

// Bind the form events
function bindEvents() {
    // open a calendar popup on a calendar button click
    $("button.calendar")
        .off("click")
        .on("click", function () {
            selectDate($(this).parent().siblings("input"), $(this));
        });

    // open a select channels popup on the appropriate button click
    $("#btnAddCnls")
        .off("click")
        .on("click", function () {
            showSelectCnlsModal();
            return false;
        });
}

// Do actions after asynchronous request
function asyncEndRequest() {
    initPopovers();
    bindEvents();
    scada.utils.scrollTo($(".main-content:first"), $(".alert"));
}

$(document).ready(function () {
    initPopovers();
    bindEvents();
});