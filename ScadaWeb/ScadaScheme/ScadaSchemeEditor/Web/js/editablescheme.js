/*
 * Extension of scheme for edit
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 *
 * Requires:
 * - jquery
 * - utils.js
 * - schemecommon.js
 * - schememodel.js
 * - schemerender.js
 */

// Rapid SCADA namespace
var scada = scada || {};
// Scheme namespace
scada.scheme = scada.scheme || {};

/********** Scheme Changes Results **********/

// Scheme changes results enumeration
scada.scheme.GetChangesResults = {
    SUCCESS: 0,
    RELOAD_SCHEME: 1,
    EDITOR_UNKNOWN: 2,
    ERROR: 3
};

/********** Types of Scheme Changes **********/

// Types of scheme changes enumeration
scada.scheme.SchemeChangeTypes = {
    NONE: 0,
    SCHEME_DOC_CHANGED: 1,
    COMPONENT_ADDED: 2,
    COMPONENT_CHANGED: 3,
    COMPONENT_DELETED: 4,
    IMAGE_ADDED: 5,
    IMAGE_RENAMED: 6,
    IMAGE_DELETED: 7
};

/********** Select Component Actions **********/

scada.scheme.SelectActions = {
    SELECT: "select",
    APPEND: "append",
    DESELECT: "deselect",
    DESELECT_ALL: "deselectall"
};

/********** Dragging **********/

// Dragging type
scada.scheme.Dragging = function () {
    this.startX = 0;
    this.startY = 0;
}

/********** Editable Scheme **********/

// Editable scheme type
scada.scheme.EditableScheme = function () {
    scada.scheme.Scheme.call(this, true);

    // Stamp of the last processed change
    this.lastChangeStamp = 0;
    // Adding new component mode
    this.newComponentMode = false;
    // IDs of the selected components
    this.selComponentIDs = [];
    // Provides dragging and resizing
    this.dragging = new scada.scheme.Dragging();
};

scada.scheme.EditableScheme.prototype = Object.create(scada.scheme.Scheme.prototype);
scada.scheme.EditableScheme.constructor = scada.scheme.EditableScheme;

// Apply the received scheme changes
scada.scheme.EditableScheme.prototype._processChanges = function (changes) {
    var SchemeChangeTypes = scada.scheme.SchemeChangeTypes;

    for (var change of changes) {
        var changedObject = change.ChangedObject;

        switch (change.ChangeType) {
            case SchemeChangeTypes.SCHEME_DOC_CHANGED:
                this._updateSchemeProps(changedObject);
                break;
            case SchemeChangeTypes.COMPONENT_ADDED:
            case SchemeChangeTypes.COMPONENT_CHANGED:
                if (this._validateComponent(changedObject)) {
                    this._updateComponentProps(changedObject);
                }
                break;
            case SchemeChangeTypes.COMPONENT_DELETED:
                var component = this.componentMap.get(change.DeletedComponentID);
                if (component) {
                    this.componentMap.delete(component.id);
                    if (component.dom) {
                        component.dom.remove();
                    }
                }
                break;
            case SchemeChangeTypes.IMAGE_ADDED:
                if (this._validateImage(changedObject)) {
                    this.imageMap.set(changedObject.Name, changedObject);
                    this._refreshImages([changedObject.Name]);
                }
                break;
            case SchemeChangeTypes.IMAGE_RENAMED:
                var image = this.imageMap.get(change.OldImageName);
                if (image) {
                    this.imageMap.delete(change.OldImageName);
                    image.Name = change.ImageName;
                    this.imageMap.set(image.Name, image);
                    this._refreshImages([change.OldImageName, change.ImageName]);
                }
                break;
            case SchemeChangeTypes.IMAGE_DELETED:
                this.imageMap.delete(change.ImageName);
                this._refreshImages([change.ImageName]);
                break;
        }

        this.lastChangeStamp = change.Stamp;
    }
};

