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
    SEC_PER_DAY: 86400,
    // Milliseconds per day
    MS_PER_DAY: 86400 * 1000
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

/********** Trend Point Indexes **********/

// Trend point indexes enumeration
scada.chart.TrendPointIndexes = {
    VAL_IND: 0,
    TEXT_IND: 1,
    TEXT_WITH_UNIT_IND: 2,
    COLOR_IND: 3
};

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
    // Desirable number of horizontal grid lines
    this._GRID_HOR_LINE_CNT = 10;

    // Chart left padding
    this.LEFT_PADDING = 10;
    // Chart right padding
    this.RIGHT_PADDING = 20;
    // Chart top padding
    this.TOP_PADDING = 20;
    // Chart bottom padding
    this.BOTTOM_PADDING = 10;
    // Tick mark size
    this.TICK_SIZE = 3;
    // Data label left and right margins
    this.LBL_LR_MARGIN = 10;
    // Data label top and bottom margins
    this.LBL_TB_MARGIN = 5;
    // Data labels font
    this.LBL_FONT = "12px Arial";
    // Data labels font size the same as specified above
    this.LBL_FONT_SIZE = 12;
    // Line height of various kinds of texts
    this.LINE_HEIGHT = 18;
    // Chart back color
    this.BACK_COLOR = "#ffffff";
    // Default fore color
    this.DEF_COLOR = "#000000";
    // Chart frame color
    this.FRAME_COLOR = "#808080";
    // Grid lines color
    this.GRID_COLOR = "#e0e0e0";
    // Tick marks color
    this.TICK_COLOR = "#808080";
    // Data labels color
    this.LBL_COLOR = "#000000";

    // Chart width
    this.width = 0;
    // Chart height
    this.height = 0;

    // Grid step for the x-axis
    this.gridXStep = 0;
    // Start grid value for the y-axis
    this.gridYStart = 0;
    // Grid step for the y-axis
    this.gridYStep = 0;
    // Number of decimal places to use in labels for the y-axis
    this.gridYDecDig = 0;
    // Max data label width for the y-axis
    this.maxYLblWidth = 0;

    // Left coordinate of the drawing area
    this.plotAreaLeft = 0;
    // Right coordinate of the drawing area
    this.plotAreaRight = 0;
    // Top coordinate of the drawing area
    this.plotAreaTop = 0;
    // Bottom coordinate of the drawing area
    this.plotAreaBottom = 0;
    // Drawing area width
    this.plotAreaWidth = 0;
    // Drawing area height
    this.plotAreaHeight = 0;
}

// Calculate grid parameters for the x-axis
scada.chart.ChartLayout.prototype._calcGridX = function (minX, maxX) {
    var cnt = 8;
    var ranges = [16, 8, 4, 2, 1, 1 / 2, 1 / 4, 1 / 12]; // displayed ranges, days
    var steps = [24, 12, 6, 3, 2, 1, 1 / 2, 1 / 4];      // grid steps according to the ranges, hours
    var minStep = 1 / 12; // 5 minutes
    var range = maxX - minX;

    for (var i = 0; i < cnt; i++) {
        if (range > ranges[i]) {
            minStep = steps[i];
            break;
        }
    }

    this.gridXStep = 1 / 24 * minStep;
};

// Calculate grid parameters for the y-axis
scada.chart.ChartLayout.prototype._calcGridY = function (context, minY, maxY) {
    this.gridYStep = (maxY - minY) / this._GRID_HOR_LINE_CNT;
    this.gridYDecDig = 0;
    var n = 1;

    if (this.gridYStep >= 1) {
        while (this.gridYStep > 10) {
            this.gridYStep /= 10;
            n *= 10;
        }
    } else {
        while (this.gridYStep < 1) {
            this.gridYStep *= 10;
            n /= 10;
            this.gridYDecDig++
        }
    }

    this.gridYStep = Math.floor(this.gridYStep);

    // the first significant digit of the grid step is 1, 2 or 5
    if (3 <= this.gridYStep && this.gridYStep <= 4) {
        this.gridYStep = 2;
    }
    else if (6 <= this.gridYStep && this.gridYStep <= 9) {
        this.gridYStep = 5;
    }

    this.gridYStep *= n;
    this.gridYStart = Math.floor(minY / this.gridYStep) * this.gridYStep + this.gridYStep;

    // measure max data label width
    var maxWidth = 0;
    for (var y = this.gridYStart; y < maxY; y += this.gridYStep) {
        var w = context.measureText(y.toFixed(this.gridYDecDig)).width;
        if (maxWidth < w)
            maxWidth = w;
    }
    this.maxYLblWidth = maxWidth;
}

