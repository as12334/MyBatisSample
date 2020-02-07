 define(function (require, exports, module) {

	var $ = require('jquery');
	require('myLayer')($);

	$("#submitAction").click(function () {
		
		$(this).myLayer({
			isMiddle: true,
			isShowBtn: true,
			content: "是否確定清空微調？",
			okCallBack: function () {
				$.myLayer.close(true);
				$("#form").submit();
			}
		});
	});
 });