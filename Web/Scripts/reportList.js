define(function (require, exports, module) {
	var $  = require('jquery');
    require('plus')($);
    // var qtip = require('qtip');

	$(".tip").hover(function () {
		var that = $(this);
		that.myxTips({
			content: '<h2 class="reportListTitle">' + that.attr('tips-title') +'</h2>'+ that.attr('tips'),
			ishide: false
		});
	},function () {
		$('#myxTips').remove();
	});

});
