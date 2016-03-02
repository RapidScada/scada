/*
 * Rapid SCADA API for access data and send commands
 * Rapid SCADA API для доступа к данным и отправки команд
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

// Namespace
var scada = scada || {};

// Объект API
scada.api = {
    // Получить текущее значение входного канала
    getCnlVal: function (cnlNum, callback) {
        callback(0.0);
    },

    // Получить текущий статус входного канала
    getCnlStat: function (cnlNum, callback) {
        callback(0);
    },

    // Получить текущие данные входного канала 
    getCnlData: function (cnlNum, callback) {
        callback(new scada.CnlDataView());
    }
};

// Тип данных входного канала
scada.CnlDataView = function () {
    this.val = 0.0;
    this.stat = 0;
    this.text = "";
    this.textWithUnit = "";
    this.color = "";
};