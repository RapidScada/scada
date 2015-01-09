<%@ Page Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" Inherits="Scada.Web.WFrmInfo" Title="SCADA - Информация" Codebehind="Info.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contHead" Runat="Server">
    <link href="css/info.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contMain" Runat="Server" EnableViewState="False">
    <table id="tblInfo" cellpadding="0" cellspacing="0">
        <tr>
            <td></td>
            <td><h2><asp:Label id="lblTitle" runat="server" Text="Информация о SCADA-Web"></asp:Label></h2></td>
        </tr>
        <tr>
            <td><asp:Label id="lblVersionCation" runat="server" Text="Версия:"></asp:Label></td>
            <td>4.4</td>
        </tr>
        <tr>
            <td><asp:Label id="lblServerCaption" runat="server" Text="Сервер:"></asp:Label></td>
            <td><asp:Label id="lblServer" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td><asp:Label id="lblUserCaption" runat="server" Text="Пользователь:"></asp:Label></td>
            <td><asp:Label id="lblUser" runat="server"></asp:Label><asp:Label id="lblUserNotLoggedOn" 
                runat="server" Text="<Вход не выполнен>" Visible="False"></asp:Label></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:Button id="btnBack" runat="server" Text="Вернуться назад" UseSubmitBehavior="False" 
                OnClientClick="history.go(-1); return false;"></asp:Button></td>
        </tr>
    </table>
</asp:Content>

