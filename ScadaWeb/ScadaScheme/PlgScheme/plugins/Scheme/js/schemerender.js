/*
 * Scheme rendering
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2020
 *
 * Requires:
 * - jquery
 * - eventtypes.js
 * - schemecommon.js
 *
 * Inheritance hierarchy:
 * Renderer
 *   SchemeRenderer
 *   ComponentRenderer
 *     StaticTextRenderer
 *       DynamicTextRenderer
 *     StaticPictureRenderer
 *       DynamicPictureRenderer
 */

// Rapid SCADA namespace
var scada = scada || {};
// Scheme namespace
scada.scheme = scada.scheme || {};

/********** Renderer **********/

// Abstract parent type for renderers
scada.scheme.Renderer = function () {
    // Color bound to an input channel status
    this.STATUS_COLOR = "Status";
    // Color displayed in edit mode for the Status color
    this.STATUS_DISPLAY_COLOR = "Silver";
    // Width of the wrapper frame of a component in edit mode
    this.COMP_FRAME_WIDTH = 1;
};

// Set background color of the jQuery object
scada.scheme.Renderer.prototype.setBackColor = function (jqObj, backColor, opt_removeIfEmpty, opt_statusColor) {
    if (backColor) {
        if (backColor === this.STATUS_COLOR) {
            jqObj.css("background-color", opt_statusColor ? opt_statusColor : this.STATUS_DISPLAY_COLOR);
        } else if (typeof opt_statusColor === "undefined") {
            jqObj.css("background-color", backColor);
        }
    } else if (opt_removeIfEmpty) {
        jqObj.css("background-color", "");
    }
};

// Set border color of the jQuery object
scada.scheme.Renderer.prototype.setBorderColor = function (jqObj, borderColor, opt_removeIfEmpty, opt_statusColor) {
    if (borderColor) {
        if (borderColor === this.STATUS_COLOR) {
            jqObj.css("border-color", opt_statusColor ? opt_statusColor : this.STATUS_DISPLAY_COLOR);
        } else if (typeof opt_statusColor === "undefined") {
            jqObj.css("border-color", borderColor);
        }
    } else if (opt_removeIfEmpty) {
        jqObj.css("border-color", "transparent");
    }
};

// Set border width and style of the jQuery object
scada.scheme.Renderer.prototype.setBorderWidth = function (jqObj, borderWidth, opt_removeIfEmpty) {
    if (Number.isInteger(borderWidth) && borderWidth > 0) {
        jqObj.css({
            "border-style": "solid",
            "border-width": borderWidth
        });
    } else if (opt_removeIfEmpty) {
        jqObj.css({
            "border-style": "none",
            "border-width": ""
        });
    }
};

// Set fore color of the jQuery object
scada.scheme.Renderer.prototype.setForeColor = function (jqObj, foreColor, opt_removeIfEmpty, opt_statusColor) {
    if (foreColor) {
        if (foreColor === this.STATUS_COLOR) {
            jqObj.css("color", opt_statusColor ? opt_statusColor : this.STATUS_DISPLAY_COLOR);
        } else if (typeof opt_statusColor === "undefined") {
            jqObj.css("color", foreColor);
        }
    } else if (opt_removeIfEmpty) {
        jqObj.css("color", "");
    }
};

// Set font of the jQuery object
scada.scheme.Renderer.prototype.setFont = function (jqObj, font, opt_removeIfEmpty) {
    if (font) {
        jqObj.css({
            "font-family": font.Name,
            "font-size": font.Size + "px",
            "font-weight": font.Bold ? "bold" : "normal",
            "font-style": font.Italic ? "italic" : "normal",
            "text-decoration": font.Underline ? "underline" : "none"
        });
    } else if (opt_removeIfEmpty) {
        jqObj.css({
            "font-family": "",
            "font-size": "",
            "font-weight": "",
            "font-style": "",
            "text-decoration": ""
        });
    }
};

// Set background image of the jQuery object
scada.scheme.Renderer.prototype.setBackgroundImage = function (jqObj, image, opt_removeIfEmpty) {
    if (image) {
        jqObj.css("background-image", this.imageToDataUrlCss(image));
    } else if (opt_removeIfEmpty) {
        jqObj.css("background-image", "");
    }
};

// Returns a data URI containing a representation of the image
scada.scheme.Renderer.prototype.imageToDataURL = function (image) {
    return image ?
        "data:" + (image.MediaType ? image.MediaType : "image/png") + ";base64," + image.Data :
        "";
};

