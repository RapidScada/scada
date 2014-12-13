$(document).ready(function () {
    // корректировка ширины таблицы текущих значений
    var tblCurVals = $("#tblCurVals");
    var tblCurValsWidth = tblCurVals.width();
    var width = tblCurValsWidth + 10 - tblCurValsWidth % 10; // кратно 10
    var minWidth = 70;

    if (width < minWidth)
        width = minWidth;

    tblCurVals.width(width)

    // настройка родительской страницы - табличного представления
    if (window.parent) {
        // установка высоты фрейма, содержащего данную страницу
        var frameCurVal = $("#frameCurVal", window.parent.document);
        frameCurVal.height(tblCurVals.height());

        if (frameCurVal.width() != width) {
            // установка ширины фрейма
            frameCurVal.width(width);
            // установка ширины таблицы часовых значений
            window.parent.SetHourTableWidth();
        }
    }
});