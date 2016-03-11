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
    // Get current value of the input channel.
    // callback is function (success, cnlVal)
    getCnlVal: function (cnlNum, callback) {
        var val = new Date().getSeconds();
        callback(true, val);
    },

    // Get current status of the input channel.
    // callback is function (success, cnlStat)
    getCnlStat: function (cnlNum, callback) {
        callback(true, 1);
    },

    // Get current data of the input channel. 
    // callback is function (success, cnlDataView)
    getCnlData: function (cnlNum, callback) {
        callback(true, new scada.CnlDataView());
    }
};

// Input channel data type
scada.CnlDataView = function () {
    this.val = 0.0;
    this.stat = 0;
    this.text = "";
    this.textWithUnit = "";
    this.color = "";
};