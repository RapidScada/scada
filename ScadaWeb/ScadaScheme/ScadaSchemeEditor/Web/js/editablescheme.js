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

/********** Editable Scheme **********/

// Editable scheme type
scada.scheme.EditableScheme = function () {
    scada.scheme.Scheme.call(this, true);

    // Stamp of the last processed change
    this.lastChangeStamp = 0;
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
                    // replace component
                    if (oldComponent.dom) {
                        this.componentMap.set(componentID, newComponent);
                        oldComponent.dom.replaceWith(newComponent.dom);
                    }
                } else {
                    // add component
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
                        // TODO: highlight selected objects
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
