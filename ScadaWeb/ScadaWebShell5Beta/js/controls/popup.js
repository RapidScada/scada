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
 * Requires for modal dialogs:
 * - bootstrap
 * - eventtypes.js
 * - scada.modalButtonMap object
 */

// Rapid SCADA namespace
var scada = scada || {};

/********** Modal Dialog Buttons **********/

// Modal dialog buttons enumeration
scada.ModalButtons = {
    OK: "ok",
    YES: "yes",
    NO: "no",
    EXEC: "execute",
    CANCEL: "cancel",
    CLOSE: "close"
};

/********** Popup **********/

// Popup dialogs manipulation type
scada.Popup = function () {
    // Window that holds popups
    this._holderWindow = window;
};

// Close the dropdown popup and execute a callback with a cancel result
scada.Popup.prototype._cancelDropdown = function (popupElem) {
    var callback = popupElem.data("popup-callback");
    popupElem.remove();

    if (callback) {
        callback(false);
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

// Get html markup of a modal dialog footer buttons
scada.Popup.prototype._genModalButtonsHtml = function (buttons) {
    var html = "";

    for (var btn of buttons) {
        var btnText = scada.modalButtonMap ? scada.modalButtonMap.get(btn) : null;
        if (!btnText) {
            btnText = btn;
        }

        var subclass = btn == scada.ModalButtons.OK || btn == scada.ModalButtons.YES ? "btn-primary" :
            (btn == scada.ModalButtons.EXEC ? "btn-danger" : "btn-default");
        var dismiss = btn == scada.ModalButtons.CANCEL || btn == scada.ModalButtons.CLOSE ?
            " data-dismiss='modal'" : "";

        html += "<button type='button' class='btn " + subclass +
            "' data-result='" + btn + "'" + dismiss + ">" + btnText + "</button>";
    }

    return html;
}

// Find modal button by result
scada.Popup.prototype._findModalButton = function (modalWnd, btn) {
    var frame = $(modalWnd.frameElement);
    var modalElem = frame.closest(".modal");
    return modalElem.find(".modal-footer button[data-result='" + btn + "']");
}

// Show popup with the specified url as a dropdown menu below the anchorElem.
// opt_callback is a function (dialogResult, extraParams)
scada.Popup.prototype.showDropdown = function (url, anchorElem, opt_callback) {
    var thisObj = this;
    var popupElem = $("<div class='popup-dropdown'><div class='popup-overlay'></div>" +
        "<div class='popup-wrapper'><iframe class='popup-frame'></iframe></div></div>");

    if (opt_callback) {
        popupElem.data("popup-callback", opt_callback);
    }

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
    var removePopupOnEscapeFunc = function (event) {
        if (event.which == 27 /*Escape*/) {
            thisObj._cancelDropdown(popupElem);
        }
    }

    $(document)
    .off("keydown", removePopupOnEscapeFunc)
    .on("keydown", removePopupOnEscapeFunc);

    // load the frame
    frame
    .on("load", function () {
        // remove the popup on press Escape key in the frame
        var frameWnd = frame[0].contentWindow;
        if (frameWnd.$) {
            var jqFrameDoc = frameWnd.$(frameWnd.document);
            jqFrameDoc.ready(function () {
                jqFrameDoc
                .off("keydown", removePopupOnEscapeFunc)
                .on("keydown", removePopupOnEscapeFunc);
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
    var callback = popupElem.data("popup-callback");
    popupElem.remove();

    if (callback) {
        callback(dialogResult, extraParams);
    }
};

// Show modal dialog with the specified url.
// opt_callback is a function (dialogResult, extraParams),
// requires Bootstrap
scada.Popup.prototype.showModal = function (url, opt_buttons, opt_callback) {
    var footerHtml = opt_buttons && opt_buttons.length ?
        "<div class='modal-footer'>" + this._genModalButtonsHtml(opt_buttons) + "</div>" : "";

    var modalElem = $(
        "<div class='modal fade' tabindex='-1'>" +
        "<div class='modal-dialog'>" +
        "<div class='modal-content'>" +
        "<div class='modal-header'>" +
        "<button type='button' class='close' data-dismiss='modal'><span>&times;</span></button>" +
        "<h4 class='modal-title'></h4></div>" +
        "<div class='modal-body'><iframe class='modal-frame'></iframe></div>" +
        footerHtml +
        "</div></div></div>");

    if (opt_callback) {
        modalElem
            .data("modal-callback", opt_callback)
            .data("dialog-result", false);
    }

    $("body").append(modalElem);

    // load the frame
    var modalFrame = modalElem.find(".modal-frame");
    var hideModalOnEscapeFunc = function (event) {
        if (event.which == 27 /*Escape*/) {
            modalElem.modal("hide");
        }
    }

    modalFrame
    .on("load", function () {
        // remove the modal on press Escape key in the frame
        var frameWnd = modalFrame[0].contentWindow;
        if (frameWnd.$) {
            var jqFrameDoc = frameWnd.$(frameWnd.document);
            jqFrameDoc.ready(function () {
                jqFrameDoc
                .off("keydown", hideModalOnEscapeFunc)
                .on("keydown", hideModalOnEscapeFunc);
            });
        }
    })
    .one("load", function () {
        // set the frame size
        var frameBody = modalFrame.contents().find("body");
        var frameWidth = frameBody.outerWidth(true);
        var frameHeight = frameBody.outerHeight(true);

        modalFrame.css({
            "width": "100%",
            "height": frameHeight
        });

        // tune the modal
        var modalBody = modalElem.find(".modal-body");
        var modalPaddings = parseInt(modalBody.css("padding-left")) + parseInt(modalBody.css("padding-right"));
        modalElem.find(".modal-content").css("min-width", frameWidth + modalPaddings)

        // raise event on modal button click
        modalElem.find(".modal-footer button").click(function () {
            var result = $(this).data("result");
            var frameWnd = modalFrame[0].contentWindow;
            var frameJq = frameWnd.$;
            if (result && frameJq) {
                frameJq(frameWnd).trigger(scada.EventTypes.MODAL_BTN_CLICK, result);
            }
        });

        // display the modal
        modalElem
        .on('shown.bs.modal', function () {
            modalFrame.focus();
            modalElem.find(".modal-title").text(modalFrame[0].contentWindow.document.title);
        })
        .on('hidden.bs.modal', function () {
            var callback = $(this).data("modal-callback");
            if (callback) {
                callback($(this).data("dialog-result"), $(this).data("extra-params"));
            }

            $(this).remove();
        })        
        .modal("show");
    })
    .attr("src", url);
};

// Show the modal dialog
scada.Popup.prototype.closeModal = function (modalWnd, dialogResult, extraParams) {
    this.setModalResult(modalWnd, dialogResult, extraParams).modal("hide");
}

// Set dialog result for the whole modal dialog
scada.Popup.prototype.setModalResult = function (modalWnd, dialogResult, extraParams) {
    var frame = $(modalWnd.frameElement);
    var modalElem = frame.closest(".modal");
    modalElem
        .data("dialog-result", dialogResult)
        .data("extra-params", extraParams);
    return modalElem;
}

// Show or hide the button of the modal dialog
scada.Popup.prototype.setButtonVisible = function (modalWnd, btn, val) {
    this._findModalButton(modalWnd, btn).css("display", val ? "" : "none");
}

// Enable or disable the button of the modal dialog
scada.Popup.prototype.setButtonEnabled = function (modalWnd, btn, val) {
    var btnElem = this._findModalButton(modalWnd, btn);
    if (val) {
        btnElem.removeAttr("disabled");
    } else {
        btnElem.attr("disabled", "disabled");
    }
}

/********** Popup Locator **********/

// Popup locator object
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