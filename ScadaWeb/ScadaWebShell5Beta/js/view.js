var scada = scada || {};

scada.view = {
    // Initial url of the active data window
    _activeDataWindowUrl: "",

    // Page title just after loading
    initialPageTitle: "",

    // Current view ID
    viewID: 0,


    // Get outer height of the specified object considering its displaying
    _getOuterHeight: function (jqObj) {
        return jqObj.css("display") == "none" ? 0 : jqObj.outerHeight();
    },

    // Load data window with view ID query parameter
    _loadDataWindow(initialUrl) {
        this._activeDataWindowUrl = initialUrl;

        if (initialUrl) {
            var url = this.viewID > 0 ? initialUrl.indexOf("?") >= 0 ?
                initialUrl + "&viewID=" + this.viewID :
                initialUrl + "?viewID=" + this.viewID :
                initialUrl;
            $("#frameDataWindow").attr("src", url);
        } else {
            $("#frameDataWindow").attr("src", "");
        }
    },

    // Load the splitter position from cookies
    _loadSplitterPosition: function () {
        var dataWindowHeight = scada.utils.getCookie("DataWindowHeight") || $("#divDataWindow").outerHeight();
        var minHeight = parseInt($("#divDataWindow").css("min-height"), 10);
        var maxHeight = $("#divViewContent").innerHeight() -
            parseInt($("#divView").css("min-height"), 10) -
            $("#divViewSplitter").outerHeight() -
            $("#divBottomTabs").outerHeight();

        if (dataWindowHeight > maxHeight) {
            dataWindowHeight = maxHeight;
        }

        if (dataWindowHeight < minHeight) {
            dataWindowHeight = minHeight;
        }

        $("#divDataWindow").outerHeight(dataWindowHeight);
    },

    // Load active data window URL from the cookies
    _loadActiveDataWindow: function () {
        var activeDataWindow = scada.utils.getCookie("ActiveDataWindow");
        var thisView = this;

        if (activeDataWindow) {
            $("#divBottomTabsContainer .tab").each(function () {
                var tabUrl = $(this).attr("data-url");
                if (activeDataWindow == tabUrl) {
                    thisView.activateDataWindow($(this));
                    return false; // break the loop
                }
            });
        }
    },

    // Save active data window URL in the cookies
    _saveActiveDataWindow: function () {
        scada.utils.setCookie("ActiveDataWindow", this._activeDataWindowUrl);
    },

    // Hide bottom pane if no data windows exist
    hideBottomTabsIfEmpty: function () {
        if ($("#divBottomTabsContainer .tab").length == 0) {
            $("#divBottomTabs").css("display", "none");
        }
    },

    // Update layout of the master page
    updateLayout: function () {
        var divViewContent = $("#divViewContent");
        var divViewSplitter = $("#divViewSplitter");
        var divDataWindow = $("#divDataWindow");
        var divBottomTabs = $("#divBottomTabs");

        var totalH = divViewContent.innerHeight();
        var splitterH = this._getOuterHeight(divViewSplitter);
        var dataWindowH = this._getOuterHeight(divDataWindow);
        var bottomTabsH = this._getOuterHeight(divBottomTabs);
        $("#divView").outerHeight(totalH - splitterH - dataWindowH - bottomTabsH);
    },

    // Append updateLayout() method in the message queue.
    // Allows to avoid Chrome bug when divBottomTabs height is calculated wrong
    enqueueUpdateLayout: function () {
        var thisView = this;
        setTimeout(function () { thisView.updateLayout(); }, 0);
    },

    // Make the data window, that corresponds a clicked tab, visible
    activateDataWindow: function (divClickedTab) {
        this._loadDataWindow(divClickedTab.attr("data-url"));
        $("#divBottomTabsContainer .tab").removeClass("selected");
        divClickedTab.addClass("selected");

        $("#divViewSplitter").css("display", "block");
        $("#divDataWindow").css("display", "block");
        $("#divCollapseDataWindowBtn").css("display", "inline-block");

        this._saveActiveDataWindow();
        this.updateLayout();
    },

    // Collapse a data window and release resources
    collapseDataWindow: function () {
        this._loadDataWindow("");
        $("#divBottomTabsContainer .tab").removeClass("selected");

        $("#divViewSplitter").css("display", "none");
        $("#divDataWindow").css("display", "none");
        $("#divCollapseDataWindowBtn").css("display", "none");

        this._saveActiveDataWindow();
        this.updateLayout();
    },

    // Load the specified view and reload an active data window
    loadView: function (viewID, viewUrl) {
        // load view
        document.title = this.initialPageTitle;
        this.viewID = viewID;
        var frameView = $("#frameView");

        frameView
        .load(function () {
            // set the page title the same as the frame title
            document.title = frameView[0].contentWindow.document.title;
        })
        .attr("src", viewUrl);

        // reload a data window with the new view ID
        this._loadDataWindow(this._activeDataWindowUrl);
    },

    // Load page visual state from the cookies
    loadVisualState: function () {
        this._loadSplitterPosition();
        this._loadActiveDataWindow();
    },

    // Save the splitter position in the cookies
    saveSplitterPosition: function () {
        var dataWindowHeight = $("#divDataWindow").outerHeight();
        scada.utils.setCookie("DataWindowHeight", dataWindowHeight);
    }
};

$(document).ready(function () {
    // the order of the calls below is important
    scada.view.initialPageTitle = document.title;
    scada.view.hideBottomTabsIfEmpty();
    scada.view.loadView(initialViewID, initialViewUrl); // arguments are defined in View.aspx
    scada.view.loadVisualState();
    scada.view.enqueueUpdateLayout();

    // update layout on window resize and master page layout changes
    $(window)
    .resize(function () {
        scada.view.updateLayout();
    })
    .on(scada.events.updateLayout, function () {
        scada.view.updateLayout();
    });

    // operate the splitter
    scada.splitter.prepare(function (splitterBulk) {
        if (splitterBulk.splitter.attr("id") == "divViewSplitter") {
            scada.view.saveSplitterPosition();
        }
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