// Update the scheme properties
scada.scheme.EditableScheme.prototype._updateSchemeProps = function (parsedSchemeDoc) {
    try {
        this.props = parsedSchemeDoc;
        this.dom.detach();
        this.renderer.updateDom(this, this.renderContext);
        this.parentDomElem.append(this.dom);
    }
    catch (ex) {
        console.error("Error updating scheme properties:", ex.message);
    }
};

// Update the component properties or add the new component
scada.scheme.EditableScheme.prototype._updateComponentProps = function (parsedComponent) {
    try {
        var newComponent = new scada.scheme.Component(parsedComponent);
        newComponent.renderer = scada.scheme.rendererMap.get(newComponent.type);

        if (newComponent.renderer) {
            newComponent.renderer.createDom(newComponent, this.renderContext);

            if (newComponent.dom) {
                var componentID = parsedComponent.ID;
                var oldComponent = this.componentMap.get(componentID);

                if (oldComponent) {
                    if (oldComponent.dom) {
                        // copy selection
                        if (oldComponent.dom.hasClass("selected")) {
                            newComponent.dom.addClass("selected");
                        }

                        // replace component
                        this.componentMap.set(componentID, newComponent);
                        oldComponent.dom.replaceWith(newComponent.dom);
                    }
                } else {
                    // add component
                    this.componentMap.set(componentID, newComponent);
                    this.dom.append(newComponent.dom);
                }
            }
        }
    }
    catch (ex) {
        console.error("Error updating properties of the component of type '" +
            parsedComponent.TypeName + "' with ID=" + parsedComponent.ID + ":", ex.message);
    }
};

// Refresh scheme components that contain the specified images
scada.scheme.EditableScheme.prototype._refreshImages = function (imageNames) {
    try {
        this.renderer.refreshImages(this, this.renderContext, imageNames);

        for (var component of this.components) {
            component.renderer.refreshImages(component, this.renderContext, imageNames);
        }
    }
    catch (ex) {
        console.error("Error refreshing scheme images:", ex.message);
    }
};

// Highlight the selected components
scada.scheme.EditableScheme.prototype._processSelection = function (selCompIDs) {
    // add currently selected components to the set
    var idSet = new Set(this.selComponentIDs);

    // process changes of the selection
    for (var selCompID of selCompIDs) {
        if (idSet.has(selCompID)) {
            idSet.delete(selCompID);
        } else {
            $("#comp" + selCompID).addClass("selected");
        }
    }

    for (var deselCompID of idSet) {
        $("#comp" + deselCompID).removeClass("selected");
    }

    this.selComponentIDs = Array.isArray(selCompIDs) ? selCompIDs : [];
}

// Proccess mode of the editor
scada.scheme.EditableScheme.prototype._processMode = function (mode) {
    mode = !!mode;

    if (this.newComponentMode != mode) {
        if (mode) {
            this._getSchemeDiv().addClass("new-component-mode");
        } else {
            this._getSchemeDiv().removeClass("new-component-mode");
        }

        this.newComponentMode = mode;
    }
}

// Get the main div element of the scheme
scada.scheme.EditableScheme.prototype._getSchemeDiv = function () {
    return this.dom ? this.dom.first() : $();
}

// Send a request to add a new component to the scheme
scada.scheme.EditableScheme.prototype._addComponent = function (x, y) {
    var operation = this.serviceUrl + "AddComponent";

    $.ajax({
        url: operation +
            "?editorID=" + this.editorID +
            "&viewStamp=" + this.viewStamp +
            "&x=" + x +
            "&y=" + y,
        method: "GET",
        dataType: "json",
        cache: false
    })
    .done(function () {
        scada.utils.logSuccessfulRequest(operation);
    })
    .fail(function (jqXHR) {
        scada.utils.logFailedRequest(operation, jqXHR);
    });
}

