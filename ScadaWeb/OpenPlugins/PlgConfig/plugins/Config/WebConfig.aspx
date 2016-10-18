<%@ Page Title="Web Application Configuration - Rapid SCADA" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="true" CodeBehind="WebConfig.aspx.cs" Inherits="Scada.Web.Plugins.Config.WFrmWebConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainHead" runat="server">
    <link href="css/webconfig.min.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <h1><asp:Label ID="lblTitle" runat="server" Text="Web Application Configuration"></asp:Label></h1>
    <div class="form-horizontal">
        <h2><asp:Label ID="lblConnection" runat="server" Text="Connection to Server"></asp:Label></h2>
        <div class="form-group">
            <asp:Label ID="lblServerHost" runat="server" CssClass="col-sm-2 control-label" Text="Server" AssociatedControlID="txtServerHost"></asp:Label>
            <div class="col-sm-10">
                <asp:TextBox ID="txtServerHost" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="lblServerPort" runat="server" CssClass="col-sm-2 control-label" Text="Port" AssociatedControlID="txtServerPort"></asp:Label>
            <div class="col-sm-10">
                <asp:TextBox ID="txtServerPort" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="lblServerTimeout" runat="server" CssClass="col-sm-2 control-label" Text="Timeout" AssociatedControlID="txtServerTimeout"></asp:Label>
            <div class="col-sm-10">
                <asp:TextBox ID="txtServerTimeout" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="lblServerUser" runat="server" CssClass="col-sm-2 control-label" Text="User" AssociatedControlID="txtServerUser"></asp:Label>
            <div class="col-sm-10">
                <asp:TextBox ID="txtServerUser" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="lblServerPwd" runat="server" CssClass="col-sm-2 control-label" Text="Password" AssociatedControlID="txtServerPwd"></asp:Label>
            <div class="col-sm-10">
                <asp:TextBox ID="txtServerPwd" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
</asp:Content>
