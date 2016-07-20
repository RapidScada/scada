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
 * - scada.chart
 * - scada.cmd
 * - scada.eventAck
 */

// Rapid SCADA namespace
var scada = scada || {};

// Common dialogs object
scada.dialogs = {
    // Web application root path
    rootPath: "",

    // Show chart web page
    showChart: function (viewID, date, cnlNums) {
        if (scada.chart && scada.chart.show) {
            scada.chart.show(this.rootPath, viewID, date, cnlNums);
        } else {
            console.warn("Unable to show chart because scada.chart is undefined");
        }
    },

    // Show command dialog.
    // opt_callback is a function (dialogResult),
    // dialogResult is true or false
    showCmd: function (viewID, ctrlCnlNum, opt_callback) {
        if (scada.cmd && scada.cmd.show) {
            scada.cmd.show(this.rootPath, viewID, ctrlCnlNum, opt_callback);
        } else {
            console.warn("Unable to show command dialog because scada.cmd is undefined");
            alert("Commands are not implemented yet, ctrlCnlNum=" + ctrlCnlNum);
        }
    },

    // Show event acknowledgement dialog.
    // date is a JavaScript date object,
    // opt_callback is a function (dialogResult),
    // dialogResult is true or false
    showEventAck: function (viewID, date, evNum, opt_callback) {
        if (scada.eventAck && scada.eventAck.show) {
            scada.eventAck.show(this.rootPath, viewID, date, evNum, opt_callback);
        } else {
            console.warn("Unable to show event acknowledgement dialog because scada.eventAck is undefined");
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
            popup.showDropdown(this.rootPath + "Calendar.aspx" + queryString, anchorElem, callback);
        }
    }
};