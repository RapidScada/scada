var WEBSITE = "doc.rapidscada.net";

function createLayout() {
    var articleElems = $("body").children();
    articleElems.detach();

    var layoutElem = $("<div class='sd-contents-wrapper'><div class='sd-contents'></div></div>" +
        "<div class='sd-article-wrapper'><div class='sd-article'></div></div>");
    $("body").append(layoutElem);
    $("body").css("overflow", "hidden");

    var divContents = $("div.sd-contents");
    var divArticle = $("div.sd-article");
    divArticle.append(articleElems);

    updateLayout();
    createSearch();
    createContents();
    createCounter();

    styleIOS($("div.sd-contents-wrapper"));
    styleIOS($("div.sd-article-wrapper"));
}

function updateLayout() {
    var divContentsWrapper = $("div.sd-contents-wrapper");
    var divArticleWrapper = $("div.sd-article-wrapper");

    var winH = $(window).height();
    var contW = divContentsWrapper[0].getBoundingClientRect().width; // fractional value is required
    divContentsWrapper.outerHeight(winH);
    divArticleWrapper.outerHeight(winH);
    divArticleWrapper.outerWidth($(window).width() - contW);
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

function createCounter() {
    if (location.href.indexOf(WEBSITE) >= 0) {
        var counterScript = '<!-- Yandex.Metrika counter --> <script type="text/javascript"> (function (d, w, c) { (w[c] = w[c] || []).push(function() { try { w.yaCounter42248389 = new Ya.Metrika({ id:42248389, clickmap:true, trackLinks:true, accurateTrackBounce:true }); } catch(e) { } }); var n = d.getElementsByTagName("script")[0], s = d.createElement("script"), f = function () { n.parentNode.insertBefore(s, n); }; s.type = "text/javascript"; s.async = true; s.src = "https://mc.yandex.ru/metrika/watch.js"; if (w.opera == "[object Opera]") { d.addEventListener("DOMContentLoaded", f, false); } else { f(); } })(document, window, "yandex_metrika_callbacks"); </script> <noscript><div><img src="https://mc.yandex.ru/watch/42248389" style="position:absolute; left:-9999px;" alt="" /></div></noscript> <!-- /Yandex.Metrika counter -->';
        $("body").prepend(counterScript);
    }
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

function iOS() {
    return /iPad|iPhone|iPod/.test(navigator.platform);
}

function styleIOS(jqElem) {
    if (iOS()) {
        jqElem.css({
            "overflow": "scroll",
            "-webkit-overflow-scrolling": "touch"
        });
    }
}

$(document).ready(function () {
    if ($("body").hasClass("home")) {
        // add counter only to home page
        createCounter();
    } else {
        // create layout of article page
        createLayout();

        $(window).resize(function () {
            updateLayout();
        });
    }
});