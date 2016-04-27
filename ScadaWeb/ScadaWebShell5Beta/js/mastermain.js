// Update layout of the master page
function updateMainLayout() {
    var divMainHeader = $("#divMainHeader");
    var divMainLeftPane = $("#divMainLeftPane");
    var divMainTabs = $("#divMainTabs");

    var paneH = $(window).height() - divMainHeader.outerHeight();
    divMainLeftPane.outerHeight(paneH);
    divMainTabs.outerWidth(paneH);
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

$(document).ready(function () {
    updateMainLayout();

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