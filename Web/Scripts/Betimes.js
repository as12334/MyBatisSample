define(function(require, exports, module) {
	// 引入排序
	// 
	// 引入jquery
	var $ = require('jquery');
	// 引入jquery插件
	require('plus')($);
	// 引入myLayer插件
	require('myLayer')($);
	// 引入tips插件
	var tips = require('tips');
	// 引入array插件
	require('array');
	// 引入封装好的Ajax
	var getBaseDataAjax = require('getBaseDataAjax');
	// 引入sound
	var sound = require('sound');
	// 引入封装好的array
	Array = require('array');
	// 激活consoloe
	// var console = console || {log:function(){return false;}};
	// 找到父级
	var p = window.parent;

	var dialog = top.dialog;

	var lmClassName1 = '';
	var lmClassName2 = '';

	$.arrayIntersect = function(a, b)
	{
		return $.grep(a, function(i)
		{
			return $.inArray(i, b) > -1;
		});
	};

	Array.prototype.unique = function()
	{
		this.sort();
		var re=[this[0]];
		for(var i = 1; i < this.length; i++)
		{
			if( this[i] !== re[re.length-1])
			{
				re.push(this[i]);
			}
		}
		return re;
	};

	// 判断json对象是否相等
	function compareObject(o1, o2) {
		if (typeof o1 != typeof o2) return false;
		if (typeof o1 == 'object') {
			for (var o in o1) {
				if (typeof o2[o] == 'undefined') return false;
				if (!compareObject(o1[o], o2[o])) return false;
			}
			return true;
		} else {
			return o1 === o2;
		}
	}
	// 减法函数
	function accSub(arg1, arg2) {
		var r1, r2, m, n;
		try {
			r1 = arg1.toString().split(".")[1].length;
		} catch(e) {
			r1 = 0;
		}
		try {
			r2 = arg2.toString().split(".")[1].length;
		} catch(e) {
			r2 = 0;
		}
		m = Math.pow(10, Math.max(r1, r2));
		n = (r1 >= r2) ? r1: r2;
		return ((arg1 * m - arg2 * m) / m).toFixed(n);
	}

	// 给Number增加减法方法
	Number.prototype.sub = function(arg) { return accSub(arg, this); };

	function ForDight2(Dight){
		Dight = Math.round(Dight*Math.pow(10,2))/Math.pow(10,2);
		return Dight;
	}
	function ForDight4(Dight){
		Dight = Math.round(Dight*Math.pow(10,4))/Math.pow(10,4);
		return Dight;
	}
	// 加载进度条
	var percentage = 0;
	function loadPage(per) {
		percentage = percentage + per;
		if (percentage > 100) {
			percentage = 100;
		}
		var loadHtml = '<div id="loadPage"></div>';
		if ($("#loadPage").length == 0) {
			$("body").append(loadHtml);
		}
		$("#loadPage").stop(true,false).animate({
			"width" : percentage + "%"
		}, 500, function  () {
			if (percentage == 100) {
				p.isClickAgin = true;
				$("#loadPage").hide(100);
			}
		});
	}
	var secondData = {
		'a': '<option value="No">-NO-</option><option selected="" value="5">5秒</option><option value="10">10秒</option><option value="20">20秒</option>',
		'b': '<option value="No">-NO-</option><option selected="" value="20">20秒</option>'
	};
	var oneTime = 0;
	/*
	 * [基础模块 需jQuery]
	 */
	var Betimes = {
		// 盈亏排列开关
		isSortBtn: 1,
		// 弹窗对象容器
		aOddsThat: [],
		// 走飞对象容器
		flyAwayThat: [],
		// 全局是否开封盘 n，封盘 y，开盘
		isopenning: '',
		// 赔率接口返回的注额数据缓存
		myData: {},
		// 第一次開始 開封盤倒計時 計數器 供第一次開始使用
		isFirst: 1,
		// 大id自生成对象，计算盈亏用
		list: {},
		// 当前是否开盘 默认未开盘
		isMyOpen: 'n',
		// 赔率数据缓存
		oddsData: {},
		// 1-49数据缓存
		numData: {},
		// 六肖
		sixZodia: {},
		// 当前注额
		nowAmountValue: 0,
		// 当前补货
		nowRepValue: 0,
		// 当前注数
		nowAmountNumber: 0,
		// 是否加载六合彩连码补货数据
		isLoadLmSzsz: false,
		// 连码走飞输入框值缓存对象
		LmInputCacheObject: {},
		// 是否为特码快速走飞
		isTmFastFlyAway: false,
		// 开奖Id缓存
		myWinnerID: '',
		//赔率弹窗目标id
		oddsTargetId: '',
		//走飞弹窗目标id
		flyAwayTargetId: '',
		// 初始化
		_init: function(){
			var _this = this;
			// 页面加载进度条
			percentage = 0;
			
			oneTime = 0;
			loadPage(20);
			// 修改皮肤
			$("#Iframe_skin").skinChange(p.skinPath);
			// 盈亏排列开关
			_this.isSortBtn = 1;
			// 弹窗对象容器
			_this.aOddsThat = [];
			// 走飞对象容器
			_this.flyAwayThat = [];
			// 全局是否开封盘 n，封盘 y，开盘
			_this.isopenning = '';
			// 赔率接口返回的注额数据缓存
			_this.myData = null;
			// 第一次開始 開封盤倒計時 計數器 供第一次開始使用
			_this.isFirst = 1;
			// 大id自生成对象，计算盈亏用
			_this.list = {};
			// 当前是否开盘 默认未开盘
			_this.isMyOpen = 'n';
			// 请求赔率id
			p.oldId = 0;
			// 赔率数据缓存
			_this.oddsData = {};
			// 1-49数据缓存
			_this.numData = {};

			_this.oddsTargetId = '';
			_this.flyAwayTargetId = '';
			// 首次加载清空
			_this.clearAmoutData();

			_this.nowAmountValue = 0;
			// 当前补货
			_this.nowRepValue = 0;
			// 当前注数
			_this.nowAmountNumber = 0;
			// 缓存当前页面
			var pageName = p.pathName.split('_')[2].toLowerCase();
			// pageName如果返回数字则增加'p'
			if (!isNaN(pageName)) {
				pageName = 'p'+pageName;
			}
			if (pageName == p.playpage) {
				p.htmlData[p.pathName] = $('html body').html();
			}
			// 设置六肖连生肖名称
			if(p.pathFolder == 'L_SIX' && p.playpage == 'lxl'){
				sixBet.setSixLxlPage();
			}
			// 设置倒计时
			var secondKey = '';
			if (p.usertype == '1') {
				if (p.isLm == '1') {
					secondKey = 'b';
				}else{
					secondKey = 'a';
				}
			}else{
				secondKey = 'b';
			}
			$("#upSecond").html(secondData[secondKey]);

			if(p.isShowLM_B == '1'){
				$("#six_lm_tab").show();
			}else{
				$("#six_lm_tab").hide();
			}

			// 总项3是否显示
			if( p.usertype != '1'){
				$("#zx3").show();
			}else{
				$("#ZKbox").show();
				$("#ZKbox2").show();
				$("#zx3").hide();
			}

			// 初始化更新
			_this.upType();
			// 初始化 赔率接口
			_this.oddsAjax($("#upSecond").val()-0);
			// 加载长龙
			if($("#lmcl").length>0){
				_this.getClylAjax();
			}
			// 近期開獎
			if($("#phaseList").length>0){
				_this.getPhaseList();
			}
			// 是否有权限调整赔率
			if( p.isopt == '1'){
				// 設置賠率工具
				$("#tool_wrap").oddsTool();
				if( p.isShortcut == '1'){
					// 快捷栏
					$("#KJLbtn").show();
					ShortcutFun();
				}
			}
			// 是否有权限走飞补货
			var zj = $("#zj");		// 总监
			var nozj = $("#nozj");  // 非总监
			if( p.issllowsale == '1'){
				if( p.isLm ){
					if( p.pathFolder != 'L_SIX'){
						zj.hide();
						nozj.show();
					}else{
						if(p.isopt == '1'){
							zj.show();
							nozj.hide();
						}else{
							zj.hide();
							nozj.show();
						}
					}
				}
			}else{
				if( p.isLm){
					if( p.pathFolder != 'L_SIX'){
						zj.show();
						nozj.hide();
					}else{
						if(p.isopt == '1'){
							zj.hide();
							nozj.show();
						}else{
							zj.show();
							nozj.hide();
						}
					}
				}
			}
			var oBody = $("body");

			oBody.onlyNumber({
				className: '.plNumber',
				isDecimal: true,
				isMinus: false
			});

			oBody.onlyNumber({
				className: '.zfNumber',
				isDecimal: false,
				isMinus: false
			});

			// 绑定开封盘按钮事件
			_this.openCloseHandle();

			// 绑定特码快速走飞事件
			$("#fastBtn").click(function () {
				// 打开特码快速走飞开关
				$(this).prop('disabled', 'disabled');
				_this.isTmFastFlyAway = true;
				$('#upSecond option[value=No]').prop('selected',true);
				$('#upSecond').change();
			});

			sixBet.subnavLink();

			// 处理特码生肖色波 生肖尾数
			if(p.playpage == 'sxws' || p.playpage == 'tmsxsb' || p.playpage == 'sxwsbz'){
				_this.setZodiac();
			}

			// 绑定划过事件
			var tableBody = $(".tableBody");
			tableBody.delegate('.oddsParent:not(.noHover)', 'mouseover', function () {
				$(this).addClass('oddsParentActive');
			});
			tableBody.delegate('.oddsParent:not(.noHover)', 'mouseout', function () {
				$(this).removeClass('oddsParentActive');
			});
			tableBody.delegate('.noHover .pDiv', 'mouseover', function () {
				$(this).addClass('oddsParentActive');
			});
			tableBody.delegate('.noHover .pDiv', 'mouseout', function () {
				$(this).removeClass('oddsParentActive');
			});
			$(document).delegate('.bhlbClass', 'mouseover', function () {
				$(this).addClass('oddsParentActive');
			});
			$(document).delegate('.bhlbClass', 'mouseout', function () {
				$(this).removeClass('oddsParentActive');
			});

			// 弹出层函数
			function amountAlert(t, src) {

				dialog({
					title: "明细",
					url: src,
					width: $(window).width() * .9,
					fixed: true
				}).showModal();
				
			}

			if(p.pathFolder == 'L_SIX'){
				tableBody.delegate('.amountEvent', 'click', function () {
					var t = $(this);
					var alertUrl = '';
					if(t.attr('data-alertOdds') == '92565'){
						alertUrl = '../viewbill/bill_six.aspx?phaseid='+ $("#nn").attr('data-pid') +'&oddsid=92565&lid=' + p.myLid + '&number=' + t.attr('data-amount');
					}else{
						if (p.playpage == 'tmzx3' || p.playpage == 'tmzx2' || p.playpage == 'tmzx2_2' || p.playpage == 'tmab') {
							alertUrl = '../viewbill/bill_six.aspx?phaseid='+ $("#nn").attr('data-pid') +'&oddsid=' + t.attr('data-amount') + '&lid=' + p.myLid + '&isab=1';
						}else if(p.playpage == 'tmzx1' && t.attr('id').indexOf('lx') != -1){
							alertUrl = '../viewbill/bill_six.aspx?phaseid='+ $("#nn").attr('data-pid') +'&oddsid=92565&lid=' + p.myLid + '&number=' + t.attr('data-amount');
						}else{
							alertUrl = '../viewbill/bill_six.aspx?phaseid='+ $("#nn").attr('data-pid') +'&oddsid=' + t.attr('data-amount') + '&lid=' + p.myLid;
						}
					}
					amountAlert( t, alertUrl + '&atz=' + $("#upActual").val() );
					return false;
				});
				tableBody.delegate('.tmxg, .gxmx', 'click', function () {
					var t = $(this);
					var alertUrl = '../viewbill/bill_six.aspx?phaseid='+ $("#nn").attr('data-pid') +'&playid=' + t.attr('data-bid') + '&lid=' + p.myLid;
					amountAlert( t, alertUrl + '&atz=' + $("#upActual").val() );
					return false;
				});
				//连码百大排行点击分组
				tableBody.delegate('.sixLmAlert', 'click', function() {
					var t = $(this);
					var alertUrl = t.attr('href') + '&atz=' + $("#upActual").val() + '&pk=' + $("#upHandicap").val() + '&oddsid=' + t.attr('data-radioKey');
					dialog({
						title: "明细",
						url: alertUrl,
						fixed: true
					}).showModal();
					return false;
				});
			}else{
				tableBody.delegate('.amountEvent', 'click', function () {
					var t = $(this);
					var alertUrl = '../viewbill/bill_kc.aspx?phaseid='+ $("#nn").attr('data-pid') +'&oddsid=' + t.attr('data-amount') + '&lid=' + p.myLid + '&playpage=' + p.playpage + '&playtype=' + $("#sltPlaytype").val();
					amountAlert( t, alertUrl + '&atz=' + $("#upActual").val() );
					return false;
				});
				tableBody.delegate('.kcmx', 'click', function () {
					var t = $(this);
					top.kcClickMenu(t.attr('data-page'));
					return false;
				});
			}
			p.setIframeHeight();
		},
		setZodiac: function () {
			var znData = returnZodiacNumber(49, 'str');
			for(var key in znData){
				$("#zodiac_" + key).html(znData[key].join('、'));
			}
		},
		// 长龙
		getClylAjax: function(){
			var _this = this;
			var b = new getBaseDataAjax({
				url: 'Handler/Handler.ashx',
				postData:{
					action: 'get_clyl'
				},
				completeCallBack:function () {},
				successCallBack:function (d) {
					// 遗漏
					if(d.data.hasOwnProperty('dropball')){
						var dropballHtml = '';
						for(var i=0; i<d.data.dropball.length; i++){
							dropballHtml += '<tr><td width="30" class="blue"><b>'+ d.data.dropball[i]['Key'] +'</b></td><td width="30">'+ d.data.dropball[i]['Value'] +'</td></tr>';
						}
						$("#dropball").html( dropballHtml );
					}
					if(d.data.hasOwnProperty('lmcl')){
						// 两面长龙
						var lmclHtml = '';
						for(var i=0; i<d.data.lmcl.length; i++){
							lmclHtml += '<tr><td><span class="green">'+ d.data.lmcl[i]['cl_num'] +'期</span>'+ d.data.lmcl[i]['cl_name'] +'</td></tr>';
						}
						$("#lmcl").html( lmclHtml );
					}
					// 重新设置iframe高度
					p.setIframeHeight();
				},
				errorCallBack:function () {}
			});
		},
		// 基础赔率接口 倒计时 闭包函数
		baseDomAjax: function (seconds) {
			var _this = this;
			var countSeconds = $("#countSeconds");

			countSeconds.unbind('click').bind('click', function () {
				clearTimeout(p.timer);
				update();
			});
			if(isNaN(seconds)){
				countSeconds.html('更新');
				return;
			}
			clearTimeout(p.timer);
			if( --seconds ){
				p.timer = setTimeout(function () {
					_this.baseDomAjax(seconds);
				}, 1000);
				countSeconds.html(seconds+'秒後更新數據');
			}else{
				update();
			}
			function update() {
				countSeconds.prop('disabled', true).html('更新中...');
				clearTimeout(p.timer);
				p.timer2 = setTimeout(function () {
					_this.oddsAjax( $("#upSecond").val()-0 );
				},1000);
			}
		},
		// 加载赔率函数
		oddsAjax: function (seconds) {
			var _this = this;
			var postData = {
					action: 'get_oddsinfo',
					playid: p.playids,
					szsz: $("#upActual").val(),
					pk: $("#upHandicap").val(),
					numsort: 1,
					isbh: $("#cbM").val(),
					oddsid: $("input[name=radiobutton]:checked").attr('data-radio'),
					playtype: $("#sltPlaytype").val()
				};
			if(p.pathFolder == 'L_SIX' && (p.playpage == 'lm' || p.playpage == 'bz' || p.playpage == 'lm_b')){
				var checkplayid  = '';
				if($("input[name=radiobutton]:checked").attr('data-radio')){
					checkplayid = $("#szsz_" + $("input[name=radiobutton]:checked").attr('data-radio')).attr('data-did');
				}else{
					if(p.playpage == 'lm'){
						checkplayid = '91016';
					}else if(p.playpage == 'bz'){
						checkplayid = '91037';
					}else if(p.playpage == 'lm_b'){
						checkplayid = '91060';
					}
				}
				postData.checkplayid = checkplayid;
			}
			var b = new getBaseDataAjax({
				url: 'Handler/Handler.ashx',
				postData: postData,
				async: true,
				completeCallBack:function () {},
				successCallBack:function (d) {
					if (d.data.hasOwnProperty('jeucode')) {
						p.jeucode = d.data.jeucode;
					}
					if (_this.isFirst == 1) {
						loadPage(60);
					}
					if (d.data.playpage == p.playpage) {
						if(d.data.hasOwnProperty('winnerID')){
							_this.myWinnerID = d.data.winnerID;
						}
						if( d.data.isopen == '1' || p.pathFolder == 'L_SIX'){
							// dom页面
							_this.baseDomHtml(d.data);
							// 激活更新按钮
							$("#countSeconds").prop('disabled', false);
							// 重新启动倒计时
							_this.baseDomAjax( seconds + 1 );
						}else{
							$.myLayer.close(true, window.top.myLayerIndex);
							window.location.href = "../noopen.aspx?lid=" + p.myLid + '&path=' + encodeURIComponent(p.myPath);
						}
					}
				},
				errorCallBack:function () {}
			});
		},
		// 获取奖期
		getPhaseList: function () {
			var html = '';
			var b = new getBaseDataAjax({
				url: 'Handler/Handler.ashx',
				postData:{
					action: 'get_recent'
				},
				async: true,
				completeCallBack:function () {},
				successCallBack:function (d) {
					var phaseList = $("#phaseList");
					if(phaseList.length>0){
						var data = d.data.recentphase;
						var numList = [];
						var numHtml = '';
						for(var i=0; i<data.length; i++){
							numList = data[i].number.split(',');
							numHtml = '';
							for(var j=0; j<numList.length; j++){
								numHtml += '<div class="No_'+ numList[j] +'"></div>';
							}
							html += '<tr><td>'+ data[i].phase +'期</td><td class="'+ p.pathFolder +'">'+ numHtml +'</td><td>'+ data[i].zh +'</td><td>'+ data[i].dx +'</td></tr>';
						}
						phaseList.html(html);
						// 重新设置iframe高度
						p.setIframeHeight();
					}
				},
				errorCallBack:function () {}
			});
		},
		upOpenPhaseSecond: 3,
		// 获取开奖号码
		upOpenPhase: function(nn) {
			var _this = this;
			clearTimeout(_this.upOpenPhaseTimer);
			_this.upOpenPhaseTimer = setTimeout(function () {
				var b = new getBaseDataAjax({
					url: 'Handler/Handler.ashx',
					postData:{
						action: 'get_opennumber'
					},
					async: true,
					completeCallBack:function () {},
					successCallBack:function (d) {
						if(d.data.opennumber.upopenphase != $("#upopenphase").html()){
							if($("#lmcl").length>0){
								_this.getClylAjax();
							}
						}
						if (p.pathFolder == 'L_SIX') {
							// 六合彩处理开奖接口
							// isopen是正常状态下的openning
							if (d.data.opennumber.isopen == 'n') {
								_this.upOpenPhase( nn );
							}else{
								_this.oddsAjax($("#upSecond").val()-0);
							}
						}else{
							// 快彩处理开奖接口
							if(p.pathFolder == 'L_PKBJL'){
								if( nn != d.data.opennumber.upopenphase && _this.isopenning == 'n'){
									_this.upOpenPhase( nn );
								}
							}else{
								if( (nn-1) > d.data.opennumber.upopenphase ){
									_this.upOpenPhase( nn );
								}
							}
						}
						_this.upOpenPhaseDom( d.data.opennumber );
					},
					errorCallBack:function () {}
				});
			}, _this.upOpenPhaseSecond * 1000);
		},
		// 更新开奖函数
		openningTime: function(time, openning) {
			var _this = this;
			var t = timeFormat( time );
			if( (t<=0 || isNaN(t)) && p.pathFolder != 'L_SIX'){
				p.upWindowTime = setTimeout(function () {
					$.myLayer.close(true, window.top.myLayerIndex);
					location.replace('/'+p.myPath +'?lid='+ p.myLid);
				}, 3000);
			}else{
				// 六合彩下不做处理单独处理快彩
				if (openning == 'n' && p.pathFolder == 'L_SIX') {
					// _this.oddsAjax($("#upSecond").val()-0);
				}else{
					_this.openningCountSecond(t, openning);
				}
			}
		},
		// 开封盘倒计时函数
		openningCountSecond: function(t, openning) {
			var _this = this;
			clearTimeout(p.openTimer);
			clearTimeout(p.openTimer2);
			var openTypeTime = $("#openTypeTime");
			if( --t ){
				p.openTimer = setTimeout(function () {
					_this.openningCountSecond(t);
				}, 1000);
				openTypeTime.html( unTimeFormat(t) );
			}else{
				clearTimeout(p.openTimer);

				if($("#openType").html() == '距封盤：'){
					openTypeTime.html( '正在封盤中...' );
				}else{
					openTypeTime.html( '正在開盤中...' );
				}
				$("#openType").html('狀態：');
				p.openTimer2 = setTimeout(function () {
					_this.oddsAjax($("#upSecond").val()-0);
				}, 3000);
			}
		},
		// 清除數據
		clearAmoutData:function (isNoClearTime) {
			var _this = this;
			ajaxObj = null;
			$(".clearAmout").unbind('click');
			$(".clearSzsz").removeClass('szszEvent');
			if (!isNoClearTime) {
				clearTimeout(p.openTimer);
				p.openTimer = null;
				clearTimeout(p.openTimer2);
				p.openTimer2 = null;
				clearTimeout(p.upWindowTime);
				p.upWindowTime = null;
				clearTimeout(p.timer);
				p.timer = null;
				clearTimeout(p.timer2);
				p.timer2 = null;
			}
			_this.myData = null;
			_this.myData = {};
			// 六肖总项缓存数据
			_this.sixZodia = null;
			_this.sixZodia = {};
			sixBet.sixZodiaData = null;
			sixBet.sixZodiaData = {};
			var aPlayids = [];
			_this.isFirst = 1;
			_this.maxQuota = 0;
			_this.minQuota = 0;
			_this.numData = {};
			if(p.playids.indexOf(",")>0){
				aPlayids = p.playids.split(',');
			}else{
				aPlayids.push(p.playids);
			}
			for(var i=0; i<aPlayids.length; i++){
				_this.list["sumwin_" + aPlayids[i]] = 0;
				_this.list["sumts_" + aPlayids[i]] = 0;
				_this.list["maxwin_" + aPlayids[i]] = 0;
				_this.list["minwin_" + aPlayids[i]] = 0;
			}
			_this.list['tmbbds'] = 0;
			_this.list['tmbbdx'] = 0;
			for (var i = 1; i <= 6; i++) {
				_this.list['sumts_91011_' + i] = 0;
				_this.list['sumwin_91011_' + i] = 0;
				_this.list['sumts_91012_' + i] = 0;
				_this.list['sumwin_91012_' + i] = 0;
				_this.list['sumts_91013_' + i] = 0;
				_this.list['sumwin_91013_' + i] = 0;
				_this.list['sumts_91014_' + i] = 0;
				_this.list['sumwin_91014_' + i] = 0;
			}
			for(var i=1; i<=12; i++){
				_this.sixZodia[ i < 10 ? '0'+ i : i ] = {
					"AmountNumber": 0,
					"AmountValue": 0,
					"WinValue": 0,
					"RepValue": 0
				};
			}
		},
		// DOM獎期
		upOpenPhaseDom: function (d) {
			var _this = this;
			// 今日输赢
			$("#profit").html( d.profit );
			// 最新开奖奖期
			$("#upopenphase").html(d.upopenphase);
			// 最新开奖号码
			var upopennumberHtml = '';
			var upopennumber = [];
			var upopennumberLen = 0;
			var upopennumberzodiac = [];
			if(d.hasOwnProperty('upopennumber')){
				upopennumber = d.upopennumber.split(',');
				upopennumberLen = upopennumber.length;
			}
			if(d.hasOwnProperty('upopennumberzodiac')){
				upopennumberzodiac = d.upopennumberzodiac.split(',');
			}
			if (upopennumber[0] == '') {
				upopennumberHtml = '<span class="waitWrap">等待開獎</span>';
			}else{
				$(".waitWrap").remove();
			}
			for(var i=0; i<upopennumberLen; i++){
				if(upopennumber[i]){
					if(p.pathFolder == 'L_PCDD'){
						if(i == 3){
							upopennumberHtml += '<div class="No_text">=</div><div class="No_' + upopennumber[i] + '"></div>';
						}else if(i == 2){
							upopennumberHtml += '<div class="No_' + upopennumber[i] + '"></div>';
						}else{
							upopennumberHtml += '<div class="No_' + upopennumber[i] + '"></div><div class="No_text">+</div>';
						}
					} else if(p.pathFolder == 'L_SIX'){
						if(i == 5){
							upopennumberHtml += '<div class="No_'+ upopennumber[i] +'"></div><div class="No_text">'+ upopennumberzodiac[i] +'</div><div class="No_text">+</div>';
						}else{
							upopennumberHtml += '<div class="No_'+ upopennumber[i] +'"></div><div class="No_text">'+ upopennumberzodiac[i] +'</div>';
						}
					}else{
						upopennumberHtml += '<div class="No_'+ upopennumber[i] +'"></div>';
					}
				}
			}
			// 如果六合彩开完奖则改为10秒请求
			if(p.pathFolder == 'L_SIX'){
				if (upopennumber[6] != '') {
					_this.upOpenPhaseSecond = 10;
				}
			}
			$("#upopennumber").html( upopennumberHtml );
			// sound 插入开奖声音提示
			// console.log(_this.isopenning == 'y', _this.isMyOpen == 'n' , oneTime);
			if((_this.isopenning == 'y' && _this.isMyOpen == 'n' && oneTime > 0)){
				sound('kj');
			}
			// pk百家乐 上期开奖结果展示
			if(d.hasOwnProperty('porkList')){
				var porkHtml = '';
				if(d.porkList != ''){
					var porkList = d.porkList.split(',');
					for (var i = 0; i < porkList.length; i++) {
						porkHtml+='<div class="poker'+ porkList[i] +'"></div>';
					}
				}else{
					for (var i = 0; i < 10; i++) {
						porkHtml+='<div class="poker0"></div>';
					}
				}
				$("#porkHtml").html(porkHtml);
			}

			function countPoker(n) {
				return (n%13>=10) ? 0 : n%13;
			}

			if(d.hasOwnProperty('lastResult')){
				if(d.lastResult.player != '' && d.lastResult.banker != ''){
					var op = d.lastResult.player.split(',');
					var ob = d.lastResult.banker.split(',');
					var ps = 0;
					var bs = 0;
					var ph = '';
					var bh = '';
					for (var i = 0; i < op.length; i++) {
						ph += '<div class="poker'+ op[i] +'"></div>';
						ps += countPoker(op[i]);
					}
					ps = ps % 10;

					for (var i = 0; i < ob.length; i++) {
						bh += '<div class="poker'+ ob[i] +'"></div>';
						bs += countPoker(ob[i]);
					}
					bs = bs % 10;
					
					$("#ph").html(ph);
					$("#bh").html(bh);
					$("#ps").html(ps);
					$("#bs").html(bs);
				}
			}
		},
		baseDomHtml:function (d) {
			var _this = this;

			_this.isopenning = d.openning;
			// 当封盘转为开盘清空数值
			if((_this.isopenning == 'y' && _this.isMyOpen == 'n')){
				_this.clearAmoutData();
			}

			var openType = $("#openType");
			if ( _this.isopenning == 'y' ) {
				openType.html('距封盤：');
			}else{
				if (p.pathFolder == 'L_SIX') {
					openType.html('停止下註：<span class="red">已封盤</span>');
					$("#openTypeTime").html('');
				}else{
					openType.html('距開獎：');
				}
			}
			_this.openningTime( d.stop_time, _this.isopenning );
			// 当前奖期
			$("#nn").html(d.nn).attr("data-pId", d.p_id);
			// 开奖接口触发状态
			if (p.pathFolder == 'L_SIX') {
				// 如果六合彩当前期等于开将期也就是封盘状态下触发开奖接口
				if(d.nn == d.upopenphase || _this.isopenning == 'n'){
					_this.upOpenPhase( d.nn );
				}
			}else{
				// 判断最新开奖是否为上期开奖 不是则 3秒触发
				if(p.pathFolder == 'L_PKBJL'){
					if( (d.nn != d.upopenphase) && _this.isopenning == 'n'){
						_this.upOpenPhase( nn );
					}
				}else{
					if( (d.nn-1) > d.upopenphase ){
						_this.upOpenPhase( nn );
					}
				}
			}
			_this.upOpenPhaseDom( d );
			_this.isMyOpen = _this.isopenning;

			// 清空开奖号码样式
			$(".clearSzsz").removeClass('winner');
			// 渲染开奖号码
			if(d.hasOwnProperty('winnerID') && p.isLm == '0'){
				var winnerData = d.winnerID.split(',');
				for(var i=0; i<winnerData.length; i++){
					$("#szsz_" + winnerData[i]).addClass('winner');
				}
			}

			// 基础球赔率&按虧損/球号額排列&其他赔率
			_this.baseOddsDom(d);
			// 实占/需占下注总额
			var amountCount = d.szsz_amount_count;
			var numCounts = 0;
			for(var key in amountCount){
				var sIdHtml = amountCount[key];
				numCounts += (sIdHtml-0);
				$("b[data-amountCount="+ key +"]").html( sIdHtml );
			}

			$("#numCounts").html( numCounts );
		},
		// 最大正值
		maxQuota: 0,
		// 最大负值
		minQuota: 0,
		// 基础球赔率&按虧損/球号額排列&其他赔率
		baseOddsDom: function(d) {
			var _this = this;
			// 每次清最大正负值
			for(var key in _this.list){
				if (key.indexOf('minwin') != -1 || key.indexOf('maxwin') != -1 ) {
					_this.list[key] = 0;
				}
			}
			if (d.cleardata == '0') {
				_this.myData = {};
				_this.clearAmoutData(true);
			}

			// 打开加载六合彩连码补货请求
			_this.isLoadLmSzsz = true;
			// 清除样式
			var clearSzsz = $(".clearSzsz");
			var clearAmout = $(".clearAmout");
			clearAmout.removeClass('red');
			clearSzsz.removeClass('red');
			clearSzsz.parent().removeClass('warning');
			clearAmout.removeClass('amountEvent');
			var wrap = $('.tableBody');
			// 总投注额
			var sumQuota = 0;
			// 总退水额度
			var sumRec = 0;
			// 赔率接口对象 缓存
			var playodds = d.play_odds;
			// 注额&虧盈接口对象 缓存
			var szszAmount = d.szsz_amount;

			var aPlayids = [];

			for(var key in playodds){
				if( !_this.myData.hasOwnProperty(key) ){
					_this.myData[key] = {
						// 注数
						'AmountNumber': 0,
						// 注额
						'AmountValue': 0,
						// 盈亏
						'WinValue': 0,
						// 退水
						'RecValue': 0,
						// 补货
						'RepValue': 0
					};
					// 除六肖外
					_this.numData[key] = 0;
					// 生成双赔率第二个赔率对象最大最小对象
					if(playodds[key]['pl'].indexOf(',') > 0){
						var plArr = playodds[key]['pl'].split(',');
						for(var i=0; i<plArr.length; i++){
							_this.myData[key + '_' + i] = {
								'OddsMaxValue' : 0,
								'OddsMinValue' : 0
							};
						}
					}
				}
				// 赔率
				_this.myData[key]['OddsValue'] = playodds[key]['pl'];
				// 处理双赔率
				if(playodds[key]['maxpl'].indexOf(',')>0 && playodds[key]['minpl'].indexOf(',')>0){
					// 赔率微调最大值
					_this.myData[key]['OddsMaxValue'] = Number( playodds[key]['maxpl'].split(',')[0] );
					// 赔率微调最小值
					_this.myData[key]['OddsMinValue'] = Number( playodds[key]['minpl'].split(',')[0] );
					var arrPlay = playodds[key]['maxpl'].split(',');
					for(var i=0; i<arrPlay.length; i++){
						// 赔率微调最大值
						_this.myData[key + '_' + i]['OddsMaxValue'] = Number( playodds[key]['maxpl'].split(',')[i] );
						// 赔率微调最小值
						_this.myData[key + '_' + i]['OddsMinValue'] = Number( playodds[key]['minpl'].split(',')[i] );
					}
				}else{
					// 赔率微调最大值
					_this.myData[key]['OddsMaxValue'] = Number( playodds[key]['maxpl'] );
					// 赔率微调最小值
					_this.myData[key]['OddsMinValue'] = Number( playodds[key]['minpl'] );
				}
				// 当前号码是否开盘
				_this.myData[key]['IsOpen'] = Number( playodds[key]['is_open'] );
				if( szszAmount.hasOwnProperty(key) ){
					var that = szszAmount[key];
					var szszObj = $("#szsz_"+ key);
					_this.myData[key]['AmountNumber'] += Number(that.split(',')[0]);
					_this.myData[key]['AmountValue'] += Number(that.split(',')[1]);
					_this.myData[key]['WinValue'] += Number(that.split(',')[2]);
					_this.myData[key]['RecValue'] += Number(that.split(',')[3]);
					_this.myData[key]['RepValue'] += Number(that.split(',')[4]);
					var NumKey = Number(key);
					var fBigId = '';
					// 六合彩总项一、二、特码AB-A中 特码B取不到大ID
					if(p.pathFolder == 'L_SIX'){
						var NumKey = Number(key);
						fBigId = sixBet.findBigId(szszObj, NumKey);
					}else{
						fBigId = szszObj.attr('data-did');
					}
					// 同大类下总投注额度
					_this.list["sumwin_" + fBigId] += Number(that.split(',')[1]);
					// 同大类下总退水额度
					_this.list["sumts_" + fBigId] += Number(that.split(',')[3]);
				}
			}
			// 基础球赔率&按虧損/球号額排列&其他赔率/无注盈亏
			var objCbm = $("#cbM"), objCbn = $("#cbN");
			var bbdsSum = 0, bbdsMinwin = 0, bbdsMaxwin = 0, bbdxSum = 0, bbdxMinwin = 0, bbdxMaxwin = 0;
			var objOdds = null;
			var objAmount = null;
			var objParent = null;
			var objSzsz = null;
			var bigId = '';
			var endAmountVal='', aTitle='', objCbmVal = objCbm.val(), objCbnVal = objCbn.val();
			for(var key in playodds){
				// 找到当前节点盈亏缓存对象
				var that = _this.myData[key];
				// 当前赔率节点
				objOdds = $("#odds_"+ key);
				objAmount = $("#amount_"+ key);
				objParent = null;
				if(p.pathFolder == 'L_SIX' && (p.playpage == 'lm' || p.playpage == 'lm_b' || p.playpage == 'lxl')){
					objParent = objAmount.parents('.oddsParent');
				}else{
					objParent = objOdds.parents('.oddsParent');
				}
				objSzsz = $("#szsz_"+ key);
				// 赔率
				if(playodds[key]['pl'].indexOf(',') > 0){
					var pOdds2 = playodds[key]['pl'].split(',');
					for(var i=0; i<pOdds2.length; i++){
						$("#odds_"+ key +"_"+ i).html( pOdds2[i] );
					}
				}else{
					objOdds.html( that['OddsValue'] );
				}

				bigId = '';
				// 六合彩总项一、二、特码AB-A中 特码B取不到大ID
				if(p.pathFolder == 'L_SIX'){
					var NumKey = Number(key);
					bigId = sixBet.findBigId(objSzsz, NumKey);
				}else{
					bigId = objSzsz.attr('data-did');
				}

				_this.nowAmountValue = 0;
				_this.nowRepValue = 0;
				_this.nowAmountNumber = 0;

				// 针对六合彩特码总项1，特码总项2，特码AB
				if(p.pathFolder == 'L_SIX'){
					sixBet.sixSetAmount(key, that);
				}else{
					_this.nowAmountValue = Number(that['AmountValue']);
					_this.nowRepValue = Number(that['RepValue']);
					_this.nowAmountNumber = Number(that['AmountNumber']);
				}

				var winNum = 0;
				// 盈亏计算
				if (bigId == '65' || bigId == '91009' || p.playpage == 'sxws' || p.playpage == 'sxwsbz' || bigId == '59') {
					// 正码/生肖尾数/快三 三军 盈亏一对一计算不杀其他
					winNum = - that['WinValue'] - that['RecValue'];
				}else{
					// 最后盈亏 = 总下注额 - 总水 - 当前盈亏(不包本) - 当前下注额
					winNum = _this.list["sumwin_" + bigId] - _this.list["sumts_" + bigId] - that['WinValue'] - that['AmountValue'];
				}
				// 缓存盈亏
				_this.numData[key] = winNum;
				if(key == '92565'){
					objSzsz.html( '-' );
				}else{
					if (ForDight2(winNum) == 0) {
						objSzsz.html("-");
					}else{
						objSzsz.html( parseInt(winNum) );
					}
				}

				// 计算半波总注额/最大盈亏
				if (p.playpage == 'bb') {
					var bbds = ['92122', '92123', '92126', '92127', '92130', '92131'];
					var bbdx = ['92120', '92121', '92124', '92125', '92128', '92129'];
					if ($.inArray(key, bbds) != -1) {
						bbdsSum += that['AmountValue'];
						if(bbdsMinwin > winNum){
							bbdsMinwin = winNum;
						}
						if(bbdsMaxwin < winNum){
							bbdsMaxwin = winNum;
						}
					}
					if($.inArray(key, bbdx) != -1){
						bbdxSum += that['AmountValue'];
						if(bbdxMinwin > winNum){
							bbdxMinwin = winNum;
						}
						if(bbdxMaxwin < winNum){
							bbdxMaxwin = winNum;
						}
					}
				}

				// 当有下注额，并且不是连码时绑定事件
				if(_this.nowAmountValue && !p.isLm && key != '92565'){
					if ((p.pathFolder != 'L_SIX' && p.usertype != '1') || p.pathFolder == 'L_SIX') {
						objSzsz.addClass('szszEvent');
					}
				}
				// 注额显示 這裡減了補貨了
				endAmountVal='';
				aTitle='';
				if(objCbmVal == '1' && objCbnVal == '0'){
					endAmountVal = parseInt(_this.nowAmountValue) + '/' + _this.nowRepValue;
					aTitle = '注額/補貨';
				}else if(objCbmVal == '0' && objCbnVal == '1'){
					endAmountVal = _this.nowAmountNumber + '/' + parseInt(_this.nowAmountValue);
					aTitle = '注數/注額';
				}else if(objCbmVal == '1' && objCbnVal == '1'){
					endAmountVal = _this.nowAmountNumber + '/' + parseInt(_this.nowAmountValue) + '/' + _this.nowRepValue;
					aTitle = '注數/注額/補貨';
				}else{
					endAmountVal = parseInt( _this.nowAmountValue );
					aTitle = '注額';
				}
				// 绑定注额明细
				objAmount.addClass('amountEvent');
				// 增加title添加数据
				if (_this.nowAmountValue == 0 && _this.nowAmountNumber == 0) {
					objAmount.html("-");
				}else{
					objAmount.attr("title", aTitle).html( endAmountVal );
				}
				// 負值標紅
				if(ForDight2(winNum) < 0){
					objSzsz.addClass('red');
				}else{
					objSzsz.removeClass('red');
				}
				if(winNum < 0){
					if( winNum < _this.minQuota ){
						_this.minQuota = winNum;
					}
					// 获取每个大类的最大负值
					if(Number(_this.list["minwin_" + bigId]) > Number(winNum)){
						_this.list["minwin_" + bigId] = winNum;
					}
				}else{
					if( winNum > _this.maxQuota ){
						_this.maxQuota = winNum;
					}
					// 获取每个大类的最大正值
					if(Number(_this.list["maxwin_" + bigId]) < Number(winNum)){
						_this.list["maxwin_" + bigId] = winNum;
					}
				}
				// 盈虧超額预警
				if(winNum < d.abovevalid[bigId]){
					objSzsz.parent().addClass('warning');
				}
				objParent.attr({
					'data-id': key,
					'data-win': parseInt( winNum ),
					'data-num': isNaN(Number(objOdds.attr('data-name'))) ? '' : Number(objOdds.attr('data-name'))
				});

				if(that['IsOpen'] == 1){
					objParent.removeClass('stopbg');
				}else{
					objParent.addClass('stopbg');
				}
				// 当前页面下总投注额
				sumQuota += _this.myData[key]['AmountValue'];
				sumRec += _this.myData[key]['RecValue'];
			}
			// 六合彩
			if(p.pathFolder == 'L_SIX'){
				sixBet.zxAmountAndSzsz( d );
			}
			// 更新赔率缓存数据
			_this.oddsData = playodds;
			// 是否为特码快速走飞
			if (_this.isTmFastFlyAway) {
				_this.bindTmFastFlyAway();
			}
			// 以下处理总额计算
			if(p.playpage != 'zm' && p.playpage != 'sxws' && p.playpage != 'sxwsbz'){
				// 总投注额
				$("#sumQuota").html( parseInt( sumQuota ) );
				// 总退水额度
				$("#sumRec").html( parseInt( sumRec ) );
				// 最高亏损
				$("#minQuota").html( parseInt( _this.minQuota ) );
				// 最高盈利
				$("#maxQuota").html( parseInt( _this.maxQuota ) );
			}else{
				// 正码/生肖尾数中/生肖尾数不中
				if (p.pathFolder == 'L_SIX') {
					sumZmSxWs();
				}
			}
			if(p.playpage != 'tmab' && p.playpage != 'tmzx1' && p.playpage != 'tmzx3' && p.playpage != 'tmzx2' && p.playpage != 'tmzx3'){
				// 最大正值
				$("#maxPos").html( parseInt( _this.maxQuota ) );
				// 最大负值
				$("#maxNeg").html( parseInt( _this.minQuota ) );
			}
			// 特碼生肖色波 计算总额/退水/最大盈亏
			if(p.playpage == 'tmsxsb' || p.playpage == 'tma' || p.playpage == 'tmb'){
				$(".listData").each(function () {
					var listThat = $(this);
					var thisDid = listThat.attr('data-did');
					var thisType = listThat.attr('data-type');
					listThat.html(parseInt( _this.list[thisType+'_'+thisDid]));
				});
			}
			// 半波
			if (p.playpage == 'bb') {
				$("#bbdsSum").html(parseInt(bbdsSum));
				$("#bbdsMinwin").html(parseInt(bbdsMinwin));
				$("#bbdsMaxwin").html(parseInt(bbdsMaxwin));
				$("#bbdxSum").html(parseInt(bbdxSum));
				$("#bbdxMinwin").html(parseInt(bbdxMinwin));
				$("#bbdxMaxwin").html(parseInt(bbdxMaxwin));
			}

			// pk10 xyft5
			if (p.pathFolder == 'L_PK10' || p.pathFolder == 'L_XYFT5' || p.pathFolder == 'L_JSCAR' || p.pathFolder == 'L_JSPK10' || p.pathFolder == 'L_JSFT2' || p.pathFolder == 'L_CAR168' || p.pathFolder == 'L_VRCAR' || p.pathFolder == 'L_XYFTOA' || p.pathFolder == 'L_XYFTSG') {
				if(p.playpage == 'p1'){
					$("#gjze").html(parseInt(_this.list["sumwin_1"] + _this.list["sumwin_2"] + _this.list["sumwin_3"] + _this.list["sumwin_4"]));
					$("#yjze").html(parseInt(_this.list["sumwin_5"] + _this.list["sumwin_6"] + _this.list["sumwin_7"] + _this.list["sumwin_8"]));
				}
				if(p.playpage == 'p2'){
					$("#d3ze").html(parseInt(_this.list["sumwin_9"] + _this.list["sumwin_10"] + _this.list["sumwin_11"] + _this.list["sumwin_12"]));
					$("#d4ze").html(parseInt(_this.list["sumwin_13"] + _this.list["sumwin_14"] + _this.list["sumwin_15"] + _this.list["sumwin_16"]));
					$("#d5ze").html(parseInt(_this.list["sumwin_17"] + _this.list["sumwin_18"] + _this.list["sumwin_19"] + _this.list["sumwin_20"]));
					$("#d6ze").html(parseInt(_this.list["sumwin_21"] + _this.list["sumwin_22"] + _this.list["sumwin_23"]));
				}
				if(p.playpage == 'p3'){
					$("#d7ze").html(parseInt(_this.list["sumwin_24"] + _this.list["sumwin_25"] + _this.list["sumwin_26"]));
					$("#d8ze").html(parseInt(_this.list["sumwin_27"] + _this.list["sumwin_28"] + _this.list["sumwin_29"]));
					$("#d9ze").html(parseInt(_this.list["sumwin_30"] + _this.list["sumwin_31"] + _this.list["sumwin_32"]));
					$("#d10ze").html(parseInt(_this.list["sumwin_33"] + _this.list["sumwin_34"] + _this.list["sumwin_35"]));
				}
			}
			// jsk3
			if(p.pathFolder == 'L_K3'){
				// 三军二骰三骰
				var k3Key = ['391','392','393','394','395','396'];
				var k3Jg = '';
				for(var i = 0; i<k3Key.length; i++){
					var nowWinValue = _this.numData[k3Key[i]];
					var kobj2 = $("#szsz_"+ k3Key[i] +"_2");
					var kobj3 = $("#szsz_"+ k3Key[i] +"_3");
					if (nowWinValue == 0) {
						kobj2.html("-");
						kobj3.html("-");
					}else{
						kobj2.html(parseInt(nowWinValue*2));
						kobj3.html(parseInt(nowWinValue*3));
					}
					if(nowWinValue<0){
						kobj2.addClass('red');
						kobj3.addClass('red');
					}else{
						kobj2.removeClass('red');
						kobj3.removeClass('red');
					}
					if($("#szsz_"+ k3Key[i]).hasClass('szszEvent')) {
						kobj2.addClass('szszEvent');
						kobj3.addClass('szszEvent');
					}else{
						kobj2.removeClass('szszEvent');
						kobj3.removeClass('szszEvent');
					}
				}
				// 三军退水总注额
				$("#sjtsze").html(parseInt(_this.list["sumwin_59"]-_this.list["sumts_59"]));
				// 大小最大盈利
				$("#dxyl").html(parseInt(_this.list["maxwin_58"]));
				// 混骰最大盈利
				$("#hsyl").html(parseInt(_this.list["maxwin_60"]));
				// 全骰最大盈利
				$("#qsyl").html(parseInt(_this.list["maxwin_61"]));
				// 点数最大盈利
				$("#dsyl").html(parseInt(_this.list["maxwin_62"]));
				// 长牌退水总注额
				$("#cptsze").html(parseInt(_this.list["sumwin_63"]-_this.list["sumts_63"]));
				// 短牌最大盈利
				$("#dpyl").html(parseInt(_this.list["maxwin_64"]));
			}
			// 以上处理总额计算

			// 綁定盈虧按鈕排序事件
			var descOrasc = $(".descOrasc");
			var descOrascNoAuto = $(".descOrascNoAuto");
			if(descOrascNoAuto.length > 0){
				// 控制需要生成盈亏的长度
				var autoLen = descOrascNoAuto.attr('data-len') - 0;
				var descOrascBtn = $('.descOrascBtn');
				if (_this.isSortBtn == '1') {
					descOrascNoAuto.mySort({
						attribute: 'data-win',
						descOrAsc: 'asc',
						dataObj: descOrasc,
						dataLen: autoLen
					});
				}else{
					descOrascNoAuto.mySort({
						attribute: 'data-win',
						descOrAsc: 'desc',
						dataObj: descOrasc,
						dataLen: autoLen
					});
				}
				descOrascBtn.unbind('click').click(function () {
					var sortBtn = $(this);
					var sortType = sortBtn.attr('data-id');
					$("#descOrascBtn").siblings('tbody').mySort({
						attribute: 'data-win',
						descOrAsc: sortType,
						dataObj: descOrasc,
						dataLen: autoLen
					});
					if (sortType == 'asc') {
						_this.isSortBtn = 1;
					}else{
						_this.isSortBtn = 0;
					}
					descOrascBtn.removeClass('active');
					sortBtn.addClass('active');
					// 找到上次补货窗口动作
					_this.flyAwayModuleFlyInit(_this.findLastObj(_this.flyAwayThat));
					_this.oddsTrimModuleInit(_this.findLastObj(_this.aOddsThat));
				});
			}

			// 特码总项一排序
			if(p.playpage == 'tmzx1'){
				var block_zx1 = $("#block_zx1");
				var blockZXList =  block_zx1.find('tr').toArray().sort(function(a,b){
					return (parseInt($(a).attr("data-win")) - parseInt($(b).attr("data-win")));
				});
				block_zx1.html(blockZXList);
				$(".numberId").each(function (i) {
					$(this).html(i+1);
				});
			}

			// 生肖尾数排序
			if ((p.playpage == 'sxws' || p.playpage == 'sxwsbz') && p.pathFolder == 'L_SIX') {
				var sxObj = $("#block1");
				var wsObj = $("#block3");
				var sxList = [];
				var wsList = [];
				if ($("input[name=sxwsSort]:checked").val()=='0') {
					sxList = sxObj.find('tr').toArray().sort(function(a,b){
						return (parseInt($(a).attr("data-sort")) - parseInt($(b).attr("data-sort")));
					});
					wsList = wsObj.find('tr').toArray().sort(function(a,b){
						return (parseInt($(a).attr("data-sort")) - parseInt($(b).attr("data-sort")));
					});
				}else{
					sxList = sxObj.find('tr').toArray().sort(function(a,b){
						return (parseInt($(a).attr("data-win")) - parseInt($(b).attr("data-win")));
					});
					wsList = wsObj.find('tr').toArray().sort(function(a,b){
						return (parseInt($(a).attr("data-win")) - parseInt($(b).attr("data-win")));
					});
				}
				sxObj.html(sxList);
				wsObj.html(wsList);
			}

			// 排序后绑定补货事件
			$('.tableBody').undelegate('.szszEvent', 'click');
			if(p.issllowsale == '1' && $("#upActual").val() == '1' && $("#sltPlaytype").val() != '2'){
				// 找到上次补货窗口动作
				_this.flyAwayModuleFlyInit(_this.findLastObj(_this.flyAwayThat));

				$('.tableBody').delegate('.szszEvent', 'click', function () {
					var that = $(this);
					_this.flyAwayThat = [that.attr('id'), that.attr('data-szsz')];
					_this.aOddsThat = [];
					_this.flyAwayModuleFlyInit(that);
				});
			}else{
				$('.tableBody').delegate('.szszEvent', 'click', function () {
					var that = $(this);
					if ($("#upActual").val() != '1') {
						tips.msgTips({
							msg: '請在實貨下操作！',
							type : "error"
						});
					}else if(p.issllowsale != '1'){
						tips.msgTips({
							msg: '沒有權限！',
							type : "error"
						});
					}else if($("#sltPlaytype").val() == '2'){
						tips.msgTips({
							msg: '請在一般或免傭下操作!',
							type : "error"
						});
					}
				});
			}
			// 为快捷栏更新前三和前十的数组数据和生效对应数据
			if(p.pathFolder == 'L_SIX'){
				var znData = returnZodiacNumber(49, 'num');
				for(var i=1; i<=12; i++){
					p.shortcutData['zodiac'+i] = znData[i+''];
				}
				for(var i=0; i<3; i++){
					if(p.playpage == 'tmzx1'){
						p.shortcutData['ranking'+3].push(Number($("#block_zx1 tr").eq(i).attr('data-num')));
					}else{
						p.shortcutData['ranking'+3].push(Number($(".descOrascNoAuto tr").eq(i).attr('data-num')));
					}
				}
				for(var i=0; i<10; i++){
					if(p.playpage == 'tmzx1'){
						p.shortcutData['ranking'+10].push(Number($("#block_zx1 tr").eq(i).attr('data-num')));
					}else{
						p.shortcutData['ranking'+10].push(Number($(".descOrascNoAuto tr").eq(i).attr('data-num')));
					}
				}
			}

			// 找到上次賠率窗口動作
			_this.oddsTrimModuleInit(_this.findLastObj(_this.aOddsThat));
			// 绑定赔率事件
			_this.bindOddsHandle(d);
			// 连码补货
			if(p.isLm){
				// 关闭六合彩连码总监快速走飞
				if (p.usertype != '1') {
					$(".fastbh").show();
				}else{
					$(".fastbh").hide();
				}
				if (p.pathFolder == 'L_SIX') {
					if(p.playpage == 'lm' || p.playpage == 'lm_b' || p.playpage == 'bz'){
						// 处理连码和不中
						sixBet.sixSetLm(d);
					}else if(p.playpage == 'lxl'){
						// 处理六肖连
						sixBet.sixSetLm(d);
						sixBet.sixSetLxl(d);
					}
				}else{
					var Details = $("#Details");
					var zj = $("#zj");
					var nozj = $("#nozj");
					_this.getLmBhList(function (data) {
						if(data.hasOwnProperty("lmbhlist")){
							var lmbhlist = data.lmbhlist;
							var html = '';
							var i = 0;
							var calculateInput = $("#calculateInput");
							var radioKey = $("input[name=radiobutton]:checked").attr('data-radio');
							var radioName = $("input[name=radiobutton]:checked").attr('data-name');
							for(var key in lmbhlist){
								var nameId = key.split(',').join('_');
								i++;
								if(p.issllowsale == '1'){
									zj.hide();
									nozj.show();
									var highBg = '';
									if(_this.flyAwayThat[1] == key){
										highBg = ' highBg';
									}
									html += '<tr data-key="'+ nameId +'" class="bhlbClass'+ highBg +'">'+
											'<td>'+ i +'</td>'+
											'<td>'+ key +'</td>'+
											'<td>'+ lmbhlist[key].split(',')[0] +'</td>'+
											'<td class="green">'+ lmbhlist[key].split(',')[2] +'</td>'+
											'<td class="red">-'+ parseInt(lmbhlist[key].split(',')[1]) +'</td>'+
											'<td><a id="kclmbhlb_'+ i +'" href="javascript:;" class="blue lmbhBtn" data-amount="'+ lmbhlist[key].split(',')[0] +'" data-key="'+ key +'" data-szsz="'+ radioKey +'" data-name="'+ radioName +'">補貨</a></td>'+
										'</tr>';
								}else{
									zj.show();
									nozj.hide();
									html += '<tr><td>'+ i +
										'</td><td>'+ key +
										'</td><td>'+ lmbhlist[key].split(',')[0] +
										'</td><td class="green">'+ lmbhlist[key].split(',')[2] +
										'</td><td class="red">-'+ lmbhlist[key].split(',')[1] +
										'</td></tr>';
								}
							}
							Details.html( html );
							// 重新设置iframe高度
							p.setIframeHeight();
							// 单走飞
							$(".lmbhBtn").unbind('click').bind('click', function () {
								Betimes.flyAwayThat = [];
								isUpdataFlyAway = false;
								Betimes.flyAwayTargetId = '';
								_this.lmSzszHandle($(this));
							});
							// 绑定快速走飞
							if(Details.html() !='' && p.issllowsale == '1'){
								$(".fastbh").unbind('click').bind('click', function () {
									_this.bindLmFastFlyAway();
								});
							}

						}else{
							// 当封盘转为开盘时清空
							Details.html('');
							_this.LmInputCacheObject = {};
						}
					});
				}
			}
			if (d.cleardata == '0') {
				loadPage(20);
				_this.isFirst++;
				oneTime++;
			}
			objCbm.prop('disabled', false);
			objCbn.prop('disabled', false);
			$("input[name=sxwsSort]").prop('disabled', false);
			// 重新设置iframe高度
			p.setIframeHeight();
		},
		//快彩连码获取列表
		getLmBhList: function (callback) {
			var postData = {};

			var radioKey = $("input[name=radiobutton]:checked").attr('data-radio');

			if(!radioKey){
				return;
			}

			if(p.pathFolder == 'L_SIX'){
				// 六合彩请求参数
				postData = {
					'action': 'get_szszlmbh',
					'oddsid': radioKey,
					'phaseid': $('#nn').attr('data-pId'),
					'pk': $("#upHandicap").val(),
					'playid': $("#szsz_" + radioKey).attr('data-did'),
					'szsz': $("#upActual").val()
				};
				if(p.playpage == 'lxl'){
					postData['action'] = 'get_szszlxlbh';
				}
			}else{
				// 快彩请求参数
				postData = {
					'action': 'get_lmbhlist',
					'playid': $("#szsz_" + radioKey).attr('data-did'),
					'szsz': $("#upActual").val(),
					'pk': $("#upHandicap").val(),
					'numsort': 1,
					'isbh': $("#cbM").val(),
					'oddsid': radioKey,
					'phaseid': $('#nn').attr('data-pId')
				};
			}

			var b = new getBaseDataAjax({
				url: 'Handler/Handler.ashx',
				_type: 'POST',
				dataType: 'json',
				postData: postData,
				async: true,
				completeCallBack:function () {},
				successCallBack:function (data) {
					callback(data.data);
				},
				errorCallBack:function () {}
			});
		},
		// 绑定特码快速走飞
		bindTmFastFlyAway:function () {
			// 异步加载走飞组件
			if($("#upActual").val() == '1'){
				require.async('fastFlyAway', function (fastFlyAway) {
					fastFlyAway.tmFastFlyAway();
				});
			}
		},
		// 绑定连码快速走飞
		bindLmFastFlyAway: function () {
			var _this = this;
			// 异步加载走飞组件
			if($("#upActual").val() == '1'){
				require.async('fastFlyAway', function (fastFlyAway) {
					_this.getLmBhList(function (data) {
						fastFlyAway.lmFastFlyAway(data);
					});
				});
			}
		},
		// 绑定赔率事件
		bindOddsHandle: function (d) {
			var _this = this;
			var tableBody = $('.tableBody');
			var addBtns = $('.addBtns');
			var minBtns = $('.minBtns');
			// 賠率事件綁定
			// if( p.usertype == '1'){
				if( p.isopt == '1'){
					var tip = null;
					// 弹出层设置赔率
					tableBody.undelegate('.oddsTrim', 'click');
					tableBody.delegate('.oddsTrim', 'click', function () {
						var that = $(this);
						_this.aOddsThat = [that.attr('id')];
						_this.flyAwayThat = [];
						isUpdataFlyAway = true;
						_this.oddsTrimModuleInit(that);
					});
					// 加号设置赔率
					tableBody.undelegate('.addBtns', 'click');
					tableBody.delegate('.addBtns', 'click', function () {
						var that = $(this).siblings('a');
						var key = that.attr('data-odds');
						var difference = $('#tool_input').val()-0 || 0.01;
						if(ForDight4(Number(that.html() - 0 + difference)) > Number(_this.myData[key]['OddsMaxValue'])){
							tip = tips.msgTips({
								msg: '當前修改賠率不能大於系統設定最大賠率:'+ _this.myData[key]['OddsMaxValue'] +'！',
								type : "error"
							});
						}else{
							$(this).addAndSubLoading();
							_this.baseOddsAjax('1', key, difference, '');
						}
					}).show();
					// 减号设置赔率
					tableBody.undelegate('.minBtns', 'click');
					tableBody.delegate('.minBtns', 'click', function () {
						var that = $(this).siblings('a');
						var key = that.attr('data-odds');
						var difference = $('#tool_input').val()-0 || 0.01;
						if(Number(accSub(Number(that.html()), difference)) < Number(_this.myData[key]['OddsMinValue'])){
							tip = tips.msgTips({
								msg: '當前修改賠率不能小於'+ _this.myData[key]['OddsMinValue'] +'！',
								type : "error"
							});
						}else{
							$(this).addAndSubLoading();
							_this.baseOddsAjax('2', key, difference, '');
						}
					}).show();
					// 单球开关
					if(p.usertype == '1'){
						tableBody.undelegate('.ballOpenBtn', 'click');
						tableBody.delegate('.ballOpenBtn', 'click', function () {
							var $this = $(this);
							var $thisTr = $this.parents('.oddsParent').eq(0);
							var $that = $thisTr.find('.oddsTrim');
							if(d.openning == 'y'){
								_this.baseOddsAjax('4', $that.attr('data-odds'), '', '');
							}else{
								tip = tips.msgTips({
									msg: '封盤狀態下不能操作！',
									type : "error"
								});
							}
						});
					}
				}else{
					tableBody.undelegate('.oddsTrim', 'click');
					addBtns.hide();
					minBtns.hide();
				}
		},
		// 找上一個節點(赔率、补货弹窗)
		findLastObj: function (arr) {
			if(arr.length>0) {
				return $('#'+arr[0]);
			}else{
				return null;
			}
		},
		// 補貨彈窗函數
		flyAwayModuleFlyInit:function (that) {
			var _this = this;
			if(!that){

			}else{
				if(that.length > 0){

					if(p.isLm == '1'){
						that.parents('tr').eq(0).siblings('tr').removeClass('highBg');
						that.parents('tr').eq(0).addClass('highBg');
					}
					flyAwayModule._flyInit({
						target: that,
						okCallBack: function (data) {
							function isSubmit() {
								var oddsData = Betimes.oddsData[data.oddsid];
								var isOddsOk = false;
								var nowThis = null;
								var i = 0;
								var maxOdds = 0;
								var minOdds = 0;
								$(".flyAwayOdds").each(function () {
									nowThis = $(this);
									i = Number(nowThis.attr('data-currindex'));
									if(p.usertype == '1'){
										maxOdds = 1000;
										minOdds = 0;
									}else{
										maxOdds = Number(oddsData['maxpl'].split(',')[i]);
										minOdds = Number(oddsData['minpl'].split(',')[i]);
									}
									if(Number(nowThis.val()) > maxOdds && p.pathFolder == 'L_SIX'){
										tips.msgTips({
											msg: '當前修改賠率不能大於系統設定最大賠率:'+ parseInt(maxOdds) +'！',
											type : "error"
										});
										nowThis.focus();
										isOddsOk = false;
									}else if(Number(nowThis.val()) < minOdds && p.pathFolder == 'L_SIX'){
										tips.msgTips({
											msg: '當前修改賠率不能小於:'+ parseInt(minOdds) +'！',
											type : "error"
										});
										nowThis.focus();
										isOddsOk = false;
									}else{
										isOddsOk = true;
									}
								});
								return isOddsOk;
							}
							if(_this.isopenning == 'y'){
								if((data.amount > data.max) && p.negativesale != '1'){
									tips.msgTips({
										msg: '當前補貨金額不能大於注額:'+ parseInt(data.max) +'！',
										type : "error"
									});
								}else if(data.amount == ''){
									tips.msgTips({
										msg: '補貨金額不能為空！',
										type : "error"
									});
								}else if(data.amount < top.saleMin_six && p.pathFolder == 'L_SIX'){ // 六
									tips.msgTips({
										msg: '當前補貨金額不能小於:'+ top.saleMin_six +'！',
										type : "error"
									});
								}else if(data.amount < top.saleMin_kc && p.pathFolder != 'L_SIX'){ // 快
									tips.msgTips({
										msg: '當前補貨金額不能小於:'+ top.saleMin_kc +'！',
										type : "error"
									});
								}else{
									if(isSubmit()){
										$("#flyAway").addClass('oTload');
										_this.baseSzszAjax(data);
										_this.flyAwayThat = [];
									}
								}
							}else{
								tips.msgTips({
									msg: '已經封盤！',
									type : "error"
								});
							}
						}
					});
				}else{
					$("#flyAway").hide();
				}
			}
		},
		// 補貨ajax函数
		baseSzszAjax: function (data, nowIndex, callback) {
			var _this = this;
			var postData = {
				'action': data.action,
				'phaseid': $('#nn').attr('data-pId'),
				'pk': $("#upHandicap").val(),
				'oddsid': data.oddsid,
				'currentodds': data.currentodds,
				'amount': data.amount,
				'number': data.number,
				'fast': data.fast,
				's_user': data.s_user,
				'playtype': data.playtype,
				'jeucode': p.jeucode
			}
			// 是否负值走飞
			// if(p.usertype == '1' && p.negativesale == '1'){

			// }
			var b = new getBaseDataAjax({
				url: 'Handler/Handler.ashx',
				postData: postData,
				completeCallBack:function () {},
				successCallBack:function (d) {
					if (data.fast == 2) {
						_this.isTmFastFlyAway = false;
					}

					// 快补
					if(data.fast == 1){
						$("#result_" + nowIndex).html('補貨成功！');
					}else{
						// 关闭之前对话框
						if(data.fast == 2){
							$.myLayer.close(true, window.top.myLayerIndex);
						}
						var html = '';
						if( p.isLm == '1'){
							var lmBhThat = _this.findLastObj(_this.flyAwayThat);
							if(lmBhThat){
								lmBhThat.parents('tr').eq(0).removeClass('highBg');
							}
							html = '<table id="bhdata" class="t_list">'+
										'<thead>'+
											'<tr>'+
												'<th>單號</th>'+
												'<th>明細</th>'+
												'<th>補出金額</th>'+
												'<th>可赢</th>'+
												'<th>结果</th>'+
											'</tr>'+
										'</thead>'+
										'<tbody>'+
											'<tr>'+
												'<td>'+ d.data.bhdata.ordernum +'</td>'+
												'<td>' + d.data.bhdata.playname +'【'+ d.data.bhdata.putamount +'】@<span class="red">'+ d.data.bhdata.pl +'</span></td>'+
												'<td><span class="blue">'+ d.data.bhdata.amount +'</span></td>'+
												'<td><span class="blue">'+ d.data.bhdata.ky +'</span></td>'+
												'<td>補貨成功</td>'+
											'</tr>'+
										'</tbody>'+
									'</table>';
						}else{
							var bhdata = d.data.bhdata;
							var listHtml = '';
							var msg = '';
							var list = null;
							for(var i=0; i<bhdata.length; i++){
								list = bhdata[i];
								msg = '';
								if(list['success'] == '1'){
									msg = '補貨成功';
								}else{
									msg = '補貨失敗：' + list['message'];
								}
								listHtml += '<tr>'+
												'<td>'+ list['ordernum'] +'</td>'+
												'<td>' + list['playname'] +'【'+ list['putamount'] +'】@<span class="red">'+ list['pl'] +'</span></td>'+
												'<td><span class="blue">'+ list['amount'] +'</span></td>'+
												'<td><span class="blue">'+ list['ky'] +'</span></td>'+
												'<td>' + msg + '</td>'+
											'</tr>';
							}
							html = '<table id="bhdata" class="t_list">'+
											'<thead>'+
												'<tr>'+
													'<th>單號</th>'+
													'<th>明細</th>'+
													'<th>補出金額</th>'+
													'<th>可赢</th>'+
													'<th>结果</th>'+
												'</tr>'+
											'</thead>'+
											'<tbody>'+
												listHtml+
											'</tbody>'+
										'</table>';
						}


						$("#amount_" + data.oddsid).myLayer({
							title: '提示信息',
							content: html,
							isMiddle: true,
							okText: '確定',
							okCallBack: function (obj) {
								$.myLayer.close(true, obj.attr('data-index'));
							}
						});
						if(data.fast != 2){
							_this.flyAwayThat = [];
							isUpdataFlyAway = false;
							// _this.clearAmoutData();
							$("#flyAway").removeClass('oTload').hide();
						}

						_this.oddsAjax($("#upSecond").val()-0 );
					}
					if(d.data.hasOwnProperty('jeucode')){
						p.jeucode = d.data.jeucode;
					}

				},
				errorCallBack:function (d) {},
				otherErrorCallBack:function (d) {
					if (data.fast == 2) {
						_this.isTmFastFlyAway = false;
					}

					if(data.fast == 1 && p.pathFolder != 'L_SIX'){
						$("#result_" + nowIndex).html('<div class="red">'+ d.Tipinfo +'</div>');
					}else{
						$.myLayer.close(true, window.top.myLayerIndex);
						tips.msgTips({
							msg: d.Tipinfo,
							type : "error"
						});
						_this.flyAwayThat = [];
						isUpdataFlyAway = false;
						// _this.clearAmoutData();
						$("#flyAway").removeClass('oTload').hide();
					}
					if(d.data.hasOwnProperty('jeucode')){
						p.jeucode = d.data.jeucode;
					}
					$("#flyAway").removeClass('oTload');
				},
				oddsErrorCallBack:function (d) {
					if (data.fast == 2) {
						_this.isTmFastFlyAway = false;
					}

					if(data.fast == 1 && p.pathFolder != 'L_SIX'){
						$("#result_" + nowIndex).html(d.Tipinfo);
					}else{
						data.currentodds = d.data.oddschange.newodds;
						// console.log(d.data.oddschange.newodds, data, d);
						$("#amount_" + data.oddsid).myLayer({
							title: '提示信息',
							content: d.Tipinfo,
							isMiddle: true,
							okText: '確定',
							okCallBack: function (obj) {
								_this.baseSzszAjax( data );
								$.myLayer.close(true, obj.attr('data-index'));
							},
							closeCallBack: function() {
								_this.flyAwayThat = [];
								isUpdataFlyAway = false;
								// _this.clearAmoutData();
								$("#flyAway").removeClass('oTload').hide();
							}
						});
					}
					if(d.data.hasOwnProperty('jeucode')){
						p.jeucode = d.data.jeucode;
					}
				},
				endCallBack: function () {
					if(!callback){
						return;
					}else{
						callback();
					}
				}
			});
		},
		// 賠率彈窗函數
		oddsTrimModuleInit: function (that) {
			var _this = this;
			if(!that){
				return;
			}
			var key = that.attr('data-odds');
			var isWt = that.hasClass('wt_oddsTrim');
			var oddsData = {};
			if(isWt){
				oddsData = sixBet.sixOddsData[key];
			}else{
				oddsData = _this.myData[key];
			}
			oddsTrimModule._init({
				target: that,
				okCallBack: function (v) {
					if(Number(v) < Number(oddsData['OddsMinValue'])){
						tips.msgTips({
							msg: '當前修改賠率不能小於:'+ oddsData['OddsMinValue'] +'！',
							type : "error"
						});
					}else if( Number(v) > Number(oddsData['OddsMaxValue'])){
						tips.msgTips({
							msg: '當前修改賠率不能大於系統設定最大賠率:'+ oddsData['OddsMaxValue'] +'！',
							type : "error"
						});
					}else{
						$("#oddsTrim").addClass('oTload');
						_this.aOddsThat = [];
						if(isWt){
							_this.baseOddsAjax('3', that.attr('data-tOdds'), 0, v, '', oddsData['number']);
						}else{
							_this.baseOddsAjax('3', key, 0, v);
						}
					}
				}
			});
		},
		// 调整赔率ajax函数
		baseOddsAjax: function (optype, oddsid, wtvalue, inputvalue, isopen, number, did) {
			var _this = this;
			var tip = null;
			var arrOddsId = oddsid.split(',');
			var postOddsData = {};
			postOddsData.action = 'operate_adds';
			postOddsData.optype = optype;
			postOddsData.oddsid = oddsid;
			postOddsData.phaseid = $('#nn').attr('data-pId');
			postOddsData.pk =$("#upHandicap").val();
			// 处理双赔率
			var newOdds = oddsid;
			if(p.pathFolder == 'L_SIX'){
				if(arrOddsId.length == 1){
					var oddsindex = '0';
					if(oddsid.indexOf('_') > 0){
						newOdds = oddsid.split('_')[0];
						oddsindex = oddsid.split('_')[1];
					}
					if(number){
						postOddsData.number = number;
					}
					postOddsData.oddsid = newOdds;
					postOddsData.oddsindex = oddsindex;
				}
			}
			// 处理+-调整赔率1,2 快捷栏+-调整赔率
			if(optype == '1' || optype == '2' || optype == '5' || optype == '6'){
				postOddsData.wtvalue = wtvalue ? wtvalue : '';
			}
			// 处理弹窗调整赔率3 快捷栏输入框调整赔率
			if(optype == '3' || optype == '7'){
				postOddsData.inputvalue = inputvalue ? inputvalue : '';
			}
			// 处理开封盘
			if(optype == '4' || optype == '8' || optype == '61' || optype == '62' || optype == '63' || optype == '64' || optype == '65'){
				postOddsData.action = 'operate_closeopen';
				postOddsData.isopen = isopen ? isopen : '';
				// 六合彩单独调整赔率optype变化
				if(p.pathFolder == 'L_SIX' && (optype == '4' || optype == '8')){
					if(optype == '8'){
						postOddsData.optype = '64';
					}else if(optype == '4'){
						postOddsData.optype = '63';
					}
				}
				// 处理六合彩单独页面开关
				if(optype == '62'){
					var newPlayids = '';
					if(p.playpage == 'tmzx2' || p.playpage == 'tmzx2'){
						newPlayids = p.playids.replace("91002,","");
					}else{
						newPlayids = p.playids;
					}
					postOddsData.playid = newPlayids;
					if(did){
						postOddsData.playid = did;
					}
				}
			}

			var b = new getBaseDataAjax({
				url: 'Handler/Handler.ashx',
				postData: postOddsData,
				completeCallBack:function () { 
					for(var i = 0; i<arrOddsId.length; i++){
						if(optype == '1' || optype == '2'){
							if(number){
								$(".wt_addBtns[data-num="+ number +"]").siblings(".addSubLoading").remove();
								$(".wt_addBtns[data-num="+ number +"]").myxTips({
									content: '操作超時！' 
								});
							}else{
								$("a[data-odds="+ arrOddsId[i] +"]").siblings(".addSubLoading").remove();
								$("a[data-odds="+ arrOddsId[i] +"]").myxTips({
									content: '操作超時！' 
								});
							}
						}
					}
					if(optype == '3'){
						$("#oddsTrim").removeClass('oTload');
						$("#oddsTrim").myxTips({
							content: '操作超時！' 
						});
					}
					// 六合彩连码微调值
					if(oddsid == $("#lmSelect").val() && number){
						$("#odds_" + number).siblings('.addSubLoading').remove();
						$("#odds_" + number).myxTips({
							content: '操作超時！' 
						});
					}
				},
				successCallBack:function (d) {
					if(optype == '61'){ //处理六合彩全开全关
						// 重新加载赔率接口
						_this.oddsAjax($("#upSecond").val()-0);
					}else if(optype == '62'){ //处理六合彩当前页面
						// 重新加载赔率接口
						_this.oddsAjax($("#upSecond").val()-0);
					}else if(optype == '65'){ //处理六合彩非特
						// 重新加载赔率接口
						_this.oddsAjax($("#upSecond").val()-0);
					}else{ // 处理连码和六合彩单独和批量

						for(var i = 0; i<arrOddsId.length; i++){
							var objOdds = $("a[data-odds="+ arrOddsId[i] +"]");
							var thatTr = objOdds.parents('.oddsParent');
							if(optype == '1' || optype == '2'){
								objOdds.siblings(".addSubLoading").remove();
							}
							if(optype == '4'){
								if(thatTr.hasClass('stopbg')){
									thatTr.removeClass('stopbg');
								}else{
									thatTr.addClass('stopbg');
								}
							}
							if(optype == '8'){
								if(isopen == '0'){
									thatTr.addClass('stopbg');
								}else{
									thatTr.removeClass('stopbg');
								}
							}
						}
						// 六合彩连码微调值
						if(d.data.hasOwnProperty('wtnumbervalue') && oddsid.split('_')[0] == $("#lmSelect").val()){
							// var num = number<10?'0'+number:number;
							$("#odds_"+ oddsid).siblings('.addSubLoading').remove();
							sixBet.sixSetLm(d.data);
						}

						// 六肖
						if(oddsid == '92565'){
							$("#odds_"+ oddsid).siblings('.addSubLoading').remove();
							sixBet.sixSetLm(d.data);
						}

						if(optype == '3'){
							$("#oddsTrim").removeClass('oTload').hide();
						}
						// 循环开始
						var playodds = d.data.play_odds;
						var sIdHtml = '';
						var aOdds = null;
						var tr = null;
						var obj = null;
						var arrSid = [];
						for(var key in playodds){
							sIdHtml = playodds[key]['pl'];
							aOdds = $("a[data-odds="+ key +"]");
							tr = aOdds.parents('.oddsParent');
							if(sIdHtml.toString().indexOf(',')>0){
								arrSid = sIdHtml.split(',');
								for(var i=0; i<arrSid.length; i++){
									obj = $("#odds_"+key+"_"+i);
									obj.html(arrSid[i]).siblings('.addSubLoading').remove();
									_this.myData[key+'_'+i]['OddsMaxValue'] = playodds[key]['maxpl'].split(',')[i];
									_this.myData[key+'_'+i]['OddsMinValue'] = playodds[key]['minpl'].split(',')[i];
									_this.myData[key+'_'+i]['OddsMaxValue'] = playodds[key]['maxpl'].split(',')[i];
								}
							}else{
								aOdds.html( sIdHtml );
								_this.myData[key]['OddsMaxValue'] = playodds[key]['maxpl'];
								_this.myData[key]['OddsMinValue'] = playodds[key]['minpl'];
							}
							if(playodds[key]['is_open'] == '1'){
								tr.removeClass('stopbg');
							}else{
								tr.addClass('stopbg');
							}
							_this.oddsData[key]['pl'] = sIdHtml;
							_this.myData[key]['OddsValue'] = sIdHtml;
						}
					}
				},
				otherErrorCallBack: function (d) {
					for(var i = 0; i<arrOddsId.length; i++){
						if(optype == '1' || optype == '2'){
							if(number){
								$(".wt_addBtns[data-num="+ number +"]").siblings(".addSubLoading").remove();
							}else{
								$("a[data-odds="+ arrOddsId[i] +"]").siblings(".addSubLoading").remove();
							}
						}
					}
					if(optype == '3'){
						$("#oddsTrim").removeClass('oTload');
					}
					// 六合彩连码微调值
					if(oddsid == $("#lmSelect").val()){
						$("#odds_" + number).siblings('.addSubLoading').remove();
					}
					tips.msgTips({
						msg: d.Tipinfo,
						type : "error"
					});
				},
				errorCallBack:function () {
					for(var i = 0; i<arrOddsId.length; i++){
						if(optype == '1' || optype == '2'){
							if(number){
								$(".wt_addBtns[data-num="+ number +"]").siblings(".addSubLoading").remove();
								$(".wt_addBtns[data-num="+ number +"]").myxTips({
									content: '操作失敗！' 
								});
							}else{
								$("a[data-odds="+ arrOddsId[i] +"]").siblings(".addSubLoading").remove();
								$("a[data-odds="+ arrOddsId[i] +"]").myxTips({
									content: '操作失敗！' 
								});
							}
						}
					}
					if(optype == '3'){
						$("#oddsTrim").removeClass('oTload');
						$("#oddsTrim").myxTips({
							content: '操作失敗！' 
						});
					}
					// 六合彩连码微调值
					if(oddsid == $("#lmSelect").val() && number){
						$("#odds_" + number).siblings('.addSubLoading').remove();
						$("#odds_" + number).myxTips({
							content: '操作失敗！' 
						});
					}
				}
			});
		},
		// 更新类型
		upType: function () {
			var _this = this;
			var second = $("#upSecond");
			var actual = $("#upActual");
			var sltPlaytype = $("#sltPlaytype");
			var handicap = $("#upHandicap");
			var cbN = $("#cbN");
			var cbM = $("#cbM");
			var sxwsSort = $("input[name=sxwsSort]");
			var sxwsSortVal = sxwsSort.val();

			setUpType(second);
			setUpType(actual);
			setUpType(sltPlaytype);
			setUpType(handicap);
			setCheckUpType(cbN);
			setCheckUpType(cbM);

			second.change(function () {
				setUpTypeCookie($(this));
			});
			actual.change(function () {
				p.oldId = 0;
				$("#flyAway").removeClass('oTload').hide();
				setUpTypeCookie($(this));
			});
			sltPlaytype.change(function () {
				p.oldId = 0;
				_this.flyAwayThat = [];
				isUpdataFlyAway = true;
				$("#flyAway").hide();
				setUpTypeCookie($(this));
			});
			handicap.change(function () {
				p.oldId = 0;
				setUpTypeCookie($(this));
			});

			cbN.click(function () {
				var that = $(this);
				p.oldId = 0;
				if(that.prop('checked')){
					that.val('1');
				}else{
					that.val('0');
				}
				cbN.prop('disabled', true);
				setUpTypeCookie(that);
			});

			sxwsSort.click(function () {
				var that = $(this);
				if (sxwsSortVal != that.val()) {
					sxwsSortVal = that.val();
					sxwsSort.prop('disabled', true);
					p.oldId = 0;
					_this.clearAmoutData();
					_this.oddsAjax(second.val()-0);
				}
			});

			cbM.click(function () {
				var that = $(this);
				p.oldId = 0;
				if(that.prop('checked')){
					that.val('1');
				}else{
					that.val('0');
				}
				cbM.prop('disabled', true);
				setUpTypeCookie(that);
			});

			function setUpType (obj) {
				var lmName = '';
				if (p.isLm == '1') {
					lmName = '_lm';
				}
				try {
					obj.find("option[value=" + $.cookie(obj.attr('id') + lmName) + "]").prop("selected","selected");
				}catch (e) {}
			}

			function setCheckUpType (obj) {
				if($.cookie(obj.attr('id')) == '1'){
					obj.prop('checked', true).val('1');
				}else{
					obj.prop('checked', false).val('0');
				}
			}

			function setUpTypeCookie (obj) {
				var lmName = '';
				if (p.isLm == '1') {
					lmName = '_lm';
				}
				$.cookie(obj.attr('id')+lmName, obj.val());
				clearTimeout(p.timer);
				if(p.oldId == 0){
					_this.clearAmoutData();
				}
				_this.oddsAjax(second.val()-0);
			}

			//连码
			$("input[name=radiobutton]").unbind('click').bind('click', function () {
				$(".faseName").html($(this).attr('data-name'));
				_this.oddsAjax(second.val()-0);
				// 弹窗对象容器
				_this.aOddsThat = [];
				$("#oddsTrim").hide();
				// 走飞对象容器
				_this.flyAwayThat = [];
				isUpdataFlyAway = true;
				$("#flyAway").hide();
			});
		},
		// 处理六合彩单独开封盘
		openCloseHandle: function () {
			var _this = this;
			//
			if (p.usertype == '1') {
				// 绑定全开
				$("#allOpen").unbind('click').bind('click', function () {
					_this.baseOddsAjax('61', '', '', '', '1');
				});
				// 绑定全关
				$("#allClose").unbind('click').bind('click', function () {
					_this.baseOddsAjax('61', '', '', '', '0');
				});
				// 绑定非特开
				$("#nTeOpen").unbind('click').bind('click', function () {
					_this.baseOddsAjax('65', '', '', '', '1');
				});
				// 绑定非特关
				$("#nTeClose").unbind('click').bind('click', function () {
					_this.baseOddsAjax('65', '', '', '', '0');
				});
				// 当前页面开
				$("#nowPageOpen").unbind('click').bind('click', function () {
					_this.baseOddsAjax('62', '', '', '', '1');
				});
				// 当前页面开
				$("#nowPageClose").unbind('click').bind('click', function () {
					_this.baseOddsAjax('62', '', '', '', '0');
				});
				// 当前玩法开
				$(".didOpen").unbind('click').bind('click', function () {
					_this.baseOddsAjax('62', '', '', '', '1', '', $(this).attr('data-did'));
				});
				// 当前玩法关
				$(".didClose").unbind('click').bind('click', function () {
					_this.baseOddsAjax('62', '', '', '', '0', '', $(this).attr('data-did'));
				});
			}else{
				$("#allOpen, #allClose, #nTeOpen, #nTeClose, #nowPageOpen, #nowPageClose, .didClose, .didOpen, .burster").hide();
				$("#ShortcutOpen").parent().hide();
			}
		},
		// 连码补货事件绑定
		lmSzszHandle: function (that) {
			var _this = this;
			if($("#upActual").val() == '1'){
				_this.flyAwayThat = [that.attr('id'), that.attr('data-key')];
				_this.aOddsThat = [];
				_this.flyAwayModuleFlyInit(that);
			}
		}
	};
	/*
	 * [六合彩模块 需jQuery]
	 */
	var sixBet = {
		// 六合彩分页菜单 为了处理总监分公司 tmzx2/tmzx2_2,tmzx3
		subnavLink: function () {
			var oZx2 = $("#zx2");
			if(p.usertype == '1'){
				oZx2.attr('data-src', 'Betimes_tmZX2.aspx');
			}else{
				// oZx2.attr('data-src', 'Betimes_tmZX2_2.aspx');
				oZx2.attr('data-src', 'Betimes_tmZX2.aspx');
			}

			$("#subNav li").unbind('click').click(function () {
				// if(!$(this).hasClass('on')){
				// 	p.isClickAgin = true;
				// }
				// if (p.isClickAgin) {
				// 	p.isClickAgin = false;
					// 首次加载清空
					Betimes.isFirst = 1;
					oneTime = 0;
					Betimes.clearAmoutData();
					var src = $(this).attr('data-src');
					p.pathName = p.pathFolder + src.split('.')[0];
					clearTimeout(p.upWindowTime);
					if(p.htmlData[p.pathName]){
						$('html body').html($(p.htmlData[p.pathName]));
						setScript(true);
					}else{
						$("#mainIframe", p.document).attr('src', p.pathFolder + '/' + src + '?lid=' + $("#menuText", p.document).attr("data-id"));
					}
				// }
			});
		},
		// 六合彩总项一、二、特码AB-A中 特码B取不到大ID
		findBigId:function (obj, NumKey) {
			var bigId = '';
			// 为了区分特码半波单双、大小还要单独做设置
			if(NumKey >= 92050 && NumKey <= 92098){
				//特码B
				bigId = '91002';
			}else if(NumKey == 92102 || NumKey == 92101){
				//特单特双
				bigId = '91004';
			}else if(NumKey == 92099 || NumKey == 92100){
				//特大特小
				bigId = '91003';
			}else if(NumKey == 92103 || NumKey == 92104){
				//合单合双
				bigId = '91005';
			}else if(NumKey == 92573 || NumKey == 92574){
				//尾大尾小
				bigId = '91038';
			}else if(NumKey == 92565){
				//六肖
				bigId = '91030';
			}else if(NumKey == 92117 || NumKey == 92118 || NumKey == 92119){
				//红蓝绿波
				bigId = '91007';
			}else if(NumKey == 92242 || NumKey == 92248){
				bigId = '91012_1';
			}else if(NumKey == 92243 || NumKey == 92249){
				bigId = '91012_2';
			}else if(NumKey == 92244 || NumKey == 92250){
				bigId = '91012_3';
			}else if(NumKey == 92245 || NumKey == 92251){
				bigId = '91012_4';
			}else if(NumKey == 92246 || NumKey == 92252){
				bigId = '91012_5';
			}else if(NumKey == 92247 || NumKey == 92253){
				bigId = '91012_6';
			}else if(NumKey == 92230 || NumKey == 92236){
				bigId = '91011_1';
			}else if(NumKey == 92231 || NumKey == 92237){
				bigId = '91011_2';
			}else if(NumKey == 92232 || NumKey == 92238){
				bigId = '91011_3';
			}else if(NumKey == 92233 || NumKey == 92239){
				bigId = '91011_4';
			}else if(NumKey == 92234 || NumKey == 92240){
				bigId = '91011_5';
			}else if(NumKey == 92235 || NumKey == 92241){
				bigId = '91011_6';
			}else if(NumKey == 92254 || NumKey == 92260 || NumKey == 92266){
				bigId = '91013_1';
			}else if(NumKey == 92255 || NumKey == 92261 || NumKey == 92267){
				bigId = '91013_2';
			}else if(NumKey == 92256 || NumKey == 92262 || NumKey == 92268){
				bigId = '91013_3';
			}else if(NumKey == 92257 || NumKey == 92263 || NumKey == 92269){
				bigId = '91013_4';
			}else if(NumKey == 92258 || NumKey == 92264 || NumKey == 92270){
				bigId = '91013_5';
			}else if(NumKey == 92259 || NumKey == 92265 || NumKey == 92271){
				bigId = '91013_6';
			}else if(NumKey == 92272 || NumKey == 92278){
				bigId = '91014_1';
			}else if(NumKey == 92273 || NumKey == 92279){
				bigId = '91014_2';
			}else if(NumKey == 92274 || NumKey == 92280){
				bigId = '91014_3';
			}else if(NumKey == 92275 || NumKey == 92281){
				bigId = '91014_4';
			}else if(NumKey == 92276 || NumKey == 92282){
				bigId = '91014_5';
			}else if(NumKey == 92277 || NumKey == 92283){
				bigId = '91014_6';
			}else if(NumKey >= 92120 && NumKey <= 92131){
				if(NumKey == 92120 || NumKey == 92121 || NumKey == 92124 || NumKey == 92125 || NumKey == 92128 || NumKey == 92129){
					Betimes.list['tmbbdx'] +=  Betimes.myData[NumKey]['AmountValue'];
					// 单双
					bigId = '91008';
				}else{
					Betimes.list['tmbbds'] += Betimes.myData[NumKey]['AmountValue'];
					// 大小
					bigId = '91057';
				}
			}else if(NumKey >= 92105 && NumKey <= 92116){
				//生肖
				bigId = '91006';
			}else if(NumKey >= 92561 && NumKey <= 92564){
				//特碼攤子
				bigId = '91039';
			}else{
				bigId = obj.attr('data-did');
			}
			return bigId;
		},
		// 六肖數據緩存
		sixZodiaData: {},
		// 六合彩总项一、总项二、总项AB、总项三、六肖連六肖數據注额和盈亏加载
		zxAmountAndSzsz: function (d) {
			var _thisBetimes = Betimes;
			var _this = this;
			// 特码二
			if(p.playpage == 'tmzx2' || p.playpage == 'tmzx1' || p.playpage == 'tmab' || p.playpage == 'tmzx2_2' || p.playpage == 'tmzx3' || p.playpage == 'lxl'){
				// 计算六肖
				if( d.hasOwnProperty('szsz_lx_amount') ){
					var lxAmount = d.szsz_lx_amount;
					var sumLx = d.szsz_lx_amount_count;
					for(var key in lxAmount){
						// 缓存数据
						_thisBetimes.sixZodia[key] = {
							"AmountNumber": Number(_thisBetimes.sixZodia[key]["AmountNumber"]) + Number(lxAmount[key].split(',')[0]),
							"AmountValue": Number(_thisBetimes.sixZodia[key]["AmountValue"]) + Number(lxAmount[key].split(',')[1]),
							"WinValue": Number(_thisBetimes.sixZodia[key]["WinValue"]) + Number(lxAmount[key].split(',')[2]),
							"RepValue": Number(_thisBetimes.sixZodia[key]["RepValue"]) + Number(lxAmount[key].split(',')[4])
						};
						_this.sixZodiaData[key] = {
							'AmountNumber': 0,
							"AmountValue": 0,
							"winNum": 0,
							"RepValue": 0
						};
					}
					// 计算六肖盈亏
					for(var key in lxAmount){
						_thisBetimes.sixZodia[key]["WinValue"] = Number(sumLx.split(',')[0]) - Number(sumLx.split(',')[1]) - _thisBetimes.sixZodia[key]['AmountValue'] - _thisBetimes.sixZodia[key]['WinValue'];
						// _thisBetimes.sixZodia[key]["winNum"] = Number(sumLx.split(',')[0]) - Number(sumLx.split(',')[1]) - _thisBetimes.sixZodia[key]['WinValue'];
						// 將計算好的六肖數據緩存
						_this.sixZodiaData[key]["AmountNumber"] = _thisBetimes.sixZodia[key]['AmountNumber'];
						_this.sixZodiaData[key]["AmountValue"] = _thisBetimes.sixZodia[key]['AmountValue'];
						_this.sixZodiaData[key]["winNum"] = _thisBetimes.sixZodia[key]["WinValue"];
						_this.sixZodiaData[key]["RepValue"] = _thisBetimes.sixZodia[key]["RepValue"];
					}
				}else{
					var n1 = '';
					for(var i=1; i<=12; i++){
						n1 = i<10?'0'+i:''+i;
						_this.sixZodiaData[n1] = {
							'AmountNumber': _thisBetimes.sixZodia[n1]['AmountNumber'],
							"AmountValue":  _thisBetimes.sixZodia[n1]['AmountValue'],
							"winNum":  _thisBetimes.sixZodia[n1]["WinValue"],
							"RepValue":  _thisBetimes.sixZodia[n1]["RepValue"]
						};
					}
				}

				// 渲染六肖連六肖數據
				if(p.playpage == 'lxl'){
					var objCbm = $("#cbM"), objCbn = $("#cbN");
					var zodiacWinnerId = Betimes.myWinnerID.split('|')[0].split(',')[6];
					$("#szsz_" + zodiacWinnerId).addClass('winner');
					var endAmountVal='', aTitle='';
					var _thisSixZodiaData = null;
					var oSzsz = null;
					var oAmount = null;
					var n2 = '';
					for(var i=1; i<=12; i++){
						n2 = i<10?'0'+i:''+i;
						oSzsz = $("#szsz_" + n2);
						oAmount = $("#amount_" + n2);
						_thisSixZodiaData = _this.sixZodiaData[n2];
						endAmountVal='';
						aTitle='';
						if(objCbm.val() == '1' && objCbn.val() == '0'){
							endAmountVal = parseInt(_thisSixZodiaData["AmountValue"]) + '/' + _thisSixZodiaData["RepValue"];
							aTitle = '注額/補貨';
						}else if(objCbm.val() == '0' && objCbn.val() == '1'){
							endAmountVal = _thisSixZodiaData["AmountNumber"] + '/' + parseInt(_thisSixZodiaData["AmountValue"]);
							aTitle = '注數/注額';
						}else if(objCbm.val() == '1' && objCbn.val() == '1'){
							endAmountVal = _thisSixZodiaData["AmountNumber"] + '/' + parseInt(_thisSixZodiaData["AmountValue"]) + '/' +_thisSixZodiaData["RepValue"];
							aTitle = '注數/注額/補貨';
						}else{
							endAmountVal = parseInt( _thisSixZodiaData["AmountValue"] );
							aTitle = '注額';
						}
						// 绑定下注明细
						oAmount.addClass('amountEvent');
						oAmount.attr('data-alertOdds', '92565');
						if (_thisSixZodiaData["AmountValue"] == 0 && _thisSixZodiaData["AmountNumber"] == 0) {
							oAmount.html("-");
						}else{
							oAmount.attr('title', aTitle).html( endAmountVal );
						}
						if(_thisSixZodiaData["winNum"] < 0){
							oSzsz.addClass('red');
						}else{
							oSzsz.removeClass('red');
						}
						if (_thisSixZodiaData["winNum"] == 0) {
							oSzsz.html( "-" );
						}else{
							oSzsz.html( parseInt(_thisSixZodiaData["winNum"]) );
						}
					}
				}
				var minSum = 0;
				var maxSum = 0;
				var thisSid = '';
				var thisSzszObj = null;
				var thisSzsz = 0;
				var thisParent = null;
				var lxId = '';
				for(var i=1; i<=49; i++){
					thisSid = "920" + ( i < 10 ? '0'+ i : i);
					thisSzszObj = $("#szsz_"+ thisSid);
					thisSzsz = _thisBetimes.numData[thisSid];
					thisParent = thisSzszObj.parents('.oddsParent');
					lxId = getSixZodia(i)<10 ? '0' + getSixZodia(i) : getSixZodia(i);
					// 总项一
					if(p.playpage == 'tmzx1'){
						$("#tma_" + thisSid).html( _thisBetimes.myData[thisSid]['AmountValue'] ? parseInt(_thisBetimes.myData[thisSid]['AmountValue']) : "-").attr("data-amount", thisSid);
						$("#tmb_" + thisSid).html( _thisBetimes.myData[Number(thisSid) + 49]['AmountValue'] ? parseInt(_thisBetimes.myData[Number(thisSid) + 49]['AmountValue']) : "-").attr("data-amount", Number(thisSid) + 49);
						$("#sx_" + thisSid).html( _thisBetimes.myData[getZodiacSid(i)]['AmountValue'] ? parseInt(_thisBetimes.myData[getZodiacSid(i)]['AmountValue']) : "-" ).attr("data-amount", getZodiacSid(i));
						$("#sb_" + thisSid).html( _thisBetimes.myData[getWaveSid(i)]['AmountValue'] ? parseInt(_thisBetimes.myData[getWaveSid(i)]['AmountValue']) : "-").attr("data-amount", getWaveSid(i));
						if( i != 49 ){
							// $("#ds_" + thisSid).html( _thisBetimes.myData[getWaveSingleDouble(i)]['AmountValue'] ? parseInt(_thisBetimes.myData[getWaveSingleDouble(i)]['AmountValue']) : "-" );
							$("#dx_" + thisSid).html( _thisBetimes.myData[getBigSmall(i)]['AmountValue'] ? parseInt(_thisBetimes.myData[getBigSmall(i)]['AmountValue']) : "-" ).attr("data-amount", getBigSmall(i));
							$("#hds_" + thisSid).html( _thisBetimes.myData[getSumSingleDouble(i)]['AmountValue'] ? parseInt(_thisBetimes.myData[getSumSingleDouble(i)]['AmountValue']) : "-").attr("data-amount", getSumSingleDouble(i));
							$("#ds_" + thisSid).html( _thisBetimes.myData[getSingleDouble(i)]['AmountValue'] ? parseInt(_thisBetimes.myData[getSingleDouble(i)]['AmountValue']) : "-").attr("data-amount", getSingleDouble(i));
							$("#wdx_" + thisSid).html( _thisBetimes.myData[getMantBigSmall(i)]['AmountValue'] ? parseInt(_thisBetimes.myData[getMantBigSmall(i)]['AmountValue']) : "-").attr("data-amount", getMantBigSmall(i));
							$("#bb_" + thisSid).html( _thisBetimes.myData[getWaveSingleDouble(i)]['AmountValue'] + _thisBetimes.myData[getWaveBigSmall(i)]['AmountValue'] ? parseInt(_thisBetimes.myData[getWaveSingleDouble(i)]['AmountValue'] + _thisBetimes.myData[getWaveBigSmall(i)]['AmountValue']) : "-").attr("data-amount", getWaveSingleDouble(i)+','+getWaveBigSmall(i));
							$("#lx_" + thisSid).html( (_thisBetimes.sixZodia[lxId]["AmountValue"] ) ? parseInt(_thisBetimes.sixZodia[lxId]["AmountValue"]) : "-").attr("data-amount", lxId);
							$("#tmtz_" + thisSid).html( _thisBetimes.myData[getTanZi(i)]['AmountValue'] ? parseInt(_thisBetimes.myData[getTanZi(i)]['AmountValue']): "-").attr("data-amount", getTanZi(i));
						}else{
							$("#ds_" + thisSid).html( '<span class="green">和局</span>' );
							$("#dx_" + thisSid).html( '<span class="green">和局</span>' );
							$("#hds_" + thisSid).html( '<span class="green">和局</span>' );
							// $("#ds_" + thisSid).html( '<span class="green">和局</span>' );
							$("#wdx_" + thisSid).html( '<span class="green">和局</span>' );
							$("#bb_" + thisSid).html( '<span class="green">和局</span>' );
							$("#lx_" + thisSid).html( '<span class="green">和局</span>' );
							$("#tmtz_" + thisSid).html( '<span class="green">和局</span>' );
						}
					}
					// 特码AB
					if(p.playpage != 'tmab'){
						// 生肖
						thisSzsz += _thisBetimes.numData[getZodiacSid(i)];
						// 色波
						thisSzsz += _thisBetimes.numData[getWaveSid(i)];
					}
					// 特码B
					thisSzsz += _thisBetimes.numData[Number(thisSid) + 49];
	
					if(i != 49){
						if(p.playpage != 'tmab'){
							// 单双
							thisSzsz += _thisBetimes.numData[getSingleDouble(i)];
							// 大小
							thisSzsz += _thisBetimes.numData[getBigSmall(i)];
							// 合单合双
							thisSzsz += _thisBetimes.numData[getSumSingleDouble(i)];
							// 尾大尾小
							thisSzsz += _thisBetimes.numData[getMantBigSmall(i)];
							// if(p.playpage != 'tmab'){
							// 六肖
							thisSzsz += _thisBetimes.sixZodia[lxId]["WinValue"];
							// 色波单双
							thisSzsz += _thisBetimes.numData[getWaveSingleDouble(i)];
							// 色波大小
							thisSzsz += _thisBetimes.numData[getWaveBigSmall(i)];
							// 特码摊子
							thisSzsz += _thisBetimes.numData[getTanZi(i)];
						}
					}

					thisParent.attr('data-win', parseInt(thisSzsz));
					if(thisSzsz<0){
						thisSzszObj.addClass('red').html( parseInt(thisSzsz) );
					}else if (thisSzsz == 0) {
						thisSzszObj.removeClass('red').html( "-" );
					}else{
						thisSzszObj.removeClass('red').html( parseInt(thisSzsz) );
					}
					if(thisSzsz < d.abovevalid[thisSzszObj.attr('data-did')]){
						thisSzszObj.parent().addClass('warning');
					}
					if(minSum > thisSzsz){
						minSum = thisSzsz;
					}
					if(maxSum < thisSzsz){
						maxSum = thisSzsz;
					}
				}

				// 总项三/分公司总项二
				if(p.playpage == 'tmzx2' || p.playpage == 'tmzx3' || p.playpage == 'tmzx2_2'){
					var oTmxg = $(".tmxg");
					var oTmxgLen = oTmxg.length;
					var oThis = null;
					for(var i = 0; i<oTmxgLen; i++){
						oThis = oTmxg.eq(i);
						oThis.html( parseInt( _thisBetimes.list["sumwin_" + oThis.attr('data-bid')] ));
					}
					// $(".tmxgbb").each(function () {
					// 	oThis = $(this);
					// 	oThis.html( parseInt(_thisBetimes.list["tmbb" + oThis.attr('data-bid')]) );
					// });

					if( d.hasOwnProperty('gxmx_szsz') ){
						var gxmxData = d.gxmx_szsz;
						var zmtAmount = 0;
						var gxmxId = '';
						$(".gxmx").each(function () {
							oThis = $(this);
							gxmxId = oThis.attr('data-bid');
							if(gxmxId == '91010' || gxmxId == '91025' || gxmxId == '91026' || gxmxId == '91027' || gxmxId == '91028' || gxmxId == '91029'){
								if(gxmxData.hasOwnProperty(gxmxId)){
									zmtAmount = zmtAmount + parseInt(gxmxData[gxmxId]);
								}
							}
							if( gxmxData.hasOwnProperty(gxmxId) ){
								oThis.html( parseInt(gxmxData[gxmxId]) );
							}else{
								oThis.html('0');
							}
						});
					}
				}
				// 最大正值/最大负值
				$("#maxNeg").html( parseInt(minSum) );
				$("#maxPos").html( parseInt(maxSum) );
			}
		},
		// 六合彩设置总项一、总项二、总项AB、总项三注额加载
		sixSetAmount: function (key, that) {
			if(p.playpage == 'tmzx2' || p.playpage == 'tmzx1' || p.playpage == 'tmab' || p.playpage == 'tmzx2_2' || p.playpage == 'tmzx3'){
				var bThat = null;
				if(Number(key) >= 92001 && Number(key) <= 92049){
					bThat = Betimes.myData[ Number(key) + 49 ];
					Betimes.nowAmountValue = Number(that['AmountValue']) + Number(bThat['AmountValue']);
					Betimes.nowRepValue = Number(that['RepValue']) + Number(bThat['RepValue']);
					Betimes.nowAmountNumber = Number(that['AmountNumber']) + Number(bThat['AmountNumber']);
				}else{
					Betimes.nowAmountValue = Number(that['AmountValue']);
					Betimes.nowRepValue = Number(that['RepValue']);
					Betimes.nowAmountNumber = Number(that['AmountNumber']);
				}
			}else{
				Betimes.nowAmountValue = Number(that['AmountValue']);
				Betimes.nowRepValue = Number(that['RepValue']);
				Betimes.nowAmountNumber = Number(that['AmountNumber']);
			}
		},
		// 六合彩连码赔率数据缓存
		sixOddsData: {},
		// 六合彩连码不中
		sixSetLm: function (d) {
			var _this = this;
			var obj = $("input[name=radiobutton]:checked");
			var radioKey = obj.attr('data-radio');
			var radioName = obj.attr('data-name');
			var _zj = $("#zj");
			var _nozj = $("#nozj");
			var lmSelect = $("#lmSelect");
			var lmbhTitle = $(".lmbhTitle");
			lmbhTitle.html(radioName);
			lmSelect.find("option[value=" + radioKey + "]").prop("selected","selected");
			lmSelect.unbind('change').bind('change', function () {
				$("input[data-radio="+ $(this).val() +"]").prop("checked","checked");
				Betimes.oddsAjax($("#upSecond").val()-0);
				// 弹窗对象容器
				Betimes.aOddsThat= [];
				$("#oddsTrim").hide();
				// 走飞对象容器
				Betimes.flyAwayThat= [];
				$("#flyAway").hide();
			});
			if(p.isopt  == '1'){
				_zj.show();
				_nozj.hide();
				// 处理连码微调
				var wtnumbervalue = d.wtnumbervalue;
				var html = '';
				var wtBtnHtml = '';
				var num = key<10 ? "0"+key : key;
				var wtStr = '';
				for(var key in wtnumbervalue){
					html = '';
					wtBtnHtml = '';
					num = key<10 ? "0"+key : key;
					wtStr = wtnumbervalue[key];
					if(p.usertype == '1'){
						wtBtnHtml = '<em class="wt_addBtns" data-num="'+ num +'"></em>'+
							'<i class="wt_minBtns" data-num="'+ num +'"></i>';
					}

					if(radioKey == '92286' || radioKey == '92288' || radioKey == '92639' || radioKey == '92641'){
						// 缓存双赔率第一个赔率数据
						_this.sixOddsData[num] = {
							'oddsindex': 0,
							'number': key,
							'oddsid': lmSelect.val(),
							'OddsMaxValue': wtStr.split('|')[0].split(',')[1],
							'OddsMinValue': wtStr.split('|')[0].split(',')[2]
						};
						// 缓存双赔率第二个赔率数据
						_this.sixOddsData[num+'_1'] = {
							'oddsindex': 1,
							'number': key,
							'oddsid': lmSelect.val(),
							'OddsMaxValue': wtStr.split('|')[1].split(',')[1],
							'OddsMinValue': wtStr.split('|')[1].split(',')[2]
						};
						html = '<div class="pDiv wt oddsParent" data-id>'+
							wtBtnHtml+
							'<a class="blue1 wt_oddsTrim" data-tOdds="'+ lmSelect.val()+'_0' +'" data-odds="'+ num +'" id="odds_'+ num +'_0" data-name="'+ num +'" href="javascript:;">'+ wtStr.split('|')[0].split(',')[0] +'</a>'+
							'<a class="blue1 wt_oddsTrim" data-tOdds="'+ lmSelect.val()+'_1' +'" data-odds="'+ num +'_1" id="odds_'+ num +'_1" data-name="'+ num +'" href="javascript:;">'+ wtStr.split('|')[1].split(',')[0] +'</a>'+
						'</div>';
					}else{
						// 缓存连码单赔率数据
						_this.sixOddsData[num] = {
							'number': key,
							'oddsid': lmSelect.val(),
							'OddsMaxValue': wtStr.split(',')[1],
							'OddsMinValue': wtStr.split(',')[2]
						};
						// 针对六肖连处理
						var dataName = '';
						var dataTodds = '';
						if(p.playpage == 'lxl'){
							dataName = returnAnimal(Number(num));
							dataTodds = '92565';
						}else{
							dataName = num;
							dataTodds = lmSelect.val();
						}
						html = '<div class="pDiv oddsParent">'+
							wtBtnHtml+
							'<a class="blue1 wt_oddsTrim" data-tOdds="'+ dataTodds +'" data-odds="'+ num +'" id="odds_'+ num +'" data-name="'+ dataName +'" href="javascript:;">'+ wtStr.split(',')[0] +'</a>'+
						'</div>';
					}
					$("#l_w_"+key).html( html );
				}

				if(p.usertype == '1'){
					// 绑定六合彩赔率微调
					_this.sixSetOdds();
				}
				// 总监下加载连码下投注列表，为补货
				if(radioKey && Betimes.isLoadLmSzsz){
					_this.setSixLmSzsz(_zj, radioKey, radioName);
				}
			}else{
				_zj.hide();
				_nozj.show();
				// 非总监下加载连码下投注列表，为补货
				if(radioKey && Betimes.isLoadLmSzsz){
					_this.setSixLmSzsz(_nozj, radioKey, radioName);
				}
			}
			// 重新设置iframe高度
			p.setIframeHeight();
		},
		// DOM六肖连生肖号码和名称
		setSixLxlPage: function () {
			var znData = returnZodiacNumber(48, 'str');
			for(var key in znData){
				var numHtml = $.map(znData[key], function (item) { return '<span class="No_'+ item+'"></span>';}).join('');
				$("#znData_" + key).html( $(numHtml) )
			}
			var html = '不連' + returnAnimal(p.zodiacData[1]);
			$(".exlTitle").html( html ).siblings('a.oddsTrim').attr('data-name', html);
			$(".sx_"+p.zodiacData[1]).addClass('red');
			if(p.usertype == '1' && p.isopt == '1'){
				$(".hide").removeClass('hide');
			}else if(p.usertype == '2' && p.isopt == '1'){
				$(".hide").siblings('a').removeClass('oddsTrim')
				$(".sx_"+p.zodiacData[1]).siblings('.hide').removeClass('hide');
				$(".sx_"+p.zodiacData[1]).siblings('a').addClass('oddsTrim');
				$(".exlTitle").siblings('.hide').removeClass('hide');
				$(".exlTitle").siblings('a').addClass('oddsTrim');
				$("#odds_92569_10, #odds_92569_0, #odds_92570_10, #odds_92570_0, #odds_92571_10, #odds_92571_0, #odds_92637_10, #odds_92637_0").siblings('.hide').removeClass('hide');
				$("#odds_92569_10, #odds_92569_0, #odds_92570_10, #odds_92570_0, #odds_92571_10, #odds_92571_0, #odds_92637_10, #odds_92637_0").addClass('oddsTrim');
			}
		},
		// 处理六肖连
		sixSetLxl: function (d) {
			var _this = this;
			var obj = $("input[name=radiobutton]:checked");
			var radioKey = obj.attr('data-radio');
			var radioName = obj.attr('data-name');
			var lmbhTitle = $(".lmbhTitle");
			lmbhTitle.html( radioName );
			if(radioKey && Betimes.isLoadLmSzsz){
				_this.setSixLmSzsz($("#bh"), radioKey, radioName);
			}
		},
		// 六合彩赔率微调
		sixSetOdds: function () {
			var _this = this;
			var lmSelect = $("#lmSelect");

			var bidId = '';
			if(p.playpage == 'lxl'){
				bidId = '92565';
			}else{
				bidId = lmSelect.val();
			}
			// 绑定微调赔率调整事件
			$(".wt_oddsTrim").unbind('click').bind('click', function () {
				var that = $(this);
				Betimes.aOddsThat = [that.attr('id')];
				Betimes.flyAwayThat = [];
				isUpdataFlyAway = true;
				Betimes.oddsTrimModuleInit(that);
			});
			// 加号设置赔率
			$('.wt_addBtns').unbind('click').bind('click', function () {
				var that = $(this).siblings('a');
				var isSubmit = false;
				var difference = $('#tool_input').val()-0 || 0.01;
				that.each(function () {
					var _that = $(this);
					var key = that.attr('data-odds');
					if(ForDight4(Number(that.html()) + difference) > Number(_this.sixOddsData[key]['OddsMaxValue'])){
						tip = tips.msgTips({
							msg: '當前修改賠率不能大於系統設定最大賠率:'+ _this.sixOddsData[key]['OddsMaxValue'] +'！',
							type : "error"
						});
						isSubmit = false;
					}else{
						isSubmit = true;
					}
					return isSubmit;
				});
				if(isSubmit){
					$(this).addAndSubLoading();
					// 关闭连码补货请求
					Betimes.isLoadLmSzsz = false;
					Betimes.baseOddsAjax('1', bidId, difference, '', '', $(this).attr('data-num'));
				}
			});
			// 减号设置赔率
			$('.wt_minBtns').unbind('click').bind('click', function () {
				var that = $(this).siblings('a');
				var isSubmit = false;
				var difference = $('#tool_input').val()-0 || 0.01;
				that.each(function () {
					var _that = $(this);
					var key = that.attr('data-odds');
					if(accSub(Number(that.html()), difference) < Number(_this.sixOddsData[key]['OddsMinValue'])){
						tip = tips.msgTips({
							msg: '當前修改賠率不能小於'+ _this.sixOddsData[key]['OddsMinValue'] +'！',
							type : "error"
						});
						isSubmit = false;
					}else{
						isSubmit = true;
					}
					return isSubmit;
				});
				if(isSubmit){
					$(this).addAndSubLoading();
					// 关闭连码补货请求
					Betimes.isLoadLmSzsz = false;
					Betimes.baseOddsAjax('2', bidId, difference, '', '', $(this).attr('data-num'));
				}
			});
		},
		// 连码补货数据缓存
		sixSzszData: {},
		// DOM六合彩连码补货数据
		setSixLmSzsz: function (obj, radioKey, radioName) {
			var _this = this;
			var tbody = obj.find('.lmbh_list');
			var html = '';
			var num = 0;
			Betimes.getLmBhList(function (data) {
				var _data = null;
				if(p.playpage == 'lm' || p.playpage == 'lm_b' || p.playpage == 'bz'){
					_data = data.lm_szsz_amount;
					var pcTitle = $(".pcTitle");
					var pc2 = $(".pc2");
					for(var key in _data){
						num++;
						if (Betimes.myWinnerID) {
							winnerFun(Betimes.myWinnerID, key);
						}
						_this.sixSzszData[key] = {
							'oddsid': radioKey,
							'amount': _data[key].split(',')[0],
							'number': key
						};
						var highBg = '';
						if(Betimes.flyAwayThat[1] == key){
							highBg = ' highBg';
						}
						var pcHtml = '';
						if(radioKey == '92286' || radioKey == '92288' || radioKey == '92639' || radioKey == '92641'){
							pcTitle.attr('colspan', 5);
							pc2.show();
							pcHtml = '</td><td class="red '+ lmClassName1 +'">-'+parseInt(_data[key].split(',')[1])+
										'</td><td class="red '+ lmClassName2 +'">-'+parseInt(_data[key].split(',')[2]);
						}else{
							pcTitle.attr('colspan', 4);
							pc2.hide();
							pcHtml = '</td><td class="red '+ lmClassName1 +'">-'+parseInt(_data[key].split(',')[1]);
						}
						var keyHtml = '';
						//连码百大排行点击分组
						if(p.playpage == 'lm' || p.playpage == 'lm_b'){
							keyHtml = '<a data-radioKey="'+ radioKey +'" class="sixLmAlert" href="/ReportSearch/LM_RankShow_six.aspx?items='+ key +'&ispay=0&ow=0&islike=0&page=1">'+ key + '</a>';
						}else{
							keyHtml = key;
						}
						html += '<tr class="bhlbClass'+ highBg +'"><td>'+num+'</td><td>'+keyHtml+'</td><td>'+parseInt(_data[key].split(',')[0])+
						pcHtml +
						'</td><td><a href="javascript:;" id="lmbhlist_'+ num +'" class="blue lmbhBtn" data-amount="'+ _data[key].split(',')[0] +'" data-key="'+ key +'" data-szsz="'+ radioKey +'" data-name="'+ radioName +'">補貨</a></td></tr>';
					}
				}else if(p.playpage == 'lxl'){
					_data = data.lxl_szsz_amount;
					var name = '';
					var highBg = '';
					for(var key in _data){
						name = key;
						if (Betimes.myWinnerID) {
							winnerFun(Betimes.myWinnerID, key);
						}
						num++;
						_this.sixSzszData[key] = {
							'oddsid': radioKey,
							'amount': _data[key].split(',')[0],
							'number': key
						};
						highBg = '';
						if(Betimes.flyAwayThat[1] == key){
							highBg = 'highBg';
						}
						if(radioKey == 92565 || radioKey == 92566 || radioKey == 92567 || radioKey == 92568 || radioKey == 92636){
							var arr = key.split(','), newStr = [];
							for(var i=0; i<arr.length; i++){
								newStr.push(returnAnimal(Number(arr[i])));
							}
							name = newStr.join(',');
						}else{
							var arrWs = key.split(','), newWsStr = [];
							for(var i=0; i<arrWs.length; i++){
								var wsName = Number(arrWs[i]);
								newWsStr.push((wsName==10?0:wsName)+'尾');
							}
							name = newWsStr.join(',');
						}
						html += '<tr class="bhlbClass'+ highBg +'"><td>'+num+'</td><td>'+name+'</td><td>'+ parseInt(_data[key].split(',')[0])+
						'</td><td class="red '+ lmClassName1 +'">-'+parseInt(_data[key].split(',')[1])+
						'</td><td><a href="javascript:;" id="lmbhlist_'+ num +'" class="blue lmbhBtn" data-amount="'+ _data[key].split(',')[0] +'" data-key="'+ key +'" data-szsz="'+ radioKey +'" data-name="'+ radioName +'">補貨</a></td></tr>';
					}
				}
				tbody.html(html);
				// 绑定连码补货事件
				$(".lmbhBtn").click(function () {
					Betimes.flyAwayThat = [];
					isUpdataFlyAway = true;
					Betimes.flyAwayTargetId = '';
					Betimes.lmSzszHandle($(this));
				});
				// 绑定快速走飞
				if(tbody.html() !='' && p.issllowsale == '1'){
					$(".fastbh").unbind('click').bind('click', function () {
						Betimes.bindLmFastFlyAway();
					});
				}
				// 重新设置高度
				p.setIframeHeight();
			});
		}
	};

	/*
	 * [赔率微调模块 需jQuery]
	 */
	var oddsTrimModule = (function ($) {
		var _tpl = '<div id="oddsTrim" class="oddsTrimBox">'+
					'<div id="oddsTrimHeader">'+
						'<h4>04</h4>'+
					'</div>'+
					'<div id="oddsTrimMain">'+
						'<div id="oddsTrimLoading">'+
							'<img src="/images/loader.gif">'+
							'<p>提交中，請稍後</p>'+
						'</div>'+
						'<div class="oddsTrimMainLine">'+
							'<label class="oddsTrimLabel" for="">新賠率:</label>'+
							'<input class="oddsTrimText plNumber" type="text" value="" />'+
						'</div>'+
						'<div class="oddsTrimMainLine">'+
							'<a class="oddsTrimNo" href="javascript:;">取消</a>'+
							'<a class="oddsTrimYes" href="javascript:;">確定</a>'+
						'</div>'+
					'</div>'+
					'<div id="oddsTrimFooter"></div>'+
					'<div id="oddsTrimIcon"></div>'+
				'</div>';

		var _init = function (options) {
			var defaults = {
				target: null,
				okCallBack: function () {}
			};
			var opts = $.extend(defaults, options);
			var obj = $(_tpl), _top, _left, isFocus = false;

			if (Betimes.oddsTargetId != opts.target.attr('id')) {
				Betimes.oddsTargetId = opts.target.attr('id');
				isFocus = true;
				if(!$("#oddsTrim").length){
					$('body').append(obj);
				}
			}
			var obj = $("#oddsTrim");
			if(!opts.target.length){
				obj.hide();
				return;
			}
			setLeftAndTop();
			function setLeftAndTop() {
				var th = opts.target.innerHeight();
				var ot = opts.target.offset().top;
				var oh = obj.height();
				var inputText = obj.find(".oddsTrimText");
				obj.removeClass('oTload');
				if($(window).height() - ot - th <= oh){
					_top = ot + th/2 - oh -7;
					obj.addClass('oddsTrimDown');
				}else{
					_top = ot + th + 7;
					obj.removeClass('oddsTrimDown');
				}
				obj.find('h4').html(opts.target.attr('data-name'));
				obj.find('.oddsTrimNo').click(function () {
					obj.hide();
					Betimes.aOddsThat = [];
					Betimes.oddsTargetId = '';
				});
				obj.find('.oddsTrimText').unbind('keyup').bind('keyup', function (ev) {
					 var code = ev.keyCode || ev.which;
					if (code == 13) {
						obj.find('.oddsTrimYes').click();
					}
				});
				obj.find('.oddsTrimYes').unbind('click').bind('click', function () {
					if(typeof opts.okCallBack == "function"){
						opts.okCallBack(inputText.val());
					}else{
						console.log('Plaese add errorCallBack Function for getBaseDataAjaxHandle!');
					}
				});
				_left = opts.target.offset().left + opts.target.innerWidth()/2 - 75;
				$("#flyAway").hide();
				// obj.show().stop(true,false).animate({
				// 	top: _top,
				// 	left: _left
				// }, 300);
				obj.show().css({
					top: _top,
					left: _left
				}, 300);
				if (isFocus) {
					obj.find(".oddsTrimText").val(opts.target.html());
					obj.find('input').eq(0).focus().select();
				}
			}
		};

		return {
			_init: _init
		};
	})(jQuery);

	/*
		Fly away 走飞
	 */
	var isUpdataFlyAway = true;
	var flyAwayModule = (function ($) {
		var _flyInit = function (options) {
			var defaults = {
				target: null,
				okCallBack: function () {}
			};
			var opts = $.extend(defaults, options);
			var aAllowSaleUserNameHtml = '';
			var nowAllowType = 0;
			var nowsltPlaytype = 0;
			var allowSaleData = p.aAllowSaleUserName.saleuser;
			// for(var i=0; i<p.aAllowSaleUserName.length; i++){
			// 	aAllowSaleUserNameHtml += '<option value="'+ p.aAllowSaleUserName[i] +'">' + p.aAllowSaleUserName[i] +'</option>';
			// }
			var idText = '';
			var idClass= '';
			for(var key in allowSaleData){
				if (allowSaleData[key] == '1') {
					idText = '【內補】';
					idClass = 'red';
				}else{
					idText = '【外補】';
					idClass = 'blue';
				}
				aAllowSaleUserNameHtml += '<option class="'+ idClass +'" value="'+ key +'">' + key + idText +'</option>';
			}

			var fragHtml = '';
			var key = opts.target.attr('data-szsz');
			var nowOdds = Betimes.oddsData[key]['pl'];
			var doubleHtml = '';
			var isDisabled = 'disabled=true';
			if(p.usertype == '1'){
				isDisabled = '';
			}
			var odds1index = 0;
			var odds2index = 0;
			if (key == '92566' || key == '92567' || key == '92568' || key == '92636') {
				odds1index = 0;
				odds2index = p.zodiacData[1];
			}else if(key == '92569' || key == '92570' || key == '92571' || key == '92637'){
				odds1index = 0;
				odds2index = 10;
			}else{
				odds1index = 0;
				odds2index = 1;
			}
			if(nowOdds.indexOf(',') > 0){
				doubleHtml =  '<div class="flyAwayMainLine">'+
					'<label class="flyAwayLabel" for="">賠率1:</label>'+
					'<input id="flyAwayTextOdds1" class="flyAwayText flyAwayOdds plNumber" data-currindex="'+ odds1index +'" type="text" '+ isDisabled +' value="'+ nowOdds.split(',')[odds1index] +'" />'+
				'</div>'+
				'<div class="flyAwayMainLine">'+
					'<label class="flyAwayLabel" for="">賠率2:</label>'+
					'<input id="flyAwayTextOdds2" class="flyAwayText flyAwayOdds plNumber" data-currindex="'+ odds2index +'" type="text" '+ isDisabled +' value="'+ nowOdds.split(',')[odds2index] +'" />'+
				'</div>';
			}else{
				doubleHtml = '<div class="flyAwayMainLine">'+
					'<label class="flyAwayLabel" for="">賠率:</label>'+
					'<input id="flyAwayTextOdds" class="flyAwayText flyAwayOdds plNumber" data-currindex="0" type="text" '+ isDisabled +' value="'+ nowOdds +'" />'+
				'</div>';
			}
			// 如果是总监 usertype == '1'
			if(p.usertype == '1'){
				fragHtml = '<div class="flyAwayMainLine">'+
					'<label class="flyAwayLabel" for="">會員:</label>'+
					'<select id="AllowSaleUserName" class="h18" name="select">'+
						 aAllowSaleUserNameHtml +
					'</select>'+
				'</div>'+ doubleHtml;
			}else{
				fragHtml = doubleHtml;
			}
			// 百家乐补货
			var bjlHtml = '';
			var bjlOption = '';

			if($("#sltPlaytype").val() == '0'){
				// bjlOption = '<option value="1">一般</option><option value="0" selected>免傭</option>';
				bjlOption = '<option value="0" selected>免傭</option>';
			}else{
				// bjlOption = '<option value="1" selected>一般</option><option value="0">免傭</option>';
				bjlOption = '<option value="1" selected>一般</option>';
			}
			if(p.pathFolder == 'L_PKBJL'){
				bjlHtml = '<div class="flyAwayMainLine">'+
					'<label class="flyAwayLabel" for="asBjlPlaytype">類型:</label>'+
					'<select id="asBjlPlaytype" class="h18" name="asBjlPlaytype">'+
						bjlOption+
					'</select>'+
				'</div>';
			}

			var _temp = '<div id="flyAway" class="flyAwayBox">'+
						'<div id="flyAwayHeader">'+
							'<h4>04</h4>'+
						'</div>'+
						'<div id="flyAwayMain">'+
							'<form>'+
							'<div id="flyAwayLoading">'+
								'<img src="/images/loaderf.gif">'+
								'<p>提交中，請稍後</p>'+
							'</div>'+
							'<div id="flyAwayUpdata">'+
							fragHtml +
							'</div>'+
							bjlHtml +
							'<div class="flyAwayMainLine">'+
								'<label class="flyAwayLabel" for="">金額:</label>'+
								'<input id="flyAwayValue" class="flyAwayText zfNumber" type="text" value="" />'+
							'</div>'+
							'<div class="flyAwayMainLine">'+
								'<a class="flyAwayNo" href="javascript:;">取消</a>'+
								'<a class="flyAwayYes" href="javascript:;">補出</a>'+
							'</div>'+
							'</form>'+
						'</div>'+
						'<div id="flyAwayFooter"></div>'+
						'<div id="flyAwayIcon"></div>'+
						'</div>';


			var obj = $(_temp), _top, _left, isFocus = false;
			if(Betimes.flyAwayTargetId != opts.target.attr('id')){
				Betimes.flyAwayTargetId = opts.target.attr('id');
				isFocus = true;
				if(!$("#flyAway").length){
					$('body').append(obj);
				}else{
					$("#flyAwayLoading").find('p').html('提交中，請稍後');
					$("#flyAwayUpdata").html(fragHtml);
				}
			}
			var obj = $("#flyAway");
			if(!opts.target.length){
				obj.hide();
				return;
			}
			var flyAwayData = null;

			function setFlyAwayOdds(nowAllowType, s_user) {
				var oddsObj = $("#flyAwayTextOdds");
				var oddsObj1 = $("#flyAwayTextOdds1");
				var oddsObj2 = $("#flyAwayTextOdds2");
				if (nowAllowType == '1') {
					obj.addClass('oTload');
					$("#flyAwayLoading").find('p').html('獲取賠率中，請稍後');
					var b = new getBaseDataAjax({
						url: 'Handler/Handler.ashx',
						_type: 'POST',
						dataType: 'json',
						postData: {
							'action': 'get_oddsinfoex',
							's_user': s_user,
							'oddsid': key,
							'number': opts.target.attr('data-key')
						},
						async: false,
						completeCallBack:function () {},
						successCallBack:function (data) {
							obj.removeClass('oTload');
							$("#flyAwayLoading").find('p').html('提交中，請稍後');
							nowOdds = data.data['pl'];
							if (nowOdds.indexOf(',') > 0) {
								oddsObj1.val(nowOdds.split(',')[0]);
								oddsObj2.val(nowOdds.split(',')[1]);
								if(p.usertype == '1'){
									oddsObj1.attr('disabled', true);
									oddsObj2.attr('disabled', true);
								}
							}else{
								oddsObj.val(nowOdds);
								if(p.usertype == '1'){
									oddsObj.attr('disabled', true);
								}
							}
						},
						errorCallBack:function () {}
					});
				}else{
					nowOdds = Betimes.oddsData[key]['pl'];
					if (nowOdds.indexOf(',') > 0) {
						if(p.usertype == '1'){
							oddsObj1.val(nowOdds.split(',')[odds1index]).attr('disabled', false);
							oddsObj2.val(nowOdds.split(',')[odds2index]).attr('disabled', false);
						}
					}else{
						if(p.usertype == '1'){
							oddsObj.val(nowOdds).attr('disabled', false);
						}
					}
				}
			}

			setLeftAndTop();
			function setLeftAndTop() {
				var th = opts.target.innerHeight();
				var ot = opts.target.offset().top;
				var oh = obj.height();
				var oddsObj = $(".flyAwayOdds");
				var AllowSaleUserName = $("#AllowSaleUserName");
				var asBjlPlaytype = $("#asBjlPlaytype");
				var sltPlaytype = $("#sltPlaytype");

				nowAllowType = allowSaleData[AllowSaleUserName.val()];
				AllowSaleUserName.unbind('change').bind('change', function () {
					nowAllowType = allowSaleData[$(this).val()];
					setFlyAwayOdds(nowAllowType, $(this).val());
				});
				if(isUpdataFlyAway){
					isUpdataFlyAway = false;
					asBjlPlaytype.find("option").removeAttr("selected");
					asBjlPlaytype.find("option[value="+ ((sltPlaytype.val() == '0') ? '0' : '1') +"]").prop("selected",true);
				}

				asBjlPlaytype.unbind('change').bind('change', function () {
					var asBjlPlaytypeVal = 0;
					var pBanker = 0;
					if(key == '82005'){ // 庄
						if(p.pkbjl_bankeramount == ''){
							asBjlPlaytypeVal = 0;
						}else{
							asBjlPlaytypeVal = Number(p.pkbjl_bankeramount);
						}
					}else if(key == '82006'){ // 闲
						if(p.pkbjl_playeramount == ''){
							asBjlPlaytypeVal = 0;
						}else{
							asBjlPlaytypeVal = Number(p.pkbjl_playeramount);
						}
					}
					if($(this).val() == '0' && sltPlaytype.val() != '0'){
						$("#flyAwayTextOdds").val(Number(nowOdds) + asBjlPlaytypeVal);
					}else if($(this).val() == '1' && sltPlaytype.val() == '0'){
						$("#flyAwayTextOdds").val(Number(nowOdds) - asBjlPlaytypeVal);
					}else{
						$("#flyAwayTextOdds").val(Number(nowOdds));
					}
				});

				if(Betimes.flyAwayTargetId != opts.target.attr('id')){
					Betimes.flyAwayTargetId = opts.target.attr('id');
					isFocus = true;
					setFlyAwayOdds(nowAllowType, AllowSaleUserName.val());
				}

				if(nowAllowType == '1'){
					setFlyAwayOdds(nowAllowType, AllowSaleUserName.val());
				}

				obj.removeClass('oTload');

				if($(window).height() - ot - th <= oh){
					_top = ot + th/2 - oh -7;
					obj.addClass('flyAwayDown');
				}else{
					_top = ot + th + 7;
					obj.removeClass('flyAwayDown');
				}
				if($(window).width() - opts.target.innerWidth() - opts.target.offset().left <= obj.width()){
					_left = opts.target.offset().left + opts.target.innerWidth()/2 - obj.width();
					obj.addClass('flyAwayRight');
				}else{
					_left = opts.target.offset().left + opts.target.innerWidth()/2 - 75;
					obj.removeClass('flyAwayRight');
				}

				obj.find('h4').html(opts.target.attr('data-name'));
				obj.find('.flyAwayNo').click(function () {
					if(p.isLm == '1'){
						var lmbhThat = Betimes.findLastObj(Betimes.flyAwayThat);
						if(lmbhThat){
							lmbhThat.parents('tr').eq(0).removeClass('highBg');
						}
					}
					obj.hide();
					Betimes.flyAwayThat = [];
					isUpdataFlyAway = true;
					Betimes.flyAwayTargetId = '';
				});
				obj.find('.flyAwayText').unbind('keyup').bind('keyup', function (ev) {
					 var code = ev.keyCode || ev.which;
					if (code == 13) {
						obj.find('.flyAwayYes').click();
					}
				});
				obj.find('.flyAwayYes').unbind('click').bind('click', function () {
					var currentoddsArr = [];
					// 获取当前赔率值
					oddsObj.each(function () {
						currentoddsArr.push($(this).val());
					});

					var maxVal = '';
					if(p.isLm == '1'){
						maxVal = opts.target.attr('data-amount');
					}else{
						if(p.playpage == 'tmab' || p.playpage == 'tmzx1' || p.playpage == 'tmzx2' || p.playpage == 'tmzx3'){
							if (Number(key)>=92001 && Number(key) <= 92049) {
								maxVal = Number(Betimes.myData[key]['AmountValue']) + Number(Betimes.myData[Number(key)+49]['AmountValue']);
							}else{
								maxVal = Betimes.myData[key].AmountValue;
							}
						}else{
							maxVal = Betimes.myData[key].AmountValue;
						}
					}

					flyAwayData = {
						action: 'set_allowsale',
						phaseid: $('#nn').attr('data-pId'),
						oddsid: key,
						currentodds: currentoddsArr.join(','),
						amount: $("#flyAwayValue").val()-0,
						number: opts.target.attr('data-key'),
						fast: 0,
						s_user: AllowSaleUserName.length > 0 ? AllowSaleUserName.val() : '',
						max: maxVal,
						playtype: asBjlPlaytype.val(),
						jeucode: p.jeucode
					};
					if(typeof opts.okCallBack == "function"){
						opts.okCallBack(flyAwayData);
					}else{
						console.log('Plaese add errorCallBack Function for getBaseDataAjaxHandle!');
					}
					Betimes.flyAwayThat = [];
					isUpdataFlyAway = false;
				});

				$("#oddsTrim").hide();

				// 动画不用
				// obj.show().stop(true,false).animate({
				// 	top: _top,
				// 	left: _left
				// }, 300);

				obj.show().css({
					top: _top,
					left: _left
				}, 300);
				if (isFocus) {
					obj.find('#flyAwayValue').focus().select();
				}
			}

		};

		return {
			_flyInit: _flyInit
		};
	})(jQuery);

	// 格式化时间 hh:mm:ss => s
	function timeFormat(time) {
		var hms = time.split(":"),
			s = hms[2]-0,
			m = hms[1]-0,
			h = hms[0]-0;
		return h * 3600 + m * 60 + s;
	}
	// 反格式化时间 s => hh:mm:ss
	function unTimeFormat(time) {
		var h = parseInt( time/60/60 ) + '', m = parseInt( time/60 )-60*h + '', s = time % 60 +'';
		h < 10 ? h = '0' + h : h;
		m < 10 ? m = '0' + m : m;
		s < 10 ? s = '0' + s : s;
		return h + ':' + m + ':' + s;
	}

	/*
	 * [获取日期模块 可扩展]
	 * modulename				[type]		description
	 * getDateModule.getDay		[number]	-1: 昨天; 0: 今天; 1: 明天; Return YYYY-MM-DD //getDateModule.getDay(1);
	 */
	var getDateModule = (function () {
		var getDay = function (day) {
			var today = new Date(), target = today.getTime() + 1000 * 60 * 60 * 24 * day;
			today.setTime(target);
			var tYear = today.getFullYear(), tMonth = today.getMonth(), tDate = today.getDate();
			tMonth = doHandleMonth( tMonth + 1 );
			tDate = doHandleMonth( tDate );
			return tYear + "-" + tMonth + "-" + tDate;
		};
		function doHandleMonth(month) {
			var m = month;
			if(month.toString().length == 1){
				m = "0" + month;
			}
			return m;
		}
		return {
			getDay : getDay
		};
	})();

	// 判断对象是否为空
	function isNullObj(obj){
		for(var i in obj){
			if(obj.hasOwnProperty(i)){
				return false;
			}
		}
		return true;
	}
	// 色波数组
	var waveData = [0, 0, 0, 1, 1, 2, 2, 0, 0, 1, 1, 2, 0, 0, 1, 1, 2, 2, 0, 0, 1, 2, 2, 0, 0, 1, 1, 2, 2, 0, 0, 1, 2, 2, 0, 0, 1, 1, 2, 2, 0, 1, 1, 2, 2, 0, 0, 1, 1, 2];
	// 返回生肖对应的号码 lef === 48(六肖，49号码为打和) || len === 49
	function returnZodiacNumber(len, type) {
		var zodiacNumberData = {};
		for(var i=1;i<=12;i++){
			zodiacNumberData[i] = [];
		}
		for(var i=1;i<=len;i++){
			if(type == 'str'){
				zodiacNumberData[p.zodiacData[i]].push(i<10?'0'+i:i+'');
			}else if(type == 'num'){
				zodiacNumberData[p.zodiacData[i]].push(i);
			}
		}
		return zodiacNumberData;
	}
	// 返回生肖
	function returnAnimal(i) {
		switch (i) {
			case 1:
				return "鼠";
			case 2:
				return "牛";
			case 3:
				return "虎";
			case 4:
				return "兔";
			case 5:
				return "龍";
			case 6:
				return "蛇";
			case 7:
				return "馬";
			case 8:
				return "羊";
			case 9:
				return "猴";
			case 10:
				return "雞";
			case 11:
				return "狗";
			case 12:
				return "豬";
			}
	}
	// 六肖 返回后端返回的值
	function getSixZodia(i) {
		return p.zodiacData[i];
	}
	// 特码摊子
	function getTanZi(i) {
		var initSid = 92560;
		var s = i%4;
		var b = 0;
		if(s == 0){
			b = 4;
		}else{
			b = s;
		}
		return initSid + b;
	}
	// 生肖
	function getZodiacSid (i) {
		var initSid = 92105 - 1;
		return (initSid + p.zodiacData[i]);
	}
	// 色波
	function getWaveSid (i) {
		var initSid = 92117;
		return (initSid + waveData[i]);
	}
	// 色波单双
	function getWaveSingleDouble (i) {
		var initSid = 0;
		if(getWaveSid(i) == 92117){
			initSid = 92122;
		}else if(getWaveSid(i) == 92118){
			initSid = 92126;
		}else if(getWaveSid(i) == 92119){
			initSid = 92130;
		}
		if((i % 2) == 0){
			return initSid + 1;
		} else {
			return initSid;
		}
	}
	// 色波大小
	function getWaveBigSmall(i) {
		var initSid = 0;
		if(getWaveSid(i) == 92117){
			initSid = 92120;
		}else if(getWaveSid(i) == 92118){
			initSid = 92124;
		}else if(getWaveSid(i) == 92119){
			initSid = 92128;
		}
		if(i > 24){
			return initSid;
		} else {
			return initSid + 1;
		}
	}
	// 单双
	function getSingleDouble(i) {
		var initSid = 92101;
		if((i % 2) == 0){
			return initSid + 1;
		} else {
			return initSid;
		}
	}
	// 大小
	function getBigSmall(i) {
		var initSid = 92099;
		if(i > 24){
			return initSid;
		} else {
			return initSid + 1;
		}
	}
	// 合数单双
	function getSumSingleDouble(i) {
		var initSid = 92103;
		var newSid = 0;
		if (i.toString().length == 1) {
			if ((i % 2) == 0) {
				newSid = 1;
			} else {
				newSid = 0;
			}
		} else {
			if (((Number(i.toString().substring(0, 1)) + Number(i.toString().substring(1, 2))) % 2) == 0) {
				newSid = 1;
			} else {
				newSid = 0;
			}
		}
		return initSid + newSid;
	}
	// 尾大尾小
	function getMantBigSmall(i) {
		var initSid = 92573;
		var newSid = 0;
		if (i.toString().length == 1) {
			if (Number(i) > 4) {
				newSid = 0;
			} else {
				newSid = 1;
			}
		} else {
			if (Number(i.toString().substring(1, 2)) > 4) {
				newSid = 0;
			} else {
				newSid = 1;
			}
		}
		return initSid + newSid;
	}

	// 页面加载后立即执行
	$(function () {
		Betimes._init();
	});

	// 快捷栏函数
	function ShortcutFun(){
		var _this = this;
		var num = 0;
		var box = $("#ShortcutBox");
		var menu = $("#ShortcutMenu a");
		var retention = $("#retention");
		var blockChange = $("#blockChange");
		var arr = [];
		var endArr = [];
		var tip = null;
		$("#ShortcutBtn").click(function () {
			if(num++ %2 == 0){
				box.show();
			}else{
				box.hide();
			}
		});

		menu.click(function () {
			var that = $(this);
			var prent = that.parent();
			if(!prent.hasClass('active')){
				prent.addClass('active');
			}else{
				prent.removeClass('active');
			}
			returnArr();
		});

		retention.change(function () {
			returnArr();
		});

		function returnArr() {
			var td = $("#ShortcutMenu td.active");
			var tdLen = td.length;
			// 清空数组
			arr = [];
			// 合并选中数组
			for(var i=0; i<tdLen; i++){
				var tdA = td.eq(i).find('a');
				arr = Array.union(arr, p.shortcutData[tdA.attr('data-id')]);
			}
			if(retention.val() != 'empty'){
				arr = Array.intersect(arr, p.shortcutData[retention.val()]);
			}
			endArr = arr;
		}


		var ShortcutOpen = $("#ShortcutOpen");
		var ShortcutClose = $("#ShortcutClose");

		ShortcutOpen.click(function () {
			if(endArr.length>0){
				Betimes.baseOddsAjax('8', returnSid(endArr), '', '', '1');
			}else{
				tip = tips.msgTips({
					msg: '請選擇快捷操作項',
					type : "error"
				});
			}
		});

		ShortcutClose.click(function () {
			if(endArr.length>0){
				Betimes.baseOddsAjax('8', returnSid(endArr), '', '', '0');
			}else{
				tip = tips.msgTips({
					msg: '請選擇快捷操作項',
					type : "error"
				});
			}
		});

		var ShortcutAdd = $("#ShortcutAdd");
		var ShortcutMin = $("#ShortcutMin");
		var ShortcutOddr = $("#ShortcutOddr");
		ShortcutAdd.click(function () {
			if(endArr.length>0){
				Betimes.baseOddsAjax('5', returnSid(endArr), ShortcutOddr.val(), '');
			}else{
				tip = tips.msgTips({
					msg: '請選擇快捷操作項',
					type : "error"
				});
			}
		});
		ShortcutMin.click(function () {
			if(endArr.length>0){
				Betimes.baseOddsAjax('6', returnSid(endArr), ShortcutOddr.val(), '');
			}else{
				tip = tips.msgTips({
					msg: '請選擇快捷操作項',
					type : "error"
				});
			}
		});

		var ShortcutSubmit = $("#ShortcutSubmit");
		var ShortcutText = $("#ShortcutText");
		ShortcutSubmit.click(function () {
			if(endArr.length>0){
				if(ShortcutText.val() == ''){
					tip = tips.msgTips({
						msg: '請輸入微調值',
						type : "error"
					});
					ShortcutText.focus();
				}else{
					Betimes.baseOddsAjax('7', returnSid(endArr), '', ShortcutText.val());
				}
			}else{
				tip = tips.msgTips({
					msg: '請選擇快捷操作項',
					type : "error"
				});
			}
		});

		function returnSid (arr) {
			var sidArr = [];
			var str = '';
			var block = '';
			if(blockChange.length>0){
				block = '#'+ blockChange.val() +' ';
			}
			for(var i=0; i<arr.length; i++){
				sidArr.push($( block + 'tr[data-num='+ arr[i] +']').attr('data-id'));
			}
			str = sidArr.uniquelize().join(',');
			return str;
		}
	}

	// 绑定改变窗口尺寸事件
	$(window).unbind('resize').bind('resize', function () {
		// 找到最后一次走飞窗口触发对象，并重新跑一遍走飞数据
		Betimes.flyAwayModuleFlyInit(Betimes.findLastObj(Betimes.flyAwayThat));
		// 找到最后一次赔率窗口触发对象，并重新跑一遍赔率数据
		Betimes.oddsTrimModuleInit(Betimes.findLastObj(Betimes.aOddsThat));
	});
	// 绑定窗口尺寸事件
	$(window).resize();

	// 开奖号码盈亏提示方法
	function winnerFun(lw, ta) {
		var mxVal = $("input[name=radiobutton]:checked").attr("data-radio");
		// 每次都清空className
		lmClassName1 = '';
		lmClassName2 = '';

		var a=[], b=[], c = [], l=0, m=0;

		switch (mxVal)
		{
		case '92285': //三全中
			a = lw.split(',');
			a.splice(a.length-1, 1); //不算特码
			b = ta.split(',');
			l = $.arrayIntersect(a, b).length;
			if(l == 3){
				lmClassName1 = 'winner';
			}
		  break;
		case '92638': //lm_b三全中
			a = lw.split(',');
			a.splice(a.length-1, 1); //不算特码
			b = ta.split(',');
			l = $.arrayIntersect(a, b).length;
			if(l == 3){
				lmClassName1 = 'winner';
			}
		  break;
		case '92286': //三中二
			a = lw.split(',');
			a.splice(a.length-1, 1); //不算特码
			b = ta.split(',');
			l = $.arrayIntersect(a, b).length;
			if(l == 3){
				lmClassName2 = 'winner';
			}else if(l == 2){
				lmClassName1 = 'winner';
			}
		  break;
		case '92639': //三中二
			a = lw.split(',');
			a.splice(a.length-1, 1); //不算特码
			b = ta.split(',');
			l = $.arrayIntersect(a, b).length;
			if(l == 3){
				lmClassName2 = 'winner';
			}else if(l == 2){
				lmClassName1 = 'winner';
			}
		  break;
		case '92287': //二全中
			a = lw.split(',');
			a.splice(a.length-1, 1); //不算特码
			b = ta.split(',');
			l = $.arrayIntersect(a, b).length;
			if(l == 2){
				lmClassName1 = 'winner';
			}
		  break;
		case '92640': //二全中
			a = lw.split(',');
			a.splice(a.length-1, 1); //不算特码
			b = ta.split(',');
			l = $.arrayIntersect(a, b).length;
			if(l == 2){
				lmClassName1 = 'winner';
			}
		  break;
		case '92288': //二中特
			a = lw.split(',');
			c = a.splice(a.length-1, 1); //不算特码
			b = ta.split(',');
			l = $.arrayIntersect(a, b).length;
			m = $.arrayIntersect(c, b).length;
			if(m == 1 && l == 1){
				lmClassName1 = 'winner';
			}else if(l == 2){
				lmClassName2 = 'winner';
			}
		  break;
		case '92641': //二中特
			a = lw.split(',');
			c = a.splice(a.length-1, 1); //不算特码
			b = ta.split(',');
			l = $.arrayIntersect(a, b).length;
			m = $.arrayIntersect(c, b).length;
			if(m == 1 && l == 1){
				lmClassName1 = 'winner';
			}else if(l == 2){
				lmClassName2 = 'winner';
			}
		  break;
		case '92289': //特串
			a = lw.split(',');
			c = a.splice(a.length-1, 1); //不算特码
			b = ta.split(',');
			l = $.arrayIntersect(a, b).length;
			m = $.arrayIntersect(c, b).length;
			if(m == 1 && l == 1){
				lmClassName1 = 'winner';
			}
		  break;
		case '92642': //特串
			a = lw.split(',');
			c = a.splice(a.length-1, 1); //不算特码
			b = ta.split(',');
			l = $.arrayIntersect(a, b).length;
			m = $.arrayIntersect(c, b).length;
			if(m == 1 && l == 1){
				lmClassName1 = 'winner';
			}
		  break;
		case '92575': //四中一
			a = lw.split(',');
			b = ta.split(',');
			l = $.arrayIntersect(a, b).length;
			if(l>0){
				lmClassName1 = 'winner';
			}
		  break;
		case '92643': //四中一
			a = lw.split(',');
			b = ta.split(',');
			l = $.arrayIntersect(a, b).length;
			if(l>0){
				lmClassName1 = 'winner';
			}
		  break;
		case '92572': //五不中
		case '92588': //六不中
		case '92589': //七不中
		case '92590': //八不中
		case '92591': //九不中
		case '92592': //十不中
			a = lw.split(',');
			b = ta.split(',');
			l = $.arrayIntersect(a, b).length;
			if(l == 0){
				lmClassName1 = 'winner';
			}
		  break;
		case '92565': //六肖
			a = lw.split('|')[0].split(',');
			c = a.splice(a.length-1, 1); // 特码
			b = ta.split(',');
			l = $.arrayIntersect(c, b).length;
			if(l > 0 && lw.split('|')[2] != '49'){ //49为打和
				lmClassName1 = 'winner';
			}
		  break;
		case '92566': //二肖连
			a = lw.split('|')[0].split(',');
			b = ta.split(',');
			l = $.arrayIntersect(a, b).unique().length;
			if(l >= 2){
				lmClassName1 = 'winner';
			}
		  break;
		case '92567': //三肖连
			a = lw.split('|')[0].split(',');
			b = ta.split(',');
			l = $.arrayIntersect(a, b).unique().length;
			if(l >= 3){
				lmClassName1 = 'winner';
			}
		  break;
		case '92568': //四肖连
			a = lw.split('|')[0].split(',');
			b = ta.split(',');
			l = $.arrayIntersect(a, b).unique().length;
			if(l >= 4){
				lmClassName1 = 'winner';
			}
		  break;
		case '92636': //五肖连
			a = lw.split('|')[0].split(',');
			b = ta.split(',');
			l = $.arrayIntersect(a, b).unique().length;
			if(l >= 5){
				lmClassName1 = 'winner';
			}
		  break;
		case '92569': //二尾连
			a = lw.split('|')[1].split(',');
			b = ta.split(',');
			l = $.arrayIntersect(a, b).unique().length;
			if(l >= 2){
				lmClassName1 = 'winner';
			}
		  break;
		case '92570': //三尾连
			a = lw.split('|')[1].split(',');
			b = ta.split(',');
			l = $.arrayIntersect(a, b).unique().length;
			if(l >= 3){
				lmClassName1 = 'winner';
			}
		  break;
		case '92571': //四尾连
			a = lw.split('|')[1].split(',');
			b = ta.split(',');
			l = $.arrayIntersect(a, b).unique().length;
			if(l >= 4){
				lmClassName1 = 'winner';
			}
		  break;
		case '92637': //五尾连
			a = lw.split('|')[1].split(',');
			b = ta.split(',');
			l = $.arrayIntersect(a, b).unique().length;
			if(l >= 5){
				lmClassName1 = 'winner';
			}
		  break;
		}
	}

	// 处理正码/生肖尾数中/生肖尾数不中
	function sumZmSxWs() {
		var i = 0;
		var amountData = Betimes.myData;
		var sumAmount = 0;
		var sumRec = 0;
		// 正码盈亏数组
		var arrZmWin = [];
		var arrSxZhWin = [];
		var arrWsZhWin = [];
		var arrSxBzWin = [];
		var arrWsBzWin = [];
		var arrZmAmount = [];
		var arrSxZhAmount = [];
		var arrWsZhAmount = [];
		var arrSxBzAmount = [];
		var arrWsBzAmount = [];
		// 所有排序按照从小到大排列
		if (p.playpage == 'zm') {
			// 正码
			for (i = 92132; i<=92180; i++) {
				arrZmWin.push(0-amountData[i]['WinValue'])
				arrZmAmount.push(amountData[i]['AmountValue']);
				sumAmount += amountData[i]['AmountValue'];
				sumRec += amountData[i]['RecValue'];
			}
			arrZmWin = myQuickSort(arrZmWin);
			arrZmAmount = myQuickSort(arrZmAmount);
			$("#sumQuota").html( parseInt(sumAmount) );
			$("#sumRec").html( parseInt(sumRec) );
			// 最大负值（从大到小）盈亏前6（负的）的和 + 注额（从小到大）前（0 ~ 49-6）的和 - 总退水
			$("#minQuota").html( parseInt(mySum(0, 6, arrZmWin) + mySum(0, arrZmAmount.length-6, arrZmAmount) - sumRec) );
			// 最小负值（从大到小）盈亏后6（负的）的和 + 注额（从小到大）前（6 ~ 49）的和 - 总退水
			$("#maxQuota").html( parseInt(mySum(arrZmWin.length-6, arrZmWin.length, arrZmWin) + mySum(6, arrZmAmount.length, arrZmAmount) - sumRec) );
		}
		if (p.playpage == 'sxws'){
			// 生肖中
			for (i = 92290; i<=92301; i++) {
				arrSxZhWin.push(0-amountData[i]['WinValue'])
				arrSxZhAmount.push(amountData[i]['AmountValue']);
				sumAmount += amountData[i]['AmountValue'];
				sumRec += amountData[i]['RecValue'];
			}
			arrSxZhWin = myQuickSort(arrSxZhWin);
			arrSxZhAmount = myQuickSort(arrSxZhAmount);
			$("#sxSum").html( parseInt(sumAmount) );
			$("#sxRec").html( parseInt(sumRec) );
			// （前7都中）最大负值（从大到小）盈亏前7（负的）的和 + 注额（从小到大）前（0 ~ 12-7）的和 - 总退水
			$("#sxMin").html( parseInt(mySum(0, 7, arrSxZhWin) + mySum(0, arrSxZhAmount.length-7, arrSxZhAmount) - sumRec) );
			// （最小的2个中）最小负值（从大到小）盈亏后2（负的）的和 + 注额（从小到大）前（2 ~ 12）的和 - 总退水
			$("#sxMax").html( parseInt(mySum(arrSxZhWin.length-2, arrSxZhWin.length, arrSxZhWin) + mySum(2, arrSxZhAmount.length, arrSxZhAmount) - sumRec) );

			sumAmount = 0;
			sumRec = 0;
			// 尾数中
			for (i = 92302; i<=92311; i++) {
				arrWsZhWin.push(0-amountData[i]['WinValue'])
				arrWsZhAmount.push(amountData[i]['AmountValue']);
				sumAmount += amountData[i]['AmountValue'];
				sumRec += amountData[i]['RecValue'];
			}
			arrWsZhWin = myQuickSort(arrWsZhWin);
			arrWsZhAmount = myQuickSort(arrWsZhAmount);
			$("#wsSum").html( parseInt(sumAmount) );
			$("#wsRec").html( parseInt(sumRec) );
			// （前7都中）最大负值（从大到小）盈亏前7（负的）的和 + 注额（从小到大）前（0 ~ 10-7）的和 - 总退水
			$("#wsMin").html( parseInt(mySum(0, 7, arrWsZhWin) + mySum(0, arrWsZhAmount.length-7, arrWsZhAmount) - sumRec) );
			// （最小的2个中）最小负值（从大到小）盈亏后2（负的）的和 + 注额（从小到大）前（2 ~ 10）的和 - 总退水
			$("#wsMax").html( parseInt(mySum(arrWsZhWin.length-2, arrWsZhWin.length, arrWsZhWin) + mySum(2, arrWsZhAmount.length, arrWsZhAmount) - sumRec) );
		}
		if (p.playpage == 'sxwsbz'){
			// 生肖不中
			for (i = 92614; i<=92625; i++) {
				arrSxBzWin.push(0-amountData[i]['WinValue'])
				arrSxBzAmount.push(amountData[i]['AmountValue']);
				sumAmount += amountData[i]['AmountValue'];
				sumRec += amountData[i]['RecValue'];
			}
			arrSxBzWin = myQuickSort(arrSxBzWin);
			arrSxBzAmount = myQuickSort(arrSxBzAmount);
			$("#sxBzSum").html( parseInt(sumAmount) );
			$("#sxBzRec").html( parseInt(sumRec) );
			// （前10都不中）最大负值（从大到小）盈亏前10（负的）的和 + 注额（从小到大）前（0 ~ 12-10）的和 - 总退水
			$("#sxBzMin").html( parseInt(mySum(0, 10, arrSxBzWin) + mySum(0, arrSxBzAmount.length-10, arrSxBzAmount) - sumRec) );
			// （最小2个不中）最大负值（从大到小）盈亏后2（负的）的和 + 注额（从小到大）前（2 ~ 12）的和 - 总退水
			$("#sxBzMax").html( parseInt(mySum(arrSxBzWin.length-2, arrSxBzWin.length, arrSxBzWin) + mySum(2, arrSxBzAmount.length, arrSxBzAmount) - sumRec) );
			sumAmount = 0;
			sumRec = 0;
			// 尾数不中
			for (i = 92626; i<=92635; i++) {
				arrWsBzWin.push(0-amountData[i]['WinValue'])
				arrWsBzAmount.push(amountData[i]['AmountValue']);
				sumAmount += amountData[i]['AmountValue'];
				sumRec += amountData[i]['RecValue'];
			}
			arrWsBzWin = myQuickSort(arrWsBzWin);
			arrWsBzAmount = myQuickSort(arrWsBzAmount);
			$("#wsBzSum").html( parseInt(sumAmount) );
			$("#wsBzRec").html( parseInt(sumRec) );
			// （前8都不中）最大负值（从大到小）盈亏前8（负的）的和 + 注额（从小到大）前（0 ~ 10-8）的和 - 总退水
			$("#wsBzMin").html( parseInt(mySum(0, 8, arrWsBzWin) + mySum(0, arrWsBzAmount.length-8, arrWsBzAmount) - sumRec) );
			// （最小2个不中）最大负值（从大到小）盈亏后2（负的）的和 + 注额（从小到大）前（2 ~ 10）的和 - 总退水
			$("#wsBzMax").html( parseInt(mySum(arrWsBzWin.length-2, arrWsBzWin.length, arrWsBzWin) + mySum(2, arrWsBzAmount.length, arrWsBzAmount) - sumRec) );
		}

		function myQuickSort (arr) {
			var newArr = [];
			newArr = arr.sort(function(a, b){
				return a - b
			});
			return newArr;
		}

		function mySum (start, end, arr) {
			var sum = 0;
			var newArr = arr.slice(start, end);
			for (var i = 0; i < newArr.length; i++) {
				sum += newArr[i] << 0;
			}
			return sum;
		}
	}

	module.exports = Betimes;
});
