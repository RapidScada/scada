// Установить высоту фрейма таблицы событий
function SetEvTableFrameHeight() {
    var height = $(window).height() - $("#divControl").outerHeight(true);
    $("#frameEvTable").height(height);
}

// Отобразить события по выбранному фильтру
function ShowEvents() {
    var search = window.location.search;
    var query = "EvTable.aspx" + (search.indexOf("?") < 0 ? "?" : search + "&") +
        "filter=" + ($("#rbEvAll:checked").val() ? "all" : "view");
    $("#frameEvTable").attr("src", query);
    $("#frameEvUpdater").attr("src", "EvUpdater.aspx" + search);
}

// Воспроизвести звук события
function PlayEventSound(evNum) {
    var hidLastEvNum = $("#hidLastEvNum");
    var lastEvNum = hidLastEvNum.val();
    var evNumInt = parseInt(evNum);

    if (lastEvNum) {
        if (evNumInt > parseInt(lastEvNum)) {
            hidLastEvNum.val(evNum);
            var audio = $("#audioEvSound");

            if (audio[0].play)
                audio[0].play();
            else
                $("#embedEvSound")[0].Play();
        }
    } else if (evNumInt > 0) {
        hidLastEvNum.val(evNum);
    }
}

$(document).ready(function () {
    // установка высоты фрейма таблицы событий
    SetEvTableFrameHeight();

    $(window).resize(function () {
        SetEvTableFrameHeight();
    });

    // отображение событий
    ShowEvents();

    $("#tblControl input[type=radio]").change(function () {
        ShowEvents();
    });
});