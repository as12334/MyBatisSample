define(function (require, exports, module) {
	var $  = require('jquery');
    require('myLayer')($);
    require('wdatePicker');
	require('listHover');
	var dialog = top.dialog;
	$(".logOddsChanges").click(function () {
		var that = $(this);

		var d = dialog({
			title: that.html(),
			url: '/ViewLog/LogOddsChangePhaseManager.aspx?lid=' + $('#sltlottery').val() + '&phaseNum=' + that.attr('data-phaseNum'),
			fixed: true,
			width: $(window).width() * .9,
			onreset: function () {
			},
			oniframeload: function () {
				this.reset()
			}
		});
		d.showModal();		
		// that.myLayer({
		// 	title: that.html(),
		// 	isMiddle: true,
		// 	isShowBtn: false,
		// 	url: '/ViewLog/LogOddsChangePhaseManager.aspx?lid=' + $('#sltlottery').val() + '&phaseNum=' + that.attr('data-phaseNum')
		// });
	});

	$(".logOpenLottery").click(function () {
	    var that = $(this);

	    var d = dialog({
	        title: that.html(),
	        url: '/ViewLog/LogOpenLottery.aspx?lid=' + $('#sltlottery').val() + '&phaseNum=' + that.attr('data-phaseNum'),
	        fixed: true,
	        width: $(window).width() * .9,
	        onreset: function () {
	        },
	        oniframeload: function () {
	            this.reset()
	        }
	    });
	    d.showModal();
	});

	$("#btnopendate").click(function () {
		var that = $(this);
		var d = dialog({
			title: that.html(),
			url: '/SixOpenSchedule/six_open_date.aspx',
			fixed: true,
			onreset: function () {
			},
			oniframeload: function () {
				this.reset()
			}
		});
		d.showModal();
		// that.myLayer({
		// 	title: that.html(),
		// 	isShowBtn: false,
		// 	isMiddle: true,
		// 	url: '/SixOpenSchedule/six_open_date.aspx'
		// });
	});

	function setUrl(src, str) {
		var a = src.split("/");
		a[a.length-2] = str;
		return a.join('/');
	}

	var mySkin = '';

	$(".Wdate").click(function () {
		if (mySkin != top.skinPath && mySkin != '') {
			var skinLink = $('div[lang=zh-cn]', top.document).find('iframe').contents().find('link');
			skinLink.attr('href', setUrl(skinLink.attr('href'), top.skinPath));
		}
		mySkin = top.skinPath;
		WdatePicker({
			skin: top.skinPath
		});
	});


});
