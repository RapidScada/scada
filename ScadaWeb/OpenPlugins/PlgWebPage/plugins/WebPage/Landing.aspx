<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Landing.aspx.cs" Inherits="Scada.Web.Plugins.WebPage.WFrmLanding" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Web Page</title>
    <link href="~/lib/open-sans/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="~/plugins/WebPage/css/webpage.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../lib/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../../js/api/utils.js"></script>
    <script type="text/javascript">
        <%= sbClientScript.ToString() %>

        function replaceLocalhost(url) {
            if (url) {
                var urlLo = url.toLowerCase();

                if (urlLo.startsWith("http://localhost") ||
                    urlLo.startsWith("https://localhost") &&
                    location.hostname.toLowerCase() !== "localhost") {

                    var urlObj = new URL(url);
                    urlObj.hostname = location.hostname;
                    return urlObj.href;
                }
            }

            return url;
        }

        $(document).ready(function () {
            const REDIRECT_DELAY = 500;
            var url = replaceLocalhost(viewPath);

            if (url) {
                $("body").addClass("loading");
                console.log(scada.utils.getCurTime() + " Redirect to " + url)
                setTimeout(function () { location.href = url }, REDIRECT_DELAY);
            } else {
                $("#lblEmptyUrl").css("display", "");
            }
        });
    </script>
</head>
<body>
    <div id="divMessage"><asp:Label ID="lblEmptyUrl" runat="server" Text="Unable to redirect because URL is empty" Style="display: none"></asp:Label></div>    
</body>
</html>
