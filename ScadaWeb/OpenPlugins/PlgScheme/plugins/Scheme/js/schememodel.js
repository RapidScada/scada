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
    IMAGES_LOADING: 3,
    COMPLETED: 4
};

/********** Scheme **********/

// Scheme type
scada.scheme.Scheme = function () {
    scada.scheme.BaseElement.call(this);
    this.renderer = new scada.scheme.SchemeRenderer();

    // Count of elements received by a one request
    this.LOAD_ELEM_CNT = 100;
    // Maximum count of elements
    this.MAX_ELEM_CNT = 10000;
    // Total data size of images received by a one request, 1 MB
    this.LOAD_IMG_SIZE = 1048576;

    // Expected element count
    this._expectedElemCnt = 0;
    // Expected image count
    this._expectedImageCnt = 0;

    // Scheme loading state
    this.loadState = scada.scheme.LoadStates.UNDEFINED;
    // Scheme view ID
    this.viewID = 0;
    // Stamp of the view, unique within application scope
    this.viewStamp = 0;
    // Scheme elements
    this.elements = [];
    // Scheme images
    this.images = [];
    // Scheme images
    this.imageMap = new Map();
    // Parent jQuery object
    this.parentDomElem = null;
};

scada.scheme.Scheme.prototype = Object.create(scada.scheme.BaseElement.prototype);
scada.scheme.Scheme.constructor = scada.scheme.Scheme;

// Load scheme properties
// callback is function (success, complete)
scada.scheme.Scheme.prototype._loadSchemeProps = function (viewID, callback) {
    var operation = "SchemeSvc.svc/GetSchemeProps";
    var thisScheme = this;

    $.ajax({
        url: operation +
            "?viewID=" + viewID +
            "&viewStamp=" + this.viewStamp,
        method: "GET",
        dataType: "json"
    })
    .done(function (data, textStatus, jqXHR) {
        if (data.d) {
            scada.utils.logSuccessfulRequest(operation/*, data*/);
            var parsedProps = thisScheme._parseSchemeProps(data.d);
            if (parsedProps) {
                thisScheme.loadState = scada.scheme.LoadStates.ELEMS_LOADING;
                callback(true, false);
            } else {
                callback(false, false);
            }
        } else {
            scada.utils.logServiceError(operation);
            callback(false, false);
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        scada.utils.logFailedRequest(operation, jqXHR);
        callback(false, false);
    });
};

// Parse received scheme properties
scada.scheme.Scheme.prototype._viewStampsMatched = function (viewStamp1, viewStamp2) {
    if (viewStamp1 && viewStamp2 && viewStamp1 != viewStamp2) {
        console.warn(scada.utils.getCurTime() + " Scheme view stamp mismatch");
        return false;
    } else {
        return true;
    }
};

// Parse received scheme properties
scada.scheme.Scheme.prototype._parseSchemeProps = function (json) {
    try {
        var parsedProps = $.parseJSON(json);

        if (typeof parsedProps.ViewStamp === "undefined") {
            throw { message: "ViewStamp property is missing." };
        }

        if (typeof parsedProps.SchemeProps === "undefined") {
            throw { message: "SchemeProps property is missing." };
        }

        if (typeof parsedProps.ElementCount === "undefined") {
            throw { message: "ElementCount property is missing." };
        }

        if (typeof parsedProps.ImageCount === "undefined") {
            throw { message: "ImageCount property is missing." };
        }

        if (this._viewStampsMatched(this.viewStamp, parsedProps.ViewStamp)) {
            this.type = parsedProps.Type;
            this.props = parsedProps.SchemeProps;
            this.viewStamp = parsedProps.ViewStamp;
            this._expectedElemCnl = parsedProps.ElementCount;
            this._expectedImageCnt = parsedProps.ImageCount;

            if (parsedProps.SchemeProps.Title) {
                document.title = parsedProps.SchemeProps.Title + " - Rapid SCADA";
            }

            return parsedProps;
        } else {
            return null;
        }
    }
    catch (ex) {
        console.error(scada.utils.getCurTime() + " Error parsing scheme properties:", ex.message);
        return null;
    }
};

// Load scheme elements
// callback is function (success, complete)
scada.scheme.Scheme.prototype._loadElements = function (viewID, callback) {
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
            scada.utils.logSuccessfulRequest(operation/*, data*/);
            var parsedElems = thisScheme._parseElements(data.d);
            if (parsedElems) {
                if (parsedElems.EndOfElements) {
                    thisScheme.loadState = scada.scheme.LoadStates.IMAGES_LOADING;
                }
                callback(true, false);
            } else {
                callback(false, false);
            }
        } else {
            scada.utils.logServiceError(operation);
            callback(false, false);
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        scada.utils.logFailedRequest(operation, jqXHR);
        callback(false, false);
    });
}

// Parse received scheme elements
scada.scheme.Scheme.prototype._parseElements = function (json) {
    try {
        var parsedElems = $.parseJSON(json);

        if (typeof parsedElems.ViewStamp === "undefined") {
            throw { message: "ViewStamp property is missing." };
        }

        if (typeof parsedElems.EndOfElements === "undefined") {
            throw { message: "EndOfElements property is missing." };
        }

        if (typeof parsedElems.Elements === "undefined") {
            throw { message: "Elements property is missing." };
        }

        if (this._viewStampsMatched(this.viewStamp, parsedElems.ViewStamp)) {
            this.viewStamp = parsedElems.ViewStamp;
            this._appendElements(parsedElems.Elements);
            return parsedElems;
        } else {
            return null;
        }
    }
    catch (ex) {
        console.error(scada.utils.getCurTime() + " Error parsing scheme elements:", ex.message);
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
        schemeElem.renderer = scada.scheme.rendererMap.getRenderer(schemeElem.type);

        if (schemeElem.renderer) {
            // add the element to the scheme
            this.elements.push(schemeElem);
        } else {
            console.warn("The element of type '" + schemeElem.type +
                "' with ID=" + schemeElem.id + " is not supported");
        }
    }
};

