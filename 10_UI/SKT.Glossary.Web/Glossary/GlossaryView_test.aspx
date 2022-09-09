<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossarySearch.Master" AutoEventWireup="true" CodeBehind="GlossaryView_test.aspx.cs" Inherits="SKT.Glossary.Web.Glossary.GlossaryView_test" %>

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

    <%--<script src="../Common/Controls/DextUploadFl/DEXTUploadFL.js" type="text/javascript"></script>--%>

    <script src="../Common/Js/dhtmlHistory.js"></script>

       <!-- 기본 CSS 및 JS 정의 -->        
    <link href="/Common/Css/board_G.css" rel="stylesheet" />
    <!-- Script -->
   <%--<script type="text/javascript" src="/Common/Js/jquery-1.11.2.js"></script>--%>
    <script type="text/javascript" src="/common/Js/TnetBoard_Control.js"></script>
    <!-- 디자인팀적용 JS -->
    <script type="text/javascript" src="/Common/Js/css.browser.detect.js"></script>

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

    //    $(".popup").click(function () {
    //        $("div[class^='view-func-']").hide();
    //        $("div[class='view-func-" + $(this).attr('target') + "-outer']").show();
    //    });
    var m_Totorial = '<%=TutorialYN %>';
    var TagCheck = false;   // 2014-06-23 Mr.No


    function initialize() {
        // 초기화
        dhtmlHistory.initialize();
        // historyfunc 가 콜백함수이다
        dhtmlHistory.addListener(historyfunc);
    }

    function historyfunc() {
        if (historyStorage.get('state')) {
            history.back();
            if ($('#<%= this.txtTag.ClientID %>').val() == "") {
                $("#<%= this.TagLists.ClientID %>").html(historyStorage.get('state'))
            }
        }
    }

    // 2014-07-10 Mr.No

    function checkForHash() {
        if (document.location.hash) {

            var HashLocationName = document.location.hash;
            HashLocationName = HashLocationName.replace("#", "");

            if ($('#<%= this.txtTag.ClientID %>').val() == "") {
                $("#<%= this.TagLists.ClientID %>").html(HashLocationName)
                document.location.hash = "";
                document.location.hash = document.location.hash.replace("#", "");
            } else {
                // Tagset()
            }
        }
    }

    $(document).ready(function () {
        // 2014-07-10 Mr.No
        //checkForHash();


        if ("<%=GatheringYN%>" == "Y") {
            $("#container").attr('class', 'Gathering');
            $("#tabMenu").css('padding-bottom', '5px');
            $("#btnScrap").css({ 'top': '120px', 'right': '50px' });

            PermissionsStringThrow();

            //$('#UserAndDepartment').hide();
        }
        else {

        }

        $("#<%= GatheringDDL.ClientID %>").change(function () {
            var GatheringID = $(this).find(":selected").val();
            var GatheringName = $(this).find(":selected").text();

            if (GatheringID != "") {
                pushToArySave(GatheringName, GatheringID, "M");
            }
        });

        initialize();

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

        // Nr.No 태그 기능 추가
        //////////////////////////////////////Tag 자동 검색///////////////////////////////////////////////////////////////////

        $("#<%= this.txtTag.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "/Common/Controls/AjaxControl.aspx/GetAutoCompleteTagData",
                    data: "{'username':'" + $('#<%= this.txtTag.ClientID %>').val() + "'}",
                    dataType: "json",
                    success: function (data) {
                        if ($('#<%= this.txtTag.ClientID %>').val().replace(/^\s+|\s+$/g, '') == "") {
                            return;
                        } else {
                            response(data.d);
                        }
                    },
                    error: function (result) {
                        //alert("Error Tag");
                        alert('허용하지 않는 문자입니다.');
                        $("#<%= this.txtTag.ClientID %>").val("");
                    }
                });
            }
        });



        if ($('#<%= this.txtTag.ClientID %>').val() != "") {
            TagSet();
        }

    });

    //스크랩 해제
    $(document).click(function (event) {
        /*
        if ($(".scrap")[1].innerText == "스크랩 해제") {
            $(".view-func-scrap-outer").hide();
        }
*/
    });

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
                        location.href = "/Glossary/GlossaryWrite.aspx?mode=History&ItemID=" + m_ItemID + "&CommonID=" + m_CommonID + "&GatheringYN=<%=this.GatheringYN %>&GatheringID=<%=this.GatheringID %>";
                        /*
                        if (ModifyYNTableCnt.d == "0") {
                            alert("수정 중인 문서 입니다.");
                            return false;
                        } else {
                            
                        }
                        */
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

                    var beforeCount = parseInt($('#<%= this.lbLikeCounts.ClientID %>').text());
                $.ajax({
                    type: "POST",
                    url: "/Common/Controls/AjaxControl.aspx" + "/GlossaryLikeInsert",
                    data: "{UserID : '" + m_UserID + "', GlossaryID : '" + m_CommonID + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {

                        var d = data.d;

                        //{$(.text('이미 감사의 마음을 남기셨습니다.');
                        $('#<%= this.LikeAdd.ClientID %>').attr("href", "javascript:confirmLikeYn();");
                        $('#thanksArea img').removeAttr(onclick);

                        if (d.TotalCount == 0) {
                            $('#<%= this.lblLikeText01.ClientID %>').text(d.LatestUserName + '님이 이 지식에 감사의 마음을 남기셨습니다.');
                    	} else {
                    	    $('#<%= this.lblLikeName.ClientID %>').text(d.LatestUserName);
                    	    $('#<%= this.lblLikeText01.ClientID %>').text('님 외');
                    	    $('#<%= this.lblLikeNum.ClientID %>').text(d.TotalCount);
                    	    $('#<%= this.lblLikeText02.ClientID %>').text(' 명이 이 지식에 감사의 마음을 남기셨습니다.');
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
                confirmLikeYn();
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
                //window.location = "/GlossaryHistory/HistoryList.aspx?ItemID=" + m_ItemID + "&SearchKeyword=" + '<%=GetSearchKeyword()%>' + "&GatheringYN=<%=GatheringYN%>&GatheringID=<%=GatheringID%>";
                window.location = "/GlossaryHistory/HistoryList.aspx?ItemID=" + m_ItemID + "&GatheringYN=<%=GatheringYN%>&GatheringID=<%=GatheringID%>";
                break;
            default:
                break;
        }
    }

    function confirmLikeYn() {
        $('#<%= this.LikeAdd.ClientID %>').text("이미 감사의 마음을 남기셨습니다.");
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
        return confirm('현재 티끌을 정말 삭제하시겠습니까?');
    }

    function fnTikleGoToHall() {
        return confirm('현재 티끌을 명예의 전당으로 보내시겠습니까?');
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


    // 2014-05-22

    //dextupload

    var downloadManager = null;

    // 다운로드 관리자에서 오류가 발생하면 호출됩니다.
    function onErrorForDEXTDOWNMAN(err) {
        if (err.code == "E20000117") {
            alert(err.message);
            return;
        }
        alert(err.code + "\r\n" + err.message + "\r\n" + err.detail);
    }

    // 다운로드 관리자가 준비되면 호출됩니다.
	<%--function onApplicationReadyForDEXTDOWNMAN() {
	    // 다운로드 관리자 객체를 얻습니다.
	    downloadManager = DEXTUploadFL.getDownloadManager("DEXTDOWNMAN");

	    // iis mimeType 추가 필요함. ".air" mimeType="application/vnd.adobe.air-application-installer-package+zip"
	    // AIR application 경로를 설정합니다.

	    // 절대경로
	    downloadManager.setDownloaderAppUrl("<%= System.Configuration.ConfigurationManager.AppSettings["BaseURL"].ToString() %>Common/Controls/DextUploadFL/DEXT_MULTI_DOWN_MONITOR.air");

        downloadManager.setUIStyle({
            buttonPanel: {
                toolButton: { visible: false, enabled: false }
                    , transferButton: { visible: false, enabled: false }
            },
            context: {
                edit: { visible: false, enabled: false },
                upload: { visible: false, enabled: false }
            },
            // 타이틀을 설정합니다.
            title: { text: "Multiple Downloading" },
            transferButton: { visible: false, enabled: true }// 다운로드 관리자 하단 버튼 영역 다운로드 버튼 속성
        });
        var downloadUrl = '<%= ConfigurationManager.AppSettings["DownloadControlServerHandlerUrl"]%>';
        var AttachInfoString = replaceAll('<%=this.AttachInfo %>', "//", "'");
        var AttachInfo = jQuery.parseJSON(AttachInfoString);
        if (AttachInfo.length > 0) {
            var fileRangeInfo = [];
            for (var i = 0; i < AttachInfo.length; i++) {
                fileRangeInfo.push({
                    id: AttachInfo[i].AttachID,
                    fileName: AttachInfo[i].FileName,
                    filePath: downloadUrl + "?Folder=Glossary&ItemGuid=" + AttachInfo[i].ItemGuid + "&FileName=" + encodeURIComponent(AttachInfo[i].FileName),
                    size: AttachInfo[i].FileSize
                });
            }

            // 다운로드할 파일을 설정합니다.
            downloadManager.addFileRange(fileRangeInfo);
        }
    }

    // 파일명을 더블클릭하면 호출됩니다. 
    function onFileDoubleClickForDEXTDOWNMAN(id) {
        var f = downloadManager.getFileInfoById(id);
        if (f && f.formType == "virtual" && f.filePath) {
            // DEXTUploadFL File Executor 응용 프로그램으로 파일 열기를 수행합니다.
            downloadManager.execute(id);
        }
    };--%>

    function replaceAll(sValue, param1, param2) {
        return sValue.split(param1).join(param2);
    }

    // Mr.No
    // Tag enter Event
    function txtTagEnterEvent() {
        if ($.trim($('#<%= this.txtTag.ClientID %>').val()) == "") {
            alert("연관단어로 공백을 넣을 수 없습니다."); return false;
        }
        var f = TagRedundancy_Check($.trim($('#<%= this.txtTag.ClientID %>').val())); // 입력한 글자

        if (f) {
            alert("연관단어에 중복된 값이 존재합니다."); return false;
        } else {
            TagSet();
            TagInsertAction($('#<%= this.hdTagTemp.ClientID %>').val());
            }

        }
        // onblur 기능 삭제 2014-06-30
        function txtTagFocusoutEvent() {
            TagSet();
            TagInsertAction($('#<%= this.hdTagTemp.ClientID %>').val());
        }

        // Tag Delete Animation
        function fnTagDelete(Thistag, ID) {
            IsEditMode = false;
            $(Thistag).remove();

            // 실제로 삭제하는 WebService 호출
            TagDeleteAction(ID);
        }
        // Tag Delete WebService
        function TagDeleteAction(ID) {
            $.ajax({
                type: "POST",
                url: "/Common/Controls/AjaxControl.aspx" + "/GlossaryTagDelete_One",
                data: "{ID : '" + ID + "'}",
                contentType: "application/json; charset=utf-8",
                async: false,
                success: function (ret) {
                }
            });
        }
        // Tag Update WebService
        function TagInsertAction(TagTitle) {
            if ($('#<%= this.hdTagTemp.ClientID %>').val() != "") {
                $.ajax({
                    type: "POST",
                    url: "/Common/Controls/AjaxControl.aspx" + "/GlossaryTagInsert_One",
                    data: "{TagTitle : '" + TagTitle + "', CommonID : '" + '<%= CommonID %>' + "', UserID : '" + '<%= UserID %>' + "', Title : '" + '<%= Title %>' + "'}",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: function (returnValue) {
                        if (TagTitle.replace(/^\s+|\s+$/g, '') != "") {
                            TagTitle = "<span class=\"tag-list\">" + returnValue.d[1].replace(/^\s+|\s+$/g, '') + "<a href=\"javascript:\" onclick=\"fnTagDelete(this.parentElement,'" + returnValue.d[0] + "'); \" >X</a></span>";
                            $('#<%= this.TagLists.ClientID %>').html($("#<%= this.TagLists.ClientID %>").html() + TagTitle);
                        }
                        $('#<%= this.hdTagTemp.ClientID %>').val("");
                    },
                    error: function (result) {
                        //alert("Error Tag");
                        alert('허용하지 않는 문자입니다.');
                        $("#<%= this.txtTag.ClientID %>").val("");
                    }
                });
            }
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
                        TagSearch = "<dd>" + TagSearchArr[i] + "</dd>";
                        $('#<%= this.TagLists.ClientID %>').html($("#<%= this.TagLists.ClientID %>").html() + TagSearch);
                }
            }
            // 2014-07-10 Mr.No

            if (historyStorage.get('state')) {
                history.back();
                var temp = $.trim($('#<%= this.TagLists.ClientID %>').html().replace("\n", ""));
                    AddHashInfo = historyStorage.get('state').replace(temp, "");
                    $('#<%= this.TagLists.ClientID %>').html($("#<%= this.TagLists.ClientID %>").html() + AddHashInfo);
                }
            }
            $('#<%= this.txtTag.ClientID %>').val("");
        }

        // 2014-06-17 Mr.No  // 2015-10-27 ksh
        function fnPrevListUrl() {
            if (document.referrer.indexOf("write") > -1) {
                history.back(-1);
            } else if (document.referrer.indexOf("Platform") > -1 || "<%= PlatformYN%>" == "Y") {
            if ((document.referrer.indexOf("Glossary.aspx") > 0 || document.referrer.indexOf("GlossaryNewsList.aspx") > 0)) {
                history.back(-1);
            } else if (document.referrer.indexOf("Trend") > -1) {
                location.href("/Trend/PlatformBiz.aspx");
            } else {
                location.href("/Platform/GlossaryPlatform.aspx");
            }
        } else if (document.referrer.toUpperCase().indexOf("Marketing") > -1 || "<%= MarketingYN%>" == "Y") {
            if ((document.referrer.indexOf("Glossary.aspx") > 0 || document.referrer.indexOf("GlossaryNewsList.aspx") > 0)) {
                history.back(-1);
            } else {
                location.href("/Marketing/GlossaryMarketing.aspx");
            }
        } else if (document.referrer.toUpperCase().indexOf("Trend") > -1 || "<%= TechTrendYN%>" == "Y") {
            if ((document.referrer.indexOf("Glossary.aspx") > 0 || document.referrer.indexOf("GlossaryNewsList.aspx") > 0)) {
                history.back(-1);
            } else {
                location.href("/Trend/TechTrend.aspx");
            }
        } else if (document.referrer.toUpperCase().indexOf("TNET") > -1) {
            if ((document.referrer.indexOf("Glossary.aspx") > 0 || document.referrer.indexOf("GlossaryNewsList.aspx") > 0)) {
                history.back(-1);
            } else {
                location.href("/Glossary/GlossaryNewsList_Tnet.aspx?TagTitle=&SearchSort=CreateDate&GatheringYN=<%=this.GatheringYN%>&GatheringID=<%=this.GatheringID%>");
            }
        }
        else {
            location.href("/Glossary/GlossaryNewsList.aspx?TagTitle=&SearchSort=CreateDate&GatheringYN=<%=this.GatheringYN%>&GatheringID=<%=this.GatheringID%>");
        }
}

