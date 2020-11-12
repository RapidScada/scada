<%@ Page Title="Reports" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="Scada.Web.WFrmReports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainHead" runat="server">
    <link href="css/reports.min.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <h1><asp:Label ID="lblTitle" runat="server" Text="Available Reports"></asp:Label></h1>
    <asp:Repeater ID="repReports" runat="server">
        <ItemTemplate>
            <div class="report-item">
                <asp:HyperLink ID="hlReportItem" runat="server" 
                    NavigateUrl='<%# string.IsNullOrEmpty((string)Eval("Url")) ? "" : VirtualPathUtility.ToAbsolute((string)Eval("Url")) %>'><%# (Container.ItemIndex + 1).ToString() + ". " + HttpUtility.HtmlEncode(Eval("Text")) %></asp:HyperLink>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Label ID="lblNoReports" runat="server" Text="No reports available"></asp:Label>
</asp:Content>