// Load scheme images
// callback is function (success, complete)
scada.scheme.Scheme.prototype._loadImages = function (viewID, callback) {
    var operation = "SchemeSvc.svc/GetImages";
    var thisScheme = this;

    $.ajax({
        url: operation +
            "?viewID=" + viewID +
            "&viewStamp=" + this.viewStamp +
            "&startIndex=" + this.images.length +
            "&totalDataSize=" + this.LOAD_IMG_SIZE,
        method: "GET",
        dataType: "json"
    })
    .done(function (data, textStatus, jqXHR) {
        if (data.d) {
            scada.utils.logSuccessfulRequest(operation/*, data*/);
            var parsedImages = thisScheme._parseImages(data.d);
            if (parsedImages) {
                if (parsedImages.EndOfImages) {
                    thisScheme.loadState = scada.scheme.LoadStates.COMPLETED;
                    callback(true, true);
                } else {
                    callback(true, false);
                }
            } else {
                callback(false, false);
            }
        } else {
            scada.utils.logServiceError(operation);
            callback(false, false);
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        scada.utils.logFailedRequest(operation, jqXHR);
        callback(false, false);
    });
}

// Parse received scheme images
scada.scheme.Scheme.prototype._parseImages = function (json) {
    try {
        var parsedImages = $.parseJSON(json);

        if (typeof parsedImages.ViewStamp === "undefined") {
            throw { message: "ViewStamp property is missing." };
        }

        if (typeof parsedImages.EndOfImages === "undefined") {
            throw { message: "EndOfImages property is missing." };
        }

        if (typeof parsedImages.Images === "undefined") {
            throw { message: "Images property is missing." };
        }

        if (this._viewStampsMatched(this.viewStamp, parsedImages.ViewStamp)) {
            this.viewStamp = parsedImages.ViewStamp;
            this._appendImages(parsedImages.Images);
            return parsedImages;
        } else {
            return null;
        }
    }
    catch (ex) {
        console.error(scada.utils.getCurTime() + " Error parsing scheme images:", ex.message);
        return null;
    }
};

// Append received images to the scheme
scada.scheme.Scheme.prototype._appendImages = function (parsedImages) {
    for (var parsedImage of parsedImages) {
        if (typeof parsedImage === "undefined" ||
            typeof parsedImage.Name === "undefined" ||
            typeof parsedImage.Data === "undefined") {
            console.warn("The image is skipped because the required properties are missed");
            continue;
        }

        // add the image to the scheme
        this.images.push(parsedImage);
        this.imageMap.set(parsedImage.Name, parsedImage);
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
    this.props = null;
    this.dom = null;

    this.loadState = scada.scheme.LoadStates.UNDEFINED;
    this.viewID = 0;
    this.viewStamp = 0;
    this.elements = [];
    this.images = [];
    this.imageMap = new Map();
};

// Load scheme.
// callback is function (success, complete)
scada.scheme.Scheme.prototype.load = function (viewID, callback) {
    var LoadStates = scada.scheme.LoadStates;

    if (this.viewID == 0) {
        this.viewID = viewID;
    } else if (this.viewID != viewID) {
        console.warn(scada.utils.getCurTime() +
            " Scheme view ID mismatch. The existing ID=" + this.viewID + ". The requsested ID=" + viewID);
        callback(false, false);
        return;
    }

    if (this.loadState == LoadStates.UNDEFINED) {
        this.loadState = LoadStates.PROPS_LOADING;
    }

    switch (this.loadState) {
        case LoadStates.PROPS_LOADING:
            this._loadSchemeProps(viewID, callback);
            break;
        case LoadStates.ELEMS_LOADING:
            this._loadElements(viewID, callback);
            break;
        case LoadStates.IMAGES_LOADING:
            this._loadImages(viewID, callback);
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
    var renderContext = new scada.scheme.RenderContext();
    renderContext.imageMap = this.imageMap;

    try
    {
        this.renderer.createDom(this, renderContext);

        if (this.dom) {
            this.parentDomElem.append(this.dom);
        } else {
            return;
        }
    }
    catch (ex) {
        console.error("Error creating DOM content for the scheme:", ex.message);
        this.dom = null;
        return;
    }

    for (var elem of this.elements) {
        try {
            elem.renderer.createDom(elem, renderContext);
            if (this.dom) {
                this.dom.append(elem.dom);
            }
        }
        catch (ex) {
            console.error("Error creating DOM content for the element of type '" +
                elem.type + "' with ID=" + elem.id + ":", ex.message);
            elem.dom = null;
        }
    }
};

// Update the scheme elements
// callback is function (success)
scada.scheme.Scheme.prototype.update = function (clientAPI, callback) {
    var thisScheme = this;

    clientAPI.getCurCnlDataExtByView(this.viewID, function (success, cnlDataExtArr) {
        if (success) {
            var curCnlDataMap = thisScheme._createCurCnlDataMap(cnlDataExtArr);

            if (curCnlDataMap) {
                var renderContext = new scada.scheme.RenderContext();
                renderContext.curCnlDataMap = curCnlDataMap;
                renderContext.imageMap = thisScheme.imageMap;

                for (var elem of thisScheme.elements) {
                    thisScheme._updateElement(elem, renderContext);
                }
            }

            callback(true);
        } else {
            callback(false);
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
