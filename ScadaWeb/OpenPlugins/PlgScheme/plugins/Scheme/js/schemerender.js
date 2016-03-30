/*
 * Scheme rendering
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

/*
 * Requires:
 * - jquery
 * - clientapi.js
 * - schemecommon.js
 */

// Rapid SCADA namespace
var scada = scada || {};
// Scheme namespace
scada.scheme = scada.scheme || {};

/********** Renderer **********/

// Renderer type
scada.scheme.Renderer = function () {
    // Color bound to an input channel status
    this.STATUS_COLOR = "Status";
    // Default value of the status color
    this.DEF_STATUS_COLOR = "Silver";
    // Element border width if border presents
    this.BORDER_WIDTH = 1;
};

// Set fore color of the jQuery object
scada.scheme.Renderer.prototype.setForeColor = function (jqObj, foreColor, opt_removeIfEmpty) {
    if (foreColor) {
        jqObj.css("color", foreColor == this.STATUS_COLOR ? this.DEF_STATUS_COLOR : foreColor);
    } else if (opt_removeIfEmpty) {
        jqObj.css("color", "");
    }
};

// Set background color of the jQuery object
scada.scheme.Renderer.prototype.setBackColor = function (jqObj, backColor, opt_removeIfEmpty) {
    if (backColor) {
        jqObj.css("background-color", backColor == this.STATUS_COLOR ? this.DEF_STATUS_COLOR : backColor);
    } else if (opt_removeIfEmpty) {
        jqObj.css("background-color", "");
    }
};

