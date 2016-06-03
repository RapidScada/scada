/*
 * Popup dialogs manipulation
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 *
 * Requires:
 * - jquery
 * - utils.js
 *
 * Optional:
 * - bootstrap
 */

// Rapid SCADA namespace
var scada = scada || {};

// Popup dialogs manipulation type
scada.Popup = function () {
};

// Event handler that removes the popup on press Escape key
scada.Popup.prototype._removePopupOnEscape = function (event) {
    if (event.which == 27 /*Escape*/) {
        var popupElem = event.data;
        popupElem.remove();
    }
}

// Show popup with the specified url as a dropdown menu below the anchorElem.
// callback is function (success, dialogResult, extraParams)
scada.Popup.prototype.showDropdown = function (url, anchorElem, callback) {
    var popupElem = $("<div class='popup-overlay'></div>" +
        "<div class='popup-wrapper'><iframe class='popup-frame'></iframe></div>");
    $("body").append(popupElem);

    var overlay = popupElem.filter(".popup-overlay");
    var wrapper = popupElem.filter(".popup-wrapper");
    var frame = popupElem.find(".popup-frame");

    // setup overlay
    overlay
    .css("z-index", scada.utils.FRONT_ZINDEX)
    .click(function () {
        popupElem.remove();
    });

    // setup wrapper
    wrapper
    .focus()
    .css({
        "z-index": scada.utils.FRONT_ZINDEX + 1, // above the overlay
        "opacity": 0.0 // hide the popup while it's loading
    }); 

    // remove the popup on press Escape key in the parent window
    /*$(document)
    .off(this._removePopupOnEscape)
    .on("keydown", null, popupElem, this._removePopupOnEscape);*/

    // load the frame
    var thisObj = this;
    frame
    .on("load", function () {
        // remove the popup on press Escape key in the frame
        /*var frameWnd = frame[0].contentWindow;
        if (frameWnd.$) {
            var jqFrameDoc = frameWnd.$(frameWnd.document);
            jqFrameDoc.ready(function () {
                jqFrameDoc
                .off(thisObj._removePopupOnEscape)
                .on("keydown", null, popupElem, thisObj._removePopupOnEscape);
            });
        }*/
    })
    .one("load", function () {
        // set the popup position
        var frameBody = frame.contents().find("body");
        var width = frameBody.outerWidth(true);
        var height = frameBody.outerHeight(true);
        var left = 0;
        var top = 0;

        if (anchorElem.length > 0) {
            left = anchorElem.offset().left;
            top = anchorElem.offset().top + anchorElem.outerHeight();
            var borderWidthX2 = parseInt(wrapper.css("border-width"), 10) * 2;

            if (left + width + borderWidthX2 > $(document).width())
                left = Math.max($(document).width() - width - borderWidthX2, 0);

            if (top + height + borderWidthX2 > $(document).height())
                top = Math.max($(document).height() - height - borderWidthX2, 0);
        }
        else {
            left = Math.max(($(window).width() - width) / 2, 0);
            top = Math.max(($(window).height() - height) / 2, 0);
        }

        wrapper.css({
            "left": left,
            "top": top
        });

        // set the popup size and display the popup
        frame.css({
            "width": width,
            "height": height
        });

        wrapper.css({
            "width": width,
            "height": height,
            "opacity": 1.0
        });
    })
    .attr("src", url);
};

// Show modal dialog with the specified url.
// callback is function (success, dialogResult, extraParams),
// requires Bootstrap
scada.Popup.prototype.showModal = function (url, callback) {

};

// Popup instance locator object
scada.popupLocator = {
    // Find and return an existing popup object
    getPopup: function () {
        var wnd = window;
        while (wnd) {
            if (wnd.popup) {
                return wnd.popup;
            }
            wnd = wnd == window.top ? null : window.parent;
        }
        return null;
    }
};