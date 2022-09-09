<%@ Page Language="C#" MasterPageFile="~/MasterPages/GlossarySearch.Master" AutoEventWireup="true"
    CodeBehind="MyUseGroup.aspx.cs" Inherits="SKT.Glossary.Web.GlossaryMyPages.MyUseGroup" %>
<%@ Register Src="~/Common/Controls/UserAndDepartmentList.ascx" TagName="UserAndDepartment" TagPrefix="common" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">
    <script src="/Common/js/json2.js" type="text/javascript"></script>
    <script type="text/javascript">

        var m_titleCheck = false;
        var m_mode = '<%= mode %>';
    	var initText = "새로운 그룹명을 입력해주세요";

        $(document).ready(function () {

            if (m_mode == 'Insert') {
                $('#<%= this.txtTitle.ClientID %>').one("click", function () {
                    $('#<%= this.txtTitle.ClientID %>').val("");
                });
            }

        	getMyGroup();

        });

    	//사용자 정의 그룹 목록
    	function getMyGroup() {

    		var m_UserID = '<%= UserID %>'

			try {
				$.ajax({
					type: "POST",
					contentType: "application/json; charset=utf-8",
					url: "/GlossaryMyPages/MyUseGroup.aspx/GetMyGroupWeb",
					data: "{UserID : '" + m_UserID + "'}",
					dataType: "json",
					async: false,
					success: function (data) {
						if (data.d.length != 0) {
							drawMyGrpTable(data.d);
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

    	// 그룹 리스트 출력
    	function drawMyGrpTable(d) {
    		var strHTML = "";

    		for (var i = 0; i < d.length; i++) {
    			var obj = d[i];

    			var MyGrpID = obj.MyGrpID;
    			var MyGrpNM = obj.MyGrpNM;

    			if ((i % 2) == 0) {
    				strHTML += "<tr>";
    				strHTML += "	<td class='al'>";
    				strHTML += "		<a href=\"javascript:fnSelect('" + MyGrpID + "');\" id=\"a_" + MyGrpID + "\">" + MyGrpNM + "</a>";
    				strHTML += "	</td>";
    				strHTML += "	<td><a href=\"javascript:fnMyGrpDelete('" + MyGrpID + "');\" class=\"btn1\"><span>삭제</span></a></td>";
    			}
    			else {
    				strHTML += "	<td class='al'>";
    				strHTML += "		<a href=\"javascript:fnSelect('" + MyGrpID + "');\" id=\"a_" + MyGrpID + "\">" + MyGrpNM + "</a>";
    				strHTML += "	</td>";
    				strHTML += "	<td><a href=\"javascript:fnMyGrpDelete('" + MyGrpID + "');\" class=\"btn1\"><span>삭제</span></a></td>";
    				strHTML += "</tr>";
    			}
    		}

    		if ((i % 2) == 1) {
    			strHTML += "<td class='al'></td><td></td></tr>";
    		}

    		$("#tblGroupList").html(strHTML);
    	}

        function fnSelect(id) {

            var type = "MyGroup";
            var m_UserID = "<%= UserID %>";
        	var MyGrpNM = $("#a_"+id).text();

        	getGroupInfo();

        	$('#<%= this.txtTitle.ClientID %>').val(MyGrpNM);
            $('#<%= txtGrpID.ClientID %>').val(id); 
            $('#<%= this.hidMode.ClientID %>').val("Update"); 

            try {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "/Common/Controls/AjaxControl.aspx/GetMyGroupList",
                    data: "{Type : '" + type + "', UserID : '" + m_UserID + "', MyGrpID : '" + id + "'}",
                    dataType: "json",
                    async: false,
                    success: function (data) {
            
                    	if (data.d.length == 0) return;

                        if (data.d[0].length != 0) {
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


        function setSelectRadioCheck(rdoName, rdoValue) {
            $('input:radio[name="' + rdoName + '"]').each(function () {
                $(this).removeAttr("checked");
            });
            var selectedItem = $('input:radio[name="' + rdoName + '"][value="' + rdoValue + '"]');
            selectedItem.attr('checked', 'checked');
            selectedItem.prop('checked', true);
        }

		// MyGroup 등록/수정 팝업
        function getGroupInfo() {

        	$("div.pop").show();
        	$("#pop_group").show();

        	DefaultClear();

        	$('#<%= txtGrpID.ClientID %>').val("");
        	$('#<%= this.txtTitle.ClientID %>').val(initText);
            $('#<%= this.hidMode.ClientID %>').val("Insert"); 

        }

        //그룹저장
        function fnSave() {
            
            /*
              Author : 개발자-김성환D, 리뷰자-진현빈G
              Create Date : 2016.08.04 
              Desc : 특수문자 " ' \ 처리 
           */
            if ($('#<%= this.txtTitle.ClientID %>').val().indexOf('\'') >= 0 || $('#<%= this.txtTitle.ClientID %>').val().indexOf('\"') >= 0 || $('#<%= this.txtTitle.ClientID %>').val().indexOf('\\') >= 0) {
                alert("그룹명에 ' 또는 \" 또는 \\ 를 제거하고 저장해주세요.");
                return;
            }

            $('#<%= this.txtTitle.ClientID %>').val(strip_tag($('#<%= this.txtTitle.ClientID %>').val()));

            //그룹명 체크
            if ($('#<%= this.txtTitle.ClientID %>').val() == initText || $('#<%= this.txtTitle.ClientID %>').val().replace(/^\s+|\s+$/g, '') == "" || $('#<%= this.txtTitle.ClientID %>').val().replace(/\'/gi, "").replace(/\"/gi, "") == "") {
                alert(initText);
                $('#<%= this.txtTitle.ClientID %>').unbind();
                $('#<%= this.txtTitle.ClientID %>').val("");
                $('#<%= this.txtTitle.ClientID %>').focus();
                return;
            }

        	if ($("#CommonUserList li").length == 0) {
        		alert("그룹에 등록할 구성원을 입력해주세요");
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

		// 그룹 삭제
        function fnMyGrpDelete(id) {

        	if(confirm('그룹을 정말 삭제하시겠습니까?')) {
        		$('#<%= this.hidMode.ClientID %>').val("Delete"); 
        		$('#<%= this.hdGrpID.ClientID %>').val(id);
        		__doPostBack('<%=btnDelete.UniqueID %>', '');
			}
        }

        function TitleDBCheck() {
        	//var txtTitle = $('#txtTitle').val().replace(/'/g, "&#39;");
			//string Title, string itemID, string UserID, string mode

        	var m_UserID = "<%= UserID %>";
        	var itemID = $('#<%= txtGrpID.ClientID %>').val();
        	var txtTitle = $('#<%= this.txtTitle.ClientID %>').val();
        	var mode = "MyGroup";

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

    	function hidePop(pid) {
    		$("#" + pid).hide();
    		$("div.pop").hide();
    	}
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Common_Top" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript">
    var lnbDep1 = 0;		//LNB 1depth
</script>

<div id="container" class="Mypage">
    <!--CONTENTS-->
	<div id="contents">
		<h2><img src="/common/images/text/Mypage.png" alt="마이페이지" /></h2>
		<!--article-->
		<div id="article">
			<ul id="tabMenu">
				<li><a href="/GlossaryMyPages/MyProfile.aspx"><img src="/common/images/btn/Mypage_tab1.png" alt="my 프로필" /></a></li>
				<li><a href="/GlossaryMyPages/MyDocumentsList.aspx"><img src="/common/images/btn/Mypage_tab2.png" alt="my 지식 스크랩" /></a></li>
				<li><a href="/GlossaryMyPages/MyScrapList.aspx"><img src="/common/images/btn/Mypage_tab3.png" alt="my 지식 스크랩" /></a></li>
				<%--<li><a href="/GlossaryMyPages/MyPeopleScrapList.aspx"><img src="/common/images/btn/Mypage_tab4.png" alt="my 담당자 스크랩" /></a></li>--%>
                <li><a href="/GlossaryMyPages/MyUseGroup.aspx" class="on"><img src="/common/images/btn/Mypage_tab5.png" alt="my 그룹 " /></a></li>
			</ul>

			<table class="listTable">
				<colgroup><col width="35%" /><col width="15%" /><col width="35%" /><col width="15%" /></colgroup>
				<thead>
				<tr>
					<th>그룹명</th>
					<th>삭제</th>
					<th>그룹명</th>
					<th>삭제</th>
				</tr>
				</thead>
				<tbody id="tblGroupList">                    
				</tbody>
			</table>
			<p class="btn_r"><a href="javascript:getGroupInfo();" class="btn2"><b>새 그룹 추가</b></a></p>
		</div>
		<!--/article-->
	</div>
	<!--/CONTENTS-->
</div>

<div class="pop">
	<div class="popBg"></div>
	<!--/담당자_새그룹추가-->
	<div id="pop_group" class="layer_pop">
		<h3>My 그룹 관리</h3>
		<div id="addWrap">
			<p><input type="text" id="txtTitle" name="txtTitle" runat="server" class="txt t5" onfocus="if(this.value==initText){this.value = '';}" value=""/></p>
			<input type="hidden" id="txtGrpID" runat="server" />
			<fieldset class="authority">
				<h4>그룹의 멤버를 선택해 주세요.</h4>
				<common:UserAndDepartment ID="UserControl" runat="server" />
			</fieldset>
		</div>
		<p class="btn_c">
			<a href="javascript:hidePop('pop_group');" class="btn2"><b>취소하기</b></a>
			<a href="javascript:fnSave();" class="btn3"><b>완료하기</b></a>
		</p>
		<a href="javascript:;"><img src="/common/images/btn/pop_close.png" alt="닫기" class="close" /></a>
	</div>
	<!--/담당자_새그룹추가-->
</div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Common_Footer" runat="server">
        <div style="display:none;">
            <asp:HiddenField ID="hidMenuType" runat="server" />
            <asp:Button ID="btnTebMenu" runat="server" OnClick="btnTebMenu_Click"  />
            <asp:HiddenField ID="hdIsLoginUser" runat="server" />
            <asp:HiddenField ID="hdGrpID" runat="server" />
            <asp:HiddenField ID="hdTitle" runat="server" />
            <asp:HiddenField ID="hidMode" runat="server" />
<%--            <asp:HiddenField ID="hdUserItemID" runat="server" />
            <asp:HiddenField ID="hdUserName" runat="server" />--%>
            <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click"  />  
<%--            <asp:HiddenField ID="hdGroupString" runat="server" />--%>
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click"  /> 
        </div>
</asp:Content>


