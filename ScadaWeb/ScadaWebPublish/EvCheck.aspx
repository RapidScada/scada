<%@ Page Title="Квитирование события" Language="C#" MasterPageFile="~/MasterLight.master" AutoEventWireup="true" Inherits="Scada.Web.WFrmEvCheck" Codebehind="EvCheck.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contHead" Runat="Server">
    <link href="css/evCheck.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contLight" Runat="Server">
    <asp:MultiView ID="mvMain" runat="server" ActiveViewIndex="0">
        <asp:View ID="viewCheck" runat="server">
            <div id="divMessage">
                <asp:Label ID="lblMessage" runat="server" Text="Ошибка" ForeColor="Red"></asp:Label>
            </div>
            <div id="divEvent">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td><asp:Label ID="lblNumCaption" runat="server" Text="№:"></asp:Label></td>
                        <td>
                            <table cellpadding="0" cellspacing="0"><tr>
                                <td><asp:Label ID="lblNum" runat="server"></asp:Label></td>
                                <td><asp:Label ID="lblDateCaption" runat="server" Text="Дата:"></asp:Label></td>
                                <td><asp:Label ID="lblDate" runat="server"></asp:Label></td>
                                <td><asp:Label ID="lblTimeCaption" runat="server" Text="Время:"></asp:Label></td>
                                <td><asp:Label ID="lblTime" runat="server"></asp:Label></td>
                            </tr></table>
                        </td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lblObjCaption" runat="server" Text="Объект:"></asp:Label></td>
                        <td><asp:Label ID="lblObj" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lblKPCaption" runat="server" Text="КП:"></asp:Label></td>
                        <td><asp:Label ID="lblKP" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lblCnlCaption" runat="server" Text="Канал:"></asp:Label></td>
                        <td><asp:Label ID="lblCnl" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lblEventCaption" runat="server" Text="Событие:"></asp:Label></td>
                        <td><asp:Label ID="lblEvent" runat="server"></asp:Label></td>
                    </tr>
                </table>
            </div>
            <div id="divActions">
                <asp:Button 
                    ID="btnCheck" runat="server" CssClass="Button" Text="Квитировать" OnClick="btnCheck_Click" /><asp:Button 
                    ID="btnCancel" runat="server" CssClass="Button" Text="Отмена" OnClientClick="window.close(); return false;" />
            </div>
        </asp:View>
        <asp:View ID="viewResult" runat="server">
            <div id="divResult">
                <div><asp:Label ID="lblResultSuccessful" 
                    runat="server" Text="Квитирование выполнено успешно" ForeColor="Green"></asp:Label><asp:Label ID="lblResultFailed" 
                    runat="server" Text="Квитирование не выполнено" Visible="False" ForeColor="Red"></asp:Label></div>
                <div><asp:Button ID="btnClose" runat="server" Text="Закрыть" OnClientClick="window.close(); return false;" /></div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>

