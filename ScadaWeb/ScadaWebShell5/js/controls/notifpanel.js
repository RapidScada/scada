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
    this.id = 0;
    this.dateTime = dateTime;
    this.messageHtml = messageHtml;
    this.notifType = notifType;
};

/********** Notification Panel **********/

// Notification panel type
scada.NotifPanel = function () {
    // jQuery object of the notification panel
    this._panel = null;
    // jQuery object of the notification bell
    this._bell = null;
    // Highest notification type of the existing notifications
    this.notifType = null;
    // Counter of notification IDs
    this.lastNotifID = 0;
};

// Initialize the panel based on the given element
scada.NotifPanel.prototype.init = function (panelID, bellID) {
    var thisObj = this;
    this._panel = $("#" + panelID);
    this._bell = $("#" + bellID);

    if (scada.utils.isSmallScreen()) {
        this._panel.addClass("mobile");
    }

    this._bell.click(function () {
        if (thisObj.isVisible()) {
            thisObj.show();
        } else {
            thisObj.hide();
        }
    });
};

// Show the panel
scada.NotifPanel.prototype.show = function (opt_animate) {
    this._panel.removeClass("hidden");

    if (opt_animate) {
        this._panel.css("right", -this._panel.outerWidth());
        this._panel.animate({ right: 0 }, "fast");
    }
};

// Hide the panel
scada.NotifPanel.prototype.hide = function () {
    this._panel.addClass("hidden");
};

// Determine whether the panel is visible
scada.NotifPanel.prototype.isVisible = function () {
    return this._panel.hasClass("hidden");
};

// Add a notification to the panel. Returns the notification ID
scada.NotifPanel.prototype.addNotification = function (notification) {
    notification.id = this.lastNotifID++;
    var notifElem = $("<div id='notif" + notification.id + "' class='notif'>" + notification.messageHtml + "<div>");
    this._panel.append(notifElem);
    return notification.id;
};

// Remove the notification with the specified ID from the panel
scada.NotifPanel.prototype.removeNotification = function (notifID) {
    this._panel.children("#notif" + notifID).remove();
};

// Show the bell
scada.NotifPanel.prototype.showBell = function () {
    this._bell.removeClass("hidden empty info warning error");

    switch (this.notifType) {
        case scada.NotifTypes.INFO:
            this._bell.addClass("info");
            break;
        case scada.NotifTypes.WARNING:
            this._bell.addClass("warning");
            break;
        case scada.NotifTypes.ERROR:
            this._bell.addClass("error");
            break;
        default:
            this._bell.addClass("empty");
            break;
    }
};

// Hide the bell
scada.NotifPanel.prototype.hideBell = function () {
    this._bell.addClass("hidden");
};

/********** Notification Panel Locator **********/

// Notification panel locator object
scada.notifPanelLocator = {
    // Find and return an existing notification panel object
    getNotifPanel: function () {
        return window.notifPanel;
    }
};
