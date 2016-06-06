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
    // Window that holds popups
    this._holderWindow = window;

    // Callback functions of the existing popups
    this._popupCallbacks = new Map();
};

// Event handler that removes the popup on press Escape key
scada.Popup.prototype._removePopupOnEscape = function (event) {
    if (event.which == 27 /*Escape*/) {
        this._cancelDropdown(event.data);
    }
};

// Close the dropdown popup and execute a callback with a cancel result
scada.Popup.prototype._cancelDropdown = function (popupElem) {
    var frame = popupElem.find(".popup-frame");
    if (frame.length > 0) {
        var frameWnd = frame[0].contentWindow;
        var callback = this._popupCallbacks.get(frameWnd);
        this._popupCallbacks.delete(frameWnd);
        popupElem.remove();

        if (callback) {
            callback(false);
        }
    } else {
        popupElem.remove();
    }
};

// Get coodinates of the specified element relative to the holder window
scada.Popup.prototype._getOffset = function (elem) {
    // validate the element
    var defaultOffset = { left: 0, top: 0 };
    if (!(elem && elem.length)) {
        return defaultOffset;
    }

    // get coodinates within a window that contains the element
    var wnd = elem[0].ownerDocument.defaultView;
    var offset = elem.offset();
    var left = offset.left + $(wnd).scrollLeft();
    var top = offset.top + $(wnd).scrollTop();

    // add coordinates of the parent frames
    do {
        var parentWnd = wnd.parent;
        if (wnd != parentWnd) {
            if (parentWnd.$) {
                var frame = parentWnd.$(wnd.frameElement);
                if (frame.length > 0) {
                    offset = frame.offset();
                    left += offset.left + $(parentWnd).scrollLeft();
                    top += offset.top + $(parentWnd).scrollTop();
                }
            } else {
                console.warn("Unable to get offset, because jQuery is not found");
                return defaultOffset;
            }
            wnd = parentWnd;
        }
    } while (wnd != this._holderWindow && wnd != wnd.parent);

    return { left: left, top: top };
};

// Show popup with the specified url as a dropdown menu below the anchorElem.
// callback is function (dialogResult, extraParams)
scada.Popup.prototype.showDropdown = function (url, anchorElem, callback) {
    var thisObj = this;
    var popupElem = $("<div class='popup-dropdown'><div class='popup-overlay'></div>" +
        "<div class='popup-wrapper'><iframe class='popup-frame'></iframe></div></div>");
    $("body").append(popupElem);

    var overlay = popupElem.find(".popup-overlay");
    var wrapper = popupElem.find(".popup-wrapper");
    var frame = popupElem.find(".popup-frame");

    // setup overlay
    overlay
    .css("z-index", scada.utils.FRONT_ZINDEX)
    .click(function () {
        thisObj._cancelDropdown(popupElem);
    });

    // setup wrapper
    wrapper.css({
        "z-index": scada.utils.FRONT_ZINDEX + 1, // above the overlay
        "opacity": 0.0 // hide the popup while it's loading
    }); 

    // remove the popup on press Escape key in the parent window
    var removePopupOnEscape = this._removePopupOnEscape.bind(this);
    $(document)
    .off("keydown", null, removePopupOnEscape)
    .on("keydown", null, popupElem, removePopupOnEscape);

    // load the frame
    frame
    .on("load", function () {
        // store callback function
        var frameWnd = frame[0].contentWindow;
        thisObj._popupCallbacks.set(frameWnd, callback);

        // remove the popup on press Escape key in the frame
        if (frameWnd.$) {
            var jqFrameDoc = frameWnd.$(frameWnd.document);
            jqFrameDoc.ready(function () {
                jqFrameDoc
                .off("keydown", null, removePopupOnEscape)
                .on("keydown", null, popupElem, removePopupOnEscape);
            });
        }
    })
    .one("load", function () {
        // set the popup position
        var frameBody = frame.contents().find("body");
        var width = frameBody.outerWidth(true);
        var height = frameBody.outerHeight(true);
        var left = 0;
        var top = 0;

        if (anchorElem.length > 0) {
            var offset = thisObj._getOffset(anchorElem);
            left = offset.left;
            top = offset.top + anchorElem.outerHeight();
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
        frame
        .css({
            "width": width,
            "height": height
        })
        .focus();

        wrapper.css({
            "width": width,
            "height": height,
            "opacity": 1.0
        });
    })
    .attr("src", url);
};

// Close the dropdown popup and execute a callback with the specified result
scada.Popup.prototype.closeDropdown = function (popupWnd, dialogResult, extraParams) {
    var frame = $(popupWnd.frameElement);
    var popupElem = frame.closest(".popup-dropdown");
    var callback = this._popupCallbacks.get(popupWnd);
    this._popupCallbacks.delete(popupWnd);
    popupElem.remove();

    if (callback) {
        callback(dialogResult, extraParams);
    }
};

// Show modal dialog with the specified url.
// callback is function (dialogResult, extraParams),
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