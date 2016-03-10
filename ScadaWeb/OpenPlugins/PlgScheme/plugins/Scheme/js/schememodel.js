// Rapid SCADA namespace
var scada = scada || {};
// Scheme namespace
scada.scheme = scada.scheme || {};

/********** Constants **********/

scada.scheme.const = {
    // Count of elements received by a one request
    LOAD_ELEM_CNT: 100
};

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

    // Stamp of the view unique within application scope
    this.viewStamp = 0;
    // Elements of the scheme
    this.elements = [];
    // Binary objects of the scheme
    this.blobs = [];
};

scada.scheme.Scheme.prototype = Object.create(scada.scheme.BaseElement.prototype);
scada.scheme.Scheme.constructor = scada.scheme.BaseElement;

// Clear scheme
scada.scheme.Scheme.prototype.clear = function () {
    this.viewStamp = 0;
    this.elements = [];
    this.blobs = [];
};

// Load scheme.
// callback is function (success, complete)
scada.scheme.Scheme.prototype.load = function (viewID, callback) {
    var request = $.ajax({
        url: "SchemeSvc.svc/GetElements" +
            "?viewID=" + viewID +
            "&viewStamp=0" +
            "&startIndex=0" +
            "&count=" + scada.scheme.const.LOAD_ELEM_CNT,
        method: "GET",
        dataType: "json"
    });

    requst.done(function (data, textStatus, jqXHR) {
        if (data.d == "") {
            console.log("Empty data received. Internal service error");
        } else {
            console.log("Data received successfully");
            console.log(data.d);
            var parsedData = $.parseJSON(data.d);
        }
    });

    requst.fail(function (jqXHR, textStatus, errorThrown) {
        console.log("Error: " + result.status + " " + result.statusText);
    });

    req.always(function () {
        // update scheme
    });

    //callback(true, true);
};

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