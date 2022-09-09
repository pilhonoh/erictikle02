/* jQuery Nxeed's Tree Menu v1 | (c) 2014 Nxeed | https://github.com/nxeed */

(function($) {
    var defaults = {
        autoParentDetection: true,
        autoActiveDetection: true,
        itemsUniqueClasses: true,
        parentClass: 'parent',
        activeClass: 'active',
        selectedClass: 'selected',
        expandClass: 'opened',
        collapseClass: 'closed',
        spoilerButtonClickMinX: 0,
        spoilerButtonClickMaxX: 20,
        spoilerButtonClickMinY: 0,
        spoilerButtonClickMaxY: 40,
        slideEffect: false //2016.03.23 트리 접힐때 속도 문제로 true->false 로변경
    };

    var methods = {
        init: function (params) {           

            var options = $.extend({}, defaults, params);

            var items = this.find('li');

            $.each(items, function(num, item) {
                item = $(item);

                if (options.autoParentDetection) {
                    if (item.has('ul')[0]) {
                        item.addClass(options.parentClass);
                    }
                }

                if (options.itemsUniqueClasses) {
                    item.addClass('item-' + num);
                }

            });

            var parents = this.find('.' + options.parentClass);

            $.each(parents, function(num, parent) {
                parent = $(parent);

                parent.addClass(options.collapseClass);

                if (parent.has('.' + options.selectedClass)[0]) {
                    parent.removeClass(options.collapseClass).addClass(options.expandClass);

                    if (options.autoActiveDetection) {
                        parent.addClass(options.activeClass);
                    }
                }

                if (parent.hasClass(options.selectedClass)) {
                    parent.removeClass(options.activeClass).removeClass(options.collapseClass).addClass(options.expandClass);
                }
            });

            $('.' + options.collapseClass + ' > ul', this).hide();
            
            $('.' + options.parentClass + ' > a', this).click(function (e) {

                //2016-03-30, plus/minus 백그라운드 이미지가 존재하는 이벤트 객체에만 조직트리 collapse,expand 변화
                //2016-04-14, plus/minus 백그라운드 이미지가 존재하는 이벤트 객체의 id가 없는것이 존재해서 아래 내용 주석처리
                var $eventObj = $("#" + this.id);                
                var $eventObjCSS = $eventObj.css("background-image");

                if ($eventObjCSS != null) {
                    if ($eventObjCSS.indexOf("plus_icon.png".toLowerCase()) > -1 || $eventObjCSS.indexOf("minus_icon.png".toLowerCase()) > -1) {

                    }
                }
                        var posX = $(this).offset().left;
                        var posY = $(this).offset().top;

                        var clickX = e.pageX - posX;
                        var clickY = e.pageY - posY;

                        if (clickX <= options.spoilerButtonClickMaxX && clickX >= options.spoilerButtonClickMinX && clickY <= options.spoilerButtonClickMaxY && clickY >= options.spoilerButtonClickMinY) {
                            var item = $(this).parent('li');
                            var content = $(this).parent('li').children('ul');

                            if (item[0] != $(".weekly-treelist>li")[0]) {
                                item.toggleClass(options.expandClass).toggleClass(options.collapseClass);



                                if (options.slideEffect) {
                                    content.slideToggle();
                                } else {
                                    content.toggle();
                                }

                                e.preventDefault();

                                for (var l = 0; l < $('.weekly-treelist>li>ul li.opened').children('ul').children('li').length; l++) {
                                    if (($('.weekly-treelist>li>ul li.opened').children('ul').children('li').eq(l).width() < 10 + $('.weekly-treelist>li>ul li.opened').children('ul').children('li').eq(l).children('a').width() + $('.weekly-treelist>li>ul li.opened').children('ul').children('li').eq(l).children('span').width()) && $('.weekly-treelist>li>ul li.opened').children('ul').children('li').eq(l).children('span').text() != "") {
                                        $('.weekly-treelist>li>ul li.opened').children('ul').children('li').eq(l).addClass('d-Line');
                                    }
                                }
                            }
                        }
                
                

            });
        }
    };

    $.fn.ntm = function(method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Метод "' + method + '" не найден в плагине jQuery.ntm');
        }
    };
})(jQuery);
