//<![CDATA[
//메인 탭메뉴
$(document).ready(function() {		
	
	// 달력 초기화
	$("#idea3_1").datepicker();
	$("#idea4_1").datepicker();
	$("#idea3_2").datepicker();
	$("#idea4_2").datepicker();
});


// Idea 검색 클릭 시 레이어팝업
function noticeViewFunc(idx) {
	$.blockUI({ message: $('#popNoticeDiv'+idx), css:{border:"", margin:"-203px 0 0 -259px",  textAlign:"left"} });
}

// 달력 이미지 클릭 시
function calenderFunc(inputId) {
	$("#" + inputId).focus();
}
//]]>