// Открыть дочернее окно
function OpenWin(url, winName, width, height) {
    var features = "";

    if (width) {
        features += "width=" + width + ",";
        features += "left=" + (screen.width - width) / 2 + ",";
    }

    if (height) {
        features += "height=" + height + ",";
        features += "top=" + (screen.height - height) / 2 + ",";
    }

    return window.open(url, winName, features + "toolbar=no,status=no,scrollbars=yes,resizable=yes");
}

// Окно календаря для выбора даты
var calendarWin = null;

// Открыть календарь для выбора даты в заданное текстовое поле
function ShowCalendar(txtID, date, path) {
    CloseCalendar();
    calendarWin = window.open(path + "Calendar.aspx?txtID=" + txtID + "&date=" + date, "CalendarWin",
        "width=200,height=180,left=" + event.screenX + ",top=" + event.screenY +
        ",toolbar=no,status=no,scrollbars=no,resizable=no");
}

// Закрыть календарь
function CloseCalendar() {
    if (calendarWin && !calendarWin.closed)
        calendarWin.close();
}

// Получить значение cookie по имени
function GetCookie(name) {
    var cookie = " " + window.document.cookie;
    var search = " " + name + "=";
    var offset = cookie.indexOf(search);

    if (offset >= 0) {
        offset += search.length;
        var end = cookie.indexOf(";", offset)

        if (end < 0)
            end = cookie.length;

        return cookie.substring(offset, end);
    } else {
        return null;
    }
}

// Установить cookie
function SetCookie(name, value, daysCount) {
    var expireDate = new Date();
    expireDate.setDate(expireDate.getDate() + daysCount);
    window.document.cookie = name + "=" + value + "; expires=" + expireDate.toGMTString();
}

// Определить количество дней в месяце
function DaysInMonth(year, month) {
    return new Date(year, month, 0).getDate();
}

// Открыть форму графика входного канала
function ShowDiag(viewSet, view, year, month, day, cnlNum, path) {
    window.open((path ? path : "") + "Diag.aspx?viewSet=" + viewSet + "&view=" + view + 
        "&year=" + year + "&month=" + month + "&day=" + day + "&cnlNum=" + cnlNum);
}

// Открыть форму отправки команды телеуправления
function SendCmd(viewSet, view, ctrlCnlNum, path) {
    OpenWin((path ? path : "") + "CmdSend.aspx?viewSet=" + viewSet + "&view=" + view + "&ctrlCnlNum=" + ctrlCnlNum, 
        "CommandWin", 400, 340);
}

// Открыть форму квитирования события
function CheckEvent(viewSet, view, year, month, day, evNum) {
    OpenWin("EvCheck.aspx?viewSet=" + viewSet + "&view=" + view + "&year=" + year + 
    "&month=" + month + "&day=" + day + "&evNum=" + evNum, "CheckWin", 400, 250);
}