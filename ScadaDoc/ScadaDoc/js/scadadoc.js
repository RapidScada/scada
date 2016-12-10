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
    createSearch();
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

function createSearch() {
    var searchHtml =
        "<script>" +
        "  (function() {" +
        "    var cx = '003943521229341952511:vsuy-pqfiri';" +
        "    var gcse = document.createElement('script');" +
        "    gcse.type = 'text/javascript';" +
        "    gcse.async = true;\n" +
        "    gcse.src = 'https://cse.google.com/cse.js?cx=' + cx;" +
        "    var s = document.getElementsByTagName('script')[0];" +
        "    s.parentNode.insertBefore(gcse, s);" +
        "  })();" +
        "</script>" +
        "<gcse:search></gcse:search>";

    $("div.sd-contents").append(searchHtml);
}

function createContents() {
    var context = createContext();

    $.getScript(context.siteRoot + "js/contents-" + context.lang + ".js", function () {
        addContents(context);

        var selItem = $(".sd-contents-item.selected:first");
        if (selItem.length > 0) {
            context.contents.scrollTop(selItem.offset().top);
        }

        if (typeof onContentsCreated == "function") {
            onContentsCreated();
        }
    });
}

function createContext() {
    var siteRoot = location.origin + "/";
    var docRoot = siteRoot + "content/en/";
    var lang = "en";

    var href = location.href;
    var i1 = href.indexOf("/content/");

    if (i1 >= 0) {
        siteRoot = href.substring(0, i1 + 1);
        docRoot = siteRoot + "content/en/";
        var i2 = i1 + "/content/".length;
        var i3 = href.indexOf("/", i2);

        if (i3 >= 0) {
            lang = href.substring(i2, i3);
            docRoot = siteRoot + "content/" + lang + "/";
        }
    }

    return {
        contents: $("div.sd-contents"),
        siteRoot: siteRoot,
        docRoot: docRoot,
        lang: lang
    };
}

function addArticle(context, link, title, level) {
    var url = context.docRoot + link;
    var itemInnerHtml = link ? "<a href='" + url + "'>" + title + "</a>" : title;
    var levClass = level ? " level" + level : "";
    var selClass = link && url == location.href ? " selected" : "";

    var contentsItem = $("<div class='sd-contents-item" + levClass + selClass + "'>" + itemInnerHtml + "</div>");
    context.contents.append(contentsItem);
}

function copyContentsToArticle() {
    var selItem = $(".sd-contents-item.selected:first");

    if (selItem.length) {
        var stopClass = selItem.attr("class").replace(" selected", "");
        var reqClass = selItem.next(".sd-contents-item").attr("class");
        var divArticle = $(".sd-article");

        var titleText = selItem.find("a").text();
        document.title = titleText + " - " + document.title;
        $("<h1>").text(titleText).appendTo(divArticle);

        selItem.nextAll().each(function () {
            var curClass = $(this).attr("class");

            if (curClass == reqClass) {
                var linkElem = $(this).find("a");
                if (linkElem.length) {
                    $("<p>").append(linkElem.clone()).appendTo(divArticle);
                }
            } else if (curClass == stopClass) {
                return false;
            }
        });
    }
}

$(document).ready(function () {
    createLayout();

    $(window).resize(function () {
        updateLayout();
    });
});