<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CurVals.aspx.cs" Inherits="Scada.Web.WFrmCurVals" EnableViewState="False" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Текущие значения</title>
	<link href="css/curVals.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="script/jquery-1.10.1.min.js"></script>
    <script type="text/javascript" src="script/curVals.js"></script>
</head>
<body>
    <form id="CurValsForm" runat="server">
        <asp:Table id="tblCurVals" runat="server" CellPadding="0" CellSpacing="0"></asp:Table>
    </form>
</body>
</html>
