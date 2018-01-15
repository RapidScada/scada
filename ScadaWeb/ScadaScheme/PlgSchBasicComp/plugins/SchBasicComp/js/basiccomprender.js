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

    var divComp = $("<div id='comp" + component.id + "'></div>");
    this.prepareComponent(divComp, component, true);
    this.setBackColor(spanComp, props.FillColor);
    divComp.css("border-radius", "50%");

    component.dom = divComp;
};

scada.scheme.LedRenderer.prototype.setSize = function (component, width, height) {
    scada.scheme.ComponentRenderer.prototype.setSize.call(this, component, width, height);

    // TODO: set size of the border
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
        if (curCnlDataExt.Stat && props.Conditions) {
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
    }
};

/********** Renderer Map **********/

// Add components to the renderer map
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchBasicComp.Led", new scada.scheme.LedRenderer());