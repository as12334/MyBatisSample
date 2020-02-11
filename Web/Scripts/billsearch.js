define(function (require, exports, module) {
	var $  = require('jquery');
	require('plus')($);
	require('myLayer')($);
	require('wdatePicker');


	var dialog = top.dialog;
	var tipDialog = null;

	function setUrl(src, str) {
		var a = src.split("/");
		a[a.length-2] = str;
		return a.join('/');
	}

	var mySkin = '';
	// 绑定日期
	$("#txttime").click(function () {
		if (mySkin != top.skinPath && mySkin != '') {
			var skinLink = $('div[lang=zh-cn]', top.document).find('iframe').contents().find('link');
			skinLink.attr('href', setUrl(skinLink.attr('href'), top.skinPath));
		}
		mySkin = top.skinPath;
		WdatePicker({
			skin: top.skinPath
		});
	});

	// 加载列表
	function gotoList() {
		var sltLottery = $("#sltLottery").val();
		var txttime = $("#txttime").val();
		var sltPlay = $("#sltPlay").val();
		var sltIsBH = $("#sltIsBH").val();
		var sltUsertype = $("#sltUsertype").val();
		var txtUsername = $("#txtUsername").val();
		var txtAmountmin = $("#txtAmountmin").val();
		var txtAmountmax = $("#txtAmountmax").val();
		var txtResultmin = $("#txtResultmin").val();
		var txtResultmax = $("#txtResultmax").val();

		var src = "BillSearchList.aspx?lid=" + sltLottery + "&finddate=" + txttime + "&playid=" + sltPlay + "&isbh=" + sltIsBH + "&usertype=" + sltUsertype + "&username=" + txtUsername + "&amountmin=" + txtAmountmin + "&amountmax=" + txtAmountmax + "&resultmin=" + txtResultmin + "&resultmax=" + txtResultmax + "&page=1";


		// $("#gotoList").myLayer({
		// 	title: '註單搜索',
		// 	isMiddle: true,
		// 	isShowBtn: false,
		// 	url: src
		// });


		dialog({
			title: "註單搜索",
			url: src,
			width: $(window).width() * .8,
			fixed: true
		}).showModal();

	}

	// 绑定加载列表事件
	$("#gotoList").click(function () {
		if (isOk()) {
			gotoList();
		}
	});

	function isOk() {
		var isOk1 = false;
		var isOk2 = false;
		var txtAmountmin = $("#txtAmountmin");
		var txtAmountmax = $("#txtAmountmax");
		var txtResultmin = $("#txtResultmin");
		var txtResultmax = $("#txtResultmax");
		if ((txtAmountmin.val() == '' && txtAmountmax.val() != '') || (txtAmountmin.val() != '' && txtAmountmax.val() == '') || (txtAmountmin.val() != '' && txtAmountmax.val() != '')) {
			if (txtAmountmin.val() == '') {
				tipDialog && tipDialog.close().remove();
				tipDialog = dialog({
					title: "提示信息",
					content: "輸入有誤!",
					fixed: true,
					okValue: '确定',
					onremove: function () {
						txtAmountmin.focus();
					}
				}).showModal();

				isOk1 = false;
			}else if (txtAmountmax.val() == '') {
				tipDialog && tipDialog.close().remove();
				tipDialog = dialog({
					title: "提示信息",
					content: "輸入有誤!",
					fixed: true,
					okValue: '确定',
					onremove: function () {
						txtAmountmax.focus();
					}
				}).showModal();
				isOk1 = false;
			}else if (Number(txtAmountmin.val()) >= Number(txtAmountmax.val())) {
				tipDialog && tipDialog.close().remove();
				tipDialog = dialog({
					title: "提示信息",
					content: "註額範圍輸入有誤,第一個輸入值需小於第二個的值!",
					fixed: true,
					okValue: '确定',
					onremove: function () {
						txtAmountmin.focus();
					}
				}).showModal();
				isOk1 = false;
			}else {
				isOk1 = true;
			}
		}else{
			isOk1 = true;
		}
		if ((txtResultmin.val() == '' && txtResultmax.val() != '') || (txtResultmin.val() != '' && txtResultmax.val() == '') || (txtResultmin.val() != '' && txtResultmax.val() != '')) {
			if (txtResultmin.val() == '') {
				tipDialog && tipDialog.close().remove();
				tipDialog = dialog({
					title: "提示信息",
					content: "輸入有誤!",
					fixed: true,
					okValue: '确定',
					onremove: function () {
						txtResultmin.focus();
					}
				}).showModal();
				isOk2 = false;
			}else if (txtResultmax.val() == '') {
				tipDialog && tipDialog.close().remove();
				tipDialog = dialog({
					title: "提示信息",
					content: "輸入有誤!",
					fixed: true,
					okValue: '确定',
					onremove: function () {
						txtResultmax.focus();
					}
				}).showModal();
				isOk2 = false;
			}else if (Number(txtResultmin.val()) >= Number(txtResultmax.val()) ) {
				tipDialog && tipDialog.close().remove();
				tipDialog = dialog({
					title: "提示信息",
					content: "結果範圍輸入有誤,第一個輸入值需小於第二個的值!",
					fixed: true,
					okValue: '确定',
					onremove: function () {
						txtResultmin.focus();
					}
				}).showModal();
				isOk2 = false;
			}else {
				isOk2 = true;
			}
		}else{
			isOk2 = true;
		}
		return (isOk1 && isOk2);
	}

	setSelect($("#sltLottery").val());
	// 绑定彩种 联动 下注类型
	$("#sltLottery").change(function () {
		var v = $(this).val();
		setSelect(v);
	});

	function setSelect(v) {
		var html = '<option value="0">全部</option>';
		$.ajax({
			url: 'Handler/QueryHandler.ashx?action=get_playbylottery',
			type: 'POST',
			cache: false,
			dataType: 'json',
			timeout: 5000,
			async: false,
			data:{
				lid: v
			},
			success:function (d) {
				if(d.Success == 200){
					if (d.data.hasOwnProperty('playoption')) {
						var list = d.data.playoption.split('|');
						var listLen = list.length;
						for(var i=0; i<listLen; i++){
							html+='<option value="'+ list[i].split(',')[0] +'">'+ list[i].split(',')[1] +'</option>';
						}
					}
					$("#sltPlay").html(html);
				}
			},
			error:function () {}
		});
	}
});
