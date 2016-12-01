$(document).ready(function () {
    var divTest = $("#divTest");

    for (var i = 0; i < 100; i++) {
        divTest.html(divTest.html() + "<br/>Test " + i);
    }

    $(window).resize(function () {
        $("#divH").text($(window).height());
    });
});