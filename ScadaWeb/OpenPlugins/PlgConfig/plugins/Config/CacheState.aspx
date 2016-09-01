<%@ Page Title="Cache State - Rapid SCADA" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="true" CodeBehind="CacheState.aspx.cs" Inherits="Scada.Web.Plugins.Config.WFrmCacheState" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainHead" runat="server">
    <link href="../../css/contentform.min.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <h1>Hour Table Cache</h1>
    <asp:Label ID="lblHourTableCacheInfo" runat="server" Text=""></asp:Label>
    <asp:Repeater ID="repHourTableCache" runat="server">
        <ItemTemplate>
            <div>
                ValueAge: <%# Eval("ValueAge") %>,
                ValueRefrDT (UTC): <%# Eval("ValueRefrDT") %>,
                AccessDT (UTC): <%# Eval("AccessDT") %>,
                SrezTableLight: <%# HttpUtility.HtmlEncode(Eval("Value.TableName")) %>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <h1>View Cache</h1>
    <asp:Label ID="lblViewCacheInfo" runat="server" Text=""></asp:Label>
    <asp:Repeater ID="repViewCache" runat="server">
        <ItemTemplate>
            <div>
                ValueAge: <%# Eval("ValueAge") %>,
                ValueRefrDT (UTC): <%# Eval("ValueRefrDT") %>,
                AccessDT (UTC): <%# Eval("AccessDT") %>,
                BaseView: <%# HttpUtility.HtmlEncode(Eval("Value.ItfObjName")) %>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
