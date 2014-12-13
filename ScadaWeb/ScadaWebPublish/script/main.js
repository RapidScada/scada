var MinAreaHeight = 100; // минимальная высота области представления и области событий
var resizing = false;    // выполняется изменение размеров областей

var divView = null;
var divEvents = null;
var frameView = null;
var frameEvents = null;

// Установить размеры фреймов в соответствии с областями, в которые они входят
function SyncFrameSize() {
    frameView.width(divView.width());
    frameView.height(divView.height());
    frameEvents.width(divEvents.width());
    frameEvents.height(divEvents.height());
}

// Установить высоту области
function SetAreaHeight(area, height) {
    area.height(height);
    area.children("iframe").height(height);
}

// Установить высоту области представлений, чтобы страница заполнила окно браузера
function SetViewHeight() {
    var viewH = $(window).height() - $("#divTitle").outerHeight() - $("#divToolbar").outerHeight();

    if (viewH < MinAreaHeight)
        viewH = MinAreaHeight;

    divView.height(viewH);
}

// Вычислить высоту области событий в зависимости от высоты области представления
function CalcEventsHeight(viewHeight) {
    return eventsH = $(window).height() - $("#divTitle").outerHeight() - $("#divToolbar").outerHeight() -
         viewHeight - $("#divSplitter").outerHeight();
}

// Установить высоту области событий, чтобы страница заполнила окно браузера
function SetEventsHeight() {
    var eventsH = CalcEventsHeight(divView.outerHeight());
    if (eventsH < MinAreaHeight)
        eventsH = MinAreaHeight;
    divEvents.height(eventsH);
}

// Завершить изменение размеров областей
function StopResizing() {
    if (resizing) {
        resizing = false;
        $("#divSplitterMask").css("visibility", "hidden");

        // сохранение высоты области представления в cookie
        SetCookie("ScadaViewHeight", $("#divView").height(), 30);
    }
}

$(document).ready(function () {
    divView = $("#divView");
    divEvents = $("#divEvents");
    frameView = $("#frameView");
    frameEvents = $("#frameEvents");

    // установка высоты области представления
    var viewH = divEvents.length > 0 ?
        GetCookie("ScadaViewHeight")/* получение высоты из cookie */ :
        $(window).height() - $("#divTitle").outerHeight() - $("#divToolbar").outerHeight()/* расчёт высоты */;

    if (viewH) {
        if (viewH < MinAreaHeight)
            viewH = MinAreaHeight;
        divView.height(viewH);
    }

    // установка высоты области событий и синхронизация размеров областей
    SetEventsHeight();
    SyncFrameSize();

    // настройка работы разделителя, изменяющего размеры областей
    var startMouseY = 0; // начальная позиция указателя мыши при изменении размеров областей
    var startViewH = 0;  // начальная высота области представления

    $("#divSplitter").mousedown(function (event) {
        // начало изменения размеров областей
        startMouseY = event.pageY;
        startViewH = divView.height();
        $("#divSplitterMask").css({ "opacity": 0.0, "visibility": "visible" });
        resizing = true;
    });

    $("body")
    .mousemove(function (event) {
        // изменение размеров областей
        if (resizing) {
            var delta = event.pageY - startMouseY;
            var curViewH = divView.height();
            var newViewH = startViewH + delta;
            var newEventsH = CalcEventsHeight(newViewH);

            if (newViewH >= MinAreaHeight && (newEventsH >= MinAreaHeight || curViewH > newViewH)) {
                SetAreaHeight(divView, newViewH);

                if (newEventsH < MinAreaHeight)
                    newEventsH = MinAreaHeight;
                SetAreaHeight(divEvents, newEventsH);
            }
        }
    })
    .mouseup(function () {
        StopResizing();
    })
    .mouseleave(function () {
        StopResizing();
    });

    $(window).resize(function () {
        StopResizing();

        if (divEvents.length > 0)
            SetEventsHeight();
        else
            SetViewHeight();

        SyncFrameSize();
    });
});