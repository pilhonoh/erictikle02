
////암호화 스크립트 추가
document.write("<script type='text/javascript' src='/Common/js/aes_new.js'></script>");
document.write("<script type='text/javascript' src='/Common/js/aes_encrypt.js'></script>");

var AttachInfo;
var secuWeeklyYN = false;
//r nowweeklydateYN = false;
var tmpCmmWeeklyFlag = "";


/*
Author : 개발자- 백충기G, 리뷰자-이정선G
CreateDae :  2016.04.06
Desc : 임원 겸직부서에 대해 위클리 작성하도록 기능 수정 -    deptCode, deptName 추가    
*/
function DetailView(weeklyID, currentUserID, viewLevel, weeklyUserID, commWeeklyFlag, deptCode, deptName) {
    $("#weekly-attach").hide();
    
    //alert(deptCode + "/" + deptName);

    if (commWeeklyFlag == "N" || commWeeklyFlag == undefined)
        tmpCmmWeeklyFlag = "N";
    else
        tmpCmmWeeklyFlag = "Y";

    if (!(weeklyID == null || weeklyID == "null" || weeklyID == 0)) {
        paramWeeklyID = weeklyID; // 전역변수 in WeeklyTop.aspx
        var para = new Object();
        para.weeklyID = weeklyID;
        para.userID = currentUserID;
        para.cmmWeeklyFlag = tmpCmmWeeklyFlag;

        var url = "";

        //2015.07.10 zz17779 : 공용위클리인 경우 호출 url 변경
        if (tmpCmmWeeklyFlag == "Y") {

            url = "/Monthly/MonthlyListTeam.aspx/GetCommonWeekly"; // 2016-03-07 노창현 Weekly -> Monthly
        }
        else
            url = "/Monthly/MonthlyListTeam.aspx/GetWeekly"; // 2016-03-07 노창현 Weekly -> Monthly

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: url, // "/Weekly/WeeklyListTeam.aspx/GetWeekly",
            data: JSON.stringify(para),
            dataType: "json",
            success: function (data) {
            //    alert("d");
                var contextTable = data.d.Table;

                var permission = true;
                             
                /*
                Author : 개발자- 백충기G, 리뷰자-이정선G
                CreateDae :  2016.04.06
                Desc : 임원 겸직부서에 대해 위클리 작성하도록 기능 수정 -    deptCode, deptName 추가    
                */
                //BindingView(contextTable, currentUserID, viewLevel, weeklyUserID);

                BindingView(contextTable, currentUserID, viewLevel, contextTable[0].UserID, deptCode, deptName);

                
                if (permission) {
                    AttachArea(weeklyID);

                    //CommentCtlCreate(weeklyID);

                    // 메모내용 
                    if (contextTable[0].MemoWriterID == currentUserID) {
                        $("#textMemo").val(contextTable[0].MemoContents);
                        if (contextTable[0].MemoContents != "" || contextTable[0].MemoContents != undefined) {
                            $(".change").text("수정하기");
                        }
                    }
                    else {
                        $("#textMemo").val("");
                        $(".change").text("저장하기");
                    }
                }

            },
            error: function (result) {
                if (result.status != 0)
                    alert("Error1" + ":::" + result.responseText);//
            },
            complete: function () {
            }
        });
    }
    else {
        /*
           Author : 개발자- 백충기G, 리뷰자-이정선G
           CreateDae :  2016.04.06
           Desc : 임원 겸직부서에 대해 위클리 작성하도록 기능 수정 -    deptCode, deptName 추가    
           */
        //BindingView("", currentUserID, viewLevel, weeklyUserID);
        BindingView("", currentUserID, viewLevel, weeklyUserID, deptCode, deptName);
        $(".weekly-reply").html("");
    }
}


//2015-07-06 김성환 3주 체크
function BindingView2(weeklyID, currentUserID, viewLevel, weeklyUserID) {

    var dHtml = "";

    dHtml = '<li class="text-no"><p>전사보안기준에 따라 3주가 경과한 Weekly는 삭제 처리 되었습니다</p></li>';


    $(".weekly-text").html(dHtml);

    
    fnGetUser2(weeklyUserID);
}

