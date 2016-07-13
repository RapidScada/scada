$(document).ready(function () {
    // change the form style if it is shown as a popup
    if (window != window.top) {
        $("body").addClass("popup");
        $("#divHeader").css("display", "none");
    }
});