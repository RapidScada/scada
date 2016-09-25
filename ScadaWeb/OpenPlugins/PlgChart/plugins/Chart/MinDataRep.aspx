<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="true" CodeBehind="MinDataRep.aspx.cs" Inherits="Scada.Web.Plugins.Chart.WFrmMinDataRep" %>
<%@ Import Namespace="Scada.Web.Plugins.Chart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainHead" runat="server">
    <link href="../../css/common/contentform.min.css" rel="stylesheet" type="text/css" />
    <link href="css/cnllist.min.css" rel="stylesheet" type="text/css" />
    <link href="css/mindatarep.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/mindatarep.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <h1><asp:Label ID="lblTitle" runat="server" Text="Minute Data Report"></asp:Label></h1>
    <div class="form-group period">
        <asp:Label ID="lblDateFrom" runat="server" Text="From" AssociatedControlID="txtDateFrom"></asp:Label>
        <div class="input-group">
            <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
            <span class="input-group-btn">
                <button class="btn btn-default calendar" type="button"><i class="fa fa-calendar"></i></button>
            </span>
        </div>
    </div>
    <div class="form-group period">
        <asp:Label ID="lblDateTo" runat="server" Text="To" AssociatedControlID="txtDateTo"></asp:Label>
        <div class="input-group">
            <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
            <span class="input-group-btn">
                <button class="btn btn-default calendar" type="button"><i class="fa fa-calendar"></i></button>
            </span>
        </div>
    </div>
    <div class="form-group">
        <asp:Button ID="btnAddCnls" runat="server" CssClass="btn btn-default" Text="Add Channels" OnClick="btnAddCnls_Click" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblSelCnls" runat="server" Text="Selected Channels:" AssociatedControlID="pnlSelCnls"></asp:Label>
        <asp:Panel ID="pnlSelCnls" runat="server" CssClass="cnl-list">
            <asp:Repeater ID="repSelCnls" runat="server" OnItemCommand="repSelCnls_ItemCommand">
                <ItemTemplate>
                    <div class="cnl-item">
                        <div class="cnl-field cnl-name">[<%# Eval("CnlNum") %>] <%# HttpUtility.HtmlEncode(Eval("CnlName")) %></div>
                        <div class="cnl-field cnl-btns"><asp:Button 
                            ID="btnRemoveCnl" runat="server" CssClass="btn btn-default btn-sm" 
                                CommandName="RemoveCnl" CommandArgument='<%# Container.ItemIndex %>' Text='<%# ChartPhrases.RemoveCnlBtn %>' /><a 
                            tabindex="0" role="button" class="btn btn-default btn-sm" data-toggle="popover" data-trigger="focus" data-placement="auto left" 
                            data-content="<%# Scada.Web.WebUtils.HtmlEncodeWithBreak(Eval("Info")) %>"><%# ChartPhrases.CnlInfoBtn %></a></div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </asp:Panel>
        <asp:Label ID="lblNoSelCnls" runat="server" CssClass="cnl-list-msg" Text="Input channels are not selected"></asp:Label>
    </div>
    <div class="form-group">
        <asp:Button ID="btnGenerate" runat="server" CssClass="btn btn-primary" Text="Download Report" />
    </div>
</asp:Content>