// Returns a css property value for the image data URI
scada.scheme.Renderer.prototype.imageToDataUrlCss = function (image) {
    return "url('" + this.imageToDataURL(image) + "')";
};

// Create DOM content of the component according to the component properties.
// If component properties are incorrect, no DOM content is created
scada.scheme.Renderer.prototype.createDom = function (component, renderContext) {
};

// Refresh the component if it contains the specified images
scada.scheme.Renderer.prototype.refreshImages = function (component, renderContext, imageNames) {
};

// Update the component according to the current input channel data
scada.scheme.Renderer.prototype.updateData = function (component, renderContext) {
};

/********** Scheme Renderer **********/

// Scheme renderer type extends scada.scheme.Renderer
scada.scheme.SchemeRenderer = function () {
    scada.scheme.Renderer.call(this);
};

scada.scheme.SchemeRenderer.prototype = Object.create(scada.scheme.Renderer.prototype);
scada.scheme.SchemeRenderer.constructor = scada.scheme.SchemeRenderer;

scada.scheme.SchemeRenderer.prototype._setTitle = function (title, renderContext) {
    if (title && renderContext.schemeEnv.viewHub) {
        document.title = title + " - " + renderContext.schemeEnv.viewHub.getEnv().productName;

        // set title of a popup in case the scheme is in the popup
        var popup = scada.popupLocator.getPopup();
        if (popup) {
            popup.setModalTitle(window, document.title);
        }

        // send notification about title change
        renderContext.schemeEnv.viewHub.notify(scada.EventTypes.VIEW_TITLE_CHANGED, window, document.title);
    }
};

scada.scheme.SchemeRenderer.prototype._calcScale = function (component, scaleType, scaleValue) {
    var ScaleTypes = scada.scheme.ScaleTypes;

    if (scaleType === ScaleTypes.NUMERIC) {
        if (!isNaN(scaleValue) && scaleValue >= 0) {
            return scaleValue;
        }
    }
    else if (component.parentDomElem) {
        var areaWidth = component.parentDomElem.innerWidth();
        var schemeWidth = component.props.Size.Width;
        var horScale = areaWidth / schemeWidth;

        if (scaleType === ScaleTypes.FIT_SCREEN) {
            var schemeHeight = component.props.Size.Height;
            var areaHeight = component.parentDomElem.innerHeight();
            var vertScale = areaHeight / schemeHeight;
            return Math.min(horScale, vertScale);
        } else if (scaleType === ScaleTypes.FIT_WIDTH) {
            return horScale;
        }
    }

    return 1.0;
};

scada.scheme.SchemeRenderer.prototype.createDom = function (component, renderContext) {
    var divScheme = $("<div class='scheme'></div>");
    component.dom = divScheme;
    this.updateDom(component, renderContext);
};

scada.scheme.SchemeRenderer.prototype.refreshImages = function (component, renderContext, imageNames) {
    if (component.dom) {
        var props = component.props;

        if (Array.isArray(imageNames) && imageNames.includes(props.BackImageName)) {
            var divSchemeBack = component.dom.find(".scheme-back:first");
            var backImage = renderContext.getImage(props.BackImageName);
            this.setBackgroundImage(divSchemeBack, backImage, true);
        }
    }
};

// Update existing DOM content of the scheme according to the scheme properties
scada.scheme.SchemeRenderer.prototype.updateDom = function (component, renderContext) {
    if (component.dom) {
        var props = component.props; // scheme document properties
        var schemeWidth = props.Size.Width;
        var schemeHeight = props.Size.Height;

        var divScheme = component.dom.first();
        divScheme.css({
            "width": schemeWidth,
            "height": schemeHeight,
        });

        if (!renderContext.editMode) {
            this.setBackColor($("body"), props.BackColor, true);
        }

        this.setBackColor(divScheme, props.BackColor, true);
        this.setFont(divScheme, props.Font, true);
        this.setForeColor(divScheme, props.ForeColor, true);

        // set background image if presents,
        // the additional div is required for correct scaling
        var divSchemeBack = divScheme.children(".scheme-back:first");

        if (divSchemeBack.length === 0) {
            divSchemeBack = $("<div class='scheme-back'></div>");
            divScheme.append(divSchemeBack);
        }

        divSchemeBack.css({
            "width": schemeWidth,
            "height": schemeHeight,
            "background-size": schemeWidth + "px " + schemeHeight + "px",
            "background-repeat": "no-repeat"
        });

        var backImage = renderContext.getImage(props.BackImageName);
        this.setBackgroundImage(divSchemeBack, backImage, true);

        // set title
        if (!renderContext.editMode) {
            this._setTitle(props.Title, renderContext);
        }
    }
};

