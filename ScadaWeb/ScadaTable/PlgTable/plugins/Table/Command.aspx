<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Command.aspx.cs" Inherits="Scada.Web.Plugins.Table.WFrmCommand" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <!-- The above 3 meta tags are required by Bootstrap -->
    <title>Command</title>
    <link href="~/lib/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="~/lib/open-sans/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="~/plugins/Table/css/command.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../lib/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../js/api/utils.js"></script>
    <script type="text/javascript" src="../../js/api/eventtypes.js"></script>
    <script type="text/javascript" src="../../js/controls/popup.js"></script>
    <script type="text/javascript" src="js/command.js"></script>
</head>
<body>
    <form id="frmCommand" runat="server">
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
        <asp:MultiView ID="mvCommand" runat="server" ActiveViewIndex="0">
            <asp:View ID="viewCmdParams" runat="server">
                <asp:Panel ID="pnlErrMsg" runat="server" CssClass="alert alert-danger">
                    <asp:Label ID="lblCtrlCnlNotFound" runat="server" Text="Output channel {0} not found."></asp:Label><asp:Label 
                        ID="lblWrongPwd" runat="server" Text="Incorrect password."></asp:Label><asp:Label 
                        ID="lblNoRights" runat="server" Text="Insufficient rights."></asp:Label><asp:Label 
                        ID="lblIncorrectCmdVal" runat="server" Text="Incorrect command value."></asp:Label><asp:Label 
                        ID="lblIncorrectCmdData" runat="server" Text="Incorrect command data."></asp:Label>
                </asp:Panel>

                <asp:Panel ID="pnlInfo" runat="server" CssClass="form-group" Visible="False">
                    <table id="tblInfo">
                        <tr>
                            <th><asp:Label ID="lblCtrlCnlCaption" runat="server" Text="Out channel:"></asp:Label></th>
                            <td><asp:Label ID="lblCtrlCnl" runat="server" Text=""></asp:Label></td>
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
                </asp:Panel>

                <asp:Panel ID="pnlPassword" runat="server" CssClass="form-group has-feedback" Visible="False">
                    <asp:Label ID="lblPassword" runat="server" CssClass="control-label" Text="Password" AssociatedControlID="txtPassword"></asp:Label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                    <span class="glyphicon glyphicon-exclamation-sign form-control-feedback"></span>
                </asp:Panel>

                <asp:Panel ID="pnlRealValue" runat="server" CssClass="form-group has-feedback" Visible="False">
                    <asp:Label ID="lblCmdVal" runat="server" CssClass="control-label" Text="Value" AssociatedControlID="txtCmdVal"></asp:Label>
                    <asp:TextBox ID="txtCmdVal" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    <span class="glyphicon glyphicon-exclamation-sign form-control-feedback"></span>
                </asp:Panel>

                <asp:Panel ID="pnlDiscreteValue" runat="server" CssClass="form-group" Visible="False">
                    <div><asp:Label ID="lblCommand" runat="server" CssClass="control-label" Text="Command" AssociatedControlID="repCommands"></asp:Label></div>
                    <div id="divCommands">
                        <asp:Repeater ID="repCommands" runat="server" OnItemCommand="repCommands_ItemCommand">
                            <ItemTemplate><asp:Button ID="btnCmd" runat="server" 
                                CssClass="btn btn-danger" UseSubmitBehavior="False" Text='<%# HttpUtility.HtmlEncode(Eval("Text")) %>' data-cmdval='<%# Eval("Val") %>' /></ItemTemplate>
                        </asp:Repeater>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlData" runat="server" CssClass="form-group" Visible="False">
                    <asp:Label ID="lblCmdData" runat="server" CssClass="control-label" Text="Data" AssociatedControlID="txtCmdData"></asp:Label>
                    <div id="divCmdDataFormat">
                        <label class="radio-inline">
                            <asp:RadioButton ID="rbStr" runat="server" Checked="True" GroupName="CmdDataFormat" /><asp:Label ID="lblStr" runat="server" Text="String"></asp:Label>
                        </label>
                        <label class="radio-inline">
                            <asp:RadioButton ID="rbHex" runat="server" GroupName="CmdDataFormat" /><asp:Label ID="lblHex" runat="server" Text="Hexadecimal"></asp:Label>
                        </label>
                    </div>
                    <asp:TextBox ID="txtCmdData" runat="server" CssClass="form-control" Rows="3" TextMode="MultiLine"></asp:TextBox>
                </asp:Panel>

                <div class="alert alert-danger invisible">Vertical Spacer Begin</div>
                <div class="invisible" style="height:0">Vertical Spacer End</div>
            </asp:View>

            <asp:View ID="viewCmdResult" runat="server">
                <asp:Panel ID="pnlSuccess" runat="server" CssClass="alert alert-success" Visible="False">
                    <asp:Label ID="lblCmdEnqueded" runat="server" Text="The command is enqueued for execution."></asp:Label>
                    <asp:Label ID="lblCloseAfter" runat="server" Text="The window will be closed after"></asp:Label>
                    <span id="spanCountdown"></span>
                    <asp:Label ID="lblSec" runat="server" Text="sec."></asp:Label>
                </asp:Panel>
                <asp:Panel ID="pnlFail" runat="server" CssClass="alert alert-danger" Visible="False">
                    <asp:Label 
                        ID="lblCmdNotSent" runat="server" Text="Unable to send the command. Server is unavailable." Visible="False"></asp:Label><asp:Label 
                        ID="lblCmdRejected" runat="server" Text="The command is rejected by the server." Visible="False"></asp:Label>
                </asp:Panel>
            </asp:View>
        </asp:MultiView>
    </form>
</body>
</html>
