$(document).ready(function () {
    // обновление таблицы событий, если не установлена пауза и данные устарели
    if (window.parent) {
        if ($("#chkEvPause:not(:checked)", window.parent.document).val()) {
            var frameEvTable = $("#frameEvTable", window.parent.document);
            var hidEvStamp1 = frameEvTable.contents().find("#hidEvStamp").val();
            var hidEvStamp2 = $("#hidEvStamp").val();

            if (hidEvStamp1 && hidEvStamp2 && hidEvStamp1 != hidEvStamp2) {
                var frameWnd = frameEvTable[0].contentWindow;
                frameWnd.location = frameWnd.location;
            }
        }
    }
});