/*
Author : 개발자- 백충기G, 리뷰자-이정선G
CreateDae :  2016.04.06
Desc : 임원 겸직부서에 대해 위클리 작성하도록 기능 수정 -    deptCode, deptName 추가    
*/
function BindingView(table, currentUserID, viewLevel, weeklyUserID, deptCode, deptName) {
    var dHtml = "";

    if (viewLevel != '1' && table.length > 0 && table[0].PermissionYN == 'Y' && (table[0].PermissionsUserID == null || table[0].PermissionsUserID != currentUserID) && weeklyUserID != currentUserID) {

        //권한 체크 먼저
        dHtml += '<li class="text-no"><p>조회권한이 없습니다.</p>';
        dHtml += '</li>';
        $(".weekly-text").html(dHtml);
        fnGetUser2(weeklyUserID);
        secuWeeklyYN = false;
        return;
    }

    if (table.length > 0 && table[0].PermissionYN != 'Y') {


        var tempYN = table[0].TempYN.replace(/(^\s*)|(\s*$)/gi, "");
        //임시저장을 다른 사용자가 URL로 치고온 경우 제외

        if (tempYN == 'Y' && table[0].UserID != currentUserID) {
            dHtml += '<li class="text-no"><p>작성된 내용이 없습니다.</p>';
            dHtml += '  <div class="request" >';

            if (currentUserID == weeklyUserID) {
                //2016-12-16
                //P097010 BACKUP1
                //dHtml += '      <a href="javascript:///" class="btnB" onclick="WriteMonthly()"><span class="memo">Monthly 작성하기</span></a>'; // 2016-03-07 노창현 WriteWeekly -> WriteMonthly
            }

          

            dHtml += '  </div>';
            dHtml += '</li>';
            $(".weekly-text").html(dHtml);
            fnGetUser2(weeklyUserID);
            return;
        }
        dHtml += '<li style="padding-top:5px !important;">';

        if (table[0].UserID == currentUserID && table[0].CreateDateTime != null) {
            //$(".btnArea").show();

            if (tempYN == 'Y')
                dHtml += '<p class="temporary-save">※ 임시저장 중 입니다.</p>';

        }

        //2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련 : 위임해서 썻다는 내용 표시하기 위해 추가.

        if (table[0].AbsenceMsg != "-" && table[0].AbsenceMsg != "undefined" && table[0].AbsenceMsg != undefined) {
            dHtml += '<p class="temporary-save1">' + table[0].AbsenceMsg + '</p>';
        }
        
        if (!(table[0].DeptName == null || table[0].DeptName == "null")) {
            //dHtml += "<table style='width:100%;cellspacing='0' cellpadding='0'><tr><td style='padding:0;margin:0;'>";
            // MostiSoft - padding-bottom : 50px; 추가.
            dHtml += "<table style='width: 100%; padding-bottom: 50px;' cellspacing='0' cellpadding='0'><tr><td style='padding:0;margin:0;'>";
                        
            // 팀원,팀장 - 기간별
            if ($('.btnB.on').text() == "기간별") {

                //공용위클리에 대해 좌측 내용보여주는 부분 수정(00팀 공용 Weekly 공간입니다. 마지막작성자:홍길동)
                if (tmpCmmWeeklyFlag == "Y") {
                    dHtml += '<p class="date" style="text-align:left;">' +  table[0].department + " 공용 Weekly 공간입니다." + '</p>';
                } else {
                    dHtml += '<p class="date" style="text-align:left;">' + table[0].DeptName + ' / ' + table[0].UserName + '</p>'; //padding-left:18px; 
                }

            }
                // 팀원,팀장 - 작성자별
            else if ($('.btnB.on').text() == "작성자별") {
                dHtml += '<p class="date" style="text-align:left;">' + table[0].Month + '</p>'; //padding-left:18px; 
            }
                // 임원 - 조직도
            else {
                if (tmpCmmWeeklyFlag == "Y") {
                    dHtml += '<p class="date" style="text-align:left;">' + table[0].department + " 공용 Weekly 공간입니다." + '</p>';
                } else {
                    /*
                    Author : 개발자- 백충기G, 리뷰자-이정선G
                    CreateDae :  2016.04.06
                    Desc : 임원 겸직부서에 대해 위클리 작성하도록 기능 수정       
                    */
                    if (deptCode == "undefined" || deptCode == undefined)
                        dHtml += '<p class="date" style="text-align:left;">' + table[0].DeptName + '</p>'; //여기서 표시 padding-left:18px;
                    else
                        dHtml += '<p class="date" style="text-align:left;">' + deptName + '</p>'; //여기서 표시 padding-left:18px;

                    //dHtml += '<p class="date" style="text-align:left;">' + table[0].DeptName + '</p>'; //padding-left:18px;
                }


            }
            dHtml += "</td><td style='padding:0;margin:0;'>";
        }

        //if (viewLevel == "1" || viewLevel == "2") {
        dHtml += "</td><td style='padding:0;margin:0;'>";
        //}

        if (!(table[0].UpdateDateTime == null || table[0].UpdateDateTime == "null")) {
            //공용위클리에 대해 좌측 내용보여주는 부분 수정(00팀 공용 Weekly 공간입니다. 마지막작성자:홍길동)
            if (tmpCmmWeeklyFlag == "Y") {
                dHtml += '<p class="date">' + GetJsonDateTimeString(table[0].UpdateDateTime) + "(마지막 작성자:" + table[0].UserName + ')</p>'; //2015-10-08 (목) 오전 09:35(마지막 작성자:전소영)
            } else {
                dHtml += '<p class="date">' + GetJsonDateTimeString(table[0].UpdateDateTime) + '</p>';
            }
            
        }
        //if (viewLevel == "1" || viewLevel == "2") {
        dHtml += '</td></td></table>';
        //}


        // Mr.No 2015-07-06 iframe 적용중
        if (table[0].WeeklyID == "0" || table[0].WeeklyID == undefined || table[0].WeeklyID == "undefined") {
            // 기존로직
            dHtml += '<p class="text" data-weeklyID=' + table[0].WeeklyID + '><div class="view_ct_area" style="padding-top:10px;"><div class="view_ct" style="margin:0;">' + table[0].Contents + '</div></div>'
        } else {
            // iframe 적용 로직

            /*
           Author : 개발자- 최현미C, 리뷰자-진현빈D
           CreateDae :  2016.04.25
           Desc : 파라미터 암호화
           */
            var tmpWeeklyID = EncryptParam(table[0].WeeklyID);

            dHtml += '<p class="text" data-weeklyID=' + table[0].WeeklyID + '><iframe class="StandaloneView' + table[0].WeeklyID + '" src="MonthlyIframe_TeamMember.aspx?WeeklyID=' + tmpWeeklyID + '"  scrolling="no" frameborder="0" style="width:100%"><div class="view_ct_area" style="padding-top:10px;"><div class="view_ct" style="margin:0;">' + table[0].Contents + '</div></div></iframe>'
        }
        dHtml += '</p>';
        

        if (viewLevel == 3) { //팀장인경우.

            //start -------------------->2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련 
            dHtml += '<div class="f_left m_t20" style="padding-top:10px; padding-bottom:10px;">';          
            if (currentUserID == table[0].UserID){    // 자기 자신의 위클리에만 편집하기 버튼이 보입니다 
                if (table[0].WeeklyID == "0" || table[0].WeeklyID == undefined || table[0].WeeklyID == "undefined")
                    dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(0);"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기</label>';
                else
                    dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(\'' + table[0].WeeklyID + '\' );"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기</label>';
            }
            else {
                if (tmpCmmWeeklyFlag == "N") { //N이면 기존 편집 , y이면 공통편집
                    if (table[0].ViewLevel == "4") {
                        //2015.09.09 zz1777: 3주삭제 처리 안하기로 요청(전소영M)
                        if (table[0].WeeklyID == "0" || table[0].WeeklyID == undefined || table[0].WeeklyID == "undefined")
                            dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(0);"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기</label>';
                        else
                            dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(\'' + table[0].WeeklyID + '\' );"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기</label>';
                    }
                }
                else {

                    if (viewLevel == 3 || viewLevel == 4) {
                        //2015.09.09 zz1777: 3주삭제 처리 안하기로 요청(전소영M)
                        if (table[0].WeeklyID == "0" || table[0].WeeklyID == undefined || table[0].WeeklyID == "undefined")
                            dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(0);"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기</label>';
                        else
                            dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(\'' + table[0].WeeklyID + '\' );"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기</label>';
                    }

                }
            }

            dHtml += '</div>';

            //end -------------------->2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련 

                        
            dHtml += '<div class="f_right m_t20">';
            //if (currentUserID == weeklyUserID)   2015-05-31 mr.kim
            if (currentUserID == table[0].UserID)    // 자기 자신의 위클리에만 편집하기 버튼이 보입니다 
            {

                //2015.09.09 zz1777: 3주삭제 처리 안하기로 요청(전소영M)  
                //if (table[0].WeeklyID == "0" || table[0].WeeklyID == undefined || table[0].WeeklyID == "undefined") 
                //    dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(0);"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기1</label>';
                //else
                //    dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(\'' + table[0].WeeklyID + '\' );"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기1</label>';

                dHtml += '<a href="javascript:///" class="btnB" onclick="fnEmail_init();layer_open(\'pop_email_mailorgchart\');" id="erpmemoview"  style="margin-left:5px;"><span class="memo">이메일보내기</span></a>';

                if (tmpCmmWeeklyFlag == "N") //N이면 기존 편집 , y이면 공통편집
                    dHtml += '<a href="javascript:///" class="btnB" onclick="WriteMonthly();" id="erpmemoview" style="margin-left:5px;"><span class="memo">편집하기</span></a>'; // 2016-03-07 노창현 WriteWeekly -> WriteMonthly
                else {

                    //--2015.10.08 ZZ17779:임원에서 공용위클리 출력 가능하도록 수정  : 임원은 작성,편집 못하게 설정
                    if(viewLevel == 3 || viewLevel == 4)
                        dHtml += '<a href="javascript:///" class="btnB" onclick="WriteCommonWeekly();" id="erpmemoview" style="margin-left:5px;"><span class="memo">편집하기</span></a>';
                }

            }
            else {
                if (tmpCmmWeeklyFlag == "N") { //N이면 기존 편집 , y이면 공통편집

                    if (table[0].ViewLevel == "4") {

                        ////2015.09.09 zz1777: 3주삭제 처리 안하기로 요청(전소영M)
                        //if (table[0].WeeklyID == "0" || table[0].WeeklyID == undefined || table[0].WeeklyID == "undefined")
                        //    dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(0);"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기2</label>';
                        //else
                        //    dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(\'' + table[0].WeeklyID + '\' );"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기2</label>';


                        // MostiSoft - 설명 띄워주는 부분 제거.
                        dHtml += '<a href="javascript:///" class="btnB" onclick="layer_open(\'layer2\');return false;" id="coachy"><span class="memo">Weekly Note</span></a>';

                        if (table[0].MemoWriterID == "") {    // 메모가 있다면 색변경
                            dHtml += '<a href="javascript:///" class="btnB m_l5" id="erpmemoview" onclick="MemoViewEHR(\'' + currentUserID + '\',\'' + weeklyUserID + '\');return false;"><span class="memo-list">Note 보기</span></a>';
                        } else {
                            dHtml += '<a href="javascript:///" class="btnB m_l5 completion" id="erpmemoview" onclick="MemoViewEHR(\'' + currentUserID + '\',\'' + weeklyUserID + '\');return false;"><span class="memo-list">Note 보기</span></a>';
                        }

                        dHtml += '<a href="javascript:///" class="btnB" onclick="WriteMonthly();" id="erpmemoview" style="margin-left:5px;"><span class="memo">편집하기</span></a>'; // 2016-03-07 노창현 WriteWeekly -> WriteMonthly

                    }
                }
                else {

                    //2015.09.09 zz1777: 3주삭제 처리 안하기로 요청(전소영M)
                    //if (table[0].WeeklyID == "0" || table[0].WeeklyID == undefined || table[0].WeeklyID == "undefined")
                    //    dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(0);"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기3</label>';
                    //else
                    //    dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(\'' + table[0].WeeklyID + '\' );"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기3</label>';

                    //--2015.10.08 ZZ17779:임원에서 공용위클리 출력 가능하도록 수정  : 임원은 작성,편집 못하게 설정
                    if (viewLevel == 3 || viewLevel == 4)
                        dHtml += '<a href="javascript:///" class="btnB" onclick="WriteCommonWeekly();" id="erpmemoview" style="margin-left:5px;"><span class="memo">편집하기</span></a>';
                }
                                
            }

            dHtml += '</div>';

        } //end if viewLevel=3(팀장인경우)
        else { //팀장이 아닌 경우.


            //start -------------------->2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련 
            dHtml += '<div class="f_left m_t20" style="padding-top:10px; padding-bottom:10px;">';
            if (tmpCmmWeeklyFlag == "N") {

                if (currentUserID == table[0].UserID)    // 자기 자신의 위클리에만 편집하기 버튼이 보입니다
                {
                    //2015.09.09 zz1777: 3주삭제 처리 안하기로 요청(전소영M) : 위클리삭제는 팀장과 작성자만 권한 존재.
                    if (table[0].WeeklyID == "0" || table[0].WeeklyID == undefined || table[0].WeeklyID == "undefined")
                        dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(0);"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기</label>';
                    else {
                        /*
                         Author : 개발자- 백충기G, 리뷰자-이정선G
                         CreateDae : 2016.04.06
                         Desc : 임원 겸직부서에 대해 위클리 작성하도록 기능 수정        - 여기다
                         */
                        dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(\'' + table[0].WeeklyID + '\',\'' + deptCode + '\', \'' + deptName + '\' );"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기</label>';

                        //2016.04.06 원본
                        //dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(\'' + table[0].WeeklyID + '\' );"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기</label>';
                    }
                }
            }//end if (tmpCmmWeeklyFlag == "N")
            else {

                if (viewLevel == 3 || viewLevel == 4) {
                    //2015.09.09 zz1777: 3주삭제 처리 안하기로 요청(전소영M) : 위클리삭제는 팀장과 작성자만 권한 존재.
                    if (table[0].WeeklyID == "0" || table[0].WeeklyID == undefined || table[0].WeeklyID == "undefined")
                        dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(0);"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기</label>';
                    else
                        dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(\'' + table[0].WeeklyID + '\' );"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기</label>';
                }
            }
            dHtml += '</div>';
            //end -------------------->2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련 


            dHtml += '<div class="f_right m_t20">';

            if (tmpCmmWeeklyFlag == "N") {

                if (currentUserID == table[0].UserID)    // 자기 자신의 위클리에만 편집하기 버튼이 보입니다
                {
                    ////2015.09.09 zz1777: 3주삭제 처리 안하기로 요청(전소영M) : 위클리삭제는 팀장과 작성자만 권한 존재.
                    //if (table[0].WeeklyID == "0" || table[0].WeeklyID == undefined || table[0].WeeklyID == "undefined")
                    //    dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(0);"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기4</label>';
                    //else
                    //    dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(\'' + table[0].WeeklyID + '\' );"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기4</label>';


                    if (viewLevel == 2) { //임원은 이메일 보내기 버튼이 있다.
                        dHtml += '<a href="javascript:///" class="btnB" onclick="fnEmail_init();layer_open(\'pop_email_mailorgchart\');" id="erpmemoview" style="margin-left:5px;"><span class="memo">이메일보내기</span></a>';
                    }
                }

                if (table[0].SameTeamYN == "Y" && table[0].ViewLevel != 3 || currentUserID == table[0].UserID) {

                    /*
                    Author : 개발자- 백충기G, 리뷰자-이정선G
                    CreateDae :  2016.04.06
                    Desc : 임원 겸직부서에 대해 위클리 작성하도록 기능 수정        - 여기다
                    */

                    if (deptCode == "undefined" || deptCode == undefined) {
                        dHtml += '<a href="javascript:///" class="btnB" onclick="WriteMonthly();" id="erpmemoview" style="margin-left:5px;"><span class="memo">편집하기</span></a>';
                    } else {
                        dHtml += '<a href="javascript:///" class="btnB" onclick="WriteMonthly(\'' + "N" + '\',\'' + deptCode + '\', \'' + deptName + '\');" id="erpmemoview" style="margin-left:5px;"><span class="memo">편집하기</span></a>';
                    }
                    //dHtml += '<a href="javascript:///" class="btnB" onclick="WriteMonthly();" id="erpmemoview" style="margin-left:5px;"><span class="memo">편집하기</span></a>'; // 2016-03-07 노창현 WriteWeekly -> WriteMonthly
                }
            }//end if (tmpCmmWeeklyFlag == "N")
            else {
                //2015.09.09 zz1777: 3주삭제 처리 안하기로 요청(전소영M) : 위클리삭제는 팀장과 작성자만 권한 존재.
                //if (table[0].WeeklyID == "0" || table[0].WeeklyID == undefined || table[0].WeeklyID == "undefined")
                //    dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(0);"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기5</label>';
                //else
                //    dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(\'' + table[0].WeeklyID + '\' );"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기5</label>';

                //--2015.10.08 ZZ17779:임원에서 공용위클리 출력 가능하도록 수정  : 임원은 작성,편집 못하게 설정
             
                if (viewLevel == 3 || viewLevel == 4)
                    dHtml += '<a href="javascript:///" class="btnB" onclick="WriteCommonWeekly();" id="erpmemoview" style="margin-left:5px;"><span class="memo">편집하기</span></a>';
            }

            dHtml += '</div>';
        }

        dHtml += '</li>';
        $(".weekly-text").html(dHtml);


    }//end if table.length > 0 && table[0].PermissionYN != 'Y'
    else if (table.length > 0 && table[0].PermissionYN == 'Y') {

        var tempYN = table[0].TempYN.replace(/(^\s*)|(\s*$)/gi, "");
        if (tempYN == 'Y' && table[0].UserID != currentUserID) {
            dHtml += '<li class="text-no"><p>작성된 내용이 없습니다.</p>';
            dHtml += '  <div class="request" >';
            if (currentUserID == weeklyUserID) {
                //2016-12-16
                //P097010 BACKUP1
                //dHtml += '      <a href="javascript:///" class="btnB" onclick="WriteMonthly()"><span class="memo">Monthly 작성하기</span></a>'; // 2016-03-07 노창현 WriteWeekly -> WriteMonthly
            }

            dHtml += '  </div>';
            dHtml += '</li>';
            $(".weekly-text").html(dHtml);
            fnGetUser2(weeklyUserID);
            return;
        }
        dHtml += '<li style="padding-top:5px !important;">';

        if (table[0].UserID == currentUserID && table[0].CreateDateTime != null) {
            if (tempYN == 'Y')
                dHtml += '<p class="temporary-save">※ 임시저장 중 입니다.</p>';
        }

        if (!(table[0].DeptName == null || table[0].DeptName == "null")) {
            dHtml += "<table style='width:100%;cellspacing='0' cellpadding='0'><tr><td style='padding:0;margin:0;'>";
            // 팀원,팀장 - 기간별
            if ($('.btnB.on').text() == "기간별") {
                //공용위클리에 대해 좌측 내용보여주는 부분 수정(00팀 공용 Weekly 공간입니다. 마지막작성자:홍길동)
                if (tmpCmmWeeklyFlag == "Y") {
                    dHtml += '<p class="date" style="text-align:left;">' + table[0].department + " 공용 Weekly 공간입니다." + '</p>';
                } else {
                    dHtml += '<p class="date" style="text-align:left;">' + table[0].DeptName + ' / ' + table[0].UserName + '</p>';
                }

               
            }
                // 팀원,팀장 - 작성자별
            else if ($('.btnB.on').text() == "작성자별") {
                dHtml += '<p class="date" style="text-align:left;">' + table[0].Month + '</p>';
            }
                // 임원 - 조직도
            else {
                if (tmpCmmWeeklyFlag == "Y") {
                    dHtml += '<p class="date" style="text-align:left;">' + table[0].department + " 공용 Weekly 공간입니다." + '</p>';
                } else {
                    dHtml += '<p class="date" style="text-align:left;">' + table[0].DeptName + '</p>';
                }
            }
            dHtml += "</td><td style='padding:0;margin:0;'>";
        }

        dHtml += "</td><td style='padding:0;margin:0;'>";

        if (!(table[0].UpdateDateTime == null || table[0].UpdateDateTime == "null")) {

            if (tmpCmmWeeklyFlag == "Y") {
                
                dHtml += '<p class="date">' + GetJsonDateTimeString(table[0].UpdateDateTime) + "(마지막 작성자:" + table[0].UserName + ')</p>'; //2015-10-08 (목) 오전 09:35(마지막 작성자:전소영)
            } else {
                dHtml += '<p class="date">' + GetJsonDateTimeString(table[0].UpdateDateTime) + '</p>';
            }
            
        }
        dHtml += '</td></td></table>';

        dHtml += '<p class="text" data-weeklyID=' + table[0].WeeklyID + '><div class="view_ct_area" style="padding-top:10px;"><div class="view_ct" style="margin:0;">' + table[0].Contents + '</div></div>'
        dHtml += '</p>';

        if (viewLevel == 3) {

            //start -------------------->2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련 
            dHtml += '<div class="f_left m_t20" style="padding-top:10px; padding-bottom:10px;">';

            if (currentUserID == table[0].UserID)    // 자기 자신의 위클리에만 편집하기 버튼이 보입니다 
            {
                //2015.09.09 zz1777: 3주삭제 처리 안하기로 요청(전소영M) : 위클리삭제는 팀장과 작성자만 권한 존재.
                if (table[0].WeeklyID == "0" || table[0].WeeklyID == undefined || table[0].WeeklyID == "undefined")
                    dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(0);"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기</label>';
                else
                    dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(\'' + table[0].WeeklyID + '\' );"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기</label>';
            }
            else {
                if (table[0].ViewLevel == "4") {
                    //2015.09.09 zz1777: 3주삭제 처리 안하기로 요청(전소영M) : 위클리삭제는 팀장과 작성자만 권한 존재.
                    if (table[0].WeeklyID == "0" || table[0].WeeklyID == undefined || table[0].WeeklyID == "undefined")
                        dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(0);"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기</label>';
                    else
                        dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(\'' + table[0].WeeklyID + '\' );"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기</label>';

                }
            }

            dHtml += '</div>';
            //end -------------------->2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련 


            dHtml += '<div class="f_right m_t20">';          
            if (currentUserID == table[0].UserID)    // 자기 자신의 위클리에만 편집하기 버튼이 보입니다 
            {
                //2015.09.09 zz1777: 3주삭제 처리 안하기로 요청(전소영M) : 위클리삭제는 팀장과 작성자만 권한 존재.
                //if (table[0].WeeklyID == "0" || table[0].WeeklyID == undefined || table[0].WeeklyID == "undefined")
                //    dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(0);"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기6</label>';
                //else
                //    dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(\'' + table[0].WeeklyID + '\' );"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기6</label>';


                dHtml += '<a href="javascript:///" class="btnB" onclick="fnEmail_init();layer_open(\'pop_email_mailorgchart\');" id="erpmemoview"  style="margin-left:5px;"><span class="memo">이메일보내기</span></a>';
                dHtml += '<a href="javascript:///" class="btnB" onclick="WriteMonthly();" id="erpmemoview" style="margin-left:5px;"><span class="memo">편집하기</span></a>'; // 2016-03-07 노창현 WriteWeekly -> WriteMonthly
            }
            else {
                if (table[0].ViewLevel == "4") {
                    //2015.09.09 zz1777: 3주삭제 처리 안하기로 요청(전소영M) : 위클리삭제는 팀장과 작성자만 권한 존재.
                    //if (table[0].WeeklyID == "0" || table[0].WeeklyID == undefined || table[0].WeeklyID == "undefined")
                    //    dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(0);"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기7</label>';
                    //else
                    //    dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(\'' + table[0].WeeklyID + '\' );"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기7</label>';


                    // MostiSoft - 설명 띄워주는 부분 제거.
                    dHtml += '<a href="javascript:///" class="btnB" onclick="layer_open(\'layer2\');return false;" id="coachy"><span class="memo">Weekly Note</span></a>';

                    if (table[0].MemoWriterID == "") {    // 메모가 있다면 색변경
                        dHtml += '<a href="javascript:///" class="btnB m_l5" id="erpmemoview" onclick="MemoViewEHR(\'' + currentUserID + '\',\'' + weeklyUserID + '\');return false;"><span class="memo-list">Note 보기</span></a>';
                    } else {
                        dHtml += '<a href="javascript:///" class="btnB m_l5 completion" id="erpmemoview" onclick="MemoViewEHR(\'' + currentUserID + '\',\'' + weeklyUserID + '\');return false;"><span class="memo-list">Note 보기</span></a>';
                    }
                    dHtml += '<a href="javascript:///" class="btnB" onclick="WriteMonthly();" id="erpmemoview" style="margin-left:5px;"><span class="memo">편집하기</span></a>'; // 2016-03-07 노창현 WriteWeekly -> WriteMonthly
                }                
            }

            dHtml += '</div>';
        }//end if (viewLevel == 3)
        else {

            //start -------------------->2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련 
            dHtml += '<div class="f_left m_t20" style="padding-top:10px; padding-bottom:10px;">';

            if (currentUserID == table[0].UserID)    // 자기 자신의 위클리에만 편집하기 버튼이 보입니다
            {
                //2015.09.09 zz1777: 3주삭제 처리 안하기로 요청(전소영M) : 위클리삭제는 팀장과 작성자만 권한 존재.
                if (table[0].WeeklyID == "0" || table[0].WeeklyID == undefined || table[0].WeeklyID == "undefined")
                    dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(0);"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기</label>';
                else {
                    dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(\'' + table[0].WeeklyID + '\' );"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기</label>';
                }
            }

            dHtml += '</div>';
            //end -------------------->2015.08.10 zz17779 : 팀장/임원 부재시 임원 관련 


            dHtml += '<div class="f_right m_t20">';
            if (currentUserID == table[0].UserID)    // 자기 자신의 위클리에만 편집하기 버튼이 보입니다
            {
                //2015.09.09 zz1777: 3주삭제 처리 안하기로 요청(전소영M) : 위클리삭제는 팀장과 작성자만 권한 존재.
                //if (table[0].WeeklyID == "0" || table[0].WeeklyID == undefined || table[0].WeeklyID == "undefined")
                //    dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(0);"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기8</label>';
                //else
                //    dHtml += '<input type="checkbox" id="chkContentDel" onclick="fnWeeklyDelete(\'' + table[0].WeeklyID + '\' );"  style="cursor:pointer;"/><label style="font-size:13px;color:red;vertical-align:-1px;margin-left:5px;">본문 삭제하기8</label>';


                if (viewLevel == 2) { //임원은 이메일 보내기 버튼이 있다.
                    /*
                         Author : 개발자- 백충기G, 리뷰자-이정선G
                         CreateDae :  2016.04.06
                         Desc : 임원 겸직부서에 대해 위클리 작성하도록 기능 수정  - 여기다     
                    */
                    if (deptCode == "undefined" || deptCode == undefined) {
                        dHtml += '<a href="javascript:///" class="btnB" onclick="fnEmail_init();layer_open(\'pop_email_mailorgchart\');" id="erpmemoview" style="margin-left:5px;"><span class="memo">이메일보내기</span></a>';
                    } else {
                        dHtml += '<a href="javascript:///" class="btnB" onclick="fnEmail_init();fnAdditionJobEmailCheck(\'' + deptCode + '\', \'' + deptName + '\');layer_open(\'pop_email_mailorgchart\');" id="erpmemoview" style="margin-left:5px;"><span class="memo">이메일보내기</span></a>';
                    }

                    //dHtml += '<a href="javascript:///" class="btnB" onclick="fnEmail_init();layer_open(\'pop_email_mailorgchart\');" id="erpmemoview" style="margin-left:5px;"><span class="memo">이메일보내기</span></a>';
                }
            }
            if (table[0].SameTeamYN == "Y" && table[0].ViewLevel != 3 || currentUserID == table[0].UserID) {


                //dHtml += '<a href="javascript:///" class="btnB" onclick="WriteMonthly();" id="erpmemoview" style="margin-left:5px;"><span class="memo">편집하기</span></a>'; // 2016-03-07 노창현 WriteWeekly -> WriteMonthly


            }

            dHtml += '</div>';
        }

        dHtml += '</li>';

        if (table[0].PermissionYN == 'Y') {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Weekly/WeeklyListTeam.aspx/GetWeeklyPermissions",
                data: "{weeklyID : '" + table[0].WeeklyID + "'}",  //JSON.stringify(table[0].WeeklyID),
                dataType: "json",
                success: function (data) {
                    var Table1 = data.d.Table;

                    if (Table1.length > 0) {
                        dHtml += '<li><div style="margin-top:30px;padding:10px;border:1px solid #CCC;word-wrap:normal;"><b>조회권한</b><br/>';
                        for (var i = 0; i < Table1.length; i++) {
                            if (Table1.length - 1 == i) {
                                dHtml += Table1[i].ToUserName + " ";
                            } else {
                                dHtml += Table1[i].ToUserName + ",  ";
                            }
                            //alert(((i + 1) % 5) == 0);
                            //if (((i + 1) % 5) == 0) {
                            //    dHtml += "<br/>";
                            //}
                        }
                        dHtml += '</div></li>';
                    }

                    $(".weekly-text").html(dHtml);
                },
                error: function (result) {
                    if (result.status != 0)
                        alert("Error" + "::::" + result);
                },
            });
        }
    }
    else //작성된 위클리가 없는경우 아래로직 처리..
    {

        //2015-07-02 김성환 추가 
        if (secuWeeklyYN == true) {
            dHtml += '<li class="text-no"><p>조회권한이 없습니다.</p></li>';
            $(".weekly-text").html(dHtml);
            secuWeeklyYN = false;
        } else {
            //$(".btnArea").hide();

            dHtml += '<li class="text-no"><p>작성된 내용이 없습니다.</p>';
            dHtml += '  <div class="request" >';

            //2015.07.10 ZZ17779 : 공용위클리 버튼
            if (tmpCmmWeeklyFlag == "Y") {
                //alert(viewLevel);
                //--2015.10.08 ZZ17779:임원에서 공용위클리 출력 가능하도록 수정  : 임원은 작성,편집 못하게 설정
                if (viewLevel == 3 || viewLevel == 4)
                    dHtml += '      <br/><a href="javascript:///" class="btnB" onclick="WriteCommonWeekly()" id="alinkCommon" style="display:none;"><span class="memo">공용 Weekly 작성하기</span></a>';


            } else {
                /*
                 Author : 개발자- 백충기G, 리뷰자-이정선G
                 CreateDae : 2016.04.06
                 Desc : 임원 겸직부서에 대해 위클리 작성하도록 기능 수정  - deptcode, deptname추가
                 */
                
                if (currentUserID == weeklyUserID) {
                    //2016-12-16
                    //P097010 BACKUP1
                    //dHtml += '      <a href="javascript:///" class="btnB" onclick="WriteMonthly(\'' + "N" + '\',\'' + deptCode + '\', \'' + deptName + '\')"><span class="memo">Monthly 작성하기</span></a>';
                    ////dHtml += '      <a href="javascript:///" class="btnB" onclick="WriteMonthly()"><span class="memo">Monthly 작성하기</span></a>'; // 2016-03-07 노창현 WriteWeekly -> WriteMonthly
                }
                
            }

            dHtml += '  </div>';
            dHtml += '</li>';
            $(".weekly-text").html(dHtml);

            if (tmpCmmWeeklyFlag == "Y") {
                $("#alinkCommon").show();
                $("#hidCommonWeeklyFlag").val("Y");
            }

        }


    }



}

function sleep(msecs) {
    var start = new Date().getTime();
    varcur = start;
    while (cur - start < msecs) {
        cur = new Date().getTime();
    }
}

function CommentCtlCreate(weeklyID) {

    if (!(weeklyID == 0 || weeklyID == null || weeklyID == "" || weeklyID == "null")) {
        var userID = $("li.on").attr("id");
        if (!(userID == null || userID == undefined)) {
            if (userID.indexOf('-') != -1) {
                userID = $("li.on").attr("id").split('-')[0]
            }
        }

        // 댓글 리스트
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Monthly/MonthlyCommentControl.aspx?WeeklyID=" + weeklyID + "&FirstUserID=" + userID, // 2016-03-07 노창현 Monthly - > Monthly
            data: "",
            dataType: "html",
            success: function (data) {
                $(".weekly-reply").html(data);
                //commentList(weeklyID); // Commnet List 
            },
            error: function (result) {
                if (result.status != 0)
                    alert("Error " + ":::" + result);
            },
            complete: function () {
                //alert('complete');
            }
        });
    }
    else {
        $(".weekly-reply").html("");
    }
}


//첨부파일이 존재할 경우 동적으로 처리하는 영역
function AttachArea(weeklyID) {


    //alert("AttachArea");
    var para = new Object();
    para.weeklyID = weeklyID;

    //첨부 목록
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Monthly/MonthlyListTeam.aspx/GetAttachInfo", // 2016-03-07 노창현 Monthly - > Monthly
        data: JSON.stringify(para),
        dataType: "json",
        success: function (data) {
            
            AttachInfo = jQuery.parseJSON(data.d);
            if (AttachInfo.length > 0) {

                //old code : 덱스트업로드 관련
                //dHtml = '<br/>';
                //dHtml += '<div id="manager_container" class="fileArea" ></div>';
                //dHtml += '<script type="text/javascript">';
                //dHtml += 'DEXTUploadFL.createDownloadManager(';
                //dHtml += '    "manager_container", ';
                //dHtml += '    "DEXTDOWNMAN", ';
                //dHtml += '    "../Common/Controls/DextUploadFl/DEXT_LIST_DOWN_MANAGER.swf", ';
                //dHtml += '    "#ffffff", ';
                //dHtml += '    "transparent", '; //window, transparent
                //dHtml += '    "", ';
                //dHtml += '    "", ';
                //dHtml += '    "multi", ';// simple, multi // right button
                //dHtml += '    "ForDEXTDOWNMAN" ';
                //dHtml += ');';
                //dHtml += '</script>';
                

                //2015.10.26 zz17779 : 파일업로드 변경관련 추가
                var dHtml2 = "";
                var fileLink = "";
                var totalSize = "";
             

                dHtml2 += "<tr class='t_bold' style='background-color:#efefef;'>";
                dHtml2 += "<td><input type='checkbox' id='chkAllFile' name='chkAllFile' onclick='chkAll(this);'/></td></td>";
                dHtml2 += '<td colspan="2" class="fileTitle">파일명</td>';
                dHtml2 += '<td class="fileSize" style="padding-right:10px;">크기</td>';
                dHtml2 += '</tr>'


               // AttachInfo[i].FileName + "', '/SKT_MultiUploadedFiles/" + AttachInfo[i].Folder.replace("\\", "/") + "/" + AttachInfo[i].FileName + "' 


                for (var i = 0; i < AttachInfo.length; i++) {

                    fileLink = "<a href=\"javascript:void(0);\" onClick=\"FileCtrl_FileDownload('" + AttachInfo[i].FileName + "', '/SKT_MultiUploadedFiles/" + AttachInfo[i].Folder.replace("\\", "/") + "/" + AttachInfo[i].FileName + "' );\">" + AttachInfo[i].FileName + "</a>";

                    dHtml2 += "<tr FileName='" + AttachInfo[i].FileName + "'>";

                    dHtml2 += StringFormat('<td class="fileIcon"><input type="checkbox"  name="chkFile" id="chk{0}" value="{1}" /></td>', i, AttachInfo[i].AttachID + "#" + AttachInfo[i].FileName + "#" + AttachInfo[i].Folder.replace("\\", "/") + "#");

                    dHtml2 += StringFormat('<td class="fileIcon"><img class="fileUploadTypeIcon" src="/Images/ICON/IC{0}.gif"></td>', AttachInfo[i].Extension.toUpperCase().replace(".",""));
                    dHtml2 += StringFormat('<td>{0}</td>', fileLink);
                    dHtml2 += StringFormat('<td class="fileSize">{0}</td>', AttachInfo[i].FileSizeString);
                    dHtml2 += '</tr>'


                    totalSize = AttachInfo[i].TotalFileSize;
                }

                dHtml = '<div class="weekly_filewrite writeBox">';
                dHtml += '<div class="FileUploadCtrl">';
                dHtml += '<div class="FileUloadTable">';

                dHtml += '<table>';

                dHtml += dHtml2;

                dHtml += '</table>';

                dHtml += '</div>'; //FileUloadTable

                dHtml += '<div id="divmulti">';
                dHtml += "<p class=\"writeBox_text\">";
                dHtml += "전체 : " + AttachInfo.length + "개 (" + totalSize + ")";
                dHtml += "</p>";
                dHtml += "<a href=\"javascript:///\" class=\"btnB\" onclick=\"fnMultiDown('" + weeklyID + "', 'chkFile');\" id=\"btnMultidown\"><span>다운로드</span></a>";
                dHtml += '</div>'; //divmulti

                dHtml += '</div>'; //FileUploadCtrl
                dHtml += '</div>'; //weekly_filewrite
                


                $("#weekly-attach").html(dHtml);
                $("#weekly-attach").show();


            }


        },
        error: function (result) {
            if (result.status != 0)
                alert("Error" + ":::" + result);
        },
        complete: function () {
        }
    });


}


function chkAll(all) {

    var cb = document.getElementsByName("chkFile"); 
    var boo = false;

    if (all.checked) boo = true;
    for (var i = 0; i < cb.length; i++) {
        cb[i].checked = boo;
    }

}




function StringFormat() {
    var expression = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var prttern = "{" + (i - 1) + "}"
        expression = expression.replace(prttern, arguments[i]);
    }

    return expression;
}



//사용안함 -weeklytop으로 이동
function MemoViewEHR222222222(userID, weeklyUserID) {
    var para = new Object();
    para.userid = encodeURIComponent(weeklyUserID);
    para.key = "EHRTIKLE";

    //첨부 목록
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Weekly/WeeklyListTeam.aspx/Encrypt",
        data: JSON.stringify(para),//{ "userid": userID,"aeskey":"EHRTIKLE" },
        dataType: "json",
        success: function (data) {
            //개발
            var url = 'http://erpsmd.sktelecom.com:51000//irj/servlet/prt/portal/prtroot/skt.pct.par.sap_r3.hr_ess.SSO_Bridge.EP';
            //운영
            //var url = 'http://ep.sktelecom.com:50000/irj/servlet/prt/portal/prtroot/skt.pct.par.sap_r3.hr_ess.SSO_Bridge.EP';
            //alert(data.d);

            url += '?target_name=CoachingDiary';
            url += '&domain_name=tnet.sktelecom.com';
            url += '&user_name=' + base64_encode(userID);
            //base64 일때만 아래 System=Approval 추가
            url += '&System=Approval';

            var todayDate = new Date();
            todayDate.setDate(todayDate.getDate() + 1);
            document.cookie = "HRTargetID=" + data.d + "; path=/; expires=" + todayDate.toGMTString() + "; domain=sktelecom.com"

            window.open(url, "weekly_SAP");
        },
        error: function (result) {
            if (result.status != 0)
                alert("Error" + ":::" + result);
        },
        complete: function () {

        }
    });

    //setCookie("HRTargetID", "Uzd2VGc5ZWRHMEZyR3B2VTFUTE5hQT09", 1);

    //var url = 'http://erpsmd.sktelecom.com:51000//irj/servlet/prt/portal/prtroot/skt.pct.par.sap_r3.hr_ess.SSO_Bridge.EP';
    ////var url = 'http://ep.sktelecom.com:50000/irj/servlet/prt/portal/prtroot/skt.pct.par.sap_r3.hr_ess.SSO_Bridge.EP';
    //url += '?target_name=CoachingDiary';
    //url += '&domain_name=tnet.sktelecom.com';
    //url += '&user_name=' + base64_encode(userID);
    ////url += '&theme_name=' + weeklyUserID;
    //url += '&System=Approval';
    //window.open(url, "weekly_SAP");
}


