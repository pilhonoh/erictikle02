

$(document).ready(function(){	

	var text = $('.listTable td.al > a');
	$('.tooltip').hide();
	 text.css('cursor','pointer').mouseover(function(e){
	
		 var MTop = e.pageY;
		 var MLeft = e.pageX;
	  $('.tooltip').show().css({
	   'top':MTop+'px',
		'left':MLeft+'px'
	   });
	   $('.tooltip').text( $(this).text() );
	   

	  return false;
	 }).mouseout(function(){
	  $('.tooltip').hide();
 });

});















