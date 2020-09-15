/*
 * Chart control
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2020
 *
 * Requires:
 * - jquery
 * - utils.js
 */

// Rapid SCADA namespace.
var scada = scada || {};
// Chart namespace.
scada.chart = scada.chart || {};

/********** Constants **********/

// The constants object.
scada.chart.const = {
    // Seconds per day.
    SEC_PER_DAY: 86400,
    // Milliseconds per day.
    MS_PER_DAY: 86400 * 1000,
    // The minimal time step, 1 ms.
    TIME_EPSILON: 1 / 86400000,
    // The start timestamp for time encoding.
    SCADA_EPOCH: Date.UTC(1899, 11 /*December*/, 30)
};

/********** Area Position Enumeration **********/

// Specifies the chart area positions.
scada.chart.AreaPosition = {
    NONE: 0,
    TOP: 1,
    RIGHT: 2,
    BOTTOM: 3,
    LEFT: 4
};

/********** Include Zero Enumeration **********/

// Specifies the options for including zero in the Y-axis range.
scada.chart.IncludeZero = {
    OFF: 0,
    ON: 1,
    WITHOUT_ZOOM: 2
};

/********** Display Options **********/

// Represents chart display options.
scada.chart.DisplayOptions = function () {
    this.locale = "en-GB";
    this.gapBetweenPoints = 90;
    this.alignXToGrid = true;

    this.chartArea = {
        chartPadding: [10, 20, 10, 10],
        fontName: "Arial",
        backColor: "#ffffff"
    };

    this.titleConfig = {
        showTitle: true,
        showMenu: true,
        showStatus: true,
        height: 30,
        fontSize: 17,
        foreColor: "#333333"
    };

    this.plotArea = {
        frameColor: "#808080",
        gridColor: "#e0e0e0",
        backColor: "#ffffff",
        markerColor: "#000000",
        selectionColor: "#6aaaea",
        lineWidth: 1,
        trendColors: ["#ff0000", "#0000ff", "#008000", "#ff00ff", "#ffa500",
            "#00ffff", "#00ff00", "#4b0082", "#ff1493", "#8b4513"]
    };

    this.xAxis = {
        height: 30,
        showGridLines: true,
        showDates: true,
        majorTickSize: 4,
        minorTickSize: 2,
        showMinorTicks: true,
        labelMargin: [2, 3],
        fontSize: 12,
        lineColor: "#808080",
        textColor: "#000000"
    };

    this.yAxes = [{
        position: scada.chart.AreaPosition.LEFT,
        autoWidth: true,
        width: 0,
        showTitle: true,
        showGridLines: true,
        majorTickSize: 4,
        minorTickSize: 2,
        minorTickCount: 4,
        labelMargin: [2, 3],
        fontSize: 12,
        lineColor: "#808080",
        textColor: "#000000",
        trendColor: "",
        includeZero: scada.chart.IncludeZero.WITHOUT_ZOOM,
        quantityIDs: []
    }];

    this.legend = {
        position: scada.chart.AreaPosition.BOTTOM,
        columnWidth: 300,
        columnMargin: [10, 10, 0],
        columnCount: 1,
        lineHeight: 18,
        iconWidth: 12,
        iconHeight: 12,
        fontSize: 12,
        foreColor: "#000000"
    };
};

// Gets the specified maring using CSS rules. Index: 0 - top, 1 - right, 2 - bottom, 3 - left.
scada.chart.DisplayOptions.getMargin = function (margin, index) {
    if (Array.isArray(margin) && margin.length > 0 && 0 <= index && index <= 3) {
        if (index < margin.length) {
            return margin[index];
        } else if (index >= 2) {
            return scada.chart.DisplayOptions.getMargin(margin, index - 2);
        } else if (index >= 1) {
            return scada.chart.DisplayOptions.getMargin(margin, index - 1);
        } else {
            return 0;
        }
    } else {
        return 0;
    }
};

/********** Time Range **********/

// Represents a time range.
scada.chart.TimeRange = function () {
    // The left edge of the range. A double value started from SCADA_EPOCH, 0 is 00:00 and 1 is 24:00, etc.
    this.startTime = 0;
    // The right edge of the range.
    this.endTime = 1;
};

/********** Trend **********/

// Represents a trend.
scada.chart.Trend = function () {
    // The input channel number.
    this.cnlNum = 0;
    // The input channel name.
    this.cnlName = "";
    // The quantity ID.
    this.quantityID = "";
    // The quantity name.
    this.quantityName = "";
    // The trend points where each point is array of [value, "text", "text with unit", "color"].
    this.trendPoints = [];
    // The trend color.
    this.color = "";
    // The trend caption.
    this.caption = "";
};

/********** Trend Point Index Enumeration **********/

// Specifies the trend point indexes.
scada.chart.TrendPointIndex = {
    VAL_IND: 0,
    TEXT_IND: 1,
    TEXT_WITH_UNIT_IND: 2,
    COLOR_IND: 3
};

/********** Chart Data **********/

// Represents chart data.
scada.chart.ChartData = function () {
    // The time points which number is matched with the number of trend points. Array of numbers.
    this.timePoints = [];
    // The trends to display. Each array element is of the Trend type.
    this.trends = [];
};

/********** Axis Tag **********/

// Represents information related to an axis.
scada.chart.AxisTag = function () {
    // The minimum value.
    this.min = 0;
    // The maximum value.
    this.max = 0;
    // The transformation coefficient.
    this.coef = 1;
    // The axis title.
    this.title = "";
    // The trends that use this axis.
    this.trends = [];
    // The axis configuration.
    this.axisConfig = null;
    // The axis layout.
    this.axisLayout = null;
};

/********** Axis Layout **********/

// Represents an axis layout.
scada.chart.AxisLayout = function () {
    // The start grid value.
    this.gridStart = 0;
    // The grid step.
    this.gridStep = 0;
    // The step of the minor ticks.
    this.minorTickStep = 0;
    // The number of decimal digits to use in labels.
    this.gridDigits = 0;
    // The axis area width.
    this.areaWidth = 0;
    // The axis area rectangle.
    this.areaRect = new DOMRect(0, 0, 0, 0);
};

/********** Chart Layout **********/

// Represents a chart layout.
scada.chart.ChartLayout = function () {
    // The desirable number of horizontal grid lines.
    this._GRID_HOR_LINE_CNT = 10;
    // The vertical hint offset relative to the cursor.
    this.HINT_OFFSET = 20;

    // The chart area width.
    this.width = 0;
    // The chart area height.
    this.height = 0;
    // The title area rectangle.
    this.titleAreaRect = new DOMRect(0, 0, 0, 0);
    // The canvas area rectangle.
    this.canvasAreaRect = new DOMRect(0, 0, 0, 0);
    // The plot area rectangle.
    this.plotAreaRect = new DOMRect(0, 0, 0, 0);
    // The plot area rectangle relative to the canvas.
    this.canvasPlotAreaRect = new DOMRect(0, 0, 0, 0);
    // The plot area rectangle relative to the document.
    this.absPlotAreaRect = new DOMRect(0, 0, 0, 0);
    // The legend area rectangle.
    this.legendAreaRect = new DOMRect(0, 0, 0, 0);
    // The X-axis layout.
    this.xAxisLayout = new scada.chart.AxisLayout();
    // The X-axis layouts.
    this.yAxisLayouts = [];

    // The canvas X offset. Add this value to the X coordinate of an area to draw on the canvas.
    this.canvasXOffset = 0;
    // The canvas Y offset. Add this value to the Y coordinate of an area to draw on the canvas.
    this.canvasYOffset = 0;
};

