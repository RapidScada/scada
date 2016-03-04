// Rapid SCADA namespace
var scada = scada || {};
// Scheme namespace
scada.scheme = scada.scheme || {};

/********** Base Element **********/

// Parent base type of scheme elements
scada.scheme.BaseElement = function (type) {
    // Name of the element type
    this.type = type;
    // Element properties received from server. They are different depending on element type
    this.props = null;
    // jQuery objects representing DOM elements
    this.dom = null;
    // Renderer of the element
    this.renderer = null;
};

// Check that the specified properties are defined for the object
scada.scheme.BaseElement.prototype.propsExist = function (propNames) {
    return this.props != null;
};

/********** Scheme **********/

// Scheme type
scada.scheme.Scheme = function () {
    scada.scheme.BaseElement.call(this);

    // Elements of the scheme
    this.elements = [];
    // Binary objects of the scheme
    this.blobs = [];
};

scada.scheme.Scheme.prototype = Object.create(scada.scheme.BaseElement.prototype);
scada.scheme.Scheme.constructor = scada.scheme.BaseElement;

/********** Scheme Properties **********/

// Scheme properies type
scada.scheme.SchemeProps = function () {
};

/********** Element **********/

// Element type
scada.scheme.Element = function () {
    scada.scheme.BaseElement.call(this);
};

scada.scheme.Element.prototype = Object.create(scada.scheme.BaseElement.prototype);
scada.scheme.Element.constructor = scada.scheme.BaseElement;

/********** Element Properties **********/

// Element properies type
scada.scheme.ElementProps = function () {
};