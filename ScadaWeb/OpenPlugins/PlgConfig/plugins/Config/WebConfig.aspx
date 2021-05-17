<%@ Page Title="Web Application Configuration" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="true" CodeBehind="WebConfig.aspx.cs" Inherits="Scada.Web.Plugins.Config.WFrmWebConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainHead" runat="server">
    <link href="css/webconfig.min.css" rel="stylesheet" type="text/css" />
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
    <h1><asp:Label ID="lblTitle" runat="server" Text="Web Application Configuration"></asp:Label></h1>
    <div class="form-horizontal">
        <h2><asp:Label ID="lblConnection" runat="server" Text="Connection to Server"></asp:Label></h2>
        <div class="form-group">
            <asp:Label ID="lblServerHost" runat="server" CssClass="col-sm-4 control-label" Text="Server" AssociatedControlID="txtServerHost"></asp:Label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtServerHost" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <asp:Panel ID="pnlServerPort" runat="server" CssClass="form-group">
            <asp:Label ID="lblServerPort" runat="server" CssClass="col-sm-4 control-label" Text="Port" AssociatedControlID="txtServerPort"></asp:Label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtServerPort" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlServerTimeout" runat="server" CssClass="form-group">
            <asp:Label ID="lblServerTimeout" runat="server" CssClass="col-sm-4 control-label" Text="Timeout" AssociatedControlID="txtServerTimeout"></asp:Label>
            <div class="col-sm-8">
                <div class="input-group">
                    <asp:TextBox ID="txtServerTimeout" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:Label ID="txtServerTimeoutUnit" runat="server" CssClass="input-group-addon" Text="ms"></asp:Label>
                </div>
            </div>
        </asp:Panel>
        <div class="form-group">
            <asp:Label ID="lblServerUser" runat="server" CssClass="col-sm-4 control-label" Text="User" AssociatedControlID="txtServerUser"></asp:Label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtServerUser" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="lblServerPwd" runat="server" CssClass="col-sm-4 control-label" Text="Password" AssociatedControlID="txtServerPwd"></asp:Label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtServerPwd" runat="server" CssClass="form-control" TextMode="Password" autocomplete="new-password"></asp:TextBox>
                <asp:Label ID="lblServerPwdHelp" runat="server" CssClass="help-block" Text="Leave blank to keep unchanged."></asp:Label>
            </div>
        </div>

        <h2><asp:Label ID="lblCommonParams" runat="server" Text="Application Parameters"></asp:Label></h2>
        <div class="form-group">
            <asp:Label ID="lblCulture" runat="server" CssClass="col-sm-4 control-label" Text="Culture" AssociatedControlID="txtCulture"></asp:Label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtCulture" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:Label ID="lblCultureHelp" runat="server" CssClass="help-block" Text="Example: en-GB. Empty is the default."></asp:Label>
            </div>
        </div>
        <asp:Panel ID="pnlDataRefrRate" runat="server" CssClass="form-group">
            <asp:Label ID="lblDataRefrRate" runat="server" CssClass="col-sm-4 control-label" Text="Data refresh rate" AssociatedControlID="txtDataRefrRate"></asp:Label>
            <div class="col-sm-8">
                <div class="input-group">
                    <asp:TextBox ID="txtDataRefrRate" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:Label ID="lbltxtDataRefrRateUnit" runat="server" CssClass="input-group-addon" Text="ms"></asp:Label>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlArcRefrRate" runat="server" CssClass="form-group">
            <asp:Label ID="lblArcRefrRate" runat="server" CssClass="col-sm-4 control-label" Text="Archive refresh rate" AssociatedControlID="txtArcRefrRate"></asp:Label>
            <div class="col-sm-8">
                <div class="input-group">
                    <asp:TextBox ID="txtArcRefrRate" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:Label ID="lblArcRefrRateUnit" runat="server" CssClass="input-group-addon" Text="ms"></asp:Label>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlDispEventCnt" runat="server" CssClass="form-group">
            <asp:Label ID="lblDispEventCnt" runat="server" CssClass="col-sm-4 control-label" Text="Display event count" AssociatedControlID="txtDispEventCnt"></asp:Label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtDispEventCnt" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlChartGap" runat="server" CssClass="form-group">
            <asp:Label ID="lblChartGap" runat="server" CssClass="col-sm-4 control-label" Text="Chart gap" AssociatedControlID="txtChartGap"></asp:Label>
            <div class="col-sm-8">
                <div class="input-group">
                    <asp:TextBox ID="txtChartGap" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:Label ID="lblChartGapUnit" runat="server" CssClass="input-group-addon" Text="sec"></asp:Label>
                </div>
            </div>
        </asp:Panel>
        <div class="form-group">
            <asp:Label ID="lblStartPage" runat="server" CssClass="col-sm-4 control-label" Text="Start page" AssociatedControlID="txtStartPage"></asp:Label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtStartPage" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:Label ID="lblStartPageHelp" runat="server" CssClass="help-block" Text="Example: ~/plugins/MyPlugin/MyPage.aspx"></asp:Label>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-4 col-sm-8">
                <div class="checkbox">
                    <label>
                        <asp:CheckBox ID="chkCmdEnabled" runat="server" />
                        <asp:Label ID="lblCmdEnabled" runat="server" Text="Enable commands"></asp:Label>
                    </label>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-4 col-sm-8">
                <div class="checkbox">
                    <label>
                        <asp:CheckBox ID="chkCmdPassword" runat="server" />
                        <asp:Label ID="lblCmdPassword" runat="server" Text="Require password to send command"></asp:Label>
                    </label>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-4 col-sm-8">
                <div class="checkbox">
                    <label>
                        <asp:CheckBox ID="chkRemEnabled" runat="server" />
                        <asp:Label ID="lblRemEnabled" runat="server" Text="Allow to remember logged on user"></asp:Label>
                    </label>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-4 col-sm-8">
                <div class="checkbox disabled">
                    <label>
                        <asp:CheckBox ID="chkViewsFromBase" runat="server" Enabled="False" />
                        <asp:Label ID="lblViewsFromBase" runat="server" Text="Load view settings from the database"></asp:Label>
                    </label>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-4 col-sm-8">
                <div class="checkbox">
                    <label>
                        <asp:CheckBox ID="chkShareStats" runat="server" />
                        <asp:Label ID="lblShareStats" runat="server" Text="Share depersonalized stats with the developers"></asp:Label>
                    </label>
                </div>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="lblChartScript" runat="server" CssClass="col-sm-4 control-label" Text="Chart plugin" AssociatedControlID="ddlChartScript"></asp:Label>
            <div class="col-sm-8">
                <asp:DropDownList ID="ddlChartScript" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="lblCmdScript" runat="server" CssClass="col-sm-4 control-label" Text="Command plugin" AssociatedControlID="ddlCmdScript"></asp:Label>
            <div class="col-sm-8">
                <asp:DropDownList ID="ddlCmdScript" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="lblEventAckScript" runat="server" CssClass="col-sm-4 control-label" Text="Acknowledgement plugin" AssociatedControlID="ddlEventAckScript"></asp:Label>
            <div class="col-sm-8">
                <asp:DropDownList ID="ddlEventAckScript" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="lblUserProfile" runat="server" CssClass="col-sm-4 control-label" Text="User profile plugin" AssociatedControlID="ddlUserProfile"></asp:Label>
            <div class="col-sm-8">
                <asp:DropDownList ID="ddlUserProfile" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-4 col-sm-8">
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save Configuration" OnClick="btnSave_Click" />
            </div>
        </div>
    </div>
</asp:Content>
