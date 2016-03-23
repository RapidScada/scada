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
};

// Set font of the jQuery object
scada.scheme.Renderer.prototype.setFont = function (jqObj, font) {
    jqObj.css({
        "font-family": font.Name,
        "font-size": font.Size + "px",
        "font-weight": font.Bold ? "bold" : "normal",
        "font-style": font.Italic ? "italic" : "normal",
        "text-decoration": font.Underline ? "underline" : "none"
    });
};

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
        $("<div id='divScheme'></div>")
        .css({
            "position": "relative", // to position scheme elements
            "background-color": props.BackColor,
            "color": props.ForeColor,
            "width": props.Size.Width,
            "height": props.Size.Height,
        });

    this.setFont(divScheme, props.Font);

    // set background if present
    var backImage = renderContext.imageMap.get(elem.props.BackImage.Name);

    if (backImage) {
        divScheme.css({
            "background-image": "url('data:image/png;base64," + backImage.Data + "')",
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
    elem.dom = $("<div id='divElem" + elem.id + "'></div>")
        .text(props.Text)
        .css({
            "position": "absolute",
            "left": props.Location.X,
            "top": props.Location.Y
        });
};

/********** Dynamic Text Renderer **********/

scada.scheme.DynamicTextRenderer = function () {
    scada.scheme.StaticTextRenderer.call(this);
};

scada.scheme.DynamicTextRenderer.prototype = Object.create(scada.scheme.StaticTextRenderer.prototype);
scada.scheme.DynamicTextRenderer.constructor = scada.scheme.DynamicTextRenderer;

scada.scheme.DynamicTextRenderer.prototype.createDom = function (elem, renderContext) {
    scada.scheme.StaticTextRenderer.prototype.createDom.call(this, elem, renderContext);
};

scada.scheme.DynamicTextRenderer.prototype.update = function (elem, renderContext) {
    elem.dom.text(renderContext.getCurCnlTextWithUnit(elem.props.InCnlNum));
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
            "left": props.Location.X,
            "top": props.Location.Y,
            "width": props.Size.Width,
            "height": props.Size.Height,
            "border": "1px solid gray"
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
        imgElem.attr("src", "data:image/png;base64," + image.Data);
        //imgElem.css("background-image", "url('data:image/png;base64," + image.Data + "')");
    }

    elem.dom = divElem.append(imgElem);
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

// Get current input channel text with unit by channel number
scada.scheme.RenderContext.prototype.getCurCnlTextWithUnit = function (cnlNum) {
    var curCnlDataExt = this.curCnlDataMap.get(cnlNum);
    return curCnlDataExt ? curCnlDataExt.TextWithUnit : "";
}

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