// Set the scheme scale
scada.scheme.SchemeRenderer.prototype.setScale = function (component, scaleType, scaleValue) {
    if (component.dom) {
        var scale = this._calcScale(component, scaleType, scaleValue);
        //var sizeCoef = Math.min(scale, 1);

        component.dom.css({
            "transform": "scale(" + scale + ", " + scale + ")",
            //"width": component.props.Size.Width * sizeCoef,
            //"height": component.props.Size.Height * sizeCoef
        });

        return scale;
    }
};

/********** Component Renderer **********/

// Abstract component renderer type extends scada.scheme.Renderer
scada.scheme.ComponentRenderer = function () {
    scada.scheme.Renderer.call(this);
};

scada.scheme.ComponentRenderer.prototype = Object.create(scada.scheme.Renderer.prototype);
scada.scheme.ComponentRenderer.constructor = scada.scheme.ComponentRenderer;

// Get dynamic color that may depend on input channel status
scada.scheme.ComponentRenderer.prototype._getDynamicColor = function (color, cnlNum, renderContext) {
    if (color) {
        if (color === this.STATUS_COLOR) {
            var curCnlDataExt = renderContext.curCnlDataMap ?
                renderContext.curCnlDataMap.get(cnlNum) : null;
            return curCnlDataExt ? curCnlDataExt.Color : "";
        } else {
            return color;
        }
    } else {
        return "";
    }
};

// Set fore color of the jQuery object that may depend on input channel status
scada.scheme.ComponentRenderer.prototype.setDynamicForeColor = function (jqObj, foreColor,
    cnlNum, renderContext, opt_removeIfEmpty) {
    this.setForeColor(jqObj, this._getDynamicColor(foreColor, cnlNum, renderContext), opt_removeIfEmpty);
};

// Set background color of the jQuery object that may depend on input channel status
scada.scheme.ComponentRenderer.prototype.setDynamicBackColor = function (jqObj, backColor,
    cnlNum, renderContext, opt_removeIfEmpty) {
    this.setBackColor(jqObj, this._getDynamicColor(backColor, cnlNum, renderContext), opt_removeIfEmpty);
};

// Set border color of the jQuery object that may depend on input channel status
scada.scheme.ComponentRenderer.prototype.setDynamicBorderColor = function (jqObj, borderColor,
    cnlNum, renderContext, opt_removeIfEmpty) {
    this.setBorderColor(jqObj, this._getDynamicColor(borderColor, cnlNum, renderContext), opt_removeIfEmpty);
};

// Choose a color according to hover state
scada.scheme.ComponentRenderer.prototype.chooseColor = function (isHovered, color, colorOnHover) {
    return isHovered && colorOnHover ? colorOnHover : color;
};

// Set text wrapping of the jQuery object
scada.scheme.ComponentRenderer.prototype.setWordWrap = function (jqObj, wordWrap) {
    jqObj.css("white-space", wordWrap ? "normal" : "nowrap");
};

// Set horizontal algnment of the jQuery object
scada.scheme.ComponentRenderer.prototype.setHAlign = function (jqObj, hAlign) {
    var HorizontalAlignments = scada.scheme.HorizontalAlignments;

    switch (hAlign) {
        case HorizontalAlignments.CENTER:
            jqObj.css("text-align", "center");
            break;
        case HorizontalAlignments.RIGHT:
            jqObj.css("text-align", "right");
            break;
        default:
            jqObj.css("text-align", "left");
            break;
    }
};

// Set vertical algnment of the jQuery object
scada.scheme.ComponentRenderer.prototype.setVAlign = function (jqObj, vAlign) {
    var VerticalAlignments = scada.scheme.VerticalAlignments;

    switch (vAlign) {
        case VerticalAlignments.CENTER:
            jqObj.css("vertical-align", "middle");
            break;
        case VerticalAlignments.BOTTOM:
            jqObj.css("vertical-align", "bottom");
            break;
        default:
            jqObj.css("vertical-align", "top");
            break;
    }
};

