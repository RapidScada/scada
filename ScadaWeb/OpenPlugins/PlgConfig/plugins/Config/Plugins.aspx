<%@ Page Title="Installed Plugins" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="true" CodeBehind="Plugins.aspx.cs" Inherits="Scada.Web.Plugins.Config.WFrmPlugins" %>
<%@ Import Namespace="Scada.Web" %>
<%@ Import Namespace="Scada.Web.Plugins.Config" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainHead" runat="server">
    <link href="css/plugins.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/plugins.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <asp:Panel ID="pnlErrMsg" runat="server" CssClass="alert alert-danger alert-dismissible">
        <button type="button" class="close" data-dismiss="alert"><span>&times;</span></button>
        <asp:Label ID="lblErrMsg" runat="server" Text=""></asp:Label>
    </asp:Panel>
    <asp:Panel ID="pnlSuccMsg" runat="server" CssClass="alert alert-success alert-dismissible">
        <button type="button" class="close" data-dismiss="alert"><span>&times;</span></button>
        <asp:Label ID="lblSuccMsg" runat="server" Text=""></asp:Label>
    </asp:Panel>
    <h1><asp:Label ID="lblTitle" runat="server" Text="Installed Plugins"></asp:Label></h1>
    <asp:Repeater ID="repPlugins" runat="server" OnItemCommand="repPlugins_ItemCommand" OnItemDataBound="repPlugins_ItemDataBound">
        <HeaderTemplate>
            <table class="table table-striped">
                <tr>
                    <th><%= PlgPhrases.NameCol %></th>
                    <th><%= PlgPhrases.DescrCol %></th>
                    <th><%= PlgPhrases.StateCol %></th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class='state-<%# Eval("State").ToString().ToLowerInvariant() %>'>
                <td class="name-cell"><span><%# HttpUtility.HtmlEncode(Eval("Name")) %></span><br/><asp:LinkButton 
                    ID="lbtnActivate" runat="server" CssClass="btn-confirm" CommandName="Activate" CommandArgument='<%# Eval("FileName") %>'><%= PlgPhrases.ActivateBtn %></asp:LinkButton><asp:LinkButton 
                    ID="lbtnDeactivate" runat="server" CssClass="btn-confirm" CommandName="Deactivate" CommandArgument='<%# Eval("FileName") %>'><%= PlgPhrases.DeactivateBtn %></asp:LinkButton>
                <td><%# WebUtils.HtmlEncodeWithBreak(Eval("FullDescr")) %></td>
                <td class="state-cell"><%# StateToStr((PlaginStates)Eval("State")) %></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>
