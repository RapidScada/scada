/*
 * Popup dialogs manipulation
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2018
 *
 * Requires:
 * - jquery
 * - utils.js
 *
 * Requires for modal dialog:
 * - eventtypes.js
 *
 * Requires for parent form of modal dialog:
 * - bootstrap
 * - eventtypes.js
 * - scada.modalButtonCaptions object
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

/********** Modal Dialog Sizes **********/

// Modal dialog sizes enumeration
scada.ModalSizes = {
    NORMAL: 0,
    SMALL: 1,
    LARGE: 2
};

/********** Modal Dialog Options **********/

// Modal dialog options class
scada.ModalOptions = function (buttons, opt_size, opt_height) {
    this.buttons = buttons;
    this.size = opt_size ? opt_size : scada.ModalSizes.NORMAL;
    this.height = opt_height ? opt_height : 0;
};

/********** Popup **********/

// Popup dialogs manipulation type
scada.Popup = function () {
    // Maximum length of a title
    this.MAX_TITLE_LEN = 50;

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

// Get caption for the specified modal dialog button
scada.Popup.prototype._getModalButtonCaption = function (btn) {
    var btnCaption = scada.modalButtonCaptions ? scada.modalButtonCaptions[btn] : null;
    return btnCaption ? btnCaption : btn;
};

// Get html markup of a modal dialog footer buttons
scada.Popup.prototype._genModalButtonsHtml = function (buttons) {
    var html = "";

    for (var btn of buttons) {
        var subclass = btn == scada.ModalButtons.OK || btn == scada.ModalButtons.YES ? "btn-primary" :
            (btn == scada.ModalButtons.EXEC ? "btn-danger" : "btn-default");
        var dismiss = btn == scada.ModalButtons.CANCEL || btn == scada.ModalButtons.CLOSE ?
            " data-dismiss='modal'" : "";

        html += "<button type='button' class='btn " + subclass + "' data-result='" + btn + "'" + dismiss + ">" +
            this._getModalButtonCaption(btn) + "</button>";
    }

    return html;
};

// Find modal button by result
scada.Popup.prototype._findModalButton = function (modalWnd, btn) {
    var frame = $(modalWnd.frameElement);
    var modalElem = frame.closest(".modal");
    return modalElem.find(".modal-footer button[data-result='" + btn + "']");
};

// Truncate the title if it is too long
scada.Popup.prototype._truncateTitle = function (s) {
    return s.length <= this.MAX_TITLE_LEN ? s : s.substr(0, this.MAX_TITLE_LEN) + "…";
};

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
    .off("keydown.scada.dropdown", removePopupOnEscapeFunc)
    .on("keydown.scada.dropdown", removePopupOnEscapeFunc);

    // load the frame
    frame
    .on("load", function () {
        // remove the popup on press Escape key in the frame
        var frameWnd = frame[0].contentWindow;
        if (scada.utils.checkAccessToFrame(frameWnd) && frameWnd.$) {
            var jqFrameDoc = frameWnd.$(frameWnd.document);
            jqFrameDoc.ready(function () {
                jqFrameDoc
                .off("keydown.scada.dropdown", removePopupOnEscapeFunc)
                .on("keydown.scada.dropdown", removePopupOnEscapeFunc);
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
scada.Popup.prototype.showModal = function (url, opt_options, opt_callback) {
    // create temporary overlay to prevent user activity
    var tempOverlay = $("<div class='popup-overlay'></div>");
    $("body").append(tempOverlay);

    // create the modal
    var buttons = opt_options ? opt_options.buttons : null;
    var footerHtml = buttons && buttons.length ?
        "<div class='modal-footer'>" + this._genModalButtonsHtml(buttons) + "</div>" : "";

    var size = opt_options ? opt_options.size : scada.ModalSizes.NORMAL;
    var sizeClass = "";
    if (size == scada.ModalSizes.SMALL) {
        sizeClass = " modal-sm";
    } else if (size == scada.ModalSizes.LARGE) {
        sizeClass = " modal-lg";
    }

    var modalElem = $(
        "<div class='modal fade' tabindex='-1'>" +
        "<div class='modal-dialog" + sizeClass + "'>" +
        "<div class='modal-content'>" +
        "<div class='modal-header'>" +
        "<button type='button' class='close' data-dismiss='modal'><span>&times;</span></button>" +
        "<h4 class='modal-title'></h4></div>" +
        "<div class='modal-body'></div>" +
        footerHtml +
        "</div></div></div>");

    if (opt_callback) {
        modalElem
            .data("modal-callback", opt_callback)
            .data("dialog-result", false);
    }

    // create the frame
    var modalFrame = $("<iframe class='modal-frame'></iframe>").css({
        "position": "fixed",
        "opacity": 0.0 // hide the frame while it's loading
    });

    var modalBody = modalElem.find(".modal-body");
    modalBody.append(modalFrame);
    $("body").append(modalElem);

    // create a function that hides the modal on press Escape key
    var hideModalOnEscapeFunc = function (event) {
        if (event.which == 27 /*Escape*/) {
            modalElem.modal("hide");
        }
    }

    // load the frame
    var thisObj = this;
    modalFrame
    .on("load", function () {
        // remove the modal on press Escape key in the frame
        var frameWnd = modalFrame[0].contentWindow;
        if (scada.utils.checkAccessToFrame(frameWnd) && frameWnd.$) {
            var jqFrameDoc = frameWnd.$(frameWnd.document);
            jqFrameDoc.ready(function () {
                jqFrameDoc
                .off("keydown.scada.modal", hideModalOnEscapeFunc)
                .on("keydown.scada.modal", hideModalOnEscapeFunc);
            });
        }
    })
    .one("load", function () {
        // get the frame size
        var frameBody = modalFrame.contents().find("body");
        var frameWidth = frameBody.outerWidth(true);
        var frameHeight = frameBody.outerHeight(true);
        var specifiedHeight = opt_options ? opt_options.height : 0;

        // tune the modal
        var frameWnd = modalFrame[0].contentWindow;
        var frameAccessible = scada.utils.checkAccessToFrame(frameWnd);
        var modalPaddings = parseInt(modalBody.css("padding-left")) + parseInt(modalBody.css("padding-right"));
        modalElem.find(".modal-content").css("min-width", frameWidth + modalPaddings);
        modalElem.find(".modal-title").text(thisObj._truncateTitle(frameAccessible ? frameWnd.document.title : url));

        // set the frame style
        modalFrame.css({
            "width": "100%",
            "height": specifiedHeight ? specifiedHeight : frameHeight,
            "position": "",
            "opacity": 1.0
        });

        // raise event on modal button click
        if (frameAccessible && frameWnd.$) {
            modalElem.find(".modal-footer button").click(function () {
                var result = $(this).data("result");

                if (result) {
                    frameWnd.$(frameWnd).trigger(scada.EventTypes.MODAL_BTN_CLICK, result);
                }
            });
        }

        // display the modal
        modalElem
        .on('shown.bs.modal', function () {
            if (!specifiedHeight) {
                modalFrame.css("height", frameBody.outerHeight(true)); // final update of the height
            }
            tempOverlay.remove();
            modalFrame.focus();
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

// Close the modal dialog
scada.Popup.prototype.closeModal = function (modalWnd, dialogResult, extraParams) {
    this.setModalResult(modalWnd, dialogResult, extraParams).modal("hide");
};

// Update the modal dialog height according to a frame height
scada.Popup.prototype.updateModalHeight = function (modalWnd) {
    var frame = $(modalWnd.frameElement);
    var frameBody = frame.contents().find("body");
    var modalElem = frame.closest(".modal");

    var iosScrollFix = scada.utils.iOS();
    if (iosScrollFix) {
        modalElem.css("overflow-y", "hidden");
    }

    frame.css("height", frameBody.outerHeight(true));

    if (iosScrollFix) {
        modalElem.css("overflow-y", "");
    }

    modalElem.modal("handleUpdate");
};

// Set dialog result for the whole modal dialog
scada.Popup.prototype.setModalResult = function (modalWnd, dialogResult, extraParams) {
    var modalElem = $(modalWnd.frameElement).closest(".modal");
    modalElem
        .data("dialog-result", dialogResult)
        .data("extra-params", extraParams);
    return modalElem;
};

// Set title of the modal dialog
scada.Popup.prototype.setModalTitle = function (modalWnd, title) {
    var modalElem = $(modalWnd.frameElement).closest(".modal");
    modalElem.find(".modal-title").text(title);
};

// Show or hide the button of the modal dialog
scada.Popup.prototype.setButtonVisible = function (modalWnd, btn, val) {
    this._findModalButton(modalWnd, btn).css("display", val ? "" : "none");
};

// Enable or disable the button of the modal dialog
scada.Popup.prototype.setButtonEnabled = function (modalWnd, btn, val) {
    var btnElem = this._findModalButton(modalWnd, btn);
    if (val) {
        btnElem.removeAttr("disabled");
    } else {
        btnElem.attr("disabled", "disabled");
    }
};

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
