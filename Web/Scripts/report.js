define(function (require, exports, module) {
	var $ = require('jquery');
	require('plus')($);
	require('myLayer')($);
	require('wdatePicker');
	var sub = require('sub');

	function setUrl(src, str) {
		var a = src.split("/");
		a[a.length - 2] = str;
		return a.join('/');
	}
	var mySkin = '';
	$("#txtbegintime").click(function () {
		var that = $(this);
		if (mySkin != top.skinPath && mySkin != '') {
			var skinLink = $('div[lang=zh-cn]', top.document).find('iframe').contents().find('link');
			skinLink.attr('href', setUrl(skinLink.attr('href'), top.skinPath));
		}
		mySkin = top.skinPath;
		WdatePicker({
			skin: top.skinPath,
			dateFmt: 'yyyy-MM-dd',
			onpicked: function () {
				var obj = this;
				txtendtime.value = obj.value;
				txtendtime.focus();
			},
			minDate: that.attr('data-min')
		});
	});
	$("#txtendtime").click(function () {
		if (mySkin != top.skinPath && mySkin != '') {
			var skinLink = $('div[lang=zh-cn]', top.document).find('iframe').contents().find('link');
			skinLink.attr('href', setUrl(skinLink.attr('href'), top.skinPath));
		}
		mySkin = top.skinPath;
		WdatePicker({
			skin: top.skinPath,
			dateFmt: 'yyyy-MM-dd',
			minDate: '#F{$dp.$D(\'txtbegintime\',{d:0})}'
		});
	});

	// 设置彩种
	var t_TYPE = $("#t_TYPE");
	var type_list = $("#type_list");
	var sltLottery = $("#sltLottery");
	var t_TYPE_Html = '';
	var select_Stat = "";
	for (var key in jsonLottery) {
		select_Stat = "";
		if (key.split(',')[0] == lid) {
			select_Stat = "selected = 'selected'";
		}
		t_TYPE_Html += '<option value="' + key.split(',')[0] + '" ' + select_Stat + '>' + key.split(',')[1] + '</option>';
	}
	t_TYPE.html(t_TYPE_Html);

	function setLottery(v) {
		var lotteryHtml = '';
		var opendate = '';
		if (v == '1') {
			lotteryHtml = '<option value="100">香港⑥合彩</option>';
			sltLottery.html(lotteryHtml);
			setSelect(sltLottery.val());
			type_list.hide();
			opendate = minDateString.split(',')[0];
		} else {
			lotteryHtml = '<option value="">全部</option>';
			var kcData = jsonLottery['2,快彩'];
			for (var key in kcData) {
			    lotteryHtml += '<option value="' + $.trim(key) + '">' + kcData[key] + '</option>';
			}
			sltLottery.html(lotteryHtml);
			setSelect(sltLottery.val());
			type_list.show();
			opendate = minDateString.split(',')[1];
		}
		$("#reportOpenDate").html(opendate);
		$("#txtbegintime").attr({
			'data-min': opendate
		});
	}
	setLottery(t_TYPE.val());
	t_TYPE.change(function () {
		setLottery($(this).val());
	});

	// 绑定彩种 联动 下注类型
	sltLottery.change(function () {
		setSelect($(this).val());
	});

	$("#phasebylottery").change(function () {
		$("#phasebylotteryRadio").prop('checked', true);
	});

	$(".valignM").click(function () {
		$("#DateRadio").prop('checked', true);
	});


	function setSelect(v) {
		var html = '<option value="0">全部</option>';
		var html2 = '<option value="">--請選擇獎期--</option>';

		$("#pkbjlisdisplay").html('');

		if (v == '') {
			$("#sltPlay").html(html);
			$("#phasebylottery").html(html2);
			$("#phasebylotteryRadio").prop('disabled', true);
			$("#DateRadio").prop('checked', true);
			return;
		} else {
			$("#phasebylotteryRadio").prop('disabled', false);
		}

		if ($("#sltLottery").val() == '100') {
			html2 = '';
			if (openPhaseSix == '1') {
				$("#balance_2").prop('checked', true);
			} else {
				$("#balance_1").prop('checked', true);
			}
		} else {
			$("#balance_1").prop('checked', true);
		}
		$.ajax({
			url: '../Handler/QueryHandler.ashx?action=get_phasebylottery',
			type: 'POST',
			cache: false,
			dataType: 'json',
			timeout: 5000,
			async: false,
			data: { lid: v },
			success: function (d) {
				if (d.success == 200) {
					if (d.data.hasOwnProperty('playoption')) {
						var list = d.data.playoption.split('|');
						var listLen = list.length;
						for (var i = 0; i < listLen; i++) {
							html += '<option value="' + list[i].split(',')[0] + '">' + list[i].split(',')[1] + '</option>';
						}
					}
					$("#sltPlay").html(html);
					if (d.data.hasOwnProperty('phaseoption')) {
						var list2 = d.data.phaseoption.split('|');
						var listLen2 = list2.length;
						for (var i = 0; i < listLen2; i++) {
							html2 += '<option value="' + list2[i].split(',')[0] + '">' + list2[i].split(',')[1] + '</option>';
						}
						$("#phasebylotteryRadio").prop('disabled', false);
					} else {
						$("#phasebylotteryRadio").prop('disabled', true);
						$("#DateRadio").prop('checked', true);
					}
					$("#phasebylottery").html(html2);
					// 百家乐
					if(v == '8'){
						var html3 = '';
						if(d.data.hasOwnProperty('tabletype')){
							var list3 = d.data.tabletype.split('|');
							var listLen3 = list3.length;
							html3 += '<select name="sltTabletype" id="sltTabletype"><option value="">全部</option>';
							for (var i = 0; i < listLen3; i++) {
								html3 += '<option value="' + list3[i].split(',')[0] + '">' + list3[i].split(',')[1] + '</option>';
							}
							html3 += '</select>';
						}
						if(d.data.hasOwnProperty('playtype')){
							var list4 = d.data.playtype.split('|');
							var listLen4 = list4.length;
							html3 += '<select name="sltPlaytype" id="sltPlaytype"><option value="">全部</option>';
							for (var i = 0; i < listLen4; i++) {
								html3 += '<option value="' + list4[i].split(',')[0] + '">' + list4[i].split(',')[1] + '</option>';
							}
							html3 += '</select>';
						}
						$("#pkbjlisdisplay").html(html3);
					}
				}
			},
			error: function () { }
		});
	}

	$("#btnSearch").click(function () {
		var that = $(this);
		var src = '';
		var radioVal = $("input[name=ReportType]:checked").val();
		if (sltLottery.val() == '100') {
			src = 'ReportList_' + radioVal + '_six.aspx';
		} else {
			src = 'ReportList_' + radioVal + '_kc.aspx';
		}

		sub.setAjaxLoading();
		window.location.href = src + '?' + $('#searchForm').serialize();
		/*
		that.myLayer({
			title: that.html(),
			isShowBtn: false,
			isMiddle: true,
			width: 0.95,
			height: 0.95,
			url: src +'?'+ $('#searchForm').serialize()
		});
		*/
	});
});
