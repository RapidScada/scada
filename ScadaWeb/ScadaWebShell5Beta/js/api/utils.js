/*
 * JavaScript utilities
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 *
 * No dependencies
 */

// Rapid SCADA namespace
var scada = scada || {};

// JavaScript utilities object
scada.utils = {
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
            var end = cookie.indexOf(";", offset)

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
        document.cookie = name + "=" + encodeURIComponent(value) + "; expires=" + expires.toUTCString();
    },

    // Get the query string parameter value
    getQueryParam: function (paramName, opt_url) {
        if (paramName) {
            var url = opt_url ? opt_url : decodeURIComponent(window.location);
            var begInd = queryString.indexOf("?");

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

    // Convert array to a query string parameter by joining array elements with a comma
    arrayToQueryParam: function (arr) {
        var queryParam = arr ? (Array.isArray(arr) ? arr.join(",") : arr) : "";
        // space instead of empty string is required by Mono WCF implementation
        return encodeURIComponent(queryParam ? queryParam : " ");
    },

    // Extract year, month and day from the date, and join them into a query string
    dateToQueryString: function (date) {
        return "year=" + date.getFullYear() +
            "&month=" + (date.getMonth() + 1) +
            "&day=" + date.getDate();
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

    // Check if browser is in fullscreen mode
    // See https://developer.mozilla.org/en-US/docs/Web/API/Fullscreen_API
    isFullscreen: function() {
        return document.fullscreenElement || document.mozFullScreenElement ||
            document.webkitFullscreenElement || document.msFullscreenElement;
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

    // Detect if iOS is used
    iOS: function () {
        return /iPad|iPhone|iPod/.test(navigator.platform);
    }
};
