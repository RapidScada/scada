<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvTable.aspx.cs" Inherits="Scada.Web.WFrmEvTable" EnableViewState="False" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Таблица событий</title>
	<link href="css/scada.css" rel="stylesheet" type="text/css" />
	<link href="css/evTable.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="script/jquery-1.10.1.min.js"></script>
    <script type="text/javascript" src="script/scada.js"></script>
    <script type="text/javascript" src="script/evTable.js"></script>
</head>
<body>
    <form id="EvTableForm" runat="server">
        <div id="divEvents">
            <asp:Table ID="tblEv" runat="server" CellPadding="0" CellSpacing="0">
                <asp:TableRow runat="server" CssClass="hdr">
                    <asp:TableCell runat="server" Width="5%"><asp:Label ID="lblNumColumn" runat="server" Text="№"></asp:Label></asp:TableCell>
                    <asp:TableCell runat="server" Width="5%"><asp:Label ID="lblDateColumn" runat="server" Text="Дата"></asp:Label></asp:TableCell>
                    <asp:TableCell runat="server" Width="5%"><asp:Label ID="lblTimeColumn" runat="server" Text="Время"></asp:Label></asp:TableCell>
                    <asp:TableCell runat="server" Width="10%"><asp:Label ID="lblObjColumn" runat="server" Text="Объект"></asp:Label></asp:TableCell>
                    <asp:TableCell runat="server" Width="10%"><asp:Label ID="lblKPColumn" runat="server" Text="КП"></asp:Label></asp:TableCell>
                    <asp:TableCell runat="server" Width="20%"><asp:Label ID="lblCnlColumn" runat="server" Text="Канал"></asp:Label></asp:TableCell>
                    <asp:TableCell runat="server" Width="40%"><asp:Label ID="lblEventColumn" runat="server" Text="Событие"></asp:Label></asp:TableCell>
                    <asp:TableCell runat="server" Width="5%"><asp:Label ID="lblCheckColumn" runat="server" Text="Квит."></asp:Label></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>    
        <div id="divNoData"><asp:Label ID="lblNoData" 
            runat="server" Text="События отсутствуют." Visible="False"></asp:Label><asp:Label ID="lblLoading" 
            runat="server" Text="Загрузка..." Visible="False"></asp:Label></div>
        <asp:HiddenField ID="hidEvStamp" runat="server" />
        <asp:HiddenField ID="hidSndEvNum" runat="server" />
    </form>
</body>
</html>
