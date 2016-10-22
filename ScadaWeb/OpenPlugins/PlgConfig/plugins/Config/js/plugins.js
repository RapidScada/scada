$(document).ready(function () {
    // bind action confirmations
    $(".btn-confirm").click(function () {
        var linkBtn = $(this);

        scada.dialogs.showConfirm(function (dialogResult) {
            if (dialogResult) {
                scada.utils.clickLink(linkBtn); // click handler doesn't fire
            }
        });

        return false;
    });
});