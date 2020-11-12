<%@ Page Title="User Profile" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="Scada.Web.WFrmUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainHead" runat="server">
    <link href="css/user.min.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <h1><asp:Label ID="lblTitle" runat="server" Text="User Profile"></asp:Label></h1>
    <table class="props-table">
        <tr>
            <th><asp:Label ID="lblUserNameCaption" runat="server" Text="Username:"></asp:Label></th>
            <td><asp:Label ID="lblUserName" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <th><asp:Label ID="lblRoleNameCaption" runat="server" Text="Role:"></asp:Label></th>
            <td><asp:Label ID="lblRoleName" runat="server" Text=""></asp:Label></td>
        </tr>
    </table>
    <h2><asp:Label ID="lblGeneralRightsTitle" runat="server" Text="General Rights"></asp:Label></h2>
    <table class="props-table">
        <tr>
            <th><asp:Label ID="lblViewAllRightCaption" runat="server" Text="View all data:"></asp:Label></th>
            <td><asp:Label ID="lblViewAllRight" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <th><asp:Label ID="lblControlAllRightCaption" runat="server" Text="Control all devices:"></asp:Label></th>
            <td><asp:Label ID="lblControlAllRight" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <th><asp:Label ID="lblConfigRightCaption" runat="server" Text="System configuration:"></asp:Label></th>
            <td><asp:Label ID="lblConfigRight" runat="server" Text=""></asp:Label></td>
        </tr>
    </table>
</asp:Content>
