var scada = scada || {};
var viewHub = new scada.ViewHub(window);

scada.view = {
    // The active data window object
    _dataWindow: {
        // Initial url of the data window
        url: "",
        // Append the current view ID to the query string
        dependsOnView: false,

        // Load the data window considering the current view if required
        load: function (url, dependsOnView) {
            this.url = url;
            this.dependsOnView = dependsOnView;
            this.reload();
        },
        // Reload the data window using the current properties and the current view
        reload: function () {
            if (this.url) {
                var viewID = viewHub.currentViewID;
                var newUrl = this.dependsOnView && viewID > 0 ?
                    scada.utils.setQueryParam("viewID", viewID, this.url) :
                    this.url;
                var frameDataWindow = $("#frameDataWindow");
                frameDataWindow
                .load(function () {
                    viewHub.addDataWindow(frameDataWindow[0].contentWindow);
                })
                .attr("src", newUrl);
            }
        },
        // Clear the data window and release resources
        clear: function () {
            viewHub.removeDataWindow();
            this.url = "";
            this.dependsOnView = false;
            $("#frameDataWindow").attr("src", "");
        }
    },


    // Events data window code
    _EVENTS_WND_CODE: "Events",

    // Page title just after loading
    initialPageTitle: "",


    // Get outer height of the specified object considering its displaying
    _getOuterHeight: function (jqObj) {
        return jqObj.css("display") == "none" ? 0 : jqObj.outerHeight();
    },

    // Load the splitter position from the cookies
    _loadSplitterPosition: function () {
        var dataWindowHeight = parseInt(scada.utils.getCookie("Shell.DataWindowHeight")) ||
            $("#divDataWindow").outerHeight();
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
        var activeDataWindow = scada.utils.getCookie("Shell.ActiveDataWindow");
        var thisView = this;

        if (activeDataWindow) {
            $("#divBottomTabsContainer .tab").each(function () {
                var tabUrl = $(this).data("url");
                if (activeDataWindow == tabUrl) {
                    thisView.activateDataWindow($(this));
                    return false; // break the loop
                }
            });
        } else {
            // activate events window if presented
            $("#divBottomTabsContainer .tab").each(function () {
                var code = $(this).data("code");
                if (code == thisView._EVENTS_WND_CODE) {
                    thisView.activateDataWindow($(this));
                    return false;
                }
            });
        }
    },

    // Save active data window URL in the cookies
    _saveActiveDataWindow: function () {
        scada.utils.setCookie("Shell.ActiveDataWindow", this._dataWindow.url);
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
        $("#divBottomTabsContainer .tab").removeClass("selected");
        $("#divViewSplitter").css("display", "none");
        $("#divDataWindow").css("display", "none");
        $("#divCollapseDataWindowBtn").css("display", "none");

        this._dataWindow.clear();
        this._saveActiveDataWindow();
        this.updateLayout();
    },

    // Load the specified view and reload an active data window
    loadView: function (viewID, viewUrl) {
        // load view
        document.title = this.initialPageTitle;
        viewHub.currentViewID = viewID;
        var frameView = $("#frameView");

        frameView
        .load(function () {
            var wnd = frameView[0].contentWindow;
            // add the view to the view hub
            viewHub.addView(wnd);
            // set the page title the same as the frame title
            document.title = wnd.document.title;
        })
        .attr("src", viewUrl);

        // reload a data window with the new view ID
        if (this._dataWindow.dependsOnView) {
            this._dataWindow.reload();
        }
    },

    // Load page visual state from the cookies
    loadVisualState: function () {
        this._loadSplitterPosition();
        this._loadActiveDataWindow();
    },

    // Save the splitter position in the cookies
    saveSplitterPosition: function () {
        var dataWindowHeight = $("#divDataWindow").outerHeight();
        scada.utils.setCookie("Shell.DataWindowHeight", dataWindowHeight);
    },

    // Reload view and data windows on iOS to fix frame size,
    // because setting frame size using jQuery doesn't work on iOS
    reloadWindowsOnIOS: function () {
        if (scada.utils.iOS()) {
            if (viewHub.viewWindow) {
                viewHub.viewWindow.location.reload();
            }

            if (viewHub.dataWindow) {
                viewHub.dataWindow.location.reload();
            }
        }
    }
};

$(document).ready(function () {
    // the order of the calls below is important
    scada.view.initialPageTitle = document.title;
    scada.view.hideBottomTabsIfEmpty();
    scada.view.loadView(initialViewID, initialViewUrl); // arguments are defined in View.aspx
    scada.view.loadVisualState();
    scada.view.enqueueUpdateLayout();

    // operate the splitter
    scada.splitter.prepare(function (splitterBulk) {
        if (splitterBulk.splitter.attr("id") == "divViewSplitter") {
            scada.view.saveSplitterPosition();
            scada.view.reloadWindowsOnIOS();
        }
    });

    // update layout on window resize and master page layout changes
    $(window).on("resize " + scada.EventTypes.UPDATE_LAYOUT, function () {
        scada.view.updateLayout();
        scada.view.reloadWindowsOnIOS();
    });

    // set window title on view title changed
    $(window).on(scada.EventTypes.VIEW_TITLE_CHANGED, function (event, sender, extraParams) {
        if (sender == viewHub.viewWindow) {
            document.title = extraParams;
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