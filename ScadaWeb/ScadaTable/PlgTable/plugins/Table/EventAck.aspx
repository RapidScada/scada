<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EventAck.aspx.cs" Inherits="Scada.Web.Plugins.Table.WFrmEventAck" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <!-- The above 3 meta tags are required by Bootstrap -->
    <title>Event Acknowledgement</title>
    <link href="~/lib/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="~/lib/open-sans/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="~/plugins/Table/css/eventack.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../lib/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../js/api/utils.js"></script>
    <script type="text/javascript" src="../../js/api/eventtypes.js"></script>
    <script type="text/javascript" src="../../js/controls/popup.js"></script>
    <script type="text/javascript" src="js/eventack.js"></script>
</head>
<body>
    <form id="frmEventAck" runat="server">
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />

        <asp:Panel ID="pnlErrMsg" runat="server" CssClass="alert alert-danger">
            <asp:Label ID="lblEventNotFound" runat="server" Text="Event not found."></asp:Label><asp:Label 
                ID="lblAckNotSent" runat="server" Text="Unable to send the acknowledgement. Server is unavailable."></asp:Label><asp:Label 
                ID="lblAckRejected" runat="server" Text="The acknowledgement is rejected by the server."></asp:Label>
        </asp:Panel>

        <asp:Panel ID="pnlInfo" runat="server" CssClass="form-group" Visible="False">
            <table id="tblInfo">
                <tr>
                    <th><asp:Label ID="lblNumCaption" runat="server" Text="Number:"></asp:Label></th>
                    <td><asp:Label ID="lblNum" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lblTimeCaption" runat="server" Text="Date and time:"></asp:Label></th>
                    <td><asp:Label ID="lblTime" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lblObjCaption" runat="server" Text="Object:"></asp:Label></th>
                    <td><asp:Label ID="lblObj" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lblDevCaption" runat="server" Text="Device:"></asp:Label></th>
                    <td><asp:Label ID="lblDev" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lblCnlCaption" runat="server" Text="Channel:"></asp:Label></th>
                    <td><asp:Label ID="lblCnl" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lblTextCaption" runat="server" Text="Description:"></asp:Label></th>
                    <td><asp:Label ID="lblText" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lblAckCaption" runat="server" Text="Acknowledged:"></asp:Label></th>
                    <td class="ack"><asp:Label ID="lblAck" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblByUser" runat="server" Text="by {0}" Visible="False"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>

        <asp:Panel ID="pnlTip" runat="server" CssClass="alert alert-info">
            <asp:Label ID="lblTip" runat="server" Text="Click OK button to acknowledge the event."></asp:Label>
        </asp:Panel>
    </form>
</body>
</html>
