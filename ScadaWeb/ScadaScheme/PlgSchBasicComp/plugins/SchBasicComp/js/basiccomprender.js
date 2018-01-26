/*
 * Basic components rendering
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2017
 *
 * Requires:
 * - jquery
 * - schemecommon.js
 * - schemerender.js
 */

/********** Led Renderer **********/

scada.scheme.LedRenderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.LedRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.LedRenderer.constructor = scada.scheme.LedRenderer;

scada.scheme.LedRenderer.prototype.createDom = function (component, renderContext) {
    var props = component.props;

    var divComp = $("<div id='comp" + component.id + "'>" +
        "<div class='basic-led-fill'><div class='basic-led-border'></div></div></div>");
    var divFill = divComp.children().first();
    var divBorder = divFill.children().first();

    this.prepareComponent(divComp, component, true);
    this.setBorderColor(divComp, null, true);
    this.setBackColor(divFill, props.FillColor);
    this.setToolTip(divBorder, props.ToolTip);
    this.bindAction(divBorder, component, renderContext);

    if (props.BorderWidth > 0) {
        this.setBorderColor(divBorder, props.BorderColor, true);

        var opacity = props.BorderOpacity / 100;
        if (opacity < 0) {
            opacity = 0;
        } else if (opacity > 1) {
            opacity = 1;
        }

        divBorder.css({
            "border-width": Math.min(props.BorderWidth, props.Size.Width / 2),
            "border-style" : "solid",
            "opacity": opacity
        });
    }

    component.dom = divComp;
};

scada.scheme.LedRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;
    var divComp = component.dom;
    var curCnlDataExt = renderContext.curCnlDataMap.get(props.InCnlNum);

    if (divComp && curCnlDataExt) {
        // set fill color
        var fillColor = props.FillColor;

        // define fill color according to the channel status
        if (fillColor == this.STATUS_COLOR) {
            fillColor = curCnlDataExt.Color;
        }

        // define fill color according to the led conditions and channel value
        if (curCnlDataExt.Stat > 0 && props.Conditions) {
            var cnlVal = curCnlDataExt.Val;

            for (var cond of props.Conditions) {
                if (scada.scheme.calc.conditionSatisfied(cond, cnlVal)) {
                    fillColor = cond.Color;
                    break;
                }
            }
        }

        // apply fill color
        var divFill = divComp.children().first();
        divFill.css("background-color", fillColor);

        // set border color
        if (props.BorderColor == this.STATUS_COLOR) {
            var divBorder = divFill.children().first();
            divBorder.css("border-color", curCnlDataExt.Color);
        }
    }
};

/********** Toggle Renderer **********/

scada.scheme.ToggleRenderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.ToggleRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.ToggleRenderer.constructor = scada.scheme.ToggleRenderer;

scada.scheme.ToggleRenderer.prototype.createDom = function (component, renderContext) {
    var props = component.props;

    var divComp = $("<div id='comp" + component.id + "' class='basic-toggle undef'>" +
        "<div class='basic-toggle-lever'></div></div>");

    this.prepareComponent(divComp, component, true);
    this.setBorderColor(divComp, null, true);
    this.setBackColor(divFill, props.FillColor);
    this.setToolTip(divBorder, props.ToolTip);
    this.bindAction(divBorder, component, renderContext);

    component.dom = divComp;
}

/********** Renderer Map **********/

// Add components to the renderer map
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchBasicComp.Led", new scada.scheme.LedRenderer());
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchBasicComp.Toggle", new scada.scheme.ToggleRenderer());