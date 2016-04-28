// Update layout of the master page
function updateMainLayout() {
    var divMainHeader = $("#divMainHeader");
    var divMainLeftPane = $("#divMainLeftPane");
    var divMainTabs = $("#divMainTabs");

    var paneH = $(window).height() - divMainHeader.outerHeight();
    divMainLeftPane.outerHeight(paneH);
    divMainTabs.outerWidth(paneH);
}

// Make visible main menu or views explorer
function showMenuOrExplorer() {
    if (mainMenuVisible) {
        $("#divMainMenu").css("display", "block");
    } else {
        $("#divMainExplorer").css("display", "block");
    }
}

// Expand or collapse the menu item
function toggleMenuItem(divExpander) {
    var divMenuItem = divExpander.parent();
    var divMenuSubitems = divMenuItem.nextUntil(".menu-item", ".menu-subitem");

    if (divExpander.hasClass("expanded")) {
        divExpander.removeClass("expanded");
        divMenuSubitems.css("display", "none");
    } else {
        divExpander.addClass("expanded");
        divMenuSubitems.css("display", "block");
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
    showMenuOrExplorer();
    expandSelectedMenuItem();

    // update layout on window resize
    $(window).resize(function () {
        updateMainLayout();
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