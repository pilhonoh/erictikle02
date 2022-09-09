<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserAndDepartmentList.ascx.cs" Inherits="SKT.Glossary.Web.Common.Controls.UserAndDepartmentList" %>

<script src="../Common/Js/json2.js" type="text/javascript"></script>
<script type="text/javascript" src="/OrgChart/common/orgchart.js"></script>
<script type="text/javascript" language="javascript">
    var ShareSearchTemp = null;
    var m_ItemID = '<%= ItemID %>';
	var m_UserID = '<%= UserID %>';
    var m_selectID = "<%=ddlUserGroup.ClientID%>";

    // 최초 조직도 클릭시 페이지 나감 alert 창 제거
    onBeforeUnloadFired = true;

    $(document).ready(function () {
        //자동겁색 기능
        $("#user").autocomplete({
            source: function (request, response) {
                //var kw = new String($('#user').val());
                //if (kw.trim() == "이름으로 검색") return;
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "/Common/Controls/AjaxControl.aspx/GetAutoCompleteUserData",
                    data: "{'username':'" + $('#user').val() + "'}",
                    dataType: "json",
                    success: function (data) {
                        ////alert(data.d);
                        //for (var i = 0; i < data.d.length; i++) {
                        //	pushToAry(data.d[i], data.d[i + 1]);
                        //	i += 1;
                        //}
                        //for (var i = 0; i < data.d.length; i++) {
                        //    if (data.d[i].indexOf("/") == -1) {
                        //		data.d.splice(i, 1);
                        //		i = 0;
                        //   } 
                        //}

                        //response(data.d);

                        //$('.ui-autocomplete').css("z-index" , "1000");
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
                //alert("select------>" + i.item.val);
                //alert(i.item.val);

                // 대상자 목록에 추가
                pushToArySave(i.item.label + "/" + i.item.val, i.item.val, 'U');
                $('#user').val("");
            },
            focus: function (event, ui) {
                //this.value = ui.item.label;
                event.preventDefault();
            },

            selectFirst: true,
            // 자동완성 실행을 위한 최소 검색어 자리 수
            minLength: 2,
            autoFocus: true
        });

        //엔터를 첫을 경우 공유 대상자 추가
        $('#user').keydown(function (e) {
            if (e.keyCode == 13 ) {
                
                var SearchTxt = $('#user').val();
                if (SearchTxt.replace(/^\s+|\s+$/g, '') != "") {

                    if (SearchTxt.indexOf("/") != -1) {

                        ShareSearchTemp = $('#user').val();
                        // 대상자 목록에 추가
                        //20150329 위랑 중복 추가됨
                        //pushToArySave(SearchTxt, obj[SearchTxt], 'U');
                    }
                    // debugger;
                    $('#user').val("");
                }

            }

        });


        //클릭을 하였을 경우 대상자 추가
        $('.ui-autocomplete').click(function (e, ui) {
            //    alert(ui.item.EmpID);
            //	var SearchTxt = $('#user').val();

            //	if (SearchTxt.replace(/^\s+|\s+$/g, '') != "" && SearchTxt.indexOf("/") != -1) {
            //		ShareSearchTemp = $('#user').val();


            //	    // 대상자 목록에 추가
            //		pushToArySave(SearchTxt, obj[SearchTxt], 'U');



            $('#user').val("");

            //	}
        });

        // 사용자 그룹을 선택했을 경우 그룹 추가
        $("select#" + m_selectID).change(OnSelectMyGroup);
    });


    var obj = {};
    var objj = {};
    var objt = {};
    //공유 검색에서 사용자 배열 저장(자동완성목록)
    function pushToAry(name, val) {
        obj[name] = val;

    }

    //공유 검색에서 사용자 선택시 배열에 저장 
    function pushToArySave(name, val, utype) {
        //debugger;
        //alert(name + "/" + val + "/" + utype);
        if (objj[name] == undefined) {

            var objcut = name.split('/');
            if (utype == "U") {
                //alert("TYPE U : " + name + " ★ " + val + " ★ " + utype);

                var NameSet = "";
                /*
                Author : 개발자-최현미C, 리뷰자-진현빈D
                Create Date : 2016.06.01 
                Desc : 본인인 생성한 모임 삭제 금지
                */
                if ("<%=boolCheckSelf%>" == "True") {
                    //var author = $("#ctl00_ContentPlaceHolder_Common_Footer_hdGatheringAuther").val();

                    var author = "";
                    if (parent.$("#ctl00_ContentPlaceHolder_Common_Footer_hdGatheringAuther").val() == undefined) {
                        author = parent.$("#ctl00_MainContent_GatheringInfomation1_hdGatheringAuther").val();
                    } else {
                        author = parent.$("#ctl00_ContentPlaceHolder_Common_Footer_hdGatheringAuther").val();
                    }                    

                    if (val == author)
	                    NameSet = "<li>" + objcut[0] + "/" + objcut[1] + "</li> ";
	                else
	                    NameSet = "<li><a href=\"javascript:\" onclick=\"fnDelete(this,'" + name + "');\">" + objcut[0] + "/" + objcut[1] + "</a></li> ";
	            }
	            else {
	                NameSet = "<li><a href=\"javascript:\" onclick=\"fnDelete(this,'" + name + "');\">" + objcut[0] + "/" + objcut[1] + "</a></li> ";
	            }

            } else {
	            //alert("TYPE UX : " + name + " ★ " + val + " ★ " + utype);
                var NameSet = "<li><a href=\"javascript:\" onclick=\"fnDelete(this,'" + name + "');\">" + objcut[0] + "</a></li> ";

            }
           
            $("#CommonUserList").prepend(NameSet);
            $("#CommonUserList").show();

            objj[name] = val;
            objt[name] = utype;
        }
        else {
            alert('이미 추가 되어 있습니다.');
        }
    }

    //선택된 공유 사용자 제거
    function fnDelete(obj, val) {

        $(window).off('beforeunload');
        delete objj[val];
        delete objt[val];
        
        $(obj).parent().remove();

        if ($("#CommonUserList li").length > 0) {
            $("#CommonUserList").show();
        }
        else {
            //$("#CommonUserList").hide();
        }
    }

    //글작성 완료 버튼시 
    function fnShareSave() {
        var UserIDData = "";
        var UserTypeData = "";
        var UserNameData = "";

        //for (var i = 0; i < $("#CommonUserList li").length; i++) {
        //	var tempID = objj[$("#CommonUserList li")[i].innerText.replace('X', "")];
        //    var tempType = objt[$("#CommonUserList li")[i].innerText.replace('X', "")];

        //	if (tempID != "") {
        //		if (UserIDData == null) {
        //			UserIDData = tempID + "/";
        //			UserTypeData = tempType + "/";
        //			UserNameData = $("#CommonUserList li")[i].innerText + "&";
        //		} else {
        //			UserIDData += tempID + "/";
        //			UserTypeData += tempType + "/";
        //			UserNameData += $("#CommonUserList li")[i].innerText + "&";
        //		}
        //	}
        //}
        //// 공유사용자 ID, 이름
        //if (UserIDData != null) {
        //	saveUserItemID(UserIDData, UserNameData, UserTypeData);
        //}
        //else {
        //	//alert('공유할 사용자를 선택하세요');
        //}


        var objTmp = [];

        if (!isEmptyObj(objj)) {
            for (var key in objj) {
                if (objj.hasOwnProperty(key)) {
                    objTmp.push(key);
                }
            }
            //debugger;
            objTmp.sort();
            for (i in objTmp) {

                var key = objTmp[i];
                var value = objj[objTmp[i]];

                if (objj.hasOwnProperty(key)) {

                    //alert("key=" + key + ", value = " + value);
                    var nameCut = key.split("/");

                    UserIDData += value + "/";
                    UserNameData += nameCut[0] + "/" + nameCut[1] + "&";
                    if (value.length == 8) {
                        UserTypeData += "O/";
                    } else if (value.length == 5) {
                        UserTypeData += "G/";
                    } else {
                        if (!isEmptyObj(objt)) {
                            if (objt[objTmp[i]] == "M") {
                                UserTypeData += "M/";
                            } else {
                                UserTypeData += "U/";
                            }
                        }
                    }
                }
            }
            //alert("id : " + UserIDData + "\n" + "Name : " + UserNameData + "\n : " + "Type" + UserTypeData);


            // 공유사용자 ID, 이름
            if (UserIDData != "") {
                saveUserItemID(UserIDData, UserNameData, UserTypeData);
            }
            else {
                //alert('공유할 사용자를 선택하세요');
            }
        } else {
            saveUserItemID("", "", "");
            //alert('공유할 사용자를 선택하세요');
        }
    }

    function isEmptyObj(obj) {
        for (var key in obj) return false; return true;
    }
    // hidden값 셋팅
    function saveUserItemID(UserIDData, UserNameData, UserTypeData) {
        $('#<%= this.hdUserItemID.ClientID %>').val("");
	    $('#<%= this.hdUserItemID.ClientID %>').val(UserIDData);
	    $('#<%= this.hdUserName.ClientID %>').val("");
	    $('#<%= this.hdUserName.ClientID %>').val(UserNameData);
	    $('#<%= this.hdUserType.ClientID %>').val("");
	    $('#<%= this.hdUserType.ClientID %>').val(UserTypeData);
	}

	function getItemValue(ck) {
	    var rtnItemValue = "";
	    if (ck == "ID") {
	        rtnItemValue = $('#<%= this.hdUserItemID.ClientID %>').val();
        } else if (ck == "NM") {
            rtnItemValue = $('#<%= this.hdUserName.ClientID %>').val();
        } else if (ck == "TY") {
            rtnItemValue = $('#<%= this.hdUserType.ClientID %>').val();
        }

    return rtnItemValue;
}

function fnfocusMove() {
    $("#user").focus();
    $("#user").val("");
}

//공유 닫기
function fnShareClose() {
    $(".view-func-share-outer").hide();
    ShareOpen = true;
}

// 편집화면에서 일부공개 일 경우 list 
function DefaultSetting(str) {
    var JsonData = JSON.parse(str);
    for (var i = 0; i < JsonData.length; i++) {

        // 대상자 목록에 추가
        pushToArySave(JsonData[i].ToUserName + "/" + JsonData[i].ToUserID, JsonData[i].ToUserID, JsonData[i].ToUserType);
    }
    fnShareSave();
}

//사원값 초기화
function DefaultClear() {
    $("#CommonUserList").html("");
    obj = {};
    objj = {};
    objt = {};

    $("#CommonUserList").hide();
}

function fnCheckValue() {
    var rtn = false;
    
    if ($('#<%= this.hdUserItemID.ClientID %>').val() == "") {
	    rtn = true;
    }

	return rtn;
 }

    // 사원찾기(조직도)
    function fn_SearchUser() {
        function callback(data) {
            var orgchartObj = {};
            if (typeof (data) == "string" && data.length > 0) {
                orgchartObj = eval(data);

                if (orgchartObj) {
                    for (var i = 0; i < orgchartObj.length; i++) {
                        if (orgchartObj[i].EmpID) {
                            // 사원
                            OrgChartUser(orgchartObj[i].UserName, orgchartObj[i].DeptName, orgchartObj[i].EmpID);
                        } else {
                            //부서
                            OrgChartDept(orgchartObj[i].DeptName, orgchartObj[i].DeptCode);
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

    // OrgChart 로 사원입력
    function OrgChartUser(UserName, DeptName, EmpID) {
        ShareSearchTemp = $('#user').val();

        // 대상자 목록에 추가 김성환20150121
        pushToArySave(UserName + "/" + DeptName + "/" + EmpID, EmpID, 'U');
    }

    // OrgChar로 부서입력
    function OrgChartDept(DeptName, DeptCode) {
        ShareSearchTemp = $('#user').val();

        // 대상자 목록에 추가
        pushToArySave(DeptName, DeptCode, 'O');
    }

    // 사용자 그룹 선택
    function OnSelectMyGroup() {
        var obj = $("select#" + m_selectID + " option");

        var grpNM = obj.filter(":selected").text();
        var grpID = obj.filter(":selected").val();

        if (grpID != "0" && grpID != null && grpID != undefined) {
            pushToArySave(grpNM, grpID, 'G');

            obj.eq(0).attr('selected', 'selected');
        }
    }

    // 게시글 조회/수정 시 권한 목록 조회
    function fnSelectAuth(stype, id, uid) {

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
                        DefaultClear();
                        DefaultSetting("[" + data.d + "]");
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


<div id="UserAndDepartment">

    <%if (viewType == "weekly" || viewType == "WriteNew" ) {%>

    <table style="width:100%; border-spacing:0;border-collapse:collapse;line-height:1.0em; border:1px solid #ddd">
        <tr>
            <th style="width:50%; vertical-align:top;text-align:center; padding:0 5px 0 0;">
              
    <%} %>
            <% if (bGrp) { %>
                <p id="SelectUserGroup">
                <asp:DropDownList ID="ddlUserGroup" Width="266px" CssClass="select" runat="server" />
                </p>
            <% } %>
	        <p style="padding-bottom:10px">
		        <input type="text" id="user"  name="" class="txt t3" value="  이름으로 검색 하세요."  onclick="fnfocusMove();"/>         
		        <a href="javascript:fn_SearchUser();" class="btn4"><b>조직도</b></a>
	        </p>

    <%if (viewType == "weekly" || viewType == "WriteNew")
      {%>
        </th>
        <th style="width:50%; vertical-align:top;text-align:left; padding : 0 0 10px 0;">
    <%} %>

    
	<ul id="CommonUserList" style="display:block;min-height:80px;text-align:left;" ></ul>

    <%if (viewType == "weekly" || viewType == "WriteNew")
      {%>
            </th>
            </tr>
    </table>
    <%} %>        
     
    <%--2016.03.17 백충기 원본 주석처리--%>
	<%--<p>
		<input type="text" id="user"  name="" class="txt t3" value="  이름으로 검색"  onclick="fnfocusMove();"/>         
		<a href="javascript:fn_SearchUser();" class="btn4"><b>조직도</b></a>
	</p>
	<ul id="CommonUserList" style="display:none">

	</ul>--%>


</div>
<!--/지식_조회 권한 설정하기-->
<asp:HiddenField ID="hdUserItemID" runat="server" />
<asp:HiddenField ID="hdUserName" runat="server" />
<asp:HiddenField ID="hdUserType" runat="server" />