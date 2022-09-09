var timeoutTimer;
var tempWeeklyId = "";

//tab menu
$(function () {
    //$("ul.tabs  li:first-child").addClass("tabs_on");
    //$("ul.mtabs li:first-child").addClass("mtabs_on");
    $(".tab_content").hide();
    $(".tab_content:first").show();

    $("ul.tabs li").click(function () {
        $("ul.tabs li").removeClass("tabs_on");
        $(this).addClass("tabs_on");
        $(".tab_content").hide();
        var activeTab = $(this).attr("rel");
        $("#" + activeTab).fadeIn();
    });
});


// 최신작성준 아코디언
$(function () {
    var allPanels = $('.accordion > dd').hide();
    //$('.accordion > dd:first-of-type').show();
    //$('.accordion > dt:first-of-type').addClass('accordion-active');
    //alert(tempWeeklyId);

    jQuery('.accordion > dt').on('click', function () {
        $this = $(this);
        if ($this.data('empty') == 'Y') return;
        //Mr.No
        //alert($this.data('weeklyid'));
        //var ff = $('.StandaloneView' + Number($this.data('weeklyid'))).contents().find('.type1').height();
        //alert(ff);


        $target = $this.next();
        
        if (!$this.hasClass('accordion-active')) {
            //$this.parent().children('dd').slideUp(); //slideUp / hide
            //jQuery('.accordion > dt').removeClass('accordion-active');
            $this.addClass('accordion-active');
            $target.addClass('active').slideDown();	 //slideDown / show  
            // Mr.No 2015-07-14 iframe
            var iframeHeight = $('.StandaloneView' + Number($this.data('weeklyid'))).contents().find('.type1').height();
            $('.StandaloneView' + Number($this.data('weeklyid'))).height(iframeHeight + 20); // Mr.No 2015-07-27 
        } else {
            //$this.parent().children('dd').slideUp(); //slideUp / hide
            //$('.accordion > dt').removeClass('accordion-active');
            $this.removeClass('accordion-active');
            $target.addClass('active').slideUp();
            
        }
        return false;
    });
});

//layer popup2
function layer_open(el) {
    
    //2015.08.10 zz17779 : 팀장/임원 부재시 위임관련 팝업 layer_absence 추가
    $('#layer1, #layer2, #layer3, #layer4, #pop_email, #layer_absence, #pop_email_mailorgchart,#GatheringPop').css('display', 'none'); //팝업 레이어 n개수

    var temp = $('#' + el);

    var bg = temp.parents('bg');
    // MostiSoft - Weekly Note에서만 배경 밝게 보이게 하기 위해 .bg || .bg2로 클래스를 설정.
    var bg2 = $('.bg').length == 0 ? $('.bg2') : $('.bg');

    if (el == "layer2") {
        bg2.removeClass('bg');
        bg2.addClass('bg2');
        $("input:checkbox[id='chkMailShare']").attr("checked", false);
    } else {
        bg2.removeClass('bg2');
        bg2.addClass('bg');
    }


    if (bg) {
        $('.layer').show();
    } else {
        temp.show();
    }

    temp.css('display', 'block');
    $('.btnArea.t_center').css('display', 'block');

    // MostiSoft - 
    // Weekly Note일 경우 하단 / 좌측 에서 window 기준 5% 씩 떨어져서 보이도록 설정.
    // 5% 떨어져서 팝업이 모두 보이지 않을 때에는 0px 떨어져서 보이도록 설정.
    if (el == "layer2") {
        if ($.cookie("layer2") == "Y") {
            $("#layer2 .coachy_img").hide();
            $('#divRemark').hide();
        } else {
            CoachyOpen();
            $('#divRemark').show();
        }
        var iHeight = $(window).height() * 0.05;
        var iWidth = $(window).width() * 0.05;
        // window의 height가 팝업 길이 + window height의 5% 보다 클 때 
        if (temp.outerHeight() + iHeight < $(window).height()) {
            temp.css('top', 'auto');
            temp.css('bottom', '5%');
            temp.css('margin-top', '0px');
        }
            // window의 height가 팝업 길이 + window height의 5% 보다 작을 때 
        else {
            temp.css('top', '0px');
            temp.css('margin-top', '0px');
            temp.css('bottom', 'auto');
        }

        // window의 width가 팝업 너비 + window width의 5% 보다 클 때 
        if (temp.outerWidth() + iWidth < $(window).width()) {
            temp.css('left', '5%');
            temp.css('margin-left', '0px');
        }
            // window의 width가 팝업 너비 + window width의 5% 보다 작을 때 
        else {
            temp.css('left', '0px');
            temp.css('margin-left', '0px');
        }
    } else {

        if (temp.outerHeight() < $(document).height()) temp.css('margin-top', '-' + temp.outerHeight() / 2 + 'px');
        else temp.css('top', '0px');
        if (temp.outerWidth() < $(document).width()) temp.css('margin-left', '-' + temp.outerWidth() / 2 + 'px');
        else temp.css('left', '0px');
    }

    temp.find('a.cbtn').click(function (e) {
        if (bg) {
            $('.layer').hide();
            clearTimeout(timeoutTimer);
        } else {
            temp.hide();
            clearTimeout(timeoutTimer);
        }
        e.preventDefault();
    });

    /*
       Author : 개발자-최현미C, 리뷰자-진현빈D
       Create Date : 2016.05.11 
       Desc : 레이어팝업 영역에 따른 자동 닫기 금지
    */
    $('.layer .bg').click(function (e) {
        //$('.layer').hide();
        clearTimeout(timeoutTimer);
        e.preventDefault();
    });

    // MostiSoft - bg2 클릭시에도 팝업 꺼지도록 추가.
    $('.layer .bg2').click(function (e) {
        //$('.layer').hide();
        clearTimeout(timeoutTimer);
        e.preventDefault();
    });
}

// coachy mark 
$(function () {
    $("#coachy").click(function () {
        $("#layer2").append("<div class='coachy_img'></div>");
        timeoutTimer = setTimeout("testFunction()", 6000);
        //clearTimeout(timeoutTimer);
    });
});

function testFunction() {
    $(".coachy_img").fadeOut("slow", function () {
        $(".coachy_img").remove();
    });
}


