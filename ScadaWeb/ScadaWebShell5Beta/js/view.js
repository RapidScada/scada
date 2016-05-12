var scada = scada || {};

scada.view = {
    // Page title just after loading
    initialPageTitle: "",

    // Load view
    load: function (url) {
        document.title = this.initialPageTitle;
        var frameView = $("#frameView");

        frameView
        .load(function () {
            // set the page title the same as the frame title
            document.title = frameView[0].contentWindow.document.title;
        })
        .attr("src", url);
    }
};

$(document).ready(function () {
    scada.view.initialPageTitle = document.title;
});