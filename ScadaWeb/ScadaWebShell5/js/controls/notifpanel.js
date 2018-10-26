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
    // jQuery object of an empty notification
    this._emptyNotif = null;
    // Highest notification type of the existing notifications
    this._notifType = null;
    // Counter of notification IDs
    this._lastNotifID = 0;
    // Counters of notifications by type
    this._notifCounters = [];
};

// Get HTML of an icon appropriate to the notification type
scada.NotifPanel.prototype._getNotifTypeIcon = function (notifType) {
    switch (notifType) {
        case scada.NotifTypes.INFO:
            return "<i class='fa fa-info-circle info'></i>";
        case scada.NotifTypes.WARNING:
            return "<i class='fa fa-exclamation-triangle warning'></i>";
        case scada.NotifTypes.ERROR:
            return "<i class='fa fa-exclamation-circle error'></i>";
        default:
            return "";
    }
};

// Increase a notification counter corresponding to the specified type
scada.NotifPanel.prototype._incNotifCounter = function (notifType) {
    this._notifCounters[notifType]++;

    if (this._notifType === null || this._notifType < notifType) {
        this._notifType = notifType;
    }
};

// Decrease a notification counter corresponding to the specified type
scada.NotifPanel.prototype._decNotifCounter = function (notifType) {
    if (this._notifCounters[notifType] > 0) {
        this._notifCounters[notifType]--;
    }

    if (this._notifCounters[scada.NotifTypes.ERROR] > 0) {
        this._notifType = scada.NotifTypes.ERROR;
    } else if (this._notifCounters[scada.NotifTypes.WARNING] > 0) {
        this._notifType = scada.NotifTypes.WARNING;
    } else if (this._notifCounters[scada.NotifTypes.INFO] > 0) {
        this._notifType = scada.NotifTypes.INFO;
    } else {
        this._notifType = null;
    }
};

// Initialize the panel based on the given element
scada.NotifPanel.prototype.init = function (panelID, bellID) {
    var thisObj = this;
    this._panel = $("#" + panelID);
    this._bell = $("#" + bellID);
    this._emptyNotif = this._panel.children(".notif.empty");

    if (this._emptyNotif.length === 0) {
        this._emptyNotif = $("<div class='notif empty'>No notifications</div>").appendTo(this._panel);
    }

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

    this._notifCounters[scada.NotifTypes.INFO] = 0;
    this._notifCounters[scada.NotifTypes.WARNING] = 0;
    this._notifCounters[scada.NotifTypes.ERROR] = 0;
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
    notification.id = ++this._lastNotifID;

    var notifElem = $("<div id='notif" + notification.id + "' class='notif'>" +
        "<div class='notif-type'>" + this._getNotifTypeIcon(notification.notifType) + "</div>" +
        "<div class='notif-time'>" + notification.dateTime + "</div>" +
        "<div class='notif-msg'>" + notification.messageHtml + "</div></div>");
    notifElem.data("notif", notification);
    this._emptyNotif.detach();
    this._panel.append(notifElem);

    this._incNotifCounter(notification.notifType);
    this.showBell();

    return notification.id;
};

// Remove the notification with the specified ID from the panel
scada.NotifPanel.prototype.removeNotification = function (notifID) {
    var notifElem = this._panel.children("#notif" + notifID);
    var notification = notifElem.data("notif");
    notifElem.remove();

    if (this._panel.children(".notif:first").length === 0) {
        this._emptyNotif.prependTo(this._panel);
    }

    if (notification) {
        this._decNotifCounter(notification.notifType);
        this.showBell();
    }
};

// Show the bell
scada.NotifPanel.prototype.showBell = function () {
    this._bell.removeClass("hidden empty info warning error");

    switch (this._notifType) {
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
