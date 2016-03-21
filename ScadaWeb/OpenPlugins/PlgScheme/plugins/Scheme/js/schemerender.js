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
scada.scheme.Renderer.prototype.createDom = function (element) {
};

// Update the element using its existing DOM content
scada.scheme.Renderer.prototype.update = function (element, clientAPI) {
};

/********** Static Text Renderer **********/

scada.scheme.StaticTextRenderer = function () {
    scada.scheme.Renderer.call(this);
};

scada.scheme.StaticTextRenderer.prototype = Object.create(scada.scheme.Renderer.prototype);
scada.scheme.StaticTextRenderer.constructor = scada.scheme.StaticTextRenderer;

scada.scheme.StaticTextRenderer.prototype.createDom = function (element) {
    var props = element.props;
    element.dom = $("<div id='divElem" + props.ID + "'></div>")
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

scada.scheme.DynamicTextRenderer.prototype.createDom = function (element) {
    scada.scheme.StaticTextRenderer.prototype.createDom.call(this, element);
};

scada.scheme.DynamicTextRenderer.prototype.update = function (element, clientAPI) {
    clientAPI.getCurCnlDataExt(element.props.InCnlNum, function (success, cnlData) {
        element.dom.text(success ? cnlData.TextWithUnit : "");
    });
};

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