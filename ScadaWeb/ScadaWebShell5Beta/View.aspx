<%@ Page Title="Views - Rapid SCADA" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Scada.Web.WFrmView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainHead" runat="server">
    <link href="css/view.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/view.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <div id="divView">
        <iframe id="frameView"></iframe>
        <div id="divBottomTabs">
            <div id="divEventsTab" class="tab"><asp:Label ID="lblEventsTab" runat="server" Text="Events"></asp:Label></div>
        </div>
    </div>
</asp:Content>
