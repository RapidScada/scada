/*
 * View hub provides data exchange between a view, data windows and the shell
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2019
 *
 * Requires:
 * - jquery
 * - utils.js
 * - eventtypes.js
 *
 * Optional:
 * - dialogs.js
 */

// Rapid SCADA namespace
var scada = scada || {};

// View hub type
scada.ViewHub = function (mainWindow) {
    // Current view ID
    this.curViewID = 0;

    // Current view date for displaying data (in milliseconds)
    this.curViewDateMs = 0;

    // Main window object that manages a view and data windows
    this.mainWindow = mainWindow;

    // View window object
    this.viewWindow = null;

    // Data window object
    this.dataWindow = null;

    // Reference to a dialogs object
    this.dialogs = scada.dialogs;
};

// Get the environment object, or an empty object if environment is not available
scada.ViewHub.prototype.getEnv = function () {
    return this.mainWindow && this.mainWindow.scada ? this.mainWindow.scada.env : {};
};

// Add the specified view to the hub.
// The method is called by the code that manages windows
scada.ViewHub.prototype.addView = function (wnd) {
    this.viewWindow = wnd;
};

// Add the specified data window to the hub.
// The method is called by the code that manages windows
scada.ViewHub.prototype.addDataWindow = function (wnd) {
    this.dataWindow = wnd;
};

// Remove the data window reference.
// The method is called by the code that manages windows
scada.ViewHub.prototype.removeDataWindow = function () {
    this.dataWindow = null;
};

// Send notification to a view or data window.
// The method is called by a child window
scada.ViewHub.prototype.notify = function (eventType, senderWnd, opt_extraParams) {
    // preprocess events
    if (eventType === scada.EventTypes.VIEW_DATE_CHANGED) {
        this.curViewDateMs = opt_extraParams.getTime();
    }

    // pass the notification to the main window
    if (this.mainWindow && this.mainWindow !== senderWnd) {
        var jq = this.mainWindow.$;
        if (jq) {
            jq(this.mainWindow).trigger(eventType, [senderWnd, opt_extraParams]);
        }
    }

    // pass the notification to the view window
    if (this.viewWindow && this.viewWindow !== senderWnd) {
        jq = this.viewWindow.$;
        if (jq) {
            jq(this.viewWindow).trigger(eventType, [senderWnd, opt_extraParams]);
        }
    }

    // pass the notification to the data window
    if (this.dataWindow && this.dataWindow !== senderWnd) {
        jq = this.dataWindow.$;
        if (jq) {
            jq(this.dataWindow).trigger(eventType, [senderWnd, opt_extraParams]);
        }
    }
};

// Get absolute URL of the view
scada.ViewHub.prototype.getFullViewUrl = function (viewID, opt_openInFrame) {
    return this.getEnv().rootPath + scada.utils.getViewUrl(viewID, opt_openInFrame);
};

// View hub locator object
scada.viewHubLocator = {
    // Find and return an existing view hub object
    getViewHub: function () {
        var wnd = window;
        while (wnd) {
            if (wnd.viewHub) {
                return wnd.viewHub;
            }
            wnd = wnd === window.top ? null : wnd.parent;
        }
        return null;
    }
};
