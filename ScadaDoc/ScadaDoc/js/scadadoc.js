function createLayout() {
    var articleElems = $("body").children();
    articleElems.detach();

    var layoutElem = $("<div class='sd-contents'></div><div class='sd-article'></div>");
    $("body").append(layoutElem);
    $("body").css("overflow", "hidden");

    var divContents = $("div.sd-contents");
    var divArticle = $("div.sd-article");
    divArticle.append(articleElems);

    updateLayout();
    createContents();
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

function createContents() {
    var context = createContext();

    $.getScript(context.siteRoot + "js/contents-" + context.lang + ".js", function () {
        addContents(context);
    });
}

function createContext() {
    var siteRoot = "http://localhost:60765/";
    var docRoot = "http://localhost:60765/content/en/";
    var lang = "en";

    return {
        parDiv: $("div.sd-contents"),
        siteRoot: siteRoot,
        docRoot: docRoot,
        lang: lang
    };
}

function addArticle(context, link, title) {
    context.parDiv.append("<div><a href='" + context.docRoot + link + "'>" + title + "</a></div>");
}

$(document).ready(function () {
    createLayout();

    $(window).resize(function () {
        updateLayout();
    });
});