// Set border color and width of the jQuery object
scada.scheme.Renderer.prototype.setBorderColor = function (jqObj, borderColor) {
    jqObj.css({
        "border-width": this.BORDER_WIDTH,
        "border-style": "solid",
        "border-color": borderColor ? borderColor == this.STATUS_COLOR ? this.DEF_STATUS_COLOR : borderColor : "transparent"
    });
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

// Set text wrapping of the jQuery object
scada.scheme.Renderer.prototype.setWordWrap = function (jqObj, wordWrap) {
    jqObj.css("white-space", wordWrap ? "normal" : "nowrap");
};

// Set horizontal algnment of the jQuery object
scada.scheme.Renderer.prototype.setHAlign = function (jqObj, hAlign) {
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
scada.scheme.Renderer.prototype.setVAlign = function (jqObj, vAlign) {
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

// Returns a data URI containing a representation of the image
scada.scheme.Renderer.prototype.imageToDataURL = function (image) {
    return "data:" + (image.MediaType ? image.MediaType : "image/png") + ";base64," + image.Data
}

// Create DOM content of the element according to element properties.
// If element properties are incorrect, no DOM content is created
scada.scheme.Renderer.prototype.createDom = function (elem, renderContext) {
};

// Update the element using its existing DOM content
scada.scheme.Renderer.prototype.update = function (elem, renderContext) {
};

/********** Scheme Renderer **********/

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

    this.setBackColor(divScheme, props.BackColor);
    this.setFont(divScheme, props.Font);
    this.setForeColor(divScheme, props.ForeColor);

    // set background image if presents
    var backImage = renderContext.imageMap.get(elem.props.BackImage.Name);
    if (backImage) {
        divScheme.css({
            "background-image": "url('" + this.imageToDataURL(backImage) + "')",
            "background-size": props.Size.Width + "px " + props.Size.Height + "px",
            "background-repeat": "no-repeat"
        });
    }

    elem.dom = divScheme;
};

/********** Static Text Renderer **********/

scada.scheme.StaticTextRenderer = function () {
    scada.scheme.Renderer.call(this);
};

scada.scheme.StaticTextRenderer.prototype = Object.create(scada.scheme.Renderer.prototype);
scada.scheme.StaticTextRenderer.constructor = scada.scheme.StaticTextRenderer;

scada.scheme.StaticTextRenderer.prototype.createDom = function (elem, renderContext) {
    var props = elem.props;
    var spanElem =
        $("<span id='elem" + elem.id + "'></span>")
        .css({
            "position": "absolute",
            "z-index": props.ZIndex,
            "left": props.Location.X - this.BORDER_WIDTH,
            "top": props.Location.Y - this.BORDER_WIDTH
        });

    var spanText = $("<span></span>");
    spanElem.append(spanText);
    
    this.setBackColor(spanElem, props.BackColor);
    this.setBorderColor(spanElem, props.BorderColor);
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
    elem.dom = spanElem;
};

/********** Dynamic Text Renderer **********/

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

    var Actions = scada.scheme.Actions;
    var props = elem.props;
    var spanElem = elem.dom;
    var spanText = spanElem.children();

    // set tooltip
    if (props.ToolTip) {
        spanElem.prop("title", props.ToolTip);
    }

    // apply properties on hover
    var thisRenderer = this;
    spanElem.hover(
        function () {
            thisRenderer.setBackColor(spanElem, props.BackColorOnHover);
            thisRenderer.setBorderColor(spanElem, props.BorderColorOnHover);
            thisRenderer.setForeColor(spanElem, props.ForeColorOnHover);
            thisRenderer._setUnderline(spanElem, props.UnderlineOnHover);
        },
        function () {
            thisRenderer.setBackColor(spanElem, props.BackColor, true);
            thisRenderer.setBorderColor(spanElem, props.BorderColor);
            thisRenderer.setForeColor(spanElem, props.ForeColor, true);
            thisRenderer._restoreUnderline(spanElem, props.Font);
        }
    );

    // bind action
    if (props.Action) {
        spanElem.css("cursor", "pointer");

        spanElem.click(function () {
            switch (props.Action) {
                case Actions.DRAW_DIAGRAM:
                    alert("Draw diagramm of the input channel " + props.InCnlNum); // TODO: use SCADA API
                    break;
                case Actions.SEND_COMMAND:
                    alert("Send command for the output channel " + props.OutCnlNum); // TODO: use SCADA API
                    break;
            }
        });
    }
};

scada.scheme.DynamicTextRenderer.prototype.update = function (elem, renderContext) {
    var curCnlDataExt = renderContext.curCnlDataMap.get(elem.props.InCnlNum);
    var spanElem = elem.dom;
    var spanText = spanElem.children();

    if (curCnlDataExt) {
        spanText.text(curCnlDataExt.TextWithUnit);

        if (elem.props.ForeColor == this.STATUS_COLOR) {
            spanElem.css("color", curCnlDataExt.Color);
        }
    } else {
        spanText.text("");
    }
};

/********** Static Picture Renderer **********/

scada.scheme.StaticPictureRenderer = function () {
    scada.scheme.Renderer.call(this);
};

scada.scheme.StaticPictureRenderer.prototype = Object.create(scada.scheme.Renderer.prototype);
scada.scheme.StaticPictureRenderer.constructor = scada.scheme.StaticPictureRenderer;

scada.scheme.StaticPictureRenderer.prototype.createDom = function (elem, renderContext) {
    var props = elem.props;

    var divElem =
        $("<div id='divElem" + elem.id + "'></div>")
        .css({
            "position": "absolute",
            "z-index": props.ZIndex,
            "left": props.Location.X - this.BORDER_WIDTH,
            "top": props.Location.Y - this.BORDER_WIDTH,
            "width": props.Size.Width,
            "height": props.Size.Height
        });

    var imgElem =
        $("<img alt='test image' />")
        .css({
            "width": props.Size.Width,
            "height": props.Size.Height,
            "border": 0
        });

    var image = renderContext.imageMap.get(elem.props.Image.Name);
    if (image) {
        imgElem.attr("src", this.imageToDataURL(image));
    }

    divElem.append(imgElem);
    elem.dom = divElem;
};

/********** Dynamic Picture Renderer **********/

scada.scheme.DynamicPictureRenderer = function () {
    scada.scheme.StaticPictureRenderer.call(this);
};

scada.scheme.DynamicPictureRenderer.prototype = Object.create(scada.scheme.StaticPictureRenderer.prototype);
scada.scheme.DynamicPictureRenderer.constructor = scada.scheme.DynamicPictureRenderer;

scada.scheme.DynamicPictureRenderer.prototype.createDom = function (elem, renderContext) {
    scada.scheme.StaticPictureRenderer.prototype.createDom.call(this, elem, renderContext);
};

scada.scheme.DynamicPictureRenderer.prototype.update = function (elem, renderContext) {
    var cnlDataExt = renderContext.curCnlDataMap.get(elem.props.InCnlNum);
    if (cnlDataExt) {
        // choose the image depending on the conditions
        var imageName = elem.props.Image.Name;

        if (cnlDataExt.Stat && elem.props.Conditions) {
            var cnlVal = cnlDataExt.Val;

            for (var cond of elem.props.Conditions) {
                if (scada.scheme.utils.conditionSatisfied(cond, cnlVal)) {
                    imageName = cond.Image.Name;
                    break;
                }
            }
        }

        // set the image
        var image = renderContext.imageMap.get(imageName);
        if (image) {
            elem.dom.find("img").attr("src", "data:image/png;base64," + image.Data);
        }
    }
};

/********** Render Context **********/

// Render context type
scada.scheme.RenderContext = function () {
    this.curCnlDataMap = null;
    this.imageMap = null;
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
