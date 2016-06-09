<%@ Page Title="Cache State - Rapid SCADA" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="true" CodeBehind="CacheState.aspx.cs" Inherits="Scada.Web.Plugins.Config.WFrmCacheState" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <h2>Hour Table Cache</h2>
    <asp:Label ID="lblHourTableCacheInfo" runat="server" Text=""></asp:Label>
    <asp:Repeater ID="repHourTableCache" runat="server">
        <ItemTemplate>
            <div>
                ValueAge: <%#: Eval("ValueAge") %>,
                ValueRefrDT (UTC): <%#: Eval("ValueRefrDT") %>,
                AccessDT (UTC): <%#: Eval("AccessDT") %>,
                SrezTableLight: <%#: Eval("Value.TableName") %>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <h2>View Cache</h2>
    <asp:Label ID="lblViewCacheInfo" runat="server" Text=""></asp:Label>
    <asp:Repeater ID="repViewCache" runat="server">
        <ItemTemplate>
            <div>
                ValueAge: <%#: Eval("ValueAge") %>,
                ValueRefrDT (UTC): <%#: Eval("ValueRefrDT") %>,
                AccessDT (UTC): <%#: Eval("AccessDT") %>,
                BaseView: <%#: Eval("Value.ItfObjName") %>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