// Calculates the X-axis layout.
scada.chart.ChartLayout.prototype._calcXLayout = function (xAxisTag) {
    var cnt = 8;
    var ranges =     [16,  8, 4, 2, 1, 1 / 2, 1 / 4, 1 / 12]; // displayed ranges, days
    var gridSteps =  [24, 12, 6, 3, 2,     1, 1 / 2, 1 / 4];  // grid steps according to the ranges, hours
    var minorSteps = [12,  6, 3, 1, 1, 1 / 2, 1 / 4, 1 / 12 ];
    var gridStep = 1 / 12;  // 5 minutes
    var minorStep = 1 / 60; // 1 minute
    var range = xAxisTag.max - xAxisTag.min;

    for (var i = 0; i < cnt; i++) {
        if (range > ranges[i]) {
            gridStep = gridSteps[i];
            minorStep = minorSteps[i];
            break;
        }
    }

    this.xAxisLayout.gridStart = xAxisTag.min;
    this.xAxisLayout.gridStep = gridStep / 24;
    this.xAxisLayout.minorTickStep = minorStep / 24;
    this.xAxisLayout.gridDigits = 0;
    this.xAxisLayout.areaWidth = 0;
};

// Calculates the Y-axis layout.
scada.chart.ChartLayout.prototype._calcYLayout = function (yAxisTag, fontName, context) {
    var gridYStep = (yAxisTag.max - yAxisTag.min) / this._GRID_HOR_LINE_CNT;
    var gridYDecDig = 0;
    var n = 1;

    if (gridYStep >= 1) {
        while (gridYStep > 10) {
            gridYStep /= 10;
            n *= 10;
        }
    } else {
        while (gridYStep < 1) {
            gridYStep *= 10;
            n /= 10;
            gridYDecDig++;
        }
    }

    gridYStep = Math.floor(gridYStep);

    // the first significant digit of the grid step is 1, 2 or 5
    if (3 <= gridYStep && gridYStep <= 4) {
        gridYStep = 2;
    }
    else if (6 <= gridYStep && gridYStep <= 9) {
        gridYStep = 5;
    }

    gridYStep *= n;
    var gridYStart = Math.floor(yAxisTag.min / gridYStep) * gridYStep + gridYStep;

    // measure max data label width
    var yAxisConfig = yAxisTag.axisConfig;
    var maxLabelWidth = 0;
    context.font = yAxisConfig.fontSize + "px " + fontName;

    for (var y = gridYStart; y < yAxisTag.max; y += gridYStep) {
        var w = context.measureText(y.toFixed(gridYDecDig)).width;
        if (maxLabelWidth < w)
            maxLabelWidth = w;
    }

    var yAxisLayout = yAxisTag.axisLayout;
    yAxisLayout.gridStart = gridYStart;
    yAxisLayout.gridStep = gridYStep;
    yAxisLayout.minorTickStep = yAxisConfig.minorTickCount > 0 ? gridYStep / (yAxisConfig.minorTickCount + 1) : 0;
    yAxisLayout.gridDigits = gridYDecDig;
    yAxisLayout.areaWidth = yAxisConfig.majorTickSize + Math.ceil(maxLabelWidth) +
        scada.chart.DisplayOptions.getMargin(yAxisConfig.labelMargin, 1) +
        scada.chart.DisplayOptions.getMargin(yAxisConfig.labelMargin, 3);

    if (yAxisConfig.showTitle) {
        yAxisLayout.areaWidth += yAxisConfig.fontSize +
            scada.chart.DisplayOptions.getMargin(yAxisConfig.labelMargin, 0) +
            scada.chart.DisplayOptions.getMargin(yAxisConfig.labelMargin, 2);
    }
};

// Corrects edges of the X-axis to align to grid.
scada.chart.ChartLayout.prototype._alignXToGrid = function (xAxisTag) {
    var gridXStep = this.xAxisLayout.gridStep;
    xAxisTag.min = Math.floor(xAxisTag.min / gridXStep) * gridXStep;
    xAxisTag.max = Math.ceil(xAxisTag.max / gridXStep) * gridXStep;
};

// Offsets the specified rectangle.
scada.chart.ChartLayout.prototype._offsetRect = function (rect, xOffset, yOffset) {
    return new DOMRect(rect.x + xOffset, rect.y + yOffset, rect.width, rect.height);
};

// Calculates the chart layout.
scada.chart.ChartLayout.prototype.calculate = function (displayOptions, chartJqElem, context,
    xAxisTag, yAxisTags, trendCnt) {

    // chart area
    this.width = chartJqElem.outerWidth();
    this.height = chartJqElem.outerHeight();

    var chartPadding = displayOptions.chartArea.chartPadding;
    var mainLeft = scada.chart.DisplayOptions.getMargin(chartPadding, 3);
    var mainRight = this.width - scada.chart.DisplayOptions.getMargin(chartPadding, 1);
    var mainTop = scada.chart.DisplayOptions.getMargin(chartPadding, 0);
    var mainBottom = this.height - scada.chart.DisplayOptions.getMargin(chartPadding, 2);
    var mainWidth = mainRight - mainLeft;
    var mainHeight = mainBottom - mainTop;

    // title
    var titleHeight = displayOptions.titleConfig.showTitle ? displayOptions.titleConfig.height : 0;
    var plotAreaTop = mainTop + titleHeight;
    this.titleAreaRect = new DOMRect(mainLeft, mainTop, mainWidth, titleHeight);

    // canvas
    this.canvasAreaRect = new DOMRect(0, mainTop + titleHeight, this.width, mainHeight - titleHeight);
    this.canvasXOffset = -this.canvasAreaRect.left;
    this.canvasYOffset = -this.canvasAreaRect.top;

    // legend
    var legendWidth = displayOptions.legend.columnCount * (displayOptions.legend.columnWidth +
        scada.chart.DisplayOptions.getMargin(displayOptions.legend.columnMargin, 1) +
        scada.chart.DisplayOptions.getMargin(displayOptions.legend.columnMargin, 3));
    var legendLineCnt = Math.ceil(trendCnt / displayOptions.legend.columnCount);
    var legendHeight = legendLineCnt * displayOptions.legend.lineHeight +
        scada.chart.DisplayOptions.getMargin(displayOptions.legend.columnMargin, 0) +
        scada.chart.DisplayOptions.getMargin(displayOptions.legend.columnMargin, 2);
    var legendAtBottom = displayOptions.legend.position === scada.chart.AreaPosition.BOTTOM;
    var legendAtRight = displayOptions.legend.position === scada.chart.AreaPosition.RIGHT;

    var xAxisBottom = legendAtBottom ? mainBottom - legendHeight : mainBottom;
    var xAxisTop = xAxisBottom - displayOptions.xAxis.height;
    var plotAreaHeight = xAxisTop - plotAreaTop;

    if (legendAtBottom) {
        this.legendAreaRect = new DOMRect(mainLeft, xAxisBottom, mainWidth, legendHeight);
    } else if (legendAtRight) {
        this.legendAreaRect = new DOMRect(mainRight - legendWidth, plotAreaTop, legendWidth, plotAreaHeight);
    } else {
        this.legendAreaRect = new DOMRect(0, 0, 0, 0);
    }

    // X-axis
    this._calcXLayout(xAxisTag);
    this.xAxisLayout.areaRect = new DOMRect(0, xAxisTop, this.width, displayOptions.xAxis.height);

    if (displayOptions.alignXToGrid) {
        this._alignXToGrid(xAxisTag);
    }

    // Y-axes and plot area
    this.yAxisAreaRects = [];
    this.yAxisLayouts = [];
    var xLeft = mainLeft;
    var xRight = legendAtRight ? this.legendAreaRect.left : mainRight;

    // Y-axes on the left
    for (let i = 0, len = yAxisTags.length; i < len; i++) {
        let yAxisTag = yAxisTags[i];

        if (yAxisTag.axisConfig.position === scada.chart.AreaPosition.LEFT) {
            yAxisTag.coef = (plotAreaHeight - 3) / (yAxisTag.max - yAxisTag.min);
            yAxisTag.axisLayout = new scada.chart.AxisLayout();
            this._calcYLayout(yAxisTag, displayOptions.chartArea.fontName, context);

            let yAxisConfig = yAxisTag.axisConfig;
            let yAxisLayout = yAxisTag.axisLayout;
            let yAxisWidth = yAxisConfig.autoWidth ? yAxisLayout.areaWidth : yAxisConfig.width;

            let yAxisLeft = xLeft;
            xLeft += yAxisWidth;

            yAxisLayout.areaRect = new DOMRect(yAxisLeft, plotAreaTop, yAxisWidth, plotAreaHeight);
            this.yAxisLayouts.push(yAxisLayout);
        }
    }

    // Y-axes on the right
    for (let i = yAxisTags.length - 1; i >= 0; i--) {
        let yAxisTag = yAxisTags[i];

        if (yAxisTag.axisConfig.position === scada.chart.AreaPosition.RIGHT) {
            yAxisTag.coef = (plotAreaHeight - 3) / (yAxisTag.max - yAxisTag.min);
            yAxisTag.axisLayout = new scada.chart.AxisLayout();
            this._calcYLayout(yAxisTag, displayOptions.chartArea.fontName, context);

            let yAxisConfig = yAxisTag.axisConfig;
            let yAxisLayout = yAxisTag.axisLayout;
            let yAxisWidth = yAxisConfig.autoWidth ? yAxisLayout.areaWidth : yAxisConfig.width;

            xRight -= yAxisWidth;
            let yAxisLeft = xRight;

            yAxisLayout.areaRect = new DOMRect(yAxisLeft, plotAreaTop, yAxisWidth, plotAreaHeight);
            this.yAxisLayouts.push(yAxisLayout);
        }
    }

    // plot area
    this.plotAreaRect = new DOMRect(xLeft, plotAreaTop, xRight - xLeft, plotAreaHeight);
    this.canvasPlotAreaRect = this._offsetRect(this.plotAreaRect, this.canvasXOffset, this.canvasYOffset);
    xAxisTag.coef = (this.plotAreaRect.width - 1) / (xAxisTag.max - xAxisTag.min);
};

