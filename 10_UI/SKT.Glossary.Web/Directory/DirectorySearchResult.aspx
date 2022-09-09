<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/GlossarySearch.Master"  CodeBehind="DirectorySearchResult.aspx.cs" Inherits="SKT.Glossary.Web.Directory.DirectorySearchResult" %>

<%@ Register Src="~/Common/Controls/GatheringInfomation.ascx" TagName="GatheringInfomation" TagPrefix="common" %>
<%@ Register Src="~/Common/Controls/GatheringMenuTab.ascx" TagName="GatheringMenuTab" TagPrefix="common" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Common_Top" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript">
	    var lnbDep1 = 3;		//LNB 1depth

	    //파일오픈 
	    function fileOpen(dir, file) {
	        var url = "FileOpenTransfer.aspx?file=" + dir + "/" + file;
	        var win = window.open(url, "_blank", "left=10, top=10, width=10, height=10, toolbar=no, menubar=no, scrollbars=yes, resizable=no");
	    }

	    //파일다운로드  
	    function fileDownload(dir, file) {

	        $('#<%= this.hdDirectoryID.ClientID %>').val(dir);
		    $('#<%= this.hdFileID.ClientID %>').val(file);

		    __doPostBack('<%=btnDownload.UniqueID %>', '');
		}

		//파일 삭제 
		function fnDeleteFile(dir, file) {
		    //debugger

		    $('#<%= this.hdDirectoryID.ClientID %>').val(dir);
	        $('#<%= this.hdFileID.ClientID %>').val(file);

	        //삭제
	        if (confirm("[" + file + "] 문서를 삭제하시겠습니까?")) {
	            __doPostBack('<%=btnDelete.UniqueID %>', '');
    	    }

        }

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

            var iTotCnt = "<%=iTotalCnt%>";

            if(parseInt(iTotCnt) == 0)
            {
                $("#no_data").attr("style", "display:;text-align:center; padding-top:30px;");
                //$("#search_no_data").attr("style", "display:");

            }

	    });
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
                    location.href = "/Directory/DirectorySearchResult.aspx?DivType=<%=DivType%>&q=" + encodeURIComponent($("#txt_SearchKeyword").val()) + "&GatheringYN=<%=GatheringYN%>&GatheringID=<%=GatheringID%>&MenuType=Directory";
                }

            }

        }
		</script>
		<!--CONTENTS-->
		<div id="contents">
			<div class="h2tag">
                <p>
                <%if (GatheringYN == "Y"){ %>
                    <%--<a href="/Gathering/Main.aspx"><img src="/common/images/text/Gathering_text.png" alt="끌.모임" style="left: 50px; top: 30px; position: absolute; width: 83px; height: 26px;" /></a>--%>
                    <common:GatheringInfomation ID="GatheringInfomation1" runat="server" />
                <%}else{ %>
                    <h2 style="padding-left: 175px;"><img src="/common/images/text/Directory.png" alt="끌.문서" /></h2>
                <%} %>
                </p>
			</div>
			<%--	<p class="search_top">
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

                    <%if (GatheringYN == "Y"){ %>            
                        <common:GatheringMenuTab ID="GatheringMenuTabCtrl" runat="server" />
                    <%} %>
					<%--<ul id="tabMenu">
						<li><a href="/Directory/DirectoryListNew.aspx?DivType=Pub" <%= m_pub%>><img src="/common/images/btn/Directory_tab1.png" alt="모든 문서공유방" /></a></li>
						<li><a href="/Directory/DirectoryListNew.aspx?DivType=Pri" <%= m_pri%>><img src="/common/images/btn/Directory_tab2.png" alt="내가만든 문서공유방" /></a></li>
						<li><a href="/Directory/DirectoryListNew.aspx?DivType=Vis" <%= m_vis%>><img src="/common/images/btn/Directory_tab3.png" alt="초대된 문서공유방" /></a></li>
					</ul>--%>

					<!--문서공유방 상세-->
					<div id="directory_view_area">
						<div id="directory_search">
							<%--<h2>'<asp:Literal ID="litSearchKeyword" runat="server" />' &nbsp;&nbsp;검색결과</h2>--%>
							<ul>
								<!--리피터  -->
								<asp:Repeater ID="rptSearchResult" runat="server" OnItemDataBound="rptSearchResult_OnItemDataBound">
									<ItemTemplate>
										<asp:Literal ID="litDirectory" runat="server" />
									</ItemTemplate>
									<%--<FooterTemplate>
										<asp:Literal ID="lblEmptyData" runat="server" Visible="false">
											
										</asp:Literal>
									</FooterTemplate>--%>
								</asp:Repeater>
								<!--리피터  -->
							</ul>
						</div>
					</div>
					<!--문서공유방 상세-->

                     <!--검색결과 없음-->
                     <div id="no_data" style="text-align:center; padding-top:30px;display:none;"  >
				        <dl>
					        <dt>"<b><%=SearchKeyword %></b>"에 대한 검색결과를 찾을 수 없습니다.</dt>
					        <%--<dd>- 단어의 철자가 정확한지 확인해 보세요.</dd>--%>
					        <dd>- 검색어의 단어 수를 줄이거나, 보다 일반적인 검색어로 다시 검색해 보세요.</dd>
				        </dl>
			        </div>
			        <!--/검색결과 없음-->
			        <div id="search_no_data" style="display:none;"">
				        <a href="/QnA/QnAWrite.aspx" class="btn5"><b>질문 하러 가기</b></a>
			        </div>

				</div>
				<!--/article-->
               

		</div>
		<!--/CONTENTS-->
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Common_Footer" runat="server">
    <div style="display:none;">
        <asp:HiddenField ID="hidMenuType" runat="server" />
        <asp:HiddenField ID="hdDirectoryID" runat="server" />
        <asp:HiddenField ID="hdDirectoryNM" runat="server" /> 
        <asp:HiddenField ID="hdFileID" runat="server" /> 
        <asp:HiddenField ID="hdFileName" runat="server" />
        <asp:HiddenField ID="hdCommonID" runat="server" />
        <asp:HiddenField ID="hdBoardID" runat="server" />
        <asp:HiddenField ID="hdItemGuid" runat="server" />
		<asp:Button ID="btnDownload" runat="server" OnClick="btnDownload_Click" /> 
        <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" />

    </div>
</asp:Content>