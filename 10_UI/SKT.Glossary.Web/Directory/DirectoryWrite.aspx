<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossarySearch.master" AutoEventWireup="true"
    CodeBehind="DirectoryWrite.aspx.cs" Inherits="SKT.Glossary.Web.Directory.DirectoryWrite" %>
<%@ Register Src="~/Common/Controls/UserAndDepartmentList.ascx" TagName="UserAndDepartment" TagPrefix="common" %>
<%@ Register Src="~/Common/Controls/GatheringInfomation.ascx" TagName="GatheringInfomation" TagPrefix="common" %>
<%@ Register Src="~/Common/Controls/GatheringMenuTab.ascx" TagName="GatheringMenuTab" TagPrefix="common" %>

<%@ Register assembly="SKT.Tnet" namespace="SKT.Tnet.Controls" tagprefix="SKTControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">
     <!-- 기본 CSS 및 JS 정의 -->        
    <link href="/Common/Css/board_G.css" rel="stylesheet" />
     <style type="text/css">
        .FileUloadTable {width:790px;}
    </style>
    <!-- Script -->
   <%--<script type="text/javascript" src="/Common/Js/jquery-1.11.2.js"></script>--%>
    <script type="text/javascript" src="/common/Js/TnetBoard_Control_dir.js"></script>
    <!-- 디자인팀적용 JS -->
    <script type="text/javascript" src="/Common/Js/css.browser.detect.js"></script>

    <script type="text/javascript">
        var filelist = "";
        var m_mode = '<%= mode %>';
        var m_focus = false;

        var m_ItemID = '<%= ItemID %>';
        var m_CommonID = '<%= CommonID %>';
        var m_UserID = '<%= UserID %>';         

        var AlarmOpen = true;
        var ShareOpen = true;
        var DownLoadOpen = true;

        var m_titleCheck = false;

        var initTitle = "새로운 문서함 이름을 입력해주세요";

        $(document).ready(function () {
            location.href = "/Directory/DirectoryListNew.aspx?DivType=Pri";

            if ("<%=GatheringYN%>" == "Y") {
                $("#container").attr('class', 'Gathering');
                $("#txt_SearchKeyword").val("우리 모임의 문서를 검색해 보세요.");
            } else {
                $("#container").attr('class', 'Directory');
            }

            $('#<%= this.txtTitle.ClientID %>').one("click", function () {
                $('#<%= this.txtTitle.ClientID %>').val("");
            });
        });

        //닫기  
        function CloseWindow() {

            location.href = "/Directory/DirectoryListNew.aspx?DivType=<%= DivType %>&GatheringID=<%=GatheringID%>&GatheringYN=<%=GatheringYN%>&MenuType=Directory";
        }

        //저장  
        function fnSave() {
            var uploadType = $("input:radio[name='rdFileUp']:checked").val();
            /*
               Author : 개발자-김성환D, 리뷰자-진현빈G
               Create Date : 2016.08.04 
               Desc : 특수문자 " ' \ 처리 
            */
            if ($('#<%= this.txtTitle.ClientID %>').val().indexOf('\'') >= 0 || $('#<%= this.txtTitle.ClientID %>').val().indexOf('\"') >= 0 || $('#<%= this.txtTitle.ClientID %>').val().indexOf('\\') >= 0) {
                alert("문서함 이름에 ' 또는 \" 또는 \\ 를 제거하고 저장해주세요.");
                return;
            }
            $('#<%= this.txtTitle.ClientID %>').val(strip_tag($('#<%= this.txtTitle.ClientID %>').val()));

            $("#txtFileName").val(strip_tag($("#txtFileName").val()));
            //2016.02.17[보류]
            /*
            Author : 개발자-김성환D, 리뷰자-이정선G
            Create Date : 2016.02.24
            Desc : 끌문서 파일명 특수문자 제한
            */
            //특수문자 체크
            if (!checkStringFormat($("#txtFileName").val())) {
                alert('파일 이름에 제한된 특수문자가 포함되어 있습니다.\r\n\r\n제한된 특수문자: * ? < > | # { } % ~ & \" \' \\ ');
                //alert('파일 이름에는 다음 문자를 사용할 수 없습니다.\r\n\r\n         제한된 특수문자: \ / : * " < > |');
                return;
            }

            if ($('#<%= this.txtTitle.ClientID %>').val() == initTitle || $('#<%= this.txtTitle.ClientID %>').val().replace(/^\s+|\s+$/g, '') == "") {
        	    alert("문서함 이름을 입력하세요");
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

            //// 업로드 관리자 객체를 얻습니다.
            //uploadManager = DEXTUploadFL.getUploadManager("DEXTUPMAN");
            //// 전체 파일 개수를 반환합니다.
            //var uploadfilecnt = uploadManager.getTotalFileCount();
            // 2015-08-10 Mr.No doc, ppt, xls file 확장자를 구분합니다.
            //var fileExtensionYN = false;
            
            if (uploadType == "upload") {

                //if (uploadfilecnt == 0) {
                //    alert("업로드할 파일을 추가해주세요.");
                //    return;
                //}

                //for (var i = 0; i < uploadfilecnt; i++) {
                //    var fileInfo = uploadManager.getFileInfoByIndex(i);
                //    if (fileInfo.fileSize == 0) {
                //        alert('파일 크기가 0인 ' + i + '번째 파일은 업로드하실수 없습니다.');
                //        return;
                //    }

                //    // 2015-08-10 Mr.No doc, ppt, xls file 확장자를 구분합니다.
                //    var fileExtension = fileInfo.fileName.substring(fileInfo.fileName.indexOf("."), fileInfo.length);
                //    if (fileExtension == ".doc" || fileExtension == ".ppt" || fileExtension == ".xls") {
                //        fileExtensionYN = true;
                //    }
                //}
                //FileCtrl_FileChange();
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

            //// 2015-08-10 Mr.No doc, ppt, xls file 확장자를 구분합니다.
            //if (fileExtensionYN) {
            //    alert("현재 사내 MS-Office 기준보다\n낮은 버전의 doc, ppt, xls 파일을 업로드 하셨습니다.\n2명 이상의 동시편집을 위해서는 ISAC으로 연락하셔서 MS-Office의 버전을 업그레이드(docx, pptx, xlsx 파일) 해 주시기 바랍니다.\n감사합니다.");
            //}

            //IsEditMode = false;

            //조직도 
            if ('<%=GatheringYN%>' != "Y") {
                fnShareSave();
            }

            //if (uploadManager.hasChanged()) {
            //    uploadManager.transferByForce();
            //} else {
                //저장 
                __doPostBack('<%=btnSave.UniqueID %>', '');
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

                $('#<%= hidFileRadioCheck.ClientID %>').val(rd.val());
            }
        }

        // 제목중복 체크
        function TitleDBCheck() {
            //var txtTitle = $('#txtTitle').val().replace(/'/g, "&#39;");
            //string Title, string itemID, string UserID, string mode

            var m_UserID = "<%= UserID %>";
    	    var itemID = "";
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
                        /*
                        Author : 개발자-김성환D, 리뷰자-이정선G
                        Create Date : 2016.02.17 
                        Desc : alert 메세지 제거
                        */
                        //alert("Error DB Check");
                    }
                });
            }
            catch (exception) {
                alert('Exception error' + exception.toString());
            }
        }

        function hidePop(pid) {
            $("#" + pid).hide();
            $("div.pop").hide();
        }

        function inputLengthCheck(eventInput, gubun) {
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
                if (gubun == "title")
                    alert("문서함 이름 글자수를 초과하였습니다. 50자 이내로 입력해주세요.");
                else
                    alert("파일 이름 글자수를 초과하였습니다. 30자 이내로 입력해주세요.");
                $(eventInput).val(inputText.substr(0, count));
            }
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
	</script>

	
		<!--CONTENTS-->
		<div id="contents">
            <div class="h2tag">
                <%if (GatheringYN == "Y")
              { %>
                <%--<a href="/Gathering/Main.aspx"><img src="/common/images/text/Gathering_text.png" alt="끌.모임" style="left: 50px; top: 30px; position: absolute; width: 83px; height: 26px;" /></a>--%>
                <common:GatheringInfomation ID="GatheringInfomation1" runat="server" />
            <%}else{ %>
            <img src="/common/images/text/Directory.png" alt="끌.문서" />
        <%} %>
			</div>
				<%--<p class="search_top">
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

					        location.href = "/Directory/DirectorySearchResult.aspx?q=" + t.val() + "&GatheringYN=<%=GatheringYN%>&GatheringID=<%=GatheringID%>&MenuType=Directory";
                        }
					</script>
				</p>--%>
				<!--article-->
				<div id="article">
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
					<table class="writeTable">
						<colgroup><col width="180px" /><col width="*" /></colgroup>
						<tbody>
						<tr>
							<th><span style="color:orangered;">*</span> 문서함 이름</th>
                            <%--Author : 개발자-김성환D, 리뷰자-이정선G
                            Create Date : 2016.02.17 
                            Desc : MaxLength 처리--%>
							<td><input type="text" id="txtTitle" name="txtTitle" runat="server" class="txt t1" value="새로운 문서함 이름을 입력해주세요" maxlength="100" onkeyUp="inputLengthCheck(this, 'title');"/></td>
						</tr>
						<tr>
							<th>함께 작업할<br />문서 올리기</th>
							<td>
								<p class="radio_area">
                                    <input type="radio" name="rdFileUp" onclick="changeFileUpload();" class="radio" id="rd_upload" value="upload" checked="checked" /> <label for="rd_upload">PC에서 파일 불러오기</label>
									<input type="radio" name="rdFileUp" onclick="changeFileUpload();" class="radio" id="rd_excel" value="excel" /> <label for="rd_excel">Excel 열기</label>
									<input type="radio" name="rdFileUp" onclick="changeFileUpload();" class="radio" id="rd_word" value="word" /> <label for="rd_word">Word 열기</label>
									<input type="radio" name="rdFileUp" onclick="changeFileUpload();" class="radio" id="rd_ppt" value="ppt" /> <label for="rd_ppt">PowerPoint 열기</label>
								</p>
								
								<div id="upload_file_container">
									<div id="manager_container" class="fileArea"></div>
									<%--<script src="/Common/Controls/DextUploadFl/DEXTUploadFL.js" type="text/javascript"></script>--%>
									<script type="text/javascript">
									    var g_itemGuid = $("#<%=hdItemGuid.ClientID%>").val();
									</script>
									<%--<script type="text/javascript">
									    //var onBeforeUnloadFired = false;
									    //// DextUploadFL
									    //var uploadManager = null;

									    //function ResetOnBeforeUnloadFired() {
									    //    onBeforeUnloadFired = false;
									    //}

									    // 업로드 관리자에서 오류가 발생하면 호출됩니다.
									    function onErrorForDEXTUPMAN(err) {
									        alert(err.code + "\r\n" + err.message + "\r\n" + err.detail);
									    }

									    // 업로드 관리자가 준비되면 호출됩니다.
									    function onApplicationReadyForDEXTUPMAN() {

									        // 업로드 관리자 객체를 얻습니다.
									        uploadManager = DEXTUploadFL.getUploadManager("DEXTUPMAN");

									        // Guid 값을 얻습니다
									        var itemGuid = $("#<%=hdItemGuid.ClientID%>").val();

                            			    // 업로드 경로를 설정합니다. (상대경로가 아닌 전체경로가 필요합니다.)
                            				var uploadUrl = "<%= ConfigurationManager.AppSettings["UploadControlServerHandlerUrl"]%>";

                            			    // 파라미터를 설정합니다.
                            			    uploadUrl += "?BoardID=" + $("#<%=hdBoardID.ClientID%>").val() + "&ItemGuid=" + itemGuid;
                            			    uploadUrl += "&UniqueItemPath=" + encodeURIComponent("ShareFiles\\" + itemGuid);
                            			    uploadManager.setUploadUrl(uploadUrl);
                            			    // UI를 설정합니다.
                            			    uploadManager.setUIStyle({
                            			        buttonPanel: {
                            			            toolButton: { visible: false, enabled: false }
														, transferButton: { visible: false, enabled: false }
                            			        },
                            			        context: {
                            			            edit: { visible: false, enabled: false },
                            			            upload: { visible: false, enabled: false }
                            			        }
                            			    });

                            			    // 확장자 제한(true : 제한 , false : 허용)
                            			    //uploadManager.setFilter([{ description: "exe파일은 업로드 하실 수 없습니다.", extension: "*.exe"}], true);
                            			    uploadManager.setFilter([
												{ description: "Allowed File", extension: "<%= ConfigurationManager.AppSettings["DocsAttachAllowExtension_plus"]%>" }
                            				]);

                                        }

                                        // 업로드 관리자가 업로드를 완료하면 호출됩니다.
                                        function onTransferCompletedForDEXTUPMAN() {
                                            __doPostBack('<%=btnSave.UniqueID %>', '');
										}

										//DEXTUploadFL.createUploadManager(
										//	"manager_container", // target div container
										//	"DEXTUPMAN", // id
										//	"../Common/Controls/DextUploadFl/DEXT_LIST_UP_MANAGER.swf", // swf path
										//	"#ffffff", // background color
										//	"transparent", // window, transparent
										//	"", // ko, en
										//	"", // reserved name
										//	"simple", // simple, monitor
										//	"ForDEXTUPMAN" // postfix name
									    //);

									</script>--%>
                                    <SKTControls:filectrl ID="fileCtrl" runat="server" />
								</div>
								<div id="select_file_contaioner" style="display:none;">
									<span class=""><input type="text" name="txtFileName" id="txtFileName" value="" class="txt t2" maxlength="60" onkeyUp="inputLengthCheck(this, 'file');" /> <label for="txtFileName" id="lblFileExt"></label></span>
								</div>
								<div style='font-family:"맑은고딕","Malgun Gothic","nanumgothic","나눔고딕","돋움",dotum,applegothic,sans-serif;font-size:12px;color:#bfbfbf;'>
									<%--파일명에 포함된 특수문자(<%=HttpUtility.HtmlEncode("* ? \" < > | # { } % ~ &")%> 등)는 언더바(_)로 자동 변환됩니다.--%>
								</div>
							</td>
						</tr>
                        <%if(GatheringYN != "Y"){ %>
						<tr>
							<th>문서함 멤버<br />설정하기</th>
							<td>
								<%--<a href="#pop_dc_member" class="btn1 btn_pop"><span>설정하기</span></a>--%>
                                <fieldset class="authority" style="border:0;">
				                <common:UserAndDepartment ID="UserControl" runat="server" ViewType="WriteNew" />
			                    </fieldset>
							</td>
						</tr>
                        <%} %>
						</tbody>
					</table>
					<p class="btn_r">
						<a href="javascript:history.back(-1)" class="btn2"><b>취소하기</b></a>
						<a href="javascript:fnSave();" class="btn3"><b>저장하기</b></a>
					</p>
				</div>
				<!--/article-->
		</div>
		<!--/CONTENTS-->

<%--<div class="pop">
	<div class="popBg"></div>

	<!--조회권한 설정하기-->
	<div id="pop_dc_member" class="layer_pop">
		<h3>문서함 멤버 설정하기</h3>
		<div id="addWrap">
			<fieldset class="authority">
				<common:UserAndDepartment ID="UserControl" runat="server" />
			</fieldset>
		</div>
		<p class="btn_c">
			<a href="javascript:hidePop('pop_dc_folder');" class="btn2"><b>취소하기</b></a>
			<a href="javascript:hidePop('pop_dc_folder');" class="btn3"><b>설정하기</b></a>
		</p>
		<img src="/common/images/btn/pop_close.png" alt="닫기" class="close" />
	</div>
	<!--/조회권한 설정하기-->
</div>--%>


	

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Common_Footer" runat="server">
        
    <div style="display:none;">
            <asp:HiddenField ID="hidMenuType" runat="server" />     
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
            <asp:HiddenField ID="hdDirctoryID" runat="server" />
            <asp:HiddenField ID="hdCommonID" runat="server" />
            <asp:HiddenField ID="hdBoardID" runat="server" />
            <asp:HiddenField ID="hdItemGuid" runat="server" />
    </div>

</asp:Content>
