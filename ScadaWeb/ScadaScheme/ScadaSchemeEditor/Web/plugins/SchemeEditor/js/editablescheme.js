/*
 * Extension of scheme for edit
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2018
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
    DATA_ERROR: 3,
    COMM_ERROR: 4
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

/********** Main Form Actions **********/

scada.scheme.FormActions = {
    NEW: "new",
    OPEN: "open",
    SAVE: "save",
    CUT: "cut",
    COPY: "copy",
    PASTE: "paste",
    UNDO: "undo",
    REDO: "redo",
    POINTER: "pointer",
    DELETE: "delete"
};

/********** Drag Modes **********/

scada.scheme.DragModes = {
    NONE: 0,
    MOVE: 1,
    NW_RESIZE: 2,
    NE_RESIZE: 3,
    SW_RESIZE: 4,
    SE_RESIZE: 5,
    W_RESIZE: 6,
    E_RESIZE: 7,
    N_RESIZE: 8,
    S_RESIZE: 9
};

/********** Dragging **********/

// Dragging type
scada.scheme.Dragging = function () {
    // Width of the border allows to resize component
    this.BORDER_WIDTH = 5;
    // Minimum size required for enable resizing
    this.MIN_SIZE = 15;
    // Minimally moving
    this.MIN_MOVING = 5;

    // Dragging mode
    this.mode = scada.scheme.DragModes.NONE;
    // X coordinate of dragging start
    this.startX = 0;
    // Y coordinate of dragging start
    this.startY = 0;
    // Last value of horizontal moving
    this.lastDx = 0;
    // Last value of vertical moving
    this.lastDy = 0;
    // Last value of resized width
    this.lastW = 0;
    // Last value of resized height
    this.lastH = 0;
    // Dragged elements
    this.draggedElem = $();
    // Element was moved during dragging
    this.moved = false;
    // Element was resized during dragging
    this.resized = false;
};

// Get drag mode depending on the pointer position over the element
scada.scheme.Dragging.prototype._getDragMode = function (compJqObj, pageX, pageY, singleSelection) {
    var DragModes = scada.scheme.DragModes;
    var component = compJqObj.data("component");

    if (singleSelection && component && component.renderer.allowResizing(component)) {
        var elemOffset = compJqObj.offset();
        var elemPtrX = pageX - elemOffset.left;
        var elemPtrY = pageY - elemOffset.top;
        var compW = compJqObj.outerWidth();
        var compH = compJqObj.outerHeight();

        if (compW >= this.MIN_SIZE && compH >= this.MIN_SIZE) {
            // check if the cursor is over the border
            var onTheLeft = elemPtrX <= this.BORDER_WIDTH;
            var onTheRight = elemPtrX >= compW - this.BORDER_WIDTH;
            var onTheTop = elemPtrY <= this.BORDER_WIDTH;
            var atTheBot = elemPtrY >= compH - this.BORDER_WIDTH;

            if (onTheTop && onTheLeft) {
                return DragModes.NW_RESIZE;
            } else if (onTheTop && onTheRight) {
                return DragModes.NE_RESIZE;
            } else if (atTheBot && onTheLeft) {
                return DragModes.SW_RESIZE;
            } else if (atTheBot && onTheRight) {
                return DragModes.SE_RESIZE;
            } else if (onTheLeft) {
                return DragModes.W_RESIZE;
            } else if (onTheRight) {
                return DragModes.E_RESIZE;
            } else if (onTheTop) {
                return DragModes.N_RESIZE;
            } else if (atTheBot) {
                return DragModes.S_RESIZE;
            }
        }
    }

    return DragModes.MOVE;
};

// Move element horizontally during dragging
scada.scheme.Dragging.prototype._moveElemHor = function (compJqObj, dx) {
    this.lastDx = dx;
    this.moved = true;
    var component = compJqObj.data("component");
    var startLocation = compJqObj.data("start-location");
    var location = component.renderer.getLocation(component);
    component.renderer.setLocation(component, startLocation.x + dx, location.y);
};

