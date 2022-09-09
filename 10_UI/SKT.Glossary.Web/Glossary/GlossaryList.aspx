<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Glossary.master" AutoEventWireup="true"
    CodeBehind="GlossaryList.aspx.cs" Inherits="SKT.Glossary.Web.Glossary.GlossaryList" ValidateRequest="false" %>

<%@ Register Namespace="ASPnetControls" Assembly="SKT.Common" TagPrefix="Paging" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">
<script type="text/javascript">
        //제목, 내용 뷰화면 가기
        function fnGoView(ItemID, CommonID) {
            location.href = "/Glossary/GlossaryView.aspx?ItemID=" + ItemID;
        }

        //QnA 뷰화면 가기
        function fnQnAGoView(ItemID) {
            location.href = "/QnA/QnAView.aspx?ItemID=" + ItemID; // +"&SearchKeyword=<%= SearchKeyword %>";
        }

        // QnA 더보기
        function fnQnAMore() {
            location.href = "/QnA/QnAList.aspx?SearchKeyword=<%= DisplaySearchKeyword %>";
        }

        //문서작성 페이지
        function fnGlossaryWrite() {
            location.href = "/Glossary/GlossaryWrite.aspx?mode=NotSearch&SearchKeyword=<%= DisplaySearchKeyword %>";
        }
                                   
        //검색 리스트가 없을 경우 버튼을 선택 하여 이동
        function fnQnAWrite() {
            location.href = "/QnA/QnAWrite.aspx?mode=NotSearch&SearchKeyword=<%= DisplaySearchKeyword %>";
        }

        //프로필로 이동
        function fnProfileView(UserID) {
            location.href = "/GlossaryMyPages/MyProfile.aspx?UserID=" + UserID;
        }

        //제목 검색 더보기 버튼
        function fnTitleList() {
            location.href = "/Glossary/GlossaryList.aspx?mode=Titles&SearchKeyword=<%= DisplaySearchKeyword %>";
        }

        //내용 검색 더보기 버튼
        function fnContentsList() {
            location.href = "/Glossary/GlossaryList.aspx?mode=Contents&SearchKeyword=<%= DisplaySearchKeyword %>";
        }

        //QnA 검색 더보기 버튼
        function fnQnAList() {
            location.href = "/QnA/QnAList.aspx?SearchKeyword=<%= DisplaySearchKeyword %>";
        }

        //프로필 검색 검색 더보기 버튼
        function fnProfileList() {
            location.href = "/Glossary/GlossaryList.aspx?mode=Profiles&SearchKeyword=<%= DisplaySearchKeyword %>";
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Common_Top" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
        <div id="cont">
        <asp:Literal  ID="SearchResult" runat="server"></asp:Literal>
	<div id="Result" runat="server" visible="true">
        		<!--DASH -->
		<div class="main-dash">
			<div class="main-dash-left">
				<h3>'<span><%= DisplaySearchKeyword%></span>'에 대한 검색 결과</h3>
			</div>
			<div class="main-dash-right">
				<ul>
					<li>
						<dl>
							<dt>검색 결과</dt>
					            <dd><label runat="server" id="lbCount" ></label><span class="unit">건</span></dd>
						</dl>
					</li>
				</ul>
			</div>
		</div>
		<!-- result -->
		<div id="search-result-list">
            <div id="BodyTitle" runat ="server" visible="true">
		        <h3 class="result-heading"><a href="#">제목 검색 결과</a> <span class="search-result-num"><label runat="server" id="lbltitle" ></label>건</span></h3>
		        <dl class="search-li-wrap">
			        <asp:Repeater ID="rptInGeneral" runat="server" OnItemDataBound="rptInTitle_OnItemDataBound" >
				        <ItemTemplate>
					        <dt class="search-li-heading">
                                <asp:Literal ID="litTitle" runat="server"></asp:Literal>
					        </dt>
					        <dd class="search-li-desc"> 
                                <a href="javascript:fnGoView('<%# DataBinder.Eval(Container.DataItem, "CommonID")%>')" class="desc">   
						            <%# DataBinder.Eval(Container.DataItem, "Summary")%>  
                                </a>
                                <p class="li-summary"><span class="editor">최근 편집자 : <a href="javascript:fnProfileView('<%# DataBinder.Eval(Container.DataItem, "UserID")%>')"><%# DataBinder.Eval(Container.DataItem, "UserName")%>/<%# DataBinder.Eval(Container.DataItem, "DeptName")%></a></span><span class="rating">추천수 : <%# DataBinder.Eval(Container.DataItem, "LikeCount")%></span><span class="count">편집횟수 : <%# DataBinder.Eval(Container.DataItem, "HistoryCount")%>회</span><span class="date">최근 편집일 : <%# DataBinder.Eval(Container.DataItem, "CreateDate")%></span> <%# DataBinder.Eval(Container.DataItem, "TagsInHtml")%></p>
					        </dd>
						   
				        </ItemTemplate>
			        </asp:Repeater>
		        </dl>

			    <!-- btn more -->
			    <div class="search-btn-bottom">
                    <div runat ="server" id="TitleMore" visible="true">
                       <a href="javascript:fnTitleList();" class ="btn-more"><span>더 보기</span></a>
                    </div>
			    </div>
			    <!-- BTN -->
		        <div id="list-btn">
				    <div class="pagging" runat ="server" id="TitlePage" visible="false">
						    <Paging:PagerV2_8 ID="PagerTitle" OnCommand="pager_Command" runat="server" GenerateHiddenHyperlinks="true" /> 
				    </div>
		        </div>
            </div>
            <div id ="BodyContent" runat="server" visible="true" >
			    <h3 class="result-heading-st"><a href="#">내용 검색 결과</a> <span class="search-result-num"><label runat="server" id="lblContents" ></label>건</span></h3>
			    <dl class="search-li-wrap">
                    <asp:Repeater ID="rptInGeneral_" runat="server" OnItemDataBound="rptInContent_OnItemDataBound" >
				        <ItemTemplate>
					        <dt class="search-li-heading">
						        <a href="javascript:fnGoView('<%# DataBinder.Eval(Container.DataItem, "CommonID")%>');"><%# DataBinder.Eval(Container.DataItem, "Title")%></a>
					        </dt>
					        <dd class="search-li-desc">  
                                <a href="javascript:fnGoView('<%# DataBinder.Eval(Container.DataItem, "CommonID")%>')" class="desc">    
						            <asp:Literal ID="litContent" runat="server"></asp:Literal></a>
                                    <p class="li-summary"><span class="editor">최근 편집자 : <a href="javascript:fnProfileView('<%# DataBinder.Eval(Container.DataItem, "UserID")%>')"><%# DataBinder.Eval(Container.DataItem, "UserName")%>/<%# DataBinder.Eval(Container.DataItem, "DeptName")%></a></span><span class="rating">추천수 : <%# DataBinder.Eval(Container.DataItem, "LikeCount")%></span><span class="count">편집횟수 :<%# DataBinder.Eval(Container.DataItem, "HistoryCount")%></span><span class="date">최근 편집일 : <%# DataBinder.Eval(Container.DataItem, "CreateDate")%></span> <%# DataBinder.Eval(Container.DataItem, "TagsInHtml")%></p>
					        </dd>
						   
				        </ItemTemplate>
			        </asp:Repeater>
			    </dl>
			    <!-- btn more -->
			    <div class="search-btn-bottom">
                    <div runat ="server" id="ContentMore" visible="true">
                       <a href="javascript:fnContentsList();" class ="btn-more"><span>더 보기</span></a>
                    </div>
			    </div>
			    <!-- BTN -->
		        <div id="list-btn1">
				    <div class="pagging" runat ="server" id="ContentPage" visible="false">
                        <Paging:PagerV2_8 ID="PagerContent" OnCommand="pager_Command1" runat="server" GenerateHiddenHyperlinks="true" /> 
				    </div>
		        </div>
            </div>
             <div id="BodyQnA" runat ="server" visible="true">
		        <h3 class="result-heading-st"><a href="#">질문/답변 검색 결과</a> <span class="search-result-num"><label runat="server" id="lblQnA" ></label>건</span></h3>
		        <dl class="search-li-wrap">
			        <asp:Repeater ID="rptInGeneralQnA" runat="server" OnItemDataBound="rptInGeneralQnA_OnItemDataBound" >
				        <ItemTemplate>
					        <dt class="search-li-heading">
                                <asp:Literal ID="litQnATitle" runat="server"></asp:Literal>
					        </dt>
					        <dd class="search-li-desc">    
                                <a href="javascript:fnQnAGoView('<%# DataBinder.Eval(Container.DataItem, "ID")%>')" class="desc">    
						            <asp:Literal ID="litQnABody" runat="server"></asp:Literal></a>
                                    <p class="li-summary"><span class="editor">최근 편집자 : <a href="javascript:fnProfileView('<%# DataBinder.Eval(Container.DataItem, "UserID")%>')"><%# DataBinder.Eval(Container.DataItem, "UserName")%>/<%# DataBinder.Eval(Container.DataItem, "DeptName")%></a></span><span class="rating">추천수 : <%# DataBinder.Eval(Container.DataItem, "CommentHits")%></span></p>
					        </dd>
						   
				        </ItemTemplate>
			        </asp:Repeater>
		        </dl>

			    <!-- btn more -->
			    <div class="search-btn-bottom">
                    <div runat ="server" id="QnAMore" visible="true">
                       <a href="javascript:fnQnAList();" class ="btn-more"><span>더 보기</span></a>
                    </div>
			    </div>

			    <!-- BTN -->
		        <div id="list-btn2">
				    <div class="pagging" runat ="server" id="QnAPage" visible="false">
						    <Paging:PagerV2_8 ID="PagerQnA" OnCommand="pager_Command3" runat="server" GenerateHiddenHyperlinks="true" /> 
				    </div>
		        </div>
            </div>


             <div id="BodyProfile" runat ="server" visible="true">
		        <h3 class="result-heading-st"><a href="#">담당자 검색 결과</a> <span class="search-result-num"><label runat="server" id="lblPerson" ></label>명</span></h3>
		        <ul class="search-li-wrap-pic">
			        <asp:Repeater ID="rptInProfile" runat="server" OnItemDataBound="rptInProfile_OnItemDataBound" >
				        <ItemTemplate>
                            <li>
					            <div class="pic"><a href="javascript:fnProfileView('<%# DataBinder.Eval(Container.DataItem, "UserID")%>')"><asp:Literal runat="server" ID="UserImg"></asp:Literal></a></div>
					            <div class="name"><a href="javascript:fnProfileView('<%# DataBinder.Eval(Container.DataItem, "UserID")%>')"> <%# DataBinder.Eval(Container.DataItem, "UserName")%> /  <%# DataBinder.Eval(Container.DataItem, "DeptName")%></a></div>
				            </li>						   
				        </ItemTemplate>
			        </asp:Repeater>
		        </ul>

			    <!-- btn more -->
			    <div class="search-btn-bottom">
                    <div runat ="server" id="ProfileMore" visible="true">
                       <a href="javascript:fnProfileList();" class ="btn-more"><span>더 보기</span></a>
                    </div>
			    </div>

			    <!-- BTN -->
		        <div id="list-btn3">
				    <div class="pagging" runat ="server" id="ProfilePage" visible="false">
						    <Paging:PagerV2_8 ID="PagerProfile" OnCommand="pager_Command4" runat="server" GenerateHiddenHyperlinks="true" /> 
				    </div>
		        </div>
            </div>


		</div>
    </div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Common_Footer"
    runat="server">
    <asp:Button ID="btnSearch" runat="server" Style="display: none;" OnClick="btnSearch_Click" />
    <asp:HiddenField ID="hdMore" runat="server" /> 
<%--    <asp:HiddenField ID="hdContentMore" runat="server" />--%>
</asp:Content>
