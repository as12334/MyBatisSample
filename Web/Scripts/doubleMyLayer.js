define(function(require, exports, module) {

	var $  = require('jquery');
	// 引入myLayer插件
	require('myLayer')($);

	var dialog = top.dialog;

	if (!dialog) {
		dialog = require('dialog');
	}

	var aSrc = [];

	Array.prototype.indexOf = function(val) { for (var i = 0; i < this.length; i++) { if (this[i] == val) return i; } return -1; };
	Array.prototype.remove = function(val) { var index = this.indexOf(val); if (index > -1) { this.splice(index, 1); } };



	$("body").delegate('.groupshow', 'click', function () {
		var src = $(this).attr('data-href');
		if (aSrc.indexOf(src) == -1) {

				dialog({
					title: "分組明細",
					url: src,
					fixed: true,
					onshow: function () {
						aSrc.push(src);
					},
					onclose: function () {
						aSrc.remove(src);
					}
				}).showModal();
				
			// window.parent.$('body').myLayer({
			// 	title: '分組明細',
			// 	isMiddle: true,
			// 	isShowBtn: false,
			// 	url: src,
			// 	openCallBack: function () {
			// 		aSrc.push(src);
			// 	},
			// 	closeCallBack: function () {
			// 		aSrc.remove(src);
			// 	}
			// });
		}

		return false;
	});
	
});