// Set tooltip (title) of the jQuery object
scada.scheme.ComponentRenderer.prototype.setToolTip = function (jqObj, toolTip) {
    if (toolTip) {
        jqObj.prop("title", toolTip);
    }
};

// Set primary css properties of the jQuery object of the scheme component
scada.scheme.ComponentRenderer.prototype.prepareComponent = function (jqObj, component, opt_skipSize, opt_skipColors) {
    var props = component.props;
    jqObj
        .addClass("comp")
        .data("id", props.ID)
        .css({
            "left": props.Location.X,
            "top": props.Location.Y,
            "z-index": props.ZIndex
        });

    if (!opt_skipSize) {
        jqObj.css({
            "width": props.Size.Width,
            "height": props.Size.Height
        });
    }

    if (!opt_skipColors) {
        this.setBackColor(jqObj, props.BackColor);
        this.setBorderColor(jqObj, props.BorderColor);
        this.setBorderWidth(jqObj, props.BorderWidth);
    }

    this.setToolTip(jqObj, props.ToolTip);
};

// Bind user action to the component
scada.scheme.ComponentRenderer.prototype.bindAction = function (jqObj, component, renderContext) {
    var Actions = scada.scheme.Actions;
    var props = component.props;
    var action = props.Action;
    var actionIsBound =
        action === Actions.DRAW_DIAGRAM && props.InCnlNum > 0 ||
        (action === Actions.SEND_COMMAND || action === Actions.SEND_COMMAND_NOW) &&
        props.CtrlCnlNum > 0 && renderContext.controlRight;

    if (actionIsBound) {
        jqObj.addClass("action");

        if (!renderContext.editMode) {
            var viewHub = renderContext.schemeEnv.viewHub;
            var dialogs = viewHub ? viewHub.dialogs : null;

            jqObj.click(function () {
                switch (props.Action) {
                    case Actions.DRAW_DIAGRAM:
                        if (dialogs) {
                            var date = viewHub.curViewDateMs ? new Date(viewHub.curViewDateMs) : new Date();
                            dialogs.showChart(props.InCnlNum, renderContext.viewID, date);
                        } else {
                            console.warn("Dialogs object is undefined");
                        }
                        break;

                    case Actions.SEND_COMMAND:
                        if (dialogs) {
                            dialogs.showCmd(props.CtrlCnlNum, renderContext.viewID);
                        } else {
                            console.warn("Dialogs object is undefined");
                        }
                        break;

                    case Actions.SEND_COMMAND_NOW:
                        if (renderContext.schemeEnv) {
                            renderContext.schemeEnv.sendCommand(props.CtrlCnlNum, component.cmdVal,
                                renderContext.viewID, component.id);
                        } else {
                            console.warn("Scheme environment object is undefined");
                        }
                        break;
                }
            });
        }
    }
};

// Get location of the component. Returns an object containing the properties x and y
scada.scheme.ComponentRenderer.prototype.getLocation = function (component) {
    if (component.props && component.props.Location) {
        return {
            x: component.props.Location.X,
            y: component.props.Location.Y
        };
    } else {
        return {
            x: 0,
            y: 0
        };
    }
};

// Set location of the component
scada.scheme.ComponentRenderer.prototype.setLocation = function (component, x, y) {
    component.props.Location = { X: x, Y: y };

    if (component.dom) {
        var compParent = component.dom.parent();

        if (compParent.is(".comp-wrapper")) {
            compParent.css({
                "left": x - this.COMP_FRAME_WIDTH,
                "top": y - this.COMP_FRAME_WIDTH,
                "z-index": component.props.ZIndex
            });

            component.dom.css({
                "left": "",
                "top": ""
            });
        } else {
            component.dom.css({
                "left": x,
                "top": y
            });
        }
    }
};

// Set properties of the component wrapper in edit mode
scada.scheme.ComponentRenderer.prototype.setWrapperProps = function (component) {
    // set location
    var location = this.getLocation(component);
    this.setLocation(component, location.x, location.y);

    // set font
    if (component.props && component.dom) {
        var wrapper = component.dom.parent(".comp-wrapper");
        this.setFont(wrapper, component.props.Font, true);
    }
};

