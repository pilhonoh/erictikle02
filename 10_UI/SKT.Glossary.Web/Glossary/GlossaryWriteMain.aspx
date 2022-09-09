<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Glossary.master" AutoEventWireup="true"
    CodeBehind="GlossaryWriteMain.aspx.cs" Inherits="SKT.Glossary.Web.Glossary.GlossaryWriteMain"
    ValidateRequest="false" %>

<asp:Content ID="cphHead" ContentPlaceHolderID="ContentPlaceHolder_Common_Head" runat="server">
    <script src="/common/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Common/js/jquery.cookie.js" type="text/javascript"></script>
    <script src="/common/js/design.js" type="text/javascript"></script>
    <style type="text/css">
        .infoTitle
        {
            background: url('/Common/images/icon_write_warn.png') left 4px no-repeat;
            font-size: 18px;
        }

        .selectDocType
        {
            border-top: 2px solid #686868;
            margin-left: 20px;
        }

        .selectDocType dl
        {
            border-bottom: 1px solid #dedede;
        }

        .selectDocType dt
        {
            float: left;
            width: 160px;
            padding: 10px;
            font-weight: bold;
        }

        .selectDocType dd
        {
            float: left;
            padding: 10px;
        }

        .selectDocType .clear
        {
            clear: both;
        }

        #docTypeA, #docTypeB
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="cphTop" ContentPlaceHolderID="ContentPlaceHolder_Common_Top" runat="server">
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="MainContent" runat="server">
    <!-- CONT -->
    <div id="cont">
        <!--DASH -->
        <div class="main-dash">
            <div class="main-dash-left">
                <h3>
                    티끌작성 Type 선택</h3>
            </div>
        </div>
        <!-- context -->
        <div id="view-context-wrap">
            <div class="context-db">                
                <div class="selectDocType" style="margin-top: -5px">
                    <dl><dt>정보/지식</dt><dd>
                        <fieldset><legend>라디오리스트</legend><ul class="raTable">
                            <asp:Repeater ID="rptCategory" runat="server">
                                <ItemTemplate><li>
                                <a href="javascript:;">
                                    <input id="rbtnTikle<%# DataBinder.Eval(Container.DataItem, "ID")%>" type="radio" name="rod_type01" 
                                            value='<%# DataBinder.Eval(Container.DataItem, "ID")%>' 
                                            onclick="clickCategory(this,'<%# DataBinder.Eval(Container.DataItem, "ID")%>')" 
                                            categorycontents='<%# DataBinder.Eval(Container.DataItem, "CategoryContents")%>' />
                                    <label for="rbtnTikle<%# DataBinder.Eval(Container.DataItem, "ID")%>">
                                        <%# DataBinder.Eval(Container.DataItem, "CategoryTitle")%>
                                        <span class="desc">
                                            <i><%# DataBinder.Eval(Container.DataItem, "NOTES")%></i>
                                        </span></label><input type="hidden" id="hdCategoryID" value="" style="display: none;" /></a>
                                        </li>
                                </ItemTemplate>
                            </asp:Repeater></ul>
                        </fieldset></dd>
                        <div class="clear">
                        </div>
                    </dl>
                    <dl>
                        <dt>질문</dt>
                        <dd>
                            <ul class="raTable">
                                <li><a>
                                    <input id="rBtnQna" name="rBtnA" type="radio" value="rBtnQna" /><label for="rBtnQna">질문등록</label>
                                </a></li>
                            </ul>
                        </dd>
                        <div class="clear">
                        </div>
                    </dl>
                </div>
            </div>
        </div>
        <div class="sub-contents" id="writeContents">
            <iframe class="ifrContents" id="ifrContents" frameborder="0" height="0px" scrolling="yes" >
            </iframe>
        </div>
    </div>    
</asp:Content>
<asp:Content ID="cphFoot" ContentPlaceHolderID="ContentPlaceHolder_Common_Footer"
    runat="server">
    <script type="text/javascript">
        
        $(document).ready(function () {
        });

        // Page 이동
        function rBtnChange(attrSrc) {
            $('#ifrContents').attr("src", attrSrc);
            $('#ifrContents').load(function () {
                this.style.height = this.contentWindow.document.body.offsetHeight + 'px';
            });
            $('#writeContents').show();
        }

        // 질문 선택시
        $("input[name='rBtnA']").change(function () {

            // 정보/지식에 클릭이 되어있다면
            // ifram 자체를 Reload
            if (document.getElementById('hdCategoryID').value != "") {
                if (!confirm("변경시 작성중인 내용이 모두 사라집니다. 그래도 진행 하시겠습니까?")) {
                    setSelectRadioCheck('rBtnA', '');
                    return false;
                }
                document.getElementById('hdCategoryID').value = "";
                setSelectRadioCheck('rod_type01', '');
                rBtnChange("../QnA/QnAWriteSimple.aspx");
            }
            else {
                rBtnChange("../QnA/QnAWriteSimple.aspx");
            }
        });

        // 2014-04-29 Mr.No
        function clickCategory(el, ID) {
            var hdCategoryID = document.getElementById('hdCategoryID').value;
            
            // 질문등록이 체크가 되어 있다면
            // ifram 자체를 Reload
            if ($("input:[NAME='rBtnA']").is(":checked")) {
                if (!confirm("변경시 작성중인 내용이 모두 사라집니다. 그래도 진행 하시겠습니까?")) {
                    setSelectRadioCheck('rod_type01', '');
                    return false;
                }
                setSelectRadioCheck('rBtnA', '');
                savaHiddenCategoryID(ID);
                rBtnChange("GlossaryWriteSimple.aspx");
                // 템플릿 적용
                setTimeout(function () { DefaultTemplate($(el).attr("CategoryContents")) }, 2 * 1000);
            }
            // 처음으로 정보/지식을 선택하는 경우
            else {
                if (hdCategoryID == "") {
                    // 템플릿이 존재하는 경우
                    if ($(el).attr("CategoryContents") != "") {
                        savaHiddenCategoryID(ID);
                        rBtnChange("GlossaryWriteSimple.aspx");
                        // 템플릿 적용
                        setTimeout(function () { DefaultTemplate($(el).attr("CategoryContents")) }, 2 * 1000);
                    } else {
                        savaHiddenCategoryID(ID);
                        rBtnChange("GlossaryWriteSimple.aspx");
                    }
                }
                else {
                    // 템플릿이 존재하는 경우
                    //if ($(el).attr("CategoryContents") != "") {
                        // Category를 변경하는 경우
                        if (!confirm("변경시 작성중인 내용이 모두 사라집니다. 그래도 진행 하시겠습니까?")) {
                            setSelectRadioCheck('rod_type01', hdCategoryID);
                            return false;
                        }
                        // Template Add
                        setTimeout(function () { DefaultTemplate($(el).attr("CategoryContents")) }, 0.5 * 1000);
                        savaHiddenCategoryID(ID);
//                    }
//                    else { // 템플릿이 존재하지 않는 경우
//                        savaHiddenCategoryID(ID);
//                    }
                }
            }
        }

        // Hidden CategoryID Save
        function savaHiddenCategoryID(ID) {
            document.getElementById('hdCategoryID').value = ID;
        }

        // Template Add
        function DefaultTemplate(CategoryContents) {
            var ifr = document.getElementById('ifrContents');
            var doc = ifr.contentWindow || ifr.contentDocument;
            doc.oEditors.getById["ir1"].exec("SET_CONTENTS", [""]);
            doc.oEditors.getById["ir1"].exec("PASTE_HTML", [CategoryContents]);
        }

        // Radio Checked
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