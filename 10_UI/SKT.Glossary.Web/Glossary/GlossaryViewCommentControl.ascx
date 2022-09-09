<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GlossaryViewCommentControl.ascx.cs" Inherits="SKT.Glossary.Web.Glossary.GlossaryViewCommentControl" %>
    <script src="/common/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Common/js/jquery-ui.js" type="text/javascript"></script>
    <script src="/Common/js/jquery.cookie.js" type="text/javascript"></script>
    <script src="/FloatingMenu/js/json2.js" type="text/javascript"></script>

    <script language ="javascript" type="text/javascript">

    var CommonID = "<%= m_ItemID %>";
	var UserID = "<%= m_UserID %>";
	var UserName = "<%= m_UserName %>";

    //수정에 관하여
	var Modifytag = null;
	var CommentItemID = null;

	$(document).ready(function () {
	    /* 댓글 텍스트영역 */
	    $("#reply_textarea").val('˝상대방에 대한 배려와 존중은 SK텔레콤 구성원의 기본 자세입니다. 댓글을 작성할 준비가 되셨나요? 그럼, 이곳을 클릭 하세요 ^^˝');
	    $("#reply_textarea").one("focus", function () {
	        $(this).val("");
	        CommentItemID = null;
	    });

	    $("#Save_button").click(function () {
	        var Comment = {};
	        Comment.Contents = $('#reply_textarea').val();
	        Comment.ID = CommentItemID;
	        if ($("#btnPrivate")[0].checked == true) {
	            Comment.PublicYN = "Y";
	        } else {
	            Comment.PublicYN = "N";
	        }
	        if ($.trim($("#reply_textarea").val()) == '' || $("#reply_textarea").val() == '˝상대방에 대한 배려와 존중은 SK텔레콤 구성원의 기본 자세입니다. 댓글을 작성할 준비가 되셨나요? 그럼, 이곳을 클릭 하세요 ^^˝') {
	            alert('댓글 내용을 입력하세요');
	            $("#reply_textarea").focus();
	            $("#reply_textarea").val('');
	            CommentItemID = null;
	            return false;
	        }
	        $('#reply_textarea').val('');
	        fnAddComment(Comment);
	        return false;
	    });
	});


	//답글을 서버에 저장하고, 화면에 표시한다.
	function fnAddComment(commentIN, addWhere) {

	    // 14062 ljm url 추가

	    $.ajax({
	        type: "POST",
	        url: location.href + "&reply=Y",
	        data: { AJAX_METHOD: "INSERT", Comment: JSON.stringify(commentIN) },
	        dataType: "json",
	        success: function (Comment) {
	            if (CommentItemID != null) {
	                Modifytag.find("dl dd")[0].innerHTML = Comment.Contents;
	                $("#Save_button b")[0].innerText = "댓글 등록";
	                $("#CheckYN").show();
	                CommentItemID = null;
	                $("#reply_textarea").val('˝상대방에 대한 배려와 존중은 SK텔레콤 구성원의 기본 자세입니다. 댓글을 작성할 준비가 되셨나요? 그럼, 이곳을 클릭 하세요 ^^˝');
	                $("#reply_textarea").one("focus", function () {
	                    $(this).val("");
	                });
	            } else {
	                var s = '';
	                s += '<li>';
	                s += '<img alt="' + Comment.UserName + '/' + Comment.DeptName + '" src="' + Comment.PhotoUrl + '">';
	                s += '<dl>';
	                s += '<dt>';
	                s += '<a href="javascript:" onclick="fnProfileCommontView(' + "'" + Comment.UserID + "'" + ');">' + Comment.UserName + '/' + Comment.DeptName + '</a>';
	                s += '<time>' + Comment.CreateDate + '</time>';
	                s += '</dt>';
	                s += '<dd>' + Comment.Contents.replace(/\n/gi, "<br />") + '</dd>';
	                s += '</dl>';
	                s += '<p>';
	                if (Comment.UserName != '비공개') {
	                    s += '<a class="btn_s" onclick=\"return fnModify(this, ' + "'" + Comment.ID + "'" + ');\" href=\"javascript:\"><b>편집</b></a>';
	                }
	                s += '</p>';
	                s += '</li>';
	                $('#CommentAdd').prepend(s);
	            }
	            $('li.delete').hide();
	        },

	        error: function (result) {
	            alert('댓글 저장 오류:' + result);
	            return;
	        }
	    });
	    $("input[id=btnPrivate]:checkbox").attr("checked", false);
                                         //end ajax
	}

	function fnBestsComment() {
	    var s = '';
	    s += '<div class="qna-comment-view" style="margin-top:0px; class="qna-comment-view-tb">';
	    s += '<table width="100%" border="0" cellpadding="0" cellspacing="0" class="qna-comment-view-tb">';
	    s += '<tbody>';
	    s += '<tr class="best">';
	    s += BestComment.replace('<SPAN class=rating>추천', '<SPAN class=qna-best>베스트').replace('<span class="rating">추천', '<span class="qna-best">베스트');
	    s += '</tr>';
	    s += '</tbody>';
	    s += '</table>';
	    s += '</div>';
	    $('#CommentBestAdd').prepend(s);
    }


    //좋아요 추가
    function fnCommentLike(thistag, ItemID, BestYN) {
        $.ajax({
            type: "POST",
            url: location.href,
            data: { AJAX_METHOD: "Like", CommentItemID: ItemID },
            success: function (dup) {
                if (dup == 'Y') {
                    var CommenLikeCount = $(thistag).find("span")[0].innerText;
                    //BestComment = $(thistag)[0].parentNode.parentNode.innerHTML;
                    if (BestYN == "N") {
                        $(thistag).find("span")[0].children[0].outerText = parseInt($(thistag).find("span")[0].children[0].outerText) + 1;
                        if (CommenLikeCount == 2) {
                            //fnBestsComment();
                            $(thistag).find("b")[0].innerText = "추천" + (parseInt(CommenLikeCount) + 1).toString() + "개";
                            $("." + ItemID + "")[0].innerText = (parseInt(CommenLikeCount) + 1).toString();
                        }
                        if (CommenLikeCount >= 3) {
                            $(thistag).find("b")[0].innerText = "추천" + (parseInt(CommenLikeCount) + 1).toString() + "개";
                            $("." + ItemID + "")[0].innerText = (parseInt(CommenLikeCount) + 1).toString();
                        }
                    } else {
                        if (BestYN == "Y") {
                            $(thistag).find("b")[0].innerText = "추천" + (parseInt(CommenLikeCount) + 1).toString() + "개";
                            $("." + ItemID + "")[0].innerText = (parseInt(CommenLikeCount) + 1).toString();

                        }
                    }
                }
                else {
                    alert("이미 추천하였습니다.");
                }
            },
            error: function (result) {
                alert('추천오류:' + result);
                return;
            }
        });                        //end ajax
    }

	//프로필 화면 이동
	function fnProfileCommontView(UserID) {
	    location.href = "/GlossaryMyPages/MyProfile.aspx?UserID=" + UserID;
	}

	//댓글 수정
	function fnModify(thistag, ID) {
	    //	    var $litag = $('div div tr').has(thistag).find("td")[2].innerText;
	    var $litag = $('div div li').has(thistag);
	    Modifytag = $litag;
	    $("#Save_button b")[0].innerText = "편집 완료";
	    $("#CheckYN").hide();
	    $.ajax({
	        type: "POST",
	        url: location.href,
	        data: { AJAX_METHOD: "SELECT", CommentItemID: ID },
	        dataType: 'json',
	        success: function (comment) {
	            $("#reply_textarea").focus();
	            $("#reply_textarea").val(comment.Contents);
	            if (comment.PublicYN == "Y") {
	                $("#btnPrivate")[0].checked = true;
	            } else {
	                $("#btnPrivate")[0].checked = false;
	            }
	            CommentItemID = ID;

	        },
	        error: function (result) {
	            alert('수정 삭제 오류:' + result);
	        }
	    });
	}

	function fnCheck() {
	    if ($('#btnPrivate')[0].checked == true) {
	        if (!confirm('익명으로 댓글을 작성하시면 \n작성자 정보가 저장되지 않아 \n이후 수정이 불가능합니다.\n그래도 익명으로 작성하시겠습니까?') == false) {
	            return true;
	        } else {
	            $("input[id=btnPrivate]:checkbox").attr("checked", false);
	        }
	    }

	}

	function fnDeleteComment() {
	    return confirm('댓글을 정말 삭제하시겠습니까?');
	}
        

