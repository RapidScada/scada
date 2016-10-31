<%@ Page Title="Cache State - Rapid SCADA" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="true" CodeBehind="CacheState.aspx.cs" Inherits="Scada.Web.Plugins.Monitor.WFrmCacheState" EnableViewState="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainHead" runat="server">
    <link href="../../css/common/contentform.min.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <h1>Hour Table Cache</h1>
    <p><asp:Label ID="lblHourTableCacheInfo" runat="server" Text=""></asp:Label></p>
    <asp:Repeater ID="repHourTableCache" runat="server">
        <HeaderTemplate>
            <table class="table table-striped table-condensed">
                <tr>
                    <th>Key</th>
                    <th>Value Age</th>
                    <th>Value Refresh Time (UTC)</th>
                    <th>Access Time (UTC)</th>
                    <th>Snapshot Table</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%# HttpUtility.HtmlEncode(Eval("Key", "{0:d}")) %></td>
                <td><%# Eval("ValueAge") %></td>
                <td><%# Eval("ValueRefrDT") %></td>
                <td><%# Eval("AccessDT") %></td>
                <td><%# HttpUtility.HtmlEncode(Eval("Value.TableName")) %></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <h1>View Cache</h1>
    <p><asp:Label ID="lblViewCacheInfo" runat="server" Text=""></asp:Label></p>
    <asp:Repeater ID="repViewCache" runat="server">
        <HeaderTemplate>
            <table class="table table-striped table-condensed">
                <tr>
                    <th>Key</th>
                    <th>Value Age</th>
                    <th>Value Refresh Time (UTC)</th>
                    <th>Access Time (UTC)</th>
                    <th>View</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%# HttpUtility.HtmlEncode(Eval("Key")) %></td>
                <td><%# Eval("ValueAge") %></td>
                <td><%# Eval("ValueRefrDT") %></td>
                <td><%# Eval("AccessDT") %></td>
                <td><%# HttpUtility.HtmlEncode(Eval("Value.Title")) %></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>
