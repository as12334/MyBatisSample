define(function(require, exports, module) {
	var $  = require('jquery');

	var getBaseDataAjax = require('getBaseDataAjax');


	// (function() {

	//   var root = this;

	  var Countdown = function(duration, onTick, onComplete) {
		var secondsLeft = Math.round(duration)
			, tick = function() {
			  if (secondsLeft > 0) {
				  onTick(secondsLeft);
				  secondsLeft -= 1;
			  } else {
				  clearInterval(interval);
				  onComplete();
			  }
			}
			, interval = setInterval(
			  (function(self){
				return function() { tick.call(self); };
			  })(this), 1000
			);

		// First tick.
		tick.call(this);

		return {
		  abort: function() {
			clearInterval(interval);
		  }

		  , getRemainingTime: function() {
			return secondsLeft;
		  }
		};
	  };

	//   if (typeof exports !== 'undefined') module.exports = exports = Countdown;
	//   else root.Countdown = Countdown;

	// }).call(this);

	//根据剩余时间字符串计算出总秒数
	function getTotalSecond(timestr) {
		var reg = /\d+/g;
		var timenums = new Array();
		while ((r = reg.exec(timestr)) != null) {
			timenums.push(parseInt(r));
		}
		var second = 0, i = 0;
		if (timenums.length == 4) {
			second += timenums[0] * 24 * 3600;
			i = 1;
		}
		second += timenums[i] * 3600 + timenums[++i] * 60 + timenums[++i];
		return second;
	}
 
	//根据剩余秒数生成时间格式
	function getNewSyTime(sec) {
		var s = sec % 60;
		sec = (sec - s) / 60; //min
		var m = sec % 60;
		sec = (sec - m) / 60; //hour
		var h = sec % 24;
		var d = (sec - h) / 24;//day
		var syTimeStr = "";
		if (d > 0) {
			syTimeStr += d.toString() + ":";
		}
  
		syTimeStr += ("0" + h.toString()).substr(-2) + ":"
			  + ("0" + m.toString()).substr(-2) + ":"
			  + ("0" + s.toString()).substr(-2);
  
		return syTimeStr;
	}	


	var objDate = new Object;


　　function ctdFun(that){
		this.that = that;
　　}
	ctdFun.prototype = {
		ctd:function(){
			var _this = this;
			var that = _this.that;
			var sTimer = null;
			var timer = null;
			var s = 0;
			if(that.attr('data-val') != ''){
				if(that.attr('data-id') == '100'){
					if(that.attr('data-state') == '0'){
						s = 60;
						objDate['submitCountdown' + that.attr('data-id')] = new Countdown(s, function(seconds) {
						}, function() {
							clearTimeout(timer);
							timer = null;
							timer = setTimeout(function () {
								_this.getData(that);
							}, 1500);
						});
					}else{
						var date = new Date(that.attr('data-val').replace(/-/g, '/'));
						var data2 = new Date(that.attr('data-now').replace(/-/g, '/'));
						s = Math.round((date.getTime() - data2.getTime())/1000);
						if( s <= 0){
							clearTimeout(sTimer);
							sTimer = null;
							sTimer = setTimeout(function() {
								_this.getData(that);
							}, 60000);
						}else{
							objDate['submitCountdown' + that.attr('data-id')] = new Countdown(s, function(seconds) {
								that.val(getNewSyTime(seconds));
							}, function() {
								clearTimeout(timer);
								timer = null;
								timer = setTimeout(function () {
									_this.getData(that);
								}, 1500);
							});
						}
					}
				}else{
					if(that.attr('data-state') == '0'){
						var date = new Date(that.attr('data-val').replace(/-/g, '/'));
						var data2 = new Date(that.attr('data-now').replace(/-/g, '/'));
						s = Math.round((date.getTime() - data2.getTime())/1000);
						if( s <= 0){
							clearTimeout(sTimer);
							sTimer = null;
							sTimer = setTimeout(function() {
								_this.getData(that);
							}, 60000);
						}else{
							objDate['submitCountdown' + that.attr('data-id')] = new Countdown(s, function(seconds) {
								if(seconds%60 == 0){
									// console.log(that.attr('data-id'), s, seconds, date, data2);
									clearTimeout(timer);
									timer = null;
									timer = setTimeout(function () {
										_this.getData(that);
										objDate['submitCountdown' + that.attr('data-id')].abort();
									}, 1500);
								}
							}, function() {
								clearTimeout(timer);
								timer = null;
								timer = setTimeout(function () {
									_this.getData(that);
								}, 1500);
							});
						}
					}else{
						s = getTotalSecond(that.val());
						objDate['submitCountdown' + that.attr('data-id')] = new Countdown(s, function(seconds) {
							that.val(getNewSyTime(seconds));
						}, function() {
							clearTimeout(timer);
							timer = null;
							timer = setTimeout(function () {
								_this.getData(that);
							}, 1500);
						});
					}
				}
			}
		},
		getData:function(){
			var _this = this;
			var that = _this.that;
			var b = new getBaseDataAjax({
				url: '/Handler/QueryHandler.ashx?action=get_gamehall&lid=' + that.attr('data-id'),
				_type: 'POST',
				dataType: 'json',
				postData: {},
				completeCallBack: function (d) {
					// that.requsetErrorHandlers(d);
				},
				successCallBack: function (d) {
					if(d.success == '200'){
						var arr = d.data.isopendata[that.attr('data-id')].split(',');
						// if( arr[0] == '0'){
						// 	console.log(that.attr('data-id'),'系统返回', arr[1], arr[2])
						// }
						that.attr('data-state', arr[0]);
						that.attr('data-val', arr[1]);
						that.attr('data-now', arr[2]);
						that.val(arr[1]);
						$("#spn_s_"+that.attr('data-id')).find('img').attr('src', '/images/lottery_state_'+ arr[0] +'.gif');
						_this.ctd(that);
					}
				},
				errorCallBack: function (d) {

				}
			});
		}
	};

	$('input').each(function() {
		var that = $(this);
		var c =  new ctdFun(that);
		c.ctd();
	});


	$(".lotteryClick").click(function () {
		top.menuClick($(this).attr('data-id'));
		top.ddddd.close().remove();
	});

	$("#kjBtn").click(function () {
	    var that = $(this);
	    window.parent.setMyLayer('/SixOpenSchedule/six_open_date.aspx', '【香港⑥合彩】開獎日期表');
	});

});