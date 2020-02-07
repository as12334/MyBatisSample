define(function (require, exports, module) {
	var $ = require('jquery');
	// require('myLayer')($);
	var dialog = top.dialog;
	var d = null;

	function delManageZJProfit(v, obj) {
		d = dialog({
			id: 'del',
			title: "提示信息",
			content: "是否確定刪除盈利設置？",
			fixed: true,
			cancelValue: '取消',
			cancel: function () {
					
			},
			okValue: '确定',
			ok: function () {
				submitForm(v);
			}
		}).showModal();
	}
	function submitForm(v) {
		$.ajax({
			type: 'POST',
			url: "?",
			data: "isdel=1&adid="+v,
			error: function () { alert('处理程序出错,请通知管理员检查！'); },
			success: function (msg) {
				d.close().remove();
				$("#alert_show").html(msg);
				location.reload();
			}
		});
	}

	$(".editBtn, #newsAdd").click(function () {
		var that = $(this);
		dialog({
			title: that.html(),
			url: '/ManageZJProfit/' + that.attr('href'),
			fixed: true
		}).showModal();
		return false;
	});

	$(".del").click(function () {
	    delManageZJProfit($(this).attr('data-adid'), $(this));
		return false;
	});

	$("#viewLog").click(function () {
	    var that = $(this);
	    dialog({
	        title: '查看盈利設置日誌',
	        url: '/ManageZJProfit/' + that.attr('href'),
	        fixed: true
	    }).showModal();
	    return false;
	});

});
