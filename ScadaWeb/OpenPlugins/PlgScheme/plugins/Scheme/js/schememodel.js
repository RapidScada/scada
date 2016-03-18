/*
 * Scheme data model
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

/*
 * Requires:
 * - jquery
 * - scadautils.js
 * - clientapi.js
 * - schemerender.js
 */

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
    // jQuery objects representing DOM content
    this.dom = null;
    // Renderer of the element
    this.renderer = null;
};

/********** Scheme **********/

// Scheme type
scada.scheme.Scheme = function () {
    scada.scheme.BaseElement.call(this);

    // Count of elements received by a one request
    this.LOAD_ELEM_CNT = 100;

    // Scheme view ID
    this.viewID = 0;
    // Stamp of the view unique within application scope
    this.viewStamp = 0;
    // Elements of the scheme
    this.elements = [];
    // Binary objects of the scheme
    this.blobs = [];
};

scada.scheme.Scheme.prototype = Object.create(scada.scheme.BaseElement.prototype);
scada.scheme.Scheme.constructor = scada.scheme.Scheme;

// Parse received scheme data
scada.scheme.Scheme.prototype._parseReceivedData = function (json) {
    var getCurTime = scada.utils.getCurTime;

    try {
        var parsedData = $.parseJSON(json);

        if (typeof parsedData.ViewStamp === "undefined") {
            throw { message: "ViewStamp is missing." };
        }

        if (typeof parsedData.EndOfScheme === "undefined") {
            throw { message: "EndOfScheme is missing." };
        }

        if (typeof parsedData.Elements === "undefined") {
            throw { message: "Elements are missing." };
        }

        this.viewStamp = parsedData.ViewStamp;
        this._appendElements(parsedData.Elements);
        return parsedData;
    }
    catch (ex) {
        console.error(getCurTime() + " Error parsing scheme data:", ex.message);
        return null;
    }
};

// Append received elements to the scheme
scada.scheme.Scheme.prototype._appendElements = function (receivedElements) {
    for (var receivedElem of receivedElements) {
        if (typeof receivedElem === "undefined" ||
            typeof receivedElem.Type === "undefined" ||
            typeof receivedElem.ID === "undefined") {
            console.warn("The element is skipped because the required properties are missed");
            continue;
        }

        var schemeElem = new scada.scheme.Element();
        schemeElem.type = receivedElem.Type;
        schemeElem.props = receivedElem;

        try {
            schemeElem.renderer = scada.scheme.rendererFactory.createRenderer(schemeElem.type);
        }
        catch (ex) {
            console.error("Error creating renderer for the element of type '" + schemeElem.Type + "':", ex.message);
            continue;
        }

        if (schemeElem.renderer) {
            // add the element to the scheme
            this.elements.push(schemeElem);
        } else {
            console.warn("The element of type '" + schemeElem.Type +
                "' with ID=" + schemeElem.props.ID + " is not supported");
        }        
    }
};

// Clear scheme
scada.scheme.Scheme.prototype.clear = function () {
    this.viewID = 0;
    this.viewStamp = 0;
    this.elements = [];
    this.blobs = [];
};

// Load scheme.
// callback is function (success, complete)
scada.scheme.Scheme.prototype.load = function (viewID, callback) {
    if (this.viewID == 0) {
        this.viewID = viewID;
    } else if (this.viewID != viewID) {
        console.warn("Scheme view ID mismatch. The existing ID=" + this.viewID + ". The requsested ID=" + viewID);
        callback(false, false);
        return;
    }

    var operation = "SchemeSvc.svc/GetElements";
    var thisScheme = this;

    $.ajax({
        url: operation +
            "?viewID=" + viewID +
            "&viewStamp=" + this.viewStamp +
            "&startIndex=" + this.elements.length +
            "&count=" + this.LOAD_ELEM_CNT,
        method: "GET",
        dataType: "json"
    })
    .done(function (data, textStatus, jqXHR) {
        if (data.d) {
            scada.utils.logSuccessfulRequest(operation, data);
            var parsedData = thisScheme._parseReceivedData(data.d);
            if (parsedData) {
                callback(true, parsedData.EndOfScheme);
            } else {
                callback(false, false);
            }
        } else {
            scada.utils.logServiceError(operation);
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        scada.utils.logFailedRequest(operation, jqXHR);
        callback(false, false);
    })
    .always(function () {
    });
};

// Create DOM content of the scheme elements
scada.scheme.Scheme.prototype.createDom = function () {
    for (var elem of this.elements) {
        try {
            elem.renderer.createDom(elem);
            if (this.dom) {
                this.dom.append(elem.dom);
            }
        }
        catch (ex) {
            console.error("Error creating DOM content for the element of type '" +
                elem.Type + "' with ID=" + elem.props.ID + ":", ex.message);
            elem.dom = null;
        }
    }
};

// Update the scheme elements
scada.scheme.Scheme.prototype.update = function (clientAPI) {
    for (var elem of this.elements) {
        try {
            if (elem.dom)
                elem.renderer.update(elem, clientAPI);
        }
        catch (ex) {
            console.error("Error updating the element of type '" +
                elem.Type + "' with ID=" + elem.props.ID + ":", ex.message);
            elem.dom = null;
        }
    }
};

/********** Element **********/

// Element type
scada.scheme.Element = function () {
    scada.scheme.BaseElement.call(this);
};

scada.scheme.Element.prototype = Object.create(scada.scheme.BaseElement.prototype);
scada.scheme.Element.constructor = scada.scheme.Element;