// Updates the absolute coordinates of the plot area.
scada.chart.ChartLayout.prototype.updateAbsCoordinates = function (canvasJqObj) {
    var offset = canvasJqObj.offset();
    this.absPlotAreaRect = this._offsetRect(this.canvasPlotAreaRect, offset.left, offset.top);
};

// Checks if the specified point is located within the plot area.
scada.chart.ChartLayout.prototype.pointInPlotArea = function (pageX, pageY) {
    return this.absPlotAreaRect.left <= pageX && pageX < this.absPlotAreaRect.right &&
        this.absPlotAreaRect.top <= pageY && pageY < this.absPlotAreaRect.bottom;
};

/********** Chart Control **********/

// Represents a chart.
scada.chart.Chart = function (chartJqElem) {
    // The date format options.
    this._DATE_OPTIONS = { month: "short", day: "2-digit", timeZone: "UTC" };
    // The time format options.
    this._TIME_OPTIONS = { hour: "2-digit", minute: "2-digit", timeZone: "UTC" };
    // The time format options that include seconds.
    this._TIME_OPTIONS_SEC = { hour: "2-digit", minute: "2-digit", second: "2-digit", timeZone: "UTC" };
    // The date and time format options.
    this._DATE_TIME_OPTIONS = $.extend({}, this._DATE_OPTIONS, this._TIME_OPTIONS);
    // The date and time format options that include seconds.
    this._DATE_TIME_OPTIONS_SEC = $.extend({}, this._DATE_OPTIONS, this._TIME_OPTIONS_SEC);
    // The threshold that determines whether to show seconds on X-axis. Equals 1 hour.
    this.SHOW_SEC_THRESHOLD = 1 / 24;
    // The default fore color.
    this.DEFAULT_FORE_COLOR = "#000000";

    // The chart jQuery element having the div tag.
    this._chartJqElem = chartJqElem;
    // The title jQuery element.
    this._titleJqElem = null;
    // The canvas jQuery element.
    this._canvasJqElem = null;
    // The canvas DOM element where the chart is drawn.
    this._canvas = null;
    // The drawing context of the canvas.
    this._context = null;
    // The time mark jQuery element.
    this._timeMarkJqElem = null;
    // The trend hint jQuery element.
    this._trendHintJqElem = null;
    // The information related to the X-axis.
    this._xAxisTag = new scada.chart.AxisTag();
    // The information related to the Y-axes.
    this._yAxisTags = [];
    // Describes the chart layout.
    this._chartLayout = new scada.chart.ChartLayout();
    // The zoom mode flag that affects calculation of the vertical range.
    this._zoomMode = false;
    // Indicates whether to enable or disable a trend hint.
    this._hintEnabled = true;

    // The display options.
    this.displayOptions = new scada.chart.DisplayOptions();
    // THe time range.
    this.timeRange = new scada.chart.TimeRange();
    // The chart data.
    this.chartData = null;
};

// Initializes the axis tags.
scada.chart.Chart.prototype._initAxisTags = function (opt_reinit) {
    if (this._xAxisTag.min === this._xAxisTag.max /*not initialized yet*/ || opt_reinit) {
        // reset zoom
        this._zoomMode = false;

        // define X-axis range
        this._xAxisTag.min = this.timeRange.startTime;
        this._xAxisTag.max = this.timeRange.endTime;

        // create Y-axis tags and assign Y-axis for each trend
        this._yAxisTags = [];
        var axisTagMap = new Map();

        for (var yAxis of this.displayOptions.yAxes) {
            var yAxisTag = new scada.chart.AxisTag();
            yAxisTag.axisConfig = yAxis;
            this._yAxisTags.push(yAxisTag);

            for (var id of yAxis.quantityIDs) {
                axisTagMap.set(id, yAxisTag);
            }
        }

        if (this._yAxisTags.length > 0) {
            var firstTag = this._yAxisTags[0];

            for (var trend of this.chartData.trends) {
                var tag = axisTagMap.has(trend.quantityID) ? axisTagMap.get(trend.quantityID) : firstTag;
                trend.color = tag.axisConfig.trendColor;
                tag.trends.push(trend);
            }
        }

        // remove unused Y-axis tags
        var idx = this._yAxisTags.length;
        while (--idx >= 0) {
            if (this._yAxisTags[idx].trends.length === 0) {
                this._yAxisTags.splice(idx, 1);
            }
        }

        // define Y-axis ranges
        this._calcAllYRanges();
    }
};

// Calculates ranges for all Y-axes.
scada.chart.Chart.prototype._calcAllYRanges = function () {
    for (var yAxisTag of this._yAxisTags) {
        this._calcYRange(yAxisTag);
    }
};

