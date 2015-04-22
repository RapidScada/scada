<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Events.aspx.cs" Inherits="Scada.Web.WFrmEvents" EnableViewState="False" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cобытия</title>
	<link href="css/scada.css" rel="stylesheet" type="text/css" />
	<link href="css/events.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="script/jquery-1.10.1.min.js"></script>
    <script type="text/javascript" src="script/events.js"></script>
</head>
<body>
    <form id="EventsForm" runat="server">
        <div id="divControl">
            <table id="tblControl" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0"><tr>
                            <td><asp:Label ID="lblEvents" runat="server" Text="События:"></asp:Label></td>
                            <td><asp:RadioButton ID="rbEvAll" runat="server" GroupName="Event" Text="Все" /></td>
                            <td><asp:RadioButton ID="rbEvView" runat="server" GroupName="Event" Text="По представлению" /></td>
                            <td><iframe id="frameEvUpdater" frameborder="0"></iframe></td>
                            <td><audio id="audioEvSound" preload="auto">
                                    <source src="sound/event.wav" type="audio/wav" />
                                    <source src="sound/event.mp3" type="audio/mpeg" />
                                    <embed id="embedEvSound" src="sound/event.wav" type="audio/wav" 
                                        autostart="false" loop="false" hidden="true" /><input id="hidLastEvNum" type="hidden" />
                                </audio>
                            </td>
                        </tr></table>
                    </td>
                    <td id="tdEvPause">
                        <asp:CheckBox ID="chkEvPause" ClientIDMode="Static" runat="server" /><asp:Label ID="lblEvPause" runat="server" 
                            Text="Пауза" AssociatedControlID="chkEvPause"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <iframe id="frameEvTable" frameborder="0"></iframe>
        </div>
    </form>
</body>
</html>
