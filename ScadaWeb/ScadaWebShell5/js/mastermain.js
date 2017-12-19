var scada = scada || {};
var popup = new scada.Popup();
scada.view = scada.view || {}; // defined if the current page is View.aspx

scada.masterMain = {
    // Check user logged on rate, ms
    CHECK_LOGGEDON_RATE: 10000,
    // Delay before redirecting to login page
    LOGIN_DELAY: 3000,

    // The left pane, which displays tool windows, is expanded
    leftPaneExpanded: true,
    // Fullsreen mode is active
    isFullscreen: false,


    // Hide the left pane
    _hideLeftPane: function () {
        $("#divMainLeftPane").css("display", "none");
        $("body").css("padding-left", "0");
    },

    // Show the left pane
    _showLeftPane: function () {
        $("body").css("padding-left", "");
        $("#divMainLeftPane").css("display", "");
    },

    // Hide the page header
    _hideHeader: function () {
        $("#divMainHeader").css("display", "none");
        $("body").css("padding-top", "0");
    },

    // Show the page header
    _showHeader: function () {
        $("#divMainHeader").css("display", "");
        $("body").css("padding-top", "");
    },

    // Save the left pane visibility in the local storage
    _saveLeftPaneVisible: function () {
        localStorage.setItem("Shell.LeftPaneVisible", this.leftPaneExpanded);
    },

    // Check that a user is logged on
    _checkLoggedOn: function () {
        var thisObj = this;
        scada.clientAPI.checkLoggedOn(function (success, loggedOn) {
            if (loggedOn === false) {
                // redirect to login page
                setTimeout(function () {
                    location.href = scada.env.rootPath + "Login.aspx?return=" + encodeURIComponent(location.href);
                }, thisObj.LOGIN_DELAY);
            } else {
                // loggedOn is true or null
                // enqueue the next check
                setTimeout(function () { thisObj._checkLoggedOn(); }, thisObj.CHECK_LOGGEDON_RATE);
            }
        });
    },

    // Update layout of the master page
    updateLayout: function () {
        var divMainHeader = $("#divMainHeader");
        var divMainLeftPane = $("#divMainLeftPane");
        var divMainTabs = $("#divMainTabs");

        var headerH = divMainHeader.css("display") == "none" ? 0 : divMainHeader.outerHeight();
        var paneH = $(window).height() - headerH;
        divMainLeftPane.outerHeight(paneH);
        divMainTabs.outerWidth(paneH);
        $("#divMainContent").outerHeight(paneH);
        $(window).trigger(scada.EventTypes.UPDATE_LAYOUT);
    },

    // Choose a tool window according to the current URL and activate it
    chooseToolWindow: function () {
        var explorerVisible = scada.env.rootPath + "View.aspx" == window.location.pathname;

        if (explorerVisible) {
            this.activateToolWindow($("#divMainExplorerTab"));
        } else {
            this.activateToolWindow($("#divMainMenuTab"));
        }
    },

    // Make the tool window, that corresponds a clicked tab, visible
    activateToolWindow: function (divClickedTab) {
        // highlight clicked tab
        var tabs = $("#divMainTabs .tab");
        tabs.removeClass("selected");
        divClickedTab.addClass("selected");

        // deactivate all the tool windows
        var toolWindows = $("#divMainLeftPane .tool-window");
        toolWindows.css("display", "none");

        // activate the appropriate tool window
        var clickedTabId = divClickedTab.attr('id');
        var toolWindow;

        if (clickedTabId == "divMainMenuTab") {
            toolWindow = $("#divMainMenu")
        } else if (clickedTabId == "divMainExplorerTab") {
            toolWindow = $("#divMainExplorer")
        } else {
            toolWindow = null;
        }

        if (toolWindow) {
            toolWindow.css("display", "block");
        }
    },

    // Collapse the left pane and show the menu button
    collapseLeftPane: function () {
        $("#spanMainShowMenuBtn").css("display", "inline-block");
        this._hideLeftPane();
        this.leftPaneExpanded = false;
        this._saveLeftPaneVisible();
        $(window).trigger(scada.EventTypes.UPDATE_LAYOUT);
    },

    // Expand the left pane and hide the menu button
    expandLeftPane: function () {
        this._showLeftPane();
        $("#spanMainShowMenuBtn").css("display", "none");
        this.leftPaneExpanded = true;
        this._saveLeftPaneVisible();
        $(window).trigger(scada.EventTypes.UPDATE_LAYOUT);
    },

    // Hide all the menus and switch browser to fullscreen mode
    switchToFullscreen: function () {
        if (!this.isFullscreen) {
            this.isFullscreen = true;

            if (this.leftPaneExpanded) {
                this._hideLeftPane();
            }
            this._hideHeader();
            $("#lblMainNormalViewBtn").css("display", "inline-block");

            scada.utils.requestFullscreen();
            scada.masterMain.updateLayout();
        }
    },

    // Show all the menus and exit browser fullscreen mode
    switchToNormalView: function () {
        if (this.isFullscreen) {
            this.isFullscreen = false;

            $("#lblMainNormalViewBtn").css("display", "none");
            this._showHeader();
            if (this.leftPaneExpanded) {
                this._showLeftPane();
            }

            scada.utils.exitFullscreen();
            scada.masterMain.updateLayout();
        }
    },

    // Load page visual state from the local storage
    loadVisualState: function () {
        var leftPaneVisible = localStorage.getItem("Shell.LeftPaneVisible");
        if (leftPaneVisible == "false" || leftPaneVisible == null && scada.utils.isSmallScreen()) {
            this.collapseLeftPane();
        }
    },

    // Load view without reloading the whole page if possible
    loadView: function (viewID, viewUrl) {
        if (scada.view.loadView) {
            scada.view.loadView(viewID, viewUrl);
        } else {
            location.href = scada.env.rootPath + "View.aspx?viewID=" + viewID;
        }
    },

    // Start cyclic checking user logged on
    startCheckingLoggedOn: function () {
        var thisObj = this;
        setTimeout(function () { thisObj._checkLoggedOn(); }, this.CHECK_LOGGEDON_RATE);
    }
};