//H-hr연동시 사번 저장을 위한 setCookie
function setCookie(name, value, expiredays) {
    var todayDate = new Date();
    todayDate.setDate(todayDate.getDate() + expiredays);
    document.cookie = name + "=" + escape(value) + "; path=/; expires=" + todayDate.toGMTString() + ";"
}

function CoachyOpen() {
    $("#layer2").append("<div class='coachy_img'></div>");
    //timeoutTimer = setTimeout("testFunction()", 6000);    // 메모하기 문구 없애기
    //clearTimeout(timeoutTimer);
}

var _KeyStr = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=';
function base64_encode(input) {
    var output = '', chr1, chr2, chr3, enc1, enc2, enc3, enc4, i = 0;
    function _keyStrCharAt() {
        var ar = arguments, i, ov = '';
        for (i = 0; i < ar.length; i++) ov += _KeyStr.charAt(ar[i]);
        return ov;
    }
    function _utf8_encode(string) {
        string = string.replace(/\r\n/g, '\n');
        var utftext = '', c;
        for (var n = 0; n < string.length; n++) {
            var c = string.charCodeAt(n);
            if (c < 128)
                utftext += String.fromCharCode(c);
            else if ((c > 127) && (c < 2048))
                utftext += String.fromCharCode((c >> 6) | 192, (c & 63) | 128);
            else
                utftext += String.fromCharCode((c >> 12) | 224, ((c >> 6) & 63) | 128, (c & 63) | 128);
        }
        return utftext;
    }
    input = _utf8_encode(input);
    while (i < input.length) {
        chr1 = input.charCodeAt(i++);
        chr2 = input.charCodeAt(i++);
        chr3 = input.charCodeAt(i++);
        enc1 = chr1 >> 2;
        enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
        enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
        enc4 = chr3 & 63;
        if (isNaN(chr2)) enc3 = enc4 = 64;
        else if (isNaN(chr3)) enc4 = 64;
        output += _keyStrCharAt(enc1, enc2, enc3, enc4);
    }
    return output;
}

