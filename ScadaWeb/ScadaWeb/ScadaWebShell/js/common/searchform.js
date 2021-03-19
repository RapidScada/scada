var scada = scada || {};

scada.searchPhrases = {};

scada.searchForm = {
    // The popup dialogs manipulation object.
    popup: scada.popupLocator.getPopup(),
    // The modal dialog options.
    modalOptions: new scada.ModalOptions([scada.ModalButtons.OK, scada.ModalButtons.CANCEL]),
    // The URL template of the web form to create an entity.
    createUrlTemplate: "",
    // The URL template of the web form to edit an entity.
    editUrlTemplate: "",

    // Converts GridView pagers to Bootstrap pagers.
    convertPagers: function () {
        $(".gv-pager table").each(function () {
            var gvPagerTable = $(this);
            var bsPager = $("<ul class='pagination'></ul>");

            gvPagerTable.find("td").each(function () {
                var pageElem = $(this).children().clone();
                var itemElem = $("<li>");

                if (pageElem.is("span")) {
                    itemElem.addClass("active");
                }

                itemElem.append(pageElem).appendTo(bsPager);
            });

            gvPagerTable.replaceWith(bsPager);
        });
    },

    // Builds a URL for creating or editing an entity according to the template.
    buildUrl: function (jqParamElem, templateUrl) {
        var hiddens = jqParamElem.siblings("input:hidden");
        var url = templateUrl;

        for (let i = 0; i < hiddens.length; i++) {
            url = url.replace("{" + i + "}", hiddens.eq(i).val());
        }

        return url;
    },

    // Binds the form events.
    bindEvents: function () {
        var thisObj = this;

        // open a modal dialog to create a new entity
        $(".search-new")
            .off("click")
            .on("click", function () {
                if (thisObj.popup && thisObj.createUrlTemplate) {
                    var createUrl = thisObj.buildUrl($(this), thisObj.createUrlTemplate);

                    thisObj.popup.showModal(createUrl, thisObj.modalOptions, function (dialogResult, extraParams) {
                        if (dialogResult) {
                            // refresh displayed data
                            $(".search-refresh").click();
                        }
                    });
                }

                return false;
            });

        // open a modal dialog to edit an entity
        $(".search-row-edit")
            .off("click")
            .on("click", function () {
                if (thisObj.popup && thisObj.editUrlTemplate) {
                    var editUrl = thisObj.buildUrl($(this), thisObj.editUrlTemplate);

                    popup.showModal(editUrl, thisObj.modalOptions, function (dialogResult, extraParams) {
                        if (dialogResult) {
                            // refresh displayed data
                            $(".search-refresh").click();
                        }
                    });
                }

                return false;
            });

        // confirm delete of a row
        $(".search-row-del")
            .off("click")
            .on("click", function () {
                if (!confirm(scada.searchPhrases.deleteRowConfirm)) {
                    return false;
                }
            });
    }
}

// Performs necessary actions after asynchronous request.
function asyncEndRequest() {
    scada.searchForm.convertPagers();
    scada.searchForm.bindEvents();
    scada.utils.scrollTo($(".main-content:first"), $(".alert"));
}

$(document).ready(function () {
    scada.searchForm.convertPagers();
    scada.searchForm.bindEvents();
});
