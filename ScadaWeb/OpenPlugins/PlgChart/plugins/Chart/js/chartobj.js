/*
 * Chart object that implements displaying charts
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 *
 * Requires:
 * - utils.js
 */

// Rapid SCADA namespace
var scada = scada || {};

scada.chart = {
    // Get chart URL
    getChartUrl: function (viewID, date, cnlNums) {
        return "plugins/Chart/Chart.aspx?viewID=" + viewID +
            "&" + scada.utils.dateToQueryString(date) + "&cnlNum=" + cnlNums;
    },

    // Open chart in the new tab
    show: function (rootPath, viewID, date, cnlNums) {
        window.open(rootPath + this.getChartUrl(viewID, date, cnlNums));
    }
}