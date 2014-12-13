<%@ Page Language="C#" AutoEventWireup="true" Inherits="Scada.Web.WFrmError" Codebehind="Error.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SCADA - Ошибка приложения</title>
    <style type="text/css">
        body
        {
            margin: 5px;
            background-color: white;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 12px;
            color: red;
        }
    </style>
</head>
<body>
    <form id="ErrorForm" runat="server">
        <asp:Label ID="lblMessage" runat="server" Text="Ошибка при работе приложения"></asp:Label>
    </form>
</body>
</html>