/*
 * Scheme data model
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2017
 *
 * Requires:
 * - jquery
 * - utils.js
 * - schemerender.js
 *
 * At runtime:
 * - clientapi.js
 *
 * Inheritance hierarchy:
 * BaseComponent
 *   Scheme
 *   Component
 */

// Rapid SCADA namespace
var scada = scada || {};
// Scheme namespace
scada.scheme = scada.scheme || {};

/********** Base Component **********/

// Abstract parent type for scheme and its components
scada.scheme.BaseComponent = function (type) {
    // Name of the component type
    this.type = type;
    // Component properties received from a server. They are different depending on component type
    this.props = null;
    // jQuery objects representing DOM content
    this.dom = null;
    // Renderer of the component
    this.renderer = null;
};

/********** Scheme Loading States **********/

// Scheme loading states enumeration
scada.scheme.LoadStates = {
    UNDEFINED: 0,
    DOC_LOADING: 1,
    COMPONENTS_LOADING: 2,
    IMAGES_LOADING: 3,
    COMPLETE: 4
};

/********** Scheme **********/

// Scheme type
scada.scheme.Scheme = function (editMode) {
    scada.scheme.BaseComponent.call(this);
    this.renderer = new scada.scheme.SchemeRenderer();

    // Count of components received by a one request
    this.LOAD_COMP_CNT = 100;
    // Total data size of images received by a one request, 1 MB
    this.LOAD_IMG_SIZE = 1048576;

    // Input channel filter for request current data
    this._cnlFilter = null;

    // Indicates whether the scheme is used by an editor
    this.editMode = !!editMode;
    // WCF-service URL
    this.serviceUrl = "SchemeSvc.svc/";
    // Scheme loading state
    this.loadState = scada.scheme.LoadStates.UNDEFINED;
    // Scheme view ID
    this.viewID = 0;
    // Scheme editor ID
    this.editorID = 0;
    // Stamp of the view, unique within application scope
    this.viewStamp = 0;
    // Scheme components
    this.components = [];
    // Scheme images
    this.images = [];
    // Scheme images indexed by name
    this.imageMap = new Map();
    // Render context
    this.renderContext = new scada.scheme.RenderContext();
    // Parent jQuery object
    this.parentDomElem = null;
    // Current scale
    this.scale = 1.0;
};

scada.scheme.Scheme.prototype = Object.create(scada.scheme.BaseComponent.prototype);
scada.scheme.Scheme.constructor = scada.scheme.Scheme;

// Continue scheme loading process
// callback is a function (success)
scada.scheme.Scheme.prototype._continueLoading = function (viewOrEditorID, callback) {
    var getCurTime = scada.utils.getCurTime;
    var thisScheme = this;

    this._loadPart(viewOrEditorID, function (success, complete) {
        if (success) {
            if (complete) {
                console.info(getCurTime() + " Scheme loading completed successfully");
                thisScheme.parentDomElem.removeClass("loading");
                callback(true);
            } else {
                setTimeout(thisScheme._continueLoading.bind(thisScheme), 0, viewOrEditorID, callback);
            }
        } else {
            console.error(getCurTime() + " Scheme loading failed");
            thisScheme.parentDomElem.removeClass("loading");
            callback(false);
        }
    });
};

