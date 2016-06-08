/*
 * Rapid SCADA client API for access data and sending commands
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 *
 * Requires:
 * - jquery
 * - utils.js
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

// Hourly input channel data type
scada.HourCnlData = function () {
    this.Hour = NaN;
    this.CnlDataExtArr = [];
}

// Getting hour data modes enumeration
scada.HourDataModes = {
    // Get data for integer hours even if a snapshot doesn't exist
    INTEGER_HOURS: false,
    // Get existing snapshots
    EXISTING: true
};

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
            dataType: "json",
            cache: false
        })
        .done(function (data, textStatus, jqXHR) {
            try {
                var parsedData = $.parseJSON(data.d);
                if (parsedData.Success) {
                    scada.utils.logSuccessfulRequest(operation/*, data*/);
                    callback(true, parsedData.Data == null ? parsedData : parsedData.Data);
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

    // Extract year, month and day from the date and join them into query string
    _getDateQueryString: function (date, opt_startHour, opt_endHour) {
        return
            "year=" + date.getFullYear() +
            "&month=" + (date.getMonth() + 1) +
            "&date=" + date.getDay() +
            (opt_startHour ? "&startHour=" + opt_startHour : "") +
            (opt_endHour ? "&endHour=" + opt_endHour : "");
    },

    // Check that a user is logged on.
    // callback is function (success, loggedOn)
    checkLoggedOn: function (callback) {
        this._request(
            "ClientApiSvc.svc/CheckLoggedOn", "",
            callback, false);
    },

    // Get current value and status of the input channel.
    // callback is function (success, cnlData)
    getCurCnlData: function (cnlNum, callback) {
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

    // Get extended current data of the input channels of the specified view.
    // callback is function (success, cnlDataExtArr)
    getCurCnlDataExtByView: function (viewID, callback) {
        this._request(
            "ClientApiSvc.svc/GetCurCnlDataExtByView",
            "?viewID=" + viewID,
            callback, []);
    },

    // Get extended hourly data of the specified input channels.
    // callback is function (success, hourCnlDataArr)
    getHourCnlDataExtByCnlNums: function (date, startHour, endHour, cnlNums, mode, callback) {
        this._request(
            "ClientApiSvc.svc/GetHourCnlDataExtByCnlNums",
            "?" + this._getDateQueryString(date, startHour, endHour) + "&cnlNums=" + cnlNums + "&existing=" + mode,
            callback, []);
    },

    // Get extended hourly data of the input channels of the specified view.
    // callback is function (success, hourCnlDataArr)
    getHourCnlDataExtByView: function (date, startHour, endHour, viewID, mode, callback) {
        this._request(
            "ClientApiSvc.svc/GetHourCnlDataExtByView",
            "?" + this._getDateQueryString(date, startHour, endHour) + "&viewID=" + viewID + "&existing=" + mode,
            callback, []);
    },

    // Get the stamp of the view from the cache.
    // callback is function (success, stamp)
    getViewStamp: function (viewID, callback) {
        this._request(
            "ClientApiSvc.svc/GetViewStamp",
            "?viewID=" + viewID,
            callback, 0);
    },
    
    // Create map of extended input channel data to access by channel number
    createCnlDataExtMap: function (cnlDataExtArr) {
        try {
            var map = new Map();
            for (var cnlDataExt of cnlDataExtArr) {
                map.set(cnlDataExt.CnlNum, cnlDataExt);
            }
            return map;
        }
        catch (ex) {
            console.error(scada.utils.getCurTime() + " Error creating map of extended input channel data:",
                ex.message);
            return null;
        }
    }
};
