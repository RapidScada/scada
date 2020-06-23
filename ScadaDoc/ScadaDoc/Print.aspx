<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Print.aspx.cs" Inherits="ScadaDoc.WFrmPrint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Print Version - Rapid SCADA Documentation</title>
    <link href="lib/prism/prism.css" rel="stylesheet" />
    <link href="css/print.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="lib/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="lib/prism/prism.js"></script>
    <script type="text/javascript">
        var articleUrl = "<%= articleUrl %>";
    </script>
    <script type="text/javascript" src="js/print.js"></script>
</head>
<body>
    <% GenerateDoc(); %>
</body>
</html>
