QueryString = function(str) { 
   var str = str ? str : document.location.href; 
   this.argv = new Array(); 
   //this.queryString = str.split('?')[1]; get방식을 사용하지 않으므로 삭제처리
   if (!this.queryString) this.queryString = ''; 
   var _argv = this.queryString.split('&'); 
   for(var i=0; i<_argv.length; i++) { 
      var $=_argv[i].split('='); 
      var _key = $[0]; 
      var _val = $[1]; 
      this.argv[_key] = $[1]; 
   } 

   if (!this.argv) this.argv = new Array(); 
   this.setVar = function(key,val,mYn,snm) {
	 
	  this.methodOk = mYn; // method방식
	  this.scriptnm = snm; // 스크립트명
      if (typeof key == 'object') { 
         for (var item in key) this.argv[item] = key[item]; 
      } else { 
         this.argv[key] = val; 
      } 
      return this.getVar(); 
   } 

   this.getVar = function(key) {
		 if (key) { 
			if (!this.argv[key]) return ''; 
			else { 
				return this.argv[key]; 
			} 
		 } else { 
			var cnt = 0; 

			for(var c in this.argv) cnt++;   // XXX: 키 이름을 가진 array 는 length 속성으로 항상 0 을 벹어낸다. 
				if (cnt > 0) { 
					var _item = new Array();
					for (var x in this.argv) if (this.argv[x]) _item[_item.length] = "javascript:"+this.scriptnm+"("+this.argv[x]+")";
				else continue; 
					if(this.methodOk == "post"){
						//post방식의 경우 무조건 첫번째값만 리턴함
						return _item[0];
					}
					return _item.join('&'); 
			}
		 } 
   } 
} 

