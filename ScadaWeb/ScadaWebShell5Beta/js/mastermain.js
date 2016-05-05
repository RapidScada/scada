// Update layout of the master page
function updateMainLayout() {
    var divMainHeader = $("#divMainHeader");
    var divMainLeftPane = $("#divMainLeftPane");
    var divMainTabs = $("#divMainTabs");

    var paneH = $(window).height() - divMainHeader.outerHeight();
    divMainLeftPane.outerHeight(paneH);
    divMainTabs.outerWidth(paneH);
}

// Choose a tool window and activate it
function chooseToolWindow() {
    if (mainMenuVisible) {
        activateToolWindow($("#divMainMenuTab"));
    } else {
        activateToolWindow($("#divMainExplorerTab"));
    }
}

// Make visible main menu or views explorer
function activateToolWindow(divClickedTab) {
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
}

$(document).ready(function () {
    updateMainLayout();
    chooseToolWindow();
    scada.treeView.prepare();

    // update layout on window resize
    $(window).resize(function () {
        updateMainLayout();
    });

    // activate tool window on tab click
    $("#divMainTabs .tab").click(function (event) {
        activateToolWindow($(this));
    });
});