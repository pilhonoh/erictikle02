<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossaryAdmin.Master" AutoEventWireup="true" CodeBehind="tikle.aspx.cs" Inherits="SKT.Glossary.Web.TikleAdmin.MainMng.tikle" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
<script language="javascript" type="text/javascript">
	function fnSave() {
		__doPostBack('<%=btnSave.UniqueID%>', '');
	}

	function fnDelete() {
		var r = $("input:checkbox[name$='chkNotID']");

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

	var SearchTextValue = "제목으로 검색해보세요.";

	$(document).ready(function () {
		//자동검색 bind
		$("#search-input").autocomplete({
			source: function (request, response) {

				var kw = fnSearchWord();

				if (kw == SearchTextValue) return;

				$.ajax({
					type: "POST",
					contentType: "application/json; charset=utf-8",
					url: "/Common/Controls/AjaxControl.aspx/GetAutoCompleteDataAdmin",
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
					fnGlossarySave(SearchTxt, obj[SearchTxt]);
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
						fnGlossarySave(SearchTxt, obj[SearchTxt]);
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
	function pushToAry(name, val)
	{
		obj[name] = val;
	}

	function fnGlossarySave(name, val)
	{
		$("#<%= this.hdnItemID.ClientID %>").val(val);
		$("#<%= this.hdnItemTitle.ClientID %>").val(name);

		fnSave();
	}
</script> 
<!--[if !ie]> 페이지 타이틀 박스 <![endif]-->
<table cellpadding="0" cellspacing="0" width="100%" class="PageTitleBox">
	<tr>
		<td align="left">
		<!--// 페이지 타이틀 부분 -->
		<h2 class="Title">
		<strong><%=strTitleName%></strong>
		</h2>
		<!-- 페이지 타이틀 부분 //-->
		</td>
	</tr>
</table>
<%--<div class="adminGuideBox">
<dl>
	<dd>
        <asp:RadioButtonList id="rdoGubun" RepeatDirection="Horizontal" runat="server" OnSelectedIndexChanged="rdoGubun_SelectedIndexChanged" AutoPostBack="true" CellSpacing = "10">
			<asp:ListItem value="HN" Selected="True">Hot & New</asp:ListItem>
            <asp:ListItem value="QA">T-Net Interface 5 row</asp:ListItem>
			<asp:ListItem value="GL">끌.지식메인 추천지식</asp:ListItem>
			<asp:ListItem value="GR">끌.지식메인 인기지식</asp:ListItem>
			<asp:ListItem value="TN">T-NET 메인</asp:ListItem>
            <asp:ListItem value="DT" >Data Transformation</asp:ListItem>
		</asp:RadioButtonList>
	</dd>
</dl>
</div>--%>
<!--[if !ie]> 페이지 타이틀 박스 <![endif]-->
<div style="margin-top:30px;">
   
	<dl id="dlHotnNew" style="margin-top:30px;">
		<dd>
			<!-- 리피터(Hot&New) -->
			<asp:Repeater ID="rptHnN" runat="server" OnItemDataBound="rptHnN_OnItemDataBound">
				<ItemTemplate>
					<div class="TableStyleBox" style="margin-top:10px;">
						<table cellpadding="0" cellspacing="0" width="600" class="TableStyle">
							<tr>
								<th width="100" class="tac"><strong>제목</strong></th>
								<td class="tal">
									<asp:TextBox ID="txtHnNTitle" runat="server" Value="" MaxLength="200" Width="500" />
									<asp:HiddenField ID="hdnNotID" runat="server" Value="" />
								</td>
							</tr>
							<%--<tr>
								<th width="100" class="tac"><strong>내용</strong></th>
								<td class="tal">
									<asp:TextBox ID="txtHnNContent" runat="server" Value="" MaxLength="200" Width="500" />
								</td>
							</tr>--%>
							<tr>
								<th width="100" class="tac"><strong>지식 URL</strong></th>
								<td class="tal">
									<asp:TextBox ID="txtHnNURL" runat="server" Value="" MaxLength="500" Width="800" />
								</td>
							</tr>
						</table>
					</div>
				</ItemTemplate>
				<%--<FooterTemplate>
					<asp:Literal ID="lblEmptyData"
						Text="<li style='padding:0;text-align:center'><h2>등록된 문서가 없습니다.</h2></li>" runat="server" Visible="false">
					</asp:Literal>
				</FooterTemplate>--%>
			</asp:Repeater>
			<!--// 리피터(Hot&New) -->




            <!-- 리피터(QA) -->
			<asp:Repeater ID="rptQA" runat="server" OnItemDataBound="rptQA_OnItemDataBound">
				<ItemTemplate>
					<div class="TableStyleBox" style="margin-top:10px;">
						<table cellpadding="0" cellspacing="0" width="600" class="TableStyle">
							<tr>
								<th width="100" class="tac"><strong>제목</strong></th>
								<td class="tal">
									<asp:TextBox ID="txtQATitle" runat="server" Value="" MaxLength="200" Width="500" />
									<asp:HiddenField ID="hdnQANotID" runat="server" Value="" />
								</td>
							</tr>
							<tr style="display:none;">
								<th width="100" class="tac"><strong>내용</strong></th>
								<td class="tal">
									<asp:TextBox ID="txtQAContent" runat="server" Value="" MaxLength="200" Width="500" />
								</td>
							</tr>
							<tr>
								<th width="100" class="tac"><strong>링크</strong></th>
								<td class="tal">
									http://tikle.sktelecom.com<asp:TextBox ID="txtQAURL" runat="server" Value="" MaxLength="200" Width="355" />
								</td>
							</tr>
						</table>
					</div>
				</ItemTemplate>
			</asp:Repeater>
			<!--// 리피터(QA) -->




			<!-- 리피터(지식) -->
			<asp:HiddenField ID="hdnItemID" runat="server" />
			<asp:HiddenField ID="hdnItemTitle" runat="server" />
			<asp:HiddenField ID="hdnListSeqNo" runat="server" />
			<asp:Repeater ID="rptGlossary" runat="server" OnItemDataBound="rptGlossary_OnItemDataBound">
				<HeaderTemplate>
					<div class="TableStyleBox" style="margin-top:10px;">
					<table cellpadding="0" cellspacing="0" width="600" class="TableStyle">
						<tr>
							<th width="100" class="tac"><strong>선택</strong></th>
							<th width="100" class="tac"><strong>지식코드</strong></th>
							<th class="tac"><strong>제목</strong></th>
						</tr>
				</HeaderTemplate>
				<ItemTemplate>
						<tr>
							<td class="tac"><asp:CheckBox ID="chkNotID"  runat="server" /></td>
							<td class="tac"><asp:Literal ID="litItemID" runat="server"></asp:Literal></td>
							<td class="tal"><asp:HyperLink ID="lnkTitle" runat="server"></asp:HyperLink></td>
						</tr>
				</ItemTemplate>
				<FooterTemplate>
					</table>
					</div>
				</FooterTemplate>
			</asp:Repeater>
			<!--// 리피터(지식) -->

			
		</dd>
	</dl>

	<dl style="margin-top:30px;">
		<dd style="margin-top:10px;">
			<!--// 페이징과 버튼 부분 -->
			<div class="paging_all c_box">
				<div class="paging">
					<div style="float:right;">

						<span class="button button1 button-green" onclick="fnSave()" style="cursor:pointer;">
							<button type="button">등록</button>
						</span>

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
