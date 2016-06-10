/*
 * Fixed table header
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 *
 * Requires:
 * - jquery
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
                var cellWidth = origCellSpan.width(); // jQuery may round fractional part
                origCellSpan.width(cellWidth);
                $(this).find("span").width(cellWidth);
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

            wrapper
            .off("scroll")
            .scroll(function () {
                var fixedHeaderTop = -table.position().top;
                fixedHeader.css("top", fixedHeaderTop);
            });
        });
    },

    // Update the fixed header cell widths
    update: function () {
        var thisObj = this;

        $(".table-wrapper").each(function () {
            var wrapper = $(this);
            var origHeader = $(this).find("tr.orig-table-header:first");
            var fixedHeader = $(this).find("tr.fixed-table-header:first");
            thisObj._updateHeaderCellWidths(origHeader, fixedHeader);
        });
    }
}