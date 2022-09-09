<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossarySearch.Master" AutoEventWireup="true" CodeBehind="GlossaryNewsList_Tnet.aspx.cs" Inherits="SKT.Glossary.Web.Glossary.GlossaryNewsList_Tnet" %>

<%@ Register Namespace="ASPnetControls" Assembly="SKT.Common" TagPrefix="Paging" %>
<%@ Register Src="~/Common/Controls/GatheringInfomation.ascx" TagName="GatheringInfomation" TagPrefix="common" %>
<%@ Register Src="~/Common/Controls/GatheringMenuTab.ascx" TagName="GatheringMenuTab" TagPrefix="common" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">
    <script type="text/javascript">

        function fnGlossaryList(tagtitle, searchsort) {
            var url = "/Glossary/GlossaryNewsList.aspx?TagTitle=" + tagtitle + "&SearchSort=" + searchsort + "&GatheringYN=<%=this.GatheringYN %>&GatheringID=<%=this.GatheringID %>";
            location.href = url;
        }

        $(document).ready(function () {
            $('html, body').animate({
                scrollTop: $('#move_title').offset().top
            }, 'fast');
        });


        function getGlossaryMainTagSelect(Tag_Index, Tag_Count, Tag_Id) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Glossary/Glossary.aspx/GetGlossaryMainTagSelect",
                data: "{Tag_Index : '" + Tag_Index + "', Tag_Count : '" + Tag_Count + "', GatheringYN : '<%=this.GatheringYN %>', GatheringID : '<%=this.GatheringID %>', UserID : '<%=this.UserID%>' }",
                dataType: "json",
                success: function (data) {
                    setTag(data.d.Table, Tag_Id);
                },
                error: function (result) {
                    //alert("Error");
                    //alert(result);
                }
            });
        }

        /*
        function setTag(Table, Id) {
            for (var i = 0; i < Table.length; i++) {
                var tempTag = '<li><a href=javascript:fnGlossaryList(\'' + ((Table[i].TagTitle == '전체') ? '' : encodeURIComponent(encodeURIComponent(Table[i].TagTitle))) + '\',\'\');>' + Table[i].TagTitle + '(' + Table[i].ShowCount + ')</a></li>';
                $("#" + Id + " ul").append(tempTag);
            }
        }
        */

        function setTag(Table, Id) {
            if (Table.length < 5) {
                $("#GetMoreBtn").hide();
            }
            for (var i = 0; i < Table.length; i++) {
                //var tagCss = "txt" + (Math.floor(Math.random() * 10) + 1).toString();
                var tagCss = "txt77";
                var tempTag = "";

                //var tempTag = '<li><a href=javascript:fnGlossaryList(\'' + ((Table[i].TagTitle == '전체') ? '' : encodeURIComponent(encodeURIComponent(Table[i].TagTitle))) + '\',\'\') class="' + tagCss + '" >'
                //    + Table[i].TagTitle + '(' + Table[i].ShowCount + ')</a></li>';

                //if (Table[i].TagTitle == '전체') {
                //    tempTag = '<li><a href=javascript:fnGlossaryList(\'\',\'\') class="' + tagCss + '" >' + Table[i].TagTitle + '</a></li>';
                //}
                //else {
                tempTag = '<li style=\" width:270px;\"><a href=javascript:fnGlossaryList(\'' + encodeURIComponent(encodeURIComponent(Table[i].TagTitle)) + '\',\'\') class="' + tagCss + '" >' + Table[i].TagTitle + '(' + Table[i].ShowCount + ')</a></li>';
                //}
                $("#" + Id + " ul").append(tempTag);
            }
        }

        function GetMoreTagOpen() {
            //var Tag_Index = $("#tag_box_pop ul li").length;
            //var Tag_Count = 40;

            //if (Tag_Index == 0) {
            //    getGlossaryMainTagSelect(Tag_Index, Tag_Count, 'tag_box_pop');
            //}

            //$("#tag_pop").show();

            /*
            Author : 개발자-최현미, 리뷰자-진현빈D
            Create Date : 2016.09.01 
            Desc : 태그 카테고리 UI 개선
            */
            if ($("#GetMoreBtn").attr("class") == "btn") {
                $("#tag_box").attr("style", "display:;");
                $("#tag_box").attr("style", "height:380px");
                $("#GetMoreBtn").attr("class", "btn on");
                $("#ImgTag").attr("src", "/Common/images/btn/btn_tag_close.png");
            }
            else {
                $("#tag_box").attr("style", "display:none;");
                //$("#tag_box").attr("style", "height:38px");
                $("#GetMoreBtn").attr("class", "btn");
                $("#ImgTag").attr("src", "/Common/images/btn/btn_tag_open.png");

            }
        }

        function GetMoreTagClose() {
            $("#tag_pop").hide();
        }

        getGlossaryMainTagSelect(1, 32, 'tag_box');

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Common_Top" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript">
    var lnbDep1 = 1;		//LNB 1depth
