/*
 * Fixed table header
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 *
 * Requires:
 * - jquery
 * - utils.js
 */

// Rapid SCADA namespace
var scada = scada || {};

// Fixed table headers processing object
scada.tableHeader = {
    // Set the fixed header cell widths equal to the original header cell widths
    _updateHeaderCellWidths: function (origHeader, fixedHeader) {
        var origCells = origHeader.find("td");

        fixedHeader.find("td").each(function (index) {
            // we need spans to set width exactly
            var origCell = origCells.eq(index);

            if (origCell.css("display") != "none") {
                var origCellSpan = origCell.children("span");
                if (origCellSpan.length > 0) {
                    var cellWidth = origCellSpan[0].getBoundingClientRect().width; // fractional value is required
                    $(this).find("span").width(cellWidth);
                    $(this).css("width", cellWidth);
                }
            }
        });
    },

    // Create fixed table headers and bind their events
    create: function () {
        var thisObj = this;

        $(".table-wrapper").each(function () {
            var wrapper = $(this);
            var table = wrapper.children("table");
            var origHeader = table.find("tr:first");
            var fixedHeader = origHeader.clone(false);

            origHeader.addClass("orig-table-header");
            fixedHeader.addClass("fixed-table-header");
            table.append(fixedHeader);
            thisObj._updateHeaderCellWidths(origHeader, fixedHeader);

            var setHeaderTopFunc = function () {
                fixedHeader.css("top", -table.position().top);
            };

            if (scada.utils.iOS()) {
                // prevent header blinking on iOS
                var scrollTimer = null;
                wrapper
                .off("scroll")
                .on("scroll", function () {
                    if (scrollTimer) {
                        clearTimeout(scrollTimer);
                    }
                    scrollTimer = setTimeout(setHeaderTopFunc, 100);
                });
            } else {
                wrapper.off("scroll").on("scroll", setHeaderTopFunc);
            }
        });
    },

    // Update the fixed header cell widths
    update: function () {
        var thisObj = this;

        $(".table-wrapper").each(function () {
            var wrapper = $(this);
            var table = wrapper.children("table");
            var fixedHeader = table.find("tr.fixed-table-header:first");

            if (fixedHeader.length > 0 /*already created*/) {
                var origHeader = table.find("tr.orig-table-header:first");
                thisObj._updateHeaderCellWidths(origHeader, fixedHeader);

                // make sure that the fixed header is the last row
                var lastRow = table.find("tr:last");
                if (!fixedHeader.is(lastRow)) {
                    fixedHeader.detach();
                    table.append(fixedHeader);
                }
            }
        });
    }
}