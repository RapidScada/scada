scada.scheme.LedRenderer = function () {
    scada.scheme.StaticTextRenderer.call(this);
};

scada.scheme.LedRenderer.prototype = Object.create(scada.scheme.StaticTextRenderer.prototype);
scada.scheme.LedRenderer.constructor = scada.scheme.LedRenderer;

scada.scheme.rendererMap.set("Scada.Web.Plugins.SchBasicComp.Led", new scada.scheme.LedRenderer());