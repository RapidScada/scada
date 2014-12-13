// Установить ширину таблицы событий
function SetHourTableWidth() {
    var tblEvMinWidth = 200; // мин. ширина таблицы событий
    var tblEvWidth = $("#divEvents").width();

    if ($("#divEvents").height() <= $(window).height())
        tblEvWidth -= 20; // поле справа

    if (tblEvWidth < tblEvMinWidth)
        tblEvWidth = tblEvMinWidth;

    $("#tblEv").width(tblEvWidth);
}

$(document).ready(function () {
    // установка ширины таблицы событий
    SetHourTableWidth();

    $(window).resize(function () {
        SetHourTableWidth();
    });

    // воспроизведение звука 
    if (window.parent)
        window.parent.PlayEventSound($("#hidSndEvNum").val());
});