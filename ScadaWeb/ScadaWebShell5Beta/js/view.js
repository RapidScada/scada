var scada = scada || {};

scada.view = {
    // Page title just after loading
    initialPageTitle: "",

    // Minimum height of a view or data window
    minWindowHeight: 100,


    // Get outer height of the specified object considering its displaying
    _getOuterHeight: function (jqObj) {
        return jqObj.css("display") == "none" ? 0 : jqObj.outerHeight();
    },

    // Hide bottom pane if no data windows exist
    hideBottomTabsIfEmpty: function () {
        if ($("#divBottomTabsContainer .tab").length == 0) {
            $("#divBottomTabs").css("display", "none");
        }
    },

    // Update layout of the master page
    updateLayout: function () {
        var divView = $("#divView");
        var divViewSplitter = $("#divViewSplitter");
        var divDataWindow = $("#divDataWindow");
        var divBottomTabs = $("#divBottomTabs");

        var totalH = divView.innerHeight();
        var splitterH = this._getOuterHeight(divViewSplitter);
        var dataWindowH = this._getOuterHeight(divDataWindow);
        var bottomTabsH = this._getOuterHeight(divBottomTabs);
        $("#frameView").outerHeight(totalH - splitterH - dataWindowH - bottomTabsH);
    },

    // Append updateLayout() method in the message queue.
    // Allows to avoid Chrome bug when divBottomTabs height is calculated wrong
    enqueueUpdateLayout: function () {
        var thisView = this;
        setTimeout(function () { thisView.updateLayout(); }, 0);
    },

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
    },

    // Make the data window, that corresponds a clicked tab, visible
    activateDataWindow: function (divClickedTab) {
        $("#frameDataWindow").attr("src", divClickedTab.attr("data-url"));
        $("#divBottomTabsContainer .tab").removeClass("selected");
        divClickedTab.addClass("selected");

        $("#divViewSplitter").css("display", "block");
        $("#divDataWindow").css("display", "block");
        $("#divCollapseDataWindowBtn").css("display", "inline-block");
        scada.view.updateLayout();
    },

    // Collapse a data window and release resources
    collapseDataWindow: function () {
        $("#frameDataWindow").attr("src", "");
        $("#divBottomTabsContainer .tab").removeClass("selected");

        $("#divViewSplitter").css("display", "none");
        $("#divDataWindow").css("display", "none");
        $("#divCollapseDataWindowBtn").css("display", "none");
        scada.view.updateLayout();
    }
};

$(document).ready(function () {
    scada.view.initialPageTitle = document.title;
    scada.view.hideBottomTabsIfEmpty();
    scada.view.enqueueUpdateLayout();

    // update layout on window resize and master page layout changes
    $(window)
    .resize(function () {
        scada.view.updateLayout();
    })
    .on("scada:updateLayout", function () {
        scada.view.updateLayout();
    });

    // activate a data window if the tab is clicked
    $("#divBottomTabsContainer .tab").click(function () {
        scada.view.activateDataWindow($(this));
    });

    // collapse a data window on the button click
    $("#divCollapseDataWindowBtn").click(function () {
        scada.view.collapseDataWindow();
    });
});