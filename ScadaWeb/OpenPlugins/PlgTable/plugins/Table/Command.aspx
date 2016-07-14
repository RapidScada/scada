<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Command.aspx.cs" Inherits="Scada.Web.Plugins.Table.WFrmCommand" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <!-- The above 3 meta tags are required by Bootstrap -->
    <title>Command - Rapid SCADA</title>
    <link href="~/lib/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="~/lib/open-sans/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="~/plugins/Table/css/command.min.css" rel="stylesheet" type="text/css" />
    <!--<script type="text/javascript" src="../../lib/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../lib/bootstrap/js/bootstrap.min.js"></script>-->
</head>
<body>
    <form id="frmCommand" runat="server">
        <asp:HiddenField ID="hidCmdEnabled" runat="server" Value="false" />
        <div>
            <table id="tblInfo">
                <tr>
                    <th><asp:Label ID="lblCtrlCnlCaption" runat="server" Text="Out channel:"></asp:Label></th>
                    <td><asp:Label ID="lblCtrlCnl" runat="server" Text=""></asp:Label><asp:Label 
                        ID="lblCtrlCnlNotFound" runat="server" Text="Not found" Visible="False"></asp:Label></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lblObjCaption" runat="server" Text="Object:"></asp:Label></th>
                    <td><asp:Label ID="lblObj" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lblDevCaption" runat="server" Text="Device:"></asp:Label></th>
                    <td><asp:Label ID="lblDev" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>
        </div>
        <asp:Panel ID="pnlPassword" runat="server" Visible="False">
            <div><asp:Label ID="lblPassword" runat="server" Text="Password" AssociatedControlID="txtPassword"></asp:Label></div>
            <div><asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox></div>
        </asp:Panel>
        <asp:Panel ID="pnlRealValue" runat="server" Visible="False">
            <div><asp:Label ID="lblCmdVal" runat="server" Text="Value" AssociatedControlID="txtCmdVal"></asp:Label></div>
            <div><asp:TextBox ID="txtCmdVal" runat="server" CssClass="form-control"></asp:TextBox></div>
        </asp:Panel>
        <asp:Panel ID="pnlDiscreteValue" runat="server" Visible="False">
            <div><asp:Label ID="lblCommand" runat="server" CssClass="cmd-lbl" Text="Command:"></asp:Label></div>
            <div id="divCommands">
                <asp:Repeater ID="repCommands" runat="server">
                    <ItemTemplate><asp:Button ID="btnCmd" runat="server" 
                        CssClass="btn btn-default" Text='<%#: Eval("Text") %>' data-cmdval='<%# Eval("Val") %>' /></ItemTemplate>
                </asp:Repeater>
            </div>
        </asp:Panel>
    </form>
</body>
</html>
