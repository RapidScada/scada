<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TableView.aspx.cs" Inherits="Scada.Web.WFrmTableView" EnableViewState="False" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Табличное представление</title>
	<link href="css/scada.css" rel="stylesheet" type="text/css" />
	<link href="css/tableView.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="script/jquery-1.10.1.min.js"></script>
    <script type="text/javascript" src="script/scada.js"></script>
    <script type="text/javascript" src="script/tableView.js"></script>
</head>
<body>
    <form id="TableViewForm" runat="server">
        <div id="divTableView">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td id="tdCap">
                        <asp:Table ID="tblCap" runat="server" CellPadding="0" CellSpacing="0">
                        </asp:Table>
                    </td>
                    <td id="tdCur">
                        <asp:Table ID="tblCur" runat="server" CellPadding="0" CellSpacing="0">
                        </asp:Table>
                    </td>
                    <td id="tdHour">
                        <asp:Table ID="tblHour" runat="server" CellPadding="0" CellSpacing="0">
                        </asp:Table>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divNoData"><asp:Label ID="lblNoData" runat="server" Text="Табличное представление пусто." Visible="False"></asp:Label></div>
    </form>
</body>
</html>
