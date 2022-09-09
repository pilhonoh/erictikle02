<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossarySearch.master" AutoEventWireup="true" CodeBehind="MyProfile.aspx.cs" Inherits="SKT.Glossary.Web.Glossary.MyProfile" ValidateRequest="false"%>
<%@ Register Src="~/Common/Controls/CommNateOnBizControl.ascx" TagName="NateOnBiz" TagPrefix="common" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">
    
    <script language="javascript" type="text/javascript">
        lnbDep1 = 1;
        var onBeforeUnloadFired = false;
        var IsProfileEditMode = false;
        var m_Empno = '<%= UserID %>';
        var m_UserTeamCode = '<%= UserTeamCode %>';
        var m_UserTeamName = '<%= UserTeam %>';
        var IsOnlyNumUser = '<%= onluNumUser %>';

        var m_Totorial = '<%=TutorialYN %>';
        $(document).ready(function () {
            fntdCareerARefresh();
            fntdCareerBRefresh();
            fntdDocumentRefresh();
        });

        function fntdCareerARefresh() {
            if ($('#btnCareerAMore').text() != "숨기기" && $("#tdCareerA tr").length > 2) {
                $("#tdCareerA tr").each(function (index) {
                    if (index >= 2) {
                        $(this).hide();

                        $('#btnCareerAMore').text("더보기");

                        $('#btnCareerAMore').hide();
                    }
                    else {
                        $(this).show();
                        $('#btnCareerAMore').text("숨기기");
                    }
                });
            }

            if ($("#tdCareerA tr").length <= 2) {
                $('#btnCareerAMore').hide();
            }
            else {
                $('#btnCareerAMore').show();
            }


        }

        function fntdCareerBRefresh() {
            if ($('#btnCareerBMore').text() != "숨기기" && $("#tdCareerB tr").length > 2) {
                $("#tdCareerB tr").each(function (index) {
                    if (index >= 2) {
                        $(this).hide();

                        $('#btnCareerBMore').text("더보기");
                    }
                    else {
                        $(this).show();
                        $('#btnCareerBMore').text("숨기기");
                    }
                });
            }

            if ($("#tdCareerB tr").length <= 2) {
                $('#btnCareerBMore').hide();
            }
            else {
                $('#btnCareerBMore').show();
            }
        }

        function fntdDocumentRefresh() {
            $("#tdDocument tr").each(function (index) {
                if (index >= 5) {
                    $(this).hide();

                    $('#btnDocumentMore').text("더보기");
                }
                else {
                    $(this).show();
                    $('#btnDocumentMore').text("숨기기");
                }
            });

            if ($("#tdDocument tr").length <= 5) {
                $('#btnDocumentMore').hide();
            }
            else {
                $('#btnDocumentMore').show();
            }
        }

        $(function () {
            if (IsOnlyNumUser == 'False') {
                $('a#linkGoTeam').attr('href', '#');
            }
        });

        function fnGoUserInfoModify() {
            //alert('준비중 입니다');
            //2017-01-12 EmpNo 파라미터 제거
            var url = "http://ocs.sktelecom.com/SKT/SKTYellowPage/Layouts/YellowPage/UserInfo_Modify.aspx";
        	var win = window.open(url, "_blank", "left=10, top=10, width=535, height=650, toolbar=no, menubar=no, scrollbars=no, resizable=no");

        	// 2.0 오픈이벤트 처리
        	openEventProfileUpdate();
        }

        $(window).bind('beforeunload', function (e) {
            if (IsProfileEditMode == true) {
                if (!onBeforeUnloadFired) {
                    onBeforeUnloadFired = true;
                    event.returnValue = "페이지를 이동하면 작성중인 내용이 사라집니다 정말 이동하시겠습니까?";
                }
                window.setTimeout("ResetOnBeforeUnloadFired()", 10);
            }
        });

        function ResetOnBeforeUnloadFired() {
            onBeforeUnloadFired = false;
        }


        function fnGoTeam() {
            //alert('준비중 입니다');
            location.href = "/GlossaryMyPages/MyProfile.aspx?DeptCode=" + m_UserTeamCode + "&DeptName=" + m_UserTeamName;
        }

        function fnGoDocRoom() {
            //alert('준비중 입니다');
            location.href = "/GlossaryMyPages/MyDocumentsList.aspx?ReaderUserID=" + m_Empno;
        }


        function fnModify() {
            if (IsProfileEditMode == false) {

                $('#SpanbtnModify').text('[편집완료]');

                //여기서 editor 가 보여지는 코드를 작성한다.
                document.getElementById('DivDisplay').style.display = "none";

                //editor 영역 보여주기
                //$('#DivDEditor').height(600);
                document.getElementById('DivEditor').style.display = "block";

                var iframe = document.getElementsByTagName("iframe");

                var smEditor = iframe[1]; //이미지를 위한 0번 iframe 이 있다...
                if (smEditor) {
                    smEditor.style.height = "500px";
                }

                //Editor 변경 관련 Mostisoft 2015.08.21
                <%--oEditors.getById["ir1"].exec("REFRESH_WYSIWYG");
                oEditors.getById["ir1"].exec("CHANGE_EDITING_MODE", ["WYSIWYG"]);
                var text = $("#<%=lblDisplayBody.ClientID %>").html();
                oEditors.getById["ir1"].exec("SET_CONTENTS", [text]);--%>
                //CrossEditor.SetBodyValue($("#StandaloneView").contents().find('body').html());

                var activeBody = document.aspnetForm;
                activeBody.Wec.DefaultCharSet = "utf-8";
                
                var contents_new = $("#StandaloneView").contents().find('body').html();

                $('#<%= this.hddActiveBody.ClientID %>').val(contents_new);
               

                //페이지아웃체크 실행하기
                IsProfileEditMode = true;
            }
            else {
                fncommit();
            }
            return false;  //이값이 true 이면 서버로 postback 이 일어난다.
        }

        function fncommit() {

            //var jsonObj = { UserID: "", Description: "" };
            //jsonObj.UserID = m_Empno;
            //jsonObj.Description = $(".editor").val();
            //var Descriptiontxt = document.getElementById('DivEditor').nodeValue;
            $("#SpanbtnModify").text('[편집하기]');
            //var Descriptiontxt = $("#editor").val();

            ////Editor 변경 관련 Mostisoft 2015.08.21
            //var Descriptiontxt = CrossEditor.GetBodyValue("XHTML");
            ////var Descriptiontxt = getOrgChartHTML();
            //EspDescriptiontxt = escape(Descriptiontxt);
            
            var activeBody = document.aspnetForm;
            activeBody.Wec.DefaultCharSet = "utf-8";

            $('#<%= this.hddActiveBody.ClientID %>').val(activeBody.Wec.MIMEValue);
            EspDescriptiontxt = activeBody.Wec.MIMEValue;

            var Mode = "";
            Mode = "PERSON";

            <%=Page.GetPostBackEventReference(btnSave) %>;
         
        }

        //프로필 화면으로이동
        function fnProfileView(UserID) {
            location.href = "/GlossaryMyPages/MyProfile.aspx?UserID=" + UserID;
        }

        function escape(val) {
            if (typeof (val) != "string") return val;
            return val
        .replace(/[\\]/g, '\\\\')
        .replace(/[\"]/g, '\\"')
        .replace(/\\'/g, "\\'");
        }


        function fnSKCareer() {
            if (confirm('e-HR 사내 이동 정보를 가져옵니다.\nT.끌 프로필에 추가되며 편집 할 수 있습니다.\n편집 내용은 T.끌에만 적용되며,\n내용 중 오류는 HR 운영팀에 문의해 주세요.')) {
                return true;
            }
            return false;
        }
        function fnNotSKCareer() {
            if (confirm('e-HR 사외 경력 정보를 가져옵니다.\nT.끌 프로필에 추가되며 편집 할 수 있습니다.\n편집 내용은 T.끌에만 적용되며,\n내용 중 오류는 HR 운영팀에 문의해 주세요.')) {
                return true;
            }
            return false;
        }

        function fnCareer() {

            var userid = "<%= UserID %>";

            if(userid.substring(0,1) != "1")
            {
                alert("가사번은 사용할 수 없습니다.");
                return false;
            }       
           
        	if (confirm('e-HR 사내/사외 경력 정보를 가져옵니다.\nT.끌 프로필에 추가되며 편집 할 수 있습니다.\n편집 내용은 T.끌에만 적용되며,\n내용 중 오류는 HR 운영팀에 문의해 주세요.')) {
        		//// 2.0 오픈이벤트 처리
        		////openEventProfileUpdate();
                return true;
            }
            return false;
        }

            
        function fnModifyCareer(ID) {
            var spanid = 'spanCareerA' + ID;
            var hrefid = 'HrefACareerA' + ID;
            var inputbox = 'CareerA' + ID;

            if (document.getElementById(inputbox).style.display == "block") {

                fnModifyAfterCareerCall(ID, document.getElementById(inputbox).value);
                document.getElementById(spanid).innerText = document.getElementById(inputbox).value;
                document.getElementById(spanid).style.display = "block";
                document.getElementById(inputbox).style.display = "none";
                document.getElementById(hrefid).innerText = '편집';

            }
            else {
                document.getElementById(spanid).style.display = "none";
                document.getElementById(inputbox).style.display = "block";
                document.getElementById(hrefid).innerText = '완료';
            }
        }

        function fnModifyAfterCareerCall(ID, Text) {
            try {
                $.ajax({
                    type: "POST",
                    url: "/GlossaryMyPages/MyProfile.aspx" + "/ModifyAfterCareerCall",
                    data: "{ UserID: \"" + m_Empno + "\", ID: \"" + ID + "\", Text: \"" + Text + "\"}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                    	alert('편집내용이 저장되었습니다.');

                    	// 2.0 오픈이벤트 처리
                    	openEventProfileUpdate();
                    },
                    error: function (msg) {
                        alert('fnModifyAfterCareerCall error - 편집에 실패하였습니다.');
                    }
                });
            }
            catch (exception) {
                alert('fnModifyAfterCareerCall error' + exception.toString());
            }
        }

        function fnDeleteCareer(ID) {
            //var DelhrefID = 'DelhrefACareerA' + ID;
            if (confirm('선택한 경력을 숨깁니다. 진행하시겠습니까?')) {
                fnDeleterCareerCall(ID, 'AFTER');
            }
        }

        function fnDeleteNOSKCareer(ID) {
            //var DelhrefID = 'DelhrefACareerA' + ID;
            if (confirm('선택한 경력을 숨깁니다. 진행하시겠습니까?')) {
                fnDeleterCareerCall(ID, 'BEFORE');
            }


        }

        function fnDeleterCareerCall(ID, type) {
            try {
                $.ajax({
                    type: "POST",
                    url: "/GlossaryMyPages/MyProfile.aspx" + "/DeleteCareerCall",
                    data: "{ type: \"" + type + "\", ID: \"" + ID + "\"}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        //alert('내용이 숨겨졌습니다.');
                        //라인삭제필요.
                        if (type == 'AFTER') {
                            fnDeleteAfterCareerLine(ID);
                            fntdCareerARefresh();
                        }
                        else {
                            fnDeleteBeforeCareerLine(ID);
                            fntdCareerBRefresh();
                        }

                        
                    },
                    error: function (msg) {
                        alert('fnDeleterCareerCall error - 삭제에 실패하였습니다.');
                    }
                });
            }
            catch (exception) {
                alert('fnDeleterCareerCall error' + exception.toString());
            }
        }

        function fnDeleteAfterCareerLine(ID) {
            var trid = 'trCareerA' + ID;
            $("#" + trid).remove();
        }
        function fnDeleteBeforeCareerLine(ID) {
            var trid = 'trCareerB' + ID;
            $("#" + trid).remove();
        }

        function fnCareerAMore() {
            if ($('#btnCareerAMore').text() == "더보기") {
                $('#tdCareerA tr').show();
                $('#btnCareerAMore').text("숨기기");
            }
            else {
                $('#btnCareerAMore').text("더보기");
                $("#tdCareerA tr").each(function (index) {
                    if (index >= 2) {
                        $(this).hide();
                    }
                });
            }
        }

        function fnCareerBMore() {
            if ($('#btnCareerBMore').text() == "더보기") {
                $('#tdCareerB tr').show();
                $('#btnCareerBMore').text("숨기기");
            }
            else {
                $('#btnCareerBMore').text("더보기");
                $("#tdCareerB tr").each(function (index) {
                    if (index >= 2) {
                        $(this).hide();
                    }
                });
            }
        }

        function fnDocumentMore() {
            if ($('#btnDocumentMore').text() == "더보기") {
                $('#tdDocument tr').show();
                $('#btnDocumentMore').text("숨기기");
            }
            else {
                $('#btnDocumentMore').text("더보기");
                $("#tdDocument tr").each(function (index) {
                    if (index >= 5) {
                        $(this).hide();
                    }
                });
            }
        }

        function fnNoteSend(receiveUserId) {
            //var rdoType = "Paper";
            //var url = "/Common/Controls/MessageTransfer.aspx?pageType=User&firID=" + receiveUserId + "&secID=&rdoType=" + rdoType;
            //var win = window.open(url, "_blank", "left=10, top=10, width=600, height=500, toolbar=no, menubar=no, scrollbars=yes, resizable=no");
        }


        function fnPhotoOpen() {

            var userid = "<%= UserID %>";

            if(userid.substring(0,1) != "1")
            {
                alert("가사번은 사용할 수 없습니다.");
            }       
            else
            {
                var url = "<%= ConfigurationManager.AppSettings["PhotoEditUrl"].ToString() %>"
                //+ "?user_name=" + base64_encode('<%= UserID %>')
                //+ "&domain_name=tnet.sktelecom.com"
                + "?System=Approval";

        	    window.open(url, "_blank", "width=550,height=510,toolbar=0,location=0,directories=0,status=0,menubar=0,resizable=0,scrollbars=0");

        	    // 2.0 오픈이벤트 처리
        	    openEventProfileUpdate();
            }
        }

        var _KeyStr = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=';
        function base64_encode(input) {
            var output = '', chr1, chr2, chr3, enc1, enc2, enc3, enc4, i = 0;
            function _keyStrCharAt() {
                var ar = arguments, i, ov = '';
                for (i = 0; i < ar.length; i++) ov += _KeyStr.charAt(ar[i]);
                return ov;
            }
            function _utf8_encode(string) {
                string = string.replace(/\r\n/g, '\n');
                var utftext = '', c;
                for (var n = 0; n < string.length; n++) {
                    var c = string.charCodeAt(n);
                    if (c < 128)
                        utftext += String.fromCharCode(c);
                    else if ((c > 127) && (c < 2048))
                        utftext += String.fromCharCode((c >> 6) | 192, (c & 63) | 128);
                    else
                        utftext += String.fromCharCode((c >> 12) | 224, ((c >> 6) & 63) | 128, (c & 63) | 128);
                }
                return utftext;
            }
            input = _utf8_encode(input);
            while (i < input.length) {
                chr1 = input.charCodeAt(i++);
                chr2 = input.charCodeAt(i++);
                chr3 = input.charCodeAt(i++);
                enc1 = chr1 >> 2;
                enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
                enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
                enc4 = chr3 & 63;
                if (isNaN(chr2)) enc3 = enc4 = 64;
                else if (isNaN(chr3)) enc4 = 64;
                output += _keyStrCharAt(enc1, enc2, enc3, enc4);
            }
            return output;
        }

        function base64_decode(input) {
            function _keyStrindexOfinputcharAt(p) { return _KeyStr.indexOf(input.charAt(p)); }
            function _utf8_decode(utftext) {
                var string = '', i = 0, c, c2, c3;
                while (i < utftext.length) {
                    c = utftext.charCodeAt(i);
                    if (c < 128) {
                        string += String.fromCharCode(c);
                        i++;
                    } else if ((c > 191) && (c < 224)) {
                        c2 = utftext.charCodeAt(i + 1);
                        string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                        i += 2;
                    } else {
                        c2 = utftext.charCodeAt(i + 1);
                        c3 = utftext.charCodeAt(i + 2);
                        string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                        i += 3;
                    }
                }
                return string;
            }
            var output = '', chr1, chr2, chr3, enc1, enc2, enc3, enc4, i = 0;
            input = input.replace(/[^A-Za-z0-9\+\/\=]/g, '');
            while (i < input.length) {
                enc1 = _keyStrindexOfinputcharAt(i++);
                enc2 = _keyStrindexOfinputcharAt(i++);
                enc3 = _keyStrindexOfinputcharAt(i++);
                enc4 = _keyStrindexOfinputcharAt(i++);
                chr1 = (enc1 << 2) | (enc2 >> 4);
                chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
                chr3 = ((enc3 & 3) << 6) | enc4;
                output += String.fromCharCode(chr1);
                if (enc3 != 64) output += String.fromCharCode(chr2);
                if (enc4 != 64) output += String.fromCharCode(chr3);
            }
            output = _utf8_decode(output);
            return output;
        }

        function fnView(ItemID, HistoryYN) {
            var url = "/Glossary/GlossaryView.aspx?ItemID=" + ItemID;
            location.href = url;
        }



        function fnPeopleScrap(uid) {

            var ScrapsYN;

            if ($('#btnScrap b').html() == "자주 찾는 담당자로 등록") {
                ScrapsYN = "Y";
            } else {
                ScrapsYN = "N";
            }

            $.ajax({
                type: "POST",
                //url: "/Common/Controls/AjaxControl.aspx" + "/GlossaryScrapInsert",
                url: "/People/PeopleList.aspx" + "/PeopleScrapUpdate",
                data: "{UserID : '<%= myUserId %>', ScrapUserID : '" + uid + "', ScrapsYN : '" + ScrapsYN + "'}",
                contentType: "application/json; charset=utf-8",
                success: function () {
                    if ($('#btnScrap b').html() == "자주 찾는 담당자로 등록") {
                        $('#btnScrap b').html("자주 찾는 담당자 등록해제");
                        return false;
                    } else {
                        $('#btnScrap b').html("자주찾는 담당자로 등록");
                    }
                }
            });
        }

    	function openEventProfileUpdate() {

    		$.ajax({
    			type: "POST",
    			contentType: "application/json; charset=utf-8",
    			url: "/Main.aspx/PromotionEvent_ProfileUpdate",
    			data: "{'UserID':'" + m_UserID + "','EVT_Type':'OPEN'}",
    			dataType: "json",
    			success: function (data) {
    				return;
    			},
    			error: function (result) {
    				//                            alert("Error");
    				//alert(result);
    			}
    		});
    	}


    	function fn_CareerOpen(id, nm) {
    	    var sendMsg = "";

    	    sendMsg += nm + " 매니저님~ 당신이 궁금해요 :D \n";
    	    sendMsg += "티끌에서 담당자를 검색한 <%= HttpContext.Current.Session["Name"].ToString()%> 매니저님이 보내셨습니다\n";
    	    sendMsg += "\n";
    	    sendMsg += "내 업무경력을 구성원과 공유해 볼까요? \n";


    	    //sendMsg += "<a href=\"http://tikle.sktelecom.com/GlossaryMyPages/MyProfile.aspx\">바로가기</a>";

    	    $.ajax({
    	        type: "POST",
    	        contentType: "application/json; charset=utf-8",
    	        url: "/Common/Controls/AjaxControl.aspx/CommNateOnBizSend",
    	        data: "{contentText :'" + sendMsg + "', SendIds :'" + id + "/', SendNMs :'" + nm + "&', SendTYs :'U/', SendLinkNm :'[업무경력 불러오기] 바로가기', SendLinkLink :'http://tikle.sktelecom.com/GlossaryMyPages/MyProfile.aspx', SendLinkType :'', SendDirId :'', SendFileName :''}",
    	        dataType: "json",
    	        success: function (data) {

    	            alert("당신이 궁긍해요를 요청하였습니다.");

    	        },
    	        error: function (response, textStatus, errorThrown) {
    	            alert('쪽지발송 오류:' + response + ':' + textStatus + ':' + errorThrown);

    	            return;
    	        }
    	    });

        }

        // 기존 View를 Iframe 감싸기  Mostisoft 2015.08.21
        function StandaloneView(subCurrentHeight) {
            $("#StandaloneView").height(subCurrentHeight);
        }

    </script>
    <script language="jscript" for="Wec" event="OnInitCompleted()" type="">
        var fm = document.aspnetForm;
        var Wec = document.Wec;

        Wec.Value = $('#<%= this.hddActiveBody.ClientID %>').val();

    Wec.setDefaultFont("맑은 고딕");
    Wec.setDefaultFontSize("11");
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Common_Top" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript">
    var lnbDep1 = 0;		//LNB 1depth
</script>
	<!--CONTENTS-->
	<div id="contents">
        <% if(bOwn) { %>
		<h2><img src="/common/images/text/Mypage.png" alt="마이페이지" /></h2>
        <% } else { %>
        <h2><img src="/common/images/text/Mypage_.png" alt="인물정보" /></h2>
        <% } %>
		<!--article-->
		<div id="article">
			<ul id="tabMenu">
                <% if(bOwn) { %>
				<li><a href="/GlossaryMyPages/MyProfile.aspx" class="on"><img src="/common/images/btn/Mypage_tab1.png" alt="my 프로필" /></a></li>
				<li><a href="/GlossaryMyPages/MyDocumentsList.aspx"><img src="/common/images/btn/Mypage_tab2.png" alt="my 지식 스크랩" /></a></li>
                <li><a href="/GlossaryMyPages/MyScrapList.aspx"><img src="/common/images/btn/Mypage_tab3.png" alt="my 지식 스크랩" /></a></li>
				<%--<li><a href="/GlossaryMyPages/MyPeopleScrapList.aspx"><img src="/common/images/btn/Mypage_tab4.png" alt="my 담당자 스크랩" /></a></li>--%>
                <%--<li><a href="/GlossaryMyPages/MyUseGroup.aspx"><img src="/common/images/btn/Mypage_tab5.png" alt="my 그룹 " /></a></li>--%>
                <% } else { %>
					<li><a href="/GlossaryMyPages/MyProfile.aspx" class="on"><img src="/common/images/btn/Mypage_tab1_.png" alt="프로필" /></a></li>
				<% } %>
				<!--<li><a href="/GlossaryMyPages/MyProfile.aspx">자주 사용하는 그룹</a></li>-->
			</ul>
			<div id="profile">
				<div id="my_profile">
					<dl runat="server" id="MyProfiles">
						<dt>
                            <asp:Label ID="lblName" runat="server"></asp:Label>&nbsp;<asp:Literal ID="litPositionName" runat="server"></asp:Literal> 
                            <!--
                            <a href="javascript:fnGoTeam();" id="linkGoTeam"><asp:Label ID="lblTeam" runat="server" Visible="false"></asp:Label></a>
                            <asp:Label ID="lblTeam2" runat="server" Visible="false"></asp:Label>
                            <asp:Literal runat="server" ID="WorkStatus"></asp:Literal>
                            <asp:Literal runat="server" ID="Literal1"></asp:Literal>
                            -->
						</dt>
                        <dd><span>소속</span>:  <asp:Literal ID="litSosok" runat="server"></asp:Literal></dd>
						<dd><span>연락처</span>: <asp:Literal ID="litPhoneText" runat="server"></asp:Literal></dd>
						<dd><span>근무처</span>: <asp:Literal ID="litWorkArea" runat="server"></asp:Literal></dd>
						<dd><span>담당업무</span>: <asp:Literal ID="litpart" runat="server"></asp:Literal></dd>
						<dd	class="btns">
                            <% if(bOwn) { %>
							<a href="javascript:" class="btn14 btn_pic_change" onclick="fnPhotoOpen()"><b>사진변경 신청</b></a>
							<a href="javascript:alert('개발중입니다.');" class="btn14" style="display:none"><b>자기소개 수정</b></a>
                            <a href="javascript:fnGoUserInfoModify();" class="btn14"><b>담당업무 수정</b></a>
                            <% } %>
                            <asp:LinkButton runat="server" ID="btnCarreer" CssClass="btn14" OnClick="btnCarreer_Click" OnClientClick="return fnCareer();" Visible="false" >
                                <b>업무경력 불러오기</b>
                            </asp:LinkButton>
                            <% if(!bOwn) { %>
                            <a href="javascript:" onclick="pop_NateOnBizTarget_add_Open('<%= UserID %>','<%= UserName %>');" class="btn14"><b>쪽지보내기</b></a>
                            <a id="btnMail" href="mailTo:<%= UserEmail %>" class="btn14"><b>메일보내기</b></a>
							<%--<a id="btnScrap" href="javascript:" onclick="fnPeopleScrap('<%= UserID %>');" class="btn14"><b>자주 찾는 담당자로 등록</b></a>--%>
                            <% } %>
						</dd>
					</dl>
                   	<div id="pic_change_help"><p>e-HR 사진변경신청으로<br />바로 연결됩니다. <br />신청에 대해 HR운영팀의<br />승인이 완료되면, <br />티끌, NateOn Biz, <br />e-HR의 사진이 변경됩니다.</p></div>
                    <p><asp:Image id="imgFace" runat="server" width="146" Height="170" ></asp:Image></p>
                    <script>
                        $(".btn_pic_change").hover(function () { $("#pic_change_help").stop().fadeIn(100).animate({ opacity: 1 }, 100); });
                        $(".btn_pic_change").mouseleave(function () { $("#pic_change_help").fadeOut(100) });
					</script>
					<ul style="display:none">
						<li><b>작성</b><%= HttpUtility.HtmlAttributeEncode(scoreRankingType.WrttenScore.ToString())%></li>
						<li><b>질문</b><%= HttpUtility.HtmlAttributeEncode(scoreRankingType.QnAScore.ToString())%></li>
						<li><b>편집</b><%= HttpUtility.HtmlAttributeEncode(scoreRankingType.EditScore.ToString())%></li>
						<li><b>답변</b><%= HttpUtility.HtmlAttributeEncode(scoreRankingType.QnACommentScore.ToString())%></li>
						<li><b>댓글</b><%= HttpUtility.HtmlAttributeEncode(scoreRankingType.CommentsScore.ToString())%></li>
						<li><b>방문</b><%= HttpUtility.HtmlAttributeEncode(scoreRankingType.Visits.ToString())%></li>
						<li><b>추천</b><%= HttpUtility.HtmlAttributeEncode(scoreRankingType.LikeCountScore.ToString())%></li>
					</ul>
					
				</div>
				<!--<div class="box_ct">
					<h3>중점추진과제</h3>
						<table class="listTable">
							<colgroup><col width="*" /></colgroup>
							<thead>
							<tr>
								<th></th>
							</tr>
							</thead>
							<tbody>
							<tr>
								<td></td>
							</tr>
							</tbody>
						</table>
						<p class="more"><a href="" class="btn_more_s"><b>더보기</b></a></p>
				</div>-->
				<div id="Careers" runat="server" class="box_ct">
					<h3>사내 경력</h3>
					<table class="listTable">
						<colgroup><col width="15%" /><col width="*" /><col width="25%" /><col width="150px"/></colgroup>
						<thead>
						<tr>
							<th>발령일</th>
							<th>내용</th>
							<th>담당업무</th>
							<th runat="server" id="EditSKCarrer" visible="false">항목숨기기</th>
						</tr>
						</thead>
						<tbody id="tdCareerA">
                            <asp:Repeater ID="rptInSKCareer" runat="server" OnItemDataBound="rptInSKCareer_OnItemDataBound">
				                <ItemTemplate>
					                <tr id='trCareerA<%# DataBinder.Eval(Container.DataItem, "ID")%>'>
					                    <td><%# DataBinder.Eval(Container.DataItem, "Date")%></td>
					                    <td><%# DataBinder.Eval(Container.DataItem, "Depart")%></td>
					                    <td>
                                            <span id='spanCareerA<%# DataBinder.Eval(Container.DataItem, "ID")%>'> <%# DataBinder.Eval(Container.DataItem, "Message")%> </span>
                                            <input id='CareerA<%# DataBinder.Eval(Container.DataItem, "ID")%>' type="text" value="<%# DataBinder.Eval(Container.DataItem, "Message")%>"  class="career-input" style="display:none"  />
                                        </td>
                                        <td style="display:<%=DisplayCareer %>">
                                            <a id='HrefACareerA<%# DataBinder.Eval(Container.DataItem, "ID")%>' href="javascript:" onclick="fnModifyCareer('<%# DataBinder.Eval(Container.DataItem, "ID")%>')" class="btn1" style="display:none"><span>편집</span></a>
                                            <a id='DelhrefACareerA<%# DataBinder.Eval(Container.DataItem, "ID")%>' href="javascript:" onclick="fnDeleteCareer('<%# DataBinder.Eval(Container.DataItem, "ID")%>')" class="btn1"><span>숨기기</span></a> 
                                        </td>
					                </tr>
				                </ItemTemplate>
			                </asp:Repeater>
						</tbody>
					</table>
					<p class="more">
                        <asp:LinkButton runat="server" ID="btnAfterCarreer" CssClass="btn_more_s" OnClick="btnAfterCarreer_Click" OnClientClick="return fnSKCareer();" Visible="false" >
                            <span class="btn-career">정보 가져오기</span>
                            </asp:LinkButton>
                        <a id="btnCareerAMore" href="javascript:fnCareerAMore();">더보기</a>
					</p>
                </div>
                <div id="CareersB" runat="server" class="box_ct">
					<h3>사외 경력</h3>
					<table class="listTable">
						<colgroup><col width="15%" /><col width="15%" /><col width="*" /><col width="15%" /><col width="15%" /><col width="150px" /></colgroup>
						<thead>
						<tr>
							<th>시작일</th>
							<th>종료일</th>
							<th>회사명</th>
							<th>부서</th>
							<th>담당업무</th>
							<th runat="server" id="EditNoSKCarrer" visible="false">항목숨기기</th>
						</tr>
						</thead>
						<tbody id="tdCareerB">
                            <asp:Repeater ID="rptInNotSKCareer" runat="server" OnItemDataBound="rptInNotSKCareer_OnItemDataBound">
				                <ItemTemplate>
                                    <tr id='trCareerB<%# DataBinder.Eval(Container.DataItem, "ID")%>'>
					                <td><%# DataBinder.Eval(Container.DataItem, "BeginDate")%></td>
					                <td><%# DataBinder.Eval(Container.DataItem, "EndDate")%></td>
					                <td><%# DataBinder.Eval(Container.DataItem, "Company")%></td>
					                <td><%# DataBinder.Eval(Container.DataItem, "Depart")%></td>
					                <td><%# DataBinder.Eval(Container.DataItem, "Job")%></td>
                                    <td style="display:<%=DisplayCareer%>">
                                        <a id='DelhrefACareerB<%# DataBinder.Eval(Container.DataItem, "ID")%>' href="javascript:" onclick="fnDeleteNOSKCareer('<%# DataBinder.Eval(Container.DataItem, "ID")%>')" class="btn1"><span>숨기기</span></a> 
                                        </td>
					                </tr>
                                </ItemTemplate>
			                </asp:Repeater>



                             <% if (bOwn == false && rptInNotSKCareer.Items.Count == 0 && rptInSKCareer.Items.Count == 0 ){ %>
                            <tr>
								<td colspan="5">
									<dl>
										<dt><b><%=UserName %> 매니저님</b>이 아직 사내/외 경력을 오픈 하지 않았습니다.</dt>
										<dd><%=UserName %> 매니저님께 “당신이 궁금해요”를 요청해 볼까요?</dd>
									</dl>
									<p class="btn_c"><a href="javascript:fn_CareerOpen('<%=UserID %>','<%=UserName %>')" class="btn5"><b>요청하기</b></a></p>
								</td>
							</tr>
                            <% } %>


						</tbody>
					</table>
					<p class="more">
                        <asp:LinkButton runat="server" ID="btnBeforeCarreer" CssClass="btn_more_s" OnClick="btnBeforeCarreer_Click" OnClientClick="return fnNotSKCareer();" Visible="false" >
                            <b>정보 가져오기</b>
                        </asp:LinkButton>
                        <a id="btnCareerBMore" href="javascript:fnCareerBMore();">더보기</a>
					</p>
				</div>

               
      
                
				<div class="box_ct">
					<h3>작성 지식</h3>
					<table class="listTable">
						<colgroup><col width="45%" /><col width="*" /></colgroup>
						<thead>
						<tr>
							<th>제목</th>
							<!--th>최초 작성일</th-->
							<th>마지막 편집일</th>
							<th>마지막 편집자</th>
							<th>조회</th>
							<th>추천</th>
						</tr>
						</thead>
						<tbody id="tdDocument">
                            <asp:Repeater ID="rptDocument" runat="server" OnItemDataBound="rptDocument_OnItemDataBound">
				                <ItemTemplate>
                                <tr id="trDocument<%# DataBinder.Eval(Container.DataItem, "CommonID")%>">
                                    <div class="types" style="display: none">
                                        <asp:Literal runat="server" ID="ltWiki"></asp:Literal>
                                        </div>
                                    <td class="al">
                                        <a href="javascript:fnView('<%# DataBinder.Eval(Container.DataItem, "CommonID")%>','<%# DataBinder.Eval(Container.DataItem, "HistoryYN")%>')">
                                        <asp:Literal runat="server" ID="litPermission"></asp:Literal>
                                            <%# DataBinder.Eval(Container.DataItem, "Title")%><asp:Literal runat="server" ID="litReply"></asp:Literal>
                                        </a>
                                    </td>
                                    <!--td>
                                        <%# DataBinder.Eval(Container.DataItem, "FirstCreateDate")%>
                                    </td-->
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "LastCreateDate")%>
                                    </td>
                                    <td>
                                        <asp:Literal ID="litUserInfo" runat="server"></asp:Literal>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "Hits")%>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "LikeCount")%>
                                    </td>
                                </tr>
                                </ItemTemplate>
			                </asp:Repeater>
						</tbody>
					</table>
					<p class="more"><a id="btnDocumentMore" href="javascript:" onclick="fnDocumentMore()">더보기</a></p>
				</div>
                <div class="box_ct" runat="server" id="MyTeam"  style="display:none;">
				    <h4>팀장</h4>
				    <ul class="search-li-wrap-pic">
					    <li>
						    <div class="pic"><a href="#"><asp:Literal runat="server" ID="TeamTopUserImg"></asp:Literal></a></div>
						    <div class="name"><asp:Label ID="lblTeamLader" runat="server"></asp:Label></div>
					    </li>
				    </ul>
				    <h4>팀 구성원 <span class="search-result-num"><%= TeamNum %>명</span></h4>
				    <ul class="search-li-wrap-pic">
                            <asp:Repeater ID="rptInGeneral" runat="server" OnItemDataBound="rpt_OnItemDataBound">
                                <ItemTemplate>
					                <li>
						                <div class="pic"><asp:Literal runat="server" ID="UserImg"></asp:Literal></div>
						                <div class="name">
                                            <abbr title="<%# DataBinder.Eval(Container.DataItem, "WorkStatus")%>">
                                                <a href="javascript:fnProfileView('<%# DataBinder.Eval(Container.DataItem, "UserID")%>')" class="teamur"><%# DataBinder.Eval(Container.DataItem, "UserName")%> / <%# DataBinder.Eval(Container.DataItem, "DeptName")%></a>
                                            </abbr>
                                        </div>
					                </li>
                                </ItemTemplate>
                            </asp:Repeater>
				    </ul>
			    </div>
                <div class="box_ct">
                    <h3>자기소개</h3>
                        <p class="more">
                             <asp:LinkButton ID="btnModify" CssClass="btn12" runat="server" OnClientClick="return fnModify();">
                                   <span class="edit" id="SpanbtnModify">[편집하기]</span>
                           </asp:LinkButton>
                        </p>
                   <!-- <div id="DivEditor"> -->
                              
		        </div>
	
		        <div>
			        <!-- context -->
			        <div>
                        <div id="DivDisplay">
				         <asp:Label ID="lblDisplayBody" runat="server"></asp:Label>
                         </div>
                         
                         <div id="DivEditor" style="display:none; height:100%" >
                             <script type="text/javascript" src="../NamoActive/NamoWec8.js"></script>
                         </div>
			        </div>
		        </div>

			</div>
		</div>
		<!--/article-->
	</div>
	<!--/CONTENTS-->
