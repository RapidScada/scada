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
 */

// Rapid SCADA namespace
var scada = scada || {};
// Scheme namespace
scada.scheme = scada.scheme || {};

/********** Renderer **********/

// Renderer type
scada.scheme.Renderer = function () {
};

// Create DOM content of the element according to element properties.
// If element properties are incorrect, no DOM content is created
scada.scheme.Renderer.prototype.createDom = function (elem) {
};

// Update the element using its existing DOM content
scada.scheme.Renderer.prototype.update = function (elem, renderContext) {
};

/********** Static Text Renderer **********/

scada.scheme.StaticTextRenderer = function () {
    scada.scheme.Renderer.call(this);
};

scada.scheme.StaticTextRenderer.prototype = Object.create(scada.scheme.Renderer.prototype);
scada.scheme.StaticTextRenderer.constructor = scada.scheme.StaticTextRenderer;

scada.scheme.StaticTextRenderer.prototype.createDom = function (elem) {
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
    scada.scheme.Renderer.call(this);
};

scada.scheme.DynamicTextRenderer.prototype = Object.create(scada.scheme.StaticTextRenderer.prototype);
scada.scheme.DynamicTextRenderer.constructor = scada.scheme.DynamicTextRenderer;

scada.scheme.DynamicTextRenderer.prototype.createDom = function (elem) {
    scada.scheme.StaticTextRenderer.prototype.createDom.call(this, elem);
};

scada.scheme.DynamicTextRenderer.prototype.update = function (elem, renderContext) {
    elem.dom.text(renderContext.getCurCnlTextWithUnit(elem.props.InCnlNum));
};

/********** Render Context **********/

// Render context type
scada.scheme.RenderContext = function () {
    this.curCnlDataMap = null;
};

// Get extended current input channel data by channel number
scada.scheme.RenderContext.prototype.getCurCnlDataExt = function (cnlNum) {
    return this.curCnlDataMap.get(cnlNum);
}

// Get current input channel text with unit by channel number
scada.scheme.RenderContext.prototype.getCurCnlTextWithUnit = function (cnlNum) {
    var curCnlDataExt = this.getCurCnlDataExt(cnlNum);
    return curCnlDataExt ? curCnlDataExt.TextWithUnit : "";
}

/********** Renderer Factory **********/

// Renderer factory object
scada.scheme.rendererFactory = {
    map: new Map([
        ["Static text", scada.scheme.StaticTextRenderer],
        ["Dynamic text", scada.scheme.DynamicTextRenderer],
        ["Static picture", null],
        ["Dynamic picture", null]]),

    // Create new renderer according to the specified scheme element type
    createRenderer: function (elementType) {
        var RendererType = this.map.get(elementType);
        return RendererType ? new RendererType() : null;
    }
};