Paging = function(total, itempage) { 
   this.config = { 
		pageVariable: 'page',
		scriptname: '',//post방식에서 사용할 스크립트 명
		intPage: 1,//몇번째 페이지인지 확인
		methodYn: '',//method방식값 post or get
		numberFormat: '[%n]',
		showFirstLast: true,	// 맨처음, 맨 마지막으로 가는 링크를 만들것인가.
		thisPageStyle: 'font-weight: bold;',
		otherPageStyle: 'font-size: 9pt',
		itemPerPage: itempage,	// 리스트 목록수
		pagePerView: 10,	// 페이지당 네비게이션 항목수
		prevIcon: null,	// 이전페이지 아이콘
		nextIcon: null,	// 다음페이지 아이콘
		firstIcon: null,	// 첫페이지로 아이콘
		lastIcon: null	// 마지막페이지 아이콘
   } 
	
   this.totalItem = total; 
   this.qs = new QueryString; 
   this.methodOk = this.config.pageVariable
	 
   this.calculate = function() { 
      this.totalPage = Math.ceil(this.totalItem / this.config.itemPerPage); 
      this.currentPage = this.qs.getVar('page'); 
	  
	  if(this.config.methodYn == "post")	this.currentPage = this.config.intPage;
	  
      if (!this.currentPage) this.currentPage = 1; 
      if (this.currentPage > this.totalPage) this.currentPage = this.totalPage; 
      this.lastPageItems = this.totalPage % this.config.itemPerPage; 

      this.prevPage = this.currentPage-1; 
      this.nextPage = this.currentPage+1; 
      this.seek = this.prevPage * this.config.itemPerPage; 
      this.currentScale = parseInt(this.currentPage / this.config.pagePerView); 
      if (this.currentPage % this.config.pagePerView < 1) this.currentScale--; 
      this.totalScale = parseInt(this.totalPage / this.config.pagePerView); 
      this.lastScalePages = this.totalPage % this.config.pagePerView; 
      if (this.lastScalePages == 0) this.totalScale--; 
      this.prevPage = this.currentScale * this.config.pagePerView; 
      this.nextPage = this.prevPage + this.config.pagePerView + 1; 
   } 

   this.toString = function() { 
		var ss, se;
		var firstBtn = '';
		var lastBtn = '';
		var prevBtn = '';
		var nextBtn = '';
		var pagingstr = '';

		this.calculate();

		if (this.config.showFirstLast) {
			firstBtn = '';
			if (this.config.firstIcon) firstBtn = '<img src="' + this.config.firstIcon + '" alt="" title="처음">';
			else firstBtn = '◀◀ ';
			firstBtn = firstBtn.link(this.qs.setVar(this.config.pageVariable,1,this.config.methodYn,this.config.scriptname));

			lastBtn = '';
			if (this.config.lastIcon) lastBtn = '<img src="' + this.config.lastIcon + '"  alt="" title="마지막">';
			else lastBtn = ' ▶▶';
			lastBtn = lastBtn.link(this.qs.setVar(this.config.pageVariable,this.totalPage,this.config.methodYn,this.config.scriptname));
			
		} else {
			firstBtn = lastBtn = '';
		}
		prevBtn = '';
		if (this.config.prevIcon) prevBtn = '<img src="' + this.config.prevIcon + '" alt="" title="이전">';
		else prevBtn = '◀&nbsp;';
		if (this.currentPage > this.config.pagePerView) {
			prevBtn = prevBtn.link(this.qs.setVar(this.config.pageVariable,this.prevPage,this.config.methodYn,this.config.scriptname));
		}
       
		ss = this.prevPage + 1;
		if ((this.currentScale >= this.totalScale) && (this.lastScalePages != 0)) se = ss + this.lastScalePages;
		else if (this.currentScale <= -1) se = ss;
		else se = ss + this.config.pagePerView;

		var navBtn = '';
		_btn = '';
		for (var i = ss; i < se; i++) {

			var pageText = i//this.config.numberFormat.replace(/%n/g,i);
			if (i == this.currentPage) {
				if (i == (se - 1)){
				    _btn = '<a href="javascript:" class="on">'+pageText+' </a> ';
				}else{
				    _btn = '<a href="javascript:" class="on">'+pageText+' </a> ';
				}
				
			} else {
				if (i == (se - 1)){
					_btn = '<a href="'+this.qs.setVar(this.config.pageVariable,i,this.config.methodYn,this.config.scriptname)+'" style="'+this.config.otherPageStyle+'" onfocus=blur()>'
				_btn += pageText+' </a> '
				}else{
					_btn = '<a href="'+this.qs.setVar(this.config.pageVariable,i,this.config.methodYn,this.config.scriptname)+'" style="'+this.config.otherPageStyle+'" onfocus=blur()>'
				_btn += pageText+' </a> '
				}
				
				
				
			}
			if(i != (se-1)){
					_btn += '&nbsp;'
			}
			navBtn+=_btn;
		}
		nextBtn = '';
		if (this.config.prevIcon) nextBtn = '<img src="' + this.config.nextIcon + '"  alt="" title="다음">';
		else nextBtn = '&nbsp;▶';
		if (this.totalPage > this.nextPage) {
			nextBtn = nextBtn.link(this.qs.setVar(this.config.pageVariable,this.nextPage,this.config.methodYn,this.config.scriptname));
		}
       /*
		if (this.totalItem > 0){
			pagingstr = '<TABLE border="0" cellspacing="0" cellpadding="0">'
			pagingstr += '  <tr>'
			pagingstr += '		<TD align="right">'
			pagingstr += firstBtn
			pagingstr += prevBtn
			pagingstr += '<span class="num">'
			pagingstr += navBtn
			pagingstr += '</span>'
			pagingstr += nextBtn
			pagingstr += lastBtn
			pagingstr += '		</td>'
			pagingstr += '  </tr>'
			pagingstr += '</table>'
		}else{
			pagingstr = '<table border="0" cellspacing="0" cellpadding="0">'
			pagingstr += '  <tr>'
			pagingstr += '	<TD align="right"></td>'
			pagingstr += '	<TD align="center"></td>'
			pagingstr += '	<TD></td>'
			pagingstr += '  </tr>'
			pagingstr += '</table>'
		}
        */
		
		pagingstr += " "+firstBtn;
		pagingstr += " "+prevBtn;
		pagingstr += '    <span>';
		pagingstr += navBtn
		pagingstr += '   </span>';
		pagingstr += nextBtn;
		pagingstr += " "+lastBtn;
        

		return pagingstr;
	}
	
}

