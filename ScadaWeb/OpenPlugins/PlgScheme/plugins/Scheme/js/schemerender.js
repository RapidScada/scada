/*
 * Scheme rendering
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 *
 * Requires:
 * - jquery
 * - clientapi.js
 * - eventtypes.js
 * - schemecommon.js
 *
 * Inheritance hierarchy:
 * Renderer
 *   SchemeRenderer
 *   ElementRenderer
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

// Abstract parent type of renderers
scada.scheme.Renderer = function () {
    // Color bound to an input channel status
    this.STATUS_COLOR = "Status";
    // Default foreground color
    this.DEF_FORE_COLOR = "black";
    // Default background color
    this.DEF_BACK_COLOR = "transparent";
    // Default border color
    this.DEF_BORDER_COLOR = "transparent";
    // Element border width if border presents
    this.BORDER_WIDTH = 1;
};

// Set fore color of the jQuery object
scada.scheme.Renderer.prototype.setForeColor = function (jqObj, foreColor, opt_removeIfEmpty) {
    if (foreColor) {
        jqObj.css("color", foreColor == this.STATUS_COLOR ? this.DEF_FORE_COLOR : foreColor);
    } else if (opt_removeIfEmpty) {
        jqObj.css("color", "");
    }
};

// Set background color of the jQuery object
scada.scheme.Renderer.prototype.setBackColor = function (jqObj, backColor, opt_removeIfEmpty) {
    if (backColor) {
        jqObj.css("background-color", backColor == this.STATUS_COLOR ? this.DEF_BACK_COLOR : backColor);
    } else if (opt_removeIfEmpty) {
        jqObj.css("background-color", "");
    }
};

// Set border color of the jQuery object
scada.scheme.Renderer.prototype.setBorderColor = function (jqObj, borderColor, opt_removeIfEmpty) {
    if (borderColor) {
        jqObj.css("border-color", borderColor == this.STATUS_COLOR ? this.DEF_BORDER_COLOR : borderColor);
    } else if (opt_removeIfEmpty) {
        jqObj.css("border-color", "transparent");
    }
};

// Set font of the jQuery object
scada.scheme.Renderer.prototype.setFont = function (jqObj, font) {
    if (font) {
        jqObj.css({
            "font-family": font.Name,
            "font-size": font.Size + "px",
            "font-weight": font.Bold ? "bold" : "normal",
            "font-style": font.Italic ? "italic" : "normal",
            "text-decoration": font.Underline ? "underline" : "none"
        });
    }
};

// Returns a data URI containing a representation of the image
scada.scheme.Renderer.prototype.imageToDataURL = function (image) {
    return "data:" + (image.MediaType ? image.MediaType : "image/png") + ";base64," + image.Data
}

// Returns a css property value for the image data URI
scada.scheme.Renderer.prototype.imageToDataUrlCss = function (image) {
    return "url('" + this.imageToDataURL(image) + "')";
}

// Create DOM content of the element according to element properties.
// If element properties are incorrect, no DOM content is created
scada.scheme.Renderer.prototype.createDom = function (elem, renderContext) {
};

// Update the element using its existing DOM content
scada.scheme.Renderer.prototype.update = function (elem, renderContext) {
};

/********** Scheme Renderer **********/

// Scheme renderer type extends scada.scheme.Renderer
scada.scheme.SchemeRenderer = function () {
    scada.scheme.Renderer.call(this);
};

scada.scheme.SchemeRenderer.prototype = Object.create(scada.scheme.Renderer.prototype);
scada.scheme.SchemeRenderer.constructor = scada.scheme.SchemeRenderer;

scada.scheme.SchemeRenderer.prototype.createDom = function (elem, renderContext) {
    var props = elem.props; // scheme properties
    var divScheme =
        $("<div id='scheme'></div>")
        .css({
            "position": "relative", // to position scheme elements
            "width": props.Size.Width,
            "height": props.Size.Height
        });

    this.setBackColor($("body"), props.BackColor);
    this.setBackColor(divScheme, props.BackColor);
    this.setFont(divScheme, props.Font);
    this.setForeColor(divScheme, props.ForeColor);

    // set background image if presents
    var backImage = renderContext.getImage(elem.props.BackImage);
    if (backImage) {
        divScheme.css({
            "background-image": this.imageToDataUrlCss(backImage),
            "background-size": props.Size.Width + "px " + props.Size.Height + "px",
            "background-repeat": "no-repeat"
        });
    }

    // set title
    if (props.Title) {
        document.title = props.Title + " - Rapid SCADA";
        if (scada.scheme.viewHub) {
            scada.scheme.viewHub.notify(window, scada.EventTypes.VIEW_TITLE_CHANGED, document.title);
        }
    }

    elem.dom = divScheme;
};

