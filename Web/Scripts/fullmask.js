define(function(require, exports, module) {

	var $  = require('jquery');
	// 全屏遮罩
	var fullmask = {
		topHeight: 100,
		// 获取最顶层Document
		top: top.document,
		// 设置遮罩层
		set: function () {
			var _this = this;
			var nowPageHeight = $(document).height() +  _this.topHeight;
			var _tmp = '<div id="fullmask" style="height:'+ nowPageHeight +'px;"></div>';
			$("body", _this.top).append( _tmp );
		},
		// 删除遮罩层
		remove: function () {
			var _this = this;
			$("fullmask", _this.top).remove();
		}
	};

	module.exports = fullmask;
});