// Load a part of the scheme depending on the loading state
// callback is a function (success, complete)
scada.scheme.Scheme.prototype._loadPart = function (viewOrEditorID, callback) {
    var LoadStates = scada.scheme.LoadStates;

    // check matching of view or editor
    if (this.editMode) {
        if (this.editorID == 0) {
            this.editorID = viewOrEditorID;
        } else if (this.editorID != viewOrEditorID) {
            console.warn(scada.utils.getCurTime() + " Scheme editor ID mismatch. The existing ID=" +
                this.editorID + ". The requsested ID=" + viewOrEditorID);
            callback(false, false);
            return;
        }
    } else {
        if (this.viewID == 0) {
            this.viewID = viewOrEditorID;
            this._cnlFilter = new scada.CnlFilter();
            this._cnlFilter.viewID = viewOrEditorID;
        } else if (this.viewID != viewOrEditorID) {
            console.warn(scada.utils.getCurTime() + " Scheme view ID mismatch. The existing ID=" +
                this.viewID + ". The requsested ID=" + viewOrEditorID);
            callback(false, false);
            return;
        }
    }

    // loading data depending on the state
    if (this.loadState == LoadStates.UNDEFINED) {
        this.loadState = LoadStates.DOC_LOADING;
    }

    switch (this.loadState) {
        case LoadStates.DOC_LOADING:
            this._loadSchemeDoc(viewOrEditorID, callback);
            break;
        case LoadStates.COMPONENTS_LOADING:
            this._loadComponents(viewOrEditorID, callback);
            break;
        case LoadStates.IMAGES_LOADING:
            this._loadImages(viewOrEditorID, callback);
            break;
        case LoadStates.COMPLETE:
            console.warn("Scheme loading is already complete");
            callback(true, true);
            break;
        default:
            console.warn("Unknown scheme loading state " + this.loadState);
            callback(false, false);
            break;
    }
};