function base64_decode(input) {
    function _keyStrindexOfinputcharAt(p) { return _KeyStr.indexOf(input.charAt(p)); }
    function _utf8_decode(utftext) {
        var string = '', i = 0, c, c2, c3;
        while (i < utftext.length) {
            c = utftext.charCodeAt(i);
            if (c < 128) {
                string += String.fromCharCode(c);
                i++;
            } else if ((c > 191) && (c < 224)) {
                c2 = utftext.charCodeAt(i + 1);
                string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            } else {
                c2 = utftext.charCodeAt(i + 1);
                c3 = utftext.charCodeAt(i + 2);
                string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }
        }
        return string;
    }
    var output = '', chr1, chr2, chr3, enc1, enc2, enc3, enc4, i = 0;
    input = input.replace(/[^A-Za-z0-9\+\/\=]/g, '');
    while (i < input.length) {
        enc1 = _keyStrindexOfinputcharAt(i++);
        enc2 = _keyStrindexOfinputcharAt(i++);
        enc3 = _keyStrindexOfinputcharAt(i++);
        enc4 = _keyStrindexOfinputcharAt(i++);
        chr1 = (enc1 << 2) | (enc2 >> 4);
        chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
        chr3 = ((enc3 & 3) << 6) | enc4;
        output += String.fromCharCode(chr1);
        if (enc3 != 64) output += String.fromCharCode(chr2);
        if (enc4 != 64) output += String.fromCharCode(chr3);
    }
    output = _utf8_decode(output);
    return output;
}
// Weekly 내용이 없을 경우 상단에 선택한 사람의 정보를 보여줍니다.
function fnGetUser2(WeeklyUserID) {

    // 작성한 Weekly가 없을경우
    var dHtml = "";
    dHtml += '<li style="padding-top:5px !important;">';
    dHtml += "<table style='width:100%;cellspacing='0' cellpadding='0'><tr><td style='padding:0;margin:0;'>";


    var display = "";
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Monthly/MonthlyListTeam.aspx/GetUserInfo", // 2016-03-07 노창현 Weekly -> Monthly
        data: "{weeklyUserID : '" + WeeklyUserID + "'}",
        dataType: "json",
        success: function (data) {
            var Table = data.d.Table;
            // 팀원,팀장 - 기간별
            if ($('.btnB.on').text() == "기간별") {
                display = Table[0].DeptName + ' / ' + Table[0].Name;
            }
                // 팀원,팀장 - 작성자별
            else if ($('.btnB.on').text() == "작성자별") {
                display = Table[0].Name + ' ' + Table[0].PositionName;
            }
                // 임원 - 조직도
            else {
                display = Table[0].DeptName;
            }
        },
        error: function (result) {
            if (result.status != 0) {
                alert("Error " + ":::" + result);
            }
        },
        complete: function () {
            dHtml += '<p class="date" style="text-align:left;">' + display + '</p>';
            dHtml += '<td></tr></table>';
            dHtml += '</li>';
            $('.weekly-text').prepend(dHtml);
        }
    });

}