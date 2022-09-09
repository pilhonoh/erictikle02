<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DirectoryViewIframe.aspx.cs" Inherits="SKT.Glossary.Web.Common.Controls.DirectoryViewIframe" %>
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

				var type = "Directory";
				var m_UserID = "<%= UserID %>";
				var dir = "<%= DivID %>";
				
				$('#<%= this.hdDirectoryID.ClientID %>').val(dir);

				try {
					$.ajax({
						type: "POST",
						contentType: "application/json; charset=utf-8",
						url: "/Common/Controls/AjaxControl.aspx/GetMyGroupList",
						data: "{Type : '" + type + "', UserID : '" + m_UserID + "', MyGrpID : '" + dir + "'}",
						dataType: "json",
						async: false,
						success: function (data) {
							if (data.d.length == 0) return;

							if (data.d[0].length != 0) {
								//alert("[" + data.d + "]");
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

			    /*
                Author : 개발자-김성환D, 리뷰자-진현빈G
                Create Date : 2016.08.04 
                Desc : 특수문자 " ' \ 처리
                */
			    if ($('#<%= this.txtTitle.ClientID %>').val().indexOf('\'') >= 0 || $('#<%= this.txtTitle.ClientID %>').val().indexOf('\"') >= 0 || $('#<%= this.txtTitle.ClientID %>').val().indexOf('\\') >= 0) {
			        alert("문서함 이름에 ' 또는 \" 또는 \\ 를 제거하고 저장해주세요");
			        return;
			    }
			    $('#<%= this.txtTitle.ClientID %>').val(strip_tag($('#<%= this.txtTitle.ClientID %>').val()));

				if ($('#<%= this.txtTitle.ClientID %>').val().replace(/^\s+|\s+$/g, '') == "") {
					alert("문서함명을 입력하세요");
					$('#<%= this.txtTitle.ClientID %>').unbind();
					$('#<%= this.txtTitle.ClientID %>').val("");
					$('#<%= this.txtTitle.ClientID %>').focus();
					return;
				}	

	 			//조직도 
		 		fnShareSave();

	 			//저장 
		 		__doPostBack('<%=btnSave.UniqueID %>', '');
			 }

			function closeWindow() {
				parent.hidePop('pop_dc_folder');
			}

			
		</script>
	</head>
	<body>
		<form name="aspnetForm" method="post" runat="server" action="DirectoryViewIframe.aspx" id="aspnetForm">
				<!--CONTENTS-->
<div class="pop">
	<!--<div class="popBg"></div>-->

	<!--문서_폴더관리-->
	<div id="pop_dc_folder" class="layer_pop" style="margin:0;top:0;left:0;">
		<h3>멤버 관리</h3>
		<div id="addWrap">
			<p><input type="text" id="txtTitle" name="txtTitle" runat="server" class="txt t5" value="새로운 문서공유방명을 입력해주세요"/></p>
			<fieldset class="authority">
				<h4>문서함 멤버를 선택해 주세요</h4>
				<common:UserAndDepartment ID="UserControl" runat="server" />
			</fieldset>
		</div>
		<p class="btn_c">
			<a href="javascript:closeWindow();" class="btn2"><b>취소하기</b></a>
			<a href="javascript:fnSave();" class="btn3"><b>완료하기</b></a>
		</p>
		<a href="javascript:closeWindow();"><img src="/common/images/btn/pop_close.png" alt="닫기" class="close" /></a>
	</div>
	<!--/문서_폴더관리-->
</div>
<div style="display:none;">
	<asp:HiddenField ID="hidMenuType" runat="server" />
	<asp:HiddenField ID="hdDirectoryID" runat="server" />
	<asp:HiddenField ID="hdDirectoryNM" runat="server" /> 
	<asp:HiddenField ID="hdFileID" runat="server" /> 
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