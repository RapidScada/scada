<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Events.aspx.cs" Inherits="Scada.Web.Plugins.Table.WFrmEvents" EnableViewState="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Events - Rapid SCADA</title>
    <link href="~/lib/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="~/lib/open-sans/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="~/css/controls/notifier.min.css" rel="stylesheet" type="text/css" />
    <link href="~/css/controls/tableheader.min.css" rel="stylesheet" type="text/css" />
    <link href="~/plugins/Table/css/events.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../lib/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../js/api/utils.js"></script>
    <script type="text/javascript" src="../../js/api/clientapi.js"></script>
    <script type="text/javascript" src="../../js/api/eventtypes.js"></script>
    <script type="text/javascript" src="../../js/api/viewhub.js"></script>
    <script type="text/javascript" src="../../js/controls/notifier.js"></script>
    <script type="text/javascript" src="../../js/controls/tableheader.js"></script>
    <script type="text/javascript" src="js/tablecommon.js"></script>
    <script type="text/javascript">
        var DEBUG_MODE = <%= debugMode ? "true" : "false" %>;
        var viewID = <%= viewID %>;
        var dataRefrRate = <%= dataRefrRate %>;
        var arcRefrRate = <%= arcRefrRate %>;
        var phrases = <%= phrases %>;
        var today = <%= today %>;
        var locale = "<%= Scada.Localization.Culture.Name %>";
        var dispEventCnt = <%= dispEventCnt %>;
    </script>
    <script type="text/javascript" src="js/events.js"></script>
</head>
<body>
    <form id="frmEvents" runat="server">
        <div id="divNotif" class="notifier">
        </div>
        <div id="divToolbar"><span id="spanDate" class="tool-ctrl"><asp:TextBox ID="txtDate" runat="server"></asp:TextBox><i class="fa fa-calendar" aria-hidden="true"></i></span><span 
             id="spanAllEventsBtn" runat="server" class="tool-btn">All Events</span><span 
             id="spanEventsByViewBtn" class="tool-btn">Events by View</span><span 
             id="spanExportBtn" class="tool-btn"><i class="fa fa-print" aria-hidden="true"></i></span><div id="divDebugTools"><span 
                 id="spanTitleChangedBtn" class="tool-btn">TitleChanged</span><span 
                 id="spanNavigateBtn" class="tool-btn">Navigate</span><span 
                 id="spanDateChangedBtn" class="tool-btn">DateChanged</span>
            </div>
        </div>
        <div id="divTblWrapper" class="table-wrapper">
            <table id="tblEvents">
                <tr class="hdr">
                    <td class="num">Number</td>
                    <td class="time">Date and Time</td>
                    <td class="obj">Object</td>
                    <td class="dev">Device</td>
                    <td class="cnl">Channel</td>
                    <td class="text">Description</td>
                    <td class="ack">Ack</td>
                </tr>
                <!--<tr class="event">
                    <td class="num">1</td>
                    <td class="time">14/06/2016 10:45:23</td>
                    <td class="obj">Enterprise</td>
                    <td class="dev">ADAM-6015 Server room</td>
                    <td class="cnl">t4_delayed</td>
                    <td class="text">Normal: 21.0 °C</td>
                    <td class="ack">No</td>
                </tr>-->
            </table>
        </div>
    </form>
</body>
</html>
