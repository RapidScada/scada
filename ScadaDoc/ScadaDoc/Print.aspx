<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Print.aspx.cs" Inherits="ScadaDoc.WFrmPrint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Print Version - Rapid SCADA Documentation</title>
    <link href="css/print.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/jquery.min.js"></script>
    <script type="text/javascript" src="js/print.js"></script>
</head>
<body>
    <% GenerateDoc(); %>
</body>
</html>
