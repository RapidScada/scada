/*
 * Queue for sending Ajax requests one after another.
 * Allows to avoid Mono WCF bug and improves load balancing
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 *
 * Requires:
 * - jquery
 */

// Rapid SCADA namespace
var scada = scada || {};

/********** Ajax Queue **********/

// Ajax queue type
scada.AjaxQueue = function (opt_rootPath) {
    // Abnormal size of the queue. Need to warn a user
    this.WARN_QUEUE_SIZE = 100;
    // Web application root relative to the object location
    this.rootPath = opt_rootPath,
    // Ajax requests to send
    this.requests = [];
    // ID of a timeout that is used to send the next request
    this.timeoutID = 0;
};

// Perform the first request from the queue and initiate sending of the next one
scada.AjaxQueue.prototype._request = function () {
    if (this.requests.length > 0) {
        var thisObj = this;
        var ajaxRequest = this.requests.shift();

        $.ajax(ajaxRequest.settings)
            .done(function (data, textStatus, jqXHR) {
                if (typeof ajaxRequest.doneCallback === "function") {
                    ajaxRequest.doneCallback(data, textStatus, jqXHR);
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                if (typeof ajaxRequest.failCallback === "function") {
                    ajaxRequest.failCallback(jqXHR, textStatus, errorThrown);
                }
            })
            .always(function (data_jqXHR, textStatus, jqXHR_errorThrown) {
                if (typeof ajaxRequest.alwaysCallback === "function") {
                    ajaxRequest.alwaysCallback(data_jqXHR, textStatus, jqXHR_errorThrown);
                }

                if (thisObj.requests.length > 0) {
                    thisObj.timeoutID = setTimeout(thisObj._request.bind(thisObj), 0);
                } else {
                    thisObj.timeoutID = 0;
                }
            });
    }
};

// Start sending process if it is not running
scada.AjaxQueue.prototype._run = function () {
    if (this.timeoutID <= 0) {
        this.timeoutID = setTimeout(this._request.bind(this), 0);
    }
};

// Append the Ajax request to the queue
scada.AjaxQueue.prototype.append = function (ajaxRequest) {
    if (ajaxRequest) {
        this.requests.push(ajaxRequest);

        if (this.requests.length >= this.WARN_QUEUE_SIZE) {
            console.warn("Ajax queue size is " + this.requests.lenght);
        }

        this._run();
    }
};

// Create new Ajax request and append it to the queue
scada.AjaxQueue.prototype.ajax = function (settings) {
    var ajaxRequest = new scada.AjaxRequest(settings);
    this.append(ajaxRequest);
    return ajaxRequest;
};

/********** Ajax Request **********/

// Ajax request type
scada.AjaxRequest = function (settings) {
    this.settings = settings;
    this.doneCallback = null;
    this.failCallback = null;
    this.alwaysCallback = null;
};

// Define the success callback function
scada.AjaxRequest.prototype.done = function (doneCallback) {
    this.doneCallback = doneCallback;
    return this;
};

// Define the error callback function
scada.AjaxRequest.prototype.fail = function (failCallback) {
    this.failCallback = failCallback;
    return this;
};

// Define the complete callback function
scada.AjaxRequest.prototype.always = function (alwaysCallback) {
    this.alwaysCallback = alwaysCallback;
    return this;
};

// Define this request to the queue
scada.AjaxRequest.prototype.appendTo = function (ajaxQueue) {
    ajaxQueue.append(this);
    return this;
}

/********** Ajax Queue Locator **********/

// Ajax queue locator object
scada.ajaxQueueLocator = {
    // Find and return an existing ajax queue object
    getAjaxQueue: function () {
        var wnd = window;
        while (wnd) {
            if (wnd.ajaxQueue) {
                return wnd.ajaxQueue;
            }
            wnd = wnd == window.top ? null : window.parent;
        }
        return null;
    }
};