// Send a request to change scheme component selection
scada.scheme.EditableScheme.prototype._changeSelection = function (action, opt_componentID) {
    var operation = this.serviceUrl + "ChangeSelection";

    $.ajax({
        url: operation +
            "?editorID=" + this.editorID +
            "&viewStamp=" + this.viewStamp +
            "&action=" + action +
            "&componentID=" + (opt_componentID ? opt_componentID : "-1"),
        method: "GET",
        dataType: "json",
        cache: false
    })
    .done(function () {
        scada.utils.logSuccessfulRequest(operation);
    })
    .fail(function (jqXHR) {
        scada.utils.logFailedRequest(operation, jqXHR);
    });
}

// Create DOM content of the scheme
scada.scheme.EditableScheme.prototype.createDom = function (opt_controlRight) {
    scada.scheme.Scheme.prototype.createDom.call(this, opt_controlRight);
    var SelectActions = scada.scheme.SelectActions;
    var thisScheme = this;

    this._getSchemeDiv()
        .on("mousedown", function (event) {
            if (thisScheme.newComponentMode) {
                // add new component
                thisScheme._addComponent(event.pageX, event.pageY);
            } else {
                // deselect all components
                console.log(scada.utils.getCurTime() + " Scheme background is clicked.");
                thisScheme._changeSelection(SelectActions.DESELECT_ALL);
            }
        })
        .on("mousedown", ".comp", function (event) {
            if (!thisScheme.newComponentMode) {
                // select or deselect component
                var componentID = $(this).data("id");
                console.log(scada.utils.getCurTime() + " Component with ID=" + componentID + " is clicked.");

                if (event.ctrlKey) {
                    thisScheme._changeSelection(
                        $(this).hasClass("selected") ? SelectActions.DESELECT : SelectActions.APPEND,
                        componentID);
                } else {
                    thisScheme._changeSelection(SelectActions.SELECT, componentID);
                }

                event.stopPropagation();
            }
        })
        .on("mousemove", function (event) {
            console.log("mousemove " + event.pageX + " " + event.pageY);
        })
        .on("mouseup", function () {

        })
        .on("selectstart", ".comp", false);
}

// Iteration of getting scheme changes
// callback is a function (result)
scada.scheme.EditableScheme.prototype.getChanges = function (callback) {
    var GetChangesResults = scada.scheme.GetChangesResults;
    var operation = this.serviceUrl + "GetChanges";
    var thisScheme = this;

    $.ajax({
        url: operation +
            "?editorID=" + this.editorID +
            "&viewStamp=" + this.viewStamp +
            "&changeStamp=" + this.lastChangeStamp,
        method: "GET",
        dataType: "json",
        cache: false
    })
    .done(function (data, textStatus, jqXHR) {
        try {
            var parsedData = $.parseJSON(data.d);
            if (parsedData.Success) {
                scada.utils.logSuccessfulRequest(operation);

                if (parsedData.EditorUnknown) {
                    console.error(scada.utils.getCurTime() + " Editor is unknown. Normal operation is impossible.");
                    callback(GetChangesResults.EDITOR_UNKNOWN);
                } else if (thisScheme.viewStamp && parsedData.ViewStamp) {
                    if (thisScheme.viewStamp == parsedData.ViewStamp) {
                        thisScheme._processChanges(parsedData.Changes);
                        thisScheme._processSelection(parsedData.SelCompIDs);
                        thisScheme._processMode(parsedData.NewCompMode);
                        callback(GetChangesResults.SUCCESS);
                    } else {
                        console.log(scada.utils.getCurTime() + " View stamps are different. Need to reload scheme.");
                        callback(GetChangesResults.RELOAD_SCHEME);
                    }
                } else {
                    console.error(scada.utils.getCurTime() + " View stamp is undefined on client or server side.");
                    callback(GetChangesResults.ERROR);
                }
            } else {
                scada.utils.logServiceError(operation, parsedData.ErrorMessage);
                callback(GetChangesResults.ERROR);
            }
        }
        catch (ex) {
            scada.utils.logProcessingError(operation, ex.message);
            callback(GetChangesResults.ERROR);
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        scada.utils.logFailedRequest(operation, jqXHR);
        callback(GetChangesResults.ERROR);
    });
};
