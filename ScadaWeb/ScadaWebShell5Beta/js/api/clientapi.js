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

// Hourly input channel data type
scada.HourCnlData = function () {
    this.Hour = NaN;
    this.Modified = false;
    this.CnlDataExtArr = [];
}

/********** Event **********/

// Event type
scada.Event = function () {
    this.Num = 0;
    this.Time = "";
    this.Obj = "";
    this.KP = "";
    this.Cnl = "";
    this.Text = "";
    this.Ack = "";
    this.Color = "";
    this.Sound = false;
};

/********** Auxiliary Request Parameters **********/

// Input channel filter type
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
    _EMPTY_CNL_DATA: Object.freeze(new scada.CnlData()),

    // Empty extended input channel data
    _EMPTY_CNL_DATA_EXT: Object.freeze(new scada.CnlDataExt()),

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
                    if (typeof parsedData.DataAge === "undefined") {
                        callback(true, parsedData.Data);
                    } else {
                        callback(true, parsedData.Data, parsedData.DataAge);
                    }
                } else {
                    scada.utils.logServiceError(operation, parsedData.ErrorMessage);
                    callback(false, errorResult);
                }
            } 
            catch (ex) {
                scada.utils.logProcessingError(operation, ex.message);
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

    // Check that a user is logged on.
    // callback is a function (success, loggedOn)
    checkLoggedOn: function (callback) {
        this._request("ClientApiSvc.svc/CheckLoggedOn", "", callback, false);
    },

    // Get current data of the input channel.
    // callback is a function (success, cnlData)
    getCurCnlData: function (cnlNum, callback) {
        this._request("ClientApiSvc.svc/GetCurCnlData", "?cnlNum=" + cnlNum, callback, this._EMPTY_CNL_DATA);
    },

    // Get extended current data by the specified filter.
    // callback is a function (success, cnlDataExtArr)
    getCurCnlDataExt: function (cnlFilter, callback) {
        this._request("ClientApiSvc.svc/GetCurCnlDataExt", "?" + cnlFilter.toQueryString(), callback, []);
    },

    // Get hourly data by the specified filter.
    // dataAge is an array of dates in milliseconds,
    // callback is a function (success, hourCnlDataArr, dataAge)
    getHourCnlData: function (hourPeriod, cnlFilter, selectMode, dataAge, callback) {
        this._request("ClientApiSvc.svc/GetHourCnlData",
            "?" + hourPeriod.toQueryString() + "&" + cnlFilter.toQueryString() + "&existing=" + selectMode +
            "&dataAge=" + scada.utils.arrayToQueryParam(dataAge),
            callback, []);
    },

    // Get events by the specified filter.
    // callback is a function (success, eventArr, dataAge)
    getEvents: function (date, cnlFilter, lastCount, startEvNum, dataAge, callback) {
        this._request("ClientApiSvc.svc/GetEvents",
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
    
    // Create map of hourly input channel data to access by hour
    createHourCnlDataMap: function (hourCnlDataArr) {
        try {
            var map = new Map();
            for (var hourCnlData of hourCnlDataArr) {
                map.set(hourCnlData.Hour, hourCnlData);
            }
            return map;
        }
        catch (ex) {
            console.error(scada.utils.getCurTime() + " Error creating map of hourly input channel data:",
                ex.message);
            return new Map();
        }
    }
};
