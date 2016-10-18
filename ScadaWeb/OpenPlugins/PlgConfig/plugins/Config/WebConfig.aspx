<%@ Page Title="Web Application Configuration - Rapid SCADA" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="true" CodeBehind="WebConfig.aspx.cs" Inherits="Scada.Web.Plugins.Config.WFrmWebConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainHead" runat="server">
    <link href="css/webconfig.min.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <h1><asp:Label ID="lblTitle" runat="server" Text="Web Application Configuration"></asp:Label></h1>
    <div class="form-horizontal">
        <h2><asp:Label ID="lblConnection" runat="server" Text="Connection to Server"></asp:Label></h2>
        <div class="form-group">
            <asp:Label ID="lblServerHost" runat="server" CssClass="col-sm-4 control-label" Text="Server" AssociatedControlID="txtServerHost"></asp:Label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtServerHost" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="lblServerPort" runat="server" CssClass="col-sm-4 control-label" Text="Port" AssociatedControlID="txtServerPort"></asp:Label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtServerPort" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="lblServerTimeout" runat="server" CssClass="col-sm-4 control-label" Text="Timeout" AssociatedControlID="txtServerTimeout"></asp:Label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtServerTimeout" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="lblServerUser" runat="server" CssClass="col-sm-4 control-label" Text="User" AssociatedControlID="txtServerUser"></asp:Label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtServerUser" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="lblServerPwd" runat="server" CssClass="col-sm-4 control-label" Text="Password" AssociatedControlID="txtServerPwd"></asp:Label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtServerPwd" runat="server" CssClass="form-control"></asp:TextBox>
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
        <div class="form-group">
            <asp:Label ID="lblDataRefrRate" runat="server" CssClass="col-sm-4 control-label" Text="Data refresh rate" AssociatedControlID="txtDataRefrRate"></asp:Label>
            <div class="col-sm-8">
                <div class="input-group">
                    <asp:TextBox ID="txtDataRefrRate" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:Label ID="lbltxtDataRefrRateUnit" runat="server" CssClass="input-group-addon" Text="ms"></asp:Label>
                </div>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="lblArcRefrRate" runat="server" CssClass="col-sm-4 control-label" Text="Archive refresh rate" AssociatedControlID="txtArcRefrRate"></asp:Label>
            <div class="col-sm-8">
                <div class="input-group">
                    <asp:TextBox ID="txtArcRefrRate" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:Label ID="lblArcRefrRateUnit" runat="server" CssClass="input-group-addon" Text="ms"></asp:Label>
                </div>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="lblDispEventCnt" runat="server" CssClass="col-sm-4 control-label" Text="Display event count" AssociatedControlID="txtDispEventCnt"></asp:Label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtDispEventCnt" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="lblStartPage" runat="server" CssClass="col-sm-4 control-label" Text="Start page" AssociatedControlID="txtStartPage"></asp:Label>
            <div class="col-sm-8">
                <asp:TextBox ID="txtStartPage" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:Label ID="lblStartPageHelp" runat="server" CssClass="help-block" Text="Example: ~/plugins/MyPlugin/MyPage.aspx"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
