<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="Scada.Web.WFrmMain" EnableViewState="False" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SCADA</title>
    <link href="~/images/favicon.ico" rel="shortcut icon" type="image/vnd.microsoft.icon" />
    <link href="css/scada.css" rel="stylesheet" type="text/css" />
    <link href="css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="script/jquery-1.10.1.min.js"></script>
    <script type="text/javascript" src="script/scada.js"></script>
    <script type="text/javascript" src="script/main.js"></script>
	<script type="text/javascript">
        var viewSet = <%= viewSetIndStr %>;
        var viewTypeArr = <%= viewTypeArrStr %>;
        var viewFileNameArr = <%= viewFileNameArrStr %>;

        // Получить выбранную смену
        function GetStage() {
            if ($("#rbStage1:checked").val())
                return 1;
            else if ($("#rbStage2:checked").val())
                return 2;
            else
                return 0;
        }

        // Отобразить выбранное представление за выбранную дату и смену
        function ShowView() {
            var view = $("#ddlView").val();

            if (view) {
                // определение адреса страницы представления
                var viewQuery = "viewSet=" + viewSet + "&view=" + view;
                var dateQuery = "&year=" + $("#ddlYear").val() + "&month=" + $("#ddlMonth").val() +
                        "&day=" + $("#ddlDay").val();
                var stageQuery = "&stage=" + GetStage();
            
                var viewType = viewTypeArr[view];
                var query;

                if (viewType == "TableView") {
                    query = "TableView.aspx?" + viewQuery + dateQuery + stageQuery;
                } else if (viewType == "FacesView") {
                    query = "faces/ScadaFaces.aspx?" + viewQuery;
                } else if (viewType == "SchemeView") {
                    query = "scheme/ScadaScheme.aspx?" + viewQuery + dateQuery;
                } else if (viewType == "WebPageView") {
                    fileName = viewFileNameArr[view];
                    query = fileName + (fileName.indexOf("?") < 0 ? "?" : "") + viewQuery + dateQuery + stageQuery;
                } else {
                    query = "Error.aspx?msg=" + escape("View type isn't supported.");
                }

                // загрузка представления
                var frameView = $("#frameView");
                if (frameView.attr("src") != query)
                    frameView.attr("src", query);
            }
        }

        // Отобразить события по выбранному представлению и дате
        function ShowEvents() {
            var view = $("#ddlView").val();
            var frameEvents = $("#frameEvents");

            if (view && frameEvents.length > 0) {
                // определение адреса страницы событий
                var query = "Events.aspx?viewSet=" + viewSet + "&view=" + view + "&year=" + 
                    $("#ddlYear").val() + "&month=" + $("#ddlMonth").val() + "&day=" + $("#ddlDay").val()

                // загрузка событий
                frameEvents.attr("src", query);
            }
        }

        // Установить количество дней в зависимости от выбранного месяца и года
        function SetDayCount() {
            var ddlDay = $("#ddlDay");
            var curDay = ddlDay.val();
            var curDayCnt = ddlDay.children("option").length;
            var dayCnt = DaysInMonth($("#ddlYear").val(), $("#ddlMonth").val());

            if (curDayCnt < dayCnt) {
                for (var day = curDayCnt + 1; day <= dayCnt; day++)
                    ddlDay.append($("<option>", { value: day, text: day }));
            }
            else {
                for (var day = dayCnt + 1; day <= curDayCnt; day++)
                    ddlDay.children("option[value='" + day + "']").remove();
            }

            if (curDay > dayCnt)
                ddlDay.val(dayCnt);
        }

        // Отобразить выбранное представление и события
        function ShowData() {
            SetDayCount();
            ShowView();
            ShowEvents();
        }

        // Определить ссылку для экспорта данных в Excel
        function DefineExcelRef() {
            var view = $("#ddlView").val();

            if (view) {
                var frameEvents = $("#frameEvents");
                var eventOut = frameEvents.length > 0 ? frameEvents.contents().find("#rbEvAll:checked").val() ? 
                    "all" : "view" : "none";

                $("#aExcel")
                    .attr("href", 
                        "RepHrEvTableOut.aspx?viewSet=" + viewSet + "&view=" + view + "&year=" + $("#ddlYear").val() + 
                        "&month=" + $("#ddlMonth").val() + "&day=" + $("#ddlDay").val() + "&eventOut=" + eventOut)
                    .attr("target", "_blank");
            } else {
                $("#aExcel")
                    .attr("href", "#")
                    .attr("target", "");
            }
        }

        // Поместить выпадающий список дней после списка месяцев
        function PlaceDayAfterMonth() {
            $("#tdDay").detach().insertAfter($("#tdMonth"));
        }
    </script>
</head>
<body onload="ShowData()">
    <form id="MainForm" runat="server">
        <div id="divTitle">
            <img src="images/title_light.png" width="150" height="30" alt=""/>
        </div>
        <div id="divToolbar">
            <table cellpadding="0" cellspacing="0"><tr>
                <td id="tdView">
                    <asp:DropDownList ID="ddlView" runat="server"></asp:DropDownList></td>
                <td>
                    <asp:Label ID="lblDate" runat="server" Text="Дата"></asp:Label></td>
                <td id="tdDay">
                    <asp:DropDownList ID="ddlDay" runat="server"></asp:DropDownList></td>
                <td id="tdMonth">
                    <asp:DropDownList ID="ddlMonth" runat="server"></asp:DropDownList></td>
                <td id="tdYear">
                    <asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList></td>
                <td class="toolbarCell">
                    <asp:RadioButton ID="rbStage1" runat="server" GroupName="Stage" Text="0-11 ч" /></td>
                <td class="toolbarCell">
                    <asp:RadioButton ID="rbStage2" runat="server" GroupName="Stage" Text="12-23 ч" /></td>
                <td class="toolbarCell">
                    <asp:RadioButton ID="rbStageFull" runat="server" GroupName="Stage" 
                        Text="0-23 ч" Checked="True" /></td>
                <td class="toolbarCell">
                    <a href="Login.aspx" target="_parent"><asp:Image ID="imgHome" runat="server" ImageUrl="~/images/home.gif" ToolTip="Вход в систему" /></a></td>
                <td class="toolbarCell">
                    <a id="aExcel" href="#" onclick="DefineExcelRef()"><asp:Image ID="imgExcel" runat="server" ImageUrl="~/images/excel.gif" ToolTip="Экспорт в Excel" /></a></td>
                <td class="toolbarCell">
                    <a href="Report.aspx" target="_blank"><asp:Image ID="imgReport" runat="server" ImageUrl="~/images/report.gif" ToolTip="Отчёты" /></a></td>
                <td class="toolbarCell">
                    <a href="Info.aspx" target="_parent"><asp:Image ID="imgInfo" runat="server" ImageUrl="~/images/info.gif" ToolTip="Информация" /></a></td>
                <td><iframe runat="server" id="frameLoginChecker" src="LoginChecker.aspx" frameborder="0"></iframe></td>
            </tr></table>
        </div>
        <div id="divView">
            <iframe id="frameView" frameborder="0"></iframe>
        </div>
        <asp:Panel ID="pnlEvents" runat="server">    
            <div id="divSplitter"></div>
            <div id="divSplitterMask"></div>
            <div id="divEvents">
                <iframe id="frameEvents" frameborder="0"></iframe>
            </div>
        </asp:Panel>
    </form>
</body>
</html>