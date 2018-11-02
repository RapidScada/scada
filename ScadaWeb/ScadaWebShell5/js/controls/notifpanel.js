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
scada.Notification = function (key, dateTime, messageHtml, notifType) {
    this.id = 0;
    this.key = key;
    this.dateTime = dateTime;
    this.messageHtml = messageHtml;
    this.notifType = notifType;
};

/********** Notification Panel **********/

// Notification panel type
scada.NotifPanel = function () {
    // jQuery object of an empty notification
    this._emptyNotif = null;
    // Highest notification type of the existing notifications
    this._notifType = null;
    // Counter of notification IDs
    this._lastNotifID = 0;
    // Counters of notifications by type
    this._notifCounters = [];

    // jQuery object of the notification panel
    this.panelElem = null;
    // jQuery object of the notification bell
    this.bellElem = null;
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

// Reset the notification counters
scada.NotifPanel.prototype._resetNotifCounters = function () {
    this._notifCounters[scada.NotifTypes.INFO] = 0;
    this._notifCounters[scada.NotifTypes.WARNING] = 0;
    this._notifCounters[scada.NotifTypes.ERROR] = 0;
    this._notifType = null;
};

// Create jQuery element for the notification
scada.NotifPanel.prototype._createNotifElem = function (notification) {
    var notifElem = $("<div id='notif" + notification.id + "' class='notif'>" +
        "<div class='notif-type'>" + this._getNotifTypeIcon(notification.notifType) + "</div>" +
        "<div class='notif-time'>" + notification.dateTime + "</div>" +
        "<div class='notif-msg'>" + notification.messageHtml + "</div></div>");
    notifElem.data("notif", notification);
    return notifElem;
};

// Initialize the panel based on the given element
scada.NotifPanel.prototype.init = function (panelID, bellID) {
    var thisObj = this;
    this.panelElem = $("#" + panelID);
    this.bellElem = $("#" + bellID);
    this._emptyNotif = this.panelElem.children(".notif.empty");

    if (this._emptyNotif.length === 0) {
        this._emptyNotif = $("<div class='notif empty'>No notifications</div>").appendTo(this.panelElem);
    }

    if (scada.utils.isSmallScreen()) {
        this.panelElem.addClass("mobile");
    }

    this.bellElem.click(function () {
        if (thisObj.isVisible()) {
            thisObj.hide();
        } else {
            thisObj.show();
        }
    });

    this._resetNotifCounters();
};

// Show the panel
scada.NotifPanel.prototype.show = function (opt_animate) {
    if (!this.isVisible()) {
        this.panelElem.removeClass("hidden");

        if (opt_animate) {
            this.panelElem.css("right", -this.panelElem.outerWidth());
            this.panelElem.animate({ right: 0 }, "fast");
        }
    }
};

// Hide the panel
scada.NotifPanel.prototype.hide = function () {
    this.panelElem.addClass("hidden");
};

// Determine whether the panel is visible
scada.NotifPanel.prototype.isVisible = function () {
    return !this.panelElem.hasClass("hidden");
};

// Determine whether the panel contains notifications
scada.NotifPanel.prototype.isEmpty = function () {
    return this.panelElem.children(".notif:not(.empty):first").length === 0;
};

// Add a notification to the panel. Returns the notification ID
scada.NotifPanel.prototype.addNotification = function (notification) {
    notification.id = ++this._lastNotifID;
    this._emptyNotif.detach();
    this.panelElem.prepend(this._createNotifElem(notification));
    this._incNotifCounter(notification.notifType);
    this.showBell();
    return notification.id;
};

// Remove the notification with the specified ID from the panel
scada.NotifPanel.prototype.removeNotification = function (notifID) {
    var notifElem = this.panelElem.children("#notif" + notifID);
    var notification = notifElem.data("notif");
    notifElem.remove();

    if (this.isEmpty()) {
        this._emptyNotif.prependTo(this.panelElem);
    }

    if (notification) {
        this._decNotifCounter(notification.notifType);
        this.showBell();
    }
};

// Clear all notifications from the panel
scada.NotifPanel.prototype.clearNotifications = function () {
    this.panelElem.children(".notif:not(.empty)").remove();
    this._resetNotifCounters();
    this.showBell();
};

// Gently replace the existing notifications by the specified
scada.NotifPanel.prototype.replaceNotifications = function (notifications) {
    var newNotifCnt = notifications.length;

    if (newNotifCnt > 0) {
        this._emptyNotif.detach();
        var existingNotifElems = this.panelElem.children(".notif");
        var existingNotifCnt = existingNotifElems.length;

        if (existingNotifCnt > 0) {
            var i = 0;
            var j = 0;

            while (i < existingNotifCnt || j < newNotifCnt) {
                var newNotif = j < newNotifCnt ? notifications[j] : null;
                var existingNotifElem = null;
                var existingNotif = null;

                if (i < existingNotifCnt) {
                    existingNotifElem = existingNotifElems.eq(existingNotifCnt - i - 1);
                    existingNotif = existingNotifElem.data("notif");
                }

                if (existingNotif !== null && (newNotif === null || existingNotif.key < newNotif.key)) {
                    // remove the existing notification
                    existingNotifElem.remove();
                    this._decNotifCounter(existingNotif.notifType);
                    i++;
                } else if (newNotif !== null && (existingNotif === null || existingNotif.key > newNotif.key)) {
                    // insert the new notification
                    newNotif.id = ++this._lastNotifID;
                    var notifElem = this._createNotifElem(newNotif);

                    if (existingNotif === null) {
                        this.panelElem.prepend(notifElem);
                    } else {
                        existingNotifElem.after(notifElem);
                    }

                    this._incNotifCounter(newNotif.notifType);
                    j++;
                    continue;
                } else {
                    i++;
                    j++;
                }
            }

            this.showBell();
        } else {
            this.appendNotifications(notifications);
        }
    } else {
        this.clearNotifications();
    }
};

// Append the notifications to the existing on the panel
scada.NotifPanel.prototype.appendNotifications = function (notifications) {
    if (notifications.length > 0) {
        this._emptyNotif.detach();

        var lastNotifElem = this.panelElem.children(".notif:first");
        var lastNotifKey = lastNotifElem.length > 0 ? lastNotifElem.data("notif").key : "";

        for (var notif of notifications) {
            if (notif.key > lastNotifKey) {
                notif.id = ++this._lastNotifID;
                this.panelElem.prepend(this._createNotifElem(notif));
                this._incNotifCounter(notif.notifType);
            }
        }

        this.showBell();
    }
};

// Show the bell
scada.NotifPanel.prototype.showBell = function () {
    this.bellElem.removeClass("hidden empty info warning error");

    switch (this._notifType) {
        case scada.NotifTypes.INFO:
            this.bellElem.addClass("info");
            break;
        case scada.NotifTypes.WARNING:
            this.bellElem.addClass("warning");
            break;
        case scada.NotifTypes.ERROR:
            this.bellElem.addClass("error");
            break;
        default:
            this.bellElem.addClass("empty");
            break;
    }
};

// Hide the bell
scada.NotifPanel.prototype.hideBell = function () {
    this.bellElem.addClass("hidden");
};

/********** Notification Panel Locator **********/

// Notification panel locator object
scada.notifPanelLocator = {
    // Find and return an existing notification panel object
    getNotifPanel: function () {
        return window.notifPanel;
    }
};