// Calculate coordinates of the drawing area
scada.chart.ChartLayout.prototype._calcPlotArea = function (trendCnt, showDates) {
    this.plotAreaLeft = this.LEFT_PADDING + this.LINE_HEIGHT /*y-axis title*/ +
        this.maxYLblWidth + this.LBL_LR_MARGIN * 2;
    this.plotAreaRight = this.width - this.RIGHT_PADDING;
    this.plotAreaTop = this.TOP_PADDING;
    this.plotAreaBottom = this.height - this.BOTTOM_PADDING - this.LBL_TB_MARGIN - this.LINE_HEIGHT /*time labels*/ -
         (showDates ? this.LINE_HEIGHT : 0) - this.LBL_TB_MARGIN - trendCnt * this.LINE_HEIGHT;
    this.plotAreaWidth = this.plotAreaRight - this.plotAreaLeft;
    this.plotAreaHeight = this.plotAreaBottom - this.plotAreaTop;
}

// Calculate chart layout
scada.chart.ChartLayout.prototype.calculate = function (canvasJqObj, context,
    minX, maxX, minY, maxY, trendCnt, showDates) {

    this.width = canvasJqObj.width();
    this.height = canvasJqObj.height();

    this._calcGridX(minX, maxX);
    this._calcGridY(context, minY, maxY);
    this._calcPlotArea(trendCnt, showDates);
}

/********** Chart Control **********/

// Chart type
scada.chart.Chart = function (canvasJqObj) {
    // Date format options
    this._DATE_OPTIONS = { month: "short", day: "2-digit", timeZone: "UTC" };
    // Time format options
    this._TIME_OPTIONS = { hour: "2-digit", minute: "2-digit", timeZone: "UTC" };
    // Colors assigned to trends
    this._TREND_COLORS =
        ["#ff0000" /*Red*/, "#0000ff" /*Blue*/, "#008000" /*Green*/, "#ff00ff" /*Fuchsia*/, "#ffa500"/*Orange*/,
         "#00ffff"/*Aqua*/, "#00ff00" /*Lime*/, "#4b0082" /*Indigo*/, "#ff1493"/*DeepPink*/, "#8b4513"/*SaddleBrown*/];

    // Canvas jQuery object
    this._canvasJqObj = canvasJqObj;
    // Canvas where the chart is drawn
    this._canvas = canvasJqObj.length ? canvasJqObj[0] : null;
    // Canvas is supported and ready for drawing
    this._canvasOK = this._canvas && this._canvas.getContext;
    // Canvas drawing context
    this._context = null;
    // Layout of the chart
    this._chartLayout = new scada.chart.ChartLayout();

    // Left edge of the displayed range
    this._minX = 0;
    // Right edge of the displayed range
    this._maxX = 0;
    // Bottom edge of the displayed range
    this._minY = 0;
    // Top edge of the displayed range
    this._maxY = 0;
    // Transformation coefficient of the x-coordinate
    this._coefX = 1;
    // Transformation coefficient of the y-coordinate
    this._coefY = 1;

    // Display settings
    this.displaySettings = new scada.chart.DisplaySettings();
    // Time range
    this.timeRange = new scada.chart.TimeRange();
    // Chart data
    this.chartData = null;
};

// Convert trend x-coordinate to the chart x-coordinate
scada.chart.Chart.prototype._toChartX = function (x) {
    return Math.round((x - this._minX) * this._coefX + this._chartLayout.plotAreaLeft);
};

// Convert trend y-coordinate to the chart y-coordinate
scada.chart.Chart.prototype._toChartY = function (y) {
    return Math.round((this._maxY - y) * this._coefY + this._chartLayout.plotAreaTop);
};

