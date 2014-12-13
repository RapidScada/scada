<%@ Page Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" Inherits="Scada.Web.WFrmLogin" Title="SCADA - Вход в систему" Codebehind="Login.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contHead" Runat="Server">
    <link href="css/login.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contMain" Runat="Server">
    <table id="tblLogin">
        <tr>
            <td></td>
            <td><h2><asp:Label ID="lblLoginTitle" 
                runat="server" Text="Вход в систему"></asp:Label><asp:Label ID="lblLoggedOnTitle" 
                runat="server" Text="Вход в систему выполнен" Visible="False"></asp:Label></h2></td>
        </tr>
        <tr>
            <td><asp:Label ID="lblName" runat="server" Text="Имя"></asp:Label></td>
            <td><asp:TextBox ID="txtLogin" runat="server" MaxLength="50"></asp:TextBox></td>
        </tr>
        <tr>
            <td><asp:Label ID="lblPassword" runat="server" Text="Пароль"></asp:Label></td>
            <td><asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="20"></asp:TextBox></td>
        </tr>
        <tr>
            <td><asp:Label ID="lblViewSet" runat="server" Text="Представления"></asp:Label></td>
            <td><asp:DropDownList ID="ddlViewSet" runat="server"></asp:DropDownList></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <table id="tblAction"><tr>
                    <td>
                        <asp:Button ID="btnEnter" runat="server" Text="Войти" onclick="btnEnter_Click"></asp:Button></td>
                    <td>
                        <asp:CheckBox ID="chkRememberUser" runat="server" Text="запомнить" /></td>
                    <td id="tdLast">
                        <asp:Button ID="btnExit" runat="server" Text="Выход" onclick="btnExit_Click" Visible="False"></asp:Button></td>
                </tr></table>                               
            </td>
        </tr>
    </table>
</asp:Content>