/********** Element Renderer **********/

// Abstract element renderer type extends scada.scheme.Renderer
scada.scheme.ElementRenderer = function () {
    scada.scheme.Renderer.call(this);
};

scada.scheme.ElementRenderer.prototype = Object.create(scada.scheme.Renderer.prototype);
scada.scheme.ElementRenderer.constructor = scada.scheme.ElementRenderer;

// Get dynamic color that may depend on input channel status
scada.scheme.ElementRenderer.prototype._getDynamicColor = function (color, cnlNum, renderContext) {
    if (color) {
        if (color == this.STATUS_COLOR) {
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
scada.scheme.Renderer.prototype.setDynamicForeColor = function (jqObj, foreColor,
    cnlNum, renderContext, opt_removeIfEmpty) {
    this.setForeColor(jqObj, this._getDynamicColor(foreColor, cnlNum, renderContext), opt_removeIfEmpty);
};

// Set background color of the jQuery object that may depend on input channel status
scada.scheme.Renderer.prototype.setDynamicBackColor = function (jqObj, backColor,
    cnlNum, renderContext, opt_removeIfEmpty) {
    this.setBackColor(jqObj, this._getDynamicColor(backColor, cnlNum, renderContext), opt_removeIfEmpty);
};

// Set border color of the jQuery object that may depend on input channel status
scada.scheme.Renderer.prototype.setDynamicBorderColor = function (jqObj, borderColor,
    cnlNum, renderContext, opt_removeIfEmpty) {
    this.setBorderColor(jqObj, this._getDynamicColor(borderColor, cnlNum, renderContext), opt_removeIfEmpty);
};

// Choose a color according to hover state
scada.scheme.Renderer.prototype.chooseColor = function (isHovered, color, colorOnHover) {
    return isHovered && colorOnHover ? colorOnHover : color;
};

// Set text wrapping of the jQuery object
scada.scheme.ElementRenderer.prototype.setWordWrap = function (jqObj, wordWrap) {
    jqObj.css("white-space", wordWrap ? "normal" : "nowrap");
};

// Set horizontal algnment of the jQuery object
scada.scheme.ElementRenderer.prototype.setHAlign = function (jqObj, hAlign) {
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
scada.scheme.ElementRenderer.prototype.setVAlign = function (jqObj, vAlign) {
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
scada.scheme.ElementRenderer.prototype.setToolTip = function (jqObj, toolTip) {
    if (toolTip) {
        jqObj.prop("title", toolTip);
    }
};

// Set primary css properties of the jQuery object of the scheme element
scada.scheme.ElementRenderer.prototype.prepareElem = function (jqObj, elem, opt_setSize) {
    var props = elem.props;
    jqObj.css({
        "position": "absolute",
        "z-index": props.ZIndex,
        "left": props.Location.X - this.BORDER_WIDTH,
        "top": props.Location.Y - this.BORDER_WIDTH,
        "border-width": this.BORDER_WIDTH,
        "border-style": "solid"
    });

    if (opt_setSize) {
        jqObj.css({
            "width": props.Size.Width,
            "height": props.Size.Height
        });
    }
};

// Bind user action to the element
scada.scheme.ElementRenderer.prototype.bindAction = function (jqObj, elem) {
    var Actions = scada.scheme.Actions;
    var props = elem.props;

    if (props.Action) {
        var dialogs = scada.scheme.viewHub ? scada.scheme.viewHub.dialogs : null;

        jqObj
        .css("cursor", "pointer")
        .click(function () {
            switch (props.Action) {
                case Actions.DRAW_DIAGRAM:
                    if (props.InCnlNum > 0) {
                        if (dialogs) {
                            dialogs.showChart(scada.scheme.viewHub.currentViewID, props.InCnlNum);
                        } else {
                            console.warn("Unable to show chart because viewHub.dialogs is undefined");
                        }
                    }
                    break;
                case Actions.SEND_COMMAND:
                    if (props.CtrlCnlNum > 0) {
                        if (dialogs) {
                            dialogs.showCmd(scada.scheme.viewHub.currentViewID, props.CtrlCnlNum);
                        } else {
                            console.warn("Unable to show command dialog because viewHub.dialogs is undefined");
                        }
                    }
                    break;
            }
        });
    }
};

/********** Static Text Renderer **********/

// Static text renderer type extends scada.scheme.ElementRenderer
scada.scheme.StaticTextRenderer = function () {
    scada.scheme.ElementRenderer.call(this);
};

scada.scheme.StaticTextRenderer.prototype = Object.create(scada.scheme.ElementRenderer.prototype);
scada.scheme.StaticTextRenderer.constructor = scada.scheme.StaticTextRenderer;

scada.scheme.StaticTextRenderer.prototype.createDom = function (elem, renderContext) {
    var props = elem.props;

    var spanElem = $("<span id='elem" + elem.id + "'></span>");
    var spanText = $("<span></span>");

    this.prepareElem(spanElem, elem);
    this.setBackColor(spanElem, props.BackColor);
    this.setBorderColor(spanElem, props.BorderColor, true);
    this.setFont(spanElem, props.Font);
    this.setForeColor(spanElem, props.ForeColor);

    if (!props.AutoSize) {
        spanElem.css("display", "table");

        spanText
            .css({
                "display": "table-cell",
                "overflow": "hidden",
                "max-width": props.Size.Width,
                "width": props.Size.Width,
                "height": props.Size.Height
            });

        this.setHAlign(spanElem, props.HAlign);
        this.setVAlign(spanText, props.VAlign);
        this.setWordWrap(spanText, props.WordWrap);
    }

    spanText.text(props.Text);
    spanElem.append(spanText);
    elem.dom = spanElem;
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

scada.scheme.DynamicTextRenderer.prototype.createDom = function (elem, renderContext) {
    scada.scheme.StaticTextRenderer.prototype.createDom.call(this, elem, renderContext);

    var props = elem.props;
    var spanElem = elem.dom.first();
    var spanText = elem.dom.children();

    this.setToolTip(spanElem, props.ToolTip);
    this.bindAction(spanElem, elem);

    // apply properties on hover
    var thisRenderer = this;
    var cnlNum = props.InCnlNum;

    spanElem.hover(
        function () {
            thisRenderer.setDynamicBackColor(spanElem, props.BackColorOnHover, cnlNum, renderContext);
            thisRenderer.setDynamicBorderColor(spanElem, props.BorderColorOnHover, cnlNum, renderContext);
            thisRenderer.setDynamicForeColor(spanElem, props.ForeColorOnHover, cnlNum, renderContext);
            thisRenderer._setUnderline(spanElem, props.UnderlineOnHover);
        },
        function () {
            thisRenderer.setDynamicBackColor(spanElem, props.BackColor, cnlNum, renderContext, true);
            thisRenderer.setDynamicBorderColor(spanElem, props.BorderColor, cnlNum, renderContext, true);
            thisRenderer.setDynamicForeColor(spanElem, props.ForeColor, cnlNum, renderContext, true);
            thisRenderer._restoreUnderline(spanElem, props.Font);
        }
    );
};

scada.scheme.DynamicTextRenderer.prototype.update = function (elem, renderContext) {
    var ShowValueKinds = scada.scheme.ShowValueKinds;
    var props = elem.props;

    var curCnlDataExt = renderContext.curCnlDataMap.get(props.InCnlNum);
    var spanElem = elem.dom;
    var spanText = spanElem.children();

    if (curCnlDataExt) {
        // show value of the appropriate input channel
        switch (props.ShowValue) {
            case ShowValueKinds.SHOW_WITH_UNIT:
                spanText.text(curCnlDataExt.TextWithUnit);
                break;
            case ShowValueKinds.SHOW_WITHOUT_UNIT:
                spanText.text(curCnlDataExt.Text);
                break;
        }

        // choose and set colors of the element
        var isHovered = spanElem.is(":hover");
        var backColor = this.chooseColor(isHovered, props.BackColor, props.BackColorOnHover);
        var borderColor = this.chooseColor(isHovered, props.BorderColor, props.BorderColorOnHover);
        var foreColor = this.chooseColor(isHovered, props.ForeColor, props.ForeColorOnHover);

        if (backColor == this.STATUS_COLOR) {
            spanElem.css("background-color", curCnlDataExt.Color);
        }

        if (borderColor == this.STATUS_COLOR) {
            spanElem.css("border-color", curCnlDataExt.Color);
        }

        if (foreColor == this.STATUS_COLOR) {
            spanElem.css("color", curCnlDataExt.Color);
        }
    } else {
        spanText.text("");
    }
};

/********** Static Picture Renderer **********/

// Static picture renderer type extends scada.scheme.ElementRenderer
scada.scheme.StaticPictureRenderer = function () {
    scada.scheme.ElementRenderer.call(this);
};

scada.scheme.StaticPictureRenderer.prototype = Object.create(scada.scheme.ElementRenderer.prototype);
scada.scheme.StaticPictureRenderer.constructor = scada.scheme.StaticPictureRenderer;

scada.scheme.Renderer.prototype.setBackgroundImage = function (jqObj, image, opt_removeIfEmpty) {
    if (image) {
        jqObj.css("background-image", this.imageToDataUrlCss(image));
    } else if (opt_removeIfEmpty) {
        jqObj.css("background-image", "");
    }
};

scada.scheme.StaticPictureRenderer.prototype.createDom = function (elem, renderContext) {
    var ImageStretches = scada.scheme.ImageStretches;
    var props = elem.props;

    var divElem = $("<div id='divElem" + elem.id + "'></div>");
    this.prepareElem(divElem, elem, true);
    this.setBorderColor(divElem, props.BorderColor, true);

    // set image
    switch (props.ImageStretch) {
        case ImageStretches.FILL:
            divElem.css("background-size", props.Size.Width + "px " + props.Size.Height + "px");
            break;
        case ImageStretches.ZOOM:
            divElem.css({
                "background-size": "contain",
                "background-position": "center"
            });
            break;
    }

    divElem.css("background-repeat", "no-repeat");
    var image = renderContext.getImage(props.Image);
    this.setBackgroundImage(divElem, image);

    elem.dom = divElem;
};

/********** Dynamic Picture Renderer **********/

// Dynamic picture renderer type extends scada.scheme.StaticPictureRenderer
scada.scheme.DynamicPictureRenderer = function () {
    scada.scheme.StaticPictureRenderer.call(this);
};

scada.scheme.DynamicPictureRenderer.prototype = Object.create(scada.scheme.StaticPictureRenderer.prototype);
scada.scheme.DynamicPictureRenderer.constructor = scada.scheme.DynamicPictureRenderer;

scada.scheme.DynamicPictureRenderer.prototype.createDom = function (elem, renderContext) {
    scada.scheme.StaticPictureRenderer.prototype.createDom.call(this, elem, renderContext);

    var props = elem.props;
    var divElem = elem.dom;

    this.setToolTip(divElem, props.ToolTip);
    this.bindAction(divElem, elem);

    // apply properties on hover
    var thisRenderer = this;
    var cnlNum = props.InCnlNum;

    divElem.hover(
        function () {
            thisRenderer.setDynamicBorderColor(divElem, props.BorderColorOnHover, cnlNum, renderContext);

            if (cnlNum <= 0) {
                var image = renderContext.getImage(props.ImageOnHover);
                thisRenderer.setBackgroundImage(divElem, image);
            }
        },
        function () {
            thisRenderer.setDynamicBorderColor(divElem, props.BorderColor, cnlNum, renderContext, true);

            if (cnlNum <= 0) {
                var image = renderContext.getImage(props.Image);
                thisRenderer.setBackgroundImage(divElem, image, true);
            }
        }
    );
};

scada.scheme.DynamicPictureRenderer.prototype.update = function (elem, renderContext) {
    var props = elem.props;
    var divElem = elem.dom;
    var curCnlDataExt = renderContext.curCnlDataMap.get(props.InCnlNum);

    if (curCnlDataExt) {
        // choose the image depending on the conditions
        var imageName = props.Image.Name;

        if (curCnlDataExt.Stat && props.Conditions) {
            var cnlVal = curCnlDataExt.Val;

            for (var cond of props.Conditions) {
                if (scada.scheme.calc.conditionSatisfied(cond, cnlVal)) {
                    imageName = cond.Image.Name;
                    break;
                }
            }
        }

        // set the image
        var image = renderContext.imageMap.get(imageName);
        this.setBackgroundImage(divElem, image, true);

        // set border color
        var borderColor = this.chooseColor(divElem.is(":hover"), props.BorderColor, props.BorderColorOnHover);

        if (borderColor == this.STATUS_COLOR) {
            divElem.css("border-color", curCnlDataExt.Color);
        }
    }
};

/********** Render Context **********/

// Render context type
scada.scheme.RenderContext = function () {
    this.curCnlDataMap = null;
    this.imageMap = null;
};

// Get scheme image object by image property of an element
scada.scheme.RenderContext.prototype.getImage = function (imageProp) {
    return imageProp ? this.imageMap.get(imageProp.Name) : null;
};

/********** Renderer Map **********/

// Renderer map object
scada.scheme.rendererMap = {
    map: new Map([
        ["Static text", new scada.scheme.StaticTextRenderer()],
        ["Dynamic text", new scada.scheme.DynamicTextRenderer()],
        ["Static picture", new scada.scheme.StaticPictureRenderer()],
        ["Dynamic picture", new scada.scheme.DynamicPictureRenderer()]]),

    // Get renderer according to the specified scheme element type
    getRenderer: function (elemType) {
        return this.map.get(elemType);
    }
};