// Calculates top and bottom edges of the displayed range. In addition, defines the axis title.
scada.chart.Chart.prototype._calcYRange = function (yAxisTag, opt_startPtInd) {
    // find min and max trend value
    var chartGap = this.displayOptions.gapBetweenPoints / scada.chart.const.SEC_PER_DAY;
    var minY = NaN;
    var maxY = NaN;
    var minX = this._xAxisTag.min - chartGap;
    var maxX = this._xAxisTag.max + chartGap;
    var axisTitle = yAxisTag.trends.length > 0 ? yAxisTag.trends[0].quantityName : "";

    var timePoints = this.chartData.timePoints;
    var startPtInd = opt_startPtInd ? opt_startPtInd : 0;
    var ptCnt = timePoints.length;
    var VAL_IND = scada.chart.TrendPointIndex.VAL_IND;

    for (var trend of yAxisTag.trends) {
        var trendPoints = trend.trendPoints;

        for (var ptInd = startPtInd; ptInd < ptCnt; ptInd++) {
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

        if (axisTitle !== trend.quantityName) {
            axisTitle = "";
        }
    }

    if (isNaN(minY)) {
        minY = -1;
        maxY = 1;
    } else {
        // calculate extra space
        const EXTRA_SPACE_COEF = 0.05;
        var extraSpace = minY === maxY && typeof opt_startPtInd === "undefined" ?
            1 : (maxY - minY) * EXTRA_SPACE_COEF;
        var includeZero = yAxisTag.axisConfig.includeZero === scada.chart.IncludeZero.ON ||
            yAxisTag.axisConfig.includeZero === scada.chart.IncludeZero.WITHOUT_ZOOM && !this._zoomMode;

        if (includeZero) {
            // include zero in the Y-range
            var origMinY = minY;
            var origMaxY = maxY;

            if (minY > 0 && maxY > 0) {
                minY = 0;
            }
            else if (minY < 0 && maxY < 0) {
                maxY = 0;
            }

            extraSpace = Math.max(extraSpace, (maxY - minY) * EXTRA_SPACE_COEF);

            // add extra space
            if (origMinY - minY < extraSpace) {
                minY -= extraSpace;
            }
            if (maxY - origMaxY < extraSpace) {
                maxY += extraSpace;
            }
        } else {
            // add extra space
            minY -= extraSpace;
            maxY += extraSpace;
        }
    }

    yAxisTag.min = minY;
    yAxisTag.max = maxY;
    yAxisTag.title = axisTitle;
};

// Initializes the trend colors and captions.
scada.chart.Chart.prototype._initTrendFields = function () {
    var trendInd = 0;
    for (var trend of this.chartData.trends) {
        trend.color = trend.color || this._getColorByTrend(trendInd);
        trend.caption = "[" + trend.cnlNum + "] " + trend.cnlName;
        trendInd++;
    }
};

// Checks if top and bottom edges are outdated because of new data.
scada.chart.Chart.prototype._yRangeIsOutdated = function (startPtInd) {
    for (var yAxisTag of this._yAxisTags) {
        var oldMinY = yAxisTag.min;
        var oldMaxY = yAxisTag.max;
        this._calcYRange(yAxisTag, startPtInd);
        var outdated = yAxisTag.min < oldMinY || yAxisTag.max > oldMaxY;

        // restore the range
        yAxisTag.min = oldMinY;
        yAxisTag.max = oldMaxY;

        if (outdated) {
            return true;
        }
    }

    return false;
};

// Converts the trend X-coordinate to the canvas X-coordinate.
scada.chart.Chart.prototype._trendXToCanvasX = function (x) {
    return Math.round((x - this._xAxisTag.min) * this._xAxisTag.coef +
        this._chartLayout.plotAreaRect.left + this._chartLayout.canvasXOffset);
};

// Converts the trend Y-coordinate to the canvas Y-coordinate.
scada.chart.Chart.prototype._trendYToCanvasY = function (y, yAxisTag) {
    return Math.round((yAxisTag.max - y) * yAxisTag.coef +
        this._chartLayout.plotAreaRect.top + this._chartLayout.canvasYOffset + 1);
};

// Converts the trend X-coordinate to the page X-coordinate.
scada.chart.Chart.prototype._trendXToPageX = function (x) {
    return Math.round((x - this._xAxisTag.min) * this._xAxisTag.coef + this._chartLayout.absPlotAreaRect.left);
};

// Converts the page X-coordinate to the trend X-coordinate.
scada.chart.Chart.prototype._pageXToTrendX = function (pageX) {
    return (pageX - this._chartLayout.absPlotAreaRect.left) / this._xAxisTag.coef + this._xAxisTag.min;
};

// Converts the trend X-coordinate to the date object.
scada.chart.Chart.prototype._trendXToDate = function (x) {
    return new Date(scada.chart.const.SCADA_EPOCH + Math.round(x * scada.chart.const.MS_PER_DAY));
};

// Gets the index of the point nearest to the specified page X-coordinate.
scada.chart.Chart.prototype._getPointIndex = function (pageX) {
    var chartGap = this.displayOptions.gapBetweenPoints / scada.chart.const.SEC_PER_DAY;
    var timePoints = this.chartData.timePoints;
    var ptCnt = timePoints.length;

    if (ptCnt < 1) {
        return -1;
    } else {
        var x = this._pageXToTrendX(pageX);
        var ptInd = 0;

        if (ptCnt === 1) {
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

                if (xM === x)
                    return iM;
                else if (xM < x)
                    iL = iM;
                else
                    iR = iM;
            }

            ptInd = x - timePoints[iL] < timePoints[iR] - x ? iL : iR;
        }

        return Math.abs(x - timePoints[ptInd]) <= chartGap ? ptInd : -1;
    }
};

// Converts the X-coordinate that means a time into a date and time string.
scada.chart.Chart.prototype._dateTimeToStr = function (t, showSeconds) {
    var dateTime = this._trendXToDate(t);

    if (scada.utils.iOS()) {
        var date = new Date(dateTime.getTime());
        date.setUTCMinutes(date.getUTCMinutes() + date.getTimezoneOffset());
        return date.toLocaleDateString(this.displayOptions.locale, this._DATE_OPTIONS) + ", " +
            this._simpleTimeToStr(dateTime, showSeconds);
    } else {
        return dateTime.toLocaleString(this.displayOptions.locale,
            showSeconds ? this._DATE_TIME_OPTIONS_SEC : this._DATE_TIME_OPTIONS);
    }
};

// Converts the X-coordinate that means a time into a time string.
scada.chart.Chart.prototype._timeToStr = function (t, showSeconds) {
    var time = new Date(Math.round(t * scada.chart.const.MS_PER_DAY));
    return scada.utils.iOS() ? // iOS requires manual time formatting
        this._simpleTimeToStr(time, showSeconds) :
        time.toLocaleTimeString(this.displayOptions.locale, showSeconds ? this._TIME_OPTIONS_SEC : this._TIME_OPTIONS);
};

// Converts the time to a string using explicit transformations.
scada.chart.Chart.prototype._simpleTimeToStr = function (time, showSeconds) {
    var min = time.getUTCMinutes();
    var timeStr = time.getUTCHours() + ":" + (min < 10 ? "0" + min : min);

    if (showSeconds) {
        var sec = time.getUTCSeconds();
        timeStr += ":" + (sec < 10 ? "0" + sec : sec);
    }

    return timeStr;
};

