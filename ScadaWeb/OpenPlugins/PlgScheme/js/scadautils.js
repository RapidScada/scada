/*
 * JavaScript utilities
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

// Rapid SCADA namespace
var scada = scada || {};

scada.utils = {
    // Returns the current time string
    getCurTime: function () {
        return new Date().toLocaleTimeString("en-GB");
    },

    // Write information about the successful request to console
    logSuccessfulRequest: function (operation, opt_data) {
        console.log(this.getCurTime() + " Request '" + operation + "' successful");
        if (opt_data) {
            console.log(opt_data.d);
        }
    },

    // Write information about the internal service error to console
    logServiceError: function (operation) {
        console.error(this.getCurTime() + " Request '" + operation + "' returns empty data. Internal service error");
    },

    // Write information about the failed request to console
    logFailedRequest: function (operation, jqXHR) {
        console.error(this.getCurTime() + " Request '" + operation + "' failed: " +
            jqXHR.status + " (" + jqXHR.statusText + ")");
    }
};
