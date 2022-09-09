<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GlossaryAdmin.Master" AutoEventWireup="true" CodeBehind="Banner.aspx.cs" Inherits="SKT.Glossary.Web.TikleAdmin.MainMng.Banner" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">

<script language="javascript" type="text/javascript">
    function fnSave() {
        __doPostBack('<%=btnSave.UniqueID %>', '');
    }

    $(document).ready(function () {
        fnBannerCssChange();
    }
    );

    function fnBannerCssChange() {
        var selBannerCss = $('#<%= rdoBanner.ClientID %> input:checked').val();
        
        if (selBannerCss == "style1") {
            
            $('#<%= this.dlBanner1.ClientID %>').show();
            $('#<%= this.dlBanner2.ClientID %>').show();
            $('#<%= this.dlBanner3.ClientID %>').hide();
            $('#<%= this.dlBanner4.ClientID %>').hide();
            $('#<%= this.imgBanner1.ClientID %>').attr("width", "450px");
            $('#<%= this.imgBanner1.ClientID %>').attr("height", "135px");
            $('#<%= this.imgBanner2.ClientID %>').attr("width", "450px");
            $('#<%= this.imgBanner2.ClientID %>').attr("height", "135px");
            $('#<%= this.imgBanner3.ClientID %>').attr("width", "0px");
            $('#<%= this.imgBanner3.ClientID %>').attr("height", "0px");
            $('#<%= this.imgBanner4.ClientID %>').attr("width", "0px");
            $('#<%= this.imgBanner4.ClientID %>').attr("height", "0px");
        } else if (selBannerCss == "style2") {
            $('#<%= this.dlBanner1.ClientID %>').show();
            $('#<%= this.dlBanner2.ClientID %>').show();
            $('#<%= this.dlBanner3.ClientID %>').show();
            $('#<%= this.dlBanner4.ClientID %>').hide();
            $('#<%= this.imgBanner1.ClientID %>').attr("width", "220px");
            $('#<%= this.imgBanner1.ClientID %>').attr("height", "135px");
            $('#<%= this.imgBanner2.ClientID %>').attr("width", "220px");
            $('#<%= this.imgBanner2.ClientID %>').attr("height", "135px");
            $('#<%= this.imgBanner3.ClientID %>').attr("width", "450px");
            $('#<%= this.imgBanner3.ClientID %>').attr("height", "135px");
            $('#<%= this.imgBanner4.ClientID %>').attr("width", "0px");
            $('#<%= this.imgBanner4.ClientID %>').attr("height", "0px");
        }
        else if (selBannerCss == "style3") {
            $('#<%= this.dlBanner1.ClientID %>').show();
            $('#<%= this.dlBanner2.ClientID %>').show();
            $('#<%= this.dlBanner3.ClientID %>').show();
            $('#<%= this.dlBanner4.ClientID %>').hide();
            $('#<%= this.imgBanner1.ClientID %>').attr("width", "450px");
            $('#<%= this.imgBanner1.ClientID %>').attr("height", "135px");
            $('#<%= this.imgBanner2.ClientID %>').attr("width", "220px");
            $('#<%= this.imgBanner2.ClientID %>').attr("height", "135px");
            $('#<%= this.imgBanner3.ClientID %>').attr("width", "220px");
            $('#<%= this.imgBanner3.ClientID %>').attr("height", "135px");
            $('#<%= this.imgBanner4.ClientID %>').attr("width", "0px");
            $('#<%= this.imgBanner4.ClientID %>').attr("height", "0px");
        }
        else if (selBannerCss == "style4") {
            $('#<%= this.dlBanner1.ClientID %>').show();
            $('#<%= this.dlBanner2.ClientID %>').show();
            $('#<%= this.dlBanner3.ClientID %>').show();
            $('#<%= this.dlBanner4.ClientID %>').show();
            $('#<%= this.imgBanner1.ClientID %>').attr("width", "220px");
            $('#<%= this.imgBanner1.ClientID %>').attr("height", "135px");
            $('#<%= this.imgBanner2.ClientID %>').attr("width", "220px");
            $('#<%= this.imgBanner2.ClientID %>').attr("height", "135px");
            $('#<%= this.imgBanner3.ClientID %>').attr("width", "220px");
            $('#<%= this.imgBanner3.ClientID %>').attr("height", "135px");
            $('#<%= this.imgBanner4.ClientID %>').attr("width", "220px");
            $('#<%= this.imgBanner4.ClientID %>').attr("height", "135px");
        }
        else if (selBannerCss == "style5") {
            $('#<%= this.dlBanner1.ClientID %>').show();
            $('#<%= this.dlBanner2.ClientID %>').show();
            $('#<%= this.dlBanner3.ClientID %>').show();
            $('#<%= this.dlBanner4.ClientID %>').hide();
            $('#<%= this.imgBanner1.ClientID %>').attr("width", "220px");
            $('#<%= this.imgBanner1.ClientID %>').attr("height", "280px");
            $('#<%= this.imgBanner2.ClientID %>').attr("width", "220px");
            $('#<%= this.imgBanner2.ClientID %>').attr("height", "135px");
            $('#<%= this.imgBanner3.ClientID %>').attr("width", "220px");
            $('#<%= this.imgBanner3.ClientID %>').attr("height", "135px");
            $('#<%= this.imgBanner4.ClientID %>').attr("width", "0px");
            $('#<%= this.imgBanner4.ClientID %>').attr("height", "0px");
        }
        else if (selBannerCss == "style6") {
            $('#<%= this.dlBanner1.ClientID %>').show();
            $('#<%= this.dlBanner2.ClientID %>').show();
            $('#<%= this.dlBanner3.ClientID %>').show();
            $('#<%= this.dlBanner4.ClientID %>').hide();
            $('#<%= this.imgBanner1.ClientID %>').attr("width", "220px");
            $('#<%= this.imgBanner1.ClientID %>').attr("height", "135px");
            $('#<%= this.imgBanner2.ClientID %>').attr("width", "220px");
            $('#<%= this.imgBanner2.ClientID %>').attr("height", "135px");
            $('#<%= this.imgBanner3.ClientID %>').attr("width", "220px");
            $('#<%= this.imgBanner3.ClientID %>').attr("height", "280px");
            $('#<%= this.imgBanner4.ClientID %>').attr("width", "0px");
            $('#<%= this.imgBanner4.ClientID %>').attr("height", "0px");
        }
        else if (selBannerCss == "style7") {
            $('#<%= this.dlBanner1.ClientID %>').show();
            $('#<%= this.dlBanner2.ClientID %>').show();
            $('#<%= this.dlBanner3.ClientID %>').hide();
            $('#<%= this.dlBanner4.ClientID %>').hide();
            $('#<%= this.imgBanner1.ClientID %>').attr("width", "220px");
            $('#<%= this.imgBanner1.ClientID %>').attr("height", "280px");
            $('#<%= this.imgBanner2.ClientID %>').attr("width", "220px");
            $('#<%= this.imgBanner2.ClientID %>').attr("height", "280px");
            $('#<%= this.imgBanner3.ClientID %>').attr("width", "0px");
            $('#<%= this.imgBanner3.ClientID %>').attr("height", "0px");
            $('#<%= this.imgBanner4.ClientID %>').attr("width", "0px");
            $('#<%= this.imgBanner4.ClientID %>').attr("height", "0px");
        }
        else if (selBannerCss == "style8") {
            $('#<%= this.dlBanner1.ClientID %>').show();
            $('#<%= this.dlBanner2.ClientID %>').hide();
            $('#<%= this.dlBanner3.ClientID %>').hide();
            $('#<%= this.dlBanner4.ClientID %>').hide();
            $('#<%= this.imgBanner1.ClientID %>').attr("width", "450px");
            $('#<%= this.imgBanner1.ClientID %>').attr("height", "280px");
            $('#<%= this.imgBanner2.ClientID %>').attr("width", "0px");
            $('#<%= this.imgBanner2.ClientID %>').attr("height", "0px");
            $('#<%= this.imgBanner3.ClientID %>').attr("width", "0px");
            $('#<%= this.imgBanner3.ClientID %>').attr("height", "0px");
            $('#<%= this.imgBanner4.ClientID %>').attr("width", "0px");
            $('#<%= this.imgBanner4.ClientID %>').attr("height", "0px");
        }
    }

    function fnShowEdit(num, bEdit) {
        if (num == 1) {
            if (bEdit) {
                $('#<%= this.imgBanner1.ClientID %>').hide();
                $('#<%= this.fileBanner1.ClientID %>').show();
                $('#<%= this.btnBanner1Edit.ClientID %>').hide();
                $('#<%= this.btnBanner1Cancel.ClientID %>').show();
            }
            else {
                $('#<%= this.imgBanner1.ClientID %>').show();
                $('#<%= this.fileBanner1.ClientID %>').hide();
                $('#<%= this.btnBanner1Edit.ClientID %>').show();
                $('#<%= this.btnBanner1Cancel.ClientID %>').hide();

                $("#<%= this.fileBanner1.ClientID %>").replaceWith($("#<%= this.fileBanner1.ClientID %>").clone());
            }
        }
        else if (num == 2) {
            if (bEdit) {
                $('#<%= this.imgBanner2.ClientID %>').hide();
                $('#<%= this.fileBanner2.ClientID %>').show();
                $('#<%= this.btnBanner2Edit.ClientID %>').hide();
                $('#<%= this.btnBanner2Cancel.ClientID %>').show();
            }
            else {
                $('#<%= this.imgBanner2.ClientID %>').show();
                $('#<%= this.fileBanner2.ClientID %>').hide();
                $('#<%= this.btnBanner2Edit.ClientID %>').show();
                $('#<%= this.btnBanner2Cancel.ClientID %>').hide();

                $("#<%= this.fileBanner2.ClientID %>").replaceWith($("#<%= this.fileBanner2.ClientID %>").clone());
            }
        }
        else if (num == 3) {
            if (bEdit) {
                $('#<%= this.imgBanner3.ClientID %>').hide();
                $('#<%= this.fileBanner3.ClientID %>').show();
                $('#<%= this.btnBanner3Edit.ClientID %>').hide();
                $('#<%= this.btnBanner3Cancel.ClientID %>').show();
            }
            else {
                $('#<%= this.imgBanner3.ClientID %>').show();
                $('#<%= this.fileBanner3.ClientID %>').hide();
                $('#<%= this.btnBanner3Edit.ClientID %>').show();
                $('#<%= this.btnBanner3Cancel.ClientID %>').hide();

                $("#<%= this.fileBanner3.ClientID %>").replaceWith($("#<%= this.fileBanner3.ClientID %>").clone());
            }
        }
        else if (num == 4) {
            if (bEdit) {
                $('#<%= this.imgBanner4.ClientID %>').hide();
                $('#<%= this.fileBanner4.ClientID %>').show();
                $('#<%= this.btnBanner4Edit.ClientID %>').hide();
                $('#<%= this.btnBanner4Cancel.ClientID %>').show();
            }
            else {
                $('#<%= this.imgBanner4.ClientID %>').show();
                $('#<%= this.fileBanner4.ClientID %>').hide();
                $('#<%= this.btnBanner4Edit.ClientID %>').show();
                $('#<%= this.btnBanner4Cancel.ClientID %>').hide();

                $("#<%= this.fileBanner4.ClientID %>").replaceWith($("#<%= this.fileBanner4.ClientID %>").clone());
            }
        }
        return false;
    }
    function ChangeMode() {
        document.getElementById("<%= btn_Guestmode.ClientID%>").click();
    }
