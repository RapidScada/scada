/*
 * View hub provides data exchange between a view, data windows and the shell
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

/*
 * Requires:
 * - jquery
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
    this.currentViewID = 0;

    // Main window object that manages a view and data windows
    this.mainWindow = mainWindow;

    // View window object
    this.viewWindow = null;

    // Data window object
    this.dataWindow = null;

    // Reference to a dialogs object
    this.dialogs = scada.dialogs;
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
scada.ViewHub.prototype.notify = function (senderWnd, eventType, opt_extraParams) {
    var handled = false;
    var senderIsView = senderWnd == this.viewWindow;

    // set main window title
    if (eventType == scada.eventTypes.viewTitleChanged) {
        if (senderIsView && this.mainWindow) {
            this.mainWindow.document.title = opt_extraParams;
        }
        handled = true;
    }

    // preprocess navigation
    if (eventType == scada.eventTypes.viewNavigate) {
        if (senderIsView) {
            this.currentViewID = opt_extraParams;
        } else {
            handled = true; // cancel notification
        }
    }

    // pass the notification to the view window
    if (!handled && this.viewWindow && this.viewWindow != senderWnd) {
        var jq = this.viewWindow.$;
        jq(this.viewWindow).trigger(eventType, opt_extraParams);
    }

    // pass the notification to the data window
    if (!handled && this.dataWindow && this.dataWindow != senderWnd) {
        var jq = this.dataWindow.$;
        jq(this.dataWindow).trigger(eventType, opt_extraParams);
    }
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
            wnd = window.parent;
        }
        return null;
    }
};