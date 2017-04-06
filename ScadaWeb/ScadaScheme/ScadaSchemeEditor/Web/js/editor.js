var API_PATH1 = "http://minicrm.rapidscada.net/api/v1/";
var API_PATH2 = "http://localhost:10002/ScadaSchemeEditor/";

function getPluginInfo() {
    $.ajax({
        //url: API_PATH1 + "StoreSvc.svc/GetModule?code=PlgChartPro",
        url: API_PATH2 + "SchemeEditorSvc/DoWork?arg=test",
        method: "GET",
        dataType: "json",
        cache: false
    })
    .done(function (data, textStatus, jqXHR) {
        try {
            console.log("done");
            $("div").text(data.d);
        }
        catch (ex) {
            console.error(ex.message);
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        console.error("fail");
    })
    .always(function () {
    });
}

function obtainPluginInfo(parsedData) {
    var module = parsedData.Data;

    if (module.ModuleID) {
        $("div").text(module.LongDescr);
    }
}

// Update layout of the top level div elements
function updateLayout() {
    $("#divSchWrapper")
        .outerWidth($(window).width())
        .outerHeight($(window).height());
}

$(document).ready(function () {
    //getPluginInfo();

    updateLayout();

    $(window).on("resize", function () {
        updateLayout();
    });
});