// Get size of the component. Returns an object containing the properties width and height
scada.scheme.ComponentRenderer.prototype.getSize = function (component) {
    if (component.props && component.props.Size) {
        return {
            width: component.props.Size.Width,
            height: component.props.Size.Height
        };
    } else {
        return {
            width: 0,
            height: 0
        };
    }
};

// Set size of the component
scada.scheme.ComponentRenderer.prototype.setSize = function (component, width, height) {
    component.props.Size = { Width: width, Height: height };

    if (component.dom) {
        component.dom.css({
            "width": width,
            "height": height
        });
    }
};

// Check the possibility of resizing in edit mode
scada.scheme.ComponentRenderer.prototype.allowResizing = function (component) {
    return true;
};

// Wrap the component with a frame needed in edit mode
scada.scheme.ComponentRenderer.prototype.wrap = function (component) {
    var compWrapper = $("<div class='comp-wrapper'></div>").append(component.dom);
    this.setWrapperProps(component);
    return compWrapper;
};

/********** Static Text Renderer **********/

// Static text renderer type extends scada.scheme.ComponentRenderer
scada.scheme.StaticTextRenderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.StaticTextRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.StaticTextRenderer.constructor = scada.scheme.StaticTextRenderer;

scada.scheme.StaticTextRenderer.prototype.createDom = function (component, renderContext) {
    var props = component.props;

    var spanComp = $("<span id='comp" + component.id + "'></span>");
    var spanText = $("<span></span>");

    this.prepareComponent(spanComp, component, true);
    this.setFont(spanComp, props.Font);
    this.setForeColor(spanComp, props.ForeColor);

    if (props.AutoSize) {
        spanComp.css("display", "inline-block");
        this.setWordWrap(spanText, false);
    } else {
        spanComp.css("display", "table");
        var borders = props.BorderWidth * 2;

        spanText.css({
            "display": "table-cell",
            "overflow": "hidden",
            "max-width": props.Size.Width - borders,
            "width": props.Size.Width - borders,
            "height": props.Size.Height - borders
        });

        this.setHAlign(spanComp, props.HAlign);
        this.setVAlign(spanText, props.VAlign);
        this.setWordWrap(spanText, props.WordWrap);
    }

    spanText.text(props.Text);
    spanComp.append(spanText);
    component.dom = spanComp;
};

scada.scheme.StaticTextRenderer.prototype.setSize = function (component, width, height) {
    component.props.Size = { Width: width, Height: height };

    var spanText = component.dom.children();
    spanText.css({
        "max-width": width,
        "width": width,
        "height": height
    });
};

scada.scheme.StaticTextRenderer.prototype.allowResizing = function (component) {
    return !component.props.AutoSize;
};

/********** Dynamic Text Renderer **********/

// Dynamic text renderer type extends scada.scheme.StaticTextRenderer
scada.scheme.DynamicTextRenderer = function () {
    scada.scheme.StaticTextRenderer.call(this);
};

scada.scheme.DynamicTextRenderer.prototype = Object.create(scada.scheme.StaticTextRenderer.prototype);
scada.scheme.DynamicTextRenderer.constructor = scada.scheme.DynamicTextRenderer;

scada.scheme.DynamicTextRenderer.prototype._setUnderline = function (jqObj, underline) {
    if (underline) {
        jqObj.css("text-decoration", "underline");
    }
};

scada.scheme.DynamicTextRenderer.prototype._restoreUnderline = function (jqObj, font) {
    jqObj.css("text-decoration", font && font.Underline ? "underline" : "none");
};

scada.scheme.DynamicTextRenderer.prototype.createDom = function (component, renderContext) {
    scada.scheme.StaticTextRenderer.prototype.createDom.call(this, component, renderContext);

    var ShowValueKinds = scada.scheme.ShowValueKinds;
    var props = component.props;
    var spanComp = component.dom.first();
    var spanText = component.dom.children();
    var cnlNum = props.InCnlNum;

    if (props.ShowValue > ShowValueKinds.NOT_SHOW && !props.Text) {
        spanText.text("[" + cnlNum + "]");
    }

    this.bindAction(spanComp, component, renderContext);

    // apply properties on hover
    var thisRenderer = this;

    spanComp.hover(
        function () {
            thisRenderer.setDynamicBackColor(spanComp, props.BackColorOnHover, cnlNum, renderContext);
            thisRenderer.setDynamicBorderColor(spanComp, props.BorderColorOnHover, cnlNum, renderContext);
            thisRenderer.setDynamicForeColor(spanComp, props.ForeColorOnHover, cnlNum, renderContext);
            thisRenderer._setUnderline(spanComp, props.UnderlineOnHover);
        },
        function () {
            thisRenderer.setDynamicBackColor(spanComp, props.BackColor, cnlNum, renderContext, true);
            thisRenderer.setDynamicBorderColor(spanComp, props.BorderColor, cnlNum, renderContext, true);
            thisRenderer.setDynamicForeColor(spanComp, props.ForeColor, cnlNum, renderContext, true);
            thisRenderer._restoreUnderline(spanComp, props.Font);
        }
    );
};

