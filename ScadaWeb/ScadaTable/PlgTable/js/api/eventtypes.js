/*
 * JavaScript event types used by the shell
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2018
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

    // Current view date has been changed
    // Event parameter: date
    VIEW_DATE_CHANGED: "scada:viewDateChanged",

    // Modal dialog button is clicked
    // Event parameter: dialog result
    MODAL_BTN_CLICK: "scada:modalBtnClick"
};
