define(function (require, exports, module) {
	var $  = require('jquery');
	var a = require('sub');
	require('plus')($);

	var defaultInput = $("input[data-id=defaultInput]");
	var defaultInputLength = defaultInput.length;

	$("#submitBtn").click(function () {
		var isSubmit = false;

		$("input.text").each(function() {
			var that = $(this);
			if(that.attr('data-id') == 'defaultInput'){
				var thatId = that.attr('id');
				var maxId = thatId.replace(/default/, 'max');
				var minId = thatId.replace(/default/, 'min');
				var thatVal = Number(that.val());
				var maxVal = Number($("#" + maxId).val());
				var minVal = Number($("#" + minId).val());
				if(that.val() == ''){
					that.focus();
					that.myxTips({
						content: '賠率不能為空！'
					});
					isSubmit = false;
					return false;
				}else if( thatVal > maxVal ){
					that.focus();
					that.myxTips({
						content: '開盤賠率不能大於A盤上限！'
					});
					isSubmit = false;
					return false;
				}else if(thatVal < minVal){
					that.focus();
					that.myxTips({
						content: '開盤賠率不能小於A盤下限！'
					});
					isSubmit = false;
					return false;
				}else{
					isSubmit = true;
				}
			}else{
				if(that.val() == ''){
					that.focus();
					that.myxTips({
						content: '賠率不能為空！'
					});
					isSubmit = false;
					return false;
				}else{
					isSubmit = true;
				}
			}
		});
		// for(var i=0; i<defaultInput.length; i++){
		// 	var that = defaultInput.eq(i);
		// }
		if(isSubmit){
            a.setAjaxLoading();
            $.ajax({
                url: '?',
                type: 'POST',
                cache: false,
                dataType: 'html',
                timeout: 5000,
                async: true,
                data: $('#form').serialize(),
                success: function (d) {
                    // console.log(d)
                    a.removeAjaxLoading();
                    $("#alertWrap").html(d);
                },
                error: function () {
                    a.removeAjaxLoading();
                }
            });
		}
	});
});