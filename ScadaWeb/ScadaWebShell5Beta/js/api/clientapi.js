/*
 * Rapid SCADA client API for access data and sending commands
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

/*
 * Requires:
 * - jquery
 * - scadautils.js
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
        var operation = "ClientApiSvc.svc/GetCurCnlData";

        $.ajax({
            url: this.rootPath + operation +
                "?cnlNum=" + cnlNum,
            method: "GET",
            dataType: "json"
        })
        .done(function (data, textStatus, jqXHR) {
            if (data.d) {
                scada.utils.logSuccessfulRequest(operation, data);
                var parsedData = $.parseJSON(data.d);
                callback(true, parsedData.Val, parsedData.Stat);
            } else {
                scada.utils.logServiceError(operation);
                callback(false, 0.0, 0);
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            scada.utils.logFailedRequest(operation, jqXHR);
            callback(false, 0.0, 0);
        });
    },

    // Get full current data of the input channel. 
    // callback is function (success, cnlData)
    getCurCnlDataFull: function (cnlNum, callback) {
        var operation = "ClientApiSvc.svc/GetCurCnlDataFull";

        $.ajax({
            url: this.rootPath + operation +
                "?cnlNum=" + cnlNum,
            method: "GET",
            dataType: "json"
        })
        .done(function (data, textStatus, jqXHR) {
            if (data.d) {
                scada.utils.logSuccessfulRequest(operation, data);
                var parsedData = $.parseJSON(data.d);
                callback(true, parsedData);
            } else {
                scada.utils.logServiceError(operation);
                callback(false, new scada.CnlData());
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            scada.utils.logFailedRequest(operation, jqXHR);
            callback(false, new scada.CnlData());
        });
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
