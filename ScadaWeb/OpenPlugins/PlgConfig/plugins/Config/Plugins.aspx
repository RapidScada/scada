<%@ Page Title="Plugins - Rapid SCADA" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="true" CodeBehind="Plugins.aspx.cs" Inherits="Scada.Web.Plugins.Config.WFrmPlugins" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainHead" runat="server">
    <link href="css/plugins.min.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <h1><asp:Label ID="lblTitle" runat="server" Text="Plugins"></asp:Label></h1>
    <asp:Repeater ID="repPlugins" runat="server" OnItemCommand="repPlugins_ItemCommand" OnItemDataBound="repPlugins_ItemDataBound">
        <HeaderTemplate>
            <table class="table table-striped">
                <tr>
                    <th>Name</th>
                    <th>Description</th>
                    <th>State</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td class="name-cell"><span><%# HttpUtility.HtmlEncode(Eval("Name")) %></span><br/><asp:Button 
                    ID="btnActivate" runat="server" CssClass="btn btn-link" Text="Activate" UseSubmitBehavior="False" 
                        CommandName="Activate" CommandArgument='<%# Eval("FileName") %>' /><asp:Button 
                    ID="btnDeactivate" runat="server" CssClass="btn btn-link" Text="Deactivate" UseSubmitBehavior="False" 
                        CommandName="Deactivate" CommandArgument='<%# Eval("FileName") %>' /></td>
                <td><%# HttpUtility.HtmlEncode(Eval("Descr")) %></td>
                <td><%# HttpUtility.HtmlEncode(Eval("State")) %></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>
