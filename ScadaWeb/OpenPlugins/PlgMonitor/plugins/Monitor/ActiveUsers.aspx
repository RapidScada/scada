<%@ Page Title="Active Users - Rapid SCADA" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="true" CodeBehind="ActiveUsers.aspx.cs" Inherits="Scada.Web.Plugins.Monitor.WFrmActiveUsers" EnableViewState="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainHead" runat="server">
    <link href="../../css/common/contentform.min.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <h1>Active Users</h1>
    <asp:Repeater ID="repActiveUsers" runat="server">
        <HeaderTemplate>
            <table class="table table-striped">
                <tr>
                    <th>IP Address</th>
                    <th>Session ID</th>
                    <th>Username</th>
                    <th>Logon Time</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%# Eval("IpAddress") %></td>
                <td><%# HttpUtility.HtmlEncode(Eval("SessionID")) %></td>
                <td><%# HttpUtility.HtmlEncode(Eval("UserProps.UserName")) %></td>
                <td><%# Eval("LogonDT") %></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>
