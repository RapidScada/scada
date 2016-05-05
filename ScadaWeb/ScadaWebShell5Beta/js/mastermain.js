var scada = scada || {};
scada.masterMain = scada.masterMain || {};
scada.masterMain.params = scada.masterMain.params || {};

// Left pane that displays main menu or views explorer is expanded
scada.masterMain.leftPaneExpanded = true;

// Update layout of the master page
scada.masterMain.updateLayout = function () {
    var divMainHeader = $("#divMainHeader");
    var divMainLeftPane = $("#divMainLeftPane");
    var divMainTabs = $("#divMainTabs");

    var paneH = $(window).height() - divMainHeader.outerHeight();
    divMainLeftPane.outerHeight(paneH);
    divMainTabs.outerWidth(paneH);
};

// Choose a tool window and activate it
scada.masterMain.chooseToolWindow = function () {
    if (this.params.mainMenuVisible) {
        this.activateToolWindow($("#divMainMenuTab"));
    } else {
        this.activateToolWindow($("#divMainExplorerTab"));
    }
};

// Make visible main menu or views explorer
scada.masterMain.activateToolWindow = function (divClickedTab) {
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
};

// Hide main menu and views explorer
scada.masterMain.hideLeftPane = function () {
    $("#divMainLeftPane").css("display", "none");
    $("body").css("padding-left", "0");
};

// Show main menu and views explorer
scada.masterMain.showLeftPane = function () {
    $("body").css("padding-left", "");
    $("#divMainLeftPane").css("display", "");
};

// Collapse main menu and views explorer
scada.masterMain.collapseLeftPane = function () {
    $("#spanMainShowMenu").css("display", "inline-block");
    this.hideLeftPane();
    this.leftPaneExpanded = false;
};

// Expand main menu and views explorer
scada.masterMain.expandLeftPane = function () {
    this.showLeftPane();
    $("#spanMainShowMenu").css("display", "none");
    this.leftPaneExpanded = true;
};

// Hide page header
scada.masterMain.hideHeader = function () {
    $("#divMainHeader").css("display", "none");
    $("body").css("padding-top", "0");
};

// Show page header
scada.masterMain.showHeader = function () {
    $("#divMainHeader").css("display", "");
    $("body").css("padding-top", "");
};

// Hide all the menus and switch browser to fullscreen mode
scada.masterMain.switchToFullscreen = function () {
    if (this.leftPaneExpanded) {
        this.hideLeftPane();
    }
    this.hideHeader();
    $("#divMainNormalView").css("display", "inline-block");
    scada.utils.requestFullscreen();
};

// Show all the menus and exit browser fullscreen mode
scada.masterMain.switchToNormalView = function () {
    $("#divMainNormalView").css("display", "none");
    if (this.leftPaneExpanded) {
        this.showLeftPane();
    }
    this.showHeader();
    scada.utils.exitFullscreen();
};

$(document).ready(function () {
    scada.masterMain.updateLayout();
    scada.masterMain.chooseToolWindow();
    scada.treeView.prepare();

    // update layout on window resize
    $(window).resize(function () {
        scada.masterMain.updateLayout();
    });

    // activate tool window if tab is clicked
    $("#divMainTabs .tab").click(function () {
        scada.masterMain.activateToolWindow($(this));
    });

    // collapse left pane on button click
    $("#divMainCollapsePane").click(function () {
        scada.masterMain.collapseLeftPane();
    });

    // expand left pane on button click
    $("#spanMainShowMenu").click(function () {
        scada.masterMain.expandLeftPane();
    });

    // switch to full screen mode on button click
    $("#spanMainFullScreen").click(function () {
        scada.masterMain.switchToFullscreen();
    });

    // switch to normal view mode on button click
    $("#divMainNormalView").click(function () {
        scada.masterMain.switchToNormalView();
    });

    // show menus if user exits fullscreen mode
    $(document).on("webkitfullscreenchange mozfullscreenchange fullscreenchange MSFullscreenChange", function () {
        if (!scada.utils.isFullScreen) {
            scada.masterMain.switchToNormalView();
        }
    });
});