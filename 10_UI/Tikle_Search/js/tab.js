//<![CDATA[
//메인 탭메뉴
$(document).ready(function() {		
	//////////profile_view 탭
		var li_sub_first = $(".search_tab2 > li");
		var li_sub_second = $(".profile_question");
		var li_sub_second2 = $(".profile_answer");
	
});

// profile 페이지 Q&A 클릭 시
function qnaViewFunc(type) {
	var questionImgPath = "/Supex/images/h_profile2_";
	var answerImgPath = "/Supex/images/h_profile3_";
	var suffix = ".gif";
	
	if( type == "Q" ) {
		$(".profile_answer").hide();
		$(".profile_question").show();
		
		$("#answer_all").hide();
		$("#question_all").show();
		
		$(".questionImg").attr("src", questionImgPath + "on" + suffix);
		$(".answerImg").attr("src", answerImgPath + "off" + suffix);
	} else {
		$(".profile_question").hide();
		$(".profile_answer").show();
		
		$("#question_all").hide();
		$("#answer_all").show();
		
		$(".questionImg").attr("src", questionImgPath + "off" + suffix);
		$(".answerImg").attr("src", answerImgPath + "on" + suffix);
	}
}

//]]>