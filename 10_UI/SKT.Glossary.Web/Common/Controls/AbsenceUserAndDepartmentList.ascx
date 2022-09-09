<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AbsenceUserAndDepartmentList.ascx.cs" Inherits="SKT.Glossary.Web.Common.Controls.AbsenceUserAndDepartmentList" %>

<script src="../Common/Js/json2.js" type="text/javascript"></script>
<script type="text/javascript" src="/OrgChart/common/orgchart.js"></script>
<script type="text/javascript" language="javascript">
	var ShareSearchTemp = null;
	var m_ItemID = '<%= ItemID %>';	
    var m_UserID = '<%= UserID %>'; 
	var m_selectID = "<%=ddlGroupUser.ClientID%>";

	// 최초 조직도 클릭시 페이지 나감 alert 창 제거
	onBeforeUnloadFired = true;

	$(document).ready(function () {
		//자동겁색 기능
		$("#userName").autocomplete({
		    source: function (request, response) {

				$.ajax({
					type: "POST",
					contentType: "application/json; charset=utf-8",
					url: "/Common/Controls/AjaxControl.aspx/GetAutoCompleteUserData",
					data: "{'username':'" + $('#userName').val() + "'}",
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
					error: function(result) {
					    alert("Error");
					}
				});
		    },
		    select: function (e, i) {
		        //alert(i.item.label + "/" + i.item.val);
		        // 대상자 목록에 추가
		        pushToArySave_Absence(i.item.label + "/" + i.item.val, i.item.val, 'U');
		        $('#userName').val("");
            },         

					
			selectFirst: true,
			minLength: 2,
			autoFocus: true
		});

		//엔터를 첫을 경우 공유 대상자 추가
		$('#userName').keydown(function (e) {
		    if (e.keyCode == 13) {
		        
		        var SearchTxt = $('#userName').val();
				if (SearchTxt.replace(/^\s+|\s+$/g, '') != "") {

				    if (SearchTxt.indexOf("/") != -1) {
				       
				        ShareSearchTemp = $('#userName').val();
				        // 대상자 목록에 추가
                        //20150329 위랑 중복 추가됨
				        //pushToArySave_Absence(SearchTxt, obj[SearchTxt], 'U');
				    }
				   // debugger;
				    $('#userName').val("");
				}
				
		    }
		    
		});


		//클릭을 하였을 경우 대상자 추가
		$('.ui-autocomplete').click(function (e, ui) {

		    $('#userName').val("");

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
	function pushToArySave_Absence(name, val, utype) {

	    var icount = 0;
	    if (!isEmptyObj(objj)) {
	        for (var key in objj) {
	            icount++;
	        }
	    }
	
	    if (icount > 0) {
	        alert("위임은 한명만 가능합니다.");
	        return false;
	    }


	    if ("<%= UserID %>" == val) {
	        alert("본인은 지정할 수 없습니다.");
	        return false;
	    }

<%--	    alert("<%= UserID %>");
	    alert(name + "@@@"  +val )--%>;
        
<%--	    var tmpDept = name.split("/");
        
	    if (tmpDept[1] != '<%= UserDeptName%>') {
	        alert("위임은 같은부서만 가능합니다.");
	        return false;
	    }--%>
        
	    if (objj[name] == undefined) {
	        var objcut = name.split('/');
	        if (utype == "U") {
	            //alert("TYPE U : " + name + " ★ " + val + " ★ " + utype);
	            var NameSet = "<li><a href=\"javascript:\" onclick=\"fnDelete(this,'" + name + "');\">" + objcut[0]+"/"+objcut[1]+ "</a></li> ";
	        } else {
	            //alert("TYPE UX : " + name + " ★ " + val + " ★ " + utype);
	            var NameSet = "<li><a href=\"javascript:\" onclick=\"fnDelete(this,'" + name + "');\">" + objcut[0] + "</a></li> ";
            }

	        $("#CommonUserNameList").prepend(NameSet);
	        $("#CommonUserNameList").show();

			objj[name] = val;
			objt[name] = utype;
		}
		else {
			alert('이미 추가 되어 있습니다.');
		}
	}

	//공유 검색에서 사용자 선택시 배열에 저장 
	function pushToArySave_Absence2(name, val, utype, absenceFlag) {

	    if (objj[name] == undefined) {
	        var objcut = name.split('/');
	        if (utype == "U") {

	            var NameSet = "<li><a href=\"javascript:\" onclick=\"fnDelete(this,'" + name + "');\">" + objcut[0] + "/" + objcut[1] + "</a></li> ";
	        } else {
	            var NameSet = "<li><a href=\"javascript:\" onclick=\"fnDelete(this,'" + name + "');\">" + objcut[0] + "</a></li> ";
	        }

	        $("#CommonUserNameList").prepend(NameSet);
	        $("#CommonUserNameList").show();

	        objj[name] = val;
	        objt[name] = utype;

	    }
	    else {

	        if (absenceFlag != "Y") {
                
	            alert('이미 추가 되어 있습니다.');
	        }
	    }
	}



	//선택된 공유 사용자 제거
	function fnDelete(obj, val) {
		delete objj[val];
		delete objt[val];

		$(obj).parent().remove();

		if ($("#CommonUserNameList li").length > 0) {
		    $("#CommonUserNameList").show();
		}
		else {
		    $("#CommonUserNameList").hide();
		}
	}

	//글작성 완료 버튼시 
	function fnAbsence_ShareSave() {
		var UserIDData = "";
		var UserTypeData = "";
		var UserNameData = "";


	    var objTmp = [];

	    if (!isEmptyObj(objj)) {

	        for (var key in objj) {
	            if (objj.hasOwnProperty(key)) {
	                objTmp.push(key);
	            }
	        }
            	       
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

	        // 공유사용자 ID, 이름
	        if (UserIDData != "") {
	            saveUserItemID(UserIDData, UserNameData, UserTypeData);

	            return true;
	        }
	        else {
	            alert('위임받을 사용자를 지정하세요.');
	            return false;
	        }
	    } else {
	        alert('위임받을 사용자를 지정하세요.');
	        return false;
	    }


	    return true;
	}




	function isEmptyObj(obj) {
	    for (var key in obj) return false; return true;
	}




	// hidden값 셋팅
	function saveUserItemID(UserIDData, UserNameData, UserTypeData) {
		$('#<%= this.hidUserItemID.ClientID %>').val("");
		$('#<%= this.hidUserItemID.ClientID %>').val(UserIDData);
		$('#<%= this.hidUserName.ClientID %>').val("");
		$('#<%= this.hidUserName.ClientID %>').val(UserNameData);
		$('#<%= this.hidUserType.ClientID %>').val("");
		$('#<%= this.hidUserType.ClientID %>').val(UserTypeData);
	}

    function getItemValue(ck) {
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

	function fnfocusMove() {
	    $("#userName").focus();
	    $("#userName").val("");
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
		    pushToArySave_Absence(JsonData[i].ToUserName + "/" + JsonData[i].ToUserID, JsonData[i].ToUserID, JsonData[i].ToUserType);
		}
		fnAbsence_ShareSave();
	}

    //사원값 초기화
	function DefaultClear_Absence() {
	    
	    $("#CommonUserNameList").html("");
		obj = {};
		objj = {};
		objt = {};

		$("#CommonUserNameList").hide();
	}

	function fnCheckValue() {
	    var rtn = false;
	    if ($('#<%= this.hidUserItemID.ClientID %>').val() == "") {
	        rtn = true;
	    }
	    return rtn;
	}

	// 사원찾기(조직도)
    function fn_SearchUser_Absence() {
  

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
	    ShareSearchTemp = $('#userName').val();

		// 대상자 목록에 추가 김성환20150121
	    pushToArySave_Absence(UserName + "/" + DeptName + "/" + EmpID, EmpID, 'U');
	}

	// OrgChar로 부서입력
	function OrgChartDept(DeptName, DeptCode) {
	    ShareSearchTemp = $('#userName').val();

		// 대상자 목록에 추가
	    pushToArySave_Absence(DeptName, DeptCode, 'O');
	}

	// 사용자 그룹 선택
	function OnSelectMyGroup() {
		var obj = $("select#" + m_selectID + " option");

		var grpNM = obj.filter(":selected").text();
		var grpID = obj.filter(":selected").val();

		if (grpID != "0" && grpID != null && grpID != undefined) {
		    pushToArySave_Absence(grpNM, grpID, 'G');
			
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
						DefaultClear_Absence();
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
<% if (bGrp) { %>
    <p id="SelectUserGroup">
        <asp:DropDownList ID="ddlGroupUser" Width="266px" CssClass="select" runat="server" />
    </p>
<% } %>
	<p style="padding:0;">
		<input type="text" id="userName"  name="" class="txt t3" value="  이름으로 검색"  onclick="fnfocusMove();"/>         
		<a href="javascript:fn_SearchUser_Absence();" class="btn4"><b>조직도</b></a>
	</p>
	<ul id="CommonUserNameList" style="display:none">

	</ul>
</div>
<!--/지식_조회 권한 설정하기-->
<asp:HiddenField ID="hidUserItemID" runat="server" />
<asp:HiddenField ID="hidUserName" runat="server" />
<asp:HiddenField ID="hidUserType" runat="server" />