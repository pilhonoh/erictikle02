<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossarySearch.Master" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="SKT.Glossary.Web.Gathering.Main" %>
<%@ Register Src="~/Common/Controls/UserAndDepartmentList.ascx" TagName="UserAndDepartment" TagPrefix="common" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">
    <%--<script src="http://code.jquery.com/ui/1.10.4/jquery-ui.js"></script>--%>


    <script type="text/javascript">
        var lnbDep1 = 6;		//LNB 1depth
        function lisCreate() {
            var pageNum = $("#currPageNum").val(); //페이지
            var mode = "<%= DivType%>"; //타입
			var userID = "<%= UserID%>"; //로그인사용자
		    var pageSize = 10; //가져올개수

		    //if (pageNum < 0) {
		    //    alert("더이상 가져올 모임이 없습니다.");
		   // }
		    //else {

		        //첫번째호출이며 모든의견함이거나 내가만든 의견합이라면 5개만 가져옴
		        //if (pageNum == 1 && (mode == "Pub" || mode == "Pri")) {
		        //    pageSize = 5;
		        //}

		        //$("#imgLoading").show();

		        $.ajax({
		            type: "POST",
		            contentType: "application/json; charset=utf-8",
		            url: "/Gathering/Main.aspx/GetGatheringList",
		            data: "{mode : '" + mode + "', userID : '" + userID + "', pageNum :" + pageNum + ", pageSize :" + pageSize + "}",
		            dataType: "json",
		            success: function (data) {
		                var Table = data.d.Table;
		                boxList(Table, mode, userID, pageNum, pageSize);
		                //if (Table.length > 0) {
		                //    pageNum++;
		                //} else {
		                //    pageNum = -1;
		                //}
		                //$("#currPageNum").val(pageNum);
		            },
		            error: function (result) {
		                alert("Error" + ":::" + result);
		            },
		            complete: function () {
		                //$("#imgLoading").hide();
		                //getFileList();
		            }
		        });
		    //}
		}

		function fnConfirmClose() {
		    $.cookie("pop_doc_notice", "Y", { expires: 365, path: "/" });
		    hidePop("pop_doc_notice");
		}

		// 모임 목록 조회
		function boxList(table, mode, userID, pageNum, pageSize) {

		    var dHtml = "";
		    //첫번째페이지이면서 가져올 개수가 5개인경우
		    //if (pageNum == 1 && pageSize == 5) {
		    //if (mode == "Pub" || mode == "Pri") {
		        //dHtml = "<li><a href='GatheringWrite.aspx?DivType=" + mode + "'><img src='/common/images/btn/add_Gathering.png' alt='새로운 모임을 만들어보세요!' /></a></li>";
		        dHtml = "<li id='header' style='border:0px solid #e0ddd8; '>";
		        dHtml += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
		        //dHtml += "<a href='GatheringWrite.aspx?DivType=" + mode + "' ><img id='gID1' src='/common/images/btn/add_over.png' title='새로운 모임을 만들어 보세요.'  /></a>";
		        dHtml += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href='javascript:;' onclick='viewNotiShow();' ><img id='gID2' src='/common/images/btn/alarm_normal.png' title='알림을 받고자 하는 모임을 선택해 보세요.' /></a>";
		        //dHtml += "&nbsp;&nbsp;&nbsp;&nbsp;<a href='javascript:;' onclick='fnSortSave();'><img id='gID3' src='/common/images/btn/sort_normal.png'  title='모임 첫화면 순서를 변경해 보세요.' /></a></li>";
            //}
		    //}

		    //가져온개수가 있다면
		    if (table.length > 0) {
		        for (var i = 0; i < table.length; i++) {
		            dHtml += liCreate(table[i], userID, mode);
		        }

		        ////개수 확인하여 1페이지인경우 빈칸 채우기
		        //var binLiLen = pageSize - table.length;
		        //if (binLiLen > 0) {
		        //    if (pageNum == 1) {
		        //        for (var b = 0 ; b < binLiLen; b++) {
		        //            dHtml += "<li></li>";
		        //        }
		        //    }
		        //    //더보기 버튼 숨기기
		        //    //$("#plusBtn").hide();
		        //}

		    }
		    //else if (pageNum == 1) {
		    //    dHtml += "<li></li><li></li><li></li><li></li><li></li>";
		    //    if (mode == "Vis") {
		    //        dHtml += "<li></li>";
		    //    }
		    //    //$("#plusBtn").hide();
		    //}

		    $("#directory_list_new").append(dHtml);
		}

		// 모임 목록 렌더링
		function liCreate(t, uid, mode) {
		    var rtn = "";
		    //var FileCntStr = "";
		    //var strFileList = "";
		    var AUTHUSERCNTSTR = ""

		    if (t.AUTHUSERCNT > 1) {
		        AUTHUSERCNTSTR = t.FIRSTNAME + "님 외 " + (t.AUTHUSERCNT - 1) + "명";
		    }
		    else {
		        AUTHUSERCNTSTR = t.FIRSTNAME;
		    }

		    //FileCntStr = "문서 <span id='FILECNT_" + t.DIR_ID + "'>0</span>개";
		    //strFileList = getFileList(t.DIR_ID, t.FILELIST);

		    var backcolor = "";
		    //if (mode == "Pub" || mode == "Pri") {
		        ////backcolor = "background-color:#f9f6f0;";
		        //backcolor = "background-color:#e8eaee;";
		    //}

		    rtn += '<li id="'+ t.GatheringID +'" style="background-color:#FFFFFF;cursor:move;">';
		    rtn += '	<dl style="background-color:#e8eaee;">';
            rtn += '		<dt><span onClick="fnViewMove(' + t.GatheringID + ')" style="cursor:pointer;">' + t.GatheringName + '</dt>';
		    //rtn += '		<dd>' + AUTHUSERCNTSTR + ' | ' + FileCntStr + '</dd>';
            rtn += '		<dd>' + AUTHUSERCNTSTR + '</span></dd>';
		    rtn += '	</dl>';
		    //rtn += '	<ul id="FileList_' + t.DIR_ID + '">';
		    //rtn += '	<li style="text-align:center;"><img src="/common/images/common/ajax-loader.gif" style="display:none" /><span style="display:none" class="spn_dir_id">' + t.GatheringID + '</span></li>';
		    //rtn += '	</ul>';
		    //rtn += '	<a href="DirectoryView.aspx?DivType=' + mode + '&DivID=' + t.DIR_ID + '" class="btn"><img src="/common/images/btn/more.png" alt="" title="더보기" /></a>';
		    rtn += '	<p class="btns">';
		    //생성한 사용자라면(의견방 관리자인경우)

		    var managerID = t.managerID.split(',');
		    var managercheck = false;
		    for (var i = 0; i < managerID.length; i++) {
		        if (managerID[i] == uid) {
		            managercheck = true;
		        }
		    }
		    if(managercheck){

		    //if (t.Author == uid) {
		        rtn += "	<a href=\"javascript:;\" onclick=\"DeleteGathering('" + t.GatheringID + "','" + t.GatheringName + "');\" class=\"btn_arrow_small\"><span class=\"red\">모임 삭제</span></a>";
		        rtn += "	<a href=\"javascript:;\" onclick=\"viewTagShow('" + t.GatheringID + "','" + t.GatheringName + "');\" class=\"btn_arrow_small\"><span>모임태그 관리</span></a>";
		        rtn += "	<a href=\"javascript:;\" onclick=\"viewDivShow('" + t.GatheringID + "', '" + t.GatheringName + "', '" + t.Author + "');\" class=\"btn_arrow_small\"><span>멤버 관리</span></a>";
		        rtn += "	<a href=\"javascript:;\" id=\"admindivshow\" onclick=\"viewDivManagerShow('" + t.GatheringID + "', '" + t.GatheringName + "', '" + t.Author + "');\" class=\"btn_arrow_small\"><span>관리자 설정</span></a>";
		    }
		    //rtn += "		<a href=\"javascript:;\" onclick=\"viewFileShow('" + t.DIR_ID + "', '" + t.DIR_NM + "');\" class=\"btn_arrow\"><span>문서 올리기</span></a>";
		    rtn += '	</p>';
		    rtn += '</li>';

		    return rtn;
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
		        default:
		            img = "ms_pc.png";
		    }

		    return img;
		}

		function fnViewMove(i) {
		    //document.location.href = "/Glossary/GlossaryNewsList.aspx?TagTitle=&SearchSort=CreateDate&GatheringYN=Y&GatheringID=" + i;
		    document.location.href = "/Gathering/GatheringMain.aspx?DivType=<%=DivType%>&TagTitle=&SearchSort=CreateDate&GatheringYN=Y&GatheringID=" + i;
		}

        // 노티 설정 관리 
        function viewNotiShow() {

            //$("#gID1").attr("src", "/common/images/btn/add_normal.png");
            //$("#gID2").attr("src", "/common/images/btn/alarm_over.png");
            //$("#gID3").attr("src", "/common/images/btn/sort_normal.png");

            $("#tblNoti tr").remove();

            fnGetNotiInfo();

		    $("div.pop").show();
		    $("#pop_noti").show();
		    $("#pop_dc_folder1").hide();
		    $("#pop_tag_folder").hide();
		}

		// 모임 태그 관리 
		function viewTagShow(dir, nm) {

		    $("#tblTag tr").remove();

		    $('#<%= this.hdGatheringID.ClientID %>').val(dir);
		    $('#<%= this.hdGatheringName.ClientID %>').val(nm);

		    fnGetTagInfo();


		    $("div.pop").show();
		    $("#pop_tag_folder").show();
		    $("#pop_dc_folder1").hide();
		    $("#pop_noti").hide();
		}

        // 모임 관리 
        function viewDivShow(dir, nm, author) {

            $('#<%= this.hdGatheringID.ClientID %>').val(dir);
            $('#<%= this.hdGatheringName.ClientID %>').val(nm);
            $('#<%= this.hdGatheringAuther.ClientID %>').val(author);

            $('#<%= this.txtTitle.ClientID%>').val(nm);

            //2016-03-02 김성환 제목 기본값 체크
            $('#<%= this.hdTitlebefore.ClientID%>').val(nm);
            

		    $("div.pop").show();
		    $("#pop_dc_folder").show();
		    $("#pop_dc_folder1").hide();

		    var type = "Gathering";

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
                        if (data.d.length != 0 && data.d[0].length != 0) {
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

        //모임 관리 저장  
        function fnSave() {

            /*
            Author : 개발자-김성환D, 리뷰자-진현빈G
            Create Date : 2016.08.04 
            Desc : 특수문자 " ' \ 처리
            */
            if ($('#<%= this.txtTitle.ClientID %>').val().indexOf('\'') >= 0 || $('#<%= this.txtTitle.ClientID %>').val().indexOf('\"') >= 0 || $('#<%= this.txtTitle.ClientID %>').val().indexOf('\\') >= 0) {
                alert("모임 이름에 ' 또는 \" 또는 \\ 를 제거하고 저장해주세요");
                return;
            }
            $('#<%= this.txtTitle.ClientID %>').val(strip_tag($('#<%= this.txtTitle.ClientID %>').val()));


            if ($('#<%= this.txtTitle.ClientID %>').val().replace(/^\s+|\s+$/g, '') == "") {
		        alert("모임 이름을 입력하세요");
		        $('#<%= this.txtTitle.ClientID %>').unbind();
				$('#<%= this.txtTitle.ClientID %>').val("");
			    $('#<%= this.txtTitle.ClientID %>').focus();
			    return;
			}

            /*
            Author : 개발자-김성환D, 리뷰자-이정선G
            Create Date : 2016.02.17 
            Desc : 제목 중복 체크 (기존에 주석처리 되어있던거 해제)
            */
            //제목이 같으면 제목체크를 하지 않음.
            if ($('#<%= this.hdTitlebefore.ClientID%>').val() == $('#<%= this.txtTitle.ClientID %>').val()) {
                m_titleCheck = false;
            } else {
                TitleDBCheck();
            }
            
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
		    var itemID = $('#<%= this.hdGatheringID.ClientID %>').val();
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

		// 모임 삭제
		function DeleteGathering(GatheringID, GatheringName) {
		    if (confirm("[" + GatheringName + "] 모임을 정말 삭제하시겠습니까?")) {
		        $('#<%= this.hdGatheringID.ClientID %>').val(GatheringID);
			    __doPostBack('<%=btnDelete.UniqueID %>', '');
			}
        }

        function hidePop(pid) {
            $("#" + pid).hide();
            $("div.pop").hide();
        }
        //폴더 매니저 관리
        function viewDivManagerShow(gatheringid, gatheringname, author) {
            $("#CommUserAndDepartmentManagerIframe").attr("src", "/Gathering/GatheringManagerIframe.aspx?GatheringID=" + gatheringid + "&GatheringNM=" + encodeURIComponent(gatheringname));
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Common_Top" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdTitlebefore" runat="server" />
	<script type="text/javascript">
	    $(document).ready(function () {
	        $("#currPageNum").val(1);
	        lisCreate();

	        //공지팝업
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


	        $('#down').click(function () {
	            var checkedCount = $("input[name=tag_id]:checked").length;
	            if (checkedCount == 0) {
	                alert("이동 하려는 태그명을 선택해주세요")
	            }
	            else {
	                var element = $("input[name=tag_id]:checked").parent().parent();
	                moveRowDown(element)
	            }
	        });

	        $('#up').click(function () {
	            var checkedCount = $("input[name=tag_id]:checked").length;
	            if (checkedCount == 0) {
	                alert("이동하려는 태그명을 선택해주세요")
	            }
	            else {
	                var element = $("input[name=tag_id]:checked").parent().parent();
	                moveRowUp(element)
	            }
	        });

	        //var tagText = "태그명을 입력하신 후 엔터를 눌러주세요.";
	        //$('#txtTagName').val(tagText);

	        //$("#txtTagName").one("click", function () {
	        //    $("#txtTagName").val("");
	        //});

	    });

	    var moveRowUp = function (element) {
	        //if (element.prev().html() != null && element.prev().attr("id") != "header") {
	        if (element.prev().html() != null ) {
	            element.insertBefore(element.prev());
	            changNum();
	        } else {
	            alert("최상단입니다.")
	        }
	    };

	    var moveRowDown = function (element) {
	        if (element.next().html() != null) {
	            element.insertAfter(element.next());
	            changNum();
	        } else {
	            alert("최하단입니다.")
	        }
	    };

	    var changNum = function () {
	        var num = 0;
	        $('input[name=tag_num]').each(function () {
	            num++;
	            $(this).val(num);
	        });
	    };

	    function fnTagCheck(obj)
	    {
	        //$("input[name=tag_id]").not(this).prop("checked", false);
	        $('input[type="checkbox"]').not($(obj)).prop('checked', false);
	    }

	    function fnTagDelete(obj) {

	        var element = $(obj).parent().parent().parent();
	        element.remove();

	        var startIndex = 0;
	        $("#tblTag tr").each(function () {
	            startIndex++;
	            $(this).children("td:eq(0)").find('input[type="hidden"]').val(startIndex);
	        });
	    }
	    function fnTagInput() {

	        $("#txtTagName").val(strip_tag($("#txtTagName").val().replace(/\'/gi, "").replace(/\"/gi, "").replace(/\\/gi, "")));

	        if ($("#txtTagName").val() == "") {
	            alert("태그명을 작성후 엔터를 눌러주세요.");
	            return;
	        }

	        var boolCheck = false;
	        $("#tblTag tr").each(function () {
	            //var table = $(this).children("td:eq(1)").html();
	            var td_data = $(this).children("td:eq(1)").find('input[type="hidden"]').val();
	            //alert(td_data + " " + $("#txtTagName").val());

	            if ($.trim(td_data) == $.trim($("#txtTagName").val())) {
	                alert("중복된 태그가 있습니다.");
	                boolCheck = true;
	                return;
	            }

	        });

	        if (boolCheck == false) {
	            var cnt = $("#tblTag tr").length;
	            cnt++;

	            var strData = fnMakeTagString(cnt, $("#txtTagName").val());
	            $("#tblTag").append(strData);

	            $("#txtTagName").val("");
	        }
	    }

	    function inputLengthCheck(eventInput) {

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

	        if (j > inputMaxLength) {
	            alert("모임 태그명 글자수를 초과하였습니다. 50자 이내로 입력해주세요.");
	            $(eventInput).val(inputText.substr(0, count));
	        }
	    }

	    function fnMakeTagString(TagSort, TagTitle) {

	        var strData = "";
	        strData += '<tr style="width:255px;height:30px;background-color:#f0f0f0;border:1px solid #d8d8d8;">';
	        strData += '<td style="width:7%; padding-left:4px;">';
	        strData += '<input type="checkbox" name="tag_id" onClick="javascript:fnTagCheck(this);" ><input type="hidden" name="tag_num" value="' + TagSort + '" style="width:20px;" ></td>';
	        strData += '<td style="width:92%;cursor:pointer;vertical-align:middle;" ><div style="float:left;width:100%;word-wrap: break-word;">';
	        strData += TagTitle;
	        strData += '&nbsp;<img src="/Common/images/btn/del.png" border="0" style="vertical-align:top;" onclick="fnTagDelete(this);" /><input type="hidden" name="tag_title" value="' + TagTitle + '" /></div>';
	        strData += '</td></tr>';
	        return strData;
	    }
	    function fnMakeTagInfo(Table)
	    {
	        
	        var strData = "";
	        for (var i = 0; i < Table.length; i++) {

	            strData += fnMakeTagString(Table[i].TagSort, Table[i].TagTitle);
	        }
	        $("#tblTag").append(strData);


	    }
	    function fnGetTagInfo() {
	        $.ajax({
	            type: "POST",
	            contentType: "application/json; charset=utf-8",
	            url: "/Gathering/Main.aspx/GetGatheringTagList",
	            data: "{GatheringID : '" + $('#<%= this.hdGatheringID.ClientID %>').val() + "'}",
	            dataType: "json",
	            success: function (data) {
	                var Table = data.d.Table;
	                fnMakeTagInfo(Table);
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

	    function fnTagSave() {
	        var TagSort = "";
	        var TagTitle = "";

	        $("#tblTag tr").each(function () {
	            TagTitle += $(this).children("td:eq(1)").find('input[type="hidden"]').val() + "|";
	            TagSort += $(this).children("td:eq(0)").find('input[type="hidden"]').val() + "|";

	        });

	        $.ajax({
	            type: "POST",
	            contentType: "application/json; charset=utf-8",
	            url: "/Gathering/Main.aspx/SaveGatheringTagList",
	            data: "{GatheringID : '" + $('#<%= this.hdGatheringID.ClientID %>').val() + "', TagTitle : '" + TagTitle + "' , TagSort : '" + TagSort + "', UserID : '<%=UserID%>'}",
	            dataType: "json",
	            success: function () {
	                //fnGetTagInfo();
	                hidePop('pop_tag_folder');
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
	    function fnGetNotiInfo() {
	        $.ajax({
	            type: "POST",
	            contentType: "application/json; charset=utf-8",
	            url: "/Gathering/Main.aspx/GetGatheringNotiList",
	            data: "{UserID : '<%=UserID%>'}",
	            dataType: "json",
	            success: function (data) {
	                var Table = data.d.Table;
	                fnMakeNotiInfo(Table);
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
	    function fnMakeNotiInfo(Table) {
	        var strData = "";
	        var strNotiData = "";
	        for (var i = 0; i < Table.length; i++) {

	            strData += fnMakeNotiString(i + 1, Table[i].GatheringID, Table[i].GatheringName, Table[i].NotiYN);

	            if (Table[i].NotiYN == "Y") {
	                strNotiData += Table[i].GatheringID + "|";
	            }
	        }
	        strGatheringNotiList = strNotiData;
	        $("#tblNoti").append(strData);
	    }
	    var strGatheringNotiList = "";
	    function fnMakeNotiString(row, GatheringID, GatheringName, NotiYN) {
	        
	        var strData = "";
	        
	        strData += "<tr style=\"width:255px;height:30px;background-color:#f0f0f0;border:1px solid #black;cursor:pointer;\" onClick=\"javascript:fnNotiCheckRow('" + GatheringID + "', this);\">";
	        strData += '<td style="width:7%;" align="center">' + row + '</td>';
	        strData += '<td style="width:85%;vertical-align:middle;">&nbsp;&nbsp;';
	        strData += GatheringName;
	        strData += '<td style="width:7%; padding-left:4px;">';
	        if (NotiYN == "Y") {
	            strData += "<input type=\"checkbox\" id=\"tag_id_" + GatheringID + "\"  checked=\"checked\" value=\"" + GatheringID + "\"  onClick=\"javascript:fnNotiCheckRow('" + GatheringID + "', this);\">";
	        }
	        else
	            strData += "<input type=\"checkbox\" id=\"tag_id_" + GatheringID + "\"  value=\"" + GatheringID + "\" onClick=\"javascript:fnNotiCheckRow('" + GatheringID + "', this);\">";
	        strData += '</td></tr>';

	        return strData;
	    }
	    
	    function fnNotiCheckRow(id, obj) {
	        
	        if ($("#tag_id_" + id).is(":checked") == true) {
	            $("#tag_id_" + id).prop("checked", false);
	        }
	        else {
	            $("#tag_id_" + id).prop("checked", true);
	        }

            //Row 클릭시만 동작
	        if($(obj).val() == "")
    	        fnNotiCheck(id);
	    }

	    function fnNotiCheck(id)
	    {
	        if ($("#tag_id_" + id).is(":checked") == true) 
	            strGatheringNotiList += $("#tag_id_" + id).val() + "|";
	        else
	            strGatheringNotiList = strGatheringNotiList.replace($("#tag_id_" + id).val() + "|", "");

	        //alert(strGatheringNotiList);
	    }
	    function fnNotiSave() {
	        
	        $.ajax({
	            type: "POST",
	            contentType: "application/json; charset=utf-8",
	            url: "/Gathering/Main.aspx/SaveGatheringNotiList",
	            data: "{GatheringNotiList : '" + strGatheringNotiList + "',  UserID : '<%=UserID%>'}",
	            dataType: "json",
	            success: function () {
	                fnGetNotiInfo();
	                hidePop('pop_tag_folder');
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
	    
	   //function fnSortSave() {

	   //     $('.btn_area').css("display", "block");
	   //     $('#directory_list_new li[id!=header] dl').css("cursor", "move");

	   //     sortCheck = true;
	   //     $("#directory_list_new").sortable({
	   //         containment: "#article"
	   //         //, cancel: ".disable-sort-item"
	   //         //,start: function (event, ui) {
	   //         //}
	   //         //,change: function (event, ui) {
	   //         //},
       //         , items: 'li[id!=header]'
	   //         //, cursor: 'move'
       //             , disabled: false
	   //     });
	       
	   // }

	    //function fnSortSaveCancel() {
	    //    //if (confirm("취소하시겠습니까?")) {
	            
	    //        $('.btn_area').css("display", "none");
	    //        $('#directory_list_new li[id!=header] dl').css("cursor", "");

	    //        $("#directory_list_new").sortable({
        //             disabled: true
	    //        });

	    //        $("#directory_list_new").append("");
	    //        lisCreate();
	    //        //alert("모임 순서 변경이 취소되었습니다.");
	    //        //return false;
	    //    //}
	    //}

	    function fnSortSaveAction() {

	        if ($('#<%= this.hdSortableData.ClientID %>').val() == "") {
	            //alert("모임 순서 변경이 없습니다.");
	            return;
	        }
	        //if (confirm("현재 모임순서로 저장하시겠습니까?")) {
	            //alert($('#<%= this.hdSortableData.ClientID %>').val());
	            $.ajax({
	                type: "POST",
	                contentType: "application/json; charset=utf-8",
	                url: "/Gathering/Main.aspx/SaveGatheringSortList",
	                data: "{GatheringSortList : '" + $('#<%= this.hdSortableData.ClientID %>').val() + "',  UserID : '<%=UserID%>'}",
	                dataType: "json",
	                success: function () {
	                    //alert("저장되었습니다.");
	                    //$("#directory_list_new").html("");
	                    //lisCreate();
	                },
	                error: function (result) {
	                    alert("저장 중 오류가 발생하였습니다.");
	                },
	                complete: function () {
	                    //$("#imgLoading").hide();
	                    //getFileList();

	                    //$('.btn_area').css("display", "none");
	                    //$('#directory_list_new li[id!=header] dl').css("cursor", "");

	                    //$("#directory_list_new").sortable({
	                    //    disabled: true
	                    //});
	                }
	            });

            //}
	    }

	  

	   $(function () {
	        $("#directory_list_new").sortable({
	            containment: "#article"
	            //, cancel: ".disable-sort-item"
	            //,start: function (event, ui) {
	            //}
                , update: function (event, ui) {
                    var productOrder = $(this).sortable('toArray').toString();
                    $('#<%= this.hdSortableData.ClientID %>').val(productOrder);
                    fnSortSaveAction();
                }
	            //,change: function (event, ui) {
	            //},
                , items: 'li[id!=header]'
	            //, cursor: 'move'
                , disabled: false
	        });
	    });

	</script>
	<div id="container" class="Gathering">
		<!--CONTENTS-->
		<div id="contents">
			<h2>
                <p>
                    <img src="/common/images/text/Gathering_text.png" alt="끌.모임"  />
                    <!--<span>
					 검색 넣어주세요 
				</span>-->
                </p>
			</h2><!--
				<p class="search_top">
     
					<input name="q" id="txt_SearchKeyword" type="text" value="찾고싶은 끌.문서를 검색해 보세요" onfocus="this.value=''"  onkeypress="if(event.keyCode == 13){fnFileSearch();}"  />
					<a href="javascript:fnFileSearch()"><img src="/common/images/etc/search_btn.png" alt="" title="검색" /></a>
					<script type="text/javascript">
					    function fnFileSearch() {
					        var t = $("#txt_SearchKeyword");

					        if (t.val().trim() == "" || t.val() == "찾고싶은 끌.문서를 검색해 보세요") {
					            alert("검색어를 입력해주세요.");
					            t.val("");
					            t.focus();
					            return;
					        }

					        t.val()

					        location.href = "/Directory/DirectorySearchResult.aspx?q=" + encodeURIComponent(t.val());
					    }
					</script>
				</p>
            -->
				<!--article-->
				<div id="article" style="padding-top:30px;">
					<%--<ul id="tabMenu">
						<li><a href="/Gathering/Main.aspx?DivType=Pub" <%= m_pub%>><img src="/common/images/btn/Gathering_tab1.png" alt="모든 모임" /></a></li>
						<li><a href="/Gathering/Main.aspx?DivType=Pri" <%= m_pri%>><img src="/common/images/btn/Gathering_tab2.png" alt="내가 만든 모임" /></a></li>
						<li><a href="/Gathering/Main.aspx?DivType=Vis" <%= m_vis%>><img src="/common/images/btn/Gathering_tab3.png" alt="초대된 모임" /></a></li>
					</ul>--%>
                    <div class="btn_area" >
                        <%--<a onclick="fnSortSaveCancel();" style="cursor:pointer;float:right;">취소</a>
                        <a class="pointColor" onclick="fnSortSaveAction();" style="cursor:pointer;float:right;">저장</a><br /><br />--%>
                       * 모임 Box를 Drag&Drop하면 모임 순서가 변경됩니다.
                    </div>
					<ul id="directory_list_new">
					</ul>
					<%--<p id="plusBtn"><a href="javascript:lisCreate();" class="btn_more"><span><img src="/common/images/common/ajax-loader.gif" style="display:none" id="imgLoading" alt="" />더보기</span></a></p>--%>
				</div>
				<!--/article-->
		</div>
		<!--/CONTENTS-->
		<input type="hidden" id="currPageNum" name="currPageNum" value="1" />
	</div>

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
				파일명에 포함된 특수문자(<%=HttpUtility.HtmlEncode("\\ / : * ? \" < > | # { } % ~ &")%> 등)는 언더바(_)로 자동 변환됩니다.
			</div>
		</div>
		<div id="select_file_contaioner" style="display:none;">
			<span class=""><input type="text" name="txtFileName" id="txtFileName" value="" class="txt t2" maxlength="100"/> <label for="txtFileName" id="lblFileExt"></label></span>
		</div>
		<p class="btn_c">
			<a href="javascript:hidePop('pop_dc_file_add');" class="btn2"><b>취소하기</b></a>
			<a href="javascript:;" onclick="fnFileSave();" class="btn3"><b>저장하기</b></a>
		</p>
		<a href="javascript:;"><img src="/common/images/btn/pop_close.png" alt="닫기" class="close" /></a>
	</div>
	<!--/문서_파일업로드-->

	<!--모임 관리-->
	<div id="pop_dc_folder" class="layer_pop">
		<h3>멤버 관리</h3>
		<div id="addWrap">
			<p style="padding-bottom:10px;"><input type="text" id="txtTitle" name="txtTitle" runat="server" class="txt t5" value="새로운 모임명을 입력해주세요"   style="font-weight:bold" maxlength="100" /></p>
			<fieldset class="authority">
				<h4>모임 멤버를 선택해 주세요</h4>
				<common:UserAndDepartment ID="UserControl" runat="server"  boolCheckSelf="true" />
			</fieldset>
		</div>
		<p class="btn_c">
			<a href="javascript:hidePop('pop_dc_folder');" class="btn2"><b>취소하기</b></a>
			<a href="javascript:fnSave();" class="btn3"><b>완료하기</b></a>
		</p>
		<a href="javascript:;"><img src="/common/images/btn/pop_close.png" alt="닫기" class="close" /></a>
	</div>
	<!--//모임 관리-->
    
    <!--모임 관리-->
	<div id="pop_tag_folder" class="layer_pop" style="width:400px;" >
		<h3>모임태그 관리</h3>
        <div align="left" >
            <input type="text" name="txtTagName" id="txtTagName" class="txt"  style="width:380px;" onkeypress="if(event.keyCode == 13){fnTagInput();return false;}"  onkeyUp="inputLengthCheck(this);" maxlength="50"/>
            <span style=" font-size:12px;color:#bfbfbf;">(태그명을 작성후 엔터를 눌러주세요.)</span>
        </div>
        <%--모임삭제 시 태그도 삭제--%>
        <%--삭제 시 태그로 등록된 정보가 있으면 삭제 불가--%><br />
        <div style="height: 210px; width: 100%; overflow-y: auto; border:1px solid #d8d8d8; background-color:#F7F7F7">
            <table style="width: 100%; vertical-align:top;" id="tblTag">
		        
	        </table>
        </div>
        <div align="right" >
	        <input type="button" id="up" value="△" style="width:30px;font-size:12px " /> 
	        <input type="button" id="down" value="▽" style="width:30px;font-size:12px" /> 
        </div>

        <p class="btn_c">
			<a href="javascript:hidePop('pop_tag_folder');" class="btn2"><b>취소하기</b></a>
			<a href="javascript:fnTagSave();" class="btn3"><b>완료하기</b></a>
		</p>
		<a href="javascript:;"><img src="/common/images/btn/pop_close.png" alt="닫기" class="close" /></a>
	</div>

     <!--모임 관리-->
	<div id="pop_noti" class="layer_pop" style="width:400px;" >
		<h3>모임 알림 설정</h3>
        <div style="height: 210px; width: 100%; overflow-y: auto; border:1px solid #d8d8d8; background-color:#F7F7F7">
            <table style="width: 100%; vertical-align:top;" id="tblNoti">
	        </table>
        </div>
        <span style=" font-size:12px;"><b>* 알림을 설정하면 신규 게시글, 신규 댓글, 작성한 댓글에 대한 답글 등록<br />&nbsp;&nbsp;에 대한 쪽지 알림을 받을 수 있습니다.</b></span>
        <p class="btn_c">
			<a href="javascript:hidePop('pop_noti');" class="btn2"><b>취소하기</b></a>
			<a href="javascript:fnNotiSave();" class="btn3"><b>완료하기</b></a>
		</p>
		<a href="javascript:;"><img src="/common/images/btn/pop_close.png" alt="닫기" class="close" /></a>
	</div>

	<!--공지사항-->
	<div id="pop_doc_notice" class="layer_pop">
		<img src="/Common/images/pop/doc_notice.jpg" id="img_doc_notice" alt="공지사항" />
		<a href="javascript:;"><img src="/common/images/pop2/pop_close.png" alt="닫기" class="close" /></a>
		<a href="javascript:fnConfirmClose();" class="confirmClose">이 창을 다시 보지 않기</a>
	</div>
	<!--//공지사항-->

    <!--문서_매니저관리-->
	<div id="pop_dc_folder1" class="layer_pop" style="padding:0;border:0;width:392px;height:483px;">
		<iframe id="CommUserAndDepartmentManagerIframe" name="CommUserAndDepartmentManagerIframe" src="about:blank" style="width:100%;height:100%;" frameborder="0"></iframe>
	</div>
</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Common_Footer" runat="server">
    
	<div style="display:none;">
		<asp:HiddenField ID="hidMenuType" runat="server" />
		<asp:HiddenField ID="hdGatheringID" runat="server" />
		<asp:HiddenField ID="hdGatheringName" runat="server" /> 
		<asp:HiddenField ID="hdCommonID" runat="server" />
		<asp:HiddenField ID="hdBoardID" runat="server" />
		<asp:HiddenField ID="hdItemGuid" runat="server" />
		<asp:HiddenField ID="hdFileSearch" runat="server" />
        
		<asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
		<asp:Button ID="btnFileSave" runat="server" OnClick="btnFileSave_Click" />
		<asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" />
        <asp:HiddenField ID="hdGatheringAuther" runat="server" />

        <asp:HiddenField ID="hdSortableData" runat="server" />

   
		<%--   <asp:Button ID="btnFileSearch" runat="server" OnClick="btnFileSearch_Click" />--%> 
	</div>
    
</asp:Content>
