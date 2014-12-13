<%@ Page Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" Inherits="Scada.Web.WFrmReport" Title="SCADA - Отчёты" Codebehind="Report.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contHead" Runat="Server">
    <link href="css/report.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contMain" Runat="Server" EnableViewState="False">
    <h2><asp:Label ID="lblReportList" runat="server" Text="Доступные отчёты:"></asp:Label><asp:Label ID="lblNoReports" 
        runat="server" Text="Доступные отчёты отсутствуют" Visible="False"></asp:Label></h2>
    <asp:Table ID="tblReport" ClientIDMode="Static" runat="server">
    </asp:Table>
</asp:Content>