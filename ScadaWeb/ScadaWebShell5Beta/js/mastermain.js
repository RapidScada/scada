// Update layout of the master page
function updateMainLayout() {
    var divMainHeader = $("#divMainHeader");
    var divMainLeftPane = $("#divMainLeftPane");
    var divMainTabs = $("#divMainTabs");

    var paneH = $(window).height() - divMainHeader.outerHeight();
    divMainLeftPane.outerHeight(paneH);
    divMainTabs.outerWidth(paneH);
}

$(document).ready(function () {
    updateMainLayout();

    $(window).resize(function () {
        updateMainLayout();
    });
});