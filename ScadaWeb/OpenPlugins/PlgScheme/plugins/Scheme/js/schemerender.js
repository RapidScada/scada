// Rapid SCADA namespace
var scada = scada || {};
// Scheme namespace
scada.scheme = scada.scheme || {};

/********** Renderer **********/

// Renderer type
scada.scheme.Renderer = function () {
};

// Validate element type and element properies.
// If an element is not validated by all the available renderers, it is ignored
scada.scheme.Renderer.prototype.validate = function (element) {
};

// Create DOM content of the element based on element properties
scada.scheme.Renderer.prototype.createDom = function (element) {
};

// Render the element
scada.scheme.Renderer.prototype.render = function (element, scadaApi) {
};

/********** Static Text Renderer **********/

scada.scheme.StaticTextRenderer = function () {
    scada.scheme.Renderer.call(this);
};

scada.scheme.StaticTextRenderer.prototype = Object.create(scada.scheme.Renderer.prototype);
scada.scheme.StaticTextRenderer.constructor = scada.scheme.StaticTextRenderer;

scada.scheme.StaticTextRenderer.prototype.validate = function (element) {
    return element.type && element.type == "StaticText" &&
        element.propsExist && element.propsExist(["width", "height", "text"]);
};