scada.scheme.DynamicTextRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;

    if (props.InCnlNum > 0) {
        var ShowValueKinds = scada.scheme.ShowValueKinds;
        var spanComp = component.dom;
        var spanText = spanComp.children();
        var cnlDataExt = renderContext.getCnlDataExt(props.InCnlNum);

        // show value of the appropriate input channel
        switch (props.ShowValue) {
            case ShowValueKinds.SHOW_WITH_UNIT:
                spanText.text(cnlDataExt.TextWithUnit);
                break;
            case ShowValueKinds.SHOW_WITHOUT_UNIT:
                spanText.text(cnlDataExt.Text);
                break;
        }

        // choose and set colors of the component
        var statusColor = cnlDataExt.Color;
        var isHovered = spanComp.is(":hover");

        var backColor = this.chooseColor(isHovered, props.BackColor, props.BackColorOnHover);
        var borderColor = this.chooseColor(isHovered, props.BorderColor, props.BorderColorOnHover);
        var foreColor = this.chooseColor(isHovered, props.ForeColor, props.ForeColorOnHover);

        this.setBackColor(spanComp, backColor, true, statusColor);
        this.setBorderColor(spanComp, borderColor, true, statusColor);
        this.setForeColor(spanComp, foreColor, true, statusColor);
    }
};

/********** Static Picture Renderer **********/

// Static picture renderer type extends scada.scheme.ComponentRenderer
scada.scheme.StaticPictureRenderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.StaticPictureRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.StaticPictureRenderer.constructor = scada.scheme.StaticPictureRenderer;

scada.scheme.StaticPictureRenderer.prototype.createDom = function (component, renderContext) {
    var ImageStretches = scada.scheme.ImageStretches;
    var props = component.props;

    var divComp = $("<div id='comp" + component.id + "'></div>");
    this.prepareComponent(divComp, component);

    // set image
    switch (props.ImageStretch) {
        case ImageStretches.FILL:
            var borders = props.BorderWidth * 2;
            divComp.css("background-size", props.Size.Width - borders + "px " +
                (props.Size.Height - borders) + "px");
            break;
        case ImageStretches.ZOOM:
            divComp.css("background-size", "contain");
            break;
    }

    divComp.css({
        "background-repeat": "no-repeat",
        "background-position": "center",
        "background-clip": "padding-box"
    });

    var image = renderContext.getImage(props.ImageName);
    this.setBackgroundImage(divComp, image);

    component.dom = divComp;
};

scada.scheme.StaticPictureRenderer.prototype.refreshImages = function (component, renderContext, imageNames) {
    var props = component.props;

    if (Array.isArray(imageNames) && imageNames.includes(props.ImageName)) {
        var divComp = component.dom;
        var image = renderContext.getImage(props.ImageName);
        this.setBackgroundImage(divComp, image, true);
    }
};

scada.scheme.StaticPictureRenderer.prototype.setSize = function (component, width, height) {
    scada.scheme.ComponentRenderer.prototype.setSize.call(this, component, width, height);

    var ImageStretches = scada.scheme.ImageStretches;
    var props = component.props;

    if (props.ImageStretch === ImageStretches.FILL) {
        var divComp = component.dom;
        divComp.css("background-size", props.Size.Width + "px " + props.Size.Height + "px");
    }
};

/********** Dynamic Picture Renderer **********/

// Dynamic picture renderer type extends scada.scheme.StaticPictureRenderer
scada.scheme.DynamicPictureRenderer = function () {
    scada.scheme.StaticPictureRenderer.call(this);
};

scada.scheme.DynamicPictureRenderer.prototype = Object.create(scada.scheme.StaticPictureRenderer.prototype);
scada.scheme.DynamicPictureRenderer.constructor = scada.scheme.DynamicPictureRenderer;

