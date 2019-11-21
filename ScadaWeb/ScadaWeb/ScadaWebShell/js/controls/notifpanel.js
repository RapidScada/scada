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
 * - utils.js
 */

// Rapid SCADA namespace
var scada = scada || {};

/********** Phrases **********/

scada.notifPhrases = {};

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
    // The storage key for muting
    this._MUTE_NOTIF_KEY = "Shell.MuteNotif";
    // The storage key for hiding notifications
    this._HIDDEN_NOTIF_KEY = "Shell.HiddenNotif";

    // jQuery object of an empty notification
    this._emptyNotif = null;
    // jQuery object displaying the mute state
    this._muteElem = null;
    // jQuery object representing an element to play info sound
    this._infoAudio = null;
    // jQuery object representing an element to play warning sound
    this._warningAudio = null;
    // jQuery object representing an element to play error sound
    this._errorAudio = null;
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

// Get the key of the last notification
scada.NotifPanel.prototype._getLastNotifKey = function () {
    var lastNotifElem = this.panelElem.children(".notif:first");
    return lastNotifElem.length > 0 ? lastNotifElem.data("notif").key : null;
};

// Play the information sound
scada.NotifPanel.prototype._playInfoSound = function () {
    this._warningAudio[0].pause();
    this._errorAudio[0].pause();

    if (!this._getMuted()) {
        scada.utils.playSound(this._infoAudio);
    }
};

// Play the warning sound
scada.NotifPanel.prototype._playWarningSound = function () {
    this._errorAudio[0].pause();

    if (!this._getMuted()) {
        scada.utils.playSound(this._warningAudio);
    }
};

// Play the error sound
scada.NotifPanel.prototype._playErrorSound = function () {
    this._warningAudio[0].pause();

    if (!this._getMuted()) {
        scada.utils.playSound(this._errorAudio);
    }
};

// Stop all sounds
scada.NotifPanel.prototype._stopSounds = function () {
    this._warningAudio[0].pause();
    this._errorAudio[0].pause();
};

// Continue to play sounds if needed
scada.NotifPanel.prototype._continueSounds = function () {
    if (this._notifType === scada.NotifTypes.WARNING) {
        this._playWarningSound();
    } else if (this._notifType === scada.NotifTypes.ERROR) {
        this._playErrorSound();
    }
};

// Get a value indicating whether sound is muted
scada.NotifPanel.prototype._getMuted = function () {
    var muted = sessionStorage.getItem(this._MUTE_NOTIF_KEY);
    return muted === null ? false : !!muted;
};

// Mute notification sound
scada.NotifPanel.prototype._mute = function () {
    this._stopSounds();
    this._displayMuteState(true);
    sessionStorage.setItem(this._MUTE_NOTIF_KEY, true);
};

// Unmute notification sound
scada.NotifPanel.prototype._unmute = function () {
    sessionStorage.removeItem(this._MUTE_NOTIF_KEY);
    this._continueSounds();
    this._displayMuteState(false);
};

// Set elements according to the mute state
scada.NotifPanel.prototype._displayMuteState = function (muted) {
    if (muted) {
        this._muteElem.children("i").removeClass("fa-toggle-on").addClass("fa-toggle-off");
        this._muteElem.children("span").text(scada.notifPhrases.unmute);
    } else {
        this._muteElem.children("i").removeClass("fa-toggle-off").addClass("fa-toggle-on");
        this._muteElem.children("span").text(scada.notifPhrases.mute);
    }
};

// Initialize the panel based on the given element
scada.NotifPanel.prototype.init = function (rootPath, panelID, bellID) {
    var thisObj = this;

    // get elements
    this.panelElem = $("#" + panelID);
    this.bellElem = $("#" + bellID);

    // create elements
    this._emptyNotif = $("<div class='notif empty'></div>").text(scada.notifPhrases.emptyNotif).appendTo(this.panelElem);
    this._muteElem = $("<div class='notif-mute'><i class='fa'></i><span></span></div>").appendTo(this.panelElem);
    this._displayMuteState(this._getMuted());

    var soundPath = rootPath + "sounds/";
    this._infoAudio = $("<audio preload src='" + soundPath + "notif_info.mp3' />").appendTo(this.panelElem);
    this._warningAudio = $("<audio preload loop src='" + soundPath + "notif_warning.mp3' />").appendTo(this.panelElem);
    this._errorAudio = $("<audio preload loop src='" + soundPath + "notif_error.mp3' />").appendTo(this.panelElem);

    // set appearance
    if (scada.utils.isSmallScreen()) {
        this.panelElem.addClass("mobile");
    }

    // bind events
    this.bellElem.click(function () {
        if (thisObj.isVisible()) {
            thisObj.hide();
        } else {
            thisObj.show();
        }
    });

    this._muteElem.click(function () {
        if (thisObj._getMuted()) {
            thisObj._unmute();
        } else {
            thisObj._mute();
        }
    });

    this._resetNotifCounters();
};

// Show the panel
scada.NotifPanel.prototype.show = function (opt_animate, opt_ifNewer) {
    if (!this.isVisible()) {
        var cancel = false;
        if (opt_ifNewer) {
            var hiddenNotifKey = sessionStorage.getItem(this._HIDDEN_NOTIF_KEY);
            var lastNotifKey = this._getLastNotifKey();
            cancel = hiddenNotifKey !== null && lastNotifKey !== null && hiddenNotifKey === lastNotifKey.toString();
        }

        if (!cancel) {
            sessionStorage.removeItem(this._HIDDEN_NOTIF_KEY);
            this.panelElem.removeClass("hidden");

            if (opt_animate) {
                this.panelElem.css("right", -this.panelElem.outerWidth());
                this.panelElem.animate({ right: 0 }, "fast");
            }
        }
    }
};

// Hide the panel
scada.NotifPanel.prototype.hide = function () {
    this.panelElem.addClass("hidden");
    sessionStorage.setItem(this._HIDDEN_NOTIF_KEY, this._getLastNotifKey());
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
    this.showBell(true);
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
            var playInfoSound = false;

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
                    playInfoSound = true;
                    continue;
                } else {
                    i++;
                    j++;
                }
            }

            this.showBell(playInfoSound);
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
        var lastNotifKey = this._getLastNotifKey();

        for (var notif of notifications) {
            if (lastNotifKey === null || notif.key > lastNotifKey) {
                notif.id = ++this._lastNotifID;
                this.panelElem.prepend(this._createNotifElem(notif));
                this._incNotifCounter(notif.notifType);
            }
        }

        this.showBell(true);
    }
};

// Show the bell and play sound
scada.NotifPanel.prototype.showBell = function (opt_playInfoSound) {
    this.bellElem.removeClass("hidden empty info warning error");

    switch (this._notifType) {
        case scada.NotifTypes.INFO:
            this.bellElem.addClass("info");

            if (opt_playInfoSound) {
                this._playInfoSound();
            } else {
                this._stopSounds();
            }
            break;
        case scada.NotifTypes.WARNING:
            this.bellElem.addClass("warning");
            this._playWarningSound();
            break;
        case scada.NotifTypes.ERROR:
            this.bellElem.addClass("error");
            this._playErrorSound();
            break;
        default:
            this.bellElem.addClass("empty");
            this._stopSounds();
            break;
    }
};

/********** Notification Panel Locator **********/

// Notification panel locator object
scada.notifPanelLocator = {
    // Find and return an existing notification panel object
    getNotifPanel: function () {
        return window.notifPanel;
    }
};
