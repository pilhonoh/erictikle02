<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossarySearch.Master" AutoEventWireup="true" CodeBehind="GlossaryView.aspx.cs" Inherits="SKT.Glossary.Web.Glossary.GlossaryView" ValidateRequest="false" %>

<%@ Register Src="~/Common/Controls/CommCommentControl.ascx" TagName="Comment" TagPrefix="Common" %>
<%@ Register Src="~/Common/Controls/UserAndDepartmentList.ascx" TagName="UserAndDepartment" TagPrefix="common" %>
<%@ Register Src="~/Common/Controls/GatheringInfomation.ascx" TagName="GatheringInfomation" TagPrefix="common" %>
<%@ Register Src="~/Common/Controls/GatheringMenuTab.ascx" TagName="GatheringMenuTab" TagPrefix="common" %>
<%@ Register Src="~/Common/Controls/CommNateOnBizControl.ascx" TagName="NateOnBiz" TagPrefix="common" %>
<%@ Register assembly="SKT.Tnet" namespace="SKT.Tnet.Controls" tagprefix="SKTControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">
    <!--[if IE]>
	<link href="/Common/Css/fuxkie8.css" rel="stylesheet" type="text/css" />
	<![endif]-->
    <script src="/common/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Common/js/jquery.cookie.js" type="text/javascript"></script>
    <script src="/Common/js/jquery-ui.js" type="text/javascript"></script>
    <script src="/Common/js/json2.js" type="text/javascript"></script>
    <script src="/common/js/design.js" type="text/javascript"></script>
    <script src="../Common/Js/dhtmlHistory.js" type="text/javascript"></script>
    <!-- 기본 CSS 및 JS 정의 -->        
    <link href="/Common/Css/board_G.css" rel="stylesheet" />
    <!-- Script -->
    <script type="text/javascript" src="/common/Js/TnetBoard_Control.js"></script>
    <!-- 디자인팀적용 JS -->
    <script type="text/javascript" src="/Common/Js/css.browser.detect.js"></script>

    <style>
      
    </style>

    <script type="text/javascript" language="javascript">
    var PopupSet = true;
    var m_ItemID = '<%= ItemID %>';
    var m_CommonID = '<%= CommonID %>';
    var m_UserID = '<%= UserID %>';
    var m_UserName = '<%= UserName %>';
    var m_YouUserID = '<%= YouUserID %>';
    var m_Title = '<%= Title %>';
    var AlarmOpen = true;
    var ShareOpen = true;
    var DownLoadOpen = true;

    var TagCheck = false;   // 2014-06-23 Mr.No

    //function initialize() {
    //    // 초기화
    //    dhtmlHistory.initialize();
    //    // historyfunc 가 콜백함수이다
    //    dhtmlHistory.addListener(historyfunc);
    //}

    function historyfunc() {
        if (historyStorage.get('state')) {
            history.back();
            if ($('#<%= this.txtTag.ClientID %>').val() == "") {
                $("#<%= this.TagLists.ClientID %>").html(historyStorage.get('state'))
            }
        }
    }

    $(document).ready(function () {

        $(".hashSave").click(function () {
        if ($('#<%= this.TagLists.ClientID %>').html() != "") {
                dhtmlHistory.add('state', $.trim($('#<%= this.TagLists.ClientID %>').html()));
            }
        });

        $(".csLikeNum").hover(function () {
            fnGetLikeList();
            $("div#likeUserPop").css('display', 'inline-block');
        });

        // 2014-05-22 Mr.No
        var AttachInfoString = replaceAll('<%=this.AttachInfo %>', "//", "'");
        var AttachInfo = $.parseJSON(AttachInfoString);
        if (AttachInfo.length > 0) {
        } else {
            $('#manager_container').hide();
        }
        // 최초 작성자 Check hdPermissionRtnValue
        if ($('#<%= this.hdPermissionRtnValue.ClientID %>').val() == "show") {
            $("#shareID").show();
        }

        //2015-12-28 P033028 파일첨부 호출
        AttachArea();

        if ($('#<%= this.txtTag.ClientID %>').val() != "") {
            TagSet();
        }

        LikeSelect();

    });

    function LikeSelect()
    {
        $.ajax({
            type: "POST",
            url: "/Common/Controls/AjaxControl.aspx" + "/GlossaryLikeSelect",
            data: "{GlossaryID : '" + m_CommonID + "'}",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var d = data.d;
                
                if (d.TotalCount != null) {
                    if (d.TotalCount == 0) {
                        $('#LikeCnt').text('추천하기(' + (parseInt(d.TotalCount) + 1) + ')');
                        $('#LikeMessageArea').text(d.LatestUserName + "이 감사의 마음을 남기셨습니다");
                    } else {
                        $('#LikeCnt').text('추천하기(' + (parseInt(d.TotalCount) + 1) + ')');
                        $('#LikeMessageArea').text(d.LatestUserName + " 외 " + d.TotalCount + " 명이 감사의 마음을 남기셨습니다");
                    }
                }
            },
            error: function (result) {
                alert("Error" + ":::" + result);
            }
        });
    }


    function PermissionsStringThrow() {
        var pmStr = $('#<%= this.hdPermissionsString.ClientID %>').val();
        var pmData = JSON.parse(pmStr);

        if (pmData.length > 1) {
            $("#SomePublic").attr('checked', 'checked');
            PermisstionValCheck();
        } else {
            PermisstionValCheck();
        }

        for (var i = 0; i < pmData.length; i++) {
            // 대상자 목록에 추가
            pushToArySave(pmData[i].ToUserName + "/" + pmData[i].ToUserID, pmData[i].ToUserID, pmData[i].ToUserType);
        }
    }

    //알람 오픈
    function fnAlarmOpen() {
        $("div[class^='view-func-']").hide();
        if (AlarmOpen == true) {
            $(".view-func-alarm-outer").show();
            DownLoadOpen = true;
            ShareOpen = true;
            AlarmOpen = false;
        } else {
            $(".view-func-alarm-outer").hide();
            AlarmOpen = true;
        }
    }

    function fnGetLikeList() {
        $.ajax({
            type: "POST",
            url: "/Common/Controls/AjaxControl.aspx" + "/GlossaryLikeUserList",
            data: "{GlossaryID : '" + m_CommonID + "', Mode : 'Total'}",
            contentType: "application/json; charset=utf-8",
            success: function (result) {

                $("div#likeUserPopBody").html(result.d);
            }
        });
    }



    function fnLikeUserPopClose() {
        $("div#likeUserPop").css('display', 'none');
    }

    //공유 오픈
    function fnShareOpen() {
        $("div[class^='view-func-']").hide();
        if (ShareOpen == true) {
            $(".view-func-share-outer").show();

            //UserList.ascx 컨트롤 ID
            $("#ShareSuccess").css('display', 'none');
            $("#ShareAdd").css('display', 'block');
            AlarmOpen = true;
            DownLoadOpen = true;
            ShareOpen = false;
        } else {
            $(".view-func-share-outer").hide();
            ShareOpen = true;
        }
    }

    //PDF 오픈
    function fnDownLoadOpen() {
        $("div[class^='view-func-']").hide();
        if (DownLoadOpen == true) {
            $(".view-func-save-outer").show();
            AlarmOpen = true;
            ShareOpen = true;
            DownLoadOpen = false;
        } else {
            $(".view-func-save-outer").hide();
            DownLoadOpen = true;
        }
    }

    var likeflag = '<%= Liked %>';
    function Validity(Item, AlarmYN) {

        switch (Item) {
            //편집 버튼 눌렀을때
            case "HistorySave":
                $.ajax({
                    type: "POST",
                    url: "/Common/Controls/AjaxControl.aspx" + "/GlossaryModifyYN",
                    data: "{ItemID : '" + m_ItemID + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (ModifyYNTableCnt) {
                        //location.href = "/Glossary/GlossaryWrite.aspx?mode=History&ItemID=" + m_ItemID + "&CommonID=" + m_CommonID + "&SearchKeyword=" + '<%=GetSearchKeyword()%>' + "&GatheringYN=<%=this.GatheringYN %>&GatheringID=<%=this.GatheringID %>";

                        var tagTitle = encodeURIComponent(encodeURIComponent('<%=TagTitle%>'));
                        
                        <% if (!string.IsNullOrEmpty(this.WType)) { %>
                            location.href = "/Glossary/GlossaryWriteNew.aspx?mode=History&ItemID=" + m_ItemID + "&CommonID=" + m_CommonID + "&TagTitle=" + tagTitle + "&SearchSort=<%=SearchSort%>&PageNum=<%=PageNum%>&Wtype=<%=WType%>&SchText=<%=SchText%>";
                        <% } else if (!string.IsNullOrEmpty(this.TType)) { %>
                            location.href = "/Glossary/GlossaryWriteNew.aspx?mode=History&ItemID=" + m_ItemID + "&CommonID=" + m_CommonID + "&TagTitle=" + tagTitle + "&SearchSort=<%=SearchSort%>&PageNum=<%=PageNum%>&Ttype=<%=TType%>&SchText=<%=SchText%>";
                        <% } else  { %>
                            location.href = "/Glossary/GlossaryWriteNew.aspx?mode=History&ItemID=" + m_ItemID + "&CommonID=" + m_CommonID + "&TagTitle=" + tagTitle + "&SearchSort=<%=SearchSort%>&PageNum=<%=PageNum%>";
                        <% }  %>
                    }
                });
                    break;

                //스크랩 버튼 눌렀을때 
                case "Scrap":
                    var ScrapsYN = null;
                    if ($('#btnScrap a b').html() == "스크랩하기") {
                        ScrapsYN = "Y";
                    } else {
                        ScrapsYN = "N";                        
                    }

                    

                    $.ajax({
                        type: "POST",
                        url: "/Common/Controls/AjaxControl.aspx" + "/GlossaryScrapInsert",
                        //data: "{Title : '" + $('#<%= this.lbTitle.ClientID %>').html() + "', UserID : '" + m_UserID + "', YouUserID : '" + m_YouUserID + "', GlossaryID : '" + m_CommonID + "', ScrapsYN : '" + ScrapsYN + "'}",
                        data: "{Title : '" + $('#txtTitle').html() + "', UserID : '" + m_UserID + "', YouUserID : '" + m_YouUserID + "', GlossaryID : '" + m_CommonID + "', ScrapsYN : '" + ScrapsYN + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function () {
                        if ($('#btnScrap a b').html() == "스크랩하기") {
                            $('#btnScrap a b').html("스크랩해제");
                            $('#btnScrap').addClass("on");
                            fnScrapOk();
                        } else {
                            $('#btnScrap a b').html("스크랩하기");
                            $('#btnScrap').removeClass("on");
                            fnScrapClose();
                        }
                    }
                    });                    
                break;

                //좋아요 버튼 눌렀을때
            case "Like":
                fnLikeUserPopClose();

                if (likeflag == "Y")
                {
                    confirmLikeYn();
                    return;
                }

                var beforeCount = parseInt($('#<%= this.lbLikeCounts.ClientID %>').text());

                $.ajax({
                    type: "POST",
                    url: "/Common/Controls/AjaxControl.aspx" + "/GlossaryLikeInsert",
                    data: "{UserID : '" + m_UserID + "', GlossaryID : '" + m_CommonID + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {

                        var d = data.d;

                        //{$(.text('이미 감사의 마음을 남기셨습니다.');
                        //$('#<%= this.LikeAdd.ClientID %>').attr("href", "javascript:confirmLikeYn();");
                    	//$('#thanksArea img').removeAttr(onclick);
                  
                    	if (d.TotalCount == 0) {
                    	    // CHG610000060541 / 2018-03-27 / 최현미 / 님제거
                    	    alert(d.LatestUserName + "이 감사의 마음을 남기셨습니다");
                    	    $('#LikeCnt').text('추천하기(' + (parseInt(d.TotalCount) + 1) + ')');
                    	    $('#LikeMessageArea').text(d.LatestUserName + "이 감사의 마음을 남기셨습니다");
                    	    
                    	} else {
                    	    alert(d.LatestUserName + " 외 " + d.TotalCount + " 명이 감사의 마음을 남기셨습니다");
                    	    $('#LikeCnt').text('추천하기(' + (parseInt(d.TotalCount) + 1) + ')');
                    	    $('#LikeMessageArea').text(d.LatestUserName + " 외 " + d.TotalCount + " 명이 감사의 마음을 남기셨습니다");
                        }

                        if (likeflag == "N") {
                            // 하나sk패일리카드 글지식만 쪽지 미발송 처리
                            if (m_CommonID != '6209') {
                                //alert("쪽지발송");
                                $.ajax({
                                    type: "POST",
                                    url: "/Common/Controls/AjaxControl.aspx" + "/GlossaryLikeNote",
                                    data: "{UserID : '" + m_UserID + "', GlossaryID : '" + m_CommonID + "', UserName : '" + m_UserName + "'}",
                                    contentType: "application/json; charset=utf-8",
                                    success: function () {
                                        likeflag = "Y";
                                        $("#likeicon").attr("onclick", "javascript:#");
                                    }
                                });
                            }
                        }
                    }
                });
                //confirmLikeYn();
                break;

                //변경 알람 버튼 눌렀을때
            case "Alarm":
                $.ajax({
                    type: "POST",
                    url: "/Common/Controls/AjaxControl.aspx" + "/GlossaryAlarm",
                    data: "{CommonID : '" + m_CommonID + "', UserID : '" + m_UserID + "', MailSet : 'N', NoteSet : '" + AlarmYN + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (result) {
                    }
                });
                break;
                //번경된 내용 버튼 눌렀을때 
            case "HistoryList":
                var tagTitle = encodeURIComponent(encodeURIComponent('<%=TagTitle%>'));

                <% if (!string.IsNullOrEmpty(this.WType)) { %>
                    window.location = "/GlossaryHistory/HistoryList.aspx?TagTitle=" + tagTitle + "&ItemID=" + m_ItemID + "&SearchKeyword=<%=GetSearchKeyword()%>&SearchSort=<%=SearchSort%>&PageNumList=<%=PageNum%>&Wtype=<%=WType%>&SchText=<%=SchText%>";
                <% } else if (!string.IsNullOrEmpty(this.TType)) { %>
                    window.location = "/GlossaryHistory/HistoryList.aspx?TagTitle=" + tagTitle + "&ItemID=" + m_ItemID + "&SearchKeyword=<%=GetSearchKeyword()%>&SearchSort=<%=SearchSort%>&PageNumList=<%=PageNum%>&Ttype=<%=TType%>&SchText=<%=SchText%>";
                <% } else  { %>
                    window.location = "/GlossaryHistory/HistoryList.aspx?TagTitle=" + tagTitle + "&ItemID=" + m_ItemID + "&SearchKeyword=<%=GetSearchKeyword()%>&SearchSort=<%=SearchSort%>&PageNumList=<%=PageNum%>";
                <% }  %>

                break;
            default:
                break;
        }
    }

   function confirmLikeYn() {
        alert("이미 감사의 마음을 남기셨습니다.");
	}

	function fnAlarmChanger(Item, AlarmYN) {
	    if (AlarmYN == "N") {
	        AlarmYN = "Y";
	        Validity(Item, AlarmYN);
	        $('#view-func-wrap .view-subbtn ul li a span.alarm').attr('class', 'alarm_yellow');
	        $('a#alarmid').attr('href', 'javascript:fnAlarmChanger(\'Alarm\',\'Y\');');
	        alert("쪽지 알림이 설정 되었습니다.");
	    } else {
	        AlarmYN = "N";
	        Validity(Item, AlarmYN);
	        $('#view-func-wrap .view-subbtn ul li a span.alarm_yellow').attr('class', 'alarm');
	        $('a#alarmid').attr('href', 'javascript:fnAlarmChanger(\'Alarm\',\'N\');');
	        alert("쪽지 알림이 해제 되었습니다.");
	    }
	}

	function fnTikleDelete() {
	    return confirm('현재 지식을 정말 삭제하시겠습니까?');
	}

	function fnTikleGoToHall() {
	    return confirm('현재 지식을 명예의 전당으로 보내시겠습니까?');
	}


	//인쇄 화면 프린터
	function printIt() {
	    window.open("/GlossaryControl/GlossaryPrint.aspx?ItemID=" + m_ItemID);
	}


	//프로필 보기
	function fnProfileView(UserID) {
	    //GetWork(UserID);
	    if (UserID == "") {
	        alert("작성자 비공개 글 입니다.");
	    }
	    else {
	        location.href = "/GlossaryMyPages/MyProfile.aspx?UserID=" + UserID;
	    }
	}

	//프로필 보기
	function fnMyScrapListView() {
	    location.href = "/GlossaryMyPages/MyScrapList.aspx";
	}

	//태그 View
	function TagView(ItemID) {
	    //window.open("/Glossary/GlossaryView.aspx?mode=Tag&ItemID=" + ItemID);
	    location.href = "/Glossary/GlossaryView.aspx?mode=Tag&ItemID=" + ItemID;
	}

	function GetWork(UserID) {
	    $.ajax({
	        type: "POST",
	        url: "/Common/Controls/AjaxControl.aspx" + "/EHRWorkStatus",
	        data: "{UserID : '" + UserID + "'}",
	        contentType: "application/json; charset=utf-8",
	        async: false,
	        success: function (ret) {
	            alert(ret.d[0]);
	        }
	    });
	}

    function replaceAll(sValue, param1, param2) {
        return sValue.split(param1).join(param2);
    }

    function fnGlossaryList(tagtitle, searchsort) {
        var url = "/Glossary/GlossaryNewsList.aspx?TagTitle=" + tagtitle + "&SearchSort=" + searchsort;
        location.href = url;
    }

    // Tag Default Value
    function TagSet() {
        var TagSearch = $('#<%= this.txtTag.ClientID %>').val();
        var TagValue = $('#<%= this.hdTagValue.ClientID %>').val();

        $('#<%= this.hdTagTemp.ClientID %>').val($('#<%= this.txtTag.ClientID %>').val());
        if (TagSearch.indexOf(',') == -1) {
           
        } else {

            // 처음 셋팅 시 들어옴
            var TagSearchArr = [];
            var TagValueArr = [];
            TagSearchArr = TagSearch.split(",");
            TagValueArr = TagValue.split(",");


            for (var i = 0; i < TagSearchArr.length; i++) {
            if (TagSearchArr[i].replace(/^\s+|\s+$/g, '') != "") {
                TagSearch = "<dd style=\"cursor:pointer\" onclick=\"javascript:fnGlossaryList('" + encodeURIComponent(encodeURIComponent(TagSearchArr[i])) + "', '');\">" + TagSearchArr[i] + "</dd>";
                $('#<%= this.TagLists.ClientID %>').html($("#<%= this.TagLists.ClientID %>").html() + TagSearch);
                }
            }
        }
        //$('#<%= this.txtTag.ClientID %>').val("");
    }

    // 2014-06-17 Mr.No  // 2015-10-27 ksh
    function fnPrevListUrl() {

        if ("<%=WType%>" == "I" || "<%=WType%>" == "D") {
            location.href = "/Glossary/DigitalTrans.aspx?WType=<%=WType%>&PageNum=<%=PageNum%>&SchText=<%=SchText%>";
        }
        else if ("<%=TType%>" == "A" || "<%=TType%>" == "B" || "<%=TType%>" == "C") {
            location.href = "/Glossary/WhitePaper.aspx?TType=<%=TType%>&PageNum=<%=PageNum%>&SchText=<%=SchText%>";
        }
        else if (document.referrer.toUpperCase().indexOf("SEARCH.ASPX") > -1 || document.referrer.toUpperCase().indexOf("MYSCRAPLIST.ASPX") > -1) {
            history.back(-1);
        }
        else {
            var tagTitle = encodeURIComponent(encodeURIComponent('<%=this.TagTitle%>'));
            location.href = "/Glossary/GlossaryNewsList.aspx?TagTitle=" + tagTitle + "&SearchSort=<%=SearchSort%>&PageNum=<%=PageNum%>";
        }
    }

    // Tag Title Check
    function TagRedundancy_Check(TagTitle) {

        var ID;
        $.ajax({
            type: "POST",
            url: "/Common/Controls/AjaxControl.aspx" + "/Tag_Redundancy_Check",
            data: "{TagTitle : '" + TagTitle + "', CommonID : '" + '<%= CommonID %>' + "'}",
		        contentType: "application/json; charset=utf-8",
		        async: false,
		        success: function (rtnValue) {
		            ID = rtnValue.d[0];
		        }
		    });
        return ID;
    }

    function fnScrapOk() {
        $("div.pop").show();
        $(".popBg").show();
        $("#pop_alert").show();
    }

    function fnScrapClose() {
        $("#pop_alert").hide();
        $(".popBg").hide();
        $("div.pop").hide();

        return false;
    }

    function fnScrapCancle() {

        Validity('Scrap');
        return fnScrapClose();
    }

    function pop_authority_Open() {
        TitleDBCheck();
        if (m_titleCheck == true) {
            alert('끌.지식에 동일한 제목이 존재합니다. 공유하실수 없습니다.');
            return false;
        }

        $("div.pop").show();
        $(".popBg").show();
        $("#pop_authority").show();
    }

    function pop_authority_Close() {

        $("#pop_authority").hide();
        $(".popBg").hide();
        $("div.pop").hide();
        return false;
    }

    /*
        Author : 개발자- 최현미C, 리뷰자-진현빈D
        CreateDae :  2016.05.18
        Desc : 이중클릭 방지       
    */
    var boolMailCheck = false;
    function pop_authority_Save() {

        //Author : 개발자- 김성환D, 리뷰자-진현빈D
        //CreateDae :  2016.05.25
        //Desc : 대상자 유효성 체크 
        if ($("ul#CommonUserList > li").size() < 1) {
            alert("공유할 대상자를 지정하세요.");
            return false;
        }

        try{
            //특정사람
            if ($('#SomePublic').attr("checked")) 
            {
                if (boolMailCheck == true)
                    return false;

                if (confirm("쪽지도 같이 발송 됩니다.\n진행하시겠습니까?\n")) {
                    $('#divImgLoading').show();
                    boolMailCheck = true;
                    $('#btnSend').removeClass("btn3");
                    $('#btnSend').addClass("btn3_dis");

                    $('#FullPublic').attr("disabled", true);
                    $('#SomePublic').attr("disabled", true);

                    PermissionsCheck();
                    fnShareSave();
                    __doPostBack('<%=btnSave.UniqueID %>', '');

                }
            }
            //전사지식
            else
            {
                boolMailCheck = false;
                $('#divImgLoading').hide();
                $('#btnSend').removeClass("btn3_dis");
                $('#btnSend').addClass("btn3");
                $('#FullPublic').attr("disabled", false);
                $('#SomePublic').attr("disabled", false);

                PermissionsCheck();
                fnShareSave();
                __doPostBack('<%=btnSave.UniqueID %>', '');
            }
        } catch (e) {

            boolMailCheck = false;
            $('#divImgLoading').hide();
            $('#btnSend').removeClass("btn3_dis");
            $('#btnSend').addClass("btn3");
            $('#FullPublic').attr("disabled", false);
            $('#SomePublic').attr("disabled", false);

            alert(e);
       }
    }

    //쪽지발송(끌지식 -> 공유하기)
    //2015-07-22 KSH 쪽지 추가//
    function pop_noti_Save() {
        //alert(m_UserID+"/"+m_CommonID+"/"+m_UserName);
        //alert("쪽지발송");
        //$.ajax({
        //    type: "POST",
        //    url: "/Common/Controls/AjaxControl.aspx" + "/GlossaryShareNote",
        //    data: "{UserID : '" + m_UserID + "', GlossaryID : '" + m_CommonID + "', UserName : '" + m_UserName + "'}",
        //    contentType: "application/json; charset=utf-8",
        //    success: function () {
        //        likeflag = "Y";
        //        $("#likeicon").attr("onclick", "javascript:#");
        //    }
        //});

        //PermissionsCheck();


        //Author : 개발자- 김성환D, 리뷰자-진현빈D
        //CreateDae :  2016.05.25
        //Desc : 대상자 유효성 체크 
        if ($("ul#CommonUserList > li").size() < 1) {
            alert("발송 할 대상이 없습니다.");
            return false;
        }

        try{

            if (boolMailCheck == true)
                return false;

            if (confirm("쪽지도 같이 발송 됩니다.\n진행하시겠습니까?\n")) {
                $('#divImgLoading').show();
                boolMailCheck = true;
                $('#btnSend').removeClass("btn3");
                $('#btnSend').addClass("btn3_dis");

                fnShareSave();
                __doPostBack('<%=btnNotiSend.UniqueID %>', '');
               
            }

        } catch (e) {

            boolMailCheck = false;
            $('#divImgLoading').hide();
            $('#btnSend').removeClass("btn3_dis");
            $('#btnSend').addClass("btn3");

            alert(e);
        }
    }
    // 쪽지 추가 //

    function PermissionsCheck() {
        $('#<%= this.hdPermissions.ClientID %>').val($(":input:radio[name=rBtnA]:checked").val());
    }
    function PermisstionValCheck() {
        var permissionValue = $(":input:radio[name=rBtnA]:checked").val();

        // 일부공개
        if (permissionValue == "SomePublic") {
            $("#UserAndDepartment").show();
            $("#<%= GatheringDDL.ClientID %>").show();
        }
            // 전체공개
        else {
            $("#UserAndDepartment").hide();
            $("#<%= GatheringDDL.ClientID %>").hide();
        }
    }
    var m_titleCheck = false;
    function TitleDBCheck() {
        //20140207 제목 어퍼스트로피 있을 시 에러, 처리
        var txtTitle = $('#txtTitle').text().replace(/'/g, "&#39;");
        var cid = '<%= CommonID %>';
        try {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Common/Controls/AjaxControl.aspx/ExistTitle",
                data: "{'Title':'" + txtTitle + "','ID':'" + cid + "'}",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.d[0].length != 0) {
                        //alert('success');
                        m_titleCheck = true;
                    }
                    else {
                        m_titleCheck = false;
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
    }

    // 기존 View를 Iframe 감싸기  Mostisoft 2015.08.21
    function StandaloneView(subCurrentHeight) {
        $(".StandaloneView" + Number('<%= ItemID %>')).height(subCurrentHeight);
    }

    //2015-12-28 P033028 : 파일업로드 변경관련 추가
    function fnMultiDown(glossaryID, obj) {
        var i, sum = 0, str = "";
        var chk = document.getElementsByName(obj);
        var tot = chk.length;
        var chkValue = "";
        var FileName = "";
        var Folder = "";

        for (i = 0; i < tot; i++) {
            if (chk[i].checked == true) {
                var tmpValue = chk[i].value.split("#");

                FileName = tmpValue[1];
                Folder = tmpValue[2];

                if (chkValue == "") {
                    chkValue = tmpValue[0];
                }
                else {
                    chkValue += "$" + tmpValue[0];
                }
                sum++;
            }
        }
        if (sum == 0) {
            alert("다운로드 할 파일을 선택해 주세요.");
            return false;
        }
        if (sum == 1) { //체크된 항목이 1개이면 바로 다운로드 실행
            FileCtrl_FileDownload(FileName, "/SKT_MultiUploadedFiles/" + Folder + "/" + FileName);
        } else {
            if (tot == sum) { //전체선택인 경우 플래그 'Y'로 변경
                $('#<%= this.hidChkAllFlag.ClientID %>').val("Y");
            } else {
                $('#<%= this.hidChkAllFlag.ClientID %>').val("N");
            }
            $('#<%= this.hidFileChkAttachID.ClientID %>').val(chkValue);
            $('#<%= this.hidGlossaryID.ClientID %>').val(glossaryID);
            document.getElementById('<%= btnMulti.ClientID %>').click();
        }
    }

    //첨부파일이 존재할 경우 동적으로 처리하는 영역
    function AttachArea() {
        var para = new Object();
        para.GlossaryID = m_CommonID;

        //첨부 목록
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Glossary/GlossaryView.aspx/GetAttachInfo2",
            data: JSON.stringify(para),
            dataType: "json",
            success: function (data) {
                AttachInfo = jQuery.parseJSON(data.d);

                if (AttachInfo.length > 0) {
                   
                    //2015.10.26 zz17779 : 파일업로드 변경관련 추가
                    var dHtml2 = "";
                    var fileLink = "";
                    var totalSize = "";
                    
                    dHtml2 += "<tr class='t_bold' style='background-color:#efefef;'>";
                    dHtml2 += "<td><input type='checkbox' id='chkAllFile' name='chkAllFile' onclick='chkAll(this);'/></td></td>";
                    dHtml2 += '<td colspan="2" class="fileTitle">파일명</td>';
                    dHtml2 += '<td class="fileSize" style="padding-right:10px;">크기</td>';
                    dHtml2 += '</tr>'

                    for (var i = 0; i < AttachInfo.length; i++) {
                        
                        fileLink = "<a href=\"javascript:void(0);\" onClick=\"FileCtrl_FileDownload('" + AttachInfo[i].FileName + "', '/SKT_MultiUploadedFiles/" + AttachInfo[i].Folder.replace("\\", "/") + "/" + AttachInfo[i].FileName + "' );\">" + AttachInfo[i].FileName + "</a>";
                        
                        dHtml2 += "<tr FileName='" + AttachInfo[i].FileName + "'>";
                        dHtml2 += StringFormat('<td class="fileIcon"><input type="checkbox"  name="chkFile" id="chk{0}" value="{1}" /></td>', i, AttachInfo[i].AttachID + "#" + AttachInfo[i].FileName + "#" + AttachInfo[i].Folder.replace("\\", "/") + "#");
                        dHtml2 += StringFormat('<td class="fileIcon"><img class="fileUploadTypeIcon" src="/Images/ICON/IC{0}.gif"></td>', AttachInfo[i].Extension.toUpperCase().replace(".", ""));
                        dHtml2 += StringFormat('<td>{0}</td>', fileLink);
                        dHtml2 += StringFormat('<td class="fileSize">{0}</td>', AttachInfo[i].FileSizeString);
                        dHtml2 += '</tr>'
                        
                        totalSize = AttachInfo[i].TotalFileSize;
                    }
                    dHtml = '<div class="Glossary_filewrite writeBox">';
                    dHtml += '<div class="FileUploadCtrl">';
                    dHtml += '<div class="FileUloadTable">';
                    dHtml += '<table>';
                    dHtml += dHtml2;
                    dHtml += '</table>';
                    dHtml += '</div>'; //FileUloadTable
                    dHtml += '<div id="divmulti">';
                        dHtml += "<p class=\"writeBox_text\">";
                        dHtml += "전체 : " + AttachInfo.length + "개 (" + totalSize + ")";
                        dHtml += "</p>";
                        dHtml += "<a href=\"javascript:///\" class=\"btnB\" onclick=\"fnMultiDown('" + m_CommonID + "', 'chkFile');\" id=\"btnMultidown\"><span>다운로드</span></a>";
                        dHtml += '</div>'; //divmulti

                        dHtml += '</div>'; //FileUploadCtrl
                        dHtml += '</div>'; //Glossary_filewrite
                        
                        $(".GlossaryFilebox").html(dHtml);
                        $(".GlossaryFilebox").show();


                    }
                },
                error: function (result) {
                    if (result.status != 0)
                        alert("Error" + ":::" + result);
                },
                complete: function () {
            }
        });
    }

    function StringFormat() {
        var expression = arguments[0];
        for (var i = 1; i < arguments.length; i++) {
            var prttern = "{" + (i - 1) + "}"
            expression = expression.replace(prttern, arguments[i]);
        }

        return expression;
    }

    function chkAll(all) {

        var cb = document.getElementsByName("chkFile");
        var boo = false;

        if (all.checked) boo = true;
        for (var i = 0; i < cb.length; i++) {
            cb[i].checked = boo;
        }

    }

    function fnPrint()
    {
        var w = "1000";
        var h = "710";

        //중앙위치 구해오기
        var LeftPosition = (screen.width - w) / 2;
        var TopPosition = (screen.height - h) / 2;
        var options = "top=" + TopPosition + ",left=" + LeftPosition + ",width=" + w + ", height=" + h + ", toolbar=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no";
        window.open("/Glossary/GlossaryPrint_View.aspx?ItemID=<%=ItemID%>", "_blank", options);
        
    }

    function SendThanksLetter()
    {
        var w = "1400";
        var h = "900";

        //중앙위치 구해오기
        var LeftPosition = (screen.width - w) / 2;
        var TopPosition = (screen.height - h) / 2;
        var options = "top=" + TopPosition + ",left=" + LeftPosition + ",width=" + w + ", height=" + h + ", toolbar=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no";
        window.open("http://portal.sktelecom.com/thanksletter/Main/LetterWrite.aspx?skinType=type1 ", "_blank", options);

    }
    function ShowTitle(type, gbn)
    {
        if (type == "Like") {
            if (gbn == "IN") {
                $('#LikeMessageArea').attr("style", "font-size:12px; display:;");
            }
            if (gbn == "OUT") {
                $('#LikeMessageArea').attr("style", "display:none;");
            }
        }

        //if (type == "Thanks") {
        //    if (gbn == "IN") {
        //        $('#ThanksMessageArea').attr("style", "padding-left:140px; font-size:12px; display:;");
        //    }
        //    if (gbn == "OUT") {
        //        $('#ThanksMessageArea').attr("style", "display:none;");
        //    }
        //}
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Common_Top" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hidFilePathGuid" runat="server" />
    <asp:HiddenField ID="hidFileDeleteKey" runat="server" />
    <input type="hidden" id="hidChkAllFlag" name="hidFileChkFlag" runat="server" value="N" />
    <asp:Button ID="btnMulti" runat="server" OnClick="btnMulti_Click"  Style="display: none" />
    <input type="hidden" id="hidGlossaryID" name="hidGlossaryID" runat="server" />
    <input type="hidden" id="hidFileChkAttachID" name="hidFileChkAttachID" runat="server" />
   
<script type="text/javascript">
    var lnbDep1 = 1;		//LNB 1depth
</script>
	<!--CONTENTS-->
	<div id="contents">
		<div class="h2tag">
            <% if (String.IsNullOrEmpty(WType) && String.IsNullOrEmpty(TType)) { %>
            <img src="/common/images/text/Glossary.png" alt="끌.지식" />
            <% } %>
		</div>
		<p class="btn_top">
             <% if (String.IsNullOrEmpty(WType) && String.IsNullOrEmpty(TType)) { %>
            <a href="/Glossary/GlossaryWriteNew.aspx"><img src="/common/images/btn/write.png" alt="글쓰기" /></a>
            <% } %>
        <div id="article" class="viewArea" <%if (GatheringYN == "Y") { Response.Write("style = \"padding-top:60px;\");"); }%>>
           
			<!--상세보기-->
            <div class="view_title_g">
			<h3><span id="txtTitle"><asp:Literal ID="lbTitle" runat="server"></asp:Literal></span></h3>
            <p id="btnScrap" class="btn_scrap <%= ScrapOn %>"><a href="javascript:" class="btn_pop" onclick="Validity('Scrap');"><b><asp:Literal ID="litScrap" runat="server"></asp:Literal></b></a></p>            
            </div>
			<p class="view_writer">
                <asp:Literal ID="litFirstUser" runat="server"></asp:Literal>, <asp:Literal ID="litLastUser" runat="server"></asp:Literal><asp:Literal ID="litFromQna" runat="server"></asp:Literal>
			</p>
			<p class="view_num" style="right:50px;">
                <span style="display:none">최초작성 <asp:Label runat="server" ID="lbDate"></asp:Label></span>
                <span>조회 <asp:Label runat="server" ID="lbHitCount"></asp:Label></span>
                <%--<span>추천 <asp:Label runat="server" ID="lbLikeCount"></asp:Label></span>--%>
                <span>스크랩수 <asp:Label runat="server" ID="lbScrapCount"></asp:Label></span>
                <span>편집횟수 <asp:Label runat="server" ID="lbHistorycount"></asp:Label> </span>
                <span style="display:none">감사표시 횟수 <asp:Label runat="server" ID="lbLikeCounts">0</asp:Label></span>
			</p>
			<div class="view_ct_area">
				<div class="view_ct">
					<asp:Label ID="txtBody" runat="server"></asp:Label>
				</div>
				<dl class="tag_box" runat="server" id="TagLists">
					<dt>태그</dt>
				</dl>
				<div class="GlossaryFilebox" style="display:none;">
                </div>
                <!--// CHG610000074852 / 20181108 / T생활백서  -->
				<div class="thanksArea">
                    <a id="LikeAdd" runat="server" href="javascript:Validity('Like');" class="btnLike" onmouseover="return ShowTitle('Like','IN');" onmouseout="return ShowTitle('Like','OUT');"><span id="LikeCnt">추천하기</span></a>
                    <a id="LetterAdd" runat="server" href="javascript:SendThanksLetter()" class="btnTletter" title="정보가 유용하다면 Thanks Letter를 보내주세요."><span>Thanks Letter</span></a>
                    <br />
                    <span id="LikeMessageArea" style="display:none;"></span>
                    <span id="ThanksMessageArea" style="display:none;">정보가 유용하다면 Thanks Letter를 보내주세요.</span>
				</div>
				<p class="btn_r">
                    <%--<%if(this.GatheringYN == "Y" && this.AuthorYN == "Y"){ %><a href="javascript:;" onclick="pop_authority_Open();" class="btn2"><b>공유하기</b></a>&nbsp<%} %>--%>
                    <a href="javascript:fnPrint();" class="btn2"><b>인쇄하기</b></a>
                    <asp:LinkButton ID="btnTikleDelete" Class="btn2" runat="server" OnClientClick="return fnTikleDelete();" OnClick="btnTikleDelete_Click"><b>삭제하기</b></asp:LinkButton>
                    <a href="javascript:;" onclick="pop_authority_Open();" class="btn2"><b>공유하기</b></a>
                    <a href="javascript:fnPrevListUrl();" class="btn2"><b>목록보기</b></a>
                    <% 
                        //특정 태그 일경우 에는 지정사용자만 수정 가능
                        if (WTypeWrite)
                        {
                            Response.Write("<a href=\"javascript:Validity('HistoryList');\" class=\"btn2\"><b>편집내역보기</b></a>&nbsp;");
                            Response.Write("<a href=\"javascript:Validity('HistorySave');\" class=\"btn3\"><b>편집하기</b></a>");
                        }
                        else
                        {
                            Response.Write("<a href=\"javascript:Validity('HistoryList');\" class=\"btn2\"><b>편집내역보기</b></a>&nbsp;");
                            Response.Write("<a href=\"javascript:Validity('HistorySave');\" class=\"btn3\"><b>편집하기</b></a>");
                        }
                    %>
				</p>
			</div>
            
            <Common:Comment ID="CommCommentControl" runat="server"/>
			<!--/상세보기-->
		</div>
	</div>
	<!--/CONTENTS-->
<div id="divPop" class="pop" style="display:none">
	<div id="divPopBg" class="popBg"></div>
	<!--알럿 팝업-->
	<div id="pop_alert" class="layer_pop" style="display:none">
		<dl>
			<dt>스크랩되었습니다!</dt>
			<dd>스크랩 된 지식은 <b class="point_red">My Page</b>에서<br />확인 할 수 있습니다.</dd>
		</dl>
		<p class="btn_c">
			<a href="javascript:" onclick="return fnScrapCancle();" class="btn2"><b>취소하기</b></a>
			<a href="javascript:" onclick="fnMyScrapListView();" class="btn3"><b>확인하기</b></a>
		</p>
		<img src="/common/images/btn/pop_close.png" title="닫기" class="close" onclick="return fnScrapClose();"/>
	</div>
	<!--/알럿 팝업-->
    <!--지식_조회 권한 설정하기-->
	<div id="pop_authority" class="layer_pop" style="display:none;">
        <%if (GatheringYN == "Y") { %>
		    <h3>조회 권한을 설정해 주세요!</h3>
        <%}else{%>
            <h3>공유하기</h3>
        <%} %>
		    <fieldset class="authority" style="width:300px;">
                <%if (GatheringYN == "Y") { %>
			        <p style="padding-top:10px;"> 
				        <input id="FullPublic" name="rBtnA" class="radio" type="radio" value="FullPublic" onclick="PermisstionValCheck()" checked="checked" style="padding-top:4px; vertical-align:4px; vertical-align: -17px;" /><label for="FullPublic"> 전사 지식으로 내보내기</label><br />
                        <input id="SomePublic" name="rBtnA" type="radio" class="radio" value="SomePublic" onclick="PermisstionValCheck()" style="padding-top:4px;margin-left: 2px; vertical-align: -17px;" /><label for="SomePublic" > 특정 사람과 같이 보기&nbsp;&nbsp;</label>
			        </p>
                    <div style="width:100%; text-align:center;">
                        <asp:DropDownList ID="GatheringDDL" CssClass="select" Width="266px" AutoPostBack="false" runat="server"  style="display:none; margin-top:10px;" ></asp:DropDownList>
                    </div>
                <%}else{%>
                    <p style="padding-top:10px;"> </p>
                <%} %>
                <p style="padding-top:10px;"> </p>
                <common:UserAndDepartment ID="UserControl" runat="server" />
		    </fieldset>
		    <p class="btn_c">
                <%if (GatheringYN == "Y") { %>
			        <a href="javascript:" class="btn3" onclick="pop_authority_Save()" id="btnSend"><b>공유하기</b></a>
                <%}else{%>
                    <a href="javascript:" class="btn3" onclick="pop_noti_Save()" id="btnSend"><b>쪽지발송</b></a>
                <%} %>
		    </p>
        <img src="/common/images/btn/pop_close.png" title="닫기" class="close" onclick="return pop_authority_Close();"  style="cursor:pointer;" />
        <br /><p id="divImgLoading" style="display:none; text-align:center; " >쪽지 발송중...</p>
	</div>
	<!--/지식_조회 권한 설정하기-->
	<p class="btn_c">
        <a href="javascript:sendThanksLetter();" class="btn3"><b>보내기</b></a>
	</p>
	<img src="/common/images/btn/pop_close.png" title="닫기" alt="닫기" class="close" style="cursor:pointer;" onclick="hideThanksLetter();" />
</div>

<script>
    
    //$(".schedule_add").hide();
    //$("#authorityNateOnBiz").attr("style", "display:block;");
</script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Common_Footer" runat="server">
    <span style="display:none;">
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click"/>
        <asp:Button ID="btnNotiSend" runat="server" OnClick="btnNotiSend_Click"/>
    </span>
    <asp:HiddenField ID="hdPermissionRtnValue" runat="server" />
    <asp:HiddenField ID="hdTagValue" runat="server" />
    <asp:HiddenField ID="hdTagTemp" runat="server" />
    <asp:HiddenField ID="hdfPrevListUrl" runat="server" />

    <asp:HiddenField ID="hdPermissions" runat="server" />
    <asp:HiddenField ID="hdPermissionsString" runat="server" />
    <asp:HiddenField ID="hdPermisstionCheck" runat="server" />    
    <asp:HiddenField ID="txtTag" runat="server" />    
    

    

</asp:Content>
