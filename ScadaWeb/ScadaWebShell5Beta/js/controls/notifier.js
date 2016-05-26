/*
 * Notifier control
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 *
 * Requires:
 * - jquery
 * - eventtypes.js
 */

// Rapid SCADA namespace
var scada = scada || {};


// Notifier type
scada.Notifier = function (id) {
    // jQuery object of the notification area
    this._notifier = $("#" + id);

    // Default notification message lifetime, ms
    this.DEF_NOTIF_LIFETIME = 10000;

    // Infinite notification message lifetime
    this.INFINITE_NOTIF_LIFETIME = 0;
};

// Add notification to the notification area
scada.Notifier.prototype.addNotification = function (messageHtml, error, lifetime) {
    // remove the previous message if it is equal the new
    var divPrevMessage = this._notifier.children(".message:last");

    if (divPrevMessage.html() == messageHtml) {
        divPrevMessage.remove();
    }

    // add the new message
    var divMessage = $("<div class='message'></div>").html(messageHtml);

    if (error) {
        divMessage.addClass("error");
    }

    if (lifetime) {
        divMessage.attr("data-expires", Date.now() + lifetime);
    }

    this._notifier
        .css("display", "block")
        .append(divMessage)
        .scrollTop(divNotif.prop("scrollHeight"));

    $(window).trigger(scada.EventTypes.UPDATE_LAYOUT);
};

// Clear the notifications which lifetime is expired
scada.Notifier.prototype.clearOutdatedNotifications = function () {
    var messages = this._notifier.find(".message");

    if (messages.length > 0) {
        var nowMs = Date.now();
        var removed = false;

        $.each(messages, function () {
            var expires = $(this).attr("data-expires");
            if (expires < nowMs) {
                $(this).remove();
                removed = true;
            }
        });

        if (removed) {
            if (this._notifier.find(".message").length == 0) {
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