$(document).ready(function () {
    // unbind events to avoid doubling in case of using ASP.NET AJAX
    /*$(window).off();
    $(document).off();
    $("body").off();*/

    // page setup
    scada.clientAPI.rootPath = scada.env.rootPath;
    scada.dialogs.rootPath = scada.env.rootPath;
    scada.masterMain.updateLayout();
    scada.masterMain.chooseToolWindow();
    scada.masterMain.loadVisualState();
    scada.masterMain.startCheckingLoggedOn();
    scada.treeView.prepare();

    // update layout on window resize
    $(window).resize(function () {
        scada.masterMain.updateLayout();
    });

    // activate a tool window if the tab is clicked
    $("#divMainTabs .tab")
    .off()
    .click(function () {
        scada.masterMain.activateToolWindow($(this));
    });

    // collapse the left pane on the button click
    $("#divMainCollapseLeftPaneBtn")
    .off()
    .click(function () {
        scada.masterMain.collapseLeftPane();
    });

    // expand the left pane on the button click
    $("#spanMainShowMenuBtn")
    .off()
    .click(function () {
        scada.masterMain.expandLeftPane();
    });

    // switch to full screen mode on the button click
    $("#lblMainFullscreenBtn")
    .off()
    .click(function () {
        scada.masterMain.switchToFullscreen();
    });

    // switch to normal view mode on the button click
    $("#lblMainNormalViewBtn")
    .off()
    .click(function () {
        scada.masterMain.switchToNormalView();
    });

    // show menus if user exits fullscreen mode,
    // in Chrome fires only if the mode is switched programmatically 
    $(document).on("webkitfullscreenchange mozfullscreenchange fullscreenchange MSFullscreenChange", function () {
        if (!scada.utils.isFullscreen()) {
            scada.masterMain.switchToNormalView();
        }
    });
});