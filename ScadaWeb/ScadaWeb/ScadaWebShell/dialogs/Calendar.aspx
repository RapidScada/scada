<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Calendar.aspx.cs" Inherits="Scada.Web.Dialogs.WFrmCalendar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Calendar</title>
    <link href="~/css/dialogs/calendar.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../lib/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../js/api/utils.js"></script>
    <script type="text/javascript" src="../js/controls/popup.js"></script>
    <script type="text/javascript" src="../js/dialogs/calendar.js"></script>
</head>
<body>
    <form id="frmCalendar" runat="server">
        <asp:Calendar ID="Calendar" runat="server" BorderStyle="None" OnDayRender="Calendar_DayRender">
            <DayHeaderStyle CssClass="day-header" />
            <DayStyle CssClass="day regular" />
            <OtherMonthDayStyle CssClass="day other-month" />
            <SelectedDayStyle CssClass="day selected" />
            <TitleStyle CssClass="header" />
            <TodayDayStyle CssClass="day today" />
        </asp:Calendar>
    </form>
</body>
</html>
