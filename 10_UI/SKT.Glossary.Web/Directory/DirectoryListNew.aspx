<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/GlossarySearch.Master" 
	CodeBehind="DirectoryListNew.aspx.cs" Inherits="SKT.Glossary.Web.Directory.DirectoryListNew" %>
<%@ Register Src="~/Common/Controls/UserAndDepartmentList.ascx" TagName="UserAndDepartment" TagPrefix="common" %>
<%@ Register Src="~/Common/Controls/GatheringInfomation.ascx" TagName="GatheringInfomation" TagPrefix="common" %>
<%@ Register Src="~/Common/Controls/GatheringMenuTab.ascx" TagName="GatheringMenuTab" TagPrefix="common" %>

<%@ Register assembly="SKT.Tnet" namespace="SKT.Tnet.Controls" tagprefix="SKTControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">

     <!-- 기본 CSS 및 JS 정의 -->        
    <link href="/Common/Css/board_G.css" rel="stylesheet" />
    <style type="text/css">
        .FileUloadTable {width:480px;}
    </style>
   
    <!-- Script -->
   <%--<script type="text/javascript" src="/Common/Js/jquery-1.11.2.js"></script>--%>
    <script type="text/javascript" src="/common/Js/TnetBoard_Control_dir.js"></script>
    <!-- 디자인팀적용 JS -->
    <script type="text/javascript" src="/Common/Js/css.browser.detect.js"></script>
	<script type="text/javascript">
	    var filelist = "";
	    //파일오픈 
	    function fileOpen(dir, file) {
	        var url = "FileOpenTransfer.aspx?file=" + dir + "/" + file;
	        var win = window.open(url, "_blank", "left=10, top=10, width=10, height=10, toolbar=no, menubar=no, scrollbars=yes, resizable=no");
	    }

         function fileDownload(dir, file) {

            $('#<%= this.hdDirectoryID.ClientID %>').val(dir);
            $('#<%= this.hdFileID.ClientID %>').val(file);

            __doPostBack('<%=btnDownload.UniqueID %>', '');
        }

	    //2017 3자 테스트
	    $(document).ready(function () {
	        $("#plusBtn").hide();
	    });

	    function CloseMessage()
	    {
	        alert("안녕하세요.\n\n시스템 노후화로 인해 금년도 9월, T.끌 문서함 서비스를 종료합니다.\n\n구성원 간의 문서공유를 위해서는 Teams를 사용하시기 바랍니다.\n\n기존 데이터의 경우, 이관 관련하여 개별 안내해드릴 예정입니다.\n\n감사합니다.");
	    }
	    function lisCreate() {
	        var pageNum = $("#currPageNum").val(); //페이지
	        var mode = "<%= DivType%>"; //타입
			var userID = "<%= UserID%>"; //로그인사용자
		    var pageSize = 6; //가져올개수

		    if (pageNum < 0) {
		        alert("더이상 가져올 문서함이 없습니다.");
		    }
		    else {

		        //첫번째호출이며 모든의견함이거나 내가만든 의견합이라면 5개만 가져옴
		        if (pageNum == 1 && (mode == "Pub" || mode == "Pri")) {
		            pageSize = 5;
		        }

		        //$("#imgLoading").show();

		        $.ajax({
		            type: "POST",
		            contentType: "application/json; charset=utf-8",
		            url: "/Directory/DirectoryListNew.aspx/DirectoryListSelectWeb",
		            data: "{mode : '" + mode + "', userID : '" + userID + "', pageNum :" + pageNum + ", pageSize :" + pageSize + ", GatheringYN : '<%=this.GatheringYN %>', GatheringID : '<%=this.GatheringID %>'}",
					dataType: "json",
					success: function (data) {
					    var Table = data.d.Table;
					    boxList(Table, mode, userID, pageNum, pageSize);
					    if (Table.length > 0) {
					        pageNum++;
					    } else {
					        pageNum = -1;
					    }
					    $("#currPageNum").val(pageNum);
					    $("#plusBtn").show();
					},
					error: function (result) {
					    alert("Error" + ":::" + result);
					},
					complete: function () {
					    //$("#imgLoading").hide();
					    getFileList();
					}
				});
            }
        }

        function fnConfirmClose() {
            $.cookie("pop_doc_notice", "Y", { expires: 365, path: "/" });
            hidePop("pop_doc_notice");
        }

        // 문서함 목록 조회
        function boxList(table, mode, userID, pageNum, pageSize) {

            var dHtml = "";
            //첫번째페이지이면서 가져올 개수가 5개인경우
            if (pageNum == 1 && pageSize == 5) {
                if ("<%=this.GatheringYN%>" == "Y") {
			        //dHtml = "<li><a href='DirectoryWrite.aspx?DivType=" + mode + "&GatheringYN=<%=GatheringYN%>&GatheringID=<%=GatheringID%>&MenuType=Directory'><img src='/common/images/btn/add_GatheringDir.png' alt='새로운 문서함을 만들어보세요!' /></a></li>";
			    } else {
			        if (mode == "Pub") {
			            //dHtml = "<li><a href='DirectoryWrite.aspx?DivType=" + mode + "&GatheringYN=<%=GatheringYN%>&GatheringID=<%=GatheringID%>&MenuType=Directory'><img src='/common/images/btn/add_Directory.png' alt='새로운 문서함을 만들어보세요!' /></a></li>";
			            dHtml = "<li><a href='javascript:CloseMessage();'><img src='/common/images/btn/add_Directory.png' alt='새로운 문서함을 만들어보세요!' /></a></li>";
			        } else if (mode == "Pri") {
			            //dHtml = "<li><a href='DirectoryWrite.aspx?DivType=" + mode + "&GatheringYN=<%=GatheringYN%>&GatheringID=<%=GatheringID%>&MenuType=Directory'><img src='/common/images/btn/add_Directory2.png' alt='새로운 문서함을 만들어보세요!' /></a></li>";
			            dHtml = "<li><a href='javascript:CloseMessage();'><img src='/common/images/btn/add_Directory2.png' alt='새로운 문서함을 만들어보세요!' /></a></li>";
			        }
            }
        }

		    //가져온개수가 있다면
        if (table.length > 0) {
            for (var i = 0; i < table.length; i++) {
                dHtml += liCreate(table[i], userID, mode);
            }

            //개수 확인하여 1페이지인경우 빈칸 채우기
            var binLiLen = pageSize - table.length;
            if (binLiLen > 0) {
                if (pageNum == 1) {
                    for (var b = 0 ; b < binLiLen; b++) {
                        dHtml += "<li></li>";
                    }
                }
                //더보기 버튼 숨기기
                $("#plusBtn").hide();
            }

        } else if (pageNum == 1) {
            dHtml += "<li></li><li></li><li></li><li></li><li></li>";
            if (mode == "Vis") {
                dHtml += "<li></li>";
            }
            $("#plusBtn").hide();
        }

        $("#directory_list").append(dHtml);
    }

    // 문서함 목록 생성
    function liCreate(t, uid, mode) {
        var rtn = "";
        var FileCntStr = "";
        var strFileList = "";
        var AUTHUSERCNTSTR = ""

        if (t.AUTHUSERCNT > 1) {
            AUTHUSERCNTSTR = t.FIRSTNAME + " 외 " + (t.AUTHUSERCNT - 1) + "명";
        }
        else {
            AUTHUSERCNTSTR = t.FIRSTNAME;
        }

        FileCntStr = "문서 <span id='FILECNT_" + t.DIR_ID + "'>0</span>개";
        //strFileList = getFileList(t.DIR_ID, t.FILELIST);

        rtn += '<li>';
        rtn += '	<dl onClick="fnViewMove(' + t.DIR_ID + ')" style="cursor:pointer">';
        rtn += '		<dt>' + t.DIR_NM + '</dt>';
        rtn += '		<dd>' + AUTHUSERCNTSTR + ' | ' + FileCntStr + '</dd>';
        rtn += '	</dl>';
        rtn += '	<ul id="FileList_' + t.DIR_ID + '">';
        rtn += '	<li style="text-align:center;"><img src="/common/images/common/ajax-loader.gif" style="display:none" /><span style="display:none" class="spn_dir_id">' + t.DIR_ID + '</span></li>';
        rtn += '	</ul>';
        //rtn += '	<a href="DirectoryView.aspx?DivType=' + mode + '&DivID=' + t.DIR_ID + '" class="btn"><img src="/common/images/btn/more.png" alt="" title="더보기" /></a>';
        rtn += '	<p class="btns">';
        //생성한 사용자라면(문서함 관리자인경우)
        if ("<%=this.GatheringYN%>" != "Y") {
            var managerID = t.managerID.split(',');
            var managercheck = false;
            for (var i = 0; i < managerID.length; i++) {
                if (managerID[i].toUpperCase() == uid.toUpperCase()) {
                    managercheck = true;
                }
            }
            if (managercheck) {
                //if (t.REG_ID == uid) {
                rtn += "	<a href=\"javascript:;\" onclick=\"deleteDir('" + t.DIR_ID + "');\" class=\"btn_arrow_small\"><span class=\"red\">문서함 삭제</span></a>";
                //rtn += "	<a href=\"javascript:;\" onclick=\"viewDivShow('" + t.DIR_ID + "', '" + t.DIR_NM + "', '" + t.REG_ID + "');\" class=\"btn_arrow_small\"><span>멤버 관리</span></a>";
                //rtn += "	<a href=\"javascript:;\" id=\"admindivshow\" onclick=\"viewDivManagerShow('" + t.DIR_ID + "', '" + t.DIR_NM + "', '" + t.REG_ID + "');\" class=\"btn_arrow_small\"><span>관리자 설정</span></a>";
            }
        } else {
            
            if (t.REG_ID == uid) {
                rtn += "	<a href=\"javascript:;\" onclick=\"deleteDir('" + t.DIR_ID + "');\" class=\"btn_arrow_small\"><span class=\"red\">문서함 삭제</span></a>";
                /*Author : 개발자-김성환D, 리뷰자-진현빈D
                  Create Date : 2016.12.02 
                  Desc : 멤버 관리를 문서함 관리로 문구 변경*/
                rtn += "	<a href=\"javascript:;\" onclick=\"viewDivShow('" + t.DIR_ID + "', '" + t.DIR_NM + "', '" + t.REG_ID + "');\" class=\"btn_arrow_small\"><span>문서함 관리</span></a>";
            }
        }
        //rtn += "		<a href=\"javascript:;\" onclick=\"viewFileShow('" + t.DIR_ID + "', '" + t.DIR_NM + "');\" class=\"btn_arrow_small\"><span>관리자 설정</span></a>";
        if ("<%=this.GatheringYN%>" != "Y") {
            //rtn += "		<a href=\"javascript:;\" onclick=\"viewFileShow('" + t.DIR_ID + "', '" + t.DIR_NM + "');\" class=\"btn_arrow_small\"><span>문서 올리기</span></a>";
        }
        rtn += '	</p>';
        rtn += '</li>';

        return rtn;
    }

    //문서함별 파일목록 조회
    function getFileList() {

        var arrDirID = $("span.spn_dir_id");

        arrDirID.each(function (idx, obj) {
            var DirID = $(this).text();

            // 파일 갯수 조회
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Directory/DirectoryListNew.aspx/GetFileCountWeb",
                data: "{DIR_ID : '" + DirID + "'}",
                //dataType: "json",
                success: function (rslt) {
                    $("span#FILECNT_" + DirID).html(rslt.d);
                }
            });

            // 파일 목록 조회
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Directory/DirectoryListNew.aspx/GetFileListWeb",
                data: "{DIR_ID : '" + DirID + "',Top_Count:'5'}",
                //dataType: "json",
                success: function (rslt) {

                    var li = $.parseJSON(rslt.d);
                    var strList = "";

                    for (var i = 0; i < li.length; i++) {
                        var fname = li[i].FILE_NAME;
                        var icon = getFileIcon(fname);

                        //2015-08-24 ksh 문서볼때 fullname 말풍선 추가
                        //strList += "		<li><img src=\"/common/images/icon/" + icon + "\" alt=\"\" /><a href=\"javascript:fileOpen('" + DirID + "','" + fname + "') \">" + fname + "</a><time>" + li[i].EDIT_DATE + "</time></li>";
                        strList += "		<li><img src=\"/common/images/icon/" + icon + "\" alt=\"\" /><a href=\"javascript:fileDownload('" + DirID + "','" + fname + "') \" title=\""+fname+"\">" + fname + "</a><time>" + li[i].EDIT_DATE + "</time></li>";
                    }
                    $("ul#FileList_" + DirID).html(strList);
                }
            });
        });
    }

    function getFileIcon(fname) {
        var fext = "";
        var img = "";

        if (fname != undefined) {
            var tmparr = String(fname).split(".");

            fext = tmparr[tmparr.length - 1];
        }
        else {
            fext = "";
        }

        switch (fext) {
            case "pptx":
            case "ppt":
                img = "ms_ppt.png";
                break;
            case "docx":
            case "doc":
                img = "ms_word.png";
                break;
            case "xlsx":
            case "xls":
                img = "ms_excel.png";
                break;
            case "one":
                img = "ms_onenote.png";
                break;
            case "pdf":
                img = "ms_pdf.png";
                break;
            default:
                img = "ms_pc.png";
        }

        return img;
    }

        function fnViewMove(i) {
            document.location.href = "DirectoryView.aspx?DivType=<%= DivType%>&DivID=" + i + "&GatheringYN=<%=GatheringYN%>&GatheringID=<%=GatheringID%>&MenuType=Directory";
		    }

		//파일 관리  
	    function viewFileShow(dir, nm) {
		    $('#<%= this.hdDirectoryID.ClientID %>').val(dir);
		    $('#<%= this.hdDirectoryNM.ClientID %>').val(nm);
		    $("#file_add_dir_nm").text("[" + nm + "] 문서 올리기");

		    
		    $("div.pop").show();
		    $("#pop_dc_file_add").show();
		    $("#pop_dc_folder1").hide();

		    $.ajax({
		        type: "POST",
		        contentType: "application/json; charset=utf-8",
		        url: "/Directory/DirectoryListNew.aspx/DirectoryListSelectFileName",
		        data: "{dirid : '" + dir + "'}",
		        dataType: "json",
		        success: function (rslt) {
		            var li = $.parseJSON(rslt.d);
		            var str = "";
		            for (var i = 0; i < li.length; i++) {
		                str += li[i].FILE_NAME + ",";
		            }
		            filelist = str.split(',');
		        }
            });

		    
		    //var alertmessage = "안녕하세요~ ^^ \n\n특수문자 중 '(작은 따옴표)가 파일명으로 들어가면 \n에러(파일이 업로드 안된다던가....ㅡㅜ)가 생기고 있어서 \n티끌이가 지금 열심히 수정 중에 있습니다. \n\n";
		    //alertmessage = alertmessage + "간단해 보이는 현상이지만\n뚜껑을 열면 얽힌 것들이 많아서 시간이 좀 걸리는데요\n늦어도 금주 금요일까지는 수정될 예정이오니\n불편하시겠지만 '(작은 따옴표)가 없는~ 파일명 부탁드릴께요\n\n감사합니다!";
		    //alert(alertmessage);
		}

		//파일저장 -- 
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
            Desc : 끌문서 제목,파일명 필드 스크립트 제거 및 특수문자 alert*/
		    $('#<%= this.txtTitle.ClientID %>').val(strip_tag($('#<%= this.txtTitle.ClientID %>').val()));
		    $("#txtFileName").val(strip_tag($("#txtFileName").val()));

		    /*Author : 개발자-김성환D, 리뷰자-이정선G
            Create Date : 2016.02.24
            Desc : 끌문서 파일명 특수문자 제한*/
		    if (!checkStringFormat($("#txtFileName").val())) {
		        alert('파일 이름에 제한된 특수문자가 포함되어 있습니다.\r\n\r\n제한된 특수문자: * ? < > | # { } % ~ & \" \' \\ ');
		        //alert('파일 이름에는 다음 문자를 사용할 수 없습니다.\r\n\r\n         \\ / : * " < > |');
		        return false;
		    }

	        /*Author : 개발자-김성환D, 리뷰자-진현빈D
            Create Date : 2016.06.01
            Desc : 끌문서 파일명 특수문자 제한*/
		    var filelistcheck = false;
		    var fileext = "";
		    switch (uploadType) {
		        case "excel": fileext = ".xlsx"; break;
		        case "word": fileext = ".docx"; break;
		        case "ppt": fileext = ".pptx"; break;
		    }
		    var txtFileFullName = $("#txtFileName").val() + fileext;

		    for (var i = 0; i < filelist.length - 1; i++) {
		        if (filelist[i].toString() == txtFileFullName) {
		            filelistcheck = true;
		        }
		    }
		    if (filelistcheck) {
		        var usercheck = confirm("등록하신 파일명과 동일한 문서가 존재합니다. \n기존 문서의 새로운 버전으로 관리됩니다. \n진행하시겠습니까?");
		        if (!usercheck) {
		            return;
		        }
		    }


		    var uploadType = $("input:radio[name='rdFileUp']:checked").val();

		    //// 2015-08-10 Mr.No doc, ppt, xls file 확장자를 구분합니다.
		    //var fileExtensionYN = false;

		    if (uploadType == "upload") {
		        FileCtrl_FileChange();

		    }
		    else {
		        var objFileName = $("#txtFileName");
		        var strFileName = objFileName.val().toString().trim();

		        if (strFileName == "") {
		            alert("파일명을 입력해 주세요.");
		            objFileName.val("");
		            objFileName.focus();
		            return;
		        }

		        
		        $('#<%= hidFileDefaultName.ClientID %>').val(objFileName.val().toString());
		    }

	        filelist = "";

		    //// 2015-08-10 Mr.No doc, ppt, xls file 확장자를 구분합니다.
		    //if (fileExtensionYN) {
		    //    alert("현재 사내 MS-Office 기준보다\n낮은 버전의 doc, ppt, xls 파일을 업로드 하셨습니다.\n2명 이상의 동시편집을 위해서는 ISAC으로 연락하셔서 MS-Office의 버전을 업그레이드(docx, pptx, xlsx 파일) 해 주시기 바랍니다.\n감사합니다.");
		    //}

		    //if (uploadManager.hasChanged()) {
		    //    uploadManager.transferByForce();
		    //} else {
	        //저장 

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
			//}
        }

        // 파일 업로드 구분 설정
        function changeFileUpload() {
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

        //폴더 관리 
	    function viewDivShow(dir, nm, author) {

            $('#<%= this.hdDirectoryID.ClientID %>').val(dir);
		    $('#<%= this.hdDirectoryNM.ClientID %>').val(nm);

            $('#<%= this.txtTitle.ClientID%>').val($('#<%= this.hdDirectoryNM.ClientID %>').val());
            $('#<%= this.hdGatheringAuther.ClientID %>').val(author);
		    $("div.pop").show();
		    $("#pop_dc_folder").show();
		    $("#pop_dc_folder1").hide();

		    var type = "Directory";

		    var m_UserID = '<%= UserID %>';

            try {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "/Common/Controls/AjaxControl.aspx/GetMyGroupList",
                    data: "{Type : '" + type + "', UserID : '" + m_UserID + "', MyGrpID : '" + dir + "'}",
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data.d[0] != undefined && data.d[0].length != 0) {
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
        }

	 
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

		    // 제목 중복 체크
            TitleDBCheck();

            if (m_titleCheck == true) {
                alert('동일한 제목이 존재합니다 저장하실수 없습니다.');
                $('#<%= this.txtTitle.ClientID %>').focus();
				return;
            }

		    //조직도 
            fnShareSave();

		    //저장 
            __doPostBack('<%=btnSave.UniqueID %>', '');
        }

        var m_titleCheck = false;

        // 제목중복 체크
        function TitleDBCheck() {
            //var txtTitle = $('#txtTitle').val().replace(/'/g, "&#39;");
            //string Title, string itemID, string UserID, string mode

            var m_UserID = "<%= UserID %>";
		    var itemID = $('#<%= this.hdDirectoryID.ClientID %>').val();
		    var txtTitle = $('#<%= this.txtTitle.ClientID %>').val();
		    var mode = "Directory";

		    try {
		        $.ajax({
		            type: "POST",
		            contentType: "application/json; charset=utf-8",
		            url: "/Common/Controls/AjaxControl.aspx/ExistTitleEtc", //2
		            data: "{Title : '" + txtTitle + "',itemID : '" + itemID + "',UserID : '" + m_UserID + "', mode : '" + mode + "'}",
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

		// 문서함 삭제
		function deleteDir(dir_id) {
		    if (confirm("문서함을 삭제할 경우 해당문서함에 등록된 파일도 함께 삭제됩니다.\n\n선택한 문서함을 정말 삭제하시겠습니까?")) {
		        $('#<%= this.hdDirectoryID.ClientID %>').val(dir_id);
			    __doPostBack('<%=btnDelete.UniqueID %>', '');
			}
        }

        function hidePop(pid) {
            $("#" + pid).hide();
            $("div.pop").hide();
        }

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
	    var DefaultSearchText = "";
	    if ("<%=GatheringYN%>" == "Y") {
	        DefaultSearchText = "우리 모임의 문서를 검색해 보세요."
	    }
	    else {
	        DefaultSearchText = "찾고싶은 끌.문서를 검색해 보세요."
	    }

	    $(document).ready(function () {

	        if ("<%=GatheringYN%>" == "Y") {
		        $("#container").attr('class', 'Gathering');
		        $("#txt_SearchKeyword").val(DefaultSearchText);
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

		    $("#currPageNum").val(1);
		    lisCreate();

		    ////공지팝업
		    //if ($.cookie("pop_doc_notice") != "Y") {
		    //    $("div.pop").show();
		    //    $("#img_doc_notice").css("position", "relative");
		    //    $("#pop_doc_notice .close").css({
		    //        left: "360px",
		    //        top: "15px"
		    //    });
		    //    $("#pop_doc_notice .confirmClose").css({
		    //        position: "absolute",
		    //        top: "605px",
		    //        left: "130px",
		    //        "text-decoration": "underline"
		    //    });

		    //    $("#pop_doc_notice")
			//		.css({
			//		    top: "50px",
			//		    position: "absolute",
			//		    border: "0",
			//		    padding: "0"
			//		})
			//		.show();

		    //    window.scrollTo(0, 0);
		    //}

	    });

	    //폴더 매니저 관리
	    function viewDivManagerShow(dirid, dirname, author) {
	        $("#CommUserAndDepartmentManagerIframe").attr("src", "/Directory/DirectoryManagerIframe.aspx?DivID=" + dirid + "&DivNM=" + encodeURIComponent(dirname));
	        $('#<%= this.hdGatheringAuther.ClientID %>').val(author);
            $("div.pop").show();
            $("#pop_dc_folder1").show();
            $("#pop_dc_folder").hide();
        }
	    function refreshMe(dirid, dirtype) {
	        //location.href = "DirectoryView.aspx?DivID=" + dirid + "&DivType=" + dirtype;
	        location.href = location.href;
	    }
	</script>
    <script type="text/javascript">
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
	    </script>
	
		<!--CONTENTS-->
		<div id="contents">
		    <div class="h2tag">
                <p>
                <%if (GatheringYN == "Y"){ %>
                    <common:GatheringInfomation ID="GatheringInfomation1" runat="server" />
                <%}else{ %>
                    <h2 style="padding-left: 175px;"><img src="/common/images/text/Directory.png" alt="끌.문서" /></h2>
                <%} %>
                </p>
			</div>
			    <%--<p class="search_top">
				    <input name="q" id="txt_SearchKeyword" type="text" value="찾고싶은 끌.문서를 검색해 보세요" onfocus="this.value=''"  onkeypress="if(event.keyCode == 13){fnFileSearch();}"  />
				    <a href="javascript:fnFileSearch()"><img src="/common/images/etc/search_btn.png" alt="" title="검색" /></a>
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
                    <%} else {%>
					<ul id="tabMenu">
						<li><a href="/Directory/DirectoryListNew.aspx?DivType=Pub" <%= m_pub%>><img src="/common/images/btn/Directory_tab1.png" alt="모든 문서공유방" /></a></li>
						<li><a href="/Directory/DirectoryListNew.aspx?DivType=Pri" <%= m_pri%>><img src="/common/images/btn/Directory_tab2.png" alt="내가만든 문서공유방" /></a></li>
						<li><a href="/Directory/DirectoryListNew.aspx?DivType=Vis" <%= m_vis%>><img src="/common/images/btn/Directory_tab3.png" alt="초대된 문서공유방" /></a></li>
					</ul>
                    <%} %>
					<ul id="directory_list">
						
					</ul>
					<p id="plusBtn"><a href="javascript:lisCreate();" class="btn_more"><span><img src="/common/images/common/ajax-loader.gif" style="display:none" id="imgLoading" alt="" />더보기</span></a></p>
				</div>
				<!--/article-->
		</div>
		<!--/CONTENTS-->
		<input type="hidden" id="currPageNum" name="currPageNum" value="1" />
	

<div class="pop">
	<div class="popBg"></div>
	<!--문서_파일업로드-->
	<div id="pop_dc_file_add" class="popCt layer_pop">
		<h3><b id="file_add_dir_nm">문서함명</b></h3>
		<p class="radio_area">
			<input type="radio" name="rdFileUp" onclick="changeFileUpload();" class="radio" id="rd_upload" value="upload" checked="checked" /> <label for="rd_upload">PC에서 파일 불러오기</label>
			<input type="radio" name="rdFileUp" onclick="changeFileUpload();" class="radio" id="rd_excel" value="excel" /> <label for="rd_excel">Excel 열기</label>
			<input type="radio" name="rdFileUp" onclick="changeFileUpload();" class="radio" id="rd_word" value="word" /> <label for="rd_word">Word 열기</label>
			<input type="radio" name="rdFileUp" onclick="changeFileUpload();" class="radio" id="rd_ppt" value="ppt" /> <label for="rd_ppt">PowerPoint 열기</label>
		</p>

		<div id="upload_file_container">
			<div id="manager_container" class="fileArea"></div>
			<div style='font-family:"맑은고딕","Malgun Gothic","nanumgothic","나눔고딕","돋움",dotum,applegothic,sans-serif;font-size:12px;color:#bfbfbf;'>
				<%--파일명에 포함된 특수문자(<%=HttpUtility.HtmlEncode("* ? \" < > | # { } % ~ &")%> 등)는 언더바(_)로 자동 변환됩니다.--%>
			</div>
			
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
	<div id="pop_dc_folder" class="layer_pop">
        <%--Author : 개발자-김성환D, 리뷰자-진현빈D
        Create Date : 2016.12.02 
        Desc : 모임.문서에서 멤버 관리를 문서함 관리로 문구 변경--%>
		<%if(GatheringYN != "Y"){ %>
            <h3>멤버 관리</h3>
        <%}else{ %>
            <h3>문서함 관리</h3>
        <%} %>
		<div id="addWrap">
			<p><input type="text" id="txtTitle" name="txtTitle" runat="server" class="txt t5" value="새로운 문서공유방명을 입력해주세요"/></p>
			<fieldset class="authority" <%if(GatheringYN == "Y") { %>style="display:none;"<% } %>>
				<h4>문서함 멤버를 선택해 주세요</h4>
				<common:UserAndDepartment ID="UserControl" boolCheckSelf="true" runat="server" />
			</fieldset>
            
		</div>
		<p class="btn_c">
			<a href="javascript:hidePop('pop_dc_folder');" class="btn2"><b>취소하기</b></a>
			<a href="javascript:fnSave();" class="btn3"><b>완료하기</b></a>
		</p>
		<a href="javascript:;"><img src="/common/images/btn/pop_close.png" alt="닫기" class="close" /></a>
	</div>
	<!--/문서_폴더관리-->

    <!--문서_매니저관리-->
	<div id="pop_dc_folder1" class="layer_pop" style="padding:0;border:0;width:392px;height:483px;">
		<iframe id="CommUserAndDepartmentManagerIframe" name="CommUserAndDepartmentManagerIframe" src="about:blank" style="width:100%;height:100%;" frameborder="0"></iframe>
	</div>

	<!--문서_공지사항-->
	<div id="pop_doc_notice" class="layer_pop">
		<img src="/Common/images/pop/doc_notice.jpg" id="img_doc_notice" alt="공지사항" />
		<a href="javascript:;"><img src="/common/images/pop2/pop_close.png" alt="닫기" class="close" /></a>
		<a href="javascript:fnConfirmClose();" class="confirmClose">이 창을 다시 보지 않기</a>
	</div>
	<!--/문서_공지사항-->
</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Common_Footer" runat="server">
	<div style="display:none;">
		<asp:HiddenField ID="hidMenuType" runat="server" />
		<asp:HiddenField ID="hdDirectoryID" runat="server" />
		<asp:HiddenField ID="hdDirectoryNM" runat="server" /> 
		<asp:HiddenField ID="hdCommonID" runat="server" />
		<asp:HiddenField ID="hdBoardID" runat="server" />
		<asp:HiddenField ID="hdItemGuid" runat="server" />
		<asp:HiddenField ID="hdFileSearch" runat="server" />
		<asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
		<asp:Button ID="btnFileSave" runat="server" OnClick="btnFileSave_Click" />
		<asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" />
        <asp:HiddenField ID="hdGatheringAuther" runat="server" />
        <asp:HiddenField ID="hdFileID" runat="server" /> 
        <asp:Button ID="btnDownload" runat="server" OnClick="btnDownload_Click" /> 
		<%--   <asp:Button ID="btnFileSearch" runat="server" OnClick="btnFileSearch_Click" />--%> 
	</div>
</asp:Content>