// Correct left and right edges of the displayed range to align to the grid
scada.chart.Chart.prototype._alignToGridX = function () {
    var gridXStep = this._chartLayout.gridXStep;
    this._minX = Math.floor(this._minX / gridXStep) * gridXStep;
    this._maxX = Math.ceil(this._maxX / gridXStep) * gridXStep;
}

// Convert x-coordinate that means time into a time string
scada.chart.Chart.prototype._timeToStr = function (t) {
    var time = new Date(Math.round(t * scada.chart.const.MS_PER_DAY));
    return time.toLocaleTimeString(this.displaySettings.cultureName, this._TIME_OPTIONS);
};

// Convert x-coordinate that means time into a date string
scada.chart.Chart.prototype._dateToStr = function (t) {
    var dateDelta = Math.floor(t) * scada.chart.const.MS_PER_DAY;
    var date = new Date(this.timeRange.startDate.getTime() + dateDelta);
    return date.toLocaleDateString(this.displaySettings.cultureName, this._DATE_OPTIONS);
};

// Draw pixel on the chart
scada.chart.Chart.prototype._drawPixel = function (x, y, opt_checkBounds) {
    if (opt_checkBounds) {
        // check if the given coordinates are located within the drawing area
        var layout = this._chartLayout;
        if (layout.plotAreaLeft <= x && x <= layout.plotAreaRight &&
            layout.plotAreaTop <= y && y <= layout.plotAreaBottom) {
            this._context.fillRect(x, y, 1, 1);
        }
    } else {
        // just draw a pixel
        this._context.fillRect(x, y, 1, 1);
    }
},

// Draw line on the chart
scada.chart.Chart.prototype._drawLine = function (x1, y1, x2, y2, opt_checkBounds) {
    if (opt_checkBounds) {
        var layout = this._chartLayout;
        var minX = Math.min(x1, x2);
        var maxX = Math.max(x1, x2);
        var minY = Math.min(y1, y2);
        var maxY = Math.max(y1, y2);

        if (layout.plotAreaLeft <= minX && maxX <= layout.plotAreaRight &&
            layout.plotAreaTop <= minY && maxY <= layout.plotAreaBottom) {
            opt_checkBounds = false; // the line is fully inside the drawing area
        } else if (layout.plotAreaLeft > maxX || minX > layout.plotAreaRight ||
            layout.plotAreaTop > maxY || minY > layout.plotAreaBottom) {
            return; // the line is outside the drawing area
        }
    }

    var dx = x2 - x1;
    var dy = y2 - y1;

    if (dx != 0 || dy != 0) {
        if (Math.abs(dx) > Math.abs(dy)) {
            var a = dy / dx;
            var b = -a * x1 + y1;

            if (dx < 0) {
                var x0 = x1;
                x1 = x2;
                x2 = x0;
            }

            for (var x = x1; x <= x2; x++) {
                var y = Math.round(a * x + b);
                this._drawPixel(x, y, opt_checkBounds);
            }
        } else {
            var a = dx / dy;
            var b = -a * y1 + x1;

            if (dy < 0) {
                var y0 = y1;
                y1 = y2;
                y2 = y0;
            }

            for (var y = y1; y <= y2; y++) {
                var x = Math.round(a * y + b);
                this._drawPixel(x, y, opt_checkBounds);
            }
        }
    }
};

// Clear the specified rectangle by filling it with the background color
scada.chart.Chart.prototype._clearRect = function (x, y, width, height) {
    this._setColor(this._chartLayout.BACK_COLOR);
    this._context.fillRect(x, y, width, height);
}

// Set current drawing color
scada.chart.Chart.prototype._setColor = function (color) {
    this._context.fillStyle = this._context.strokeStyle = 
        color ? color : this._chartLayout.DEF_COLOR;
}

// Get color of the trend with the specified index
scada.chart.Chart.prototype._getColorByTrend = function (trendInd) {
    return this._TREND_COLORS[trendInd % this._TREND_COLORS.length];
}

// Draw the chart frame
scada.chart.Chart.prototype._drawFrame = function () {
    var layout = this._chartLayout;
    var frameL = layout.plotAreaLeft - 1;
    var frameR = layout.plotAreaRight + 1;
    var frameT = layout.plotAreaTop - 1;
    var frameB = layout.plotAreaBottom + 1;

    this._setColor(layout.FRAME_COLOR);
    this._drawLine(frameL, frameT, frameL, frameB);
    this._drawLine(frameR, frameT, frameR, frameB);
    this._drawLine(frameL, frameT, frameR, frameT);
    this._drawLine(frameL, frameB, frameR, frameB);
}

