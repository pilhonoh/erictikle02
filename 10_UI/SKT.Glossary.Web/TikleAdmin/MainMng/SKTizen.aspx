<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/GlossaryAdmin.Master" CodeBehind="SKTizen.aspx.cs" Inherits="SKT.Glossary.Web.TikleAdmin.MainMng.SKTizen" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
<script language="javascript" type="text/javascript">
	function fnSave() {
		__doPostBack('<%=btnSave.UniqueID%>', '');
	}

	function fnDelete() {
		var r = $("input:checkbox[name$='chkRptNotID']");

		if (r.filter(":checked").length < 1) {
			alert("삭제할 게시물을 선택해주세요");
			return;
		}

		__doPostBack('<%=btnDelete.UniqueID%>', '');
	}

	function fnSearchWord() {

		var RetStr = $('#search-input').val();

		if (RetStr.lastIndexOf("\\") != -1) {
			if (RetStr.lastIndexOf("\\") % 2 == 0) {
				RetStr = $('#search-input').val() + "\\";
			}
			else {

			}
		}

		if (RetStr.lastIndexOf("\'") != -1) {
			RetStr = RetStr.replace(/\'/gi, "&#39;");

		}

		if (RetStr.lastIndexOf("\"") != -1) {
			RetStr = RetStr.replace(/\"/gi, "&quot;");
		}

		return RetStr;
	}

	var SearchTextValue = "이름으로 검색";

	$(document).ready(function () {
		//자동검색 bind
		$("#search-input").autocomplete({
			source: function (request, response) {

				var kw = fnSearchWord();

				if (kw == SearchTextValue) return;

				$.ajax({
					type: "POST",
					contentType: "application/json; charset=utf-8",
					url: "/Common/Controls/AjaxControl.aspx/GetAutoCompleteUserData",
					data: "{'username':'" + kw + "'}",
					dataType: "json",
					success: function (data) {

						var rsltArr = {};

						if ($('#search-input').val().replace(/^\s+|\s+$/g, '') == "") {
							return;
						}
						else {
							for (var i = 0, j = 0; i < data.d.length; i += 2, j++) {

								pushToAry(data.d[i], data.d[i + 1]);
								rsltArr[j] = data.d[i];
							}

							response(rsltArr);
						}
					},
					error: function (result) {
						//                            alert("Error");
						//alert(result);
					}

				});
			},
			selectFirst: true,
			minLength: 2,
			autoFocus: true
		});

		//클릭을 하였을 경우 해당 항목 저장
		$('.ui-autocomplete').click(function (e) {
			var SearchTxt = $('#search-input').val();

			if (SearchTxt.replace(/^\s+|\s+$/g, '') != "" && SearchTxt != SearchTextValue) {

				// 항목 저장
				if (obj[SearchTxt] != undefined) {
					fnAddUser(SearchTxt, obj[SearchTxt]);
				}

				$('#search-input').val("");
			}
		});

		//엔터를 첫을 경우 해당 항목 저장
		$('#search-input').keydown(function (e) {
			if (e.keyCode == 13) {

				var SearchTxt = $('#search-input').val();
				if (SearchTxt.replace(/^\s+|\s+$/g, '') != "" && SearchTxt != SearchTextValue) {

					// 항목 저장
					if (obj[SearchTxt] != undefined) {
						fnAddUser(SearchTxt, obj[SearchTxt]);
					}

					$('#search-input').val("");
				}

			}

		});

		$("#search-input").focus(function () {  // 2014-07-09 Mr.No
			if ($("#search-input").val() == SearchTextValue) {
				$("#search-input").val("");
			}
		});

		$("#search-input").val(SearchTextValue);
	});

	var obj = {};

	//자동완성 배열을 목록에 저장
	function pushToAry(name, val) {
		obj[name] = val;
	}

	function fnAddUser(name, val) {
		
		$("#<%= this.hdnItemID.ClientID %>").val(val);
		$("#<%= this.hdnItemTitle.ClientID %>").val(name);

		__doPostBack('<%=btnAddUser.UniqueID%>', '');
	}
</script> 
<!--[if !ie]> 페이지 타이틀 박스 <![endif]-->
<table cellpadding="0" cellspacing="0" width="100%" class="PageTitleBox">
	<tr>
		<td align="left">
		<!--// 페이지 타이틀 부분 -->
		<h2 class="Title">
		<strong>SKTizen</strong>
		</h2>
		<!-- 페이지 타이틀 부분 //-->
		</td>
	</tr>
</table>
    <div class="adminGuideBox">
	<dl>
		<dt><strong>구성원 검색</strong></dt>
		<dd>
            메인화면에 소개하고 하는 구성원을 성명으로 검색하여 한줄소개를 입력해주세요!
		</dd>
	</dl>
</div>
<!--[if !ie]> 페이지 타이틀 박스 <![endif]-->
<div style="margin-top:30px;">
	 <dl>
		<dt>
			<div class="TableStyleBox">
				<table cellpadding="0" cellspacing="0" width="100%" class="TableStyle">
					<tr>
						<th width="100px" class="tac"><strong>구성원 검색</strong></th>
						<td>
							<input type="text" id="search-input" value="성명으로 검색해보세요" style="width:300px;"/>
					      
						</td>
					</tr>
				</table>
			
			</div>
		</dt>
	</dl>

	<dl id="dlHotnNew" style="margin-top:30px;">
		<dd>
			<!-- 리피터 -->
			<asp:HiddenField ID="hdnItemID" runat="server" />
			<asp:HiddenField ID="hdnItemTitle" runat="server" />
			<asp:HiddenField ID="hdnListSeqNo" runat="server" />
			<asp:Repeater ID="rptGlossary" runat="server" OnItemDataBound="rptGlossary_OnItemDataBound">
				<HeaderTemplate>
					<div class="TableStyleBox" style="margin-top:10px;">
					<table cellpadding="0" cellspacing="0" width="600" class="TableStyle">
						<tr>
							<th width="50" class="tac"><strong>No.</strong></th>
							<th width="100" class="tac"><strong>선택</strong></th>
							<th width="150" class="tac"><strong>대상자(사번)</strong></th>
							<th class="tac"><strong>한줄소개</strong></th>
						</tr>
				</HeaderTemplate>
				<ItemTemplate>
						<tr>
							<td class="tac"><asp:Literal ID="litRptNo" runat="server"></asp:Literal></td>
							<td class="tac"><asp:CheckBox ID="chkRptNotID"  runat="server" /></td>
							<td class="tac"><asp:Literal ID="litRptTitle" runat="server"></asp:Literal></td>
							<td class="tal">
								<asp:TextBox ID="txtRptContent" runat="server" MaxLength="500" Width="500"></asp:TextBox>
								<asp:HiddenField ID="hdnRptSeqno" runat="server" />
								<asp:HiddenField ID="hdnRptItemID" runat="server" />
							</td>
						</tr>
				</ItemTemplate>
				<FooterTemplate>
					</table>
					</div>
				</FooterTemplate>
			</asp:Repeater>
			<!--// 리피터 -->

			
		</dd>
	</dl>

	<dl style="margin-top:30px;">
		<dd style="margin-top:10px;">
			<!--// 페이징과 버튼 부분 -->
			<div class="paging_all c_box">
				<div class="paging">
					<div style="float:right;">
						<span class="button button1 button-green" onclick="fnSave()" style="cursor:pointer;">
							<button type="button">소개저장</button>
						</span>
						<span class="button button1 button-green" onclick="fnDelete()" style="cursor:pointer;">
							<button type="button">선택삭제</button>
						</span>

						<asp:Button ID="btnAddUser" runat="server" OnClick="btnAddUser_Click" style="display:none" />
						<asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" style="display:none" />
						<asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" style="display:none" />
					</div>
				</div>
			</div>
			<!-- 페이징과 버튼 부분 //-->
		</dd>
	</dl>
</div>

</asp:Content>