// Move element vertically during dragging
scada.scheme.Dragging.prototype._moveElemVert = function (compJqObj, dy) {
    this.lastDy = dy;
    this.moved = true;
    var component = compJqObj.data("component");
    var startLocation = compJqObj.data("start-location");
    var location = component.renderer.getLocation(component);
    component.renderer.setLocation(component, location.x, startLocation.y + dy);
};

// Resize element during dragging
scada.scheme.Dragging.prototype._resizeElem = function (compJqObj, width, height) {
    this.lastW = width;
    this.lastH = height;
    this.resized = true;
    var component = compJqObj.data("component");
    component.renderer.setSize(component, width, height);
};

// Define the cursor depending on the pointer position
scada.scheme.Dragging.prototype.defineCursor = function (jqObj, pageX, pageY, singleSelection) {
    var DragModes = scada.scheme.DragModes;
    var compElem = jqObj.is(".comp-wrapper") ? jqObj.children(".comp") : jqObj.closest(".comp");

    if (compElem.length > 0) {
        var cursor = "";

        if (compElem.parent(".comp-wrapper").is(".selected")) {
            var dragMode = this._getDragMode(compElem, pageX, pageY, singleSelection);

            if (dragMode === DragModes.NW_RESIZE || dragMode === DragModes.SE_RESIZE) {
                cursor = "nwse-resize";
            } else if (dragMode === DragModes.NE_RESIZE || dragMode === DragModes.SW_RESIZE) {
                cursor = "nesw-resize";
            } else if (dragMode === DragModes.E_RESIZE || dragMode === DragModes.W_RESIZE) {
                cursor = "ew-resize";
            } else if (dragMode === DragModes.N_RESIZE || dragMode === DragModes.S_RESIZE) {
                cursor = "ns-resize";
            } else {
                cursor = "move";
            }
        }

        jqObj.css("cursor", cursor);
    }
};

// Start dragging the specified elements
scada.scheme.Dragging.prototype.startDragging = function (captCompJqObj, selCompJqObj, pageX, pageY) {
    var DragModes = scada.scheme.DragModes;

    this.mode = this._getDragMode(captCompJqObj, pageX, pageY, selCompJqObj.length <= 1);
    this.startX = pageX;
    this.startY = pageY;
    this.lastDx = 0;
    this.lastDy = 0;
    this.lastW = 0;
    this.lastH = 0;
    this.draggedElem = selCompJqObj;
    this.moved = false;
    this.resized = false;

    // save starting offset and size of the dragged components
    var thisObj = this;
    this.draggedElem.each(function () {
        var elem = $(this);
        var component = elem.data("component");
        elem.data("start-location", component.renderer.getLocation(component));

        if (thisObj.mode > DragModes.MOVE) {
            elem.data("start-size", component.renderer.getSize(component));
        }
    });
};