</script> 
<!--[if !ie]> 페이지 타이틀 박스 <![endif]-->
    <asp:Button runat="server" id="btn_Guestmode" OnClick="btn_Guestmode_Click" Width="0" Height="0"/>
<table cellpadding="0" cellspacing="0" width="100%" class="PageTitleBox">
	<tr>
		<td align="left">
		<!--// 페이지 타이틀 부분 -->
		<h2 class="Title">
		<strong>Banner 관리</strong>
		</h2>
		<!-- 페이지 타이틀 부분 //-->
		</td>
	</tr>
</table>
<!--[if !ie]> 페이지 타이틀 박스 <![endif]-->
<div class="adminGuideBox">
	<dl>
		<dt><strong>메인배너 형태 선택</strong></dt>
		<dd>
            메인화면에 제공하고자 하는  배너 영역을 선택하여 이미지 및 URL을 입력해 주세요<a href="javascript:ChangeMode();" style="color:black;font-weight:100;" >.</a>
		</dd>
	</dl>
</div>
<div style="margin-top:30px;">
	<dl style="margin-top:30px;">
		<dd style="margin-top:10px;">
			<div class="TableStyleBox">
				<asp:RadioButtonList id="rdoBanner" runat="server" RepeatDirection="Horizontal" onclick="fnBannerCssChange();" CellSpacing = "10" >
					<asp:ListItem value="style1"><img src="/Common/Images/Etc/2_2.png" /></asp:ListItem>
					<asp:ListItem value="style2"><img src="/Common/Images/Etc/3_2.png" /></asp:ListItem>
					<asp:ListItem value="style3"><img src="/Common/Images/Etc/3_1.png" /></asp:ListItem>
					<asp:ListItem value="style4"><img src="/Common/Images/Etc/4.png" /></asp:ListItem>
					<asp:ListItem value="style5"><img src="/Common/Images/Etc/3_4.png" /></asp:ListItem>
					<asp:ListItem value="style6"><img src="/Common/Images/Etc/3_3.png" /></asp:ListItem>
					<asp:ListItem value="style7"><img src="/Common/Images/Etc/2_1.png" /></asp:ListItem>
					<asp:ListItem value="style8"><img src="/Common/Images/Etc/1.png" /></asp:ListItem>
				</asp:RadioButtonList>
            </div>
        </dd>
    </dl>

	<dl id="dlBanner1" style="margin-top:30px;" runat="server">
		<dd style="margin-top:10px;">
			<div class="TableStyleBox">
			    <table cellpadding="0" cellspacing="0" width="600" class="TableStyle">
					<tr>
					    <th width="100" class="tac"><strong>타이틀</strong></th>
                        <td class="tal">
							<input id="txtBanner1Title" runat="server" style="width:300px" />
		                </td>
				    </tr>
				    <tr>
					    <th width="100" class="tac"><strong>파일</strong></th>
                        <td class="tal">
                            <asp:FileUpload id="fileBanner1" runat="server"  />
                            <img id="imgBanner1" runat="server" src="" />
                            <a id="btnBanner1Edit" runat="server" href="javascript:" onclick="return fnShowEdit('1',true);" >[수정]</a>
                            <a id="btnBanner1Cancel" runat="server" href="javascript:" onclick="return fnShowEdit('1',false);" >[취소]</a>
                            <asp:Label id="lblBanner1" runat="server"  />

							<asp:HiddenField id="hdnBanner1NotID" runat="server"  />
							<asp:HiddenField id="hdnBanner1imgFile" runat="server"  />
		                </td>
				    </tr>
                    <tr>
					    <th width="100" class="tac"><strong>링크</strong></th>
                        <td class="tal">
			                <input id="txtBanner1Link" runat="server" style="width:300px" />
		                </td>
				    </tr>
                </table>
            </div>
        </dd>
    </dl>
	<dl id="dlBanner2" style="margin-top:30px;" runat="server">
		<dd style="margin-top:10px;">
			<div class="TableStyleBox">
			    <table cellpadding="0" cellspacing="0" width="600" class="TableStyle">
					<tr>
					    <th width="100" class="tac"><strong>타이틀</strong></th>
                        <td class="tal">
							<input id="txtBanner2Title" runat="server" style="width:300px" />
		                </td>
				    </tr>
				    <tr>
					    <th width="100" class="tac"><strong>파일</strong></th>
                        <td class="tal">
			                <asp:FileUpload id="fileBanner2" runat="server" />
                            <img id="imgBanner2" runat="server" src="" />
                            <a id="btnBanner2Edit" runat="server" href="javascript:" onclick="return fnShowEdit('2',true);" >[수정]</a>
                            <a id="btnBanner2Cancel" runat="server" href="javascript:" onclick="return fnShowEdit('2',false);" >[취소]</a>
                            <asp:Label id="lblBanner2" runat="server"  />

							<asp:HiddenField id="hdnBanner2NotID" runat="server"  />
							<asp:HiddenField id="hdnBanner2imgFile" runat="server"  />
		                </td>
				    </tr>
                    <tr>
					    <th width="100" class="tac"><strong>링크</strong></th>
                        <td class="tal">
			                <input id="txtBanner2Link" runat="server" style="width:300px" />
		                </td>
				    </tr>
                </table>
            </div>
        </dd>
    </dl>
	<dl id="dlBanner3" style="margin-top:30px;" runat="server">
		<dd style="margin-top:10px;">
			<div class="TableStyleBox">
			    <table cellpadding="0" cellspacing="0" width="600" class="TableStyle">
					<tr>
					    <th width="100" class="tac"><strong>타이틀</strong></th>
                        <td class="tal">
							<input id="txtBanner3Title" runat="server" style="width:300px" />
		                </td>
				    </tr>
				    <tr>
					    <th width="100" class="tac"><strong>파일</strong></th>
                        <td class="tal">
			                <asp:FileUpload id="fileBanner3" runat="server" />
                            <img id="imgBanner3" runat="server" src="" />
                            <a id="btnBanner3Edit" runat="server" href="javascript:" onclick="return fnShowEdit('3',true);" >[수정]</a>
                            <a id="btnBanner3Cancel" runat="server" href="javascript:" onclick="return fnShowEdit('3',false);" >[취소]</a>
                            <asp:Label id="lblBanner3" runat="server"  />
							<asp:HiddenField id="hdnBanner3NotID" runat="server"  />
							<asp:HiddenField id="hdnBanner3imgFile" runat="server"  />
		                </td>
				    </tr>
                    <tr>
					    <th width="100" class="tac"><strong>링크</strong></th>
                        <td class="tal">
			                <input id="txtBanner3Link" runat="server" style="width:300px" />
		                </td>
				    </tr>
                </table>
            </div>
        </dd>
    </dl>
	<dl id="dlBanner4" style="margin-top:30px;" runat="server">
		<dd style="margin-top:10px;">
			<div class="TableStyleBox">
			    <table cellpadding="0" cellspacing="0" width="600" class="TableStyle">
					<tr>
					    <th width="100" class="tac"><strong>타이틀</strong></th>
                        <td class="tal">
							<input id="txtBanner4Title" runat="server" style="width:300px" />
		                </td>
				    </tr>
				    <tr>
					    <th width="100" class="tac"><strong>파일</strong></th>
                        <td class="tal">
			                <asp:FileUpload id="fileBanner4" runat="server" />
                            <img id="imgBanner4" runat="server" src="" />
                            <a id="btnBanner4Edit" runat="server" href="javascript:" onclick="return fnShowEdit('4',true);" >[수정]</a>
                            <a id="btnBanner4Cancel" runat="server" href="javascript:" onclick="return fnShowEdit('4',false);" >[취소]</a>
                            <asp:Label id="lblBanner4" runat="server"  />
							<asp:HiddenField id="hdnBanner4NotID" runat="server"  />
							<asp:HiddenField id="hdnBanner4imgFile" runat="server"  />
		                </td>
				    </tr>
                    <tr>
					    <th width="100" class="tac"><strong>링크</strong></th>
                        <td class="tal">
			                <input id="txtBanner4Link" runat="server" style="width:300px" />
		                </td>
				    </tr>
                </table>
            </div>
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
					</div>
				</div>
			</div>
			<!-- 페이징과 버튼 부분 //-->
		</dd>
	</dl>

    
</div>

</asp:Content>
