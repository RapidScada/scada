/*
 * Event acknowledgement object that implements showing of the appropriate dialog
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 *
 * Requires:
 * - jquery
 * - bootstrap
 * - utils.js
 * - popup.js
 */

// Rapid SCADA namespace
var scada = scada || {};

scada.eventAck = {
    // Show event acknowledgement dialog
    show: function (rootPath, viewID, date, evNum, opt_callback) {
        var popup = scada.popupLocator.getPopup();
        if (popup) {
            popup.showModal(rootPath + "plugins/Table/EventAck.aspx?viewID=" +
                viewID + "&" + scada.utils.dateToQueryString(date) + "&evNum=" + evNum,
                [scada.ModalButtons.OK, scada.ModalButtons.CANCEL], opt_callback);
        }
    }
}