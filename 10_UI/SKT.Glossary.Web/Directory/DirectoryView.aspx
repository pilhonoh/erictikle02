<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossarySearch.master" AutoEventWireup="true"
    CodeBehind="DirectoryView.aspx.cs" Inherits="SKT.Glossary.Web.Directory.DirectoryView" %>
<%@ Register Src="~/Common/Controls/UserAndDepartmentList.ascx" TagName="UserAndDepartment" TagPrefix="common" %>
<%--<%@ Register Src="~/Common/Controls/CommNateOnBizControl.ascx" TagName="NateOnBiz" TagPrefix="common" %>--%>
<%@ Register Src="~/Common/Controls/GatheringInfomation.ascx" TagName="GatheringInfomation" TagPrefix="common" %>
<%@ Register Src="~/Common/Controls/GatheringMenuTab.ascx" TagName="GatheringMenuTab" TagPrefix="common" %>

<%@ Register assembly="SKT.Tnet" namespace="SKT.Tnet.Controls" tagprefix="SKTControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">
      <!-- 기본 CSS 및 JS 정의 -->        
    <link href="/Common/Css/board.css" rel="stylesheet" />
    <style type="text/css">
        #manager_container {display:none;}
    </style>
    <!-- Script -->
   <%--<script type="text/javascript" src="/Common/Js/jquery-1.11.2.js"></script>--%>
    <script type="text/javascript" src="/common/Js/TnetBoard_Control_dir.js"></script>
    <!-- 디자인팀적용 JS -->
    <script type="text/javascript" src="/Common/Js/css.browser.detect.js"></script>

    <script type="text/javascript">

        var directoryFileName = "";
        var directoryDirName = "";
        var directoryEncFileName = "";
        var filelist = "";

        var DefaultSearchText = "";
        if ("<%=GatheringYN%>" == "Y") {
            DefaultSearchText = "우리 모임의 문서를 검색해 보세요."
        }
        else {
            DefaultSearchText = "찾고싶은 끌.문서를 검색해 보세요."
        }
        var SendNoteDefaultString = "내용을 작성해 주세요.";
        var SearchTextDefaultString = "이름이나 사번으로 검색하세요.";
        
        $(document).ready(function () {
            if ("<%=GatheringYN%>" == "Y") {
                $("#container").attr('class', 'Gathering');
                $("#txt_SearchKeyword").val("우리 모임의 문서를 검색해 보세요.");

                //PermissionsStringThrow();
            } else {
                $("#container").attr('class', 'Directory');
            }

            $("#txt_SearchKeyword").focusout(function () {
                if ($("#txt_SearchKeyword").val().replace(/^\s+|\s+$/g, '') == "") {
                    $("#txt_SearchKeyword").val(DefaultSearchText);;
                    $("#txt_SearchKeyword").click(function () {
                        if ($("#txt_SearchKeyword").val() == DefaultSearchText) {
                            $("#txt_SearchKeyword").val("");
                        }
                    });
                }
            });

            $("#DivNoteTextArea").val(SendNoteDefaultString);
            $("#DivNoteTextArea").click(function () {
                if ($("#DivNoteTextArea").val() == SendNoteDefaultString) {
                    $("#DivNoteTextArea").val("");
                }
            })

            $("#txtSchUserID").click(function () {
                if ($("#txtSchUserID").val() == SearchTextDefaultString) {
                    $("#txtSchUserID").val("");
                }
            })

            $("#txtSchUserID").keypress(function (e) {
                if (e.keyCode == 13) {
                    SearchUser();
                }
            });
        });

        function SearchUser()
        {
            if ($("#txtSchUserID").val() == "" || $("#txtSchUserID").val() == SearchTextDefaultString) {
                alert(SearchTextDefaultString);
                return;
            } else {
                document.getElementById("<%=litTargetAll.ClientID%>").selectedIndex = -1;

                var selected = $("#<%=litTargetAll.ClientID %>").children();
                selected.each(function () {
                    var userName = $(this).text();
                    var userNameString = userName.split('/');
                    var iStart = userName.indexOf('(');
                    var userNameID = userName.substr(iStart + 1, 7).toLowerCase();
                    
                    if (userNameString[0].indexOf($("#txtSchUserID").val()) > -1 || userNameID.indexOf($("#txtSchUserID").val().toLowerCase()) > -1) {
                        $(this).prop("selected", true);
                    }
                });
            }
        }
        //파일오픈
        function fileOpen(dir, file) {
            var url = "FileOpenTransfer.aspx?file=" + dir + "/" + file;

            //alert(url);

            var win = window.open(url, "_blank", "left=10, top=10, width=10, height=10, toolbar=no, menubar=no, scrollbars=yes, resizable=no");
        }

    <%--    function PermissionsStringThrow() {
            var pmStr = $('#<%= this.hdPermissionsString.ClientID %>').val();
            var pmData = JSON.parse(pmStr);
            for (var i = 0; i < pmData.length; i++) {
                // 대상자 목록에 추가
                pushToArySave(pmData[i].ToUserName + "/" + pmData[i].ToUserID, pmData[i].ToUserID, pmData[i].ToUserType);
            }
        }--%>


        //엑셀확인  
        //function fileExcelConfirm(dir, file) {

        //$('#<%--= this.hdDirectoryID.ClientID --%>').val(dir);
        //$('#<%--= this.hdFileID.ClientID --%>').val(file);


        //        $.ajax({
        //            type: "POST",
        //            contentType: "application/json; charset=utf-8",
        //            url: "/Directory/DirectoryView.aspx/GetExcelConfirmData",
        //            data: "{dirID : '" + dir + "', fileID : '" + file + "'}",
        //        dataType: "json",
        //        success: function (data) {

        //            //alert(data.d);
        //            if (data.d[0] == "-1") {
        //                alert("현재 사용자가 없습니다.");
        //            }
        //            else {
        //                alert("현재 사용자는 " + data.d[1] + "(" + data.d[3] + ")입니다");
        //            }
        //            return false;
        //        },
        //        error: function (result) {
        //        }
        //    });
        //}


        //function time9HourPlus(da, ti, g) {
        //    var defalteTime = 9;
        //    if (ti.substring(0, 2) == "오후") {
        //        defalteTime += 12;
        //    }

        //    var t = ti.replace(" ", "");
        //    t = t.replace("오전", "");
        //    t = t.replace("오후", "");

        //    dateArr = da.split("-");
        //    timeArr = t.split(":");

        //    var cd = new Date(dateArr[0], dateArr[1], dateArr[2], timeArr[0], timeArr[1], "0");
        //    cd.setHours(cd.getHours() + defalteTime);

        //    return rsltDate = cd.getFullYear() + "-" + (cd.getMonth()) + "-" + cd.getDate() + " " + (cd.getHours() < 12 ? "오전" : "오후") + " " + (cd.getHours() < 12 ? cd.getHours() : (cd.getHours() - 12)) + ":" + cd.getMinutes();


        //}

        //2015-02-04 김성환 변경
        function time9HourPlus(da, ti, g) {
            var t = ti.replace(" ", "");
            t = t.replace("오전", "");
            t = t.replace("오후", "");

            dateArr = da.split("-");
            timeArr = t.split(":");

            var milisecond = Date.UTC(dateArr[0], dateArr[1], dateArr[2], timeArr[0], timeArr[1], 0, 0);
            var UTCday = new Date(milisecond);
            var years = UTCday.getFullYear();
            var months = UTCday.getMonth() < 10 ? "0" + UTCday.getMonth() : UTCday.getMonth();
            if (months == "00") {
                months = "12";
                years = years - 1;
            }
            var days = UTCday.getDate() < 10 ? "0" + UTCday.getDate() : UTCday.getDate();
            var hours = "";
            if (UTCday.getHours() > 12) {
                 if (UTCday.getHours() == 24) {
                     hours = "오후 " + (UTCday.getHours() - 12);
                } else {
                     hours = "오전 " + (UTCday.getHours() - 12);
                }
            } else {
                if (UTCday.getHours() == 12) {
                    hours = "오후 " + UTCday.getHours();
                } else {
                    hours = "오전 " + UTCday.getHours();
                }
            }
            var minutes = UTCday.getMinutes() < 10 ? "0" + UTCday.getMinutes() : UTCday.getMinutes();
            var result = years + "-" + months + "-" + days + " " + hours + ":" + minutes;


            return result;
        }


        //파일 편집 이력보기  
        function fileVerCheck(dir, file) {

            $("#file_edit_history_body").html("<tr><td><b>데이터를 조회 중입니다..</b></td></tr>");
            $("div.pop").show();
            $("#pop_edit_history").show();
            /*
            Author : 개발자-최현미C, 리뷰자-진현빈D
            Create Date : 2016.06.01 
            Desc : 팝업숨김
            */
            $("#pop_NateOnBizTarget_add").hide();

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Directory/DirectoryView.aspx/FileEditHistoryWeb",
                data: '{DirID : "' + dir + '", FileID : "' + file + '"}',
                dataType: "json",
                success: function (data) {
                    var strHTML = "";

                    if (data.d.length > 0) {
                        var arrRslt = data.d;
                        var EDIT_DATE = "";
                        var EDIT_TIME = "";
                        var editDateTime = "";

                        for (var i = 0; i < arrRslt.length; i++) {

                            EDIT_DATE = arrRslt[i].EDIT_DATE;
                            EDIT_TIME = arrRslt[i].EDIT_TIME;
                            
                            editDateTime = time9HourPlus(EDIT_DATE, EDIT_TIME, "D");
                            editDateTimeArr = editDateTime.split(" ")

                            EDIT_DATE = editDateTimeArr[0];
                            EDIT_TIME = editDateTimeArr[1] + " " + editDateTimeArr[2];


                            strHTML += "<tr>";
                            strHTML += "<td><b>" + (i + 1) + "</b></td>";
                            strHTML += "<td>" + EDIT_DATE + "</td>";
                            strHTML += "<td>" + EDIT_TIME + "</td>";
                            strHTML += "<td>" + arrRslt[i].EDITOR + "</td>";
                            //if (arrRslt[i].IS_CURRENT_VERSION == "Y") {
                            //	strHTML += "<td>현재버전</td>";
                            //}
                            //else {
                            //	strHTML += "<td><a href=\"javascript:fnMyConfrim('" + arrRslt[i].EDIT_URL + "')\" class=\"btn_s\" ><b>확인</b></a></td>";
                            //}
                            strHTML += "</tr>";
                        }
                        //Confrim.Text = "<a href=\"javascript:fnMyConfrim('" + row["EDIT_URL"].ToString() + "')\" class=\"btn_s\" ><b>확인</b></a>";
                    }
                    else {
                        strHTML += "<tr><td><b>등록된 편집 이력이 없습니다.</b></td></tr>";
                    }

                    $("#file_edit_history_body").html(strHTML);
                },
                error: function (result) {
                    alert("Error" + ":::" + result);
                }
            });
        }


        //파일다운로드  
        function fileDownload(dir, file) {

            $('#<%= this.hdDirectoryID.ClientID %>').val(dir);
            $('#<%= this.hdFileID.ClientID %>').val(file);

            __doPostBack('<%=btnDownload.UniqueID %>', '');
        }

        //파일 삭제 
        function fnDeleteFile(dir, file) {
            //2016-10-06 문서함 삭제시 본인 확인 / 변경자: 김성환D
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Directory/DirectoryView.aspx/FileDeleteHistoryCheck",
                data: '{DirID : "' + dir + '", FileID : "' + file + '"}',
                dataType: "json",
                success: function (data) {
                    if (data.d == "Y") {
                        $('#<%= this.hdDirectoryID.ClientID %>').val(dir);
                        $('#<%= this.hdFileID.ClientID %>').val(file);

                        //삭제
                        if (confirm("[" + file + "] 문서를 삭제하시겠습니까?")) {
                            __doPostBack('<%=btnDelete.UniqueID %>', '');
                        }
                    } else {
                        alert("다른 구성원이 파일을 수정한 경우 삭제할 수 없습니다.");
                        return;
                    }
                },
                error: function (result) {
                    alert("Error" + "::" + result);
                }
            });
        }


        //쪽지보내기 
        function fnMeno(dir, file, encFile) {
            //디렉토리명
            var siteVal = $("#<%= this.ddlUserDirectory.ClientID %> option:selected").val();
            $('#<%= this.hdDirectoryID.ClientID %>').val(siteVal);
            $('#<%= this.hdFileName.ClientID %>').val(file);

            //쪽지 받는 사람 기본값 셋팅하기
            /*
				type : 'Survey', 'U' , 'O' , 'G'
				id : 문서또는의견 ID값, 사번,조직코드,그룹코드
				name : 문서또는의견은 "" , 그외는 해당명
				ajax : 'Y' ajax로 데이터 가져오기 -> fnSelectAuth(); 호출
			*/
            //debugger
            directoryDirName = dir;
            directoryFileName = file;
            directoryEncFileName = encFile;

            //nateOnBiztargetListSelect('Directory', dir, "", 'Y');
            
            showDiv();
        }
        //파일 올리기  저장  
        /*
           Author : 개발자- 최현미C, 리뷰자-진현빈D
           CreateDae :  2016.05.25
           Desc : 이중클릭 방지  (3자테스트)      
       */
        var boolFileCheck = false;
        function fnFileSave() {
            if (boolFileCheck == true)
                return false;

            var uploadType = $("input:radio[name='rdFileUp']:checked").val();

            if (uploadType == "upload") {
                if ($(".FileUloadTable>table>tbody>tr").size() < 1) {
                    alert("업로드 할 파일을 지정해주세요.");
                    return;
                }
            }

            /*Author : 개발자-김성환D, 리뷰자-이정선G
            Create Date : 2016.02.17 
            Desc : 끌문서 파일명 필드 스크립트 제거 및 특수문자 alert*/
            //2016.02.17[보류]
            $("#txtFileName").val(strip_tag($("#txtFileName").val()));

            /*Author : 개발자-김성환D, 리뷰자-이정선G
            Create Date : 2016.02.24
            Desc : 끌문서 파일명 특수문자 제한*/
            if (!checkStringFormat($("#txtFileName").val())) {
                alert('파일 이름에 제한된 특수문자가 포함되어 있습니다.\r\n\r\n제한된 특수문자: * ? < > | # { } % ~ & \" \' \\ ');
                return false;
            }

            if (uploadType == "upload") {

                FileCtrl_FileChange();
            }
            else {
                var objFileName = $("#txtFileName");
                var strFileName = objFileName.val().toString().trim();


                var filelistcheck1 = false;
                var fileext = "";
                switch (uploadType) {
                    case "excel": fileext = ".xlsx"; break;
                    case "word": fileext = ".docx"; break;
                    case "ppt": fileext = ".pptx"; break;
                }
                var txtFileFullName = $("#txtFileName").val() + fileext;

                for (var i = 0; i < filelist.length - 1; i++) {
                    if (filelist[i].toString() == txtFileFullName) {
                        filelistcheck1 = true;
                    }
                }
                if (filelistcheck1) {
                    var usercheck = confirm("등록하신 파일명과 동일한 문서가 존재합니다. \n기존 문서의 새로운 버전으로 관리됩니다. \n진행하시겠습니까?");
                    if (!usercheck) {
                        return;
                    }
                }




                if (strFileName == "") {
                    alert("파일명을 입력해 주세요.");
                    objFileName.val("");
                    objFileName.focus();
                    return;
                }

                $('#<%= hidFileDefaultName.ClientID %>').val(objFileName.val().toString());
            }

            boolFileCheck = true;
            $('#divImgLoading1').show();
            $('#btnFileUpload').removeClass("btn3");
            $('#btnFileUpload').addClass("btn3_dis");
            $('#btnFileCancel').attr("disabled", "true");
            $('#btnFileCancel').attr("style", "cursor:default");
            $('#rd_upload').attr("disabled", "true");
            $('#rd_excel').attr("disabled", "true");
            $('#rd_word').attr("disabled", "true");
            $('#rd_ppt').attr("disabled", "true");

            __doPostBack('<%=btnFileSave.UniqueID %>', '');
        }

        // 파일 업로드 구분 설정

        function changeFileUpload() {


            //if (boolFileCheck1 == true)
            //    return false;

            var rd = $("input:radio[name='rdFileUp']:checked");

            if (rd.val() == "upload") {
                // 파일업로드
                $("#txtFileName").val("");
                $("#upload_file_container").show();
                $("#select_file_contaioner").hide();
            }
            else {
                // 기본파일 셋팅
                switch (rd.val()) {
                    case "excel":
                        $("#txtFileName").val("기본 엑셀 문서");
                        $("#lblFileExt").text(".xlsx");
                        break;
                    case "word":
                        $("#txtFileName").val("기본 워드 문서");
                        $("#lblFileExt").text(".docx");
                        break;
                    case "ppt":
                        $("#txtFileName").val("기본 파워포인트 문서");
                        $("#lblFileExt").text(".pptx");
                        break;
                    default:
                        break;
                }
                //debugger;
                $("#upload_file_container").hide();
                $("#select_file_contaioner").show();
            }
            $('#<%= hidFileRadioCheck.ClientID %>').val(rd.val());
    }

    //파일올리기 레이어 
    function viewFileShow() {

        //임시로 텍스트박스 사용 
        var siteName = $("#<%= this.ddlUserDirectory.ClientID %> option:selected").text();
        var siteVal = $("#<%= this.ddlUserDirectory.ClientID %> option:selected").val();

        $('#<%= this.hdDirectoryID.ClientID %>').val(siteVal);

        $("#file_add_dir_nm").text("[" + siteName + "] 문서 올리기");

        $("div.pop").show();
        $("#pop_dc_file_add").show();
        /*
        Author : 개발자-최현미C, 리뷰자-진현빈D
        Create Date : 2016.06.01 
        Desc : 팝업숨김
        */
        $("#pop_NateOnBizTarget_add").hide();
        //var alertmessage = "안녕하세요~ ^^ \n\n특수문자 중 '(작은 따옴표)가 파일명으로 들어가면 \n에러(파일이 업로드 안된다던가....ㅡㅜ)가 생기고 있어서 \n티끌이가 지금 열심히 수정 중에 있습니다. \n\n";
        //alertmessage = alertmessage + "간단해 보이는 현상이지만\n뚜껑을 열면 얽힌 것들이 많아서 시간이 좀 걸리는데요\n늦어도 금주 금요일까지는 수정될 예정이오니\n불편하시겠지만 '(작은 따옴표)가 없는~ 파일명 부탁드릴께요\n\n감사합니다!";
        //alert(alertmessage);

    }


    function hidePop(pid) {
        $("#" + pid).hide();
        $("div.pop").hide();
    }

    //폴더관리
    function viewDivShow() {

        //임시로 텍스트박스 사용 
        var siteName = $("#<%= this.ddlUserDirectory.ClientID %> option:selected").text();
        var siteVal = $("#<%= this.ddlUserDirectory.ClientID %> option:selected").val();

        //debugger 
        /*
            Author : 개발자-김성환D, 리뷰자-진현빈G
            Create Date : 2016.08.04 
            Desc : 특수문자 " ' \ 처리
        */
        //$("#CommUserAndDepartmentIframe").attr("src", "/Directory/DirectoryViewIframe.aspx?DivID=" + siteVal + "&DivNM=" + encodeURI(siteName));
        $("#CommUserAndDepartmentIframe").attr("src", "/Directory/DirectoryViewIframe.aspx?DivID=" + siteVal + "&DivNM=" + encodeURIComponent(siteName));

        $("div.pop").show();
        $("#pop_dc_folder").show();
        $("#pop_dc_folder1").hide();
    }

    //폴더 매니저 관리
    function viewDivManagerShow() {
        var siteName = $("#<%= this.ddlUserDirectory.ClientID %> option:selected").text();
        var siteVal = $("#<%= this.ddlUserDirectory.ClientID %> option:selected").val();

        $("#CommUserAndDepartmentManagerIframe").attr("src", "/Directory/DirectoryManagerIframe.aspx?DivID=" + siteVal + "&DivNM=" + encodeURIComponent(siteName));
        $("div.pop").show();
        $("#pop_dc_folder1").show();
        $("#pop_dc_folder").hide();
    }

    function refreshMe(dirid, dirtype) {
        //location.href = "DirectoryView.aspx?DivID=" + dirid + "&DivType=" + dirtype;
        location.href = location.href;
    }

    function fnFileSearch() {

         $("#txt_SearchKeyword").val(strip_tag($("#txt_SearchKeyword").val().replace(/\'/gi, "").replace(/\"/gi, "").replace(/\\/gi, "")));

         if ($('#txt_SearchKeyword').val() == DefaultSearchText || $('#txt_SearchKeyword').val().replace(/^\s+|\s+$/g, '') == "") {
             alert(DefaultSearchText);
         }
         else {
             if ($('#txt_SearchKeyword').val().length < 2) {
                 alert('검색어는 2글자 이상 입력해 주세요.');
             }
             else {
                 location.href = "/Directory/DirectorySearchResult.aspx?q=" + encodeURIComponent($("#txt_SearchKeyword").val()) + "&GatheringYN=<%=GatheringYN%>&GatheringID=<%=GatheringID%>&MenuType=Directory";
                }
         }

    }

    // CHG610000060738 / 2018-06-21 / 최현미 / 끌문서 쪽지보내기 모임인원만 발송 
    function showDiv() {

        //수신자 목록 Clear
        ListBoxClear();

        $("div.pop").show();
        $("#pop_NateOnBizTarget_add").show();
        $("#DivNoteTextArea").val(SendNoteDefaultString);
        $("#txtSchUserID").val(SearchTextDefaultString);
            
        // 버튼 숨기고 조직도 오픈
        //neteOnBizTargetBtnHide();
        //neteOnBizTargetShow();
    }

    function divFnClear() {
        neteOnBizTargetHide();
        $("#pop_layer").hide();
    }

    // CHG610000060738 / 2018-06-21 / 최현미 / 끌문서 쪽지보내기 모임인원만 발송 
    var boolMailCheck = false;
    function targetcheck() {

        if (boolMailCheck == true)
            return;

        var Comment = {};

        if (document.getElementById('<%=litTargetSelected.ClientID%>').options.length == 0) {
        alert("수신자를 선택하여 주세요.");

        }
        else if ($.trim($("#DivNoteTextArea").val()) == '' || $("#DivNoteTextArea").val() == SendNoteDefaultString) {
            alert("쪽지 내용을 입력해 주세요.");
            $("#DivNoteTextArea").focus();
            $("#DivNoteTextArea").val('');
        } else {

            boolMailCheck = true;
            $('#divImgLoading').show();
            $('#btnSend').removeClass("btn3");
            $('#btnSend').addClass("btn3_dis");

            var targetUserList = "";
            var selected = $("#<%=litTargetSelected.ClientID %>").children();
            selected.each(function () {
                targetUserList += $(this).val() + ";";
            });
            
            var contents = $('#DivNoteTextArea').val().replace(/'/g, "`").replace(/"/g, "`");
            contents = contents.replace(/(?:\r\n|\r|\n)/g, '<br />');

            var datastring = "{";
            datastring += "DirectoryEncFileName :'" + directoryEncFileName + "' "
            datastring += ",DirectoryFileName :'" + directoryFileName + "' "
            datastring += ",TargetUserList :'" + targetUserList + "' "
            datastring += ",SendUserID :'<%=u.EmailAddress%>' "
            datastring += ",SendUserNM :'<%=u.Name%>' "
            datastring += ",DivNoteTextArea :'" + contents + "' "
            datastring += ",DivID :'<%=DivID%>' "
            datastring += "}";

            //alert(datastring);

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Directory/DirectoryView.aspx/SendNoteDirectory",
                data: datastring,
                dataType: "json",
                success: function (data) {

                    alert("발송 완료되었습니다.");
                    $("#pop_NateOnBizTarget_add").hide();

                    boolMailCheck = false;
                    $('#divImgLoading').hide();
                    $('#btnSend').removeClass("btn3_dis");
                    $('#btnSend').addClass("btn3");
                        
                    $("div.pop").hide();
                    $("#pop_NateOnBizTarget_add").hide();
                },
                error: function (response, textStatus, errorThrown) {

                    boolMailCheck = false;
                    $('#divImgLoading').hide();
                    $('#btnSend').removeClass("btn3_dis");
                    $('#btnSend').addClass("btn3");

                    ///////////////////////////////
                    alert('쪽지발송 오류:' + response + ':' + textStatus + ':' + errorThrown);
                    //$("div.pop").hide();
                    //$("#pop_NateOnBizTarget_add").hide();
                    document.getElementById("<%=litTargetAll.ClientID%>").selectedIndex = -1;
                    return;
                }
            });

        }
    }
    function ListBoxClear() {

        document.getElementById('<%=litTargetSelected.ClientID%>').options.length = 0;

        var selected = $("#<%=litTargetAll.ClientID %>").children(":disabled");
        selected.each(function () {
            $("#<%=litTargetAll.ClientID %>").children("option[value='" + $(this).val() + "']").attr("disabled", false);
        });

        document.getElementById("<%=litTargetAll.ClientID%>").selectedIndex = -1;
    }
    function btnAdd_Click() {
        var selected = $("#<%=litTargetAll.ClientID %>").children(":selected");
        var selectedTo = selected.clone();
        selected.attr("disabled", true);

        $("#<%=litTargetSelected.ClientID %>").append(selectedTo);
        document.getElementById("<%=litTargetAll.ClientID%>").selectedIndex = -1;
    }

    function btnDel_Click() {
        var selected = $("#<%=litTargetSelected.ClientID %>").children(":selected").detach(); 
        
        selected.each(function (e) {
            $("#<%=litTargetAll.ClientID %>").children("option[value='" + $(this).val() + "']").attr("disabled", false);
            //$("#<%=litTargetAll.ClientID %>").children("option[value='" + $(this).val() + "']").attr("selected", true);
        });
        document.getElementById("<%=litTargetAll.ClientID%>").selectedIndex = -1;
    }

    //링크명 보내기
    function nateOnBiztargetLInkNm() {
        return "파일 바로가기";
    }
    //링크정보 보내기
    function nateOnBiztargetLInkUrl() {
        return directoryEncFileName;
    }
    //링크타입보내기 : 문서에서 쪽지로 파일 보내기 일때 : fileSend  그외 빈값
    function nateOnBiztargetType() {
        //return "fileSend";
        return "fileSend";
    }

    //링크타입보내기 : 문서에서 쪽지로 파일 보내기 일때 : fileSend  그외 빈값
    function nateOnBiztargetDirName() {
        //return "폴더코드";
        return directoryDirName;
    }

    //링크타입보내기 : 문서에서 쪽지로 파일 보내기 일때 : fileSend  그외 빈값
    function nateOnBiztargetFileName() {
        //return "파일명";
        return directoryFileName;
    }
    //-->
</script>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Common_Top" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hidFilePathGuid" runat="server" />
    <asp:HiddenField ID="hidFileDeleteKey" runat="server" />
    <asp:HiddenField ID="hidFileRadioCheck" runat="server" Value ="upload" />
    <asp:HiddenField ID="hidFileDefaultName" runat="server"/>
    <script type="text/javascript">
        var lnbDep1 = 3;		//LNB 1depth
    </script>
    <!--CONTENTS-->
    <div id="contents">
        <div class="h2tag">
            <p>
                <%if (GatheringYN == "Y") { %>
                    <%--<a href="/Gathering/Main.aspx"><img src="/common/images/text/Gathering_text.png" alt="끌.모임" style="left: 50px; top: 30px; position: absolute; width: 83px; height: 26px;" /></a>--%>
                    <common:GatheringInfomation ID="GatheringInfomation1" runat="server" />
                <%} else { %>
                    <h2 style="padding-left: 175px;"><img src="/common/images/text/Directory.png" alt="끌.문서" /></h2>
                <%} %>
            </p>
        </div>
        <%--<p class="search_top">
                <input name="q" id="txt_SearchKeyword" type="text" value="찾고싶은 끌.문서를 검색해 보세요" onfocus="this.value=''" onkeypress="if(event.keyCode == 13){fnFileSearch();}" />
                <a href="javascript:fnFileSearch()">
                    <img src="/common/images/etc/search_btn.png" alt="" title="검색" /></a>
        </p>--%>
	    <!--article-->
        <div  id="article" <%if (GatheringYN != "Y") { Response.Write("style=\"padding-top:20px;\""); } else { Response.Write("style=\"padding-top:32px;\""); }%>>

             <%--검색 영역 --%>
                    <div style="float:right; overflow:hidden; padding-bottom:10px;">
                        <input type="text" class="ui-autocomplete-input search_txt_common" id="txt_SearchKeyword"  value="찾고싶은 끌.문서를 검색해 보세요." onkeypress="if(event.keyCode == 13){fnFileSearch();return false;}" onfocus="this.value=''" />
			            <a href="javascript:fnFileSearch();"><img src="/common/images/btn/search.png" alt="검색" /></a>
	                </div>

            <%if (GatheringYN == "Y")
              { %>
            <common:GatheringMenuTab ID="GatheringMenuTabCtrl" runat="server" />
            <%}
              else
              {%>
            <ul id="tabMenu">
                <li><a href="/Directory/DirectoryListNew.aspx?DivType=Pub" <%= m_pub%>>
                    <img src="/common/images/btn/Directory_tab1.png" alt="모든 문서공유방" /></a></li>
                <li><a href="/Directory/DirectoryListNew.aspx?DivType=Pri" <%= m_pri%>>
                    <img src="/common/images/btn/Directory_tab2.png" alt="내가만든 문서공유방" /></a></li>
                <li><a href="/Directory/DirectoryListNew.aspx?DivType=Vis" <%= m_vis%>>
                    <img src="/common/images/btn/Directory_tab3.png" alt="초대된 문서공유방" /></a></li>
            </ul>
            <%}%>
            <!--문서공유방 상세-->
            <div id="directory_view_area">
                <div id="directory_view" <%if (GatheringYN == "Y")
                                           { %>style="width:100%;" <% } %>>
                    <h2>문서함명 &nbsp;&nbsp;
							<asp:DropDownList ID="ddlUserDirectory" CssClass="select" Width="300px" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlUserDirectory_SelectedIndexChanged" />
                    </h2>

                    <p class="btn_file_add">
                        <%if (GatheringYN != "Y") { %>
                        <!--<a href="javascript:viewFileShow();" class="btn5"><b>문서올리기</b></a>-->
                        <% } %>
                    </p>

                    <ul>
                        <!--리피터  -->
                        <asp:Repeater ID="rptDirectory" runat="server" OnItemDataBound="rptDirectory_OnItemDataBound">
                            <ItemTemplate>
                                <asp:Literal ID="litDirectory" runat="server" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Literal ID="lblEmptyData"
                                    Text="<li style='padding:0;text-align:center'><h2>등록된 문서가 없습니다.</h2></li>" runat="server" Visible="false">
                                </asp:Literal>
                            </FooterTemplate>
                        </asp:Repeater>
                        <!--리피터  -->
                    </ul>
                </div>

                <div id="directory_user" <%if (GatheringYN == "Y")
                                           { %>style="display:none;" <% } %>>
                    <h3>
                        <img src="/common/images/text/Directory1.png" alt="초대된 멤버" /></h3>
                    <p id="p_dirsetting" runat="server" visible="false"><a href="javascript:viewDivShow();"><img src="/common/images/icon/setting.png" alt="" title="폴더사용자 관리 바로가기" /></a></p>

                    <ul>
                        <asp:Repeater ID="rptDirectoryUser" runat="server" OnItemDataBound="rptDirectoryUser_OnItemDataBound">
                            <ItemTemplate>
                                <asp:Literal ID="litDirectoryUser" runat="server" />
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                    
                    <dl>
                        <dt>
                            <img src="/common/images/text/Directory2.png" alt="문서함 만든이" /></dt>
                        <dd>
                            <asp:Label ID="lblCreater" runat="server"></asp:Label></dd>
                    </dl>
                    
                     <dl>
                        <dt>
                            <img src="/common/images/text/Directory3.png" alt="문서함 생설일" /></dt>
                        <dd>
                            <asp:Label ID="lblCreateDate" runat="server"></asp:Label></dd>
                    </dl>
                    <!-- <p><a href="javascript:viewDivShow();"><img src="../common/images/btn/setting.png" alt="" title="폴더사용자 관리 바로가기" /></a></p>-->
                </div>
            </div>
            <!--문서공유방 상세-->
        </div>
        <!--/article-->
    </div>
    <!--/CONTENTS-->

    <div class="pop">
        <div class="popBg"></div>
        <!--문서_파일업로드-->
        <div id="pop_dc_file_add" class="popCt layer_pop">
            <h3><b id="file_add_dir_nm">문서함명</b></h3>
            <p class="radio_area">
                <input type="radio" name="rdFileUp" onclick="changeFileUpload();" class="radio" id="rd_upload" value="upload" checked="checked" />
                <label for="rd_upload">PC에서 파일 불러오기</label>
                <input type="radio" name="rdFileUp" onclick="changeFileUpload();" class="radio" id="rd_excel" value="excel" />
                <label for="rd_excel">Excel 열기</label>
                <input type="radio" name="rdFileUp" onclick="changeFileUpload();" class="radio" id="rd_word" value="word" />
                <label for="rd_word">Word 열기</label>
                <input type="radio" name="rdFileUp" onclick="changeFileUpload();" class="radio" id="rd_ppt" value="ppt" />
                <label for="rd_ppt">PowerPoint 열기</label>
            </p>
            <div id="upload_file_container">
                <div id="manager_container" class="fileArea"></div>
                <div style='font-family: "맑은고딕","Malgun Gothic","nanumgothic","나눔고딕","돋움",dotum,applegothic,sans-serif; font-size: 12px; color: #bfbfbf;'>
                    <%--파일명에 포함된 특수문자(<%=HttpUtility.HtmlEncode("\\ / : * ? \" < > | # { } % ~ &")%> 등)는 언더바(_)로 자동 변환됩니다.--%>
                </div>
                <%--<script src="/Common/Controls/DextUploadFl/DEXTUploadFL.js" type="text/javascript"></script>--%>
                <script type="text/javascript">
                    function ResetOnBeforeUnloadFired() {
                        onBeforeUnloadFired = false;
                    }
                </script>
                <SKTControls:filectrl ID="fileCtrl" runat="server" />
        </div>
		<div id="select_file_contaioner" style="display:none;">
			<span class=""><input type="text" name="txtFileName" id="txtFileName" value="" class="txt t2" maxlength="100"/> <label for="txtFileName" id="lblFileExt"></label></span>
		</div>
		<p class="btn_c">
			<a href="javascript:hidePop('pop_dc_file_add');" class="btn2" id="btnFileCancel"><b>취소하기</b></a>
			<a href="javascript:;" onclick="fnFileSave();" class="btn3" id="btnFileUpload"><b>저장하기</b></a>
		</p>
		<a href="javascript:;"><img src="/common/images/btn/pop_close.png" alt="닫기" class="close" /></a>
        <br /><p id="divImgLoading1" style="display:none; text-align:center; " >파일 저장중...</p>
	</div>
	<!--/문서_파일업로드-->

	<!--문서_폴더관리-->
	<div id="pop_dc_folder" class="layer_pop" style="padding:0;border:0;width:392px;height:586px;">
		<iframe id="CommUserAndDepartmentIframe" name="CommUserAndDepartmentIframe" src="about:blank" style="width:100%;height:100%;" frameborder="0"></iframe>
	</div>
	<!--/문서_폴더관리-->

        <!--문서_매니저관리-->
	<div id="pop_dc_folder1" class="layer_pop" style="padding:0;border:0;width:392px;height:483px;">
		<iframe id="CommUserAndDepartmentManagerIframe" name="CommUserAndDepartmentManagerIframe" src="about:blank" style="width:100%;height:100%;" frameborder="0"></iframe>
	</div>
	<!--/문서_매니저관리-->

	<!--문서_편집이력보기-->
	<div id="pop_edit_history" class="layer_pop">
		<h3>편집 이력보기</h3>
			<div>
				<table>
					<colgroup><col width="8%" /><col width="23%" /><col width="22%" /><col width="*" /></colgroup>
					<tbody id="file_edit_history_body">
					</tbody>
				</table>
			</div>
		<img src="/common/images/btn/pop_close.png" alt="닫기" class="close" />
	</div>
	<!--/문서_편집이력보기-->

	<!--의견_쪽지로알리기-->
    <!-- CHG610000060738 / 2018-06-21 / 최현미 / 끌문서 쪽지보내기 모임인원만 발송 -->
	<div id="pop_NateOnBizTarget_add" class="layer_pop" style="width:580px;">
		<h3>쪽지보내기</h3>
		<div id="addWrap">
	        <ul class="dir_memo">
                <li>
                    <div>
                    <input type="text" id="txtSchUserID"   style="width:220px; height:24px" />
                    <img src="/common/images/btn/search.png" style="vertical-align:middle; width:25px; height:25px;" onclick="SearchUser();" />
                    (초대된 멤버만 발송가능합니다.)
                    </div>
                </li>
                <li><br /></li>
                <li class="dir_col3">
                    <div>
                        <asp:ListBox runat="server" ID="litTargetAll" Rows="10" Width="250" Height="200" Style="vertical-align: middle;" EnableViewState="false" SelectionMode="Multiple">
						</asp:ListBox>
					</div>
					<div class="btns">
						<a class="btn1" href="javascript:btnAdd_Click();">
							<span class="name">추가</span>
						</a>
                        <a class="btn1" href="javascript:btnDel_Click();">
							<span class="name">삭제</span>
						</a>						
					</div>
					<div>
						<asp:ListBox runat="server" ID="litTargetSelected" Rows="10" Width="250" Height="200" Style="vertical-align: middle;" EnableViewState="false" SelectionMode="Multiple">
						</asp:ListBox>	
					</div>
                </li>
                <li><textarea rows="30" style="width:96.3%; height:180px" id="DivNoteTextArea" name="DivNoteTextArea">내용을 작성해 주세요.</textarea></li>
            </ul>           
        </div>
		<p class="btn_c">
            <a href="javascript:targetcheck();" class="btn3" id="btnSend"><b>보내기</b></a>
		</p>
		<img src="/common/images/btn/pop_close.png" title="닫기" class="close" style="cursor:pointer;" />
        <br /><p id="divImgLoading" style="display:none; text-align:center; " >쪽지 발송중...</p>
	</div>

	<!--/의견_쪽지로알리기-->
</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Common_Footer" runat="server">
    <div style="display:none;">
        <asp:HiddenField ID="hidMenuType" runat="server" />
        <asp:HiddenField ID="hdDirectoryID" runat="server" />
        <asp:HiddenField ID="hdDirectoryNM" runat="server" /> 
        <asp:HiddenField ID="hdFileID" runat="server" /> 
        <asp:Button ID="btnFileSave" runat="server" OnClick="btnFileSave_Click" />
        <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" />
        <asp:Button ID="btnDownload" runat="server" OnClick="btnDownload_Click" /> 
        <asp:HiddenField ID="hdCommonID" runat="server" />
        <asp:HiddenField ID="hdBoardID" runat="server" />
        <asp:HiddenField ID="hdItemGuid" runat="server" />
        <asp:HiddenField ID="hdFileName" runat="server" />
        <asp:HiddenField ID="hdFileList" runat="server" />
       <%-- <asp:Button ID="btnExcelConfirm" runat="server" OnClick="btnExcelConfirm_Click" /> --%>

   </div>
   <script type="text/javascript">
        $(window).load(function () {
            filelist = $("#<%=hdFileList.ClientID%>").val().split(',');
        });
    </script>
</asp:Content>
