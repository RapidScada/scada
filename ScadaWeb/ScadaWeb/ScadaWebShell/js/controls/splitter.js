/*
 * Splitter control
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

/********** Splitter Bulk **********/

// Splitter bulk contains the splitter and the resized divs
// Splitter bulk type
scada.SplitterBulk = function () {
    // Splitter is in resizing mode
    this._isResizing = false;

    // Current X coordinate of the cursor
    this._curX = 0;

    // Current Y coordinate of the cursor
    this._curY = 0;

    // The splitter
    this.splitter = null;

    // The left or top div
    this.prevDiv = null;

    // The right or bottom div
    this.nextDiv = null;

    // The splitter orientation is horizontal
    this.isHorizontal = true;

    // Default minimum size of the resized divs
    this.minSize = 50;
};

// Get minimum width of the resized div
scada.SplitterBulk.prototype._getMinWidth = function (jqObj) {
    var minWidth = parseInt(jqObj.css("min-width"), 10);
    return minWidth ? minWidth : this.minSize;
};

// Get minimum height of the resized div
scada.SplitterBulk.prototype._getMinHeight = function (jqObj) {
    var minHeight = parseInt(jqObj.css("min-height"), 10);
    return minHeight ? minHeight : this.minSize;
};

// Add overlay div into the resized div too allow receiving events over iframe
scada.SplitterBulk.prototype._addOverlay = function () {
    var overlay = $("<div class='splitter-overlay'><div/>").css({
        "cursor": this.isHorizontal ? "ns-resize" : "ew-resize",
        "z-index": scada.utils.FRONT_ZINDEX
    });
    $("body").append(overlay);
};

// Remove overlay div
scada.SplitterBulk.prototype._removeOverlay = function () {
    $("div.splitter-overlay").remove();
};

// Enter resizing mode
scada.SplitterBulk.prototype._startResizing = function (x, y) {
    this._isResizing = true;
    this._curX = x;
    this._curY = y;
    this._addOverlay(); // allow to receive events
    this.splitter.addClass("splitter-active");
};

// Change splitter position according to the cursor coordinates
scada.SplitterBulk.prototype._changePosition = function (x, y) {
    if (this._isResizing) {
        // resize divs
        if (this.isHorizontal) {
            var nextDivCurH = this.nextDiv.height();
            var prevDivCurH = this.prevDiv.height();
            var deltaY = y - this._curY;
            var delta = deltaY >= 0 ?
                Math.min(nextDivCurH - this._getMinHeight(this.nextDiv), deltaY) :
                Math.max(this._getMinHeight(this.prevDiv) - prevDivCurH, deltaY);
            this._curY += delta;

            this.prevDiv.height(prevDivCurH + delta);
            this.nextDiv.height(nextDivCurH - delta);
        } else {
            var nextDivCurW = this.nextDiv.width();
            var prevDivCurW = this.prevDiv.width();
            var deltaX = x - this._curX;
            var delta = deltaX >= 0 ?
                Math.min(nextDivCurW - this._getMinWidth(this.nextDiv), deltaX) :
                Math.max(this._getMinWidth(this.prevDiv) - prevDivCurW, deltaX);
            this._curX += delta;

            this.prevDiv.width(this.prevDiv.width() + delta);
            this.nextDiv.width(this.nextDiv.width() - delta);
        }
    }
};

// Exit resizing mode
scada.SplitterBulk.prototype.stopResizing = function (opt_stopResizingCallback) {
    if (this._isResizing) {
        this._isResizing = false;
        this._removeOverlay();
        this.splitter.removeClass("splitter-active");

        if (opt_stopResizingCallback) {
            opt_stopResizingCallback(this);
        }
    }
};

// Bind splitter events
scada.SplitterBulk.prototype.bindEvents = function () {
    var thisBulk = this;

    this.splitter
    .off()
    .on("mousedown touchstart", function (event) {
        if (event.type == "touchstart") {
            $(this).off("mousedown");
            event = event.originalEvent.touches[0];
        }

        thisBulk._startResizing(event.pageX, event.pageY);
    });

    $(document).on("mousemove touchmove", function (event) {
        if (event.type == "touchmove") {
            $(this).off("mousemove");
            event = event.originalEvent.touches[0];
        }

        thisBulk._changePosition(event.pageX, event.pageY);
    });
};

/********** Splitters Processing **********/

// Splitters processing object
scada.splitter = {
    // Splitter bulk array
    _splitterBulks: null,

    // Callback function on exit splitter resizing mode
    _stopResizingCallback: null,

    // Create splitter bulk array and bind bulk events
    _createBulks: function () {
        this._splitterBulks = [];
        var thisObj = this;

        $(".splitter").each(function () {
            var bulk = new scada.SplitterBulk();
            bulk.splitter = $(this);
            bulk.prevDiv = bulk.splitter.prev("div");
            bulk.nextDiv = bulk.splitter.next("div");
            bulk.isHorizontal = !bulk.splitter.hasClass("vert");

            if (bulk.prevDiv.length > 0 && bulk.nextDiv.length > 0) {
                bulk.bindEvents();
                thisObj._splitterBulks.push(bulk);
            }
        });
    },

    // Stop resizing all the splitters
    _stopAllResizing: function () {
        for (var bulk of this._splitterBulks) {
            bulk.stopResizing(this._stopResizingCallback);
        }
    },

    // Bind events for all the splitters
    _bindCommonEvents: function () {
        var thisObj = this;

        $(document).on("mouseup mouseleave touchend touchcancel", function () {
            thisObj._stopAllResizing();
        });

        $(window).resize(function () {
            thisObj._stopAllResizing();
        });
    },

    // Prepare splitters to work.
    // stopResizingCallback is function (splitterBulk)
    prepare: function (opt_stopResizingCallback) {
        this._stopResizingCallback = opt_stopResizingCallback;
        this._createBulks();
        this._bindCommonEvents();
    }
};