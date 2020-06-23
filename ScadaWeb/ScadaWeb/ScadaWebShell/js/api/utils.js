/*
 * JavaScript utilities
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2020
 *
 * No dependencies
 */

// Rapid SCADA namespace
var scada = scada || {};

// JavaScript utilities object
scada.utils = {
    // Assumed browser scrollbar width
    _SCROLLBAR_WIDTH: 20,

    // z-index that moves element to the front
    FRONT_ZINDEX: 10000,

    // Default cookie expiration period in days
    COOKIE_EXPIRATION: 7,

    // Window width that is considered a small
    SMALL_WND_WIDTH: 800,

    // Get cookie
    getCookie: function (name) {
        var cookie = " " + document.cookie;
        var search = " " + name + "=";
        var offset = cookie.indexOf(search);

        if (offset >= 0) {
            offset += search.length;
            var end = cookie.indexOf(";", offset);

            if (end < 0)
                end = cookie.length;

            return decodeURIComponent(cookie.substring(offset, end));
        } else {
            return null;
        }
    },

    // Set cookie
    setCookie: function (name, value, opt_expDays) {
        var expDays = opt_expDays ? opt_expDays : this.COOKIE_EXPIRATION;
        var expires = new Date();
        expires.setDate(expires.getDate() + expDays);
        document.cookie = name + "=" + encodeURIComponent(value) + "; expires=" + expires.toUTCString() +
            "; samesite=lax";
    },

    // Get the query string parameter value
    getQueryParam: function (paramName, opt_url) {
        if (paramName) {
            var url = opt_url ? opt_url : decodeURIComponent(window.location);
            var begInd = url.indexOf("?");

            if (begInd > 0) {
                url = "&" + url.substring(begInd + 1);
            }

            paramName = "&" + paramName + "=";
            begInd = url.indexOf(paramName);

            if (begInd >= 0) {
                begInd += paramName.length;
                var endInd = url.indexOf("&", begInd);
                return endInd >= 0 ? url.substring(begInd, endInd) : url.substring(begInd);
            }
        }

        return "";
    },

    // Set or add the query string parameter value.
    // The method returns a new string
    setQueryParam: function (paramName, paramVal, opt_url) {
        if (paramName) {
            var url = opt_url ? opt_url : decodeURIComponent(window.location);
            var searchName = "?" + paramName + "=";
            var nameBegInd = url.indexOf(searchName);

            if (nameBegInd < 0) {
                searchName = "&" + paramName + "=";
                nameBegInd = url.indexOf(searchName);
            }

            if (nameBegInd >= 0) {
                // replace parameter value
                var valBegInd = nameBegInd + searchName.length;
                var valEndInd = url.indexOf("&", valBegInd);
                var newUrl = url.substring(0, valBegInd) + encodeURIComponent(paramVal);
                return valEndInd > 0 ?
                    newUrl + url.substring(valEndInd) :
                    newUrl;
            } else {
                // add parameter
                var mark = url.indexOf("?") >= 0 ? "&" : "?";
                return url + mark + paramName + "=" + encodeURIComponent(paramVal);
            }
        } else {
            return "";
        }
    },

    // Convert the value of the query string parameter to an array of integers
    queryParamToIntArray: function (paramVal) {
        var arr = [];

        for (var elemStr of paramVal.split(",")) {
            arr.push(parseInt(elemStr));
        }

        return arr;
    },

    // Convert array to a query string parameter by joining array elements with a comma
    arrayToQueryParam: function (arr) {
        var queryParam = arr ? Array.isArray(arr) ? arr.join(",") : arr : "";
        // space instead of empty string is required by Mono WCF implementation
        return encodeURIComponent(queryParam ? queryParam : " ");
    },

    // Extract year, month and day from the date, and join them into a query string
    dateToQueryString: function (date, opt_prependAmp) {
        return date ? 
            (opt_prependAmp ? "&" : "") +
            "year=" + date.getFullYear() +
            "&month=" + (date.getMonth() + 1) +
            "&day=" + date.getDate() :
            "";
    },

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

    // Write information about the failed request to console
    logFailedRequest: function (operation, jqXHR) {
        console.error(this.getCurTime() + " Request '" + operation + "' failed: " +
            jqXHR.status + " (" + jqXHR.statusText + ")");
    },

    // Write information about the internal service error to console
    logServiceError: function (operation,  opt_message) {
        console.error(this.getCurTime() + " Request '" + operation + "' reports internal service error" +
            (opt_message ? ": " + opt_message : ""));
    },

    // Write information about the request processing error to console
    logProcessingError: function (operation, opt_message) {
        console.error(this.getCurTime() + " Error processing request '" + operation + "'" +
            (opt_message ? ": " + opt_message : ""));
    },

    // Perform an Ajax request in assumption that response is a DataTransferObject in JSON format.
    // ajaxObj is AjaxQueue or jQuery,
    // url is relative to the site root,
    // action is a function (data),
    // callback is a function (success)
    request: function (ajaxObj, url, action, callback) {
        var thisObj = this;
        var searchInd = url.indexOf('?');
        var operation = searchInd >= 0 ? url.substr(0, searchInd) : url;

        if (ajaxObj) {
            ajaxObj
                .ajax({
                    url: (ajaxObj.rootPath || "") + url,
                    method: "GET",
                    dataType: "json",
                    cache: false
                })
                .done(function (data, textStatus, jqXHR) {
                    try {
                        var parsedData = $.parseJSON(data.d);
                        if (parsedData.Success) {
                            thisObj.logSuccessfulRequest(operation);
                            action(parsedData.Data);
                            callback(true);
                        } else {
                            thisObj.logServiceError(operation, parsedData.ErrorMessage);
                            callback(false);
                        }
                    }
                    catch (ex) {
                        thisObj.logProcessingError(operation, ex.message);
                        callback(false);
                    }
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    thisObj.logFailedRequest(operation, jqXHR);
                    callback(false);
                });
        } else {
            console.error(this.getCurTime() + " Unable to send request '" + operation + "'");
        }
    },

    // Check that the frame is accessible by the same-origin policy
    frameAvailable(frameWnd) {
        try {
            var x = frameWnd.location.href;
            return true;
        } catch (ex) {
            return false;
        }
    },        

    // Check if browser is in fullscreen mode switched on programmatically
    // See https://developer.mozilla.org/en-US/docs/Web/API/Fullscreen_API
    isFullscreen: function() {
        return document.fullscreenElement || document.mozFullScreenElement ||
            document.webkitFullscreenElement || document.msFullscreenElement;
    },

    // Check if browser is in fullscreen mode
    isRealFullscreen: function () {
        return screen.height - window.innerHeight <= 1;
    },

    // Switch browser to fullscreen mode
    requestFullscreen: function () {
        if (document.documentElement.requestFullscreen) {
            document.documentElement.requestFullscreen();
        } else if (document.documentElement.msRequestFullscreen) {
            document.documentElement.msRequestFullscreen();
        } else if (document.documentElement.mozRequestFullScreen) {
            document.documentElement.mozRequestFullScreen();
        } else if (document.documentElement.webkitRequestFullscreen) {
            document.documentElement.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
        }
    },

    // Exit browser fullscreen mode
    exitFullscreen: function () {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.msExitFullscreen) {
            document.msExitFullscreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitExitFullscreen) {
            document.webkitExitFullscreen();
        }
    },

    // Switch browser to full screen mode and back to normal view
    toggleFullscreen: function () {
        if (this.isFullscreen()) {
            this.exitFullscreen();
        } else {
            this.requestFullscreen();
        }
    },

    // Check if a browser window is small sized
    isSmallScreen() {
        return top.innerWidth <= this.SMALL_WND_WIDTH;
    },

    // Get browser scrollbar width
    getScrollbarWidth: function () {
        return this._SCROLLBAR_WIDTH;
    },

    // Click hyperlink programmatically
    clickLink: function (jqLink) {
        var href = jqLink.attr("href");
        if (href) {
            if (href.startsWith("javascript:")) {
                // execute script
                var script = href.substr(11);
                eval(script);
            } else {
                // open web page
                location.href = href;
            }
        }
    },

    // Scroll the first specified element to make the second element visible if it exists
    scrollTo: function (jqScrolledElem, jqTargetElem) {
        if (jqTargetElem.length > 0) {
            var targetTop = jqTargetElem.offset().top;

            if (jqScrolledElem.scrollTop() > targetTop) {
                jqScrolledElem.scrollTop(targetTop);
            }
        }
    },

    // Set frame source creating new frame to prevent writing frame history. Returns the new frame
    setFrameSrc: function (jqFrame, url) {
        var frameParent = jqFrame.parent();
        var frameClone = jqFrame.clone();
        jqFrame.remove();
        frameClone.attr("src", url);
        frameClone.appendTo(frameParent);
        return frameClone;
    },

    // Detect if iOS is used
    iOS: function () {
        return /iPad|iPhone|iPod/.test(navigator.platform);
    },

    // Apply additional css styles to a container element in case of using iOS
    styleIOS: function (jqElem, opt_resetSize) {
        if (this.iOS()) {
            jqElem.css({
                "overflow": "scroll",
                "-webkit-overflow-scrolling": "touch"
            });

            if (opt_resetSize) {
                jqElem.css({
                    "width": 0,
                    "height": 0
                });
            }
        }
    },

    // Get URL of the view by its ID
    getViewUrl: function (viewID, opt_openInFrame) {
        return (opt_openInFrame ? "ViewFrame.aspx?viewID=" : "View.aspx?viewID=") + viewID;
    },

    // Check that the frame is accessible due to the browser security
    checkAccessToFrame: function (frameWnd) {
        try {
            return frameWnd.document !== null;
        } catch (ex) {
            return false;
        }
    },

    // Play a sound of the audio element
    playSound: function (jqAudio) {
        if (jqAudio.length > 0) {
            var thisObj = this;
            var promise = jqAudio[0].play();
            promise.catch(function (error) {
                console.error(thisObj.getCurTime() + " Error playing sound '" + jqAudio.attr("src") + "': " + error);
            });
        }
    }
};
