<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommCommentControl.ascx.cs" Inherits="SKT.Glossary.Web.Common.Controls.CommCommentControl" %>
<script type="text/javascript">
<!--
    var commCommentType = "<%=commType%>";
    var commCommentIdx = "<%=commIdx%>";
    var commCommentuserID = "<%=UserID%>";
    var commCommentBest = "<%=commBest%>";

    //P097010 BACKUP2
    var commGatheringID = "<%=GatheringID%>";
    var commGatheringYN = "<%=GatheringYN%>";

    var commCommentTotalCnt = 0;
    var commCommentpageNum = $("#commCommentCurrPageNum").val(); //페이지
    var commCommentpageSize = 10; //가져올개수

    var editBool = true;
    var binTextArea = "당신의 소중한 댓글을 기다립니다. 이곳을 클릭 하세요.";
    var CommCommentDefaultBtnSettingText = "댓글";

    var SendYN = "<%= SendYN%>";

    //댓글목록 가져오기(페이징 포함)
    function commentList(pg) {
        $("#commCommentCurrPageNum").val(pg);
        commCommentpageNum = $("#commCommentCurrPageNum").val(); //페이지
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Common/Controls/CommCommentAjax.aspx/CommCommentListSelectWeb",
            data: "{commType : '" + commCommentType + "', commIdx : '" + commCommentIdx + "',userID : '" + commCommentuserID + "', pageNum :" + commCommentpageNum + ", pageSize :" + commCommentpageSize + "}",
            dataType: "json",
            success: function (data) {
                var Table = data.d.Table;
                commCommentList(Table, commCommentType, commCommentuserID, commCommentpageNum, commCommentpageSize);
                commCommentpageNum++;
                $("#commCommentCurrPageNum").val(commCommentpageNum);
            },
            error: function (result) {
                
                alert("Error" + ":::" + result);


            }
       
        });
    }

    //댓글목록 표시
    function commCommentList(table, type, uid, page, pageSize)
    {
        $("#commCommentUl").html("");
        var dHtml = "";
        //가져온개수가 있다면
        if (table.length > 0) {
            for (var i = 0; i < table.length; i++) {
                dHtml += commCommentLiCreate(table[i], uid, type);
            }
        }
        
        $("#commCommentUl").append(dHtml);

        commCommentChange(commCommentType, commCommentIdx);
        
    }

    //댓글하나 만들기
    function commCommentLiCreate(obj,uid,type) {
        var rtn = '';
        if (obj.ID == obj.SUP_ID) {
            rtn += '<li class="my" id="commComment_' + obj.ID + '">';//<!--내가 작성한 글일 경우 클래스삽입-->
        } else {
            if (obj.USERID == uid) {
                rtn += '<li class="my reply" id="commComment_' + obj.ID + '">';//<!--내가 작성한 글일 경우 클래스삽입-->
            } else {
                rtn += '<li class="reply" id="commComment_' + obj.ID + '">';//<!--내가 작성한 글일 경우 클래스삽입-->
            }
            
        }
        if (obj.ID == obj.SUP_ID) {
            rtn += '	<img src="' + obj.PHOTOURL + '" alt="" onerror="this.src=\'/common/images/user_none.png\'"/>';
        }
        rtn += '	<dl>';
        if (obj.PUBLICYN == 'Y') {
            rtn += '		<dt>' + obj.USERNAME + '<time>' + obj.CDATE + '</time></dt>';
        }else{
            rtn += '		<dt>' + obj.USERNAME + '<br />' + obj.DEPTNAME + '<time>' + obj.CDATE + '</time></dt>';
        }
        rtn += '		<dd id="commCommentContent_' + obj.ID + '">';
       
        rtn += '		<img src="/common/images/icon/best.png" alt="BEST" '
        if (obj.BESTREPLYYN == "Y") {
            rtn += 'style="display:block"';
        } else {
            rtn += 'style="display:none"';
        }
        rtn += ' id="commCommentBestImg_' + obj.ID + '" />';
        
        rtn += obj.CONTENTS;
        rtn += '        </dd>';
        rtn += '	</dl>';

        if (obj.USERID == uid) {
            rtn += '	<p>';
            rtn += '		<a href="javascript:commCommentView(' + obj.ID + ')" class="btn1"><span>편집하기</span></a>';
            rtn += '		<a href="javascript:commCommentDelete(' + obj.ID + ')" class="btn1"><span>삭제하기</span></a>';

            if (commCommentType == "Glossary" && commGatheringYN != "Y") {
                if (commGatheringID != "") {
                    if (obj.ID == obj.SUP_ID) {
                        rtn += '		<a href="javascript:fnCommentReply(' + obj.ID + ')" class="btn1" id="commCommentReplyBtn_' + obj.ID + '"><span>답글</span></a>';
                    }
                }
            }
            rtn += '	</p>';
        } else if (obj.ID == obj.SUP_ID) { 

            rtn += '	<p>';
            rtn += '		<a href="javascript:commCommentLike(' + obj.ID + ')" class="btn1"><span id="commCommentLike_' + obj.ID + '">추천(' + obj.LIKECOUNT + ')</span></a>';
            //if (obj.REPLYCNT == 0 || obj.ID != obj.SUP_ID ) {

            if (commCommentType == "Glossary" && commGatheringYN != "Y") {
                rtn += '		<a href="javascript:fnCommentReply(' + obj.ID + ')" class="btn1" id="commCommentReplyBtn_' + obj.ID + '"><span>답글</span></a>';
            }
            //}
            if (commCommentBest == "Y") {
                if (obj.BESTREPLYYN != "Y") {
                    rtn += '		<a href="javascript:commCommentLikeBest(' + obj.ID + ')" class="btn1 bestBtnCss" id="commCommentBestBtn_' + obj.ID + '"><span>채택</span></a>';
                }
            } 
            rtn += '	</p>';

        }
        rtn += '</li>';
       
        return rtn;
    }

    //작성자비공개 클릭 시
    function fnCheck(id) {
        if ($('#btnPrivate'+id)[0].checked == true) {
            if (!confirm('익명으로 질문을 작성하시면 \n작성자 정보가 저장되지 않아 \n이후 수정이 불가능합니다.\n그래도 익명으로 작성하시겠습니까?') == false) {
                return true;
            } else {
                $("input[id=btnPrivate"+id+"]:checkbox").attr("checked", false);
            }
        }

    }
       
    //댓글 등록하기
    function commCommentSave(Id) {
        var Comment = {};
        Comment.Contents = $('#reply_textarea').val().replace(/'/g, "`").replace(/"/g, "`");

        /*
        Author : 개발자-김성환D, 리뷰자-진현빈D
        Create Date : 2016.06.01
        Desc : 끌지식 댓글 작성시 동일인(최초/최종작성자) 체크
        */
        if ($("#btnPrivate")[0].checked == true) {
            Comment.PublicYN = "Y";
        } else {
            Comment.PublicYN = "N";
        }
        if ($.trim($("#reply_textarea").val()) == '' || $("#reply_textarea").val() == binTextArea || $('#reply_textarea').val().replace(/^\s+|\s+$/g, '') == "") {
            alert(CommCommentDefaultBtnSettingText + ' 내용을 입력하세요');
            $("#reply_textarea").focus();
            $("#reply_textarea").val('');
        } else {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Common/Controls/CommCommentAjax.aspx/GlossaryCommCommentSaveWeb",
                //P097010 BACKUP2
                data: "{commType : '" + commCommentType + "', commIdx : '" + commCommentIdx + "',userID : '" + commCommentuserID + "', contentText :'" + Comment.Contents + "', PublicYN :'" + Comment.PublicYN + "', idx :'" + Id + "', SendCheck :'" + SendYN + "', GatheringYN :'" + commGatheringYN + "', GatheringID :'" + commGatheringID + "'}",
                //data: "{commType : '" + commCommentType + "', commIdx : '" + commCommentIdx + "',userID : '" + commCommentuserID + "', contentText :'" + Comment.Contents + "', PublicYN :'" + Comment.PublicYN + "', idx :'" + Id + "', SendCheck :'" + SendYN + "'}",
                dataType: "Text",
                success: function (data) {
                    $("#commCommentUl").html("");
                    $("#reply_textarea").val(binTextArea);
                    commentList(1);
                    //alert("저장 되었습니다.");
                    
                    $("#commCommentSaveBtn").attr('onclick', 'commCommentSave(0);');
                },
                error: function (response, textStatus, errorThrown) {
                    alert('댓글 저장 오류:' + response + ':' + textStatus + ':' + errorThrown);
                    return;
                }
            });
        }
        $("input[id=btnPrivate]:checkbox").attr("checked", false);
    }

    //댓글삭제
    function commCommentDelete(id) {
        if (confirm(CommCommentDefaultBtnSettingText + "을 삭제하시겠습니까?")) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Common/Controls/CommCommentAjax.aspx/CommCommentDeleteWeb",
                data: "{commType : '" + commCommentType + "', commIdx : '" + commCommentIdx + "',userID : '" + commCommentuserID + "', idx :'" + id + "'}",
                dataType: "Text",
                success: function (data) {
                    $("#commCommentUl").html("");
                    commentList(1);
                   // alert("삭제 되었습니다.");
                },
                error: function (response, textStatus, errorThrown) {
                    alert('댓글 저장 오류:' + response + ':' + textStatus + ':' + errorThrown);
                    return;
                }
            });
        }
    }
    
    //댓글정보가져오기
    function commCommentView(id) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Common/Controls/CommCommentAjax.aspx/CommCommentSelectWeb",
            data: "{commType : '" + commCommentType + "', commIdx : '" + commCommentIdx + "',userID : '" + commCommentuserID + "', idx :'" + id + "'}",
            dataType: "json",
            success: function (object) {
                $("#reply_textarea").val("");
                var obj = object.d.Table;
                var textAear = "";
                if (obj.length > 0) {
                    for (var i = 0; i < obj.length; i++) {
                        textAear = obj[i].CONTENTS;
                    }
                }
                $("#reply_textarea").val(textAear);
                $("#reply_textarea").focus();
                $("#commCommentSaveText").text(CommCommentDefaultBtnSettingText+"편집");
                $("#commCommentSaveBtn").attr('onclick', 'fnModify("' + id + '");');

            },
            error: function (response, textStatus, errorThrown) {
                alert('댓글 가져오기 오류:' + response + ':' + textStatus + ':' + errorThrown);
                return;
            }
        });
    }

    //댓글수정하기
    function fnModify(idx) {

        var Comment = {};
        Comment.Contents = $('#reply_textarea').val().replace(/'/g, "`").replace(/"/g, "`");

        /*
        Author : 개발자-최현미C, 리뷰자-진현빈D
        Create Date : 2016.06.01 
        Desc : 수정시 필수 입력 체크
        */
        if ($.trim($("#reply_textarea").val()) == '' || $("#reply_textarea").val() == binTextArea || $('#reply_textarea').val().replace(/^\s+|\s+$/g, '') == "") {
            alert(CommCommentDefaultBtnSettingText + ' 내용을 입력하세요');
            $("#reply_textarea").focus();
            $("#reply_textarea").val('');
            return;
        }

        if ($("#btnPrivate")[0].checked == true) {
            Comment.PublicYN = "Y";
        } else {
            Comment.PublicYN = "N";
        }
        
        /*
        Author : 개발자-김성환D, 리뷰자-이정선G
        Create Date : 2016.02.17 
        Desc : 댓글 수정시 비공개 ajax 처리 추가
        */
        var timestamp = $("#commComment_" + idx + ">dl>dt>time").text();
        
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Common/Controls/CommCommentAjax.aspx/CommCommentUpdateWeb",
            data: "{userID : '" + commCommentuserID + "', contentText :'" + Comment.Contents + "', PublicYN :'" + Comment.PublicYN + "', idx :'" + idx + "'}",
            dataType: "json",
            success: function (data) {
                var obju = data.Table;
                obju = data.d.Table;
                var textAear = "";
                if (obju.length > 0) {
                    for (var i = 0; i < obju.length; i++) {
                        if (obju[i].BESTREPLYYN == "Y") {
                            textAear += '		<img src="/common/images/icon/best.png" alt="BEST" />';
                        }

                        /*
                        Author : 개발자-김성환D, 리뷰자-이정선G
                        Create Date : 2016.02.17 
                        Desc : 댓글 수정시 비공개 ajax 처리 추가
                        */
                        if (Comment.PublicYN == "Y") {
                            $("#commComment_" + idx + ">dl>dt").html("비공개<time>" + timestamp + "</time>");
                            commentList(1);
                        }
                        textAear += obju[i].CONTENTS;
                    }
                }
                $("#commCommentContent_" + idx).html(textAear);
                $("#reply_textarea").val(binTextArea);
                $("#commCommentSaveBtn").attr('onclick', 'commCommentSave(0);');
                $("#commCommentSaveText").text(CommCommentDefaultBtnSettingText+"등록");

                if (commCommentType == "QnA") {
                    if (typeof (fnSelectReplyChange) == "function") {
                        fnSelectReplyChange();
                    }
                }
            },
            error: function (response, textStatus, errorThrown) {
                alert('댓글 수정 오류:' + response + ':' + textStatus + ':' + errorThrown);
                return;
            }
        });



    }

    //인원수 표시하는경우
    function commCommentChange(type, idx) {

        
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Common/Controls/CommCommentAjax.aspx/CommCommentCountWeb",
                data: "{commType : '" + type + "', commIdx : '" + idx + "'}",
                dataType: "json",
                success: function (object) {
                    $("#reply_textarea").val("");
                    var obj = object.d.Table;
                    var CommentCnt = 0;
                    var CommentUseCnt = 0;
                    var AUTHUSERCNT = 0;

                    if (obj.length > 0) {
                        for (var i = 0; i < obj.length; i++) {
                            CommentCnt = obj[i].CommentCnt;
                            CommentUseCnt = obj[i].CommentUseCnt;
                            AUTHUSERCNT = obj[i].AUTHUSERCNT;
                        }
                    }

                    commCommentTotalCnt = CommentCnt;

                    if (type == "Survey") {
                        $("#commCommentCount").html("총 <b>" + CommentCnt + "</b>건 <span>의견함 멤버 : <b>" + AUTHUSERCNT + "</b>명</span> <span>참여인원 : <b>" + CommentUseCnt + "</b>명</span>");
                     $("#commCommentCount").css("display", "block");
                    }

                    fnPageView(commCommentTotalCnt, --commCommentpageNum, commCommentpageSize);
                   
            },
                error: function (response, textStatus, errorThrown) {
                    alert('댓글 가져오기 오류:' + response + ':' + textStatus + ':' + errorThrown);
                    return;
                }
            });

        
       

    }

    //댓글 추천
    function commCommentLike(idx) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Common/Controls/CommCommentAjax.aspx/CommCommentLikeWeb",
            data: "{idx : '" + idx + "'}",
            dataType: "json",
            success: function (data) {
                var likeCnt = "N";
                var object = data.d.Table;
                if (object.length > 0) {
                    for (var i = 0; i < object.length; i++) {
                        likeCnt = object[i].DBFLAG;
                    }
                }

                if (likeCnt == "N") {
                    alert("이미 추천하셨습니다.");
                } else {
                    $("#commCommentLike_" + idx).text("추천(" + likeCnt + ")");
                }
            },
            error: function (response, textStatus, errorThrown) {
                alert('댓글 가져오기 오류:' + response + ':' + textStatus + ':' + errorThrown);
                return;
            }
        });


    }

    //댓글채택하기
    function commCommentLikeBest(idx) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Common/Controls/CommCommentAjax.aspx/CommCommentBestWeb",
            data: "{idx : '" + idx + "'}",
            dataType: "json",
            success: function (data) {
                var BestYN = "N";
                var object = data.d.Table;
                if (object.length > 0) {
                    for (var i = 0; i < object.length; i++) {
                        BestYN = object[i].DBFLAG;
                    }
                }
                
                if (BestYN == "Y") {
                    $("#commCommentBestImg_" + idx).css("display", "block");
                    $("#commCommentBestBtn_" + idx).css("display", "none");
                    
                    if (commCommentType == "QnA") {
                        
                        if (typeof (fnSelectReplyChange) == "function") {
                            fnSelectReplyChange();
                        }


                    }

                } else {
                    alert("채택에 실패하였습니다.");
                }
                
            },
            error: function (response, textStatus, errorThrown) {
                alert('댓글 가져오기 오류:' + response + ':' + textStatus + ':' + errorThrown);
                return;
            }
        });


    }

    //채택 버튼 삭제 처리
    function fnBestButtonHide() {
        $.each(".bestBtnCss", function () {
           // alert(this.id);
            this.hide();
        });
    }

    //답변글등록화면 제공
    function fnCommentReply(idx) {
        var r = '';
        r += '<fieldset id="commCommentRepy_' + idx + '">';
        r += '	<textarea id="commCommentRepyText_' + idx + '"></textarea>';
        r += '  <p>';
        r += '   <input type="checkbox" id="btnPrivate' + idx + '" onclick="fnCheck(' + idx + ');" /> <label for="btnPrivate' + idx + '">작성자 비공개</label>';
        r += '	 <a href="javascript:" class="btn2" onclick="fnCommentReplySave(' + idx + ')"><b>답글등록</b></a>';
        r += '  </p>';
        r += '</fieldset>';
       
       
        if ($("#commCommentRepy_" + idx).html() == undefined) {
            $("#commComment_" + idx).append(r);
        } else {
            $("#commCommentRepy_" + idx).remove();
            
        }
    }

    //답변글 등록처리
    function fnCommentReplySave(idx) {
        var Comment = {};
        Comment.Contents = $('#commCommentRepyText_' + idx).val().replace(/'/g, "`").replace(/"/g, "`");
        if ($("#btnPrivate" + idx)[0].checked == true) {
            Comment.PublicYN = "Y";
        } else {
            Comment.PublicYN = "N";
        }
        if ($.trim($('#commCommentRepyText_' + idx).val()) == '') {
            alert(CommCommentDefaultBtnSettingText+' 내용을 입력하세요');
            $('#commCommentRepyText_' + idx).focus();
            $('#commCommentRepyText_' + idx).val('');
        } else {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Common/Controls/CommCommentAjax.aspx/CommCommentSupSaveWeb",
                //P097010 BACKUP2
                data: "{commType : '" + commCommentType + "', commIdx : '" + commCommentIdx + "',userID : '" + commCommentuserID + "', contentText :'" + Comment.Contents + "', PublicYN :'" + Comment.PublicYN + "', idx :'" + idx + "' , GatheringYN :'" + commGatheringYN + "', GatheringID :'" + commGatheringID + "'}",
                //data: "{commType : '" + commCommentType + "', commIdx : '" + commCommentIdx + "',userID : '" + commCommentuserID + "', contentText :'" + Comment.Contents + "', PublicYN :'" + Comment.PublicYN + "', idx :'" + idx + "'}",
                dataType: "json",
                success: function (data) {
                    $("#commCommentRepy_" + idx).remove();

                    var obj = data.d.Table;
 
                    var li;
                    if (obj.length > 0) {

                        li = commCommentLiCreate(obj[0], commCommentuserID, commCommentType);
                    }
                    
                    $("#commComment_" + idx).after(li);
                    //$("#commCommentReplyBtn_" + idx).hide();
                    
                },
                error: function (response, textStatus, errorThrown) {
                    alert('댓글 저장 오류:' + response + ':' + textStatus + ':' + errorThrown);
                    return;
                }
            });
        }
    }

    //페이징호출
    function listPageing(p) {
        commentList(p);
    }

    //페이징 화면출력
    function fnPageView(toTalCnt,cuPg,cuPs) {
        var PG;
        if (toTalCnt > 0) {
            PG = new Paging(toTalCnt, cuPs);
            with (PG.config) {

                PG.config.methodYn = 'post';//method형식 get인지 post인지 여부 확인
                PG.config.intPage = (cuPg);//현재페이지번호
                PG.config.scriptname = 'listPageing';//post방식일때 사용할 스크립트명
                PG.config.pagePerView = 10	// 페이지당 네비게이션 항목수
                PG.config.itemPerPage = cuPs
                PG.config.showFirstLast = true; //맨처음,맨끝가기 표시
                firstIcon = '/common/images/btn/paging1.png';
                prevIcon = '/common/images/btn/paging2.png';
                nextIcon = '/common/images/btn/paging3.png';
                lastIcon = '/common/images/btn/paging4.png';
                numberFormat = ' %n ';
                thisPageStyle = "";
                otherPageStyle = "";
            }
        } else {
            PG = "";
        }

//alert(PG);
        $("#commCommentPageNavi").html(PG.toString());
        $("#reply_textarea").val(binTextArea);
       

    }

    $(document).ready(function () {
        /* 댓글 텍스트영역 */
        
        $("#reply_textarea").click(function () {
            if (binTextArea == $(this).val()) {
                $(this).val("");
            }
        });
        //목록 가져오기
        commentList(1);
    });

    function CommCommentDefaultSetting(commentInitValue){
        binTextArea = commentInitValue;
        $("#reply_textarea").val(binTextArea);
    };

    function CommCommentDefaultBtnSetting(btnInitValue) {
        if (btnInitValue == null || btnInitValue == "") {
            CommCommentDefaultBtnSettingText = "댓글";
        } else {
            CommCommentDefaultBtnSettingText = btnInitValue;
        }
        $("#commCommentSaveText").text(CommCommentDefaultBtnSettingText + "달기");
        binTextArea = "당신의 소중한 " + CommCommentDefaultBtnSettingText + "을 기다립니다. 이곳을 클릭 하세요.";
    };

	function fn_TextAreaInputLimit() {
        var tempText = $("#reply_textarea");
        var tempChar = "";                                        // TextArea의 문자를 한글자씩 담는다
        var tempChar2 = "";                                        // 절삭된 문자들을 담기 위한 변수
        var countChar = 0;                                        // 한글자씩 담긴 문자를 카운트 한다
        var tempHangul = 0;                                        // 한글을 카운트 한다
        var maxSize = 2000;                                        // 최대값
        // 글자수 바이트 체크를 위한 반복
        for(var i = 0 ; i < tempText.val().length; i++) {
            tempChar = tempText.val().charAt(i);
            // 한글일 경우 2 추가, 영문일 경우 1 추가
            if(escape(tempChar).length > 4) {
                countChar += 2;
                tempHangul++;
            } else {
                countChar++;
            }
        }
	     if((countChar-tempHangul) > maxSize) {
            alert("최대 글자수를 초과하였습니다.");
            tempChar2 = tempText.val().substr(0, maxSize-1);
            tempText.val(tempChar2);
        }

    }

    //댓글달기, 의견달기,답변달기

//-->
</script>
<script src="/common/Js/paging_comment.js"></script>
<input type="hidden" id="commCommentCurrPageNum" name="commCommentCurrPageNum" value="1" />
<div class="reply_area">

	<fieldset <%if(commWrite.Equals("N")){Response.Write("style='display:none'"); }%>>
		<%--<textarea id="reply_textarea" onkeyUp="fn_TextAreaInputLimit()"  ></textarea>--%>
        <textarea id="reply_textarea"></textarea>
		<p id="commCommentBtn">
			<input type="checkbox" id="btnPrivate" onclick="fnCheck('');" /> <label for="btnPrivate">작성자 비공개</label>
            <a href="javascript:" class="btn6" onclick="commCommentSave(0)" id="commCommentSaveBtn"><b id="commCommentSaveText">댓글달기</b></a>
		</p>
		<p class="tt" style="display:none" id="commCommentCount">총 <b>0</b>건 <span>의견함 멤버 : <b>0</b>명</span> <span>참여인원 : <b>0</b>명</span></p>
	</fieldset>
	<ul id="commCommentUl"></ul>
 	<p class="pagination" id="commCommentPageNavi"></p>
</div>