// Draws the pixel on the chart.
scada.chart.Chart.prototype._drawPixel = function (x, y, opt_boundRect, opt_size) {
    if (opt_size && opt_size > 1) {
        var size = opt_size;
        let offset = size / 2;
        x -= offset;
        y -= offset;
    } else {
        size = 1;
    }

    if (opt_boundRect) {
        // check if the given coordinates are located within the drawing area
        if (opt_boundRect.left <= x && x + size <= opt_boundRect.right &&
            opt_boundRect.top <= y && y + size <= opt_boundRect.bottom) {
            this._context.fillRect(x, y, size, size);
        }
    } else {
        // just draw a pixel
        this._context.fillRect(x, y, size, size);
    }
};

// Draws the line on the chart.
scada.chart.Chart.prototype._drawLine = function (x1, y1, x2, y2, opt_boundRect) {
    if (opt_boundRect) {
        let minX = Math.min(x1, x2);
        let maxX = Math.max(x1, x2);
        let minY = Math.min(y1, y2);
        let maxY = Math.max(y1, y2);

        if (opt_boundRect.left <= minX && maxX < opt_boundRect.right &&
            opt_boundRect.top <= minY && maxY < opt_boundRect.bottom) {
            opt_boundRect = null; // the line is fully inside the drawing area
        } else if (opt_boundRect.left > maxX || minX >= opt_boundRect.right ||
            opt_boundRect.top > maxY || minY > opt_boundRect.bottom) {
            return; // the line is outside the drawing area
        }
    }

    let dx = x2 - x1;
    let dy = y2 - y1;

    if (dx !== 0 || dy !== 0) {
        if (Math.abs(dx) > Math.abs(dy)) {
            let a = dy / dx;
            let b = -a * x1 + y1;

            if (dx < 0) {
                let x0 = x1;
                x1 = x2;
                x2 = x0;
            }

            for (let x = x1; x <= x2; x++) {
                let y = Math.round(a * x + b);
                this._drawPixel(x, y, opt_boundRect);
            }
        } else {
            let a = dx / dy;
            let b = -a * y1 + x1;

            if (dy < 0) {
                let y0 = y1;
                y1 = y2;
                y2 = y0;
            }

            for (let y = y1; y <= y2; y++) {
                let x = Math.round(a * y + b);
                this._drawPixel(x, y, opt_boundRect);
            }
        }
    }
};

// Draws the trend line on the chart.
scada.chart.Chart.prototype._drawTrendLine = function (x1, y1, x2, y2, boundRect, lineWidth) {
    if (lineWidth > 1) {
        // draw the line if at least a part is inside the drawing area
        let minX = Math.min(x1, x2);
        let maxX = Math.max(x1, x2);
        let minY = Math.min(y1, y2);
        let maxY = Math.max(y1, y2);

        if (!(boundRect.left > maxX || minX >= boundRect.right ||
            boundRect.top > maxY && minY >= boundRect.bottom)) {
            // line width already set
            let ctx = this._context;
            ctx.beginPath();
            ctx.moveTo(x1, y1);
            ctx.lineTo(x2, y2);
            ctx.stroke();
        }
    } else {
        this._drawLine(x1, y1, x2, y2, boundRect);
    }
};

// Clears the specified rectangle by filling it with the background color.
scada.chart.Chart.prototype._clearRect = function (x, y, width, height) {
    this._setColor(this.displayOptions.chartArea.backColor);
    this._context.fillRect(x, y, width, height);
};

// Sets the current drawing color.
scada.chart.Chart.prototype._setColor = function (color) {
    this._context.fillStyle = this._context.strokeStyle =
        color ? color : this.DEFAULT_FORE_COLOR;
};

// Gets the color of the trend with the specified index.
scada.chart.Chart.prototype._getColorByTrend = function (trendInd) {
    var colors = this.displayOptions.plotArea.trendColors;
    return colors[trendInd % colors.length];
};

// Draws the chart frame.
scada.chart.Chart.prototype._drawFrame = function () {
    var rect = this._chartLayout.canvasPlotAreaRect;
    var frameL = rect.left - 1;
    var frameR = rect.right;
    var frameT = rect.top;
    var frameB = rect.bottom - 1;

    this._setColor(this.displayOptions.plotArea.backColor);
    this._context.fillRect(rect.left, rect.top, rect.width, rect.height);

    this._setColor(this.displayOptions.plotArea.frameColor);
    this._drawLine(frameL, frameT, frameL, frameB);
    this._drawLine(frameR, frameT, frameR, frameB);
    this._drawLine(frameL, frameT, frameR, frameT);
    this._drawLine(frameL, frameB, frameR, frameB);
};

// Draws chart grid of the X-axis.
scada.chart.Chart.prototype._drawXGrid = function () {
    var layout = this._chartLayout;
    var xAxisConfig = this.displayOptions.xAxis;

    this._context.textAlign = "center";
    this._context.textBaseline = "top";
    this._context.font = xAxisConfig.fontSize + "px " + this.displayOptions.chartArea.fontName;

    var prevLblX = NaN;
    var prevLblHalfW = NaN;
    var tickT = layout.plotAreaRect.bottom + layout.canvasYOffset;
    var tickB = tickT + xAxisConfig.majorTickSize - 1;
    var minorTickB = tickT + xAxisConfig.minorTickSize - 1;
    var gridT = layout.plotAreaRect.top + layout.canvasYOffset + 1;
    var gridB = gridT + layout.plotAreaRect.height - 3;
    var labelMarginT = scada.chart.DisplayOptions.getMargin(xAxisConfig.labelMargin, 0);
    var labelMarginR = scada.chart.DisplayOptions.getMargin(xAxisConfig.labelMargin, 1);
    var labelMarginB = scada.chart.DisplayOptions.getMargin(xAxisConfig.labelMargin, 2);
    var labelMarginL = scada.chart.DisplayOptions.getMargin(xAxisConfig.labelMargin, 3);
    var lblY = tickB + labelMarginT + 1;
    var lblDateY = lblY + xAxisConfig.fontSize + labelMarginB + labelMarginT;
    var gridStep = layout.xAxisLayout.gridStep;
    var minorTickStep = this.displayOptions.xAxis.showMinorTicks ? layout.xAxisLayout.minorTickStep : 0;
    var showSeconds = this._xAxisTag.max - this._xAxisTag.min <= this.SHOW_SEC_THRESHOLD;
    var dayBegTimeText = this._timeToStr(0, showSeconds);

    for (var x = this._xAxisTag.min; x <= this._xAxisTag.max; x += gridStep) {
        var ptX = this._trendXToCanvasX(x);

        // vertical grid line
        if (xAxisConfig.showGridLines) {
            this._setColor(this.displayOptions.plotArea.gridColor);
            this._drawLine(ptX, gridT, ptX, gridB);
        }

        // major tick
        this._setColor(xAxisConfig.lineColor);
        this._drawLine(ptX, tickT, ptX, tickB);

        // minor ticks
        if (minorTickStep > 0) {
            for (var minorTickX = x + minorTickStep,
                maxMinorTickX = Math.min(x + gridStep, this._xAxisTag.max) - scada.chart.const.TIME_EPSILON;
                minorTickX < maxMinorTickX; minorTickX += minorTickStep) {

                var minorTickCnvX = this._trendXToCanvasX(minorTickX);
                this._drawLine(minorTickCnvX, tickT, minorTickCnvX, minorTickB);
            }
        }

        // label
        this._setColor(xAxisConfig.textColor);
        var lblX = ptX;
        var timeText = this._timeToStr(x, showSeconds);
        var lblHalfW = this._context.measureText(timeText).width / 2;

        if (isNaN(prevLblX) || lblX - lblHalfW - labelMarginL > prevLblX + prevLblHalfW + labelMarginR) {
            this._context.fillText(timeText, lblX, lblY);

            if (xAxisConfig.showDates && timeText === dayBegTimeText) {
                this._context.fillText(this.dateToStr(x), lblX, lblDateY);
            }

            prevLblX = lblX;
            prevLblHalfW = lblHalfW;
        }
    }
};