// Draw chart grid of the x-axis
scada.chart.Chart.prototype._drawGridX = function (showDates) {
    var layout = this._chartLayout;
    this._context.textAlign = "center";
    this._context.textBaseline = "middle";

    var prevLblX = NaN;
    var prevLblHalfW = NaN;
    var frameB = layout.plotAreaBottom + 1;
    var tickT = frameB + 1;
    var tickB = frameB + layout.TICK_SIZE;
    var lblY = layout.plotAreaBottom + layout.LBL_TB_MARGIN + layout.LINE_HEIGHT / 2;
    var lblDateY = lblY + layout.LINE_HEIGHT;
    var dayBegTimeText = this._timeToStr(0);

    for (var x = this._minX; x <= this._maxX; x += layout.gridXStep) {
        var ptX = this._toChartX(x);

        // vertical grid line
        this._setColor(layout.GRID_COLOR);
        this._drawLine(ptX, layout.plotAreaTop, ptX, layout.plotAreaBottom);

        // tick
        this._setColor(layout.TICK_COLOR);
        this._drawLine(ptX, tickT, ptX, tickB);

        // label
        this._setColor(layout.LBL_COLOR);
        var lblX = ptX;
        var timeText = this._timeToStr(x);
        var lblHalfW = this._context.measureText(timeText).width / 2;

        if (isNaN(prevLblX) || lblX - lblHalfW > prevLblX + prevLblHalfW + layout.LBL_LR_MARGIN) {
            this._context.fillText(timeText, lblX, lblY);
            if (showDates && timeText == dayBegTimeText) {
                this._context.fillText(this._dateToStr(Math.round(x)), lblX, lblDateY);
            }
            prevLblX = lblX;
            prevLblHalfW = lblHalfW;
        }
    }
};

// Draw chart grid of the y-axis
scada.chart.Chart.prototype._drawGridY = function () {
    var layout = this._chartLayout;
    this._context.textAlign = "right";
    this._context.textBaseline = "middle";

    var prevLblY = NaN;
    var frameL = layout.plotAreaLeft - 1;
    var tickL = frameL - layout.TICK_SIZE;
    var tickR = frameL - 1;
    var lblX = frameL - layout.LBL_LR_MARGIN;

    for (var y = layout.gridYStart; y < this._maxY; y += layout.gridYStep) {
        var ptY = this._toChartY(y);

        // horizontal grid line
        this._setColor(layout.GRID_COLOR);
        this._drawLine(layout.plotAreaLeft, ptY, layout.plotAreaRight, ptY);

        // tick
        this._setColor(layout.TICK_COLOR);
        this._drawLine(tickL, ptY, tickR, ptY);

        // label
        this._setColor(layout.LBL_COLOR);
        var lblY = ptY;
        if (isNaN(prevLblY) || prevLblY - lblY > layout.LBL_FONT_SIZE) {
            this._context.fillText(y.toFixed(layout.gridYDecDig), lblX, lblY);
            prevLblY = lblY;
        }
    }
};

// Draw the y-axis title
scada.chart.Chart.prototype._drawYAxisTitle = function () {
    var titleText = "test!"; // TODO

    if (titleText) {
        var ctx = this._context;
        var layout = this._chartLayout;
        ctx.textAlign = "center";
        ctx.textBaseline = "middle";
        ctx.save();
        ctx.translate(0, layout.plotAreaBottom);
        ctx.rotate(-Math.PI / 2);
        ctx.fillText(titleText, layout.plotAreaHeight / 2, layout.LEFT_PADDING + layout.LINE_HEIGHT / 2,
            layout.plotAreaHeight);
        ctx.restore();
    }
};

