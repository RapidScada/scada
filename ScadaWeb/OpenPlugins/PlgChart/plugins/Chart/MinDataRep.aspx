<%@ Page Title="Minute Data Report" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="true" CodeBehind="MinDataRep.aspx.cs" Inherits="Scada.Web.Plugins.Chart.WFrmMinDataRep" %>
<%@ Import Namespace="Scada.Web.Plugins.Chart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainHead" runat="server">
    <link href="css/mindatarep.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/mindatarep.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <asp:ScriptManager ID="ScriptMan" runat="server"></asp:ScriptManager>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(asyncEndRequest);
    </script>
    <asp:UpdatePanel ID="upnlMessage" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Panel ID="pnlErrMsg" runat="server" CssClass="alert alert-danger alert-dismissible">
                <button type="button" class="close" data-dismiss="alert"><span>&times;</span></button>
                <asp:Label ID="lblErrMsg" runat="server" Text=""></asp:Label>
            </asp:Panel>
            <asp:Panel ID="pnlWarnMsg" runat="server" CssClass="alert alert-warning alert-dismissible">
                <button type="button" class="close" data-dismiss="alert"><span>&times;</span></button>
                <asp:Label ID="lblWarnMsg" runat="server" Text=""></asp:Label>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
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
    <asp:UpdatePanel ID="upnlCnls" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group">
                <asp:Button ID="btnAddCnls" runat="server" ClientIDMode="Static" CssClass="btn btn-default" UseSubmitBehavior="False" Text="Add Channels" OnClientClick="return false;" />
                <asp:Button ID="btnApplyAddedCnls" runat="server" ClientIDMode="Static" CssClass="hidden" UseSubmitBehavior="False" Text="Apply Added Cnls" OnClick="btnApplyAddedCnls_Click" />
                <asp:HiddenField ID="hidAddedCnlNums" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hidAddedViewIDs" runat="server" ClientIDMode="Static" />
            </div>
            <div class="form-group popovers-container">
                <asp:Label ID="lblSelCnls" runat="server" Text="Selected Channels:" AssociatedControlID="pnlSelCnls"></asp:Label>
                <asp:Panel ID="pnlSelCnls" runat="server" CssClass="cnl-list">
                    <asp:Repeater ID="repSelCnls" runat="server" OnItemCommand="repSelCnls_ItemCommand">
                        <ItemTemplate>
                            <div class="cnl-item">
                                <div class="cnl-field cnl-name">[<%# Eval("CnlNum") %>] <%# HttpUtility.HtmlEncode(Eval("CnlName")) %></div>
                                <div class="cnl-field cnl-btns"><asp:Button 
                                    ID="btnRemoveCnl" runat="server" CssClass="btn btn-default btn-sm" UseSubmitBehavior="False"
                                        CommandName="RemoveCnl" CommandArgument='<%# Container.ItemIndex %>' Text='<%# ChartPhrases.RemoveCnlBtn %>' /><a 
                                    tabindex="0" role="button" class="btn btn-default btn-sm" data-toggle="popover" data-trigger="focus" data-placement="auto right" 
                                    data-content="<%# Scada.Web.WebUtils.HtmlEncodeWithBreak(Eval("Info")) %>"><%# ChartPhrases.CnlInfoBtn %></a></div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </asp:Panel>
                <asp:Label ID="lblNoSelCnls" runat="server" CssClass="cnl-list-msg" Text="Input channels are not selected"></asp:Label>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnApplyAddedCnls" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upnlGenReport" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="form-group">
                <asp:Button ID="btnGenReport" runat="server" ClientIDMode="Static" CssClass="btn btn-primary" Text="Download Report" OnClick="btnGenReport_Click" />
                <asp:Label ID="lblGenStarted" runat="server" ClientIDMode="Static" CssClass="text-info hidden" Text="Generating the report. Please wait..."></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
