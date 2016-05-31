// Notifier control
var notifier = null;
// View ID. Must be defined in Table.aspx
var viewID = viewID || 0;
// Current data refresh rate
var refrRate = refrRate || 1000;
// Localized phrases
var phrases = phrases || {};

$(document).ready(function () {
    scada.clientAPI.rootPath = "../../";
    styleIOS();
    updateLayout();
    notifier = new scada.Notifier("#divNotif");
    notifier.startClearingNotifications();

    if (DEBUG_MODE) {
        initDebugTools();
    } else {
        // TODO: start updating
    }

    $(window).on("resize " + scada.EventTypes.UPDATE_LAYOUT, function () {
        updateLayout();
    });
});
