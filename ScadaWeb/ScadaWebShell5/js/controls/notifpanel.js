/*
 * Notification panel control
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 *
 * Requires:
 * - jquery
 * - notiftypes.js
 */

// Rapid SCADA namespace
var scada = scada || {};

/********** Notification **********/

// Notification type
scada.Notification = function (dateTime, messageHtml, notifType) {
    this.dateTime = dateTime;
    this.messageHtml = messageHtml;
    this.notifType = notifType;
};

/********** Notification Panel **********/

// Notification panel type
scada.NotifPanel = function () {
    // jQuery object of the notification panel
    this._panel = null;
};

// Initialize the panel based on the given element
scada.NotifPanel.prototype.init = function (elemID) {
    _panel = $("#" + elemID);
};

// Show the panel
scada.NotifPanel.prototype.show = function () {
    console.warn("NotifPanel.show");
};

// Hide the panel
scada.NotifPanel.prototype.hide = function () {
    console.warn("NotifPanel.hide");
};

// Add a notification to the panel. Returns the notification ID
scada.NotifPanel.prototype.addNotification = function (notification) {
};

// Remove the notification with the specified ID from the panel
scada.NotifPanel.prototype.removeNotification = function (notifID) {
};

/********** Notification Panel Locator **********/

// Notification panel locator object
scada.notifPanelLocator = {
    // Find and return an existing notification panel object
    getNotifPanel: function () {
        return window.notifPanel;
    }
};
