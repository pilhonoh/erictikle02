<%@ Page Language="C#" MasterPageFile="~/MasterPages/GlossarySearch.Master" AutoEventWireup="true" CodeBehind="Glossary.aspx.cs" Inherits="SKT.Glossary.Web.Glossary.Glossary" %>


<%@ Register Src="~/Common/Controls/GatheringInfomation.ascx" TagName="GatheringInfomation" TagPrefix="common" %>
<%@ Register Src="~/Common/Controls/GatheringMenuTab.ascx" TagName="GatheringMenuTab" TagPrefix="common" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">
    <script type="text/javascript">

        function fnView(tagtitle, ItemID, searchsort) {
            var url = "/Glossary/GlossaryView.aspx?ItemID=" + ItemID + "&TagTitle=" + tagtitle + "&SearchSort=" + searchsort;
            location.href = url;
        }

        function fnGlossaryList(tagtitle, searchsort) {
            var url = "/Glossary/GlossaryNewsList.aspx?TagTitle=" + tagtitle + "&SearchSort=" + searchsort;
            location.href = url;
        }

        function getGlossaryMainInfoSelect() {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Glossary/Glossary.aspx/GetGlossaryMainInfoSelect",
                data: "{UserID : '<%=this.UserID%>', GatheringYN : '<%=this.GatheringYN %>', GatheringID : '<%=this.GatheringID %>'}",
                dataType: "json",
                success: function (data) {
                    if (data.d != null && data.d.Table != undefined) {
                        setTag(data.d.Table3, 'tag_box');
                        $("#knowhow_main").empty();
                        setNewGlossary(data.d.Table);
                        setCommentGlossary(data.d.Table1);
                        setLikeGlossary(data.d.Table2);
                        setGlossaryTagBoard(data.d.Table4);
                        if (data.d.Table4.length < 4) {
                            $("#GetMoreArea").hide();
                        }
                    }
                },
                error: function (result) {
                    //alert("Error");
                    //alert(result.responseText);
                }
            });
        }

        function setTag(Table, Id) {
            if (Table.length < 9) {
                $("#GetMoreBtn").hide();
            }
            for (var i = 0; i < Table.length; i++) {
                //var tagCss = "txt" + (Math.floor(Math.random() * 10) + 1).toString();
                var tagCss = "txt7";
                var tempTag = "";

                var TagTitle = Table[i].TagTitle;
                //CHG610000063658 / 2018-05-17 / 최현미 / SSO 연동관련 오류 수정 요청
                //SSO 통과 시 decodeURIComponent된 url을 넘겨주기때문에 문제 되는 코드 치환
                //&문자는 $로 치환 후 cs에서 처리
                var LinkTagTitle = TagTitle.replace(/&/g, "$");

                tempTag = '<li style=\"width:270px;\"><a href=javascript:fnGlossaryList(\'' + encodeURIComponent(encodeURIComponent(LinkTagTitle)) + '\',\'\') class="' + tagCss + '" >' + TagTitle + '(' + Table[i].ShowCount + ')</a></li>';
                
                $("#" + Id + " ul").append(tempTag);
            }
        }

        function GetMoreTagOpen() {
            //var Tag_Index = $("#tag_box_pop ul li").length;
            //var Tag_Count = 32;

            //if (Tag_Index == 0) {
            //    getGlossaryMainTagSelect(Tag_Index, Tag_Count);
            //}

            //$("#tag_pop").show();

            if ($("#GetMoreBtn").attr("class") == "btn") {
                $("#tag_box").attr("style", "height:304px");
                $("#GetMoreBtn").attr("class", "btn on");
            }
            else {
                $("#tag_box").attr("style", "height:76px");
                $("#GetMoreBtn").attr("class", "btn");
            }

        }

        function getTextLength(str) {
            var len = 0;
            for (var i = 0; i < str.length; i++) {
                if (escape(str.charAt(i)).length == 6) {
                    len++;
                }
                len++;
            }

            if (len > 16)
                str = str.substring(0, 16) + "..";
            //alert(str);
            return str;
        }

        var tagTitle = "";
        var LinkTagTitle = "";
        function setGlossaryTagBoard(Table) {
            var eleTempBoard;

            for (var i = 0; i < Table.length; i++) {
                if (tagTitle != Table[i].TagTitle) {
                    if (i != 0) {
                        $("#knowhow_main").append(eleTempBoard);
                    }
                    tagTitle = Table[i].TagTitle;
                    //CHG610000063658 / 2018-05-17 / 최현미 / SSO 연동관련 오류 수정 요청
                    //SSO 통과 시 decodeURIComponent된 url을 넘겨주기때문에 문제 되는 코드 치환
                    //&문자는 $로 치환 후 cs에서 처리
                    LinkTagTitle = tagTitle.replace(/&/g, "$");

                    var tempBoard = "";
                    tempBoard += '<li>';
                    tempBoard += '<dl>';
                    tempBoard += '<dt style="cursor:pointer" onclick="javascript:fnGlossaryList(\'' + encodeURIComponent(encodeURIComponent(LinkTagTitle)) + '\',\'\')">' + tagTitle + '</dt>';
                    tempBoard += '</dl>';
                    tempBoard += '</li>'

                    var tempDD = "";
                    tempDD += '<dd><b>' + Table[i].RowNumber + '.</b><a href=javascript:fnView("' + encodeURIComponent(encodeURIComponent(LinkTagTitle)) + '","' + Table[i].CommonID + '","' + '")>' + Table[i].Title + '</a></dd>';
                    eleTempBoard = $.parseHTML(tempBoard);
                    $(eleTempBoard).find("dl").append(tempDD);
                }
                else {
                    var tempDD = "";
                    tempDD += '<dd><b>' + Table[i].RowNumber + '.</b><a href=javascript:fnView("' + encodeURIComponent(encodeURIComponent(LinkTagTitle)) + '","' + Table[i].CommonID + '","' + '")>' + Table[i].Title + '</a></dd>';
                    $(eleTempBoard).find("dl").append(tempDD);
                }

            }

            $("#knowhow_main").append(eleTempBoard);
        }

        function setNewGlossary(Table) {
            var tempBoard = "";
            var tagTitle = "최신지식";
            var searchsort = "CreateDate";
            tempBoard += '<li>';
            tempBoard += '<dl class="new">';
            tempBoard += '<dt style="cursor:pointer" onclick="javascript:fnGlossaryList(\'\',\'CreateDate\');">' + tagTitle + '</dt>';
            for (var i = 0; i < Table.length; i++) {
                tempBoard += '<dd><b style=\'color:black;\'>' + (i + 1).toString() + '.</b><a href=javascript:fnView("' + '","' + Table[i].CommonID + '","' + searchsort + '")>' + Table[i].Title + '</a></dd>';
            }
            if (Table.length == 0) {
                tempBoard += '<dd><a href="javascript:;">등록된 지식이 없네요.<br />지식을 등록해주세요.</a></dd>';
            }
            tempBoard += '</dl>';
            //tempBoard += '<a href="javascript:fnGlossaryList(\'\',\'CreateDate\');"  class="btn"><img src="/common/images/btn/more.png" alt="" title="더보기" /></a>';
            tempBoard += '</li>';
            $("#knowhow_main").append(tempBoard);
        }
        function setCommentGlossary(Table) {

            var tempBoard = "";
            var tagTitle = "인기지식";
            var searchsort = "Hits";
            tempBoard += '<li>';
            tempBoard += '<dl class="new">';
            tempBoard += '<dt style="cursor:pointer" onclick="javascript:fnGlossaryList(\'\',\'Hits\');">' + tagTitle + '</dt>';
            for (var i = 0; i < Table.length; i++) {
                tempBoard += '<dd><b style=\'color:black;\'>' + (i + 1).toString() + '.</b><a href=javascript:fnView("' + '","' + Table[i].CommonID + '","' + searchsort + '")>' + Table[i].Title + '</a></dd>';
            }
            if (Table.length == 0) {
                tempBoard += '<dd><a href="javascript:;">등록된 지식이 없네요.<br />지식을 등록해주세요.</a></dd>';
            }
            tempBoard += '</dl>';
            //tempBoard += '<a href="javascript:fnGlossaryList(\'\',\'Hits\');" class="btn"><img src="/common/images/btn/more.png" alt="" title="더보기" /></a>';
            tempBoard += '</li>';
            $("#knowhow_main").append(tempBoard);
        }
        function setLikeGlossary(Table) {

            var tempBoard = "";
            var tagTitle = "추천지식";
            var searchsort = "Like";

            tempBoard += '<li>';
            tempBoard += '<dl class="new">';
            tempBoard += '<dt style="cursor:pointer" onclick="javascript:fnGlossaryList(\'\',\'Like\');">' + tagTitle + '</dt>';
            for (var i = 0; i < Table.length; i++) {
                tempBoard += '<dd><b style=\'color:black;\'>' + (i + 1).toString() + '.</b><a href=javascript:fnView("' + '","' + Table[i].CommonID + '","' + searchsort + '")>' + Table[i].Title + '</a></dd>';
            }
            if (Table.length == 0) {
                tempBoard += '<dd><a href="javascript:;">등록된 지식이 없네요.<br />지식을 등록해주세요.</a></dd>';
            }
            tempBoard += '</dl>';
            //tempBoard += '<a href="javascript:fnGlossaryList(\'\',\'Like\');" class="btn"><img src="/common/images/btn/more.png" alt="" title="더보기" /></a>';
            tempBoard += '</li>';
            $("#knowhow_main").append(tempBoard);
        }

        function getGlossaryMainTagBoardSelect(Board_Index, Board_Count, Board_RowCount) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Glossary/Glossary.aspx/GetGlossaryMainTagBoardSelect",
                data: "{Board_Index : '" + Board_Index + "', Board_Count : '" + Board_Count + "', Board_RowCount : '" + Board_RowCount + "', UserID : '<%=this.UserID%>' }",
                dataType: "json",
                success: function (data) {
                    setGlossaryTagBoard(data.d.Table);
                },
                error: function (result) {
                    //alert("Error");
                    //alert(result);
                }
            });
        }

        function getGlossaryMainTagSelect(Tag_Index, Tag_Count) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Glossary/Glossary.aspx/GetGlossaryMainTagSelect",
                data: "{Tag_Index : '" + Tag_Index + "', Tag_Count : '" + Tag_Count + "', GatheringYN : '<%=this.GatheringYN %>', GatheringID : '<%=this.GatheringID %>', UserID : '<%=this.UserID%>' }",
                dataType: "json",
                success: function (data) {
                    setTag(data.d.Table, 'tag_box_pop');
                },
                error: function (result) {
                    //alert("Error");
                    //alert(result);
                }
            });
        }


        function GetMoreTagBoard() {
            var Board_Index = $("#knowhow_main li").length - 2;
            var Board_Count = 6;
            var Board_RowCount = 5;

            tagTitle = "";
            getGlossaryMainTagBoardSelect(Board_Index, Board_Count, Board_RowCount);
        }




        function GetMoreTagClose() {
            $("#tag_pop").hide();
        }

    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Common_Top" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var lnbDep1 = 1;		//LNB 1depth
    </script>
    <!--CONTENTS-->
    <div id="contents">
        <h2>
            <img src="/common/images/text/Glossary.png" alt="끌.지식" />
        </h2>
        <p class="btn_top">
            <a href="/Glossary/GlossaryWriteNew.aspx">
                <img src="/common/images/btn/write.png" alt="글쓰기" /></a>
        </p>
        <div id="article" style="padding-top:22px;">

            <!--태그-->
            <div id="tag_box" class="tag_box" >
                <ul>
                </ul>
                <a id="GetMoreBtn" href="javascript:" onclick="GetMoreTagOpen()" class="btn">더보기</a>
            </div>
            <div id="tag_pop">
                <div class="bg"></div>
                <div id="tag_box_pop" class="tag_box">
                    <ul>
                    </ul>
                    <a href="javascript:" onclick="GetMoreTagClose()" class="btn on">닫기</a>
                    
                </div>
            </div>
            <!--/태그-->
            <ul id="knowhow_main">
            </ul>
            <p id="GetMoreArea"><a href="javascript:GetMoreTagBoard();" class="btn_more"><span>더보기</span></a></p>
        </div>
    </div>
    <!--/CONTENTS-->
    
    <script type="text/javascript">
        $(document).ready(function () {
            if ("<%=GatheringYN%>" == "Y") {
                $("#container").attr('class', 'Gathering');
            }
            getGlossaryMainInfoSelect();
        });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Common_Footer" runat="server">    
    
</asp:Content>