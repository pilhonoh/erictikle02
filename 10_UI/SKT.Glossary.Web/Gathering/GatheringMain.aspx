<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossarySearch.Master" AutoEventWireup="true" CodeBehind="GatheringMain.aspx.cs" Inherits="SKT.Glossary.Web.Gathering.GatheringMain" %>
<%@ Register Src="~/Common/Controls/GatheringInfomation.ascx" TagName="GatheringInfomation" TagPrefix="common" %>
<%@ Register Src="~/Common/Controls/GatheringMenuTab.ascx" TagName="GatheringMenuTab" TagPrefix="common" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Common_Top" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var lnbDep1 = 6;		//LNB 1depth

        $(document).ready(function () {

            $("#GetMoreArea").hide();
            getGatheringTagSelect(1, 5);

           
        });

        function getGatheringTagSelect(Board_Index, Board_Count) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Gathering/Main.aspx/GetGatheringMainTagList",
                data: "{GatheringID : '<%= GatheringID %>', Board_Index : '" + Board_Index + "', Board_Count : '" + Board_Count + "'}",
                dataType: "json",
                success: function (data) {

                    if (Board_Index == 1)
                        setNewGlossary(data.d.Table);

                    setGlossaryTagBoard(data.d.Table1);

                    if (data.d.Table1.length == 0)
                        $("#GetMoreArea").hide();
                    else {
                        var Page_Index = (parseInt(Board_Index) / parseInt(Board_Count)) + 1;
                        var tot = parseInt(Page_Index) * parseInt(Board_Count);
                        //alert(data.d.Table1[0].TotalCount + '-' + tot);
                        var totcount = data.d.Table1[0].TotalCount;
                        if (Board_Index > 1) totcount = totcount + 1;
                        if (totcount <= tot)
                            $("#GetMoreArea").hide();
                    }
                },
                error: function (result) {
                    //alert("Error" + ":::" + result);
                },
                complete: function () {
                    //$("#imgLoading").hide();
                    //getFileList();

                    var text = $('#knowhow_main li dd > a');
                }
            });

        }

        function setNewGlossary(Table) {
            var tempBoard = "";
            var tagTitle = "전체";
            tempBoard += '<li>';
            tempBoard += '<dl class="new">'; //color:#1d87be;
            tempBoard += '<dt style="cursor:pointer;" onclick="javascript:fnGlossaryList(\'\',\'CreateDate\');">' + tagTitle + '</dt>';
            for (var i = 0; i < Table.length; i++) {
                var CommentCnt = "";
                
                if (parseInt(Table[i].CommentCount) > 0)
                    CommentCnt = " [" + Table[i].CommentCount + "]";

                tempBoard += "<dd style=\"padding-left:10px;width:340px;\">";
                tempBoard += "<table style=\"width:340px;table-layout:fixed;\" ><tr>";
                tempBoard += "<td style=\"width:270px; font-size:14px;\"><a href=\"javascript:fnView('','" + Table[i].CommonID + "');\" title=\"" + Table[i].Title + "\" >" + inputLengthCheck(Table[i].Title, 'title') + CommentCnt + "</a></td>";
                tempBoard += "<td style=\"width:70px; font-size:14px;\">" + Table[i].ModifyDate + "</td>";
                tempBoard += "</tr></table>";
                tempBoard += "</dd>";
            }
            if (Table.length == 0) {
                tempBoard += '<dd><a href="javascript:;">등록된 끌.모임 게시글이 없습니다.</a></dd>';
            }
            tempBoard += '</dl>';
            tempBoard += '</li>';
            $("#knowhow_main").append(tempBoard);
        }

        var tagTitle = "";
        var tagTitleSeq = 0;
        function setGlossaryTagBoard(Table) {
            var eleTempBoard;

            for (var i = 0; i < Table.length; i++) {


                var CommentCnt = "";
                if (parseInt(Table[i].CommentCount) > 0)
                    CommentCnt = " [" + Table[i].CommentCount + "]";

                if (tagTitle != Table[i].TagTitle) {
                    
                    tagTitleSeq++;
                    if (i != 0) {
                        $("#knowhow_main").append(eleTempBoard);
                    }
                    tagTitle = Table[i].TagTitle;
                    var tempBoard = "";
                    tempBoard += '<li>';
                    
                    if (Table[i].GatheringTag == "Y")
                        tempBoard += '<dl class="new">';
                    else 
                        tempBoard += '<dl>';
                        
                        //tempBoard += '<dt style="cursor:pointer;color:#1d87be;" onclick="javascript:fnGlossaryList(\'' + encodeURIComponent(encodeURIComponent(Table[i].TagTitle)) + '\',\'\')">' + tagTitle + '</dt>';
                    tempBoard += "<dt style=\"cursor:pointer;\" onclick=\"javascript:fnGlossaryList('" + encodeURIComponent(encodeURIComponent(Table[i].TagTitle)) + "', '');\">" + inputLengthCheck(tagTitle, '') + "</dt>";

                    tempBoard += '</dl>';
                    tempBoard += '</li>'
              
                    var tempDD = ""; //<b style=\'color:black;\'>' + Table[i].RowNumber + '.</b>
                    tempDD += "<dd style=\"padding-left:10px;width:340px;\">";
                    tempDD += "<table style=\"width:340px;table-layout:fixed;\" ><tr>";
                    tempDD += "<td style=\"width:270px; font-size:14px;\"><a href=\"javascript:fnView('','" + Table[i].CommonID + "');\" title=\"" + Table[i].Title + "\" >" + inputLengthCheck(Table[i].Title, 'title') + CommentCnt + "</a></td>";
                    tempDD += "<td style=\"width:70px; font-size:14px;\">" + Table[i].ModifyDate + "</td>";
                    tempDD += "</tr></table>";
                    tempDD += "</dd>";
                    eleTempBoard = $.parseHTML(tempBoard);
                    $(eleTempBoard).find("dl").append(tempDD);
                }
                else {
                    var tempDD = ""; //<b style=\'color:black;\'>' + Table[i].RowNumber + '.</b>
                    tempDD += "<dd style=\"padding-left:10px;width:340px;\">";
                    tempDD += "<table style=\"width:340px;table-layout:fixed;\" ><tr>";
                    tempDD += "<td style=\"width:270px; font-size:14px;\"><a href=\"javascript:fnView('','" + Table[i].CommonID + "');\" title=\"" + Table[i].Title + "\" >" + inputLengthCheck(Table[i].Title, 'title') + CommentCnt + "</a></td>";
                    tempDD += "<td style=\"width:70px; font-size:14px;\">" + Table[i].ModifyDate + "</td>";
                    tempDD += "</tr></table>";
                    tempDD += "</dd>";
                    $(eleTempBoard).find("dl").append(tempDD);
                }

            }
            $("#knowhow_main").append(eleTempBoard);
        }

        function inputLengthCheck(inputText, gbn) {

            var rtnText = "";

            var inputMaxLength = 34;

            if (gbn == 'title')
                inputMaxLength = 32;

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

                rtnText = inputText.substr(0, count) + "..";
            }
            else {
                rtnText = inputText;
            }

            return rtnText
        }

        function fnGlossaryList(tagtitle1) {
            //tagtitle = encodeURIComponent(encodeURIComponent(tagtitle));
            document.location.href = "/Glossary/GlossaryNewsList.aspx?TagTitle=" + tagtitle1 + "&SearchSort=CreateDate&GatheringYN=<%=this.GatheringYN %>&GatheringID=<%=this.GatheringID %>";
        }

        function fnView(tagtitle, ItemID) {
            var url = "/Glossary/GlossaryView.aspx?TagTitle=" + tagtitle + "&ItemID=" + ItemID + "&GatheringYN=<%=this.GatheringYN %>&GatheringID=<%=this.GatheringID %>";
             location.href = url;
        }

        function GetMoreTagBoard() {
            var Board_Index = $("#knowhow_main li").length+1;
            var Board_Count = 6;

            tagTitle = "";
            tagTitleSeq = 0;
            getGatheringTagSelect(Board_Index, Board_Count);
        }
        
    </script>
    <div id="container" class="Gathering">
		<!--CONTENTS-->
		<div id="contents">
            <div class="h2tag">
                <common:GatheringInfomation ID="GatheringInfomation1" runat="server" />
            </div>
			<p class="btn_top"><%--<a href="/Glossary/GlossaryWriteNew.aspx?GatheringYN=<%=GatheringYN%>&GatheringID=<%=GatheringID%>"><img src="/common/images/btn/write<%if(GatheringYN=="Y"){ %>_gathering<%}%>.png" alt="글쓰기" /></a>--%></p>
			<!--article-->
			<div id="article" style="padding-top:60px;">
               
                <common:GatheringMenuTab ID="GatheringMenuTabCtrl" runat="server" />
				<%--<ul id="tabMenu" style="padding-bottom:0px;">
					<li><a href="/Gathering/Main.aspx?DivType=Pub" <%= m_pub%>><img src="/common/images/btn/Gathering_tab1.png" alt="모든 모임" /></a></li>
					<li><a href="/Gathering/Main.aspx?DivType=Pri" <%= m_pri%>><img src="/common/images/btn/Gathering_tab2.png" alt="내가 만든 모임" /></a></li>
					<li><a href="/Gathering/Main.aspx?DivType=Vis" <%= m_vis%>><img src="/common/images/btn/Gathering_tab3.png" alt="초대된 모임" /></a></li>
				</ul>--%>
				
			<!--/article-->
            <ul id="knowhow_main" style=" margin-top:0px;">
				
            </ul>
            <p id="GetMoreArea"><a href="javascript:GetMoreTagBoard();" class="btn_more"><span>더보기</span></a></p>
        </div>
	</div>
	<!--/CONTENTS-->
</div>
</asp:Content>