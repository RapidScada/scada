/*
 * Export utilities
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 *
 * No dependencies
 */

// Rapid SCADA namespace
var scada = scada || {};

// Export formats enumeration
scada.ExportFormats = {
    PDF: "pdf",
    PNG: "png",
    EXCEL: "xml"
};

// Export utilities
scada.export = {
    // Duration of locking the export button after click, ms
    EXPORT_BTN_LOCK: 3000,

    // Add leading zero if the specified number less than 10
    _addLeadingZero: function (value) {
        return value < 10 ? "0" + value : value;
    },

    // Build file name of an output document
    buildFileName: function (prefix, extension) {
        var now = new Date();
        return prefix + "_" + now.getFullYear() + "-" + this._addLeadingZero((now.getMonth() + 1)) + "-" +
            this._addLeadingZero(now.getDate()) + "_" + this._addLeadingZero(now.getHours()) + "-" +
            this._addLeadingZero(now.getMinutes()) + "-" + this._addLeadingZero(now.getSeconds()) + "." +
            extension;
    },

    // Lock the export button after click
    lockExportButton: function (jqExportBtn) {
        jqExportBtn.prop("disabled", true);
        setTimeout(function () { jqExportBtn.prop("disabled", false); }, this.EXPORT_BTN_LOCK)
    }
}