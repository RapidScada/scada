/*
 * Rapid SCADA client API for access data and sending commands
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2018
 *
 * Requires:
 * - jquery
 * - utils.js
 * - ajaxqueue.js
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

// Input channel filter type.
// Only one filter criteria is applied, others are ignored. Filter criteria priority: cnlNums, viewID
scada.CnlFilter = function () {
    // Filter by the explicitly specified input channel numbers
    this.cnlNums = [];
    // View IDs correspond to the input channels for rights validation
    this.viewIDs = [];
    // Filter by input channels included in the view
    this.viewID = 0;
};

// Convert the input channel filter to a query string
scada.CnlFilter.prototype.toQueryString = function () {
    return "cnlNums=" + scada.utils.arrayToQueryParam(this.cnlNums) +
        "&viewIDs=" + scada.utils.arrayToQueryParam(this.viewIDs) +
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

    // Ajax queue used for request sequencing. Can be null
    ajaxQueue: null,

    // Execute an Ajax request
    _request: function (operation, queryString, callback, errorResult) {
        var ajaxObj = this.ajaxQueue ? this.ajaxQueue : $;

        ajaxObj.ajax({
            url: (this.ajaxQueue ? this.ajaxQueue.rootPath : this.rootPath) + operation + queryString,
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

    // Perform user login.
    // callback is a function (success, loggedOn)
    // URL example: http://webserver/scada/ClientApiSvc.svc/Login?username=admin&password=12345
    login: function (username, password, callback) {
        this._request("ClientApiSvc.svc/Login",
            "?username=" + encodeURIComponent(username) + "&password=" + encodeURIComponent(password),
            callback, false);
    },

    // Check that a user is logged on.
    // callback is a function (success, loggedOn)
    // URL example: http://webserver/scada/ClientApiSvc.svc/CheckLoggedOn
    checkLoggedOn: function (callback) {
        this._request("ClientApiSvc.svc/CheckLoggedOn", "", callback, null);
    },

    // Get current data of the input channel.
    // callback is a function (success, cnlData)
    // URL example: http://webserver/scada/ClientApiSvc.svc/GetCurCnlData?cnlNum=1
    getCurCnlData: function (cnlNum, callback) {
        this._request("ClientApiSvc.svc/GetCurCnlData", "?cnlNum=" + cnlNum, callback, this._EMPTY_CNL_DATA);
    },

    // Get extended current data by the specified filter.
    // callback is a function (success, cnlDataExtArr)
    // URL example: http://webserver/scada/ClientApiSvc.svc/GetCurCnlDataExt?cnlNums=&viewIDs=&viewID=1
    getCurCnlDataExt: function (cnlFilter, callback) {
        this._request("ClientApiSvc.svc/GetCurCnlDataExt", "?" + cnlFilter.toQueryString(), callback, []);
    },

    // Get hourly data by the specified filter.
    // dataAge is an array of dates in milliseconds,
    // callback is a function (success, hourCnlDataArr, dataAge)
    // URL example: http://webserver/scada/ClientApiSvc.svc/GetHourCnlData?year=2016&month=1&day=1&startHour=0&endHour=23&cnlNums=&viewIDs=&viewID=1&existing=true&dataAge=
    getHourCnlData: function (hourPeriod, cnlFilter, selectMode, dataAge, callback) {
        this._request("ClientApiSvc.svc/GetHourCnlData",
            "?" + hourPeriod.toQueryString() + "&" + cnlFilter.toQueryString() + "&existing=" + selectMode +
            "&dataAge=" + scada.utils.arrayToQueryParam(dataAge),
            callback, []);
    },

    // Get events by the specified filter.
    // callback is a function (success, eventArr, dataAge)
    // URL example: http://webserver/scada/ClientApiSvc.svc/GetEvents?year=2016&month=1&day=1&cnlNums=&viewIDs=&viewID=1&lastCount=100&startEvNum=0&dataAge=0
    getEvents: function (date, cnlFilter, lastCount, startEvNum, dataAge, callback) {
        this._request("ClientApiSvc.svc/GetEvents",
            "?" + scada.utils.dateToQueryString(date) + "&" + cnlFilter.toQueryString() +
            "&lastCount=" + lastCount + "&startEvNum=" + startEvNum + "&dataAge=" + dataAge,
            callback, []);
    },

    // Get the stamp of the view from the cache.
    // callback is a function (success, stamp)
    // URL example: http://webserver/scada/ClientApiSvc.svc/GetViewStamp?viewID=1
    getViewStamp: function (viewID, callback) {
        this._request("ClientApiSvc.svc/GetViewStamp", "?viewID=" + viewID, callback, 0);
    },

    // Parse date and time using the application culture
    // callback is a function (success, value),
    // value is the number of milliseconds or null in case of any error
    // URL example: http://webserver/scada/ClientApiSvc.svc/ParseDateTime?s=01%20January%202016
    parseDateTime: function (s, callback) {
        this._request("ClientApiSvc.svc/ParseDateTime", "?s=" + encodeURIComponent(s), callback, null);
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
