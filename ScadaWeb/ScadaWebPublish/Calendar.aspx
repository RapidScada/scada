<%@ Page Language="C#" AutoEventWireup="true" Inherits="Scada.Web.WFrmCalendar" Codebehind="Calendar.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Выбор даты</title>
    <script type="text/javascript">
        // Произвести выбор даты
        function DoSelect(txtID, txtVal)
        {            
            var openerWin = window.opener;
            if (openerWin != null && openerWin.closed) {
                var txt = openerWin.document.getElementById(txtID);
                if (txt != null) 
                    txt.value = txtVal;
            }
            window.close();
        }
    </script>
</head>
<body style="margin: 0px; overflow: hidden">
    <form id="CalendarForm" runat="server">
        <asp:Calendar ID="Calendar" runat="server" BackColor="White" BorderColor="#999999"
            CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt"
            ForeColor="Black" Height="180px" OnSelectionChanged="Calendar_SelectionChanged"
            Width="200px">
            <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
            <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
            <SelectorStyle BackColor="#CCCCCC" />
            <WeekendDayStyle BackColor="#FFFFCC" />
            <OtherMonthDayStyle ForeColor="Gray" />
            <NextPrevStyle VerticalAlign="Bottom" />
            <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
            <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
        </asp:Calendar>
    </form>
</body>
</html>
