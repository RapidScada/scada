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
// Event acknowledgement namespace
scada.eventAck = scada.eventAck || {};

scada.eventAck.dialog = {
    // Show event acknowledgement dialog
    show: function (rootPath, date, evNum, viewID, opt_callback) {
        var popup = scada.popupLocator.getPopup();
        if (popup) {
            popup.showModal(rootPath + "plugins/Table/EventAck.aspx?" +
                scada.utils.dateToQueryString(date) + "&evNum=" + evNum + "&viewID=" + viewID,
                new scada.ModalOptions([scada.ModalButtons.OK, scada.ModalButtons.CANCEL]), opt_callback);
        }
    }
}