/*
 * JavaScript event types used by the shell
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

// Rapid SCADA namespace
var scada = scada || {};

// JavaScript event types object
scada.eventTypes = {
    // Page layout should be updated
    updateLayout: "scada:updateLayout",

    // View title has been changed.
    // Event parameter: title
    viewTitleChanged: "scada:viewTitleChanged",

    // Before navigate to another view
    // Event parameter: view id
    viewNavigate: "scada:viewNavigate",

    // Current view date has been changed
    // Event parameter: date
    viewDateChanged: "scada:viewDateChanged",
};