// Draw lagand that is the input channel names
scada.chart.Chart.prototype._drawLegend = function (showDates) {
    var layout = this._chartLayout;
    this._context.textAlign = "left";
    this._context.textBaseline = "middle";

    var lblX = layout.plotAreaLeft + layout.LBL_FONT_SIZE + layout.LBL_LR_MARGIN;
    var lblY = layout.plotAreaBottom + layout.LBL_TB_MARGIN + layout.LINE_HEIGHT /*time labels*/ +
        (showDates ? layout.LINE_HEIGHT : 0) + layout.LBL_TB_MARGIN + layout.LINE_HEIGHT / 2;
    var rectSize = layout.LBL_FONT_SIZE;
    var rectX = layout.plotAreaLeft - 0.5;
    var rectY = lblY - rectSize / 2 - 0.5;
    var trendCnt = this.chartData.Trends.length;

    for (var trendInd = 0; trendInd < trendCnt; trendInd++) {
        var trend = this.chartData.Trends[trendInd];
        var legendText = "[" + trend.CnlNum + "] " + trend.CnlName;

        this._setColor(this._getColorByTrend(trendInd));
        this._context.fillRect(rectX, rectY, rectSize, rectSize);
        this._setColor(layout.LBL_COLOR);
        this._context.strokeRect(rectX, rectY, rectSize, rectSize);
        this._context.fillText(legendText, lblX, lblY);

        lblY += layout.LINE_HEIGHT;
        rectY += layout.LINE_HEIGHT;
    }
}

// Draw all the trends
scada.chart.Chart.prototype._drawTrends = function () {
    var trendCnt = this.chartData.Trends.length;
    for (var trendInd = 0; trendInd < trendCnt; trendInd++) {
        this._drawTrend(this.chartData.TimePoints, this.chartData.Trends[trendInd], this._getColorByTrend(trendInd));
    }
}

// Draw the specified trend
scada.chart.Chart.prototype._drawTrend = function (timePoints, trend, color) {
    var trendPoints = trend.TrendPoints;
    var chartGap = this.displaySettings.chartGap;
    var VAL_IND = scada.chart.TrendPointIndexes.VAL_IND;

    this._setColor(color);

    var prevX = NaN;
    var prevPtX = NaN;
    var prevPtY = NaN;
    var ptCnt = timePoints.length;

    for (var ptInd = 0; ptInd < ptCnt; ptInd++) {
        var pt = trendPoints[ptInd];
        var y = pt[VAL_IND];

        if (!isNaN(y)) {
            var x = timePoints[ptInd];
            var ptX = this._toChartX(x);
            var ptY = this._toChartY(y);

            if (isNaN(prevX)) {
            }
            else if (x - prevX > chartGap) {
                this._drawPixel(prevPtX, prevPtY, true);
                this._drawPixel(ptX, ptY, true);
            } else if (prevPtX != ptX || prevPtY != ptY) {
                this._drawLine(prevPtX, prevPtY, ptX, ptY, true);
            }

            prevX = x;
            prevPtX = ptX;
            prevPtY = ptY;
        }
    }

    if (!isNaN(prevPtX))
        this._drawPixel(prevPtX, prevPtY, true);
}

// Draw the chart
scada.chart.Chart.prototype.draw = function () {
    if (this._canvasOK && this.displaySettings && this.timeRange && this.chartData) {
        var layout = this._chartLayout;

        this._minX = this.timeRange.startTime; // TODO
        this._maxX = this.timeRange.endTime; // TODO
        this._minY = -10; // TODO
        this._maxY = 10; // TODO

        // prepare canvas
        this._canvas.width = this._canvasJqObj.width();
        this._canvas.height = this._canvasJqObj.height();
        this._context = this._canvas.getContext("2d");
        this._context.font = layout.LBL_FONT;

        // calculate layout
        var trendCnt = this.chartData.Trends.length;
        var showDates = true; // TODO
        layout.calculate(this._canvasJqObj, this._context,
            this._minX, this._maxX, this._minY, this._maxY, trendCnt, showDates);

        this._alignToGridX();
        this._coefX = layout.plotAreaWidth / (this._maxX - this._minX);
        this._coefY = layout.plotAreaHeight / (this._maxY - this._minY);

        // draw chart
        this._clearRect(0, 0, layout.width, layout.height);
        this._drawFrame();
        this._drawGridX(showDates);
        this._drawGridY();
        this._drawYAxisTitle();
        this._drawLegend(showDates);
        this._drawTrends();
    }
};