// Draws chart grid of the Y-axis.
scada.chart.Chart.prototype._drawYGrid = function (yAxisTag) {
    var layout = this._chartLayout;
    var yAxisConfig = yAxisTag.axisConfig;
    var yAxisLayout = yAxisTag.axisLayout;
    var yAxisRect = yAxisLayout.areaRect;

    this._context.textBaseline = "middle";
    this._context.font = yAxisConfig.fontSize + "px " + this.displayOptions.chartArea.fontName;

    var labelMarginT = scada.chart.DisplayOptions.getMargin(yAxisConfig.labelMargin, 0);
    var labelMarginR = scada.chart.DisplayOptions.getMargin(yAxisConfig.labelMargin, 1);
    var labelMarginB = scada.chart.DisplayOptions.getMargin(yAxisConfig.labelMargin, 2);
    var labelMarginL = scada.chart.DisplayOptions.getMargin(yAxisConfig.labelMargin, 3);
    var prevLblY = NaN;

    if (yAxisConfig.position === scada.chart.AreaPosition.LEFT) {
        this._context.textAlign = "right";
        var tickR = yAxisRect.right - 1;
        var tickL = tickR - yAxisConfig.majorTickSize;
        var minorTickR = tickR;
        var minorTickL = minorTickR - yAxisConfig.minorTickSize;
        var axisX = yAxisRect.right - 1;
        var lblX = tickL - labelMarginR;
    } else {
        this._context.textAlign = "left";
        tickL = yAxisRect.left;
        tickR = tickL + yAxisConfig.majorTickSize;
        minorTickL = tickL;
        minorTickR = minorTickL + yAxisConfig.minorTickSize;
        axisX = yAxisRect.left;
        lblX = tickR + labelMarginL;
    }

    var gridL = layout.plotAreaRect.left + layout.canvasXOffset;
    var gridR = gridL + layout.plotAreaRect.width - 1;
    var axisT = yAxisRect.top + layout.canvasYOffset;
    var axisB = axisT + yAxisRect.height - 1;

    for (var y = yAxisLayout.gridStart; y < yAxisTag.max; y += yAxisLayout.gridStep) {
        var ptY = this._trendYToCanvasY(y, yAxisTag);

        // horizontal grid line
        if (yAxisConfig.showGridLines) {
            this._setColor(this.displayOptions.plotArea.gridColor);
            this._drawLine(gridL, ptY, gridR, ptY);
        }

        // major tick and axis line
        this._setColor(yAxisConfig.lineColor);
        this._drawLine(tickL, ptY, tickR, ptY);
        this._drawLine(axisX, axisT, axisX, axisB);

        // minor ticks
        if (yAxisConfig.minorTickCount > 0) {
            for (var minorTickY = y + yAxisLayout.minorTickStep, maxMinorTickY = y + yAxisLayout.gridStep;
                minorTickY < maxMinorTickY && maxMinorTickY < yAxisTag.max; minorTickY += yAxisLayout.minorTickStep) {

                var minorTickCnvY = this._trendYToCanvasY(minorTickY, yAxisTag);
                this._drawLine(minorTickL, minorTickCnvY, minorTickR, minorTickCnvY);
            }
        }

        // label
        this._setColor(yAxisConfig.textColor);
        var lblY = ptY;

        if (isNaN(prevLblY) || prevLblY - lblY > yAxisConfig.fontSize) {
            this._context.fillText(y.toFixed(yAxisLayout.gridDigits), lblX, lblY);
            prevLblY = lblY;
        }
    }

    // axis title
    if (yAxisConfig.showTitle && yAxisTag.title) {
        this._context.textAlign = "center";
        this._context.save();
        this._context.translate(yAxisRect.left + layout.canvasXOffset, yAxisRect.bottom + layout.canvasYOffset);
        this._context.rotate(-Math.PI / 2);

        if (yAxisConfig.position === scada.chart.AreaPosition.LEFT) {
            this._context.textBaseline = "top";
            var titleY = labelMarginT;
        } else {
            this._context.textBaseline = "bottom";
            titleY = yAxisRect.width - labelMarginB;
        }

        this._context.fillText(yAxisTag.title, yAxisRect.height / 2, titleY, yAxisRect.height);
        this._context.restore();
    }
};

// Draws the chart legend.
scada.chart.Chart.prototype._drawLegend = function () {
    if (this.displayOptions.legend.position !== scada.chart.AreaPosition.NONE) {
        var layout = this._chartLayout;
        var legend = this.displayOptions.legend;
        var columnMarginTop = scada.chart.DisplayOptions.getMargin(legend.columnMargin, 0);
        var columnMarginRight = scada.chart.DisplayOptions.getMargin(legend.columnMargin, 1);
        var columnMarginLeft = scada.chart.DisplayOptions.getMargin(legend.columnMargin, 3);
        var trendCnt = this.chartData.trends.length;
        var lineCnt = Math.ceil(trendCnt / legend.columnCount);

        this._context.textAlign = "left";
        this._context.textBaseline = "middle";
        this._context.font = legend.fontSize + "px " + this.displayOptions.chartArea.fontName;

        var fullColumnWidth = legend.columnWidth + columnMarginLeft + columnMarginRight;
        var lineXOffset = layout.legendAreaRect.left + layout.canvasXOffset + columnMarginLeft;
        var lineYOffset = layout.legendAreaRect.top + layout.canvasYOffset + columnMarginTop;

        for (var trendInd = 0; trendInd < trendCnt; trendInd++) {
            var trend = this.chartData.trends[trendInd];
            var columnIndex = Math.trunc(trendInd / lineCnt);
            var lineIndex = trendInd % lineCnt;
            var lineX = fullColumnWidth * columnIndex + lineXOffset;
            var lineY = legend.lineHeight * lineIndex + lineYOffset;
            var iconX = lineX + 0.5;
            var iconY = lineY + (legend.lineHeight - legend.iconHeight) / 2 - 0.5;
            var lblX = lineX + legend.iconWidth + legend.fontSize / 2;
            var lblY = lineY + legend.lineHeight / 2;

            this._setColor(trend.color);
            this._context.fillRect(iconX, iconY, legend.iconWidth, legend.iconHeight);
            this._setColor(legend.foreColor);
            this._context.strokeRect(iconX, iconY, legend.iconWidth, legend.iconHeight);
            this._context.fillText(trend.caption, lblX, lblY);
        }
    }
};

// Draws the trends assigned to the specified Y-axis.
scada.chart.Chart.prototype._drawTrends = function (yAxisTag, opt_startPtInd) {
    for (var trend of yAxisTag.trends) {
        this._drawTrend(this.chartData.timePoints, trend, yAxisTag, opt_startPtInd);
    }
};

