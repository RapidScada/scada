/*
 * Chart control
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 *
 * Requires:
 * - jquery
 */

// Rapid SCADA namespace
var scada = scada || {};
// Chart namespace
scada.chart = scada.chart || {};

/********** Constants **********/

// Constants object
scada.chart.const = {
    // Seconds per day
    SEC_PER_DAY: 86400
};

/********** Display Settings **********/

// Display settings type
scada.chart.DisplaySettings = function () {
    // Date and time format culture
    this.cultureName = "en-GB";
    // Distance between chart points to make a gap
    this.chartGap = 90 / scada.chart.const.SEC_PER_DAY; // 90 seconds
};

/********** Time Range **********/

// Time range type
scada.chart.TimeRange = function () {
    // Date of the beginning of the range
    this.startDate = new Date();
    // Left edge of the range, where 0 is 00:00 and 1 is 24:00
    this.startTime = 0;
    // Right edge of the range
    this.endTime = 1;
}

/********** Extended Trend **********/

// Extended trend type
// Note: Casing is caused by C# naming rules
scada.chart.TrendExt = function () {
    // Input channel number
    this.CnlNum = 0;
    // Input channel name
    this.CnlName = "";
    // Name of input channel quantity
    this.QuantityName = "";
    // Trend points where each point is array [value, "text", "text with unit", "color"]
    this.TrendPoints = [];
}

/********** Chart Data **********/

// Chart data type
scada.chart.ChartData = function () {
    // Time points which number is matched with the number of trend points
    this.TimePoints = [];
    // Trends to display
    this.Trends = [];
}

/********** Chart Layout **********/

// Chart layout type
scada.chart.ChartLayout = function () {
    // Chart width
    this.width = 0;
    // Chart height
    this.height = 0;
}

// Calculate chart layout
scada.chart.ChartLayout.prototype.calculate = function (canvasJqObj) {
    this.width = canvasJqObj.width();
    this.height = canvasJqObj.height();
}

/********** Chart Control **********/

// Chart type
scada.chart.Chart = function (canvasJqObj) {
    // Canvas where the chart is drawn
    this._canvas = canvasJqObj.length ? canvasJqObj[0] : null;
    // Canvas drawing context
    this._context = this._canvas && this._canvas.getContext ? this._canvas.getContext("2d") : null;
    // Layout of the chart
    this._chartLayout = new scada.chart.ChartLayout();

    // Display settings
    this.displaySettings = new scada.chart.DisplaySettings();
    // Time range
    this.timeRange = new scada.chart.TimeRange();
    // Chart data
    this.chartData = null;
};

// Draw pixel on the chart
scada.chart.Chart.prototype._drawPixel = function (x, y) {
    this._context.fillRect(x, y, 1, 1);
},

// Draw the chart
scada.chart.Chart.prototype.draw = function () {
    if (this._context && this.displaySettings && this.timeRange && this.chartData) {
        this._chartLayout.calculate();

    }
};