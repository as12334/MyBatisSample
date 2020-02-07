define(['jquery', 'getBaseDataAjax'], function(require, exports, module) {

	var getBaseDataAjax = require('getBaseDataAjax');

	function returnSec (str) {
		str = str.replace(/-/g, "/");
		var d = new Date(str);
		return d.getTime();
	}
	window.parent.isClickAgin = true;
	// 格式化时间 hh:mm:ss => s
	function timeFormat(time) {
		var hms = time.split(":"),
		s = hms[2] - 0,
		m = hms[1] - 0,
		h = hms[0] - 0;
		return h * 3600 + m * 60 + s;
	}
	// 反格式化时间 s => hh:mm:ss
	function unTimeFormat(time) {
		var h = parseInt(time / 60 / 60) + '', m = parseInt(time / 60) - 60 * h + '', s = time % 60 + '';
		h < 10 ? h = '0' + h : h;
		m < 10 ? m = '0' + m : m;
		s < 10 ? s = '0' + s : s;
		return h + ':' + m + ':' + s;
	}

	(function ($) {

		$.fn.noopenDiv = function () {

			return this.each(function () {
				var top = parseInt((Math.random() - 1) * 300);
				var left = parseInt((Math.random() - 0.5) * $(window).width() / 2);
				var that = $(this);
				divanmate();
				function divanmate() {
					that.css({
						'top': top,
						'left': left
					}).animate({
						'top': parseInt((Math.random() - 2) * 300),
						'left': parseInt(Math.random() * 400)
					}, parseInt((Math.random() + 0.5) * 3000), function () {
						that.animate({
							'top': top,
							'left': left
						}, parseInt((Math.random() + 0.5) * 3000), function () {
							divanmate();
						})
					});
				}
			});

		};

	})(jQuery);

	var noopenInit = function () {

		window.parent.htmlData = {};
		parent.setIframeHeight();
		$('.bg').noopenDiv();
		var timer = null;
		var timer2 = null;
		openTimeUpdata((returnSec(endTime) - returnSec(currentTime))/1000);

		var kc_valid_seconds = 0;
		function openTimeUpdata(seconds) {
			if (isNaN(seconds)) {
				return;
			}

			if(seconds<0){
				isOpenPan();
				return;
			}
			clearTimeout(timer);

			if (kc_valid_seconds >= 1800) {
			    kc_valid_seconds = 0;
			    window.location.reload();
			}

			if (--seconds) {
				timer = setTimeout(function () {
					openTimeUpdata(seconds);
				}, 1000);
				var time = unTimeFormat(seconds);
				var h = time.split(':')[0];
				var m = time.split(':')[1];
				var s = time.split(':')[2];
				$("#openTime").html('距離開盤時間還有:' + h + '小時' + m + '分' + s + '秒');
				kc_valid_seconds++;
			} else {
				window.location.href = url;
			}
		}

		function closeTimeUpdata(seconds) {
			clearTimeout(timer2);
			if (--seconds) {
				timer2 = setTimeout(function () {
					closeTimeUpdata(seconds);
				}, 1000);
				$("#openTime").html('未開盤，'+ seconds + '秒後刷新！');
			} else {
				isOpenPan();
			}
		}

		function isOpenPan(num) {
			var b = new getBaseDataAjax({
				url: '../Handler/QueryHandler.ashx',
				postData:{
					action: 'get_openlottery',
					lid: lid
				},
				completeCallBack:function () {

				},
				successCallBack:function (d) {
					if(d.data.isopenvalue.isopen == -1){
						closeTimeUpdata(10);
					}else{
						window.location.href = url;
					}
				},
				errorCallBack:function () {

				}
			});
		}

	};

	module.exports = noopenInit;
});
