var HourColMinWidth = 20;                   // мин. ширина столбца часовых значений
var TblHourMinWidth = HourColMinWidth * 24; // мин. ширина таблицы часовых значений

// Установить ширину таблицы часовых значений
function SetHourTableWidth() {
    var tblHourWidth = $("#divTableView").width() - $("#tblCap").outerWidth() - $("#tblCur").outerWidth();

    if ($("body").height() <= $(window).height())
        tblHourWidth -= 20; // поле справа

    if (tblHourWidth < TblHourMinWidth)
        tblHourWidth = TblHourMinWidth;

    $("#tblHour").width(tblHourWidth);
    $("#tblHour").css("max-width", tblHourWidth); // особенность IE10
}

$(document).ready(function () {
    // установка одинаковой ширины столбцов таблицы часовых значений
    var cells = $("#tblHour").find("tr:first").children("td");
    var colCnt = cells.length;

    if (colCnt > 0)
        cells.css("width", (100 / colCnt) + "%");

    // установка ширины таблицы часовых значений
    TblHourMinWidth = HourColMinWidth * colCnt;
    SetHourTableWidth();

    $(window).resize(function () {
        SetHourTableWidth();
    });

    // настройка всплывающих подсказок
    $("#tblCap img").hover(
        function () {
            var imgIcon = $(this);
            var divHint = imgIcon.closest("td").children(".hint");
            var win = $(window);

            var left = imgIcon.offset().left + imgIcon.width();
            var top = imgIcon.offset().top + imgIcon.height();
            var height = divHint.outerHeight(true);

            if (top + height > win.scrollTop() + win.height()) {
                var newTop = imgIcon.offset().top - height;

                if (newTop >= win.scrollTop())
                    top = newTop;
            }

            divHint.css({
                "display": "inline",
                "left": left,
                "top": top
            });
        },

        function () {
            var divHint = $(this).closest("td").children(".hint");
            divHint.css("display", "none");
        }
    );
});