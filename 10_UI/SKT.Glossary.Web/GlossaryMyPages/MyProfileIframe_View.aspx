<%--/*
 * Mostisoft
 * */--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyProfileIframe_View.aspx.cs" Inherits="SKT.Glossary.Web.GlossaryMyPages.MyProfileIframe_View" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script src="../Common/Js/jquery-1.11.1/jquery-1.11.1.min.js"></script>
    <link href="../Common/Js/jquery-1.11.1/jquery-ui.min.css" rel="stylesheet" />
    <script src="../Common/Js/jquery-1.11.1/jquery-ui.min.js"></script>

    <title></title>

    <script type="text/javascript">

        // View Binding 시 iframe Height 조절
        $(document).ready(function () {
            var currentHeight = (document.body.scrollHeight) + 17;
            parent.StandaloneView(currentHeight);   // MyProfile.aspx
        });

    </script>

    <link href="/NamoEditor/css/namose_otherEditor.css" rel="stylesheet" />

</head>
<body contentEditable="false">
    <%=ItemHtml %>
</body>
</html>
