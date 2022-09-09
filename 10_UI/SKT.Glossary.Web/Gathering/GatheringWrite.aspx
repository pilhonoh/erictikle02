<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossarySearch.Master" AutoEventWireup="true" CodeBehind="GatheringWrite.aspx.cs" Inherits="SKT.Glossary.Web.Gathering.GatheringWrite" %>

<%@ Register Src="~/Common/Controls/UserAndDepartmentList.ascx" TagName="UserAndDepartment" TagPrefix="common" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">
    <script type="text/javascript">
        var m_mode = '<%= mode %>';
        var m_focus = false;

        var m_ItemID = '<%= ItemID %>';
        var m_CommonID = '<%= CommonID %>';
        var m_UserID = '<%= UserID %>';

        var AlarmOpen = true;
        var ShareOpen = true;
        var DownLoadOpen = true;

        var m_titleCheck = false;

        var initTitle = "새로운 모임 이름을 입력해주세요";

        $(document).ready(function () {

            $('#<%= this.txtTitle.ClientID %>').one("click", function () {
                $('#<%= this.txtTitle.ClientID %>').val("");
            });
        });

        //닫기
        function CloseWindow() {

            location.href = "/Gathering/Main.aspx?DivType=<%= DivType %>";
        }

        //저장
        function fnSave() {

            /*
                Author : 개발자-김성환D, 리뷰자-진현빈D
                Create Date : 2016.08.04 
                Desc : 특수문자 " ' \ 제외
            */
            if ($('#<%= this.txtTitle.ClientID %>').val().indexOf('\'') >= 0 || $('#<%= this.txtTitle.ClientID %>').val().indexOf('\"') >= 0 || $('#<%= this.txtTitle.ClientID %>').val().indexOf('\\') >= 0) {
                alert("모임 이름에 ' 또는 \" 또는 \\ 를 제거하고 저장해주세요.");
                return;
            }

            var uploadType = $("input:radio[name='rdFileUp']:checked").val();

            /*
            Author : 개발자-김성환D, 리뷰자-이정선G
            Create Date : 2016.02.17 
            Desc : 끌모임 제목 필드 스크립트 제거
            */
            $('#<%= this.txtTitle.ClientID %>').val(strip_tag($('#<%= this.txtTitle.ClientID %>').val()));

            if ($('#<%= this.txtTitle.ClientID %>').val() == initTitle || $('#<%= this.txtTitle.ClientID %>').val().replace(/^\s+|\s+$/g, '') == "") {
                alert("모임 이름을 입력하세요");
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

            //조직도
            fnShareSave();

            __doPostBack('<%=btnSave.UniqueID %>', '');

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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Common_Top" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        var lnbDep1 = 6;		//LNB 1depth
    </script>

    <div id="container" class="Gathering">
        <!--CONTENTS-->
        <div id="contents">
            <h2>
                <p>
                    <img src="/common/images/text/Gathering_text.png" alt="끌.모임" />
                    <!--<span>
					 검색 넣어주세요 
				</span>-->
                </p>
			</h2>
            <%--<p class="search_top">
                <input name="q" id="txt_SearchKeyword" type="text" value="찾고싶은 끌.문서를 검색해 보세요" onfocus="this.value=''" onkeypress="if(event.keyCode == 13){fnFileSearch();}" />
                <a href="javascript:fnFileSearch()">
                    <img src="/common/images/etc/search_btn.png" alt="" title="검색" /></a>
                <script type="text/javascript">
                    function fnFileSearch() {
                        var t = $("#txt_SearchKeyword");

                        if (t.val().trim() == "" || t.val() == "찾고싶은 끌.문서를 검색해 보세요") {
                            alert("검색어를 입력해주세요.");
                            t.val("");
                            t.focus();
                            return;
                        }

                        location.href = "/Directory/DirectorySearchResult.aspx?q=" + t.val();
                    }
                </script>
            </p>--%>
            <!--article-->
            <div id="article">
               <%-- <ul id="tabMenu">
                    <li><a href="/Gathering/Main.aspx?DivType=Pub" <%= m_pub%>><img src="/common/images/btn/Gathering_tab1.png" alt="모든 모임" /></a></li>
					<li><a href="/Gathering/Main.aspx?DivType=Pri" <%= m_pri%>><img src="/common/images/btn/Gathering_tab2.png" alt="내가 만든 모임" /></a></li>
					<li><a href="/Gathering/Main.aspx?DivType=Vis" <%= m_vis%>><img src="/common/images/btn/Gathering_tab3.png" alt="초대된 모임" /></a></li>
                </ul>--%>
                <table class="writeTable">
                    <colgroup>
                        <col width="180px" />
                        <col width="*" />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th><span style="color:orangered;">*</span>모임 이름</th>
                            <%--Author : 개발자-김성환D, 리뷰자-이정선G
                            Create Date : 2016.02.17 
                            Desc : MaxLength 처리--%>
                            <td>
                                <input type="text" id="txtTitle" name="txtTitle" runat="server" class="txt t1" value="새로운 모임 이름을 입력해주세요" maxlength="100"/></td>
                        </tr>                        
                        <tr>
                            <th>모임 멤버 설정하기</th>
                            <td>
                                <%--<a href="#pop_dc_member" class="btn1 btn_pop"><span>설정하기</span></a>--%>
                                <fieldset class="authority" style="border:0;">
                                    <common:UserAndDepartment ID="UserControl" runat="server" ViewType="WriteNew" />
                                </fieldset>
                            </td>
                        </tr>
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
    </div>

   <%-- <div class="pop">
        <div class="popBg"></div>

        <!--조회권한 설정하기-->
        <div id="pop_dc_member" class="layer_pop">
            <h3>모임 멤버 설정하기</h3>
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

    <div style="display: none;">
        <asp:HiddenField ID="hidMenuType" runat="server" />
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
        <asp:HiddenField ID="hdDirctoryID" runat="server" />
        <asp:HiddenField ID="hdCommonID" runat="server" />
        <asp:HiddenField ID="hdBoardID" runat="server" />
        <asp:HiddenField ID="hdItemGuid" runat="server" />
    </div>
</asp:Content>