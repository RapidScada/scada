/*
 * Notifier control
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2019
 *
 * Requires:
 * - jquery
 * - eventtypes.js
 * - notiftypes.js
 */

// Rapid SCADA namespace
var scada = scada || {};

// Notifier type
scada.Notifier = function (selector) {
    // jQuery object of the notification area
    this._notifier = $(selector);

    // Default notification message lifetime, ms
    this.DEF_NOTIF_LIFETIME = 10000;

    // Infinite notification message lifetime
    this.INFINITE_NOTIF_LIFETIME = 0;

    // Clearing outdated notifications rate
    this.CLEAR_RATE = 1000;
};

// Add a notification to the notification area
scada.Notifier.prototype.addNotification = function (messageHtml, notifType, lifetime) {
    // remove the previous message if it is equal the new
    var divPrevMessage = this._notifier.children(".message:last");

    if (divPrevMessage.html() === messageHtml) {
        divPrevMessage.remove();
    }

    // add the new message
    var divMessage = $("<div class='message'></div>").html(messageHtml);

    if (notifType === scada.NotifTypes.ERROR) {
        divMessage.addClass("error");
    }

    if (lifetime) {
        divMessage.data("expires", Date.now() + lifetime);
    }

    this._notifier
        .css("display", "block")
        .append(divMessage)
        .scrollTop(this._notifier.prop("scrollHeight"));

    $(window).trigger(scada.EventTypes.UPDATE_LAYOUT);
};

// Clear the notifications which lifetime is expired
scada.Notifier.prototype.clearOutdatedNotifications = function () {
    var messages = this._notifier.find(".message");

    if (messages.length > 0) {
        var nowMs = Date.now();
        var removed = false;

        $.each(messages, function () {
            var expires = $(this).data("expires");
            if (expires < nowMs) {
                $(this).remove();
                removed = true;
            }
        });

        if (removed) {
            if (this._notifier.find(".message").length === 0) {
                this._notifier.css("display", "none");
            }

            $(window).trigger(scada.EventTypes.UPDATE_LAYOUT);
        }
    }
};

// Clear all the notifications
scada.Notifier.prototype.clearAllNotifications = function () {
    var messages = this._notifier.find(".message");

    if (messages.length > 0) {
        messages.remove();
        $(window).trigger(scada.EventTypes.UPDATE_LAYOUT);
    }
};

// Start outdated notifications clearing process
scada.Notifier.prototype.startClearingNotifications = function () {
    var thisNotifier = this;
    setInterval(function () { thisNotifier.clearOutdatedNotifications(); }, this.CLEAR_RATE);
}
