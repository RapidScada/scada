function addContents() {
    var articleElems = $("body").children();
    articleElems.detach();

    var layoutElem = $("<div class='sd-contents'><iframe class='sd-contents-frame' src='../contents.html'></iframe></div>" +
        "<div class='sd-article'></div>");
    $("body").append(layoutElem);
    $("body").css("overflow", "hidden");

    var divContents = $("div.sd-contents");
    var divArticle = $("div.sd-article");
    divArticle.append(articleElems);

    updateLayout();
}

function updateLayout() {
    var divContents = $("div.sd-contents");
    var divArticle = $("div.sd-article");

    var winH = $(window).height();
    var contW = divContents[0].getBoundingClientRect().width; // fractional value is required
    divContents.outerHeight(winH);
    divArticle.outerHeight(winH);
    divArticle.outerWidth($(window).width() - contW);
}

$(document).ready(function () {
    addContents();

    $(window).resize(function () {
        updateLayout();
    });
});