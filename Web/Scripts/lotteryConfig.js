define(function (require, exports, module) {
	var $  = require('jquery');
    require('plus')($);
    // require('myLayer')($);
	var sub = require('sub');

	var dialog = top.dialog;
	// if (dialog) {
	// 	dialog = require('dialog');
	// }
	// console.log($("input[name=chk_lottery]:checked").length);
	setCheckBox();
	$("input[name=chk_lottery]").click(function () {
		setCheckBox();
	});

	function setCheckBox() {
		if ($("input[name=chk_lottery]:checked").length > 0 ) {
			$("#gotoList").attr('disabled', false);
		}else{
			$("#gotoList").attr('disabled', true);
		}
	}

	$("#viewLog").click(function () {
		var that = $(this);
		dialog({
			title: '查看日誌',
			url: that.attr('href'),
			width: $(window).width() * .9,
			fixed: false,
			onshow: function () {
			},
			onclose: function () {
			}
		}).showModal();
		// that.myLayer({
		// 	title: "查看日誌",
		// 	isMiddle: true,
		// 	isShowBtn: false,
		// 	url: that.attr('href')
		// });
		return false;
	});
	var avtiveLi = null;
	var nodeContent = function () {
        if ($(this).hasClass('optli')) {
	    	$(".claDown").attr('disabled', true)
	    	$(".claUp").attr('disabled', true)
	    	$(".claDown, .claUp").addClass('udd');
	    	$(this).removeClass('optli');
	    	avtiveLi = null;
        } else {
	    	$(".claDown").attr('disabled', false)
	    	$(".claUp").attr('disabled', false)
	    	$(".claDown, .claUp").removeClass('udd');
	    	avtiveLi = $(this);
	    	$(this).addClass('optli').siblings().removeClass('optli');//将选中的颜色设置为.optli，其余为原来
        }
        var length = $('#LCUL li').length;
        if($(this).index() == length - 1){
    		$(".claDown").addClass('udd');
        	$(".claDown").attr('disabled', true)
        }
        if ($(this).index() == 0) {
        	$(".claUp").addClass('udd');
    		$(".claUp").attr('disabled', true)
        }
    };
　　document.onkeydown = function(event){        //在全局中绑定按下事件  
　　　　var e  = event  ||  window.e;          
　　　　var keyCode = e.keyCode || e.which;
		if(avtiveLi != null){
			avtiveLi = $('#LCUL').find('.optli')
			switch(keyCode){
				case 40:
					avtiveLi.find('.claDown').click()
					break;
				case 38:
					avtiveLi.find('.claUp').click()
					break;
			}
		}
	}
    $('#LCUL').delegate('label','click',function (e) {
        e.stopPropagation();
        e.cancelBubble=true;
    });
    $('#LCUL').delegate('button','click',function (e) {
        e.stopPropagation();
        e.cancelBubble=true;
    });
    
    $('#LCUL').delegate('li', 'click', nodeContent);
	$('#LCUL').delegate('.claUp', 'click', function () {
        var index = $(this).parent();              //找到选中的li
        if (index.index() != 0) {   
    		index.find(".claDown, .claUp").removeClass('udd');
    		index.find(".claUp").attr('disabled', false)
    		index.find(".claDown").attr('disabled', false)
            var currentSort = (index.find('label').attr('sort'));
            var prevSort = (index.prev().find('label').attr('sort'));
            //alert("currentSort:" + currentSort + "；prevSort:" + prevSort);
            (index.find('label').attr('sort', prevSort));
            (index.prev().find('label').attr('sort', currentSort));
            index.prev().before(index);
        }else{
    		$(this).addClass('udd');
    		$(".claUp").attr('disabled', true)
        }
	});
	$('#LCUL').delegate('.claDown', 'click', function () {
        var index = $('#LCUL').find('.optli');
        var length = $('#LCUL li').length;
        if(index.index() == length - 1){
    		$(this).addClass('udd');
        	$(".claDown").attr('disabled', true)
        }
        if (index.index() != length - 1) { 
    		$(".claDown, .claUp").removeClass('udd');
    		$(".claUp").attr('disabled', false)                  //只要【下标值】不是最后一个都可以下移
        	$(".claDown").attr('disabled', false)
            var currentSort = (index.find('label').attr('sort'));
            var nextSort = (index.next().find('label').attr('sort'));
            //alert("currentSort:" + currentSort + "；nextSort:" + nextSort);
            (index.find('label').attr('sort', nextSort));
            (index.next().find('label').attr('sort', currentSort));
            index.next().after(index);
        }
	});
	$("#gotoList").click(function () {
		var a = []
		$('#LCUL li').each(function () {
			var id = $(this).find('label').attr('id').split('_')[1];
			var sort = $(this).find('label').attr('sort')
			a.push(id+','+sort)
		});
		$("#chk_lottery_sort").val(a.join('|'));
		sub.setAjaxLoading();
		$.ajax({
			url: '?',
			type: 'POST',
			cache: false,
			dataType: 'html',
			timeout: 5000,
			async: true,
			data: $('#saveForm').serialize(),
			success: function (d) {
					sub.removeAjaxLoading();
					$("#alert_show").html(d);
			},
			error: function () {
					sub.removeAjaxLoading();
			}
		});
	});

});
