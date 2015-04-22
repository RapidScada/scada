<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvUpdater.aspx.cs" Inherits="Scada.Web.WFrmEvUpdater" EnableViewState="False" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Events Table Updater</title>
    <script type="text/javascript" src="script/jquery-1.10.1.min.js"></script>
    <script type="text/javascript" src="script/evUpdater.js"></script>
</head>
<body style="margin: 0px; background-color: #ccffff">
    <form id="EvUpdaterForm" runat="server">
        <asp:HiddenField ID="hidEvStamp" runat="server" />
    </form>
</body>
</html>