// Continue dragging
scada.scheme.Dragging.prototype.continueDragging = function (pageX, pageY) {
    var DragModes = scada.scheme.DragModes;
    var thisObj = this;
    var dx = pageX - this.startX;
    var dy = pageY - this.startY;

    if (this.draggedElem.length > 0 &&
        (this.moved || this.resized || Math.abs(dx) >= this.MIN_MOVING || Math.abs(dy) >= this.MIN_MOVING)) {
        if (this.mode === DragModes.MOVE) {
            // move elements
            this.lastDx = dx;
            this.lastDy = dy;
            this.moved = true;
            this.draggedElem.each(function () {
                var component = $(this).data("component");
                var startLocation = $(this).data("start-location");
                component.renderer.setLocation(component, startLocation.x + dx, startLocation.y + dy);
            });
        } else {
            var resizeLeft = this.mode === DragModes.NW_RESIZE ||
                this.mode === DragModes.SW_RESIZE || this.mode === DragModes.W_RESIZE;
            var resizeRight = this.mode === DragModes.NE_RESIZE ||
                this.mode === DragModes.SE_RESIZE || this.mode === DragModes.E_RESIZE;
            var resizeTop = this.mode === DragModes.NW_RESIZE ||
                this.mode === DragModes.NE_RESIZE || this.mode === DragModes.N_RESIZE;
            var resizeBot = this.mode === DragModes.SW_RESIZE ||
                this.mode === DragModes.SE_RESIZE || this.mode === DragModes.S_RESIZE;
            var elem = this.draggedElem.eq(0);
            var startSize = elem.data("start-size");
            var newWidth = startSize.width;
            var newHeight = startSize.height;

            if (resizeLeft) {
                // resize by pulling the left edge
                newWidth = Math.max(newWidth - dx, this.MIN_SIZE);
                this._moveElemHor(elem, Math.min(dx, startSize.width - this.MIN_SIZE));
                this._resizeElem(elem, newWidth, newHeight);
            } else if (resizeRight) {
                // resize by pulling the right edge
                newWidth = Math.max(newWidth + dx, this.MIN_SIZE);
                this._resizeElem(elem, newWidth, newHeight);
            }

            if (resizeTop) {
                // resize by pulling the top edge
                newHeight = Math.max(newHeight - dy, this.MIN_SIZE);
                this._moveElemVert(elem, Math.min(dy, startSize.height - this.MIN_SIZE));
                this._resizeElem(elem, newWidth, newHeight);
            } else if (resizeBot) {
                // resize by pulling the bottom edge
                newHeight = Math.max(newHeight + dy, this.MIN_SIZE);
                this._resizeElem(elem, newWidth, newHeight);
            }
        }
    }
};

// Stop dragging.
// callback is a function (dx, dy, w, h)
scada.scheme.Dragging.prototype.stopDragging = function (callback) {
    this.mode = scada.scheme.DragModes.NONE;

    // clear starting offsets and sizes
    this.draggedElem.each(function () {
        $(this)
            .removeData("start-location")
            .removeData("start-size");
    });

    // execute callback function
    if ((this.moved || this.resized) && typeof callback === "function") {
        callback(this.lastDx, this.lastDy, this.lastW, this.lastH);
    }
};

// Get status of dragging
scada.scheme.Dragging.prototype.getStatus = function () {
    var DragModes = scada.scheme.DragModes;

    if (this.mode === DragModes.NONE) {
        return "";
    } else {
        var component = this.draggedElem.data("component");
        var location = component.renderer.getLocation(component);
        var locationStr = "X: " + location.x + ", Y: " + location.y;

        if (this.mode === DragModes.MOVE) {
            return locationStr;
        } else {
            var size = component.renderer.getSize(component);
            return locationStr + ", W: " + size.width + ", H: " + size.height;
        }
    }
};

/********** Editable Scheme **********/

