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
    // Application culture name
    this.locale = "en-GB";
    // Distance between chart points to make a gap
    this.chartGap = 90 / scada.chart.const.SEC_PER_DAY; // 90 seconds
};

/********** Time Range **********/

// Time range type
scada.chart.TimeRange = function () {
    // Date of the beginning of the range in milliseconds
    this.startDate = 0;
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
    // Name of input channel quantity (and unit)
    this.QuantityName = "";
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
    // Vertical hint offset relative to the cursot
    this.HINT_OFFSET = 20;
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
    // Absolute left coordinate of the drawing area relative to the document
    this.absPlotAreaLeft = 0;
    // Absolute right coordinate of the drawing area relative to the document
    this.absPlotAreaRight = 0;
    // Absolute top coordinate of the drawing area relative to the document
    this.absPlotAreaTop = 0;
    // Absolute bottom coordinate of the drawing area relative to the document
    this.absPlotAreaBottom = 0;
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
scada.chart.ChartLayout.prototype._calcPlotArea = function (canvasJqObj, trendCnt, showDates) {
    this.plotAreaLeft = this.LEFT_PADDING + this.LINE_HEIGHT /*y-axis title*/ +
        this.maxYLblWidth + this.LBL_LR_MARGIN * 2;
    this.plotAreaRight = this.width - this.RIGHT_PADDING;
    this.plotAreaTop = this.TOP_PADDING;
    this.plotAreaBottom = this.height - this.BOTTOM_PADDING - this.LBL_TB_MARGIN - this.LINE_HEIGHT /*time labels*/ -
         (showDates ? this.LINE_HEIGHT : 0) - this.LBL_TB_MARGIN - trendCnt * this.LINE_HEIGHT;
    this.plotAreaWidth = this.plotAreaRight - this.plotAreaLeft;
    this.plotAreaHeight = this.plotAreaBottom - this.plotAreaTop;

    var offset = canvasJqObj.offset();
    var canvasLeft = offset.left + parseInt(canvasJqObj.css("border-left-width"));
    var canvasTop = offset.top + parseInt(canvasJqObj.css("border-top-width"));
    this.absPlotAreaLeft = canvasLeft + this.plotAreaLeft;
    this.absPlotAreaRight = canvasLeft + this.plotAreaRight;
    this.absPlotAreaTop = canvasTop + this.plotAreaTop;
    this.absPlotAreaBottom = canvasTop + this.plotAreaBottom;
}

// Calculate chart layout
scada.chart.ChartLayout.prototype.calculate = function (canvasJqObj, context,
    minX, maxX, minY, maxY, trendCnt, showDates) {

    this.width = canvasJqObj.width();
    this.height = canvasJqObj.height();

    this._calcGridX(minX, maxX);
    this._calcGridY(context, minY, maxY);
    this._calcPlotArea(canvasJqObj, trendCnt, showDates);
}

/********** Chart Control **********/

// Chart type
scada.chart.Chart = function (canvasJqObj) {
    // Date format options
    this._DATE_OPTIONS = { month: "short", day: "2-digit", timeZone: "UTC" };
    // Time format options
    this._TIME_OPTIONS = { hour: "2-digit", minute: "2-digit", timeZone: "UTC" };
    // Date and time format options
    this._DATE_TIME_OPTIONS = $.extend({}, this._DATE_OPTIONS, this._TIME_OPTIONS);
    // Colors assigned to trends
    this._TREND_COLORS =
        ["#ff0000" /*Red*/, "#0000ff" /*Blue*/, "#008000" /*Green*/, "#ff00ff" /*Fuchsia*/, "#ffa500" /*Orange*/,
         "#00ffff" /*Aqua*/, "#00ff00" /*Lime*/, "#4b0082" /*Indigo*/, "#ff1493" /*DeepPink*/, "#8b4513" /*SaddleBrown*/];

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
    // Time mark jQuery object
    this._timeMark = null;
    // Trend hint jQuery object
    this._trendHint = null;

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
    // Show date labels below the x-axis
    this._showDates = false;

    // Display settings
    this.displaySettings = new scada.chart.DisplaySettings();
    // Time range
    this.timeRange = new scada.chart.TimeRange();
    // Chart data
    this.chartData = null;
};

// Claculate top and bottom edges of the displayed range
scada.chart.Chart.prototype._calcYRange = function () {
    // find min and max trend value
    var minY = NaN;
    var maxY = NaN;
    var minX = this._minX - this.displaySettings.chartGap;
    var maxX = this._maxX + this.displaySettings.chartGap;

    var timePoints = this.chartData.TimePoints;
    var ptCnt = timePoints.length;
    var VAL_IND = scada.chart.TrendPointIndexes.VAL_IND;

    for (var trend of this.chartData.Trends) {
        var trendPoints = trend.TrendPoints;

        for (var ptInd = 0; ptInd < ptCnt; ptInd++) {
            var x = timePoints[ptInd];

            if (minX <= x && x <= maxX) {
                var y = trendPoints[ptInd][VAL_IND];
                if (isNaN(minY) || minY > y) {
                    minY = y;
                }
                if (isNaN(maxY) || maxY < y) {
                    maxY = y;
                }
            }
        }
    }

    if (isNaN(minY)) {
        minY = -1;
        maxY = 1;
    } else {
        // calculate extra space
        var extraSpace = minY == maxY ? 1 : (maxY - minY) * 0.05;

        // include zero if zoom is off
        var origMinY = minY;
        var origMaxY = maxY;

        if (true /*zoom is off*/) {
            if (minY > 0 && maxY > 0) {
                minY = 0;
            }
            else if (minY < 0 && maxY < 0) {
                maxY = 0;
            }            
            extraSpace = Math.max(extraSpace, (maxY - minY) * 0.05);
        }

        // add extra space
        if (origMinY - minY < extraSpace) {
            minY -= extraSpace;
        }
        if (maxY - origMaxY < extraSpace) {
            maxY += extraSpace;
        }
    }

    this._minY = minY;
    this._maxY = maxY;
};

// Convert trend x-coordinate to the chart x-coordinate
scada.chart.Chart.prototype._trendXToChartX = function (x) {
    return Math.round((x - this._minX) * this._coefX + this._chartLayout.plotAreaLeft);
};

// Convert trend y-coordinate to the chart y-coordinate
scada.chart.Chart.prototype._trendYToChartY = function (y) {
    return Math.round((this._maxY - y) * this._coefY + this._chartLayout.plotAreaTop);
};

// Convert trend x-coordinate to the page x-coordinate
scada.chart.Chart.prototype._trendXToPageX = function (x) {
    return Math.round((x - this._minX) * this._coefX + this._chartLayout.absPlotAreaLeft);
};

// Convert chart x-coordinate to the trend x-coordinate
scada.chart.Chart.prototype._pageXToTrendX = function (pageX) {
    return (pageX - this._chartLayout.absPlotAreaLeft) / this._coefX + this._minX;
},

// Convert trend x-coordinate to the date object
scada.chart.Chart.prototype._trendXToDate = function (x) {
    return new Date(this.timeRange.startDate + Math.round(x * scada.chart.const.MS_PER_DAY));
}

// Get index of the point nearest to the specified page x-coordinate
scada.chart.Chart.prototype._getPointIndex = function (pageX) {
    var timePoints = this.chartData.TimePoints;
    var ptCnt = timePoints.length;

    if (ptCnt < 1) {
        return -1;
    } else {
        var x = this._pageXToTrendX(pageX);
        var ptInd = 0;

        if (ptCnt == 1) {
            ptInd = 0;
        } else {
            // binary search
            var iL = 0;
            var iR = ptCnt - 1;

            if (x < timePoints[iL] || x > timePoints[iR])
                return -1;

            while (iR - iL > 1) {
                var iM = Math.floor((iR + iL) / 2);
                var xM = timePoints[iM];

                if (xM == x)
                    return iM;
                else if (xM < x)
                    iL = iM;
                else
                    iR = iM;
            }

            ptInd = x - timePoints[iL] < timePoints[iR] - x ? iL : iR;
        }

        return Math.abs(x - timePoints[ptInd]) <= this.displaySettings.chartGap ? ptInd : -1;
    }
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
    return time.toLocaleTimeString(this.displaySettings.locale, this._TIME_OPTIONS);
};

// Convert x-coordinate that means time into a date string
scada.chart.Chart.prototype._dateToStr = function (t) {
    return this._trendXToDate(t).toLocaleDateString(this.displaySettings.locale, this._DATE_OPTIONS);
};

// Convert x-coordinate that means time into a date and time string
scada.chart.Chart.prototype._dateTimeToStr = function (t) {
    return this._trendXToDate(t).toLocaleString(this.displaySettings.locale, this._DATE_TIME_OPTIONS);
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
scada.chart.Chart.prototype._drawGridX = function () {
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
        var ptX = this._trendXToChartX(x);

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
            if (this._showDates && timeText == dayBegTimeText) {
                this._context.fillText(this._dateToStr(x), lblX, lblDateY);
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
        var ptY = this._trendYToChartY(y);

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
    if (this.chartData.QuantityName) {
        var ctx = this._context;
        var layout = this._chartLayout;
        ctx.textAlign = "center";
        ctx.textBaseline = "middle";
        ctx.save();
        ctx.translate(0, layout.plotAreaBottom);
        ctx.rotate(-Math.PI / 2);
        ctx.fillText(this.chartData.QuantityName, layout.plotAreaHeight / 2, 
    layout.LEFT_PADDING + layout.LINE_HEIGHT / 2, layout.plotAreaHeight);
        ctx.restore();
    }
};

// Draw lagand that is the input channel names
scada.chart.Chart.prototype._drawLegend = function () {
    var layout = this._chartLayout;
    this._context.textAlign = "left";
    this._context.textBaseline = "middle";

    var lblX = layout.plotAreaLeft + layout.LBL_FONT_SIZE + layout.LBL_LR_MARGIN;
    var lblY = layout.plotAreaBottom + layout.LBL_TB_MARGIN + layout.LINE_HEIGHT /*time labels*/ +
        (this._showDates ? layout.LINE_HEIGHT : 0) + layout.LBL_TB_MARGIN + layout.LINE_HEIGHT / 2;
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
            var ptX = this._trendXToChartX(x);
            var ptY = this._trendYToChartY(y);

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

// Create a time mark if it doesn't exist
scada.chart.Chart.prototype._initTimeMark = function () {
    if (this._timeMark) {
        this._timeMark.addClass("hidden");
    } else {
        this._timeMark = $("<div class='chart-time-mark hidden'></div>");
        this._canvasJqObj.after(this._timeMark);
    }
}

// Create a trend hint if it doesn't exist
scada.chart.Chart.prototype._initTrendHint = function () {
    if (this._trendHint) {
        this._trendHint.addClass("hidden");
    } else {
        var trendCnt = this.chartData.Trends.length;
        if (trendCnt > 0) {
            this._trendHint = $("<div class='chart-trend-hint hidden'><div class='time'></div><table></table></div>");
            var table = this._trendHint.children("table");

            for (var trendInd = 0; trendInd < trendCnt; trendInd++) {
                var trend = this.chartData.Trends[trendInd];
                var row = $("<tr><td class='color'><span></span></td><td class='text'></td>" +
                    "<td class='colon'>:</td><td class='val'></td></tr>");
                row.find("td.color span").css("background-color", this._getColorByTrend(trendInd));
                row.children("td.text").text("[" + trend.CnlNum + "] " + trend.CnlName);
                table.append(row);
            }

            this._canvasJqObj.after(this._trendHint);
        } else {
            this._trendHint = $();
        }
    }
}

// Draw the chart
scada.chart.Chart.prototype.draw = function () {
    if (this._canvasOK && this.displaySettings && this.timeRange && this.chartData) {
        var layout = this._chartLayout;

        this._minX = Math.min(this.timeRange.startTime, 0);
        this._maxX = Math.max(this.timeRange.endTime, 1);
        this._calcYRange();
        this._showDates = this._maxX - this._minX > 1;

        // prepare canvas
        this._canvas.width = this._canvasJqObj.width();
        this._canvas.height = this._canvasJqObj.height();
        this._context = this._canvas.getContext("2d");
        this._context.font = layout.LBL_FONT;
        this._initTimeMark();
        this._initTrendHint();

        // calculate layout
        var trendCnt = this.chartData.Trends.length;
        layout.calculate(this._canvasJqObj, this._context,
            this._minX, this._maxX, this._minY, this._maxY, trendCnt, this._showDates);

        this._alignToGridX();
        this._coefX = layout.plotAreaWidth / (this._maxX - this._minX);
        this._coefY = layout.plotAreaHeight / (this._maxY - this._minY);

        // draw chart
        this._clearRect(0, 0, layout.width, layout.height);
        this._drawFrame();
        this._drawGridX();
        this._drawGridY();
        this._drawYAxisTitle();
        this._drawLegend();
        this._drawTrends();
    }
};

// Show hint with the values nearest to the pointer
scada.chart.Chart.prototype.showHint = function (pageX, pageY) {
    var layout = this._chartLayout;
    var hideHint = true;

    if (layout.absPlotAreaLeft <= pageX && pageX <= layout.absPlotAreaRight &&
        layout.absPlotAreaTop <= pageY && pageY <= layout.absPlotAreaBottom) {
        var ptInd = this._getPointIndex(pageX);

        if (ptInd >= 0) {
            var x = this.chartData.TimePoints[ptInd];
            var ptPageX = this._trendXToPageX(x);

            if (layout.absPlotAreaLeft <= ptPageX && ptPageX <= layout.absPlotAreaRight) {
                hideHint = false;

                // set position and show the time mark
                this._timeMark
                .removeClass("hidden")
                .css({
                    "left": ptPageX,
                    "top": layout.absPlotAreaTop,
                    "height": layout.plotAreaHeight,
                });

                // set text, position and show the trend hint
                this._trendHint.find("div.time").text(this._showDates ? this._dateTimeToStr(x) : this._timeToStr(x));
                var trendCnt = this.chartData.Trends.length;
                var hintValCells = this._trendHint.find("td.val");

                for (var trendInd = 0; trendInd < trendCnt; trendInd++) {
                    var trend = this.chartData.Trends[trendInd];
                    var trendPoint = trend.TrendPoints[ptInd];
                    hintValCells.eq(trendInd)
                    .text(trendPoint[scada.chart.TrendPointIndexes.TEXT_WITH_UNIT_IND])
                    .css("color", trendPoint[scada.chart.TrendPointIndexes.COLOR_IND]);
                }

                var hintWidth = this._trendHint.outerWidth();
                var docWidth = $(document).width();
                var hintLeft = pageX + hintWidth < docWidth ? pageX : Math.max(docWidth - hintWidth, 0);

                this._trendHint
                .removeClass("hidden")
                .css({
                    "left": hintLeft,
                    "top": pageY + layout.HINT_OFFSET
                });
            }
        }
    }

    if (hideHint) {
        this._timeMark.addClass("hidden");
        this._trendHint.addClass("hidden");
    }
};