<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommNateOnBizControl.ascx.cs" Inherits="SKT.Glossary.Web.Common.Controls.CommNateOnBizControl" %>
<%@ Register Src="~/Common/Controls/UserAndDepartmentList.ascx" TagName="UserAndDepartment" TagPrefix="common" %>

<script type="text/javascript">
<!--

    var nateOnBizInitString = "내용을 작성해 주세요.";
    var nateOnBizChangeValue = "";
    function neteOnBizTargetShow() {
        $("#pop_NateOnBizTarget_add").css({ width: '600px' });
        $("#authorityNateOnBiz").fadeIn(200);
    }

    function neteOnBizTargetHide() {
        $("div.pop").hide();
        $("#authorityNateOnBiz").css("display", "none");
        $("#pop_NateOnBizTarget_add").css({ width: '275px' });

        
    }

    function neteOnBizTargetBtnShow(){
        $("#popNeteOnBizTargetwBtnP").css("display", "block");
    }

    function neteOnBizTargetBtnHide() {
        $("#popNeteOnBizTargetwBtnP").css("display", "none");
    }

    function nateOnBiztargetListSelect(type, id, name, getAjaxYn) {

        if (getAjaxYn == "Y") {
            fnSelectAuth(type, id, '<%=noteOnBizUserid%>');
        } else {
            var nateOnBizTarget;
            nateOnBizTarget = '[{ "ToUserID": "' + id + '", "ToUserName": "' + name + '", "ToUserType": "' + type + '" }]';
            DefaultSetting(nateOnBizTarget);
        }
    }

    function nateOnBizDefaultSetting(changeV) {
        $("#nateOnBizTextArea").val(changeV);
    }

    $(document).ready(function () {
        $("#nateOnBizTextArea").val(nateOnBizInitString);
        $("#nateOnBizTextArea").click(function () {
            if ($("#nateOnBizTextArea").val() == nateOnBizInitString) {
                $("#nateOnBizTextArea").val("");
            }
        })
    });


    /*
        Author : 개발자- 최현미C, 리뷰자-진현빈D
        CreateDae :  2016.05.18
        Desc : 이중클릭 방지       
    */
    var boolMailCheck = false;
    function nateOnBiztargetSend() {

        if (boolMailCheck == true)
            return;

        var Comment = {};
        
        if ($.trim($("#nateOnBizTextArea").val()) == '' || $("#nateOnBizTextArea").val() == nateOnBizInitString) {
        	alert("쪽지 내용을 입력해 주세요.");
            $("#nateOnBizTextArea").focus();
            $("#nateOnBizTextArea").val('');
        } else {

            boolMailCheck = true;
            $('#divImgLoading').show();
            $('#btnSend').removeClass("btn3");
            $('#btnSend').addClass("btn3_dis");

            fnShareSave();


            Comment.Contents = $('#nateOnBizTextArea').val().replace(/'/g, "`").replace(/"/g, "`");
            Comment.SendIds = getItemValue("ID");
            Comment.SendNMs = getItemValue("NM");
            Comment.SendTYs = getItemValue("TY");

            Comment.SendLinkNm = "";
            if (typeof (nateOnBiztargetLInkNm) == "function") {
                Comment.SendLinkNm = nateOnBiztargetLInkNm();
            }

            Comment.SendLinkLink = "";
            if (typeof (nateOnBiztargetLInkUrl) == "function") {
                Comment.SendLinkLink = nateOnBiztargetLInkUrl();
            }

            Comment.SendLinkType = "";
            if (typeof (nateOnBiztargetType) == "function") {
                Comment.SendLinkType = nateOnBiztargetType();
            }

            Comment.SendDirId = "";
            if (typeof (nateOnBiztargetDirName) == "function") {
                Comment.SendDirId = nateOnBiztargetDirName();
            }

            Comment.SendFileName = "";
            if (typeof (nateOnBiztargetFileName) == "function") {
                Comment.SendFileName = nateOnBiztargetFileName();
            }
            
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Common/Controls/AjaxControl.aspx/CommNateOnBizSend",
                data: "{contentText :'" + Comment.Contents + "', SendIds :'" + Comment.SendIds + "', SendNMs :'" + Comment.SendNMs + "', SendTYs :'" + Comment.SendTYs + "', SendLinkNm :'" + Comment.SendLinkNm + "', SendLinkLink :'" + Comment.SendLinkLink + "', SendLinkType :'" + Comment.SendLinkType + "', SendDirId :'" + Comment.SendDirId + "', SendFileName :'" + Comment.SendFileName + "'}",
                dataType: "json",
                success: function (data) {
                    
                    neteOnBizTargetHide();
                    //2016-01-13 P033028 쪽지 발송후 alert 메세지 추가
                    alert("발송 완료되었습니다.");
                    $("#pop_layer").hide();
                    ///////////////////////////////

                    boolMailCheck = false;
                    $('#divImgLoading').hide();
                    $('#btnSend').removeClass("btn3_dis");
                    $('#btnSend').addClass("btn3");

                },
                error: function (response, textStatus, errorThrown) {

                    boolMailCheck = false;
                    $('#divImgLoading').hide();
                    $('#btnSend').removeClass("btn3_dis");
                    $('#btnSend').addClass("btn3");

                    ///////////////////////////////
                    alert('쪽지발송 오류:' + response + ':' + textStatus + ':' + errorThrown);
                    neteOnBizTargetHide();
                    $("#pop_layer").hide();

                    return;
                }
            });
        }
    }
//-->
 </script>


<div id="addWrap">
	<fieldset class="schedule_add">
		<p><textarea style="height:210px" id="nateOnBizTextArea">내용을 작성해 주세요.</textarea></p>
        <%--<p<%if(targetBtnYn){%>  style="display:none"<%}%>  id="popNeteOnBizTargetwBtnP">
			<a href="javascript:" onClick="neteOnBizTargetShow()" id="popNeteOnBizTargetShowBtn" class="btn1"><span>쪽지수신인 설정하기</span></a>
			<a href="javascript:" onClick="neteOnBizTargetHide()" id="popNeteOnBizTargetHideBtn"class="btn1"><span>쪽지수신인 설정완료</span></a>
		</p>--%>
	</fieldset>
    <fieldset id="authorityNateOnBiz" class="authority" style="display:none">
		<common:UserAndDepartment ID="UserControlNateOnBizPop" runat="server" />
        <p></p>
    </fieldset>
</div>
    