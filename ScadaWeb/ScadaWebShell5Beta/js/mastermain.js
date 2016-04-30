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

// Expand or collapse the menu item
function toggleMenuItem(divExpander) {
    var menuItem = divExpander.parent();
    var menuSubitems = menuItem.nextUntil(".menu-item", ".menu-subitem");

    if (divExpander.hasClass("expanded")) {
        divExpander.removeClass("expanded");
        menuSubitems.css("display", "none");
    } else {
        divExpander.addClass("expanded");
        menuSubitems.css("display", "block");
    }
}

// Expand selected menu item
function expandSelectedMenuItem() {
    var divSelMenuItem = $("#divMainMenu .menu-item.selected"); // top level item

    if (divSelMenuItem.length == 0) {
        var divSelMenuSubitem = $("#divMainMenu .menu-subitem.selected");
        divSelMenuItem = divSelMenuSubitem.prevAll(".menu-item:first");
    }

    var divExpander = divSelMenuItem.find(".expander");
    if (!divExpander.hasClass("expanded")) {
        toggleMenuItem(divExpander);
    }
}

$(document).ready(function () {
    updateMainLayout();
    chooseToolWindow();
    expandSelectedMenuItem();
    scada.treeView.prepare();

    // update layout on window resize
    $(window).resize(function () {
        updateMainLayout();
    });

    // activate tool window on tab click
    $("#divMainTabs .tab").click(function (event) {
        activateToolWindow($(this));
    });

    // toggle main menu items on click
    $("#divMainMenu .expander").click(function() {
        toggleMenuItem($(this));
    });

    $("#divMainMenu .menu-item.with-expander a").click(function (event) {
        if (!$(this).attr("href")) {
            event.preventDefault();
            toggleMenuItem($(this).siblings(".expander"));
        }
    });
});