// 2014-06-23 Mr.No
function btnlistAction() {
    if (TagCheck) {
        $("#aboutInput").hide();
        TagCheck = false;
        aboutInputContent = false;
        document.getElementById("btn_list").innerHTML = "입력창 열기";
    } else {
        $("#aboutInput").show();
        TagCheck = true;
        aboutInputContent = true;
        document.getElementById("btn_list").innerHTML = "입력창 닫기";
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

        try {
            //특정사람
            if ($('#SomePublic').attr("checked")) {
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
            else {
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

        try {

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
            <%if (GatheringYN == "Y")
              { %>
                <%--<a href="/Gathering/Main.aspx"><img src="/common/images/text/Gathering_text.png" alt="끌.모임" style="left: 50px; top: 30px; position: absolute; width: 83px; height: 26px;" /></a>--%>
                <common:GatheringInfomation ID="GatheringInfomation1" runat="server" />
            <%}else{ %>
            <img src="/common/images/text/Glossary.png" alt="끌.지식" />
        <%} %>
		</div>
		<p class="btn_top">
            <a href="/Glossary/GlossaryWriteSimple.aspx?GatheringYN=<%=GatheringYN%>&GatheringID=<%=GatheringID%>">
                <img src="/common/images/btn/write<%if(GatheringYN=="Y"){ %>_gathering<%}%>.png" alt="글쓰기" /></a></p>
		<div id="article" class="viewArea">
            <%if (GatheringYN == "Y")
              { %>            
                <common:GatheringMenuTab ID="GatheringMenuTabCtrl" runat="server" />
            <%} %>
			<!--상세보기-->
            <p id="btnScrap" class="btn_scrap <%= ScrapOn %>" style="right:50px;"><a href="javascript:" class="btn_pop" onclick="Validity('Scrap');"><b><asp:Literal ID="litScrap" runat="server"></asp:Literal></b></a></p>            
			<h3><span id="txtTitle"><asp:Literal ID="lbTitle" runat="server"></asp:Literal></span></h3>
            
			<p class="view_writer">
                <asp:Literal ID="litFirstUser" runat="server"></asp:Literal>, <asp:Literal ID="litLastUser" runat="server"></asp:Literal><asp:Literal ID="litFromQna" runat="server"></asp:Literal>
			</p>
			<p class="view_num" style="right:50px;">
                <span style="display:none">최초작성 <asp:Label runat="server" ID="lbDate"></asp:Label></span>
                <span>조회 <asp:Label runat="server" ID="lbHitCount"></asp:Label></span>
                <span>추천 <asp:Label runat="server" ID="lbLikeCount"></asp:Label></span>
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
				<div class="tag_input" style="display:none">
					<input type="text" runat="server" id="txtTag" class="txt t1"  /><a href="javascript:btnlistAction()" id="btn_list" >입력 창 열기</a>
					<ul>
						<li><a href="">Biz</a></li>
						<li><a href="">ntelligence</a></li>
						<li><a href="">Tech Intelligence</a></li>
						<li><a href="">SKT 상품/서비스</a></li>
                    </ul>
				</div>
                <%--<div id="manager_container" class="fileArea" ></div>--%>
                <%--<script type="text/javascript">
                    DEXTUploadFL.createDownloadManager(
                        "manager_container", // target div container
                        "DEXTDOWNMAN", // id
                        "../Common/Controls/DextUploadFl/DEXT_LIST_DOWN_MANAGER.swf", // swf path
                        "#ffffff", // background color
                        "transparent", // window, transparent
                        "", // ko, en
                        "", // reserved			
                        "multi", // single, multi
                        "ForDEXTDOWNMAN" // postfix name
                    );
                </script>--%>
                <div class="GlossaryFilebox" style="display:none;">
                    <%-- <SKTControls:filectrl ID="GlossaryfileCtrl" viewType="VIEW" runat="server" />--%>
                    <%--<div class="FileUploadCtrl">
                        <div class="FileUloadTable">
                            <table>
                                dHtml2
                            </table>
                        </div>
                        <div id="divmulti">
                            <p class="writeBox_text">
                                전체 : <asp:Literal runat="server" ID="multi_filecount"></asp:Literal> (<asp:Literal runat="server" ID="multi_filesize"></asp:Literal>)
                            </p>
                            <a href="javascript:///" class="btnB" onclick="fnMultiDown('<%= CommonID %>', 'chkFile');" id="btnMultidown">
                                <span>다운로드</span>
                            </a>
                        
                        </div>
                    </div>--%>
                </div>
                <div class="thanksArea">
                <img src="../Common/images/icon/caution.png" onclick="javascript:Validity('Like');"/>
				<dl class="thanks">
                    
                        <%--<img src="../Common/images/icon/caution.png"/>--%>
                    
					<dt>
                        <a id="LikeAdd" runat="server" href="javascript:Validity('Like');"><font color="#FF5E00"> [추천하기]</font> 이 지식이 도움이 되었나요?</a>
                        <a id="LikeCount" runat="server" href="javascript:">이미 감사의 마음을 남기셨습니다.</a>
					</dt>
					<dd>
                        <asp:Label ID="lblLikeName" class="csLikeName" runat="Server" /><asp:Label ID="lblLikeText01" class="csLikeText01" runat="server" />
                        <asp:Label ID="lblLikeNum" class="csLikeNum" runat="server" />
                        <asp:Label ID="lblLikeText02" class="csLikeText02" runat="server" />
					</dd>
				</dl>
                </div>
				<p class="btn_r">
                    <%--<%if(this.GatheringYN == "Y" && this.AuthorYN == "Y"){ %><a href="javascript:;" onclick="pop_authority_Open();" class="btn2"><b>공유하기</b></a>&nbsp<%} %>--%>
                    <asp:LinkButton ID="btnTikleDelete" Class="btn2" runat="server" OnClientClick="return fnTikleDelete();" OnClick="btnTikleDelete_Click"><b>삭제하기</b></asp:LinkButton>
                    <a href="javascript:;" onclick="pop_authority_Open();" class="btn2"><b>공유하기</b></a>&nbsp
                    <a href="javascript:fnPrevListUrl();" class="btn2"><b>목록보기</b></a>
                    <a href="javascript:Validity('HistoryList');" class="btn2"><b>편집내역보기</b></a>
                    <a href="javascript:Validity('HistorySave');" class="btn3"><b>편집하기</b></a>
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
                        <asp:DropDownList ID="GatheringDDL" CssClass="select" Width="266px" AutoPostBack="false" runat="server" OnSelectedIndexChanged="GatheringDDL_SelectedIndexChanged" style="display:none; margin-top:10px;" ></asp:DropDownList>
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
		<%--<input type="image" src="/common/images/btn/pop_close.png" title="닫기" class="close" onclick="return pop_authority_Close()" tabindex="-999" />--%>
        <br /><p id="divImgLoading" style="display:none; text-align:center; " >쪽지 발송중...</p>
	</div>
	<!--/지식_조회 권한 설정하기-->
</div>

    <script>
        $(".schedule_add").hide();
        $("#authorityNateOnBiz").attr("style", "display:block;");
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
</asp:Content>