</script>

	<!--CONTENTS-->
	<div id="contents">
		<div class="h2tag">
            <!-- top-menu -->
            <%if (GatheringYN == "Y")
              { %>
                <!--<img src="/common/images/text/Gathering_text.png" alt="끌.모임" style="width: 83px; height: 26px; float:none; vertical-align: bottom; margin-right:10px; margin-bottom:8px;" />-->
                <common:GatheringInfomation ID="GatheringInfomation1" runat="server" />
            <%}else{ %>
            <img src="/common/images/text/Glossary.png" alt="끌.지식" />
        <%} %>
            <!--//모임 목록2-->
            <!-- //top-menu -->
		 </div>
		<p class="btn_top"><a href="/Glossary/GlossaryWriteNew.aspx?GatheringYN=<%=GatheringYN%>&GatheringID=<%=GatheringID%>"><img src="/common/images/btn/write<%if(GatheringYN=="Y"){ %>_gathering<%}%>.png" alt="글쓰기" /></a></p>
       

		<!-- article -->
        <div id="article" class="Glossary_list" <%if (GatheringYN != "Y") { Response.Write("style = \"padding-top:30px;\");"); }%>>
            <%if (GatheringYN == "Y")
              { %>

                <%--
                Author : 개발자-최현미, 리뷰자-진현빈D
                Create Date : 2016.09.01 
                Desc : 태그 카테고리 UI 개선
                --%>

              <div style="float:right; overflow:hidden; padding-bottom:10px;">  
              <input type="text" class="ui-autocomplete-input search_txt_common" id="txt_SearchKeyword"   onkeypress="if(event.keyCode == 13){fnFileSearch();return false;}" onfocus="this.value=''" />
			  <a href="javascript:fnFileSearch();"><img src="/common/images/btn/search.png" alt="검색" /></a>
              </div>
              <common:GatheringMenuTab ID="GatheringMenuTabCtrl" runat="server" />
            <%} %>

           <%-- <div style="width:100%;overflow:hidden; text-align:right; padding-bottom:3px;">
                <a id="GetMoreBtn" href="javascript:GetMoreTagOpen();" class="btn"><img src="/Common/images/btn/btn_tag_open.png" id="ImgTag" border="0" alt=""/></a>
            </div>--%>
           
			<!--리스트-->
			<div id="list_area">
                
                <div id="tag_box" class="tag_box" style="display:none;">
                 <ul></ul>
                </div>
                  
                <a id="move_title"></a>
                <div style="float:left;  padding-left:50px; width:80%; overflow:hidden; text-align:center; " >
                <span style='font-size:24px;padding-bottom:10px;'><strong>
                T.끌 최근 소식
                </strong></span>
                </div>
                <div style=" float:right; width:10%; overflow:hidden; padding-right:10px;    ">
                    <a id="GetMoreBtn" href="javascript:GetMoreTagOpen();" class="btn"><img src="/Common/images/btn/btn_tag_open.png" id="ImgTag" border="0" alt=""/></a>
                </div>

                <!-- 모임목록정보 -->
				<table class="listTable">
					<colgroup><col width="6%" /><col width="*" /><col width="11%" /><col width="20%" /><col width="6%" /><col width="6%" /></colgroup>
					<thead>
					<tr>
						<th>No</th>
						<th>제목</th>
						
						<th>마지막 편집일</th>
						<th>마지막 편집자</th>
						<th>조회</th>
						<th>추천</th>
					</tr>
					</thead>
					<tbody>
                        <asp:Repeater ID="rptInGeneral" runat="server" OnItemDataBound="rptInGeneral_OnItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td><asp:Literal runat="server" ID="Num"></asp:Literal></td>
                                    <div class="types" style="display: none">
                                        <asp:Literal runat="server" ID="ltWiki"></asp:Literal><%# DataBinder.Eval(Container.DataItem, "Type")%></span>
                                        </div>
                                    <td class="al">
                                        <a href="javascript:fnView2('<%# DataBinder.Eval(Container.DataItem, "CommonID")%>','<%# DataBinder.Eval(Container.DataItem, "HistoryYN")%>','<%# DataBinder.Eval(Container.DataItem, "Gubun")%>')">
                                        <asp:Literal runat="server" ID="litPermission"></asp:Literal>
                                            <%# DataBinder.Eval(Container.DataItem, "Title")%><asp:Literal runat="server" ID="litReply"></asp:Literal>
                                        </a>
                                    </td>
                                   
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "LastCreateDate")%>
                                    </td>
                                    <td>
                                        <asp:Literal ID="litUserInfo" runat="server"></asp:Literal>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "Hits")%>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "LikeCount")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
					</tbody>
				</table>
				<p class="pagination" style="position:relative;">
                    <%--2015-08-25 paging 옆에 지식올리기 버튼 추가--%>
                    <a href="/Glossary/GlossaryWriteNew.aspx?GatheringYN=<%=GatheringYN%>&GatheringID=<%=GatheringID%>" class="btn1" style="position:absolute; top:0px; right:0;"><span style="padding:5px 20px 0;"><strong>지식올리기</strong></span></a>
                    <Paging:PagerV2_8 ID="pager" OnCommand="pager_Command" runat="server" GenerateHiddenHyperlinks="true" Visible="false" />
				</p>
            <!--/모임목록정보-->
			</div>
			<!--/리스트-->
			<!--랭킹-->
			<div class="raking_list" <%if (GatheringYN == "Y"){ %>style="display:none;" <% } %>>
                                     				<dl>
					<dt><img src="/common/images/text/Glossary1.png" alt="인기조회순" /></dt>
                    <asp:Repeater ID="rptHits" runat="server" OnItemDataBound="rptHits_OnItemDataBound">
                        <ItemTemplate>
                            <dd>
                                <b style="color:black"><%# DataBinder.Eval(Container.DataItem, "RowNum")%>.</b>                                            
                                <a href="javascript:fnView('<%# DataBinder.Eval(Container.DataItem, "CommonID")%>','<%# DataBinder.Eval(Container.DataItem, "HistoryYN")%>')">
                                    <%# DataBinder.Eval(Container.DataItem, "Title")%>
                                </a>
                            </dd>
                        </ItemTemplate>
                    </asp:Repeater>
				</dl>
				<dl>
					<dt><img src="/common/images/text/Glossary2.png" alt="인기추천수" /></dt>
                    <asp:Repeater ID="rptLike" runat="server" OnItemDataBound="rptLike_OnItemDataBound">
                        <ItemTemplate>
                            <dd>
                                <b style="color:black"><%# DataBinder.Eval(Container.DataItem, "RowNum")%>.</b>                                            
                                <a href="javascript:fnView('<%# DataBinder.Eval(Container.DataItem, "CommonID")%>','<%# DataBinder.Eval(Container.DataItem, "HistoryYN")%>')">
                                    <%# DataBinder.Eval(Container.DataItem, "Title")%>
                                </a>
                            </dd>
                        </ItemTemplate>
                    </asp:Repeater>
				</dl>
			</div>
			<!--/랭킹-->
            <!--모임생성정보-->
            <div id="directory_user" <%if (GatheringYN != "Y")
                                           { %>style="display:none;" <% } %>>
                    <h3>
                        <img src="/common/images/text/Gathering1.png" alt="초대된 멤버" /></h3>
                    <ul>
                        <asp:Repeater ID="rptGatheringUser" runat="server" OnItemDataBound="rptGatheringUser_OnItemDataBound">
                            <ItemTemplate>
                                <asp:Literal ID="litGatheringUser" runat="server" />
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                    <dl>
                        <dt>
                            <img src="/common/images/text/Gathering2.png" alt="모임 만든이" style="width: 100px;" />

                        </dt>
                        <dd>
                            <asp:Label ID="lblCreater" runat="server"></asp:Label></dd>
                    </dl>
                    <dl>
                        <dt>
                            <img src="/common/images/text/Gathering3.png" alt="모임 생설일" style="width: 100px;" /></dt>
                        <dd>
                            <asp:Label ID="lblCreateDate" runat="server"></asp:Label></dd>
                    </dl>
                    <!-- <p><a href="javascript:viewDivShow();"><img src="../common/images/btn/setting.png" alt="" title="폴더사용자 관리 바로가기" /></a></p>-->
                </div>
            <!--//모임생성정보 -->

            <!--검색결과 없음-->
             <div id="no_data" style="text-align:center; padding-top:30px;display:none;"  >
				<dl>
					<dt>"<b><%=hid_SearchKeyword.Value%></b>"에 대한 검색결과를 찾을 수 없습니다.</dt>
					<%--<dd>- 단어의 철자가 정확한지 확인해 보세요.</dd>--%>
					<dd>- 검색어의 단어 수를 줄이거나, 보다 일반적인 검색어로 다시 검색해 보세요.</dd>
				</dl>
			</div>
			<!--/검색결과 없음-->
			<div id="search_no_data" style="display:none;"">
				<a href="/QnA/QnAWrite.aspx" class="btn5"><b>질문 하러 가기</b></a>
			</div>

        </div>
        <!-- //article -->
	</div>
	<!--//CONTENTS-->

    <div class="pop_glossarypermission" id="pop_permission" style="display:none;">
        <div class="pop_glossaryheader"><img style="padding-top:1px;cursor:pointer;" src="../Common/images/btn/btn_close.png" onclick="javascript:pop_permissionclose();" /></div>
        <div class="pop_glossarybody">이 지식은 조회권한이 부여된 분들만<br /> 보실 수 있습니다.
            <div onclick="javascript:pop_permissionclose();">확인 </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Common_Footer"
    runat="server">
    <asp:Button ID="btnSearch" runat="server" Style="display: none;" OnClick="btnSearch_Click" />
    <%--Author : 개발자-김성환D, 리뷰자-진현빈D
        Create Date : 2016.05.18 
        Desc : 끌모임 검색 기능 추가--%>
    <asp:Button ID="btnSearchKeyword" runat="server" Style="display:block;" OnClick="btnSearchKeyword_Click" />
    <asp:HiddenField ID="hid_SearchKeyword" runat="server"/>
    <asp:HiddenField ID="hdMore" runat="server" />
    <%--    <asp:HiddenField ID="hdContentMore" runat="server" />--%>
    <script type="text/javascript">
        //제목, 내용 뷰화면 가기

        function fnView(ItemID, HistoryYN) {
            <%--Author : 개발자-김성환D, 리뷰자-진현빈D
            Create Date : 2016.08.11 
            Desc : 끌지식 권한 체크--%>
            var result = true;
            var GatheringYN = '<%=GatheringYN%>';
            if (GatheringYN != "Y") {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "/Common/Controls/AjaxControl.aspx/GetGlossaryPermissions",
                    data: "{commonID : '" + ItemID + "', userID : '<%=UserID%>'}",
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data.d < 1) {
                            $("#pop_permission").show();
                            result = false;
                        }
                    },
                    error: function (result) {
                        //alert("Error DB Check");
                    }
                });
            }

            if (result) {
                var url = "/Glossary/GlossaryView.aspx?ItemID=" + ItemID + "&GatheringYN=<%=this.GatheringYN%>&GatheringID=<%=this.GatheringID%>";
                location.href = url;
            }
        }

        function fnView2(ItemID, HistoryYN, Gubun) {
            if (Gubun == "HN") {

                fnView(ItemID, HistoryYN);
                
            } else {
                var url = "/QnA/QnAView.aspx?ItemID=" + ItemID;
                location.href = url;
            }
        }

        function pop_permissionclose() {
            $(".pop_glossarypermission").hide();
        }

        var ThisTemp = null;

        //알람 히든
        $(document).ready(function () {
            if ("<%=GatheringYN%>" == "Y") {
                $("#container").attr('class', 'Gathering');
                $("#tag_box").hide();
            }

            $(".list-alarm-outer").hide();
            var CategoryID = getParameterByName("CategoryID");
            if (CategoryID) {
                // 최근티끌과 카테고리별티끌 통합
                //$("#listTitleName").html("카테고리 별 조회");
                //$("#listTitleName").attr('class', 'category');
                //$(".radioList").show(); // 2014-04-30 Mr.No
                //$(".main-dash").css("height","130px");
                setSelectRadioCheck('rBtnA', CategoryID)
            }
        });

            //
            //        function fnView(ItemID) {
            //            location.href = "/Glossary/GlossaryView.aspx?ItemID=" + ItemID;
            //        }

            //프로필 화면으로이동
            function fnProfileView(UserID) {
                location.href = "/GlossaryMyPages/MyProfile.aspx?UserID=" + UserID;
            }

            //알람 오픈
            function fnAlarmOpen(thisTag) {
                if (ThisTemp != thisTag)
                    $(".list-alarm-outer").hide();
                if ($(thisTag)[0].nextSibling.nextSibling.style.display == "block") {
                    $(thisTag)[0].nextSibling.nextSibling.style.display = "none";
                } else {
                    $(thisTag)[0].nextSibling.nextSibling.style.display = "block";
                }
                ThisTemp = thisTag;
            }

            //알람 저장
            function fnAlarmSave(thisTag, CommonID) {
                var MailSet = "N";
                var NoteSet = "N";

                //이메일
                if ($(thisTag)[0].parentNode.children[0].children[0].children[0].checked == true)
                    MailSet = "Y";

                //쪽지
                if ($(thisTag)[0].parentNode.children[0].children[1].children[0].checked == true)
                    NoteSet = "Y";

                $.ajax({
                    type: "POST",
                    url: "/Common/Controls/AjaxControl.aspx" + "/GlossaryAlarm",
                    data: "{CommonID : '" + CommonID + "', UserID : '" + <%= UserID%> + "', MailSet : '" + MailSet + "', NoteSet : '" + NoteSet + "'}",
                 contentType: "application/json; charset=utf-8",
                 success: function (success) {
                     $(".list-alarm-outer").hide();

                     if (MailSet == "Y" || NoteSet == "Y") {
                         $(thisTag)[0].parentNode.parentNode.parentNode.children[0].className = "alarm-icon on";

                     } else {
                         $(thisTag)[0].parentNode.parentNode.parentNode.children[0].className = "alarm-icon off";
                     }

                 }
             });
         }

         // Get QueryString
         function getParameterByName(name) {
             name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
             var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                 results = regex.exec(location.search);
             return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
         }

         // 2014-04-30 Mr.No
         function clickCategory(el, ID) {
             var url = "/Glossary/GlossaryNewsList.aspx?CategoryID=" + ID;
             location.href = url;
         }
         // 2014-04-29 Mr.No 추가
         function setSelectRadioCheck(rdoName, rdoValue) {
             $('input:radio[name="' + rdoName + '"]').each(function () {
                 $(this).removeAttr("checked");
             });
             var selectedItem = $('input:radio[name="' + rdoName + '"][value="' + rdoValue + '"]');
             selectedItem.attr('checked', 'checked');
             selectedItem.prop('checked', true);
         }
    </script>
</asp:Content>