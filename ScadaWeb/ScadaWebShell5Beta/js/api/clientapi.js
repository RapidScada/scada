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

/********** Input Channel Data **********/

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

// Extended hourly input channel data type
scada.HourCnlDataExt = function () {
    this.Hour = NaN;
    this.Modified = false;
    this.CnlDataExtArr = [];
}

/********** Auxiliary Request Parameters **********/

// Input channel filter type.
// Warning: role access rights are validated only for the view
scada.CnlFilter = function () {
    // Filter by the explicitly specified input channel numbers. No other filtering is applied
    this.cnlNums = [];
    // Filter by input channels included in the view
    this.viewID = 0;
};

// Convert the input channel filter to a query string
scada.CnlFilter.prototype.toQueryString = function () {
    return "cnlNums=" + scada.utils.arrayToQueryParam(this.cnlNums) +
        "&viewID=" + (this.viewID ? this.viewID : 0);
};

// Time period in hours type
scada.HourPeriod = function () {
    // Date is a reference point of the period
    this.date = 0;
    // Start hour relative to the date. May be negative
    this.startHour = 0;
    // End hour relative to the date
    this.endHour = 0;
};

// Convert the time period to a query string
scada.HourPeriod.prototype.toQueryString = function () {
    return "year=" + this.date.getFullYear() +
        "&month=" + (this.date.getMonth() + 1) +
        "&day=" + this.date.getDate() +
        "&startHour=" + this.startHour +
        "&endHour=" + this.endHour;
};

// Hourly data selection modes enumeration
// TODO: rename HourDataModes -> HourDataSelectModes
scada.HourDataModes = {
    // Select data for integer hours even if a snapshot doesn't exist
    INTEGER_HOURS: false,
    // Select existing hourly snapshots
    EXISTING: true
};

/********** Client API **********/

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
                    callback(true, parsedData.Data);
                } else {
                    scada.utils.logServiceError(operation, parsedData.ErrorMessage);
                    callback(false, errorResult);
                }
            } 
            catch (ex) {
                scada.utils.logServiceFormatError(operation);
                if (typeof callback === "function") {
                    callback(false, errorResult);
                }
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            scada.utils.logFailedRequest(operation, jqXHR);
            if (typeof callback === "function") {
                callback(false, errorResult);
            }
        });
    },

    // Extract year, month and day from the date, and join them into a query string
    _dateToQueryString: function (date) {
        return "year=" + date.getFullYear() +
            "&month=" + (date.getMonth() + 1) +
            "&day=" + date.getDate();
    },

    // Extract year, month and day from the date, and join them with the hours into a query string
    _getDateTimeQueryString: function (date, startHour, endHour) {
        return "year=" + date.getFullYear() +
            "&month=" + (date.getMonth() + 1) +
            "&day=" + date.getDate() +
            "&startHour=" + startHour +
            "&endHour=" + endHour;
    },

    // Check that a user is logged on.
    // callback is a function (success, loggedOn)
    checkLoggedOn: function (callback) {
        this._request(
            "ClientApiSvc.svc/CheckLoggedOn", "",
            callback, false);
    },

    // Get current value and status of the input channel.
    // callback is a function (success, cnlData)
    getCurCnlData: function (cnlNum, callback) {
        this._request(
            "ClientApiSvc.svc/GetCurCnlData",
            "?cnlNum=" + cnlNum,
            callback, this._emptyCnlData);
    },

    // Get extended current data of the input channel. 
    // callback is a function (success, cnlDataExt)
    getCurCnlDataExt: function (cnlNum, callback) {
        this._request(
            "ClientApiSvc.svc/GetCurCnlDataExt",
            "?cnlNum=" + cnlNum,
            callback, this._emptyCnlDataExt);
    },

    // Get extended current data of the specified input channels.
    // callback is a function (success, cnlDataExtArr)
    getCurCnlDataExtByCnlNums: function (cnlNums, callback) {
        this._request(
            "ClientApiSvc.svc/GetCurCnlDataExtByCnlNums",
            "?cnlNums=" + cnlNums,
            callback, []);
    },

    // Get extended current data of the input channels of the specified view.
    // callback is a function (success, cnlDataExtArr)
    // TODO: getCurCnlDataExt: function (cnlFilter, callback)
    getCurCnlDataExtByView: function (viewID, callback) {
        this._request(
            "ClientApiSvc.svc/GetCurCnlDataExtByView",
            "?viewID=" + viewID,
            callback, []);
    },

    // Get extended hourly data of the specified input channels.
    // callback is a function (success, hourCnlDataExtArr)
    getHourCnlDataExtByCnlNums: function (date, startHour, endHour, cnlNums, mode, callback) {
        this._request(
            "ClientApiSvc.svc/GetHourCnlDataExtByCnlNums",
            "?" + this._getDateTimeQueryString(date, startHour, endHour) + "&cnlNums=" + cnlNums + "&existing=" + mode,
            callback, []);
    },

    // Get extended hourly data of the input channels of the specified view.
    // callback is a function (success, hourCnlDataExtArr)
    // TODO: getHourCnlData: function (hourPeriod, cnlFilter, selectMode, dataAge /*array*/, callback)
    getHourCnlDataExtByView: function (date, startHour, endHour, viewID, mode, callback) {
        this._request(
            "ClientApiSvc.svc/GetHourCnlDataExtByView",
            "?" + this._getDateTimeQueryString(date, startHour, endHour) + "&viewID=" + viewID + "&existing=" + mode,
            callback, []);
    },

    // Get events for the specified date and channel filter.
    // callback is a function (success, eventExtArr, dataAge)
    getEvents: function (date, cnlFilter, lastCount, startEvNum, dataAge, callback) {
        this._request(
            "ClientApiSvc.svc/GetEvents",
            "?" + this._dateToQueryString(date) + "&" + cnlFilter.toQueryString() +
            "&lastCount=" + lastCount + "&startEvNum=" + startEvNum + "&dataAge=" + dataAge,
            callback, []);
    },

    // Get the stamp of the view from the cache.
    // callback is a function (success, stamp)
    getViewStamp: function (viewID, callback) {
        this._request("ClientApiSvc.svc/GetViewStamp", "?viewID=" + viewID, callback, 0);
    },

    // Parse date and time using the application culture
    // callback is a function (success, value),
    // value is the number of milliseconds or null in case of any error
    parseDateTime: function (s, callback) {
        this._request("ClientApiSvc.svc/ParseDateTime", "?s=" + s, callback, null);
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
            return new Map();
        }
    },
    
    // Create map of extended hourly input channel data to access by hour
    createHourCnlDataExtMap: function (hourCnlDataExtArr) {
        try {
            var map = new Map();
            for (var hourCnlDataExt of hourCnlDataExtArr) {
                map.set(hourCnlDataExt.Hour, hourCnlDataExt);
            }
            return map;
        }
        catch (ex) {
            console.error(scada.utils.getCurTime() + " Error creating map of extended hourly input channel data:",
                ex.message);
            return new Map();
        }
    }
};
