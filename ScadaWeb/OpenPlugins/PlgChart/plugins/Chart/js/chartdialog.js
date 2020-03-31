/*
 * Chart object that implements displaying charts
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2020
 *
 * Requires:
 * - utils.js
 */

// Rapid SCADA namespace
var scada = scada || {};
// Chart namespace
scada.chart = scada.chart || {};

scada.chart.dialog = {
    // Get chart URL
    getChartUrl: function (cnlNums, viewIDs, date) {
        return "plugins/Chart/Chart.aspx?cnlNum=" + cnlNums + "&viewID=" + viewIDs +
            scada.utils.dateToQueryString(date, true);
    },

    // Open chart in the new tab
    show: function (rootPath, cnlNums, viewIDs, date) {
        window.open(rootPath + this.getChartUrl(cnlNums, viewIDs, date));
    }
};