// Editable scheme type
scada.scheme.EditableScheme = function () {
    scada.scheme.Scheme.call(this);
    this.editMode = true;

    // Editor grid step
    this.GRID_STEP = 5;

    // Stamp of the last processed change
    this.lastChangeStamp = 0;
    // Adding new component mode
    this.newComponentMode = false;
    // IDs of the selected components
    this.selComponentIDs = [];
    // Provides dragging and resizing
    this.dragging = new scada.scheme.Dragging();
    // Useful information for a user
    this.status = "";
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
                var component = this.componentMap.get(change.ComponentID);
                if (component) {
                    this.componentMap.delete(component.id);
                    if (component.dom) {
                        component.dom.parent(".comp-wrapper").remove();
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
        var renderer = scada.scheme.rendererMap.get(newComponent.type);
        newComponent.renderer = renderer;

        if (renderer) {
            renderer.createDom(newComponent, this.renderContext);

            if (newComponent.dom) {
                newComponent.dom.first().data("component", newComponent);
                var componentID = parsedComponent.ID;
                var oldComponent = this.componentMap.get(componentID);
                this.componentMap.set(componentID, newComponent);

                if (oldComponent && oldComponent.dom) {
                    // replace component in the DOM
                    oldComponent.dom.replaceWith(newComponent.dom);
                    renderer.setWrapperProps(newComponent);
                } else {
                    // add component into the DOM
                    this.dom.append(renderer.wrap(newComponent));
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

        for (var component of this.componentMap.values()) {
            if (component.dom) {
                component.renderer.refreshImages(component, this.renderContext, imageNames);
            }
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
    var divScheme = this._getSchemeDiv();

    for (var selCompID of selCompIDs) {
        if (idSet.has(selCompID)) {
            idSet.delete(selCompID);
        } else {
            divScheme.find("#comp" + selCompID).parent(".comp-wrapper").addClass("selected");
        }
    }

    for (var deselCompID of idSet) {
        divScheme.find("#comp" + deselCompID).parent(".comp-wrapper").removeClass("selected");
    }

    this.selComponentIDs = Array.isArray(selCompIDs) ? selCompIDs : [];
};

// Proccess mode of the editor
scada.scheme.EditableScheme.prototype._processMode = function (mode) {
    mode = !!mode;

    if (this.newComponentMode !== mode) {
        if (mode) {
            this._getSchemeDiv().addClass("new-component-mode");
        } else {
            this._getSchemeDiv().removeClass("new-component-mode");
        }

        this.newComponentMode = mode;
    }
};

// Proccess editor title
scada.scheme.EditableScheme.prototype._processTitle = function (editorTitle) {
    if (editorTitle && document.title !== editorTitle) {
        document.title = editorTitle;
    }
};

// Proccess editor form state
scada.scheme.EditableScheme.prototype._processFormState = function (opt_formState) {
    var divSchWrapper = this._getSchemeDiv().closest(".scheme-wrapper");
    var prevFormState = divSchWrapper.data("form-state");
    var stickToLeft = prevFormState ? prevFormState.StickToLeft : false;
    var stickToRight = prevFormState ? prevFormState.StickToRight : false;
    var width = prevFormState ? prevFormState.Width : 0;
    var changed = false;

    if (opt_formState && opt_formState.StickToLeft && opt_formState.Width > 0) {
        if (!(stickToLeft && width === opt_formState.Width)) {
            // add space to the left
            changed = true;
            divSchWrapper.css({
                "border-left-width": opt_formState.Width,
                "border-right-width": 0
            });
        }
    } else if (opt_formState && opt_formState.StickToRight && opt_formState.Width > 0) {
        if (!(stickToRight && width === opt_formState.Width)) {
            // add space to the right
            changed = true;
            divSchWrapper.css({
                "border-left-width": 0,
                "border-right-width": opt_formState.Width
            });
        }
    } else if (stickToLeft || stickToRight) {
        // remove space
        changed = true;
        divSchWrapper.css({
            "border-left-width": 0,
            "border-right-width": 0
        });
    }

    if (changed) {
        if (opt_formState) {
            divSchWrapper.data("form-state", opt_formState);
        } else {
            divSchWrapper.removeData("form-state");
        }

        divSchWrapper.outerWidth($(window).width());
    }
};

// Get the main div element of the scheme
scada.scheme.EditableScheme.prototype._getSchemeDiv = function () {
    return this.dom ? this.dom.first() : $();
};

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
};

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
};

// Send a request to move and resize selected scheme components
scada.scheme.EditableScheme.prototype._moveResize = function (dx, dy, w, h) {
    var operation = this.serviceUrl + "MoveResize";

    $.ajax({
        url: operation +
            "?editorID=" + this.editorID +
            "&viewStamp=" + this.viewStamp +
            "&dx=" + dx +
            "&dy=" + dy +
            "&w=" + w +
            "&h=" + h,
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
};

// Send a request to perform action of the main form
scada.scheme.EditableScheme.prototype._formAction = function (action) {
    var operation = this.serviceUrl + "FormAction";

    $.ajax({
        url: operation +
            "?editorID=" + this.editorID +
            "&viewStamp=" + this.viewStamp +
            "&action=" + action,
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
};

// Create DOM content of the scheme
scada.scheme.EditableScheme.prototype.createDom = function (opt_controlRight) {
    scada.scheme.Scheme.prototype.createDom.call(this, opt_controlRight);
    var SelectActions = scada.scheme.SelectActions;
    var DragModes = scada.scheme.DragModes;
    var thisScheme = this;

    // store references to the components in the DOM
    for (var component of this.componentMap.values()) {
        if (component.dom) {
            component.dom.first().data("component", component);
        }
    }

    // bind events for dragging
    var divScheme = this._getSchemeDiv();
    divScheme
        .on("mousedown", function (event) {
            if (thisScheme.newComponentMode) {
                // add new component
                var offset = divScheme.offset();
                thisScheme._addComponent(event.pageX - parseInt(offset.left), event.pageY - parseInt(offset.top));
            } else {
                // deselect all components
                console.log(scada.utils.getCurTime() + " Scheme background is clicked.");
                thisScheme._changeSelection(SelectActions.DESELECT_ALL);
            }
        })
        .on("mousedown", ".comp-wrapper", function (event) {
            if (!thisScheme.newComponentMode) {
                // select or deselect component and start dragging
                var compElem = $(this).children(".comp");
                var componentID = compElem.data("id");
                var selected = $(this).hasClass("selected");
                console.log(scada.utils.getCurTime() + " Component with ID=" + componentID + " is clicked.");

                if (event.ctrlKey) {
                    thisScheme._changeSelection(
                        selected ? SelectActions.DESELECT : SelectActions.APPEND,
                        componentID);
                } else {
                    if (!selected) {
                        divScheme.find(".comp-wrapper.selected").removeClass("selected");
                        $(this).addClass("selected");
                        thisScheme._changeSelection(SelectActions.SELECT, componentID);
                    }

                    thisScheme.dragging.startDragging(
                        compElem, divScheme.find(".comp-wrapper.selected .comp"), event.pageX, event.pageY);
                }

                event.stopPropagation();
            }
        })
        .on("mousemove", function (event) {
            if (thisScheme.dragging.mode === DragModes.NONE) {
                thisScheme.dragging.defineCursor($(event.target), event.pageX, event.pageY,
                    thisScheme.selComponentIDs.length <= 1);

                if (thisScheme.newComponentMode) {
                    var offset = divScheme.offset();
                    thisScheme.status = "X: " + (event.pageX - parseInt(offset.left)) +
                        ", Y: " + (event.pageY - parseInt(offset.top));
                } else {
                    thisScheme.status = "";
                }
            } else {
                thisScheme.dragging.continueDragging(event.pageX, event.pageY);
                thisScheme.status = thisScheme.dragging.getStatus();
            }
        })
        .on("mouseup mouseleave", function () {
            if (thisScheme.dragging.mode !== DragModes.NONE) {
                thisScheme.dragging.stopDragging(function (dx, dy, w, h) {
                    // send changes to server under the assumption that the selection was not changed during dragging
                    thisScheme._moveResize(dx, dy, w, h);
                });
                thisScheme.status = "";
            }
        })
        .on("selectstart", ".comp-wrapper", false)
        .on("dragstart", false);
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
            "&changeStamp=" + this.lastChangeStamp +
            "&status=" + encodeURIComponent(this.status ? this.status : " "), // space needed by Mono
        method: "GET",
        dataType: "json",
        cache: false
    })
    .done(function (data, textStatus, jqXHR) {
        try {
            var parsedData = $.parseJSON(data.d);
            if (parsedData.Success) {
                scada.utils.logSuccessfulRequest(operation);
                thisScheme._processFormState(parsedData.FormState);

                if (parsedData.EditorUnknown) {
                    console.error(scada.utils.getCurTime() + " Editor is unknown. Normal operation is impossible.");
                    callback(GetChangesResults.EDITOR_UNKNOWN);
                } else if (thisScheme.viewStamp && parsedData.ViewStamp) {
                    if (thisScheme.viewStamp === parsedData.ViewStamp) {
                        thisScheme._processChanges(parsedData.Changes);
                        thisScheme._processSelection(parsedData.SelCompIDs);
                        thisScheme._processMode(parsedData.NewCompMode);
                        thisScheme._processTitle(parsedData.EditorTitle);
                        callback(GetChangesResults.SUCCESS);
                    } else {
                        console.log(scada.utils.getCurTime() + " View stamps are different. Need to reload scheme.");
                        callback(GetChangesResults.RELOAD_SCHEME);
                    }
                } else {
                    console.error(scada.utils.getCurTime() + " View stamp is undefined on client or server side.");
                    callback(GetChangesResults.DATA_ERROR);
                }
            } else {
                scada.utils.logServiceError(operation, parsedData.ErrorMessage);
                callback(GetChangesResults.DATA_ERROR);
            }
        }
        catch (ex) {
            scada.utils.logProcessingError(operation, ex.message);
            callback(GetChangesResults.DATA_ERROR);
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        scada.utils.logFailedRequest(operation, jqXHR);
        thisScheme._processFormState();
        callback(GetChangesResults.COMM_ERROR);
    });
};

// Perform an action depending on the pressed key. Returns false if the key is handled
scada.scheme.EditableScheme.prototype.processKey = function (keyChar, keyCode, ctrlKey) {
    var DragModes = scada.scheme.DragModes;
    var FormActions = scada.scheme.FormActions;

    // use keyCode instead of keyChar to provide case insensitiveness and culture independence
    if (37 <= keyCode && keyCode <= 40 /*arrow keys*/ &&
        this.dragging.mode === DragModes.NONE) {
        // move selected components
        var move = ctrlKey ? 1 : this.GRID_STEP;
        var dx = 0;
        var dy = 0;

        if (keyCode === 37 /*left arrow*/) {
            dx = -move;
        } else if (keyCode === 39 /*right arrow*/) {
            dx = move;
        } else if (keyCode === 38 /*up arrow*/) {
            dy = -move;
        } else if (keyCode === 40 /*down arrow*/) {
            dy = move;
        }

        this._getSchemeDiv().find(".comp-wrapper.selected").each(function () {
            var offset = $(this).offset();
            $(this).offset({ left: offset.left + dx, top: offset.top + dy });
        });

        // send changes to server
        this._moveResize(dx, dy, 0, 0);
    } else if (ctrlKey) {
        if (keyCode === 78 /*N*/) {
            this._formAction(FormActions.NEW); // doesn't work
        } else if (keyCode === 79 /*O*/) {
            this._formAction(FormActions.OPEN);
        } else if (keyCode === 83 /*S*/) {
            this._formAction(FormActions.SAVE);
        } else if (keyCode === 88 /*X*/) {
            this._formAction(FormActions.CUT);
        } else if (keyCode === 67 /*C*/) {
            this._formAction(FormActions.COPY);
        } else if (keyCode === 86 /*V*/) {
            this._formAction(FormActions.PASTE);
        } else if (keyCode === 90 /*Z*/) {
            this._formAction(FormActions.UNDO);
        } else if (keyCode === 89 /*Y*/) {
            this._formAction(FormActions.REDO);
        } else {
            return true;
        }
    } else if (keyCode === 27 /*Escape*/) {
        this._formAction(FormActions.POINTER);
    } else if (keyCode === 46 /*Delete*/) {
        this._formAction(FormActions.DELETE);
    } else {
        return true;
    }

    return false;
};
