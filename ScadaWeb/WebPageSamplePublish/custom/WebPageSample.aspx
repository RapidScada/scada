<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebPageSample.aspx.cs" Inherits="WebPageSample.WFrmWebPageSample" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Custom Web Page Example</title>
    <style type="text/css">
        body
        {
            padding: 0px;
            margin: 0px 0px 0px 20px;
            background-color: #F7F7E7;
	        font-family: Verdana, Arial;
	        font-size: 12px;
        }
        
        h1
        {
	        font-size: 13px;
	        margin: 0px 0px 5px;
        }
        
        a
        {
            text-decoration: none;
            color: cornflowerblue;
        }
        
        a:hover
        {
            text-decoration: underline;
        }

        table
        {
            border-collapse: collapse;
        }
 

        table tr
        {
            background-color: white;
        }
        
        table tr:first-child
        {
            background-color: #CCCCCC;
        }
        
        table td
        {
            text-align: center;
            padding: 5px;
            border: 1px solid black;
        }        
        
        table td:first-child
        {
            background-color: #CCCCCC;
        }
    </style>
    <script language="JavaScript" type="text/JavaScript" src="../script/scada.js"></script>
</head>
<body>
    <form id="PageSampleForm" runat="server">
        <h1>Server Room Temperature Control</h1>
        <asp:ScriptManager ID="ScriptManager" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlContent" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Timer ID="tmrRefresh" runat="server" Interval="5000">
                </asp:Timer>
                <table>
                    <tr>
                        <td></td>
                        <td>t4, °C</td>
                        <td>t5, °C</td>
                        <td>t avg, °C</td>
                    </tr>
                    <tr>
                        <td>Current</td>
                        <td><asp:HyperLink ID="hlT4" runat="server" CssClass="data cnl_241"></asp:HyperLink></td>
                        <td><asp:HyperLink ID="hlT5" runat="server" CssClass="data cnl_242"></asp:HyperLink></td>
                        <td><asp:HyperLink ID="hlTAvg" runat="server" CssClass="data cnl_243"></asp:HyperLink></td>
                    </tr>
                    <tr>
                        <td>Min.</td>
                        <td><asp:Label ID="lblT4Min" runat="server" CssClass="data cnl_246"></asp:Label>
                            <asp:HyperLink ID="hlT4MinReset" runat="server" CssClass="data ctrl_13">Reset</asp:HyperLink></td>
                        <td><asp:Label ID="lblT5Min" runat="server" CssClass="data cnl_247"></asp:Label>
                            <asp:HyperLink ID="hlT5MinReset" runat="server" CssClass="data ctrl_14">Reset</asp:HyperLink></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Max.</td>
                        <td><asp:Label ID="lblT4Max" runat="server" CssClass="data cnl_244"></asp:Label>
                            <asp:HyperLink ID="hlT4MaxReset" runat="server" CssClass="data ctrl_11">Reset</asp:HyperLink></td>
                        <td><asp:Label ID="lblT5Max" runat="server" CssClass="data cnl_245"></asp:Label>
                            <asp:HyperLink ID="hlT5MaxReset" runat="server" CssClass="data ctrl_12">Reset</asp:HyperLink></td>
                        <td></td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
