<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorReport.aspx.cs" Inherits="SKT.Glossary.Web.ErrorReport" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>문의/오류신고</title>
    <%--//<link href="Common/Css/tran_pop.css" rel="stylesheet" />--%>
    <link rel="stylesheet"  href="/common/css/default.css" />
    <link type="text/css"  href="Common/Css/sub.css" rel="stylesheet" />
    <style type="text/css">
        .layer_conts {text-align:center; padding: 20px 50px;}
        .layer_conts p {text-align:left;}
        .sub_title {
            color: #666;
            font-size: 14px;
            font-weight: bold;
            padding-bottom:10px;
        }
        .btn_c { margin-top:10px; text-align:center !important;}
        
    </style>
    <script src="/Common/js/jquery-1.11.1/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        //function fnGoTestBugAjax() {
        //    var senErrMsg = $("#testBugTextArea").val();
        //    if (senErrMsg == "") {
        //        alert("메시지를 입력해 주세요."); 
        //    } else {
        //        senErrMsg = "[T.끌 문의/오류신고]\n\n" + senErrMsg;
        //        $.ajax({
        //            type: "POST",
        //            contentType: "application/json; charset=utf-8",
        //            url: "/Common/Controls/AjaxControl.aspx/CommNateOnBizSend", 
        //            //data: "{contentText :'" + senErrMsg + "', SendIds :'1107160/1109387/1109271/P044795/P116224/P097010/', SendNMs :'관리자&관리자&관리자&관리자&관리자&관리자&', SendTYs :'U/U/U/U/U/U/', SendLinkNm :'', SendLinkLink :'', SendLinkType :'', SendDirId :'', SendFileName :''}",
        //            data: "{contentText :'" + senErrMsg + "', SendIds :'P097010/', SendNMs :'관리자&', SendTYs :'U/', SendLinkNm :'', SendLinkLink :'', SendLinkType :'', SendDirId :'', SendFileName :''}",
        //            dataType: "json",
        //            success: function (data) {

        //                alert("티끌 문의/오류신고가 접수되었습니다.\n빠르게 반영하도록 하겠습니다\n소중한 의견 감사합니다");
        //                window.close();

        //            },
        //            error: function (response, textStatus, errorThrown) {
        //                alert('쪽지발송 오류:' + response + ':' + textStatus + ':' + errorThrown);
        //                window.close();
        //                return;
        //            }
        //        });
        //    }
        //}

      function fn_CheckValdate() 
      {

           if ($("#txtContent").val() == "" )
           {
               $("#txtContent").focus();
               alert("내용을 입력해 주세요.");
               return;
           }

           if ($("#txtContent").val() == "내용을 입력해 주세요.") {
               $("#txtContent").focus();
               $("#txtContent").val("");
               alert("내용을 입력해 주세요.");
               return;
           }
           
          <%=Page.GetPostBackEventReference(btnSave) %>;
       }
        function EndMessage()
        {
            alert("T.끌 문의/오류신고가 접수되었습니다. \n빠르게 반영하도록 하겠습니다.\n소중한 의견 감사합니다.");
            self.close();
        }
       
    </script>
</head>
<body style="background-image:none; ">
    <form id="form2" runat="server">
    <div style="text-align:center; font-size:24px; font-weight:100; margin-top:10px;">
        <b>문의/오류신고</b>
    </div>
    <!-- // 2017-02-05 / 최현미 / 사내 시스템 Footer 일관성 조치 -->
    <div class="layer_conts">
        <p class="sub_title"><span style="font-size:16px; font-weight:bold;">ㆍ</span>담당자 : <asp:Label ID="lblChargeName" runat="server" Text=""></asp:Label>
        </p>
            <div id="addWrap2" >
	            <fieldset class="note">
			            <p><textarea  id="txtContent" style="width:290px;height:210px;" runat="server" onclick="if(this.value=='내용을 입력해 주세요.'){this.value=''};">내용을 입력해 주세요.</textarea></p>
	            </fieldset>
	        </div>
            <p class="btn_c">
		        <a href="javascript:window.close();" class="btn2" style="cursor:pointer;"><b>취소하기</b></a>
		        <a href="javascript:fn_CheckValdate();" class="btn3" style="cursor:pointer;"><b>보내기</b></a>
	        </p>
     </div>
        <div style ="display:none;">
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click"  />
       <%-- <input type="hidden" value="2pac81/kyeong0503/skt.P097010/" id="hidUserList"  runat="server"/>--%>
        </div>
    </form>
</body>
</html>
