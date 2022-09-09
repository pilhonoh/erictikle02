<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="GlossaryOutLookWrite.aspx.cs"
    Inherits="SKT.Glossary.Web.Glossary.GlossaryOutLookWrite" ValidateRequest="false" %>

<%@ Register Src="/NaverEditor/NaverSmartEditor.ascx" TagPrefix="uc1" TagName="NaverSmartEditor" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/DTD/loose.dtd">
<html lang="ko">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <script src="/common/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="/common/Css/common.css" rel="stylesheet" type="text/css" />
    <title>T.끌, 소통과 협업의 플랫폼</title>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#txtTitle").one("click", function () {
                $("#txtTitle").val("");
            });

            //빈페이지
            $("#write-tem-etc").click(function () {
                if (confirm("템플릿 변경시 작성중인 내용이 모두 사라집니다. 그래도 진행 하시겠습니까?")) {
                    Radio = "write-tem-etc";
                    oEditors.getById["ir1"].exec("SET_CONTENTS", [""]);
                } else {
                    $("input:radio[ID='" + Radio + "']").attr("checked", true);
                }

            });

            //용어
            $("#write-tem-word").click(function () {
                if (confirm("템플릿 변경시 작성중인 내용이 모두 사라집니다. 그래도 진행 하시겠습니까?")) {
                    Radio = "write-tem-word";
                    oEditors.getById["ir1"].exec("SET_CONTENTS", [""]);
                    oEditors.getById["ir1"].exec("PASTE_HTML", ['<h2>정의</h2>'
                                        + '<p>정의를 입력하세요.</p>'
                                        + '<p>&nbsp;</p>'
                                        + '<h2>약어</h2>'
                                        + '<p>약어를 입력하세요.</p>'
                                        + '<p>&nbsp;</p>'
                                        + '<h2>한글 명</h2>'
                                        + '<p>한글 명을 입력하세요.</p>']);
                } else {
                    $("input:radio[ID='" + Radio + "']").attr("checked", true);
                }
            });

            //프로세스
            $("#write-tem-work").click(function () {
                if (confirm("템플릿 변경시 작성중인 내용이 모두 사라집니다. 그래도 진행 하시겠습니까?")) {
                    Radio = "write-tem-work";
                    oEditors.getById["ir1"].exec("SET_CONTENTS", [""]);
                    oEditors.getById["ir1"].exec("PASTE_HTML", ['<h2>담당자</h2>'
                                        + '<p>담당자를 입력하세요.</p>'
                                        + '<p>&nbsp;</p>'
                                        + '<h2>연락처</h2>'
                                        + '<p>연락처를 입력하세요.</p>'
                                        + '<p>&nbsp;</p>'
                                        + '<h2>내용</h2>'
                                        + '<p>내용을 입력하세요.</p>']);
                } else {
                    $("input:radio[ID='" + Radio + "']").attr("checked", true);

                }
            });

            //////////////////////////////////////Tag 자동 검색///////////////////////////////////////////////////////////////////

            $("#<%= this.txtTag.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "/Common/Controls/AjaxControl.aspx/GetAutoCompleteTagData",
                        data: "{'username':'" + $('#<%= this.txtTag.ClientID %>').val() + "'}",
                        dataType: "json",
                        success: function (data) {
                            if ($('#<%= this.txtTag.ClientID %>').val().replace(/^\s+|\s+$/g, '') == "") {
                                return;
                            } else {
                                response(data.d);
                            }
                        },
                        error: function (result) {
                            alert("Error");
                        }
                    });
                }
            });


            $('.ui-autocomplete').click(function (e) {
                TagSet();

            });

            $('#<%= this.txtTag.ClientID %>').keyup(function (e) {
                if (e.keyCode == 13) {
                    TagSet();
                }
            });

            $('#<%= this.txtTag.ClientID %>').focusout(function () {
                TagSet();
            });

            if ($('#<%= this.txtTag.ClientID %>').val() != "") {
                TagSet();
            }

        });

        function TagSet() {
            var TagSearch = $('#<%= this.txtTag.ClientID %>').val();
            if (TagSearch.indexOf(',') == -1) {
                if (TagSearch.replace(/^\s+|\s+$/g, '') != "") {
                    TagSearch = "<span class=\"tag-list\" onclick=\"fnTagDelete(this);\">" + $('#<%= this.txtTag.ClientID %>').val().replace(/^\s+|\s+$/g, '') + "<a href=\"javascript:\">X</a></span>";
                    $('#<%= this.TagLists.ClientID %>').html($("#<%= this.TagLists.ClientID %>").html() + TagSearch);
                }

            } else {
                var TagSearchArr = [];
                TagSearchArr = TagSearch.split(",");
                for (var i = 0; i < TagSearchArr.length; i++) {
                    if (TagSearchArr[i].replace(/^\s+|\s+$/g, '') != "") {
                        TagSearch = "<span class=\"tag-list\" onclick=\"fnTagDelete(this);\">" + TagSearchArr[i] + "<a href=\"javascript:\">X</a></span>";
                        $('#<%= this.TagLists.ClientID %>').html($("#<%= this.TagLists.ClientID %>").html() + TagSearch);
                    }
                }
            }
            $('#<%= this.txtTag.ClientID %>').val("");
        }

        //////////////////////////////////////Tag 자동 검색  끝///////////////////////////////////////////////////////////////////

        function fnClose() {
            try {
                //window.close();
                window.open('about:blank', '_self').close();
            }
            catch (e) {
            }
            return false;
        }

        //저장 버튼
        function fnSave() {
            if ($("#txtTitle").val() == "  제목을 입력하세요" || $("#txtTitle").val().replace(/^\s+|\s+$/g, '') == "") {
                alert("  제목을 입력하세요");
                $("#txtTitle").unbind();
                $("#txtTitle").val("");
                $("#txtTitle").focus();
                return false;
            }
            if (oEditors.getById["ir1"].getIR() == "") {
                alert("내용을 입력하세요");
                return false;
            }

            $('#<%= this.hdTitle.ClientID %>').val($("#txtTitle").val());
            $('#<%= this.hdNamoContent.ClientID %>').val(HTagAnchor());

            var TagTotal = "";
            for (var i = 0; i < $(".tag-list").length; i++) {
                TagTotal += $(".tag-list")[i].innerText + ",";
            }

            $('#<%= this.hdTag.ClientID %>').val(TagTotal);
            return true;
        }

        function HTagAnchor() {
            var HTag = true;
            var $html = $("<div class='glossalydivviewdody'>" + getOrgChartHTML() + "</div>").wrapAll('<div />').parent();
            $.each(['H2', 'H3', 'H4'], function (idx, hEl) {

                $($html).find(hEl).each(function (idx, ele) {
                    $(this).attr("id", hEl + idx);
                    HTag = false;
                });
            });

            if (HTag == true) {
                return getOrgChartHTML();
            } else {
                return $html.html();
            }

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <!-- CONT -->
    <div id="cont">
        <!--DASH -->
        <div class="main-dash">
            <div class="main-dash-left">
                <h3 id="h1Title" runat="server" class="write">
                    티끌 작성</h3>
            </div>
            <div class="main-dash-right">
                <div class="anonymous">
                    <input runat="server" type="checkbox" id="btnPrivate" value="" name="" /><label for="write-anonymous">작성자
                        비공개</label>
                </div>
            </div>
        </div>
        <!-- context -->
        <div id="view-context-wrap">
            <!-- write area -->
            <ul class="write-docu">
                <li>
                    <h4>
                        제목</h4>
                    <fieldset>
                        <legend>제목</legend>
                        <div>
                            <input id="txtTitle" type="text" name="txtTitle" class="input-subject" value="  제목을 입력하세요" /></div>
                    </fieldset>
                </li>
                <li>
                    <h4>
                        템플릿</h4>
                    <fieldset>
                        <legend>템플릿</legend>
                        <div class="templet">
                            <input type="radio" value="1" name="rod_type01" id="write-tem-word" /><label for="write-tem-word">용어
                                설명</label>
                            <input type="radio" value="2" name="rod_type01" id="write-tem-work" /><label for="write-tem-work">업무
                                프로세스</label>
                            <input type="radio" value="3" name="rod_type01" id="write-tem-etc" checked="checked" /><label
                                for="write-tem-etx">기타</label>
                        </div>
                    </fieldset>
                </li>
                <li class="write-docu-text">
                    <h4>
                        내용</h4>
                    <fieldset>
                        <legend>내용</legend>
                        <div class="textarea">
                            <uc1:NaverSmartEditor ID="Editor" runat="server" SkinHtmlUrl="/NaverEditor/SmartEditor2Skin.html" />
                        </div>
                    </fieldset>
                </li>
                <li class="tag">
                    <h4>
                        연관 단어</h4>
                    <fieldset>
                        <legend>연관 단어</legend>
                        <div>
                            <input runat="server" id="txtTag" type="text" name="" class="input-tag" /></div>
                        <div runat="server" id="TagLists">
                        </div>
                    </fieldset>
                    <div class="write-warn">
                        <label for="write-warn1">
                            이 티끌을 요약할 수 있는 단어를 입력해 주세요.</label>
                    </div>
                </li>
            </ul>
        </div>
        <div id="view-bottom-wrap">
            <ul>
                <li class="rating"><a href="javascript:fnClose()" class="_btn_veiw_wrap"><span class="write-out">
                    작성 취소</span></a></li>
                <li class="history">
                    <asp:LinkButton ID="btnsavelink" CssClass="_btn_veiw_wrap" runat="server" OnClick="btnSave_Click"
                        OnClientClick="return fnSave();">
                        <span id="spSave" runat="server" class="edit">작성 완료</span></asp:LinkButton></li>
            </ul>
        </div>
    </div>
    <div style="display: none;">
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
        <asp:HiddenField ID="hdNamoContent" runat="server" />
        <asp:HiddenField ID="hdTitle" runat="server" />
        <asp:HiddenField ID="hdNamoContentText" runat="server" />
        <asp:HiddenField ID="hdCommonID" runat="server" />
        <asp:HiddenField ID="hidItemState" runat="server" />
        <asp:HiddenField ID="hidTempSaveID" runat="server" />
        <asp:HiddenField ID="hidRadioState" runat="server" />
        <asp:HiddenField ID="hdType" runat="server" />
        <asp:HiddenField ID="hdTag" runat="server" />
    </div>
    </form>
</body>
</html>
