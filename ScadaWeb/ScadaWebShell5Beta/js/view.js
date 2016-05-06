var scada = scada || {};

scada.view = {
    // Page title just after loading
    initialPageTitle: "",

    // Load view
    load: function (url) {
        document.title = this.initialPageTitle;
        $("#frameView").attr("src", url);
    },

    // Set page title
    setPageTitle: function (viewTitle) {
        document.title = viewTitle + " - Rapid SCADA";
    }
};

$(document).ready(function () {
    scada.view.initialPageTitle = document.title;
});