<div id="divPop" class="pop" style="display:none">
	<div id="divPopBg" class="popBg"></div>
    <!--의견_쪽지로알리기-->
    <div id="pop_NateOnBizTarget_add" class="layer_pop">
	    <h3>쪽지보내기</h3>
	    <common:NateOnBiz ID="UserControlNateOnBizPop" runat="server" />
	    <p class="btn_c">
		    <a href="javascript:" onclick="pop_NateOnBizTarget_add_Close()" class="btn2"><b>취소하기</b></a>
		    <a href="javascript:" onclick="nateOnBiztargetSend()" class="btn3"><b>보내기</b></a>
	    </p>
	    <img src="/common/images/btn/pop_close.png" title="닫기" class="close" onclick="pop_NateOnBizTarget_add_Close()" />
    </div>
</div>

<!--/의견_쪽지로알리기-->
<script type="text/javascript">
    function pop_NateOnBizTarget_add_Open(receiveUserId, receiveUserName) {

        nateOnBiztargetListSelect('U', receiveUserId, receiveUserName, 'N');

        $("#divPop").show();
        $("#pop_NateOnBizTarget_add").show();
    }

    function pop_NateOnBizTarget_add_Close() {
        $("#divPop").hide();
        $("#pop_NateOnBizTarget_add").hide();
    }
</script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Common_Footer" runat="server">
    <div style ="display:none;">
        <asp:Button ID="hdbtnModify" runat="server" OnClick="hdbtnModify_Click" />
        <asp:HiddenField ID="hdCheck" runat="server" />
         <%--액티브스퀘어--%>
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
        <asp:HiddenField ID="hddActiveBody" runat="server"/>
    </div>
</asp:Content>
