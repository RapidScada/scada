<%@ Page Language="C#" MasterPageFile="~/MasterLight.master" AutoEventWireup="true" Inherits="Scada.Web.WFrmCmdSend" Title="Команда телеуправления" ValidateRequest="false" Codebehind="CmdSend.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contHead" Runat="Server">
    <link href="css/cmdSend.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contLight" Runat="Server">
    <asp:MultiView ID="mvMain" runat="server" ActiveViewIndex="0">
        <asp:View ID="viewCommand" runat="server">
            <div id="divMessage">
                <asp:Label ID="lblMessage" runat="server" Text="Ошибка" ForeColor="Red"></asp:Label>
            </div>
            <div id="divProps">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td><asp:Label ID="lblCtrlCnlCaption" runat="server" Text="Канал упр.:"></asp:Label></td>
                        <td><asp:Label ID="lblCtrlCnl" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lblObjCaption" runat="server" Text="Объект:"></asp:Label></td>
                        <td><asp:Label ID="lblObj" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lblKPCaption" runat="server" Text="КП:"></asp:Label></td>
                        <td><asp:Label ID="lblKP" runat="server"></asp:Label></td>
                    </tr>
                </table>
            </div>
            <div id="divCmd">
                <asp:Panel ID="pnlPassword" runat="server">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td><asp:Label ID="lblPassword" runat="server" Text="Пароль"></asp:Label></td>
                            <td><asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox></td>
                        </tr>
                    </table>
                </asp:Panel>
            
                <asp:MultiView ID="mvCommand" runat="server" ActiveViewIndex="0">
                    <asp:View ID="viewStandCmd1" runat="server">
                        <table id="tblStandCmd1" cellpadding="0" cellspacing="0">
                            <tr>
                                <td><asp:Label ID="lblCmd1" runat="server" Text="Команда:"></asp:Label></td>
                                <td><asp:RadioButtonList ID="rblCmdVal" runat="server" RepeatColumns="2" 
                                        RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    </asp:RadioButtonList></td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="viewStandCmd1Simple" runat="server">
                        <table id="tblStandCmd1Simple" cellpadding="0" cellspacing="0">
                            <tr>
                                <td><asp:Label ID="lblCmd2" runat="server" Text="Команда:"></asp:Label></td>
                                <td><asp:Repeater ID="repCmdVal" runat="server" 
                                        onitemdatabound="repCmdVal_ItemDataBound" onitemcommand="repCmdVal_ItemCommand">
                                        <ItemTemplate><asp:Button ID="btnCmd" runat="server" CommandName="Exec" UseSubmitBehavior="False" /></ItemTemplate>
                                    </asp:Repeater></td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="viewStandCmd2" runat="server">
                        <table id="tblStandCmd2" cellpadding="0" cellspacing="0">
                            <tr>
                                <td><asp:Label ID="lblCmdVal" runat="server" Text="Значение"></asp:Label></td>
                                <td><asp:TextBox ID="txtCmdVal" runat="server" AutoCompleteType="Disabled"></asp:TextBox></td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="viewBinCmd" runat="server">
                        <table id="tblDataType" cellpadding="0" cellspacing="0">
                            <tr>
                                <td><asp:Label ID="lblDataType" runat="server" Text="Тип данных"></asp:Label></td>
                                <td><asp:RadioButton ID="rbStr" runat="server" GroupName="DataType" Text="Строка" Checked="True" /></td>
                                <td><asp:RadioButton ID="rbHex" runat="server" GroupName="DataType" Text="16-ричные данные" /></td>
                            </tr>
                        </table>
                        <table id="tblBinCmdData" cellpadding="0" cellspacing="0">
                            <tr>
                                <td><asp:Label ID="lblCmdData" runat="server" Text="Данные"></asp:Label></td>
                                <td><asp:TextBox ID="txtCmdData" runat="server" AutoCompleteType="Disabled" TextMode="MultiLine"></asp:TextBox></td>
                            </tr>
                        </table>
                    </asp:View>
                </asp:MultiView>
            </div>            
            <div id="divActions">
                <asp:Button 
                    ID="btnExecute" runat="server" CssClass="Button" Text="Выполнить" OnClick="btnExecute_Click" /><asp:Button 
                    ID="btnCancel" runat="server" CssClass="Button" Text="Отмена" OnClientClick="window.close(); return false;" />
            </div>
        </asp:View>
        <asp:View ID="viewResult" runat="server">
            <div id="divResult">
                <div><asp:Label ID="lblResultSuccessful" 
                    runat="server" Text="Команда поставлена в очередь на выполнение" ForeColor="Green"></asp:Label><asp:Label ID="lblResultFailed" 
                    runat="server" Text="Команда не выполнена" Visible="False" ForeColor="Red"></asp:Label></div>
                <div><asp:Button ID="btnClose" runat="server" Text="Закрыть" OnClientClick="window.close(); return false;" /></div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>