// Draws the specified trend.
scada.chart.Chart.prototype._drawTrend = function (timePoints, trend, yAxisTag, opt_startPtInd) {
    var trendPoints = trend.trendPoints;
    var chartGap = this.displayOptions.gapBetweenPoints / scada.chart.const.SEC_PER_DAY;
    var boundRect = this._chartLayout.canvasPlotAreaRect;
    var lineWidth = this.displayOptions.plotArea.lineWidth;
    var VAL_IND = scada.chart.TrendPointIndex.VAL_IND;

    // set clipping region and line width
    if (lineWidth > 1) {
        this._context.save();
        this._context.rect(boundRect.left, boundRect.top, boundRect.width, boundRect.height);
        this._context.clip();
        this._context.lineWidth = lineWidth;
    }

    // set color
    this._setColor(trend.color);

    // draw lines
    var prevX = NaN;
    var prevPtX = NaN;
    var prevPtY = NaN;
    var startPtInd = opt_startPtInd ? opt_startPtInd : 0;
    var ptCnt = timePoints.length;

    for (var ptInd = startPtInd; ptInd < ptCnt; ptInd++) {
        var pt = trendPoints[ptInd];
        var y = pt[VAL_IND];

        if (!isNaN(y)) {
            var x = timePoints[ptInd];
            var ptX = this._trendXToCanvasX(x);
            var ptY = this._trendYToCanvasY(y, yAxisTag);

            if (isNaN(prevX)) {
                // do nothing
            }
            else if (x - prevX > chartGap) {
                this._drawPixel(prevPtX, prevPtY, boundRect, lineWidth);
                this._drawPixel(ptX, ptY, boundRect, lineWidth);
            } else if (prevPtX !== ptX || prevPtY !== ptY) {
                this._drawTrendLine(prevPtX, prevPtY, ptX, ptY, boundRect, lineWidth);
            }

            prevX = x;
            prevPtX = ptX;
            prevPtY = ptY;
        }
    }

    if (!isNaN(prevPtX))
        this._drawPixel(prevPtX, prevPtY, boundRect, lineWidth);

    // restore clipping region and line width
    if (lineWidth > 1) {
        this._context.restore();
    }
};

// Creates a time mark jQuery element if it doesn't exist.
scada.chart.Chart.prototype._initTimeMark = function () {
    if (this._timeMarkJqElem) {
        this._timeMarkJqElem.addClass("hidden");
    } else {
        this._timeMarkJqElem = $("<div class='chart-time-marker hidden'></div>")
            .css("background-color", this.displayOptions.plotArea.markerColor);
        this._chartJqElem.append(this._timeMarkJqElem);
    }
};

// Creates a trend hint jQuery element if it doesn't exist.
scada.chart.Chart.prototype._initTrendHint = function () {
    if (this._trendHintJqElem) {
        this._trendHintJqElem.addClass("hidden");
    } else if (this.chartData.trends.length > 0) {
        this._trendHintJqElem = $("<div class='chart-trend-hint hidden'><div class='time'></div><table></table></div>");
        var table = this._trendHintJqElem.children("table");

        for (var trend of this.chartData.trends) {
            var row = $("<tr><td class='color'><span></span></td><td class='text'></td>" +
                "<td class='colon'>:</td><td class='val'></td></tr>");
            row.find("td.color span").css("background-color", trend.color);
            row.children("td.text").text(trend.caption);
            table.append(row);
        }

        this._chartJqElem.append(this._trendHintJqElem);
    } else {
        this._trendHintJqElem = $();
    }
};

// Shows a hint with the values nearest to the pointer.
scada.chart.Chart.prototype._showHint = function (pageX, pageY, opt_touch) {
    var layout = this._chartLayout;
    var hideHint = true;
    layout.updateAbsCoordinates(this._canvasJqElem);

    if (this._hintEnabled && layout.pointInPlotArea(pageX, pageY)) {
        var ptInd = this._getPointIndex(pageX);

        if (ptInd >= 0) {
            var areaRect = layout.absPlotAreaRect;
            var chartOffset = this._chartJqElem.offset();

            var x = this.chartData.timePoints[ptInd];
            var ptPageX = this._trendXToPageX(x);

            if (areaRect.left <= ptPageX && ptPageX < areaRect.right) {
                hideHint = false;

                // set position and show the time mark
                this._timeMarkJqElem
                    .removeClass("hidden")
                    .css({
                        "left": ptPageX - chartOffset.left,
                        "top": areaRect.top - chartOffset.top,
                        "height": areaRect.height
                    });

                // set text, position and show the trend hint
                this._trendHintJqElem.find("div.time").text(this._dateTimeToStr(x, true));
                var trendCnt = this.chartData.trends.length;
                var hintValCells = this._trendHintJqElem.find("td.val");

                for (var trendInd = 0; trendInd < trendCnt; trendInd++) {
                    var trend = this.chartData.trends[trendInd];
                    var trendPoint = trend.trendPoints[ptInd];
                    hintValCells.eq(trendInd)
                        .text(trendPoint[scada.chart.TrendPointIndex.TEXT_WITH_UNIT_IND])
                        .css("color", trendPoint[scada.chart.TrendPointIndex.COLOR_IND]);
                }

                // allow measuring the hint size
                this._trendHintJqElem
                    .css({
                        "left": 0,
                        "top": 0,
                        "visibility": "hidden"
                    })
                    .removeClass("hidden");

                var hintWidth = this._trendHintJqElem.outerWidth();
                var hintHeight = this._trendHintJqElem.outerHeight();
                var winScrollLeft = $(window).scrollLeft();
                var winRight = winScrollLeft + $(window).width();
                var chartRight = winScrollLeft + areaRect.left + layout.width;
                var maxRight = Math.min(winRight, chartRight);
                var absHintLeft = pageX + hintWidth < maxRight ? pageX : Math.max(pageX - hintWidth, 0);

                this._trendHintJqElem.css({
                    "left": absHintLeft,
                    "top": pageY - chartOffset.top - hintHeight -
                        (opt_touch ? layout.HINT_OFFSET /*above a finger*/ : 0),
                    "visibility": ""
                });
            }
        }
    }

    if (hideHint) {
        this._hideHint();
    }
};

// Hides the hint.
scada.chart.Chart.prototype._hideHint = function () {
    this._timeMarkJqElem.addClass("hidden");
    this._trendHintJqElem.addClass("hidden");
};

// Draws chart areas as colored rectangles for testing.
scada.chart.Chart.prototype._drawAreas = function () {
    var layout = this._chartLayout;

    // plot area
    this._setColor("#ffa500");
    this._context.fillRect(
        layout.plotAreaRect.left + layout.canvasXOffset, layout.plotAreaRect.top + layout.canvasYOffset,
        layout.plotAreaRect.width, layout.plotAreaRect.height);

    // X-axis area
    this._setColor("#008000");
    this._context.fillRect(
        layout.xAxisLayout.areaRect.left + layout.canvasXOffset, layout.xAxisLayout.areaRect.top + layout.canvasYOffset,
        layout.xAxisLayout.areaRect.width, layout.xAxisLayout.areaRect.height);

    // legend area
    this._setColor("#ff00ff");
    this._context.fillRect(
        layout.legendAreaRect.left + layout.canvasXOffset, layout.legendAreaRect.top + layout.canvasYOffset,
        layout.legendAreaRect.width, layout.legendAreaRect.height);

    // Y-axis areas
    var n = 0;
    for (var yAxisRect of layout.yAxisAreaRects) {
        this._setColor(++n % 2 ? "#0000ff" : "#00ffff");
        this._context.fillRect(
            yAxisRect.left + layout.canvasXOffset, yAxisRect.top + layout.canvasYOffset,
            yAxisRect.width, yAxisRect.height);
    }
};