// Get part of query string to request scheme by view ID or editor ID
scada.scheme.Scheme.prototype._getAccessParamStr = function (viewOrEditorID) {
    return (this.editMode ? "?editorID=" : "?viewID=") + viewOrEditorID;
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

// Load scheme document properties
// callback is a function (success, complete)
scada.scheme.Scheme.prototype._loadSchemeDoc = function (viewOrEditorID, callback) {
    var operation = this.serviceUrl + "GetSchemeDoc";
    var thisScheme = this;

    $.ajax({
        url: operation +
            this._getAccessParamStr(viewOrEditorID) +
            "&viewStamp=" + this.viewStamp,
        method: "GET",
        dataType: "json",
        cache: false
    })
    .done(function (data, textStatus, jqXHR) {
        try {
            var parsedData = $.parseJSON(data.d);
            if (parsedData.Success) {
                scada.utils.logSuccessfulRequest(operation);
                if (thisScheme._obtainSchemeDoc(parsedData)) {
                    thisScheme.loadState = scada.scheme.LoadStates.COMPONENTS_LOADING;
                    callback(true, false);
                } else {
                    callback(false, false);
                }
            } else {
                scada.utils.logServiceError(operation, parsedData.ErrorMessage);
                callback(false, false);
            }
        }
        catch (ex) {
            scada.utils.logProcessingError(operation, ex.message);
            callback(false, false);
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        scada.utils.logFailedRequest(operation, jqXHR);
        callback(false, false);
    });
};

// Obtain received scheme document properties
scada.scheme.Scheme.prototype._obtainSchemeDoc = function (parsedData) {
    try {
        if (typeof parsedData.ViewStamp === "undefined") {
            throw { message: "ViewStamp property is missing." };
        }

        if (typeof parsedData.SchemeDoc === "undefined") {
            throw { message: "SchemeDoc property is missing." };
        }

        if (this._viewStampsMatched(this.viewStamp, parsedData.ViewStamp)) {
            this.type = "";
            this.props = parsedData.SchemeDoc;
            this.viewStamp = parsedData.ViewStamp;
            return true;
        } else {
            return false;
        }
    }
    catch (ex) {
        console.error(scada.utils.getCurTime() + " Error obtaining scheme properties:", ex.message);
        return false;
    }
};

// Load scheme components
// callback is a function (success, complete)
scada.scheme.Scheme.prototype._loadComponents = function (viewOrEditorID, callback) {
    var operation = this.serviceUrl + "GetComponents";
    var thisScheme = this;

    $.ajax({
        url: operation +
            this._getAccessParamStr(viewOrEditorID) +
            "&viewStamp=" + this.viewStamp +
            "&startIndex=" + this.components.length +
            "&count=" + this.LOAD_COMP_CNT,
        method: "GET",
        dataType: "json",
        cache: false
    })
    .done(function (data, textStatus, jqXHR) {
        try {
            var parsedData = $.parseJSON(data.d);
            if (parsedData.Success) {
                scada.utils.logSuccessfulRequest(operation);
                if (thisScheme._obtainComponents(parsedData)) {
                    if (parsedData.EndOfComponents) {
                        thisScheme.loadState = scada.scheme.LoadStates.IMAGES_LOADING;
                    }
                    callback(true, false);
                } else {
                    callback(false, false);
                }
            } else {
                scada.utils.logServiceError(operation, parsedData.ErrorMessage);
                callback(false, false);
            }
        }
        catch (ex) {
            scada.utils.logProcessingError(operation, ex.message);
            callback(false, false);
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        scada.utils.logFailedRequest(operation, jqXHR);
        callback(false, false);
    });
}

// Obtain received scheme components
scada.scheme.Scheme.prototype._obtainComponents = function (parsedData) {
    try {
        if (typeof parsedData.ViewStamp === "undefined") {
            throw { message: "ViewStamp property is missing." };
        }

        if (typeof parsedData.EndOfComponents === "undefined") {
            throw { message: "EndOfComponents property is missing." };
        }

        if (typeof parsedData.Components === "undefined") {
            throw { message: "Components property is missing." };
        }

        if (this._viewStampsMatched(this.viewStamp, parsedData.ViewStamp)) {
            this.viewStamp = parsedData.ViewStamp;
            this._appendComponents(parsedData.Components);
            return true;
        } else {
            return false;
        }
    }
    catch (ex) {
        console.error(scada.utils.getCurTime() + " Error obtaining scheme components:", ex.message);
        return false;
    }
};

// Append received components to the scheme
scada.scheme.Scheme.prototype._appendComponents = function (parsedComponents) {
    for (var parsedComponent of parsedComponents) {
        if (typeof parsedComponent === "undefined" ||
            typeof parsedComponent.TypeName === "undefined" ||
            typeof parsedComponent.ID === "undefined") {
            console.warn("The component is skipped because the required properties are missed");
            continue;
        }

        var schemeComponent = new scada.scheme.Component();
        schemeComponent.type = parsedComponent.TypeName;
        schemeComponent.id = parsedComponent.ID;
        schemeComponent.props = parsedComponent;
        schemeComponent.renderer = scada.scheme.rendererMap.getRenderer(schemeComponent.type);

        if (schemeComponent.renderer) {
            // add the component to the scheme
            this.components.push(schemeComponent);
        } else {
            console.warn("The component of type '" + schemeComponent.type +
                "' with ID=" + schemeComponent.id + " is not supported");
        }
    }
};

// Load scheme images
// callback is a function (success, complete)
scada.scheme.Scheme.prototype._loadImages = function (viewOrEditorID, callback) {
    var operation = this.serviceUrl + "GetImages";
    var thisScheme = this;

    $.ajax({
        url: operation +
            this._getAccessParamStr(viewOrEditorID) +
            "&viewStamp=" + this.viewStamp +
            "&startIndex=" + this.images.length +
            "&totalDataSize=" + this.LOAD_IMG_SIZE,
        method: "GET",
        dataType: "json",
        cache: false
    })
    .done(function (data, textStatus, jqXHR) {
        try {
            var parsedData = $.parseJSON(data.d);
            if (parsedData.Success) {
                scada.utils.logSuccessfulRequest(operation);
                if (thisScheme._obtainImages(parsedData)) {
                    if (parsedData.EndOfImages) {
                        thisScheme.loadState = scada.scheme.LoadStates.COMPLETE;
                        callback(true, true);
                    } else {
                        callback(true, false);
                    }
                } else {
                    callback(false, false);
                }
            } else {
                scada.utils.logServiceError(operation, parsedData.ErrorMessage);
                callback(false, false);
            }
        }
        catch (ex) {
            scada.utils.logProcessingError(operation, ex.message);
            callback(false, false);
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        scada.utils.logFailedRequest(operation, jqXHR);
        callback(false, false);
    });
}

// Obtain received scheme images
scada.scheme.Scheme.prototype._obtainImages = function (parsedData) {
    try {
        if (typeof parsedData.ViewStamp === "undefined") {
            throw { message: "ViewStamp property is missing." };
        }

        if (typeof parsedData.EndOfImages === "undefined") {
            throw { message: "EndOfImages property is missing." };
        }

        if (typeof parsedData.Images === "undefined") {
            throw { message: "Images property is missing." };
        }

        if (this._viewStampsMatched(this.viewStamp, parsedData.ViewStamp)) {
            this.viewStamp = parsedData.ViewStamp;
            this._appendImages(parsedData.Images);
            return true;
        } else {
            return false;
        }
    }
    catch (ex) {
        console.error(scada.utils.getCurTime() + " Error obtaining scheme images:", ex.message);
        return false;
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

// Update the component using the current input channel data
scada.scheme.Scheme.prototype._updateComponent = function (component, renderContext) {
    try {
        if (component.dom) {
            component.renderer.update(component, renderContext);
        }
    }
    catch (ex) {
        console.error("Error updating the component of type '" +
            component.type + "' with ID=" + component.id + ":", ex.message);
        component.dom = null;
    }
};

// Clear the scheme
scada.scheme.Scheme.prototype.clear = function () {
    this.props = null;

    if (this.dom) {
        this.dom.remove();
        this.dom = null;
    }

    this._cnlFilter = null;
    this.loadState = scada.scheme.LoadStates.UNDEFINED;
    this.viewID = 0;
    this.editorID = 0;
    this.viewStamp = 0;
    this.components = [];
    this.images = [];
    this.imageMap = new Map();
    this.renderContext = new scada.scheme.RenderContext();
};

// Load the scheme
// callback is a function (success)
scada.scheme.Scheme.prototype.load = function (viewOrEditorID, callback) {
    console.info(scada.utils.getCurTime() + " Start loading scheme");
    this.parentDomElem.addClass("loading");
    this.clear();
    this._continueLoading(viewOrEditorID, callback);
};

// Create DOM content of the scheme
scada.scheme.Scheme.prototype.createDom = function (opt_controlRight) {
    this.renderContext.imageMap = this.imageMap;
    this.renderContext.controlRight = typeof opt_controlRight === "undefined" ?
        true : opt_controlRight;

    try
    {
        this.renderer.createDom(this, this.renderContext);

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

    for (var component of this.components) {
        try {
            component.renderer.createDom(component, this.renderContext);
            if (this.dom) {
                this.dom.append(component.dom);
            }
        }
        catch (ex) {
            console.error("Error creating DOM content for the component of type '" +
                component.type + "' with ID=" + component.id + ":", ex.message);
            component.dom = null;
        }
    }
};

// Update the scheme components
// callback is a function (success)
scada.scheme.Scheme.prototype.update = function (clientAPI, callback) {
    var thisScheme = this;

    clientAPI.getCurCnlDataExt(this._cnlFilter, function (success, cnlDataExtArr) {
        if (success) {
            var curCnlDataMap = clientAPI.createCnlDataExtMap(cnlDataExtArr);

            if (curCnlDataMap) {
                thisScheme.renderContext.curCnlDataMap = curCnlDataMap;
                thisScheme.renderContext.imageMap = thisScheme.imageMap;

                for (var component of thisScheme.components) {
                    thisScheme._updateComponent(component, thisScheme.renderContext);
                }
            }

            callback(true);
        } else {
            callback(false);
        }
    });
};

// Set the scheme scale.
// scale is a predefined string or floating point number
scada.scheme.Scheme.prototype.setScale = function (scale) {
    try {
        var scaleVal = $.isNumeric(scale) ? scale : this.renderer.calcScale(this, scale);
        this.renderer.setScale(this, scaleVal);
        this.scale = scaleVal;
    }
    catch (ex) {
        console.error("Error scaling the scheme:", ex.message);
    }
}

/********** Component **********/

// Component type
scada.scheme.Component = function () {
    scada.scheme.BaseComponent.call(this);

    // Component ID
    this.id = 0;
};

scada.scheme.Component.prototype = Object.create(scada.scheme.BaseComponent.prototype);
scada.scheme.Component.constructor = scada.scheme.Component;
