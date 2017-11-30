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

    var spanComp = $("<span id='comp" + component.id + "'></span>");
    var spanText = $("<span></span>");

    this.prepareComponent(spanComp, component);
    /*this.setBackColor(spanComp, props.BackColor);
    this.setBorderColor(spanComp, props.BorderColor, true);
    this.setFont(spanComp, props.Font);
    this.setForeColor(spanComp, props.ForeColor);

    if (props.AutoSize) {
        this.setWordWrap(spanText, false);
    } else {
        spanComp.css("display", "table");

        spanText
            .css({
                "display": "table-cell",
                "overflow": "hidden",
                "max-width": props.Size.Width,
                "width": props.Size.Width,
                "height": props.Size.Height
            });

        this.setHAlign(spanComp, props.HAlign);
        this.setVAlign(spanText, props.VAlign);
        this.setWordWrap(spanText, props.WordWrap);
    }

    spanText.text(props.Text);
    spanComp.append(spanText);*/

    spanText.text("I'm led!");
    spanComp.append(spanText);

    component.dom = spanComp;
};

scada.scheme.LedRenderer.prototype.setSize = function (component, width, height) {
    component.props.Size = { Width: width, Height: height };

    var spanText = component.dom.children();
    spanText.css({
        "max-width": width,
        "width": width,
        "height": height
    });
};

/********** Renderer Map **********/

// Add components to the renderer map
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchBasicComp.Led", new scada.scheme.LedRenderer());