// Builds the DOM of the chart.
scada.chart.Chart.prototype.buildDom = function () {
    if (this._chartJqElem && this.displayOptions) {
        // get chart padding
        var chartPadding = this.displayOptions.chartArea.chartPadding;
        var paddingTop = scada.chart.DisplayOptions.getMargin(chartPadding, 0) + "px";
        var paddingRight = scada.chart.DisplayOptions.getMargin(chartPadding, 1) + "px";
        var paddingBottom = scada.chart.DisplayOptions.getMargin(chartPadding, 2) + "px";
        var paddingLeft = scada.chart.DisplayOptions.getMargin(chartPadding, 3) + "px";

        // find menu
        var menuJqElem = this._chartJqElem.siblings(".chart-menu");
        var menuExists = menuJqElem.length > 0;

        // create title
        if (this.displayOptions.titleConfig.showTitle) {
            this._titleJqElem = $("<div class='chart-title'></div>");
            var titleConfig = this.displayOptions.titleConfig;

            this._titleJqElem.css({
                "height": titleConfig.height + "px",
                "padding-left": paddingLeft,
                "padding-right": paddingRight,
                "font-size": titleConfig.fontSize + "px",
                "line-height": titleConfig.fontSize + "px",
                "color": titleConfig.foreColor
            });

            var menuHolderHtml = titleConfig.showMenu && menuExists ? "<div class='chart-menu-holder'></div>" : "";
            var titleTextHtml = "<div class='chart-title-text'></div>";
            var statusHtml = titleConfig.showStatus ?
                "<div class='chart-status'><span class='chart-status-text'></span></div>" : "";

            this._titleJqElem.append(menuHolderHtml + titleTextHtml + statusHtml);
            this._chartJqElem.append(this._titleJqElem);

            if (menuHolderHtml) {
                // insert menu
                var menuBtnWidth = menuJqElem.css("width");
                menuJqElem.detach();
                this._titleJqElem.find(".chart-menu-holder:first").append(menuJqElem);
                this._titleJqElem.find(".chart-title-text:first").css("padding-left", menuBtnWidth);
            } else {
                menuJqElem.remove();
            }
        } else {
            // remove menu
            menuJqElem.remove();
        }

        // create canvas
        this._canvasJqElem = $("<canvas class='chart-canvas'>Upgrade the browser to display a chart.</canvas>");
        this._chartJqElem.append(this._canvasJqElem);

        this._canvas = this._canvasJqElem[0];
        this._context = this._canvas.getContext("2d");

        // set chart style
        this._chartJqElem.css({
            "border" : 0,
            "padding-top": paddingTop,
            "padding-right": 0,
            "padding-bottom": paddingBottom,
            "padding-left": 0,
            "font-family": this.displayOptions.chartArea.fontName,
            "background-color": this.displayOptions.chartArea.backColor
        });
    }
};

// Shows the chart title.
scada.chart.Chart.prototype.showTitle = function (s) {
    if (this._titleJqElem) {
        this._titleJqElem.find(".chart-title-text:first").text(s);
    }
};

// Shows the chart status.
scada.chart.Chart.prototype.showStatus = function (s, opt_error) {
    if (this._titleJqElem) {
        var statusElem = this._titleJqElem.find(".chart-status:first");
        statusElem.children(".chart-status-text:first").text(s);

        if (opt_error) {
            statusElem.addClass("error");
        } else {
            statusElem.removeClass("error");
        }
    }
};

// Draws the chart.
scada.chart.Chart.prototype.draw = function () {
    if (this._canvas && this.displayOptions && this.timeRange && this.chartData) {

        // initialize necessary objects
        this._initAxisTags();
        this._initTrendFields();
        this._initTimeMark();
        this._initTrendHint();

        // calculate layout
        var layout = this._chartLayout;
        layout.calculate(this.displayOptions, this._chartJqElem, this._context,
            this._xAxisTag, this._yAxisTags, this.chartData.trends.length);

        // update canvas size
        this._chartJqElem.css("overflow", "hidden"); // prevent redundant scrollbars
        this._canvasJqElem
            .outerWidth(layout.canvasAreaRect.width)
            .outerHeight(layout.canvasAreaRect.height);
        this._chartJqElem.css("overflow", "");

        this._canvas.width = layout.canvasAreaRect.width;
        this._canvas.height = layout.canvasAreaRect.height;

        // draw chart
        this._clearRect(0, 0, layout.canvasAreaRect.width, layout.canvasAreaRect.height);
        //this._drawAreas(); // draw colored rectangles for testing
        this._drawFrame();
        this._drawXGrid();
        this._drawLegend();

        for (let yAxisTag of this._yAxisTags) {
            this._drawYGrid(yAxisTag);
        }

        for (let yAxisTag of this._yAxisTags) {
            this._drawTrends(yAxisTag);
        }
    }
};

// Resumes drawing of the chart.
scada.chart.Chart.prototype.resume = function (pointInd) {
    if (pointInd < this.chartData.timePoints.length) {
        if (this._yRangeIsOutdated(pointInd)) {
            this._calcAllYRanges();
            this.draw();
        } else {
            for (var yAxisTag of this._yAxisTags) {
                this._drawTrends(yAxisTag, pointInd ? pointInd - 1 : 0);
            }
        }
    }
};

// Sets the displayed time range.
scada.chart.Chart.prototype.setRange = function (startX, endX) {
    // swap the range if needed
    if (startX > endX) {
        var xbuf = startX;
        startX = endX;
        endX = xbuf;
    }

    // correct the range
    startX = Math.max(startX, this.timeRange.startTime);
    endX = Math.min(endX, this.timeRange.endTime);

    // apply the new range
    if (startX !== endX) {
        this._xAxisTag.min = startX;
        this._xAxisTag.max = endX;
        this._zoomMode = this._xAxisTag.min > this.timeRange.startTime || this._xAxisTag.max < this.timeRange.endTime;
        this._calcAllYRanges();
        this.draw();
    }
};

// Resets the displayed time range according to the chart time range.
scada.chart.Chart.prototype.resetRange = function () {
    this._initAxisTags(true);
    this.draw();
};

// Converts the X-coordinate that means a time into a date string.
scada.chart.Chart.prototype.dateToStr = function (t) {
    var date = this._trendXToDate(t);
    if (scada.utils.iOS()) {
        date.setUTCMinutes(date.getUTCMinutes() + date.getTimezoneOffset());
    }
    return date.toLocaleDateString(this.displayOptions.locale, this._DATE_OPTIONS);
};

// Converts the X-coordinate that means a time into a time string ignoring culture with high performance.
scada.chart.Chart.prototype.timeToStrFast = function (t, opt_showSeconds) {
    var time = new Date(Math.round(t * scada.chart.const.MS_PER_DAY));
    return this._simpleTimeToStr(time, opt_showSeconds);
};

// Binds the events to allow hints.
scada.chart.Chart.prototype.bindHintEvents = function () {
    if (this._canvasJqElem) {
        var thisObj = this;

        $(this._canvasJqElem.parent())
            .off(".scada.chart.hint")
            .on("mousemove.scada.chart.hint touchstart.scada.chart.hint touchmove.scada.chart.hint", function (event) {
                var touch = false;
                var stopEvent = false;

                if (event.type === "touchstart") {
                    event = event.originalEvent.touches[0];
                    touch = true;
                }
                else if (event.type === "touchmove") {
                    $(this).off("mousemove");
                    event = event.originalEvent.touches[0];
                    touch = true;
                    stopEvent = true;
                }

                thisObj._showHint(event.pageX, event.pageY, touch);
                return !stopEvent;
            })
            .on("mouseleave.scada.chart.hint", function () {
                thisObj._hideHint();
            });
    }
};
