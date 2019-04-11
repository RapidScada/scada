/*
 * Common dialogs
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 *
 * Requires:
 * - jquery
 * - utils.js
 * - popup.js
 *
 * Requires external objects:
 * - scada.chart.dialog
 * - scada.cmd.dialog
 * - scada.eventAck.dialog
 */

// Rapid SCADA namespace
var scada = scada || {};
// Chart namespace
scada.chart = scada.chart || {};
// Command namespace
scada.cmd = scada.cmd || {};
// Event acknowledgement namespace
scada.eventAck = scada.eventAck || {};

// Common dialogs object
scada.dialogs = {
    // Web application root path
    rootPath: "",

    // Show chart web page
    showChart: function (cnlNums, viewIDs, date) {
        if (scada.chart.dialog && scada.chart.dialog.show) {
            scada.chart.dialog.show(this.rootPath, cnlNums, viewIDs, date);
        } else {
            console.warn("Unable to show chart because scada.chart.dialog is undefined");
        }
    },

    // Show command dialog.
    // opt_callback is a function (dialogResult),
    // dialogResult is true or false
    showCmd: function (ctrlCnlNum, viewID, opt_callback) {
        if (scada.cmd.dialog && scada.cmd.dialog.show) {
            scada.cmd.dialog.show(this.rootPath, ctrlCnlNum, viewID, opt_callback);
        } else {
            console.warn("Unable to show command dialog because scada.cmd.dialog is undefined");
        }
    },

    // Show event acknowledgement dialog.
    // date is a JavaScript date object,
    // opt_callback is a function (dialogResult),
    // dialogResult is true or false
    showEventAck: function (date, evNum, viewID, opt_callback) {
        if (scada.eventAck.dialog && scada.eventAck.dialog.show) {
            scada.eventAck.dialog.show(this.rootPath, date, evNum, viewID, opt_callback);
        } else {
            console.warn("Unable to show event acknowledgement dialog because scada.eventAck.dialog is undefined");
        }
    },

    // Show calendar dropdown form.
    // selectedDate is a string date representation,
    // callback is a function (dialogResult, extraParams),
    // dialogResult is true or false,
    // extraParams is object { date, dateStr }
    showCalendar: function (anchorElem, selectedDate, callback) {
        var popup = scada.popupLocator.getPopup();
        if (popup) {
            var queryString = selectedDate ? "?date=" + encodeURIComponent(selectedDate) : "";
            popup.showDropdown(this.rootPath + "dialogs/Calendar.aspx" + queryString, anchorElem, callback);
        }
    },

    // Show confirmation dialog.
    // callback is a function (dialogResult)
    showConfirm: function (callback) {
        var popup = scada.popupLocator.getPopup();
        if (popup) {
            popup.showModal(this.rootPath + "dialogs/Confirm.aspx",
                new scada.ModalOptions([scada.ModalButtons.YES, scada.ModalButtons.NO], scada.ModalSizes.SMALL),
                callback);
        }
    }
};