<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GlossaryPrint_View.aspx.cs" Inherits="SKT.Glossary.Web.Glossary.GlossaryPrint_View" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
 <title>
</title><link href="/NamoEditor/css/namose_otherEditor.css" rel="stylesheet" />
<script type="text/javascript" lang="ko">

    var initBody;
    function beforePrint() {
        boxes = document.body.innerHTML;
        document.body.innerHTML = printView.innerHTML;
    }
    function afterPrint() {
        document.body.innerHTML = boxes;
    }
    function printArea() {
        window.print();
    }
    window.onbeforeprint = beforePrint;
    window.onafterprint = afterPrint;
</script>
<style >
 
 h1 {padding:0 10px 7px; font-size:13pt;}
 .info {border-top:1px solid #ddd; border-bottom:1px solid #ddd; padding:10px; margin-bottom:20px; font-size:10pt; color:#666;}
 .viewWrap {padding:10px;}
 .viewNum {padding:3px 0 0;}
 .viewNum > span {margin-right:15px;}
 .btnWrap {padding-top:10px; text-align:center;}
 .btnWrap a {display:inline-block; height:34px; padding:0 25px; background-image: linear-gradient(to bottom, #fff, #f5f5f5); border:1px solid #b8b8b8; border-radius:5px; margin:0 0px; color:#333; text-decoration:none; line-height:32px; font-weight:bold; box-shadow: 0 2px 2px #e5e5e5; font-size:11pt}
 .btnWrap a.dark {border-color:#4f4f4f; background-image: linear-gradient(to bottom, #6b6b6b, #4f4f4f); color:#fff;}
</style>
</head>
<body>
    <div id="printView" style="height:650px; overflow-y:auto;">
        <h1> <%=Title %></h1>
  <div class="info">
   <b><asp:Literal ID="litFirstUser" runat="server"></asp:Literal>, <asp:Literal ID="litLastUser" runat="server"></asp:Literal><asp:Literal ID="litFromQna" runat="server"></asp:Literal>
  <%-- <div class="viewNum"><span>조회 47</span><span>추천 0</span><span>스크랩수 0</span><span>편집횟수 1</span></div>--%>
  </div>
  <div class="viewWrap">
   <%=ItemHtml %>
  </div>
    </div>
    <div class="btnWrap">
        <a href="javascript:self.close();">취소</a>
  <a href="javascript:printArea();" class="dark">인쇄</a>
 </div> 
</body>
</html>

 

