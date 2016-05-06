<%@ Page Title="Views - Rapid SCADA" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Scada.Web.WFrmView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainHead" runat="server">
    <link href="css/view.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/view.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <iframe id="frameView"></iframe>
</asp:Content>
