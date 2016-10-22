$(document).ready(function () {
    // bind action confirmations
    $(".btn-confirm").click(function () {
        scada.dialogs.showConfirm(function (dialogResult) {
            alert(dialogResult);
        });
    });
});