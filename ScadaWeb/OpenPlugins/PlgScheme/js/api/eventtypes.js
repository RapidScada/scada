/*
 * JavaScript event types used by the shell
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 *
 * No dependencies
 */

// Rapid SCADA namespace
var scada = scada || {};

// JavaScript event types enumeration
scada.EventTypes = {
    // Page layout should be updated
    UPDATE_LAYOUT: "scada:updateLayout",

    // View title has been changed.
    // Event parameter: title
    VIEW_TITLE_CHANGED: "scada:viewTitleChanged",

    // Before navigate to another view
    // Event parameter: view id
    VIEW_NAVIGATE: "scada:viewNavigate",

    // Current view date has been changed
    // Event parameter: date
    VIEW_DATE_CHANGED: "scada:viewDateChanged",
};
