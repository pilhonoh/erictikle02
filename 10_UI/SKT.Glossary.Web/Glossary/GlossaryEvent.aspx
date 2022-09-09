<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Glossary.Master" AutoEventWireup="true" CodeBehind="GlossaryEvent.aspx.cs" Inherits="SKT.Glossary.Web.GlossaryEvent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">
    
        
    <script src="/common/js/design.js" type="text/javascript"></script>


    <script src="/Common/js/jquery-ui.js" type="text/javascript"></script>
    <script src="/Common/js/json2.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Common_Top" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="cont">
		<!--DASH -->
		<div id="profile-dash">
			<h3 class="event">T.끌 오픈 이벤트</h3>
		</div>
				
		<div class="event1-wrap">
			<div><img src="/common/images/event1_1.png" /></div>
			<div class="event1-cont-left-wrap">
				<div><img src="/common/images/event1_2.png" /></div>
				<div class="event1-cont-box">
					<ul class="event1-state">
						<li>
							<h3>티끌왕 순위</h3>
							<div class="event1-count">
                            <asp:Literal runat="server" ID="Rank"></asp:Literal>
                            <span class="event1-unit">위</span></div>
						</li>
                        <li>
							<h3>댓글왕 순위</h3>
                            
							<div class="event1-count">
                            <asp:Literal runat="server" ID="AnswerRank"></asp:Literal>
                            <span class="event1-unit">위</span></div>
						</li>
						<li>
							<h3>모은 티끌 수</h3>
							<div class="event1-count">
                            <asp:Literal runat="server" ID="WriteCount"></asp:Literal>
                            <span class="event1-unit">개</span></div>
						</li>
						<li class="event1-last-child-fuck-ie8">
							<h3>채택 답변 수</h3>
							<div class="event1-count">
                            <asp:Literal runat="server" ID="AnswerCount"></asp:Literal>
                            <span class="event1-unit">개</span></div>
						</li>
					</ul>

					<div class="event1-rank-wrap">
						<h3 class="event1-rank-title">티끌왕 순위</h3>
						<ul class="event1-rank1">
                             <asp:Repeater ID="rptInRanking" runat="server">
                                 <ItemTemplate>
					                <li>
                                        <span><%# DataBinder.Eval(Container.DataItem, "ID")%>위</span><a href="#"><%# DataBinder.Eval(Container.DataItem, "Name")%> (<%# DataBinder.Eval(Container.DataItem, "WriteCount")%>)</a>
                                    </li>
                                </ItemTemplate>
                                </asp:Repeater>
							
						</ul>
					</div>

					<div class="event1-rank-wrap">
						<h3 class="event1-rank-title2">답변왕 순위</h3>
						<ul class="event1-rank1">
                             <asp:Repeater ID="rptInWriteRanking" runat="server">
                             <ItemTemplate>
					            <li><span><%# DataBinder.Eval(Container.DataItem, "ID")%>위</span><a href="#"><%# DataBinder.Eval(Container.DataItem, "Name")%> (<%# DataBinder.Eval(Container.DataItem, "WriteCount")%>)</a>
                                </li>
                            </ItemTemplate>
                            </asp:Repeater>
							
						</ul>
					</div>

				</div>
				<div><img src="/common/images/event1_4.png" /></div>
			</div>

			<div class="event1-cont-right-wrap">
				<div><img src="/common/images/event1_3.png" /></div>
				<div class="event1-cont-box">
					<table class="event1-check-table">
					  <tr>
					    <th>하나</td>
					    <th>둘</td>
					    <th>셋</td>
					    <th>넷</td>
					    <th>다섯</td>
					  </tr>
					  <tr>
                        <asp:Literal runat="server" ID="Stamp1"></asp:Literal>
					  </tr>
					  <tr>
					    <th>여섯</td>
					    <th>일곱</td>
					    <th>여덟</td>
					    <th>아홉</td>
					    <th>열</td>
					  </tr>
					  <tr>
                        <asp:Literal runat="server" ID="Stamp2"></asp:Literal>
					  </tr>
					</table>
				</div>
				<div><img src="/common/images/event1_6.png" /></div>
			</div>
            <div><img src="/common/images/event1_5.png" /></div>
		</div>
	</div>
     
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Common_Footer" runat="server">
    
   
</asp:Content>

    