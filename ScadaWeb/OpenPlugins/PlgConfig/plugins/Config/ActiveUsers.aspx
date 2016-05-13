<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="true" CodeBehind="ActiveUsers.aspx.cs" Inherits="Scada.Web.Plugins.Config.WFrmActiveUsers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <asp:Repeater ID="repActiveUsers" runat="server">
        <ItemTemplate>
            <div>
                IpAddress: <%#: Eval("IpAddress") %>,
                SessionID: <%#: Eval("SessionID") %>,
                UserName: <%#: Eval("UserName") %>,
                LogonDT: <%#: Eval("LogonDT") %>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
