<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GatheringInfomation.ascx.cs" Inherits="SKT.Glossary.Web.Common.Controls.GatheringInfomation" %>

<%--<%@ Register Src="~/Common/Controls/GatheringPermission.ascx" TagName="GatheringPermission" TagPrefix="common" %>--%>
<%@ Register Src="~/Common/Controls/UserAndDepartmentList.ascx" TagPrefix="common" TagName="UserAndDepartment" %>


<script type="text/javascript">
    $(document).ready(function () {
        /*
        Style
        */
        $("#contents .h2tag").css("padding-top", "50px");
        $(".top-menu").css({ "border": "none", "height": "100%" });
        $(".search_top").css({ "left": "400px", "top": "130px" });
        $(".setting-btn").css("background-image", "url('/common/images/btn/setting_btn2.png')");
        if ("<%=this.GI_MenuType%>" == "Directory") {
            $("#contents .h2tag").css({ "padding-top": "50px", "padding-bottom": "10px" });
            $(".user-info").css("padding-top", "25px");
        }

        //$("div.pop").hide();
        //$("#pop_dc_gatheringlist").hide();
        var txtGN = OntextCheck($("#<%=this.pop_GatheringList.ClientID%> option:selected").text());
        $("#txtGatheringName").html(txtGN);

        $("#<%= pop_GatheringList.ClientID %>").change(function () {
            //hideGIPop('pop_dc_gatheringlist');
            //var GatheringID = $(this).find(":selected").val();
            //location.href = "/Glossary/GlossaryNewsList.aspx?TagTitle=&SearchSort=CreateDate&GatheringYN=Y&GatheringID=" + GatheringID;
        });

        if ("<%=this.AuthorYN%>" == "Y") {
            $("#btnSetting").show();
        }

        if ("<%=this.GI_GatheringYN%>" == "Y") {
            lnbDepth = 6;
            $("#gnb li a").removeClass("active");
            var lnbCtrl = $("#gnb>li:nth-child(" + (lnbDepth) + ") a");
            if (lnbCtrl) lnbCtrl.addClass("active");
        }

    });
   
    

    function hideGIPop(pid) {
        $("div.pop").hide();
        $("#" + pid).hide();
        return false;
    }

    function OpenDDL() {
        $("div.pop").show();
        $("#pop_dc_gatheringlist").show();
        return false;
    }

    function fnGI_Move() {
        hideGIPop('pop_dc_gatheringlist');
        var GatheringID = $("#<%=this.pop_GatheringList.ClientID%>").find(":selected").val();
        //location.href = "/Glossary/GlossaryNewsList.aspx?TagTitle=&SearchSort=CreateDate&GatheringYN=Y&GatheringID=" + GatheringID;
        location.href = "/Gathering/GatheringMain.aspx?TagTitle=&SearchSort=CreateDate&GatheringYN=Y&GatheringID=" + GatheringID;
    }


    function OntextCheck(obj) {
        var nbytes = 0;
        for (i = 0; i < obj.length; i++) {
            var ch = obj.charAt(i);
            if (escape(ch).length > 4) {
                nbytes += 2;
            } else if (ch != '\r') {
                nbytes++;
            }
        }
        if (nbytes > 20) {
            obj = obj.substr(0, 12) + "...";
        } else {
            obj = obj;
        }
        return obj;
    }



</script>
<%--<a href="/Gathering/Main.aspx"><img src="/common/images/text/Gathering_text_5.png" alt="끌.모임" style="left: 50px; top: 78px; position: absolute; width: 83px; height: 26px;" /></a>--%>
<%--<a href="javascript:;" onclick="javascript:return GI_viewDivShow();" class="setting-btn" id="btnSetting" style="display:none; left: 130px; top: 78px; position: absolute;"></a>--%>
<span id="txtGatheringName" style="text-align:center; border:none; background-color:transparent; color:white; font-family:'맑은고딕','Malgun Gothic','nanumgothic','나눔고딕','나눔고딕',dotum,applegothic,sans-serif; font-size:50px; font-weight:bold;"></span>
<a href="javascript:;" onclick="return OpenDDL();" ><img src="/Common/images/btn/select3.png" style="margin-left:10px;" /></a>
<%--<asp:DropDownList ID="GI_GatheringDDL" style="display:block; text-align:center; border:none; background-color:transparent; color:white; font-family:'맑은고딕','Malgun Gothic','nanumgothic','나눔고딕','나눔고딕',dotum,applegothic,sans-serif; font-size:50px; font-weight:bold;" AutoPostBack="false" runat="server" OnSelectedIndexChanged="GI_GatheringDDL_SelectedIndexChanged"></asp:DropDownList>--%>
<br />
<p class="user-info" style="padding-top:15px; background-color:transparent; text-align:right;">
    
</p>
<!-- GatheringInfo Control Area -->
<div id="GatheringPop" class="pop" style="display:none;">
    <div class="popBg"></div>
    <!--모임 관리-->
    
    <!--//모임 관리-->
    <!--모임 목록-->
    <div id="pop_dc_gatheringlist" class="layer_pop" style="display:none;">
        <h3>이동할 모임을 선택해 주세요</h3>
        <div class="addWrap">            
            <fieldset class="authority">                
                <asp:ListBox ID="pop_GatheringList" runat="server" style="width:85%;" Rows="10"></asp:ListBox>
            </fieldset>
        </div>
        <p class="btn_c">
            <a href="javascript:;" onclick="javascript:return hideGIPop('pop_dc_gatheringlist');" class="btn2"><b>취소하기</b></a>
            <a href="javascript:fnGI_Move();" class="btn3"><b>이동하기</b></a>
        </p>
        <a href="javascript:;">
            <img src="/common/images/btn/pop_close.png" alt="닫기" class="close" onclick="javascript:return hideGIPop('pop_dc_gatheringlist');" /></a>
	</div>
    <!--//모임 목록-->
    <asp:HiddenField ID="hdTitlebefore" runat="server" />
    <asp:HiddenField ID="hdGatheringAuther" runat="server" />
</div> <!--//모임 목록1-->
<!-- //Gathering Info Area-->