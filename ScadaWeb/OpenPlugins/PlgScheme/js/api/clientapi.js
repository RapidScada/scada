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

// Input channel data type.
// Note: Casing is caused by C# naming rules
scada.CnlData = function () {
    this.Val = 0.0;
    this.Stat = 0;
};

// Extended input channel data type
scada.CnlDataExt = function () {
    scada.CnlData.call(this);

    this.CnlNum = 0;
    this.Text = "";
    this.TextWithUnit = "";
    this.Color = "";
};

scada.CnlDataExt.prototype = Object.create(scada.CnlData.prototype);
scada.CnlDataExt.constructor = scada.CnlDataExt;

// Client API object
scada.clientAPI = {
    // Empty input channel data
    _emptyCnlData: Object.freeze(new scada.CnlData()),

    // Empty extended input channel data
    _emptyCnlDataExt: Object.freeze(new scada.CnlDataExt()),

    // Web service root path
    rootPath: "",

    // Execute an AJAX request
    _request: function (operation, queryString, callback, errorResult) {
        $.ajax({
            url: this.rootPath + operation + queryString,
            method: "GET",
            dataType: "json"
        })
        .done(function (data, textStatus, jqXHR) {
            try {
                var parsedData = $.parseJSON(data.d);
                if (parsedData.Success) {
                    scada.utils.logSuccessfulRequest(operation/*, data*/);
                    callback(true, parsedData.Data ? parsedData.Data : parsedData);
                } else {
                    scada.utils.logServiceError(operation, parsedData.ErrorMessage);
                    callback(false, errorResult);
                }
            } 
            catch (ex) {
                scada.utils.logServiceFormatError(operation);
                callback(false, errorResult);
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            scada.utils.logFailedRequest(operation, jqXHR);
            callback(false, errorResult);
        });
    },

    // Get current value and status of the input channel.
    // callback is function (success, cnlData)
    getCurCnlData: function (cnlNum, cnlNum) {
        this._request(
            "ClientApiSvc.svc/GetCurCnlData",
            "?cnlNum=" + cnlNum,
            callback, this._emptyCnlData);
    },

    // Get extended current data of the input channel. 
    // callback is function (success, cnlDataExt)
    getCurCnlDataExt: function (cnlNum, callback) {
        this._request(
            "ClientApiSvc.svc/GetCurCnlDataExt",
            "?cnlNum=" + cnlNum,
            callback, this._emptyCnlDataExt);
    },

    // Get extended current data of the specified input channels.
    // callback is function (success, cnlDataExtArr)
    getCurCnlDataExtByCnlNums: function (cnlNums, callback) {
        this._request(
            "ClientApiSvc.svc/GetCurCnlDataExtByCnlNums",
            "?cnlNums=" + cnlNums,
            callback, []);
    },

    // Get extended current input channel data of the view.
    // callback is function (success, cnlDataExtArr)
    getCurCnlDataExtByView: function (viewID, callback) {
        this._request(
            "ClientApiSvc.svc/GetCurCnlDataExtByView",
            "?viewID=" + viewID,
            callback, []);
    },

    // Get the stamp of the view from the cache.
    // callback is function (success, stamp)
    getViewStamp: function (viewID, callback) {
        this._request(
            "ClientApiSvc.svc/GetViewStamp",
            "?viewID=" + viewID,
            callback, 0);
    }
};