scada.scheme.DynamicPictureRenderer.prototype.createDom = function (component, renderContext) {
    scada.scheme.StaticPictureRenderer.prototype.createDom.call(this, component, renderContext);

    var props = component.props;
    var divComp = component.dom;

    this.bindAction(divComp, component, renderContext);

    // apply properties on hover
    var thisRenderer = this;
    var cnlNum = props.InCnlNum;

    divComp.hover(
        function () {
            thisRenderer.setDynamicBackColor(divComp, props.BackColorOnHover, cnlNum, renderContext);
            thisRenderer.setDynamicBorderColor(divComp, props.BorderColorOnHover, cnlNum, renderContext);

            if (cnlNum <= 0) {
                var image = renderContext.getImage(props.ImageOnHoverName);
                thisRenderer.setBackgroundImage(divComp, image);
            }
        },
        function () {
            thisRenderer.setDynamicBackColor(divComp, props.BackColor, cnlNum, renderContext, true);
            thisRenderer.setDynamicBorderColor(divComp, props.BorderColor, cnlNum, renderContext, true);

            if (cnlNum <= 0) {
                var image = renderContext.getImage(props.ImageName);
                thisRenderer.setBackgroundImage(divComp, image, true);
            }
        }
    );
};

scada.scheme.DynamicPictureRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;

    if (props.InCnlNum > 0) {
        var divComp = component.dom;
        var cnlDataExt = renderContext.getCnlDataExt(props.InCnlNum);
        var imageName = props.ImageName;

        // choose an image depending on the conditions
        if (cnlDataExt.Stat && props.Conditions) {
            var cnlVal = cnlDataExt.Val;

            for (var cond of props.Conditions) {
                if (scada.scheme.calc.conditionSatisfied(cond, cnlVal)) {
                    imageName = cond.ImageName;
                    break;
                }
            }
        }

        // set the image
        var image = renderContext.imageMap.get(imageName);
        this.setBackgroundImage(divComp, image, true);

        // choose and set colors of the component
        var statusColor = cnlDataExt.Color;
        var isHovered = divComp.is(":hover");

        var backColor = this.chooseColor(isHovered, props.BackColor, props.BackColorOnHover);
        var borderColor = this.chooseColor(isHovered, props.BorderColor, props.BorderColorOnHover);

        this.setBackColor(divComp, backColor, true, statusColor);
        this.setBorderColor(divComp, borderColor, true, statusColor);
    }
};

/********** Unknown Component Renderer **********/

scada.scheme.UnknownComponentRenderer = function () {
    scada.scheme.Renderer.call(this);
};

scada.scheme.UnknownComponentRenderer.prototype = Object.create(scada.scheme.Renderer.prototype);
scada.scheme.UnknownComponentRenderer.constructor = scada.scheme.UnknownComponentRenderer;

/********** Render Context **********/

// Render context type
scada.scheme.RenderContext = function () {
    this.curCnlDataMap = null;
    this.editMode = false;
    this.schemeEnv = null;
    this.viewID = 0;
    this.imageMap = null;
    this.controlRight = true;
};

// Get defined channel data even if they don't exist in the map
scada.scheme.RenderContext.prototype.getCnlDataExt = function (cnlNum) {
    var curCnlDataExt = this.curCnlDataMap.get(cnlNum);

    if (!curCnlDataExt) {
        curCnlDataExt = new scada.CnlDataExt();
        var renderer = new scada.scheme.Renderer();
        curCnlDataExt.Color = renderer.STATUS_DISPLAY_COLOR;
    }

    return curCnlDataExt;
};

// Get scheme image object by the image name
scada.scheme.RenderContext.prototype.getImage = function (imageName) {
    return this.imageMap.get(imageName);
};

/********** Renderer Map **********/

// Renderer map object
scada.scheme.rendererMap = new Map([
    ["Scada.Scheme.Model.StaticText", new scada.scheme.StaticTextRenderer()],
    ["Scada.Scheme.Model.DynamicText", new scada.scheme.DynamicTextRenderer()],
    ["Scada.Scheme.Model.StaticPicture", new scada.scheme.StaticPictureRenderer()],
    ["Scada.Scheme.Model.DynamicPicture", new scada.scheme.DynamicPictureRenderer()],
    ["Scada.Scheme.Model.UnknownComponent", new scada.scheme.UnknownComponentRenderer()]
]);
