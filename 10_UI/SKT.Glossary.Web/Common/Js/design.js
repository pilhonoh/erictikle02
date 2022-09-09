
$(function(){
	
	var lnbCrt1 = $("#gnb>li:nth-child(" + (lnbDep1) + ") a");
	if(lnbCrt1) lnbCrt1.addClass("active");

	/*gnb*/
	$("#gnb li a").hover(function(){
		$(this).children("img").stop().animate({ marginTop:-51},150);
	}, function(){
		if( $(this).hasClass("active") ) {
		} else { $(this).children("img").stop().animate({ marginTop:0},150); };
	});
	
	/*mymenu*/
	$("#header p a").click(function(){
		$($(this).attr("href")).fadeIn(200);
		return false;
	});
	$("#myMenu").mouseleave(function(){
		$(this).fadeOut(200);
	})
	/*pop*/
	$(".btn_pop").click(function(){
		$($(this).attr("href")).fadeIn(0);
		$(".pop").fadeIn(300);
		$(".pop .select").css("height","38px");
		return false;
	});
	$(".pop .close").click(function(){
		$(".pop").fadeOut(300);
		$(".layer_pop").css("display","none")
	});

});