<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GatheringPermission.ascx.cs" Inherits="SKT.Glossary.Web.Common.Controls.GatheringPermission" %>

<script src="../Common/Js/json2.js" type="text/javascript"></script>
<script type="text/javascript" src="/OrgChart/common/orgchart.js"></script>
<%--<script type="text/javascript" src="http://tikle.sktelecom.com/OrgChart/common/orgchart.js"></script>--%>

<script type="text/javascript">
    var g_ShareSearchTemp = null;
    var g_ItemID = '<%= ItemID %>';
	var g_UserID = '<%= UserID %>';
    var g_selectID = "<%=ddlUserGroup.ClientID%>";

    // 최초 조직도 클릭시 페이지 나감 alert 창 제거
    onBeforeUnloadFired = true;

    $(document).ready(function () {
        //자동검색 기능
        $("#txtUser").autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "/Common/Controls/AjaxControl.aspx/GetAutoCompleteUserData",
                    data: "{'username':'" + $('#txtUser').val() + "'}",
                    dataType: "json",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                label: item.split('@')[0],
                                val: item.split('@')[1]
                            }
                        }))

                        $('.ui-autocomplete').css("z-index", "1000");
                    },
                    error: function (result) {
                        alert("Error");
                    }
                });
            },
            select: function (e, i) {
                // 대상자 목록에 추가
                pushToArraySave(i.item.label + "/" + i.item.val, i.item.val, 'U');
                $('#user').val("");
            },
            selectFirst: true,
            minLength: 2,
            autoFocus: true
        });

        //엔터를 첫을 경우 공유 대상자 추가
        $('#txtUser').keydown(function (e) {
            if (e.keyCode == 13) {

                var SearchTxt = $('#txtUser').val();
                if (SearchTxt.replace(/^\s+|\s+$/g, '') != "") {

                    if (SearchTxt.indexOf("/") != -1) {

                        g_ShareSearchTemp = $('#txtUser').val();
                        // 대상자 목록에 추가
                        //pushToArraySave(SearchTxt, obj[SearchTxt], 'U');
                    }
                    // debugger;
                    $('#txtUser').val("");
                }

            }

        });


        //클릭을 하였을 경우 대상자 추가
        $('.ui-autocomplete').click(function (e) {
            //var SearchTxt = $('#txtUser').val();

            //if (SearchTxt.replace(/^\s+|\s+$/g, '') != "" && SearchTxt.indexOf("/") != -1) {
            //    g_ShareSearchTemp = $('#txtUser').val();

            //    // 대상자 목록에 추가
            //    pushToArraySave(SearchTxt, obj[SearchTxt], 'U');

                $('#txtUser').val("");
            //}
        });

        // 사용자 그룹을 선택했을 경우 그룹 추가
        $("select#" + g_selectID).change(OnSelectGroup);
    });


    var g_obj = {};
    var g_objj = {};
    var g_objt = {};
    //공유 검색에서 사용자 배열 저장(자동완성목록)
    function pushToArray(name, val) {
        g_obj[name] = val;
    }

    //공유 검색에서 사용자 선택시 배열에 저장 
    function pushToArraySave(name, val, utype) {
        if (g_objj[name] == undefined) {

            var NameSet = "";
            /*
                Author : 개발자-최현미C, 리뷰자-진현빈D
                Create Date : 2016.06.01 
                Desc : 본인인 생성한 모임 삭제 금지
            */

            if ("<%=boolCheckSelf%>" == "True") {
                if (val == "<%=UserID%>")
                    NameSet = "<li>" + name + "</li> ";
                else
                    NameSet = "<li><a href=\"javascript:\" onclick=\"fnUserDelete(this,'" + name + "');\">" + name + "</a></li> ";
            }
            else
            {
                NameSet = "<li><a href=\"javascript:\" onclick=\"fnUserDelete(this,'" + name + "');\">" + name + "</a></li> ";
            }

            $("#CommonList").prepend(NameSet);
            $("#CommonList").show();

            g_objj[name] = val;
            g_objt[name] = utype;
        }
        else {
            alert('이미 추가 되어 있습니다.');
        }
    }

    //선택된 공유 사용자 제거
    function fnUserDelete(obj, val) {
        delete g_objj[val];
        delete g_objt[val];
        $(obj).parent().remove();

        if ($("#CommonList li").length > 0) {
            $("#CommonList").show();
        }
        else {
            $("#CommonList").hide();
        }
    }

    //글작성 완료 버튼시 
    function fnShareListSave() {

        var UserIDData = null;
        var UserTypeData = null;
        var UserNameData = null;

        for (var i = 0; i < $("#CommonList li").length; i++) {
            var tempID = g_objj[$("#CommonList li")[i].innerText.replace('X', "")];
            var tempType = g_objt[$("#CommonList li")[i].innerText.replace('X', "")];

            if (tempID != "") {
                if (UserIDData == null) {
                    UserIDData = tempID + "/";
                    UserTypeData = tempType + "/";
                    UserNameData = $("#CommonList li")[i].innerText + "&";
                } else {
                    UserIDData += tempID + "/";
                    UserTypeData += tempType + "/";
                    UserNameData += $("#CommonList li")[i].innerText + "&";
                }
            }
        }
        // 공유사용자 ID, 이름
        if (UserIDData != null) {
            savehidUserItemID(UserIDData, UserNameData, UserTypeData);
        }
        else {
            //alert('공유할 사용자를 선택하세요');
        }
    }

    // hidden값 셋팅
    function savehidUserItemID(UserIDData, UserNameData, UserTypeData) {
        $('#<%= this.hidUserItemID.ClientID %>').val("");
	    $('#<%= this.hidUserItemID.ClientID %>').val(UserIDData);
	    $('#<%= this.hidUserName.ClientID %>').val("");
	    $('#<%= this.hidUserName.ClientID %>').val(UserNameData);
	    $('#<%= this.hidUserType.ClientID %>').val("");
	    $('#<%= this.hidUserType.ClientID %>').val(UserTypeData);
	}

	function gethidItemValue(ck) {
	    var rtnItemValue = "";

	    if (ck == "ID") {
	        rtnItemValue = $('#<%= this.hidUserItemID.ClientID %>').val();
        } else if (ck == "NM") {
            rtnItemValue = $('#<%= this.hidUserName.ClientID %>').val();
        } else if (ck == "TY") {
            rtnItemValue = $('#<%= this.hidUserType.ClientID %>').val();
        }

	    return rtnItemValue;
    }

    function fnUserFocusMove() {
        $("#txtUser").focus();
        $("#txtUser").val("");
    }

    //공유 닫기
    function fnShareClose() {
        $(".view-func-share-outer").hide();
        ShareOpen = true;
    }

    // 편집화면에서 일부공개 일 경우 list 
    function DefaultListSetting(str) {
        var JsonData = JSON.parse(str);
        for (var i = 0; i < JsonData.length; i++) {
            // 대상자 목록에 추가
            pushToArraySave(JsonData[i].ToUserName, JsonData[i].ToUserID, JsonData[i].ToUserType);
        }
        fnShareListSave();
    }

    //사원값 초기화
    function DefaultListClear() {
        $("#CommonList").html("");
        g_obj = {};
        g_objj = {};
        g_objt = {};

        $("#CommonList").hide();
    }

    function fnCheckhidVal() {
        var rtn = false;
        if ($('#<%= this.hidUserItemID.ClientID %>').val() == "") {
	            rtn = true;
	        }
	        return rtn;
    }

    // 사원찾기(조직도)
    function fn_SearchUsers() {
        function callback(data) {
            var orgchartObj = {};
            if (typeof (data) == "string" && data.length > 0) {
                orgchartObj = eval(data);

                if (orgchartObj) {
                    for (var i = 0; i < orgchartObj.length; i++) {
                        if (orgchartObj[i].EmpID) {
                            // 사원
                            OrgChartUsers(orgchartObj[i].UserName, orgchartObj[i].DeptName, orgchartObj[i].EmpID);
                        } else {
                            //부서
                            OrgChartDepts(orgchartObj[i].DeptName, orgchartObj[i].DeptCode);
                        }
                    }
                }
            }
        }
        om_OpenOrgChart({
            callback: callback,
            appType: 'DeptUser',
            oneSelect: false
        });
    }

    // OrgChart로 사원입력
    function OrgChartUsers(UserName, DeptName, EmpID) {
        g_ShareSearchTemp = $('#txtUser').val();

        // 대상자 목록에 추가
        pushToArraySave(UserName + "/" + DeptName, EmpID, 'U');
    }

    // OrgChart로 부서입력
    function OrgChartDepts(DeptName, DeptCode) {
        g_ShareSearchTemp = $('#txtUser').val();

        // 대상자 목록에 추가
        pushToArraySave(DeptName, DeptCode, 'O');
    }

    // 사용자 그룹 선택
    function OnSelectGroup() {
        var obj = $("select#" + g_selectID + " option");

        var grpNM = obj.filter(":selected").text();
        var grpID = obj.filter(":selected").val();

        if (grpID != "0" && grpID != null && grpID != undefined) {
            pushToArraySave(grpNM, grpID, 'G');

            obj.eq(0).attr('selected', 'selected');
        }
    }

    // 게시글 조회/수정 시 권한 목록 조회
    function fnSelectMyAuth(stype, id, uid) {

        try {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Common/Controls/AjaxControl.aspx/GetMyGroupList",
                data: "{Type : '" + stype + "', UserID : '" + uid + "', MyGrpID : '" + id + "'}",
                dataType: "json",
                async: false,
                success: function (data) {

                    if (data.d[0] != null && data.d[0].length != 0) {
                        //alert("[" + data.d + "]");
                        DefaultListClear();
                        DefaultListSetting("[" + data.d + "]");
                    }

                },
                error: function (result) {
                    alert("Error DB Check");
                }
            });
        }
        catch (exception) {
            alert('Exception error' + exception.toString());
        }
    }
	</script>


<div id="dvUserAndDepartment">
<% if (bGrp) { %>
    <p id="pSelectUserGroup">
        <asp:DropDownList ID="ddlUserGroup" Width="266px" CssClass="select" runat="server" />
    </p>
<% } %>
	<p>
		<input type="text" id="txtUser"  name="" class="txt t3" value="  이름으로 검색"  onclick="fnUserFocusMove();"/> 
		<a href="javascript:fn_SearchUsers();" class="btn4"><b>조직도</b></a>
	</p>
	<ul id="CommonList" style="display:none; text-align:left;">

	</ul>
</div>
<!--/지식_조회 권한 설정하기-->
<asp:HiddenField ID="hidUserItemID" runat="server" />
<asp:HiddenField ID="hidUserName" runat="server" />
<asp:HiddenField ID="hidUserType" runat="server" />