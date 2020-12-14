<%@ Page Title="View" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Scada.Web.WFrmView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainHead" runat="server">
    <link href="css/view.min.css" rel="stylesheet" type="text/css" />
    <link href="css/controls/splitter.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/api/viewhub.js"></script>
    <script type="text/javascript" src="js/controls/splitter.js"></script>
    <script type="text/javascript">
        var initialViewID = <%= initialViewID %>;
        var initialViewUrl = "<%= initialViewUrl %>";
    </script>
    <script type="text/javascript" src="js/view.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <div id="divViewContent">
        <div id="divView"><iframe id="frameView"></iframe></div>
        <div id="divViewSplitter" class="splitter hor"></div>
        <div id="divDataWindow"><iframe id="frameDataWindow"></iframe></div>
        <div id="divBottomTabs">
            <div id="divBottomTabsContainer"><%= GenerateBottomTabsHtml() %></div><div id="divCollapseDataWindowBtn"><i class="fa fa-chevron-circle-down"></i></div>
        </div>
    </div>
</asp:Content>
