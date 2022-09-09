jQuery(function() { 
	$("#closeWrite").click(function() { 
		$(".unit_ideawrite").css("display","none");
	})
	$("#openWrite").click(function() { 
		$(".unit_ideawrite").css("display","block");
	})
	$("#closeWrite2").click(function() { 
		$(".unit_Questionwrite").css("display","none");
	})
	$("#openWrite2").click(function() { 
		$(".unit_Questionwrite").css("display","block");
	})
	
	var iText	= $(".item>.iLabel").next("iText");
	
	// Common
	var select_root = $('div.select');
	var select_value = $('.myValue');
	var select_a = $('div.select>ul>li>a');
	var select_input = $('div.select>ul>li>input[type=radio]');
	var select_label = $('div.select>ul>li>label');
	
	// Radio Default Value
	$('div.myValue').each(function(){
		var default_value = $(this).next('.iList').find('input[checked]').next('label').text();
		$(this).append(default_value);
	});
	
	// Line
	select_value.bind('focusin',function(){$(this).addClass('outLine');});
	select_value.bind('focusout',function(){$(this).removeClass('outLine');});
	select_input.bind('focusin',function(){$(this).parents('div.select').children('div.myValue').addClass('outLine');});
	select_input.bind('focusout',function(){$(this).parents('div.select').children('div.myValue').removeClass('outLine');});
	
	// Show
	function show_option(){
		$(this).parents('div.select:first').toggleClass('open');
	}
	
	// Hover
	function i_hover(){
		$(this).parents('ul:first').children('li').removeClass('hover');
		$(this).parents('li:first').toggleClass('hover');
	}
	
	// Hide
	function hide_option(){
		var t = $(this);
		setTimeout(function(){
			t.parents('div.select:first').removeClass('open');
		}, 1);
	}
	
	// Set Input
	function set_label(){
		var v = $(this).next('label').text();
		$(this).parents('ul:first').prev('.myValue').text('').append(v);
		$(this).parents('ul:first').prev('.myValue').addClass('selected');
	}
	
	// Set Anchor
	function set_anchor(){
		var v = $(this).text();
		$(this).parents('ul:first').prev('.myValue').text('').append(v);
		$(this).parents('ul:first').prev('.myValue').addClass('selected');
	}

	// Anchor Focus Out
	$('*:not("div.select a")').focus(function(){
		$('.aList').parent('.select').removeClass('open');
	});
	
	select_value.click(show_option);
	select_root.find('ul').css('position','absolute');
	select_root.removeClass('open');
	select_root.mouseleave(function(){$(this).removeClass('open');});
	select_a.click(set_anchor).click(hide_option).focus(i_hover).hover(i_hover);
	select_input.change(set_label).focus(set_label);
	select_label.hover(i_hover).click(hide_option);
	
	// Form Reset
	$('input[type="reset"], button[type="reset"]').click(function(){
		$(this).parents('form:first').find('.myValue').each(function(){
			var origin = $(this).next('ul:first').find('li:first label').text();
			$(this).text(origin).removeClass('selected');
		});
	});	
	
	// tab Design
	$(".unit_tab li:last").css("border-right", "1px solid #bfbfbf");
	var _loc	= $(".unit_tab li.on").children().children();
	if($("*").is(".unit_tab")) _loc.attr("src", _loc.attr("src").replace("_off", "_on"));
	
	// unit_Questionwrite
	$(".unit_questionwrite table th:first").css("padding-top", "25px");
	$(".unit_questionwrite table td:first").css("padding-top", "20px");
	$(".unit_questionwrite table th:last").css("padding-bottom", "20px");
	$(".unit_questionwrite table td:last").css("padding-bottom", "20px");
	

});