<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GatheringManagerIframe.aspx.cs" Inherits="SKT.Glossary.Web.Gathering.GatheringManagerIframe" %>
<%@ Register Src="~/Common/Controls/UserAndDepartmentList.ascx" TagName="UserAndDepartment" TagPrefix="common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/DTD/loose.dtd">
<html lang="ko">
	<head>
		<meta http-equiv="X-UA-Compatible" content="IE=edge" />
		<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
		<link rel="shortcut icon" href="/favi.ico" />
		<link rel="stylesheet" type="text/css"  href="/common/css/default.css" />
		<link rel="stylesheet" type="text/css"  href="/common/css/view.css" />
		<link rel="stylesheet" type="text/css"  href="/common/css/sub.css" />   
		<link rel="stylesheet" type="text/css"  href="/common/css/jquery-ui.css" />   
		<!--[if lt IE 9]>
		<script src="/common/js/html5shiv.js"></script>
		<![endif]-->

		<script src="/Common/js/jquery-1.11.1/jquery-1.11.1.min.js" type="text/javascript"></script>
		<link href="/Common/js/jquery-1.11.1/jquery-ui.css" rel="stylesheet" type="text/css" />
		<script src="/Common/js/jquery-1.11.1/jquery-ui.js" type="text/javascript"></script>
		<script src="/common/js/design.js" type="text/javascript"></script>
		<script src="/common/js/select.js" type="text/javascript"></script>
		<script src="/common/js/jquery.filestyle.js" type="text/javascript"></script>
		<script src="/Common/js/jquery.cookie.js" type="text/javascript"></script>
        <script src="../Common/Js/checktext.js" type="text/javascript"></script> 

		<link href="/Common/Css/jquery.bxslider.css" rel="stylesheet" type="text/css" />
		<script type="text/javascript" src="/Common/js/jquery.bxslider.min.js"></script>
		<%--<script type="text/javascript" src="http://www.gmarwaha.com/jquery/jcarousellite/js/jcarousellite_1.0.1.min.js"></script> --%>
        <script src="/common/js/jcarousellite_1.0.1.min.js" type="text/javascript"></script> 

		<title>T.끌, 소통과 협업의 플랫폼</title>
		<script type="text/javascript">
		    var lnbDep1 = 0;

		    $(document).ready(function () {
		        $("div.pop").show();
		        $("#pop_dc_folder").show();

		        $("#CommonUserList").css("height", "80px");

		        var m_UserID = "<%= UserID %>";
		        var gathering = "<%= GatheringID %>";
		        var type = "Gathering";
		        $('#<%= this.hdGatheringID.ClientID %>').val(gathering);

		        try {
		            $.ajax({
		                type: "POST",
		                contentType: "application/json; charset=utf-8",
		                url: "/Common/Controls/AjaxControl.aspx/GetManagerList",
		                data: "{commonID : '" + gathering + "',tkType : '" + type + "'}",
		                dataType: "json",
		                async: false,
		                success: function (data) {
		                    if (data.d.length == 0) return;

		                    if (data.d[0].length != 0) {
		                        DefaultClear();
		                        DefaultSetting("[" + data.d + "]");
		                    }
		                },
		                error: function (result) {
		                    alert("Error DB Check");
		                }
		            });
		        }
		        catch (exception) {
		            alert('Exception error' + exception.toString());
		        }
		    });

		    //문서함 관리 저장  
		    function fnSave() {
		        //조직도 
		        fnShareSave();

		        //부서 체크
		        var tmpType = $("#UserControl_hdUserType").val();
		        if (tmpType.length > 1) {
		            var userType = tmpType.split('/');
		            var typeCheck = $.inArray("O", userType, 0);
		            if (typeCheck > -1) {
		                alert("부서는 지정하실 수 없습니다.");
		                return;
		            }
		        } else {
		            alert("지정된 관리자가 없습니다.\n관리자를 지정하세요.");
		            return;
		        }
			    //저장 
			    __doPostBack('<%=btnSave.UniqueID %>', '');

			}

			function closeWindow() {
			    parent.hidePop('pop_dc_folder');
			}

			//function DefaultClear() {
			//    $("#CommonUserList").html("");
			//    obj = {};
			//    objj = {};
			//    objt = {};
			//}
			//// 편집화면에서 일부공개 일 경우 list 
			//function DefaultSetting(str) {
			//    var JsonData = JSON.parse(str);
			//    for (var i = 0; i < JsonData.length; i++) {

			//        // 대상자 목록에 추가
			//        pushToArySave(JsonData[i].ToUserName, JsonData[i].ToUserID, JsonData[i].ToUserType);
			//    }
			//}

			////공유 검색에서 사용자 선택시 배열에 저장 
			//function pushToArySave(name, val, utype) {
			//    alert("1");
			//    if (objj[name] == undefined) {
			//        //var NameSet = "<li><a href=\"javascript:\" onclick=\"fnDelete(this,'" + name + "');\">" + name + "</a></li> ";
			//        var author = parent.$("#ctl00_ContentPlaceHolder_Common_Footer_hdGatheringAuther").val();
			//        var author1 = $("#ctl00_ContentPlaceHolder_Common_Footer_hdGatheringAuther").val();
			//        alert(author);
			//        alert(author1);
			//        if (val == author)
			//            NameSet = "<li>" + name + "</li> ";
			//        else
			//            NameSet = "<li><a href=\"javascript:\" onclick=\"fnDelete(this,'" + name + "');\">" + name + "</a></li> ";
			//        $("#CommonUserList").append(NameSet);

			//        objj[name] = val;
			//        objt[name] = utype;
			//    }
			//    else {
			//        alert('이미 추가 되어 있습니다.');
			//    }
			//}

		</script>
	</head>
	<body  onkeydown = "return (event.keyCode!=13)">
		<form name="aspnetForm" method="post" runat="server" action="GatheringManagerIframe.aspx" id="aspnetForm">
			<!--CONTENTS-->
            <div class="pop">
	            <!--문서_폴더관리-->
	            <div id="pop_dc_folder" class="layer_pop" style="margin:0;top:0;left:0;">
		            <h3>모임 공동 관리자 설정</h3>
		            <div id="addWrap">
			            <fieldset class="authority">
				            <h4>모임 관리자 멤버를 선택해 주세요</h4>
				            <common:UserAndDepartment ID="UserControl" UserGroupVisible="false" boolCheckSelf="true" runat="server" />
			            </fieldset>
		            </div>
		            <p class="btn_c">
			            <a href="javascript:closeWindow();" class="btn2"><b>취소하기</b></a>
			            <a href="javascript:fnSave();" class="btn3"><b>완료하기</b></a>
                        
		            </p>
		            <a href="javascript:closeWindow();"><img src="/common/images/btn/pop_close.png" alt="닫기" class="close" /></a>
	            </div>
	            <!--/문서_폴더관리-->
                <
            </div>
            <div style="display:none;">
	            <asp:HiddenField ID="hidMenuType" runat="server" />
	            <asp:HiddenField ID="hdGatheringID" runat="server" />
	            <asp:HiddenField ID="hdGatheringNM" runat="server" /> 

	            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
	            <asp:HiddenField ID="hdCommonID" runat="server" />
	            <asp:HiddenField ID="hdBoardID" runat="server" />
	            <asp:HiddenField ID="hdItemGuid" runat="server" />
	            <asp:HiddenField ID="hdFileName" runat="server" />
            </div>
			<!--/CONTENTS-->
		</form>
	</body>
</html>