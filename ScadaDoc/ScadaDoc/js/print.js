var articleUrl = articleUrl || "";

function FixArticles() {
    $(".print-article").each(function () {
        var divArticle = $(this).closest(".print-article");
        var articleLink = divArticle.data("link");
        var articleDir = articleLink.substring(0, articleLink.lastIndexOf("/") + 1);

        if (articleDir) {
            var urlPrefix = articleUrl + articleDir;

            // fix images of the article
            divArticle.find("img").each(function () {
                var src = $(this).attr("src");
                $(this).attr("src", urlPrefix + src);
            });

            // fix hyperlinks of the article
            divArticle.find("a").each(function () {
                var href = $(this).attr("href");

                if (!href.startsWith("http://") && !href.startsWith("https://")) {
                    $(this).attr({
                        href: urlPrefix + href,
                        target: "_blank"
                    });
                }
            });
        }
    });
}

$(document).ready(function () {
    FixArticles();

    setTimeout(function () {
        console.log('The above "Image not found" errors are normal.');
    }, 0); 
});