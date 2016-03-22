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

/********** Scheme Loading States **********/

// Scheme loading states enumeration
scada.scheme.LoadStates = {
    UNDEFINED: 0,
    PROPS_LOADING: 1,
    ELEMS_LOADING: 2,
    BLOBS_LOADING: 3,
    COMPLETED: 4
};

/********** Scheme **********/

// Scheme type
scada.scheme.Scheme = function () {
    scada.scheme.BaseElement.call(this);

    // Count of elements received by a one request
    this.LOAD_ELEM_CNT = 100;
    // Maximum count of elements
    this.MAX_ELEM_CNT = 10000;

    // Scheme loading state
    this.loadState = scada.scheme.LoadStates.UNDEFINED;
    // Scheme view ID
    this.viewID = 0;
    // Stamp of the view unique within application scope
    this.viewStamp = 0;
    // Elements of the scheme
    this.elements = [];
    // Binary objects of the scheme
    this.blobs = [];
    // Parent jQuery object
    this.parentDomElem = null;
};

scada.scheme.Scheme.prototype = Object.create(scada.scheme.BaseElement.prototype);
scada.scheme.Scheme.constructor = scada.scheme.Scheme;

// Load scheme elements
// callback is function (success, complete)
scada.scheme.Scheme.prototype._loadElements = function (viewID, callback) {
    if (this.viewID == 0) {
        this.viewID = viewID;
    } else if (this.viewID != viewID) {
        console.warn(scada.utils.getCurTime() +
            " Scheme view ID mismatch. The existing ID=" + this.viewID + ". The requsested ID=" + viewID);
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
            var parsedElems = thisScheme._parseElements(data.d);
            if (parsedElems) {
                if (parsedElems.EndOfScheme) {
                    thisScheme.loadState = scada.scheme.LoadStates.BLOBS_LOADING;
                }
                callback(true, false);
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
    });
}

// Parse received scheme elements
scada.scheme.Scheme.prototype._parseElements = function (json) {
    var getCurTime = scada.utils.getCurTime;

    try {
        var parsedElems = $.parseJSON(json);

        if (typeof parsedElems.ViewStamp === "undefined") {
            throw { message: "ViewStamp is missing." };
        }

        if (typeof parsedElems.EndOfScheme === "undefined") {
            throw { message: "EndOfScheme is missing." };
        }

        if (typeof parsedElems.Elements === "undefined") {
            throw { message: "Elements are missing." };
        }

        if (this.viewStamp && parsedElems.ViewStamp &&
            this.viewStamp != parsedElems.ViewStamp) {
            console.warn(getCurTime() + " Scheme view stamp mismatch");
            return null;
        } else {
            this.viewStamp = parsedElems.ViewStamp;
            this._appendElements(parsedElems.Elements);
            return parsedElems;
        }
    }
    catch (ex) {
        console.error(getCurTime() + " Error parsing scheme elements:", ex.message);
        return null;
    }
};

// Append received elements to the scheme
scada.scheme.Scheme.prototype._appendElements = function (parsedElems) {
    for (var parsedElem of parsedElems) {
        if (typeof parsedElem === "undefined" ||
            typeof parsedElem.Type === "undefined" ||
            typeof parsedElem.ID === "undefined") {
            console.warn("The element is skipped because the required properties are missed");
            continue;
        }

        var schemeElem = new scada.scheme.Element();
        schemeElem.type = parsedElem.Type;
        schemeElem.id = parsedElem.ID;
        schemeElem.props = parsedElem;

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
            console.warn("The element of type '" + schemeElem.type +
                "' with ID=" + schemeElem.id + " is not supported");
        }
    }
};

// Create map of current input channel data to access by channel number
scada.scheme.Scheme.prototype._createCurCnlDataMap = function (cnlDataExtArr) {
    try {
        var map = new Map();
        for (var cnlDataExt of cnlDataExtArr) {
            map.set(cnlDataExt.CnlNum, cnlDataExt);
        }
        return map;
    }
    catch (ex) {
        console.error(scada.utils.getCurTime() + " Error creating map of current input channel data:", ex.message);
        return null;
    }
}

// Update the element using the current input channel data
scada.scheme.Scheme.prototype._updateElement = function (elem, renderContext) {
    try {
        if (elem.dom) {
            elem.renderer.update(elem, renderContext);
        }
    }
    catch (ex) {
        console.error("Error updating the element of type '" + elem.type + "' with ID=" + elem.id + ":", ex.message);
        elem.dom = null;
    }
};

// Clear scheme
scada.scheme.Scheme.prototype.clear = function () {
    this.loadState = scada.scheme.LoadStates.UNDEFINED;
    this.viewID = 0;
    this.viewStamp = 0;
    this.elements = [];
    this.blobs = [];
};

// Load scheme.
// callback is function (success, complete)
scada.scheme.Scheme.prototype.load = function (viewID, callback) {
    var LoadStates = scada.scheme.LoadStates;

    if (this.loadState == LoadStates.UNDEFINED) {
        this.loadState = LoadStates.PROPS_LOADING;
    }

    switch (this.loadState) {
        case LoadStates.PROPS_LOADING:
            console.log("Set ELEMS_LOADING state");
            this.loadState = LoadStates.ELEMS_LOADING;
            callback(true, false);
            break;
        case LoadStates.ELEMS_LOADING:
            this._loadElements(viewID, callback);
            break;
        case LoadStates.BLOBS_LOADING:
            console.log("Set COMPLETED state");
            this.loadState = LoadStates.COMPLETED;
            callback(true, true);
            break;
        case LoadStates.COMPLETED:
            console.warn("Scheme loading is already completed");
            callback(true, true);
            break;
        default:
            console.warn("Unknown scheme loading state " + this.loadState);
            callback(false, false);
            break;
    }
};

// Create DOM content of the scheme elements
scada.scheme.Scheme.prototype.createDom = function () {
    this.dom = this.parentDomElem;

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
    var thisScheme = this;

    clientAPI.getCurCnlDataExtByView(this.viewID, function (success, cnlDataExtArr) {
        if (success) {
            var curCnlDataMap = thisScheme._createCurCnlDataMap(cnlDataExtArr);

            if (curCnlDataMap) {
                var renderContext = new scada.scheme.RenderContext();
                renderContext.curCnlDataMap = curCnlDataMap;

                for (var elem of thisScheme.elements) {
                    thisScheme._updateElement(elem, renderContext);
                }
            }
        } else {
            // TODO
        }
    });
};

/********** Element **********/

// Element type
scada.scheme.Element = function () {
    scada.scheme.BaseElement.call(this);

    // Element ID
    this.id = 0;
};

scada.scheme.Element.prototype = Object.create(scada.scheme.BaseElement.prototype);
scada.scheme.Element.constructor = scada.scheme.Element;
