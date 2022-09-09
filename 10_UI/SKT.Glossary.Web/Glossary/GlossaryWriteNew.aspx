<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossarySearch.master" AutoEventWireup="true" CodeBehind="GlossaryWriteNew.aspx.cs" Inherits="SKT.Glossary.Web.Glossary.GlossaryWriteNew" ValidateRequest="false" %>
<%@ Register Src="~/Common/Controls/UserAndDepartmentList.ascx" TagName="UserAndDepartment" TagPrefix="common" %>
<%@ Register Src="~/Common/Controls/GatheringInfomation.ascx" TagName="GatheringInfomation" TagPrefix="common" %>
<%@ Register Src="~/Common/Controls/GatheringMenuTab.ascx" TagName="GatheringMenuTab" TagPrefix="common" %>
<%@ Register assembly="SKT.Tnet" namespace="SKT.Tnet.Controls" tagprefix="SKTControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">
    <style type="text/css">
        .FileUloadTable {width:620px;}
    </style>
    <link href="/Common/Css/board_G.css" rel="stylesheet" />
    <script src="/common/js/design.js" type="text/javascript"></script>
    <script src="/Common/js/json2.js" type="text/javascript"></script>
    <script type="text/javascript" src="/common/Js/TnetBoard_Control.js"></script>
    <script type="text/javascript" src="/Common/Js/css.browser.detect.js"></script>
    <script type="text/javascript">
        var m_mode = '<%= mode %>';
        var m_focus = false;
        var TempID = null;
        var Radio = "write-tem-etc";
        var m_titleCheck = false;
        var WriteTitleSearchTemp = false;
        // 2014-05-12 Mr.NohasChanged
        var ShareOpen = true;
        var m_ItemID = '<%= ItemID %>';
        var m_UserID = '<%= UserID %>';
        // 2014-06-13 Mr.No
        var Temp_EditorContents = null;
        var TagCheck = false;   // 2014-06-23 Mr.No
        var GlossaryWirteObj = {};

        var m_WType = '<%= WType %>'; //DT 블로그홈
        var m_TType = '<%= TType %>'; //생활백서

        var validNavigation = false;
        function ResetValidNavigation() {
            validNavigation = false;
        }
        //var DefaultTitle = "지식제목을 입력하세요 (질문은 끌.질문을 이용해 주세요)";
        var DefaultTitle = "지식제목을 입력하세요.";

        $(document).ready(function () {
            
            <%if (GatheringYN == "Y"){ %>
            $("#txtTitle").val("모임글 제목을 입력하세요.");
            <%} else {%>
            $("#txtTitle").val(DefaultTitle);
            <%} %>

            $(window).on('beforeunload', function (e) {
                if (!validNavigation) {
                    validNavigation = false;
                    return "페이지를 이동하면 작성중인 내용이 사라집니다 정말 이동하시겠습니까?";
                }
            });

            $("a").click(function (e) {
                validNavigation = true;
                //$(window).off('beforeunload');
                window.setTimeout("ResetValidNavigation()", 10);
            });

            if ($.trim(m_ItemID).length > 0)
            {
                if (m_mode == "MyTemp") {
                    $("#spSave").html("저장하기");
                    $("#txtTitle").val($('#<%= this.hdTitle.ClientID %>').val());
                }
                else {
                    $("#txtTitle").val($('#<%= this.hdTitle.ClientID %>').val());
                    $("#spSave").html("편집완료");
                }
            }
            else {
                $("#spSave").html("저장하기");
            }

            //2015-11-09 임원만 보기 전체보기 구분
            var TechTrendYN = document.referrer.indexOf("TechTrend.aspx");
            if (TechTrendYN > 0) {
                $("#techtrendwriter").attr("style", "display:;");
            }

            if ("<%=GatheringYN%>" == "Y") {
                $("#container").attr('class', 'Gathering');
                $("#btnSetPermission").hide();
            }

            if ($('#<%= this.hdPermissions.ClientID %>').val() == "SomePublic") {
                $("#FullPublic").attr("checked", false);
                $("#SomePublic").attr("checked", true);

                PermisstionValCheck();
                //모임일 경우 여기 체크
                PermissionsStringThrow();
            }
            else {
                $('#UserAndDepartment').hide();
            }

            //닉네임
            $('#<%= this.txtNickName.ClientID %>').attr('style', 'display:none');
            $('#<%= this.NoneNick.ClientID %>').attr("checked", true);


            function fnSearchWriteTitleWord() {

                var RetStr = $('#txtTitle').val();

                if (RetStr.lastIndexOf("\\") != -1) {
                    if (RetStr.lastIndexOf("\\") % 2 == 0) {
                        RetStr = $('#txtTitle').val() + "\\";
                    }
                }

                if (RetStr.lastIndexOf("\'") != -1) {
                    RetStr = RetStr.replace(/\'/gi, "&#39;");
                }

                if (RetStr.lastIndexOf("\"") != -1) {
                    RetStr = RetStr.replace(/\"/gi, "&quot;");
                }

                RetStr = RetStr.replace(/\\/gi, "");

                return RetStr;
            }

            //$("#txtTitle").autocomplete({
            //    source: function (request, response) {
            //        $.ajax({
            //            type: "POST",
            //            contentType: "application/json; charset=utf-8",
            //            url: "/Common/Controls/AjaxControl.aspx/GetAutoCompleteWriteTitleData",
            //            data: "{'username':'" + fnSearchWriteTitleWord() + "'}",
            //            dataType: "json",
            //            success: function (data) {
            //                if ($('#txtTitle').val().replace(/^\s+|\s+$/g, '') == "") {
            //                    return;
            //                }
            //                else {
            //                    if ($('#txtTitle').val().length < 2) {
            //                        return;
            //                    }
            //                    response(data.d);
            //                }
            //            },
            //            error: function (result) {
            //                alert("Error Title");
            //            }
            //        });
            //    }
            //    , messages: {
            //        noResults: '',
            //        results: function () { }
            //    }
            //});

            //검색어 리스트 엔터키 검색 리스트
            $('#txtTitle').keydown(function (e) {
                WriteTitleSearchTemp = true;
                if (e.keyCode == 220) {
                }

                if (e.keyCode == 13) {
                    return false;
                }
            });

            ////제목 검색 목록에서 클릭 할경우 이벤트
            //$('.ui-autocomplete').click(function (e) {
            //    if (WriteTitleSearchTemp == true) {
            //        WriteTitleSearchTemp = false;
            //    }
            //});

            //수정 모드일 경우
            if (m_ItemID.length == 0) {
           
                $("#txtTitle").one("click", function () {
                    $("#txtTitle").val("");
                });
            }

            if (m_mode == "MyTemp")
            {
                if($("#txtTitle").val() == DefaultTitle)
                {
                    $("#txtTitle").one("click", function () {
                        $("#txtTitle").val("");
                    });
                }
            }

            
            //////////////////////////////////////제목 자동 검색끝///////////////////////////////////////////////////////////////////

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
                            $("#<%= ulDefaultTag.ClientID%>").hide();
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
                , messages: {
                    noResults: '',
                    results: function () { }
                }
            });

            if ($('#<%= this.txtTag.ClientID %>').val() != "") {
                TagSet();
            }

            //모임 태그 목록 가져오기
            //fnGetTagInfo();
        });

        function TagSet() {

            var TagSearch = $('#<%= this.txtTag.ClientID %>').val();
             if (TagSearch.indexOf(',') == -1) {
                 if (TagSearch.replace(/^\s+|\s+$/g, '') != "") {
                     TagSearch = "<li><a style=\"cursor:pointer;\" onclick=\"fnTagDelete(this.parentElement,'" + TagSearch + "');\">" + $('#<%= this.txtTag.ClientID %>').val().replace(/^\s+|\s+$/g, '') + "</a></span>";
                     $('#<%= this.TagLists.ClientID %>').html($("#<%= this.TagLists.ClientID %>").html() + TagSearch);
                }

                GlossaryWirteObj[$('#<%= this.txtTag.ClientID %>').val()] = $('#<%= this.txtTag.ClientID %>').val();

            } else {
                var TagSearchArr = [];
                TagSearchArr = TagSearch.split(",");
                for (var i = 0; i < TagSearchArr.length; i++) {
                    if (TagSearchArr[i].replace(/^\s+|\s+$/g, '') != "") {
                        TagSearch = "<li><a style=\"cursor:pointer;\" onclick=\"fnTagDelete(this.parentElement,'" + TagSearchArr[i] + "');\">" + TagSearchArr[i] + "</a></span>";
                        $('#<%= this.TagLists.ClientID %>').html($("#<%= this.TagLists.ClientID %>").html() + TagSearch);

                        GlossaryWirteObj[TagSearchArr[i]] = TagSearchArr[i];
                    }
                }
            }
            $('#<%= this.txtTag.ClientID %>').val("");
         }

        //////////////////////////////////////Tag 자동 검색  끝///////////////////////////////////////////////////////////////////


        //저장 버튼
        function fnSave() {

            /*
            Author : 개발자-김성환D, 리뷰자-이정선G
            Create Date : 2016.02.17 
            Desc : 끌지식 제목 필드 스크립트 제거
            */
            //$("#txtTitle").val(strip_tag($("#txtTitle").val()));
            $("#txtTitle").val(strip_tag($("#txtTitle").val().replace(/\\/gi, "")));

            //TitleDBCheck();
            if ("<%=this.GatheringYN%>" == "Y") {
                if ($("#txtTitle").val() == "모임글 제목을 입력하세요" || $("#txtTitle").val().replace(/^\s+|\s+$/g, '') == "") {
                    alert("모임글 제목을 입력하세요");
                    $("#txtTitle").unbind();
                    $("#txtTitle").val("");
                    $("#txtTitle").focus();
                    return false;
                }
            } else {
                if ($("#txtTitle").val() == DefaultTitle || $("#txtTitle").val().replace(/^\s+|\s+$/g, '') == "") {
                    alert(DefaultTitle);
                    $("#txtTitle").unbind();
                    $("#txtTitle").val("");
                    $("#txtTitle").focus();
                    return false;
                }
            }

            if (m_titleCheck == true) {
                alert('동일한 제목이 존재합니다 저장하실수 없습니다.');
                $("#txtTitle").focus();
                return false;
            }

            //if (!WebEditCheck("content")) { return false; }

            var activeBody = document.aspnetForm;
            activeBody.Wec.DefaultCharSet = "utf-8";
            $('#<%= this.hddActiveBodyText.ClientID %>').val($.trim(activeBody.Wec.TextValue));
 
            //CHG610000081447 / 이미지만 업로드 적용 / 2019-03-07 
            var tmpBody = activeBody.Wec.BodyValue;
            tmpBody = tmpBody.replace(/&nbsp;/gi, '');
            tmpBody = tmpBody.replace(/<br>/gi, '');
            tmpBody = tmpBody.replace(/ /gi, '');
            tmpBody = tmpBody.replace(/<p>/gi, '');
            tmpBody = tmpBody.replace(/<\/p>/gi, '');
            tmpBody = tmpBody.replace(/<pstyle=\"margin-bottom:2px;margin-top:5px\">/gi, '');
            tmpBody = tmpBody.replace(/<pstyle=\"margin-bottom:2px;margin-top:5px;\">/gi, '');
            tmpBody = tmpBody.replace(/<pstyle=\"margin-top:5px;margin-bottom:2px\">/gi, '');
            tmpBody = tmpBody.replace(/<pstyle=\"margin-top:5px;margin-bottom:2px;\">/gi, '');
            tmpBody = tmpBody.trim();

            if ($.trim(activeBody.Wec.TextValue).replace(/^\s*|\s*$/g, "") == "" && tmpBody == "") {
            //if ($.trim(activeBody.Wec.TextValue) == "") {
                alert("내용을 입력해 주세요");
                return false;
            } else {
                $('#<%= this.hddActiveBody.ClientID %>').val(activeBody.Wec.MIMEValue);
            }
            
            $('#<%= this.hdTitle.ClientID %>').val($("#txtTitle").val());

            var TagTotal = "";
            for (var i = 0; i < $('#<%= this.TagLists.ClientID %> li').length; i++) {
                //TagTotal += $(".tag-list")[i].innerText + ",";
                if (i > 0) TagTotal += ",";
                TagTotal += $('#<%= this.TagLists.ClientID %> li')[i].innerText;
            }

            if (TagTotal == "") {
                alert("태그를 입력하세요");
                return false;
            }

            $('#<%= this.hdTag.ClientID %>').val(TagTotal);

            
            if ($('#<%= this.HaveNick.ClientID %>').attr("checked") == "checked") {
                var texBox = document.getElementById("<%= txtNickName.ClientID %>");
                if (texBox.value == "닉네임 설정" || texBox.value.replace(/^\s+|\s+$/g, '') == "") {
                    alert("닉네임을 입력해주세요");
                    texBox.value = "";
                    texBox.focus();
                    return false;
                } else {
                    $('#<%= this.hdUserNikName.ClientID %>').val($('#<%= this.txtNickName.ClientID %>').val());
                }
            }

            // 2014-05-12 Mr.No
            PermissionsCheck();
            fnShareSave();

            var permissionValue = $(":input:radio[name=rBtnA]:checked").val();
            if (permissionValue == "SomePublic") {
                //조직도 
                if (fnCheckValue()) {
                    alert("일부공개시 참여할 구성원을 선택해주세요");
                    return false;
                }

               
            }

            //2015-09-22 platform 플래그 추가
            var platformYN = document.referrer.indexOf("GlossaryPlatform.aspx");
            if (platformYN > 0) {
                $("#<%=hddPlatformYN.ClientID%>").val("Y");
            }

            //2015-10-12 마케팅 플래그 추가
            var MarketingYN = document.referrer.indexOf("GlossaryMarketing.aspx");
            if (MarketingYN > 0) {
                $("#<%=hddMarketingYN.ClientID%>").val("Y");
            }

            //2015-10-27 관리자 플래그 추가
            var MarketingYN = document.referrer.indexOf("GlossaryMarketing.aspx");
            if (MarketingYN > 0) {
                $("#<%=hddMarketingYN.ClientID%>").val("Y");
            }

            //2015-10-12 마케팅 플래그 추가
            var TechTrendYN = document.referrer.indexOf("TechTrend.aspx");
            if (TechTrendYN > 0) {
                $("#<%=hddTechTrendYN.ClientID%>").val("Y");
            }

            //Editor 아웃 이벤트 제거
            $(window).off('beforeunload');
            <%=Page.GetPostBackEventReference(btnSave) %>;

        }

        function fnTagDelete(Thistag, TagSearch) {
            $(Thistag).remove();
            delete GlossaryWirteObj[TagSearch.toUpperCase()];
        }


        function TitleDBCheck() {
            //20140207 제목 어퍼스트로피 있을 시 에러, 처리
            var txtTitle = $('#txtTitle').val().replace(/'/g, "&#39;");
            var cid = '<%= CommonID %>';
            if ("<%=this.GatheringYN%>" == "Y") {
                try {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "/Common/Controls/AjaxControl.aspx/ExistTitleGathering",
                        data: "{'Title':'" + txtTitle + "','ID':'" + cid + "','GatheringYN':'<%=this.GatheringYN%>','GatheringID':'<%=this.GatheringID%>'}",
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
        else {
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
    }

    // Mr.No 2014-04-29
    // CategoryType Edier Insert
    function CategoryContentsSelect(ID) {
        try {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Common/Controls/AjaxControl.aspx/CategoryContents",
                data: "{'ID':'" + ID.toString() + "'}",
                dataType: "json",
                async: false,
                success: function (data) {
                    oEditors.getById["ir1"].exec("SET_CONTENTS", [""]);
                    oEditors.getById["ir1"].exec("PASTE_HTML", [data.d[0]]);
                },
                error: function (result) {
                    alert("Error DB Check");
                    alert(result);
                }
            });
        }
        catch (exception) {
            alert('Exception error' + exception.toString());
        }
    }
    
    function PermissionsStringThrow() {
        DefaultSetting($('#<%= this.hdPermissionsString.ClientID %>').val());
    }
    //2014-05-12 Mr.No
    function PermissionsCheck() {
        $('#<%= this.hdPermissions.ClientID %>').val($(":input:radio[name=rBtnA]:checked").val());
    }

    function PermisstionValCheck() {
        var permissionValue = $(":input:radio[name=rBtnA]:checked").val();

        if (permissionValue == "SomePublic") {
            $("#UserAndDepartment").show();
        }
        else  {
            $("#UserAndDepartment").hide();

        }
    }

    function txtTagClickEvent() {
   
        if ($.trim($('#<%= this.txtTag.ClientID %>').val()) == "") {
            $("#<%= ulDefaultTag.ClientID%>").show();
        }
        else {
            $("#<%= ulDefaultTag.ClientID%>").hide();
        }
    }

    function txtTagSelectEvent(selectTag) {
 
        var f = $.trim(selectTag);

        if (GlossaryWirteObj[f] == undefined) {
            TagRedundancy_Check($.trim(selectTag));
            $("#<%= ulDefaultTag.ClientID%>").hide();
        } else {
            alert("연관단어에 중복된 값이 존재합니다.");
            $("#<%= ulDefaultTag.ClientID%>").hide();
            return false;
        }
    }
    // Chrome 에서 submit 현상때문에 밖으로 뺌
    function txtTagEnterEvent() {

        if ($.trim($('#<%= this.txtTag.ClientID %>').val()) == "") {
            alert("연관단어로 공백을 넣을 수 없습니다."); return false;
        }

        if ($.trim(m_ItemID).length == 0) //신규모드
        {
            TagRedundancy_Check($.trim($('#<%= this.txtTag.ClientID %>').val()));
            return;
        }

        var f = $.trim($('#<%= this.txtTag.ClientID %>').val());

        if (GlossaryWirteObj[f] == undefined) {
            GlossaryWirteObj[f] = f;
            TagRedundancy_Check($.trim($('#<%= this.txtTag.ClientID %>').val()));
        } else {
            alert("연관단어에 중복된 값이 존재합니다.");
            return false;
        }
    }


    // Tag Title Check
    function TagRedundancy_Check(TagTitle) {
        $.ajax({
            type: "POST",
            url: "/Common/Controls/AjaxControl.aspx" + "/Tag_Redundancy_Check",
            data: "{TagTitle : '" + TagTitle + "', CommonID : '" + '<%= CommonID %>' + "'}",
                contentType: "application/json; charset=utf-8",
                async: false,
                success: function (rtnValue) {
                    if ($.trim(m_ItemID).length == 0) //신규모드
                    {
                        if (rtnValue.d[0]) {
                            if (GlossaryWirteObj[rtnValue.d[0].toUpperCase()]) {
                                alert('연관단어에 중복된 값이 존재합니다.');
                                return false;
                            } else {
                                var TagSearch = "<li><a onclick=\"fnTagDelete(this.parentElement,'" + rtnValue.d[0] + "');\" style=\"cursor:pointer;\">" + rtnValue.d[0].replace(/^\s+|\s+$/g, '') + "</a></span>";
                                $('#<%= this.TagLists.ClientID %>').html($("#<%= this.TagLists.ClientID %>").html() + TagSearch);

                                GlossaryWirteObj[rtnValue.d[0].toUpperCase()] = rtnValue.d[0]; // 2014-07-08 Mr.No
                                $('#<%= this.txtTag.ClientID %>').val("");
                            }
                        }
                    }
                    else //수정모드
                    {
                        if (rtnValue.d[0]) {
                            alert("연관단어에 중복된 값이 존재합니다.");
                            return false;
                        } else {
                            var TagSearch = "<li><a style=\"cursor:pointer;\" onclick=\"fnTagDelete(this.parentElement,'" + rtnValue.d[1] + "');\">" + rtnValue.d[1].replace(/^\s+|\s+$/g, '') + "</a></span>";
                            $('#<%= this.TagLists.ClientID %>').html($("#<%= this.TagLists.ClientID %>").html() + TagSearch);

                             GlossaryWirteObj[rtnValue.d[1]] = rtnValue.d[1];
                             $('#<%= this.txtTag.ClientID %>').val("");
                        }
                    }
                },
            error: function (result) {
                //alert("Error Tag");
                alert('허용하지 않는 문자입니다.');
                $("#<%= this.txtTag.ClientID %>").val("");
                }
                , messages: {
                    noResults: '',
                    results: function () { }
                }
        });
    }

    function fnPreviewOpen() {

        window.open("GlossaryPop_View.aspx", "GlossaryPop_View", "height=600,width=800,toolbar=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=no");
    }

    function addZeros(n) {
        return (n < 10) ? '0' + n : (n < 100) ? '' + n : '' + n;
    }

    function pop_defaulttag_close() {
        $("#<%= ulDefaultTag.ClientID%>").hide();
    }

    var lnbDep1 = 1;		//LNB 1depth

    function fnNick(obj) {
        if (obj.value == "HaveNick") {
            $('#<%= this.NoneNick.ClientID %>').attr("checked", false);
            $('#<%= this.HaveNick.ClientID %>').attr("checked", true);
            $('#<%= this.txtNickName.ClientID %>').attr('style', 'display:');
        }
        else {
            $('#<%= this.HaveNick.ClientID %>').attr("checked", false);
            $('#<%= this.NoneNick.ClientID %>').attr("checked", true);

            $('#<%= this.txtNickName.ClientID %>').attr('style', 'display:none');
            $('#<%= this.txtNickName.ClientID %>').val("");

        }
    }

    function pop_Open(param) {

        if (param == "File") {
            if ($("#trFile").attr("style") == "display: none;")
                $("#trFile").attr("style", "display:")
            else
                $("#trFile").attr("style", "display:none")

            //$("#trNick").attr("style", "display:none")
            //$("#trAuth").attr("style", "display:none")
        }
        if (param == "Nick") {

            if ($("#trNick").attr("style") == "display: none;")
                $("#trNick").attr("style", "display:")
            else
                $("#trNick").attr("style", "display:none")

            //$("#trFile").attr("style", "display:none")
            //$("#trAuth").attr("style", "display:none")
        }

        if (param == "Auth") {
            //$("#trFile").attr("style", "display:none")
            //$("#trNick").attr("style", "display:none")

            if ($("#trAuth").attr("style") == "display: none;")
                $("#trAuth").attr("style", "display:")
            else
                $("#trAuth").attr("style", "display:none")
        }

    }

    function selectGatheringTag()
    {
        var selData = $("#ddlGatheringTag option:selected").val();

        if (selData != "") {
            $('#<%= this.txtTag.ClientID %>').val(selData);
            txtTagEnterEvent();
        }
    }

    function fnGetTagInfo() {
           
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Gathering/Main.aspx/GetGatheringTagList",
                data: "{GatheringID : '<%=GatheringID%>'}",
	            dataType: "json",
	            success: function (data) {
	                var Table = data.d.Table;

	                if (Table.length == 0)
	                {
	                    $(".tag_gathering_area").attr("style", "display:none;");
	                }

	                $("#ddlGatheringTag").get(0).options[0] = new Option("=== 모임태그 선택 ===", "");

	                for (var i = 0; i < Table.length; i++) {

	                    $("#ddlGatheringTag").get(0).options[i + 1] = new Option(Table[i].TagTitle, Table[i].TagTitle);
	                }

	            },
	            error: function (result) {
	                //alert("Error" + ":::" + result);
	            },
	            complete: function () {
	                //$("#imgLoading").hide();
	                //getFileList();
	            }
	        });
    }

     function inputLengthCheck(eventInput, type) {

         var inputText = $(eventInput).val();
         var inputMaxLength = $(eventInput).prop("maxlength");

        var j = 0;
        var count = 0;

        for (var i = 0; i < inputText.length; i++) {
            val = escape(inputText.charAt(i)).length;
            if (val == 6) {
                j++;
            }

            j++;

            if (j <= inputMaxLength) {
                count++;
            }
        }

        var msg = "";
        if (type == 'title')
            msg = "제목 글자수를 초과하였습니다. 50자 이내로 입력해주세요.";
        else
            msg = "태그명 글자수를 초과하였습니다. 50자 이내로 입력해주세요.";

        if (j > inputMaxLength) {
            alert(msg);
            $(eventInput).val(inputText.substr(0, count));
        }
     }

     function fnPrevListUrl() {

         var url = "";
         //Editor 아웃 이벤트 제거
         $(window).off('beforeunload');

         var tagTitle = encodeURIComponent(encodeURIComponent('<%=TagTitle%>'));

         if (m_mode == "MyTemp") {
             locatio.href = "/GlossaryMyPages/MyDocumentsList.aspx?ReaderUserID=<%=UserID%>";
         }
         else {

            <% if (!string.IsNullOrEmpty(this.WType)) { %>
                 url = "/Glossary/DigitalTrans.aspx?";
                 location.href = url + "TagTitle=" + tagTitle + "&SearchSort=<%=SearchSort%>&PageNum=<%=PageNum%>&WType=<%=WType%>&SchText=" + encodeURIComponent(encodeURIComponent("<%=SchText%>"));
            <% } else if (!string.IsNullOrEmpty(this.TType)) { %>
                 url = "/Glossary/WhitePaper.aspx?";
                 location.href = url + "TagTitle=" + tagTitle + "&SearchSort=<%=SearchSort%>&PageNum=<%=PageNum%>&TType=<%=TType%>&SchText=" + encodeURIComponent(encodeURIComponent("<%=SchText%>"));
             <% } else  { %>
                 url = "/Glossary/GlossaryNewsList.aspx?";
                 location.href = url + "TagTitle=" + tagTitle + "&SearchSort=<%=SearchSort%>&PageNum=<%=PageNum%>";
             <% }  %>
         }
     }
    // ***************************************************
    // 내용 : WebEdit Control의 내용 체크 함수
    // 파라매터
    //   - ContentID     : WebEdit Control의 ID 값
    //   - HiddenFieldID : WebEdit Control의 HFWriteID 값
    // ***************************************************
    function WebEditCheck(ContentID) {
        var wec = document.getElementById(ContentID);

        var content = wec.TextValue;
        content = content.replace("\r", "");
        content = content.replace("\n", "");
        content = content.trim();

        //2017-11-14 본문에 이미지만 있어도 저장될수 있도록 보완
        //g :전역 검색, 첫 번째 일치 결과에서 멈추지 않고 전체 문자열에 대해 패턴 검색  
        //i :대소문자를 구분하지 않음 
        //m :여러줄 문자열에서 시작과 끝을 의미하는 특수 문자(^과$)를 각줄에 적용 

        //alert( wec.MIMEValue ) ; 
        var tmpBody = $.trim(wec.BodyValue.replace(/&nbsp;/gi, '').replace(/ /gi, '').replace(/<p>/gi, '').replace(/<\/p>/gi, '').replace(/<pstyle=\"margin-bottom:2px;margin-top:5px\">/gi, '').replace(/<pstyle=\"margin-top:5px;margin-bottom:2px\">/gi, ''));

        if (content.length <= 0 && tmpBody == "") {
            alert('목적을 입력하세요.');
            wec.SetCaretPos(0);
            return false;
        }
        else {
            wec.DefaultCharSet = "utf-8";

            var hfWebEditor = $(document.getElementById(ContentID + "_hfWriter"));
            hfWebEditor.val(wec.MIMEValue);

            return true;
        }

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
<asp:Content id="Content4" ContentPlaceholderID="MainContent" runat="server">
<%-- 2015-12-17 --%>
<asp:HiddenField ID="hidFilePathGuid" runat="server" />
<asp:HiddenField ID="hidFileDeleteKey" runat="server" />

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
	</p>
	<div id="article">
        <%if (GatheringYN == "Y")
            { %>            
            <common:GatheringMenuTab ID="GatheringMenuTabCtrl" runat="server" />
        <%} %>
		<table class="writeTableNew">
			<colgroup>
                <%--Editor Width를 맞추기 위해 변경 Mostisoft 2015.08.21--%>
                <col style="width: *;" />
                <col style="width: 754.5px" />
			</colgroup>
			<tbody>
			<tr>
				<th><span style="color:orangered;">*</span> 제목 작성하기</th>
				<td>
                    <input id="txtTitle" type="text" name="txtTitle" class="txt t1" style="width: 704.5px"  maxlength="100" onkeyUp="inputLengthCheck(this, 'title');"  />
				</td>
			</tr>
			<tr>
				<th><span style="color:orangered;">*</span> 내용 작성하기</th>
				<td>
                    <script type="text/javascript" src="../NamoActive/NamoWec8.js"></script>
				</td>
			</tr>
            
			<tr>
				<th><span style="color:orangered;">*</span> 태그 작성하기</th>
				<td>
                    <div class="tag_gathering_area" <%if (GatheringYN != "Y") { %>style="display:none;"<% } %> >
                    <select id="ddlGatheringTag" onchange="javascript:selectGatheringTag();" style="height:30px;">
                    </select>
                    <br /><br />
                    </div>
					<div class="tag_input_area" >
						<input type="text" name="" class="txt t1" style="display:none" />
                        <input  runat="server" id="txtTag" type="text" name="" class="txt t2" onclick="txtTagClickEvent();" onkeypress="if(event.keyCode == 13){txtTagEnterEvent();return false;}" onblur="javascript:setTimeout('pop_defaulttag_close()',1000);"  onkeyUp="inputLengthCheck(this, 'tag');"  maxlength="50" />
              
                        <ul id="ulDefaultTag" class="auto_word" style="display:none;z-index:999;" runat="server"></ul>
						<a class="btnAdmin" href="javascript:///" onclick="txtTagEnterEvent()"></a>
						
                        <ul runat="server" id="TagLists" class="tag_list" style="width:450px;"></ul>
                        
					</div>
                    <div id="tagcomment">
                        <span style="font-size:12px;color:#bfbfbf; ">(작성후 엔터를 눌러주세요)</span>
                    </div>
				</td>
			</tr>
            
            <tr>
				<th>기타 사항 등록하기</th>
				<td>
					<a href="javascript:" onclick="return pop_Open('Nick')" class="btn1"><span>닉네임 작성하기</span></a>
					<a href="javascript:" onclick="return pop_Open('File')" class="btn1"><span>파일 업로드하기</span></a>
					<a href="javascript:" onclick="return pop_Open('Auth')" class="btn1" id="btnSetPermission"><span>조회 권한 설정하기</span></a>

                    <%--<input type="checkbox" name="cbEtc" onclick="return pop_Open('Nick')"/><span style=" font-size:13px; font-weight:bold;">닉네임 작성하기</span>&nbsp;&nbsp;&nbsp;
					<input type="checkbox" name="cbEtc" onclick="return pop_Open('File')"/><span style=" font-size:13px; font-weight:bold;">파일 업로드하기</span>&nbsp;&nbsp;&nbsp;
					<input type="checkbox" name="cbEtc" onclick="return pop_Open('Auth')" id="btnSetPermission"/><span style=" font-size:13px; font-weight:bold;">조회 권한 설정하기</span>--%>

				</td>
			</tr>
		    
            <tr id="trNick" style="display:none;">
                <th>닉네임 작성하기 </th>
				<td>
                    <input id="NoneNick" name="rNick" class="radio" type="radio" value="NoneNick"  onclick="javascript: fnNick(this);" runat="server" />닉네임 설정안함&nbsp;&nbsp;&nbsp;
                    <input id="HaveNick" name="rNick" type="radio" class="radio" value="HaveNick"  onclick="javascript: fnNick(this);" runat="server"  />닉네임 설정
                    <div style="padding : 5px 0 0 0;">
                    <asp:TextBox ID="txtNickName" class="txt t2" runat="server" MaxLength="14"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr id="trFile" style="display:none;">
                <th>파일 업로드하기</th>
				<td>
                    <div id="manager_container" class="fileArea"></div>
                    <SKTControls:filectrl ID="GlossaryfileCtrl" runat="server" />
                </td>
            </tr>
            <tr id="trAuth" style="display:none;">
                <th>조회 권한 설정하기</th>
				<td>
                    <input id="FullPublic" name="rBtnA" class="radio" type="radio" value="FullPublic" onclick="PermisstionValCheck()" checked="checked" />전체 공개&nbsp;&nbsp;&nbsp;
                    <input id="SomePublic" name="rBtnA" type="radio" class="radio" value="SomePublic" onclick="PermisstionValCheck()" />일부 공개
                    <div style="padding : 5px 0 0 0;">
                    <fieldset class="authority" style="border:0;line-height:30px;">
                        <common:UserAndDepartment ID="UserControl" runat="server" ViewType="WriteNew" />
		            </fieldset>
                    </div>
                </td>
            </tr>
            <tr id="techtrendwriter" style="display:none;">
				<th style="width:300px;">끌.지식 구분</th>
				<td>
                    <input type="radio" runat="server" name="allofofficer" id="rd_all" value="all" size="25" checked="true" /> 티끌 전체 공개&nbsp;&nbsp;&nbsp;
					<input type="radio" runat="server" name="allofofficer"  id="rd_officer" value="officer" size="25" /> 임원에게만 보임
				</td>
			</tr>
            
			<div runat="server" id="Reason" visible="false">
            <tr>
				<th>편집 사유/내용</th>
                <td>
					<input runat="server" type="text" name="" id="txtReason" value="" class="txt t2" style="width:500px;" />
					
					<div class="write-warn">
						<label for="write-warn2">티끌을 편집하게 된 사유/내용을 간단하게 입력해 주세요.</label>
					</div>
                </td>
            </tr>
			</div>

			</tbody>
		</table>
		<p class="btn_r">
            <%--<a href="javascript:" class="btn2" onclick="return fnPreviewOpen();"><b>미리보기</b></a>--%>
            <a href="javascript:" class="btn2" onclick="fnPrevListUrl();"><b>취소하기</b></a>
            <asp:LinkButton ID="btnTempDelete" CssClass="btn2" runat="server" OnClick="btnTempDelete_Click" OnClientClick="return fnTempDelete();" Visible="false"><b>임시저장 삭제</b></asp:LinkButton>
            <a href="javascript:" class="btn3" onclick="fnSave();"><b><span id="spSave"></span></b></a>
		</p>
	</div>
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Common_Footer" runat="server">
    <div style ="display:none;">
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
        <asp:Button ID="btnList" runat="server" OnClick="btnList_Click" />
        <%--<asp:Button ID="btnGlossary" runat="server" OnClick="btnGlossary_Click" />--%>
        <asp:HiddenField ID="hdNamoContent" runat="server" />
        <asp:HiddenField ID="hdTitle" runat="server" />
        <asp:HiddenField ID="hdNamoContentText" runat="server" />
        <asp:HiddenField ID="hdCommonID" runat="server" />
        <asp:HiddenField ID="hidItemState" runat="server" />
        <asp:HiddenField ID="hidTempSaveID" runat="server" />
        <asp:HiddenField ID="hidRadioState" runat="server" />
        <asp:HiddenField ID="hdType" runat="server" />
        <asp:HiddenField ID="hdTag" runat="server" />
        <%--2014-04-29 Mr.No 추가--%>
        <asp:HiddenField ID="hdCategoryID" runat="server" Value="129" />
        <%--2014-05-15 Mr.No--%>
        <asp:HiddenField ID="hdPermissions" runat="server" />
        <asp:HiddenField ID="hdPermissionsString" runat="server" />
        <%--2014-05-15--%>
        <%--2014-05-23--%>
        <asp:HiddenField ID="hdItemGuid" runat="server" />
        <%--2014-05-26--%>
       <%-- <asp:HiddenField ID="hdPermisstionCheck" runat="server" />--%>
        <%--2014-06-09--%>
        <asp:HiddenField ID="hdUserNikName" runat="server" />
        <%-- 2015-09-22 --%>
        <asp:HiddenField ID="hddPlatformYN" runat="server" Value="N"/>
        <%-- 2015-10-14 --%>
        <asp:HiddenField ID="hddMarketingYN" runat="server" Value="N"/>
        <%-- 2015-10-27 --%>
        <asp:HiddenField ID="hddTechTrendYN" runat="server" Value="N"/>

        <%--액티브스퀘어--%>
        <asp:HiddenField ID="hddActiveBody" runat="server"/>
        <asp:HiddenField ID="hddActiveBodyText" runat="server"/>
        <asp:HiddenField ID="hdDTBlog" runat="server"/>
        <asp:HiddenField ID="hdTWhite" runat="server"/>
    </div>
</asp:Content>