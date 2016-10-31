/*
 * Command object that implements showing of the appropriate dialog
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
// Command namespace
scada.cmd = scada.cmd || {};

scada.cmd.dialog = {
    // Show command dialog
    show: function (rootPath, ctrlCnlNum, viewID, opt_callback) {
        var popup = scada.popupLocator.getPopup();
        if (popup) {
            popup.showModal(rootPath + "plugins/Table/Command.aspx?ctrlCnlNum=" + ctrlCnlNum + "&viewID=" + viewID,
                new scada.ModalOptions([scada.ModalButtons.EXEC, scada.ModalButtons.CLOSE]), opt_callback);
        }
    }
}