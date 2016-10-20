<%@ Page Title="Plugins - Rapid SCADA" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="true" CodeBehind="Plugins.aspx.cs" Inherits="Scada.Web.Plugins.Config.WFrmPlugins" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <h1><asp:Label ID="lblTitle" runat="server" Text="Plugins"></asp:Label></h1>
    <asp:Repeater ID="repPlugins" runat="server">
        <ItemTemplate>

        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
