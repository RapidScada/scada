// Rapid SCADA namespace
var scada = scada || {};
// Scheme namespace
scada.scheme = scada.scheme || {};

// Scheme type
scada.scheme.Scheme = function () {
    // Elements of the scheme
    this.elements = [];
    // Binary objects of the scheme
    this.blobs = [];
};

// Scheme properies type
scada.scheme.SchemeProps = function () {
};

// Scheme element type
scada.scheme.Element = function () {
    this.elementProps = null;
};

// Scheme element properies type
scada.scheme.ElementProps = function () {
};