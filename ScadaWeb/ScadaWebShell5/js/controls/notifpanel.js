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

// Notification panel type
scada.NotifPanel = function (elemID) {
    // jQuery object of the notification panel
    this._panel = $("#" + elemID);
};

// Show the panel
scada.NotifPanel.prototype.show = function () {
};

// Hide the panel
scada.NotifPanel.prototype.hide = function () {
};

// Add a notification to the panel. Returns the notification ID
scada.NotifPanel.prototype.addNotification = function (messageHtml, notifType) {
};

// Remove the notification with the specified ID from the panel
scada.NotifPanel.prototype.removeNotification = function (notifID) {
};
