/*
 * View hub provides data exchange between a view, data windows and the shell
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

/*
 * Requires:
 * - jquery
 */

// Rapid SCADA namespace
var scada = scada || {};

// View hub type
scada.ViewHub = function () {
    // Current view ID
    this.currentViewID = 0;

    // View window JavaScript object
    this.viewWindow = null;

    // Data window JavaScript object
    this.dataWindow = null;
};

// Navigate to the specified view
scada.ViewHub.prototype.navigate = function (viewID, viewUrl) {

};

// Get data window URL considering the current view
scada.ViewHub.prototype.getDataWindowUrl = function (initialUrl) {

};

// Show chart web page
scada.ViewHub.prototype.showChart = function (viewID, cnlNums) {

};

// Show command dialog
scada.ViewHub.prototype.showCmdDialog = function (viewID, ctrlCnlNum) {

};

// Show event acknowledgement dialog
scada.ViewHub.prototype.showEventAckDialog = function (viewID, year, month, day, evNum) {

}

// Add the specified view to the hub
scada.ViewHub.prototype.addView = function (wnd) {

};

// Add the specified data window to the hub
scada.ViewHub.prototype.addDataWindow = function (wnd) {

};

// Send notification to a view or data window
scada.ViewHub.prototype.notify = function (senderWnd, eventType, opt_extraParams) {

};

// View hub locator object
scada.viewHubLocator = {
    // Find and return an existing view hub object
    getViewHub: function () {

    }
};