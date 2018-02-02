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

    var divComp = $("<div id='comp" + component.id + "' class='basic-led'></div>");
    this.prepareComponent(divComp, component, true);
    this.setBackColor(divComp, props.FillColor);
    this.setToolTip(divComp, props.ToolTip);
    this.bindAction(divComp, component, renderContext);

    if (props.BorderWidth > 0) {
        var divBorder = $("<div class='basic-led-border'></div>").appendTo(divComp);
        this.setBorderColor(divBorder, props.BorderColor);
        this.setBorderWidth(divBorder, props.BorderWidth);

        var opacity = props.BorderOpacity / 100;
        if (opacity < 0) {
            opacity = 0;
        } else if (opacity > 1) {
            opacity = 1;
        }

        divBorder.css("opacity", opacity);
        divComp.append(divBorder);
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
        divComp.css("background-color", fillColor);

        // set border color
        if (props.BorderColor == this.STATUS_COLOR) {
            var divBorder = divComp.children(".basic-led-border");
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

    var divComp = $("<div id='comp" + component.id + "' class='basic-toggle undef'></div>");
    var divLever = $("<div class='basic-toggle-lever'></div>");

    this.prepareComponent(divComp, component, true);
    this.setBackColor(divComp, props.BackColor);
    this.setBorderColor(divComp, props.BorderColor);
    this.setBorderWidth(divComp, props.BorderWidth);
    this.setToolTip(divComp, props.ToolTip);
    this.bindAction(divComp, component, renderContext);

    var minSize = Math.min(props.Size.Width, props.Size.Height);
    divComp.css({
        "border-radius": minSize / 2,
        "padding": props.Padding
    });

    // lever
    this.setBackColor(divLever, props.LeverColor);
    var leverSize = minSize - (props.BorderWidth + props.Padding) * 2;
    divLever.css({
        "width": leverSize,
        "height": leverSize
    });

    divComp.append(divLever);
    component.dom = divComp;
}

/********** Renderer Map **********/

// Add components to the renderer map
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchBasicComp.Led", new scada.scheme.LedRenderer());
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchBasicComp.Toggle", new scada.scheme.ToggleRenderer());