</script>
	<div class="reply_area">
		<fieldset>
			<textarea id="reply_textarea"></textarea>
			<p>
				<input type="checkbox" id="btnPrivate" onclick="fnCheck();" /> <label for="btnPrivate">작성자 비공개</label>
				<a href="javascript:" id="Save_button" class="btn_arrow arrow3"><b>댓글등록</b></a>
			</p>
		</fieldset>
        <ul id="CommentAdd" class="list1"></ul>
		<ul class="list1">
            <asp:Repeater ID="rptList" runat="server" OnItemDataBound="rptList_OnItemDataBound" >
		        <ItemTemplate>
				    <li>
					    <img src="<%# DataBinder.Eval(Container.DataItem, "PhotoUrl")%>" alt="<%# DataBinder.Eval(Container.DataItem, "UserName")%>/<%# DataBinder.Eval(Container.DataItem, "DeptName")%>" />
					    <dl>
						    <dt>
                                <asp:Literal runat="server" ID="itNameDept"></asp:Literal>
                                <asp:Literal ID="litUserInfo" runat="server"></asp:Literal>
                                <time><%# DataBinder.Eval(Container.DataItem, "CreateDate")%></time>
						    </dt>
						    <dd><%# DataBinder.Eval(Container.DataItem, "Contents")%></dd>
					    </dl>
					    <p>
                            <asp:Literal runat="server" ID="litLikeLink"></asp:Literal>
                            <asp:Literal runat="server" ID="itModify"></asp:Literal>
                            <asp:LinkButton ID="btnDeleteComment" Class="btn_s" runat="server" OnClientClick="return fnDeleteComment();" OnCommand="btnDeleteComment_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID")%>' CommandName="delete"><b>삭제</b></asp:LinkButton>
					    </p>
				    </li>
		        </ItemTemplate>
	        </asp:Repeater>
		</ul>
	</div>
       