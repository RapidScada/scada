var articleUrl = articleUrl || "";

function FixImages() {
    $("img").each(function () {
        var divArticle = $(this).closest(".print-article");
        var link = divArticle.data("link");
        var lastSlashInd = link.lastIndexOf("/");

        if (lastSlashInd > 0) {
            var dir = link.substring(0, lastSlashInd + 1);
            var src = $(this).attr("src");
            $(this).attr("src", articleUrl + dir + src);
        }
    });
}

function FixLinks() {

}

$(document).ready(function () {
    FixImages();
    FixLinks();

    setTimeout(function () {
        console.log('The above "Image not found" errors are normal.');
    }, 0); 
});