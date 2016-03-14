/*
 * Rapid SCADA client API for access data and sending commands
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

// Rapid SCADA namespace
var scada = scada || {};

// Client API object
scada.clientAPI = {
    // Web service root path
    rootPath: "",

    // Get current value and status of the input channel.
    // callback is function (success, val, stat)
    getCurCnlData: function (cnlNum, callback) {
        var operation = "ClientApiSvc.svc/Test";

        $.ajax({
            url: this.rootPath + operation,
            method: "GET",
            dataType: "json"
        })
        .done(function (data, textStatus, jqXHR) {
            console.log("Request '" + operation + "' successful");
            console.log(data.d);
            callback(true, 0.0, 0);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            console.error("Request '" + operation + "' failed: " + jqXHR.status + " (" + jqXHR.statusText + ")");
            callback(false, 0.0, 0);
        });
    },

    // Get full current data of the input channel. 
    // callback is function (success, cnlData)
    getCurCnlDataFull: function (cnlNum, callback) {
        callback(true, new scada.CnlData());
    }
};

// Input channel data type.
// Note: Casing is caused by C# naming rules
scada.CnlData = function () {
    this.Val = 0.0;
    this.Stat = 0;
    this.Text = "";
    this.TextWithUnit = "";
    this.Color = "";
};
