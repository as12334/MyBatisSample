define(function(require, exports, module){
	// 引入jQuery模块
	var $ = require('jquery');

	var Betimes = require('Betimes')
	// 引入jquery插件
	require('plus')($);
	// 引入tips插件
	var tips = require('tips');
	// 引入myLayer插件
	require('myLayer')($);

	var usertype = parent.usertype;

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

	var p = window.parent;
	// 引入封装好的ajax
	var getBaseDataAjax = require('getBaseDataAjax');

	var fastFlyAway = {
		// 特码快速走飞
		tmFastFlyAway: function	() {
			// 最大id值
			var minId = 0;
			// 最大id值
			// var maxId = 0;
			// 特码a走快速飞
			if (p.playpage == 'tmzx2' || p.playpage == 'tmzx1' || p.playpage == 'tma' || p.playpage == 'tmab' || p.playpage == 'tmzx2_2' || p.playpage == 'tmzx3') {
				minId = 92000;
				// maxId = 92049;
			// 特码b快速走飞
			}else if(p.playpage == 'tmb'){
				minId = 92049;
				// maxId = 92098;
			}else{
				return;
			}

			var fastFlyAwayUserNameHtml = '';

			var isAB = false;
			if(p.playpage == 'tmab' || p.playpage == 'tmzx1' || p.playpage == 'tmzx2' || p.playpage == 'tmzx3'){
				isAB = true;
			}
			var isReadonly = '';
			if(usertype == '1'){
				// 设置出货会员
				var allowSaleData = p.aAllowSaleUserName.saleuser;
				var idText = '';
				var idClass = '';
				for(var key in allowSaleData){
					if (allowSaleData[key] == '1') {
						idText = '【內補】';
						idClass = 'red';
					}else{
						idText = '【外補】';
						idClass = 'blue';
					}
					fastFlyAwayUserNameHtml += '<option class="'+ idClass +'" value="'+ key +'">' + key + idText +'</option>';
				}
				fastFlyAwayUserNameHtml ='<select name="select" class="h18" id="fastFlyAwayUserName">'+
										fastFlyAwayUserNameHtml+
								'</select>';
				isReadonly = '';
			}else{
				isReadonly = 'readonly';
			}
			var tmbhItemArr = [];
			var tmbhHtmlPd = '';
			var nowId = 0;
			var nowIndex = '';
			var myData = Betimes.myData;
			// 生成特码快速走飞节点
			for(var i=1; i<=49; i++){
				nowId = minId + i;
				nowIndex = i<10? '0'+i : ''+i;
				tmbhHtmlPd = '';
				if (i==1 || i==11 || i==21 || i==31 || i== 41) {
					tmbhHtmlPd += '<div class="tmbhBox">';
				}
				tmbhHtmlPd += '<div class="tmbhList">'+
								'<span class="num red">'+ nowIndex +'</span>'+
								'<input type="text" name="pl" id="tmbhPl_'+ i +'" data-odds="'+ myData[nowId]['OddsValue'] +'" class="blue1 '+ isReadonly +'" '+ isReadonly +' value="'+ myData[nowId]['OddsValue'] +'">'+
								'<input type="text" name="amount" class="zfNumber" value="">'+
							'</div>';
				if(i == 10 || i==20 || i==30 || i==40 || i==49){
					tmbhHtmlPd += '</div>';
				}
				tmbhItemArr.push(tmbhHtmlPd);
			}
			// 列表对象
			var list = '<div class="tmbhWrap">'+
					'<table>'+
						'<thead>'+
							'<tr>'+
								'<th align="left" style="font-weight:normal;" class="tdbg2" colspan="6">'+
									'&nbsp;&nbsp;每註保畱額度(超過部分補出)：<input type="text" class="text w80" id="calculateInput" name="textfield">&nbsp;&nbsp;'+
									'<button name="Submit" type="submit" id="calculate">計算補貨</button>'+
									fastFlyAwayUserNameHtml +
								'</th>'+
							'</tr>'+
						'</thead>'+
					'</table>'+
					'<div class="tmbhTitleWrap">'+
						'<div class="tmbhTitle">'+
							'<span class="num">號碼</span>'+
							'<span>賠率</span>'+
							'<span>金額</span>'+
						'</div>'+
						'<div class="tmbhTitle">'+
							'<span class="num">號碼</span>'+
							'<span>賠率</span>'+
							'<span>金額</span>'+
						'</div>'+
						'<div class="tmbhTitle">'+
							'<span class="num">號碼</span>'+
							'<span>賠率</span>'+
							'<span>金額</span>'+
						'</div>'+
						'<div class="tmbhTitle">'+
							'<span class="num">號碼</span>'+
							'<span>賠率</span>'+
							'<span>金額</span>'+
						'</div>'+
						'<div class="tmbhTitle">'+
							'<span class="num">號碼</span>'+
							'<span>賠率</span>'+
							'<span>金額</span>'+
						'</div>'+
					'</div>'+
					'<div class="tmbhMain">'+
						tmbhItemArr.join('')+
					'</div>'+
				'</div>';


			$("#fastBtn").myLayer({
				title: '快速補貨',
				content: list,
				isMiddle: true,
				okText: '快速補出',
				okCallBack: function (obj) {
					var aPlId = [];
					var aPl = [];
					var aAmount = [];
					var tmbhList = obj.find('.tmbhList');
					var tmbhListLength = tmbhList.length;
					var isOk = false;
					if (Betimes.isMyOpen == 'n') {
						tips.msgTips({
							msg: "已封盤！",
							type : "error"
						});
						Betimes.isTmFastFlyAway = false;
						$.myLayer.close(true);
						isOk = false;
						return;
					}
					// 判断并提交数据数组
					for (var i = 1; i <= tmbhListLength; i++) {
						var nowId = minId + i;
						var pl = tmbhList.eq(i-1).find('input[name=pl]');
						var amount = tmbhList.eq(i-1).find('input[name=amount]');
						var maxAmount = 0;
						var maxOdds = 0;
						if (isAB) {
							if (nowId>=92001 && nowId<=92049) {
								maxAmount = Number(myData[nowId]['AmountValue']) + Number(myData[nowId+49]['AmountValue']);
							}else{
								maxAmount = Number(myData[nowId]['AmountValue']);
							}
						}else{
							maxAmount = Number(myData[nowId]['AmountValue']);
						}
						if(usertype == '1'){
							maxOdds = 1000;
							minOdds = 0;
						}else{
							maxOdds = Number(myData[nowId]['OddsMaxValue']);
							minOdds = Number(myData[nowId]['OddsMinValue']);
						}
						if( Number(pl.val()) > maxOdds ){
							pl.focus();
							pl.myxTips({
								content: '當前修改賠率不能大於系統設定最大賠率' + maxOdds
							});
							isOk = false;
							break;
						}else if( Number(pl.val()) < minOdds ){
							pl.focus();
							pl.myxTips({
								content: '當前修改賠率不能小於' + minOdds
							});
							isOk = false;
							break;
						}else if( pl.val() == '' || isNaN(pl.val()) ){
							pl.focus();
							pl.myxTips({
								content: '請輸入有效的賠率值!'
							});
							isOk = false;
							break;
						}else if( isNaN(amount.val()) ){
							amount.focus();
							amount.myxTips({
								content: '請輸入有效的補貨金額!'
							});
							isOk = false;
							break;
						}else if( Number(amount.val()) > maxAmount && p.negativesale != '1'){
							amount.focus();
							amount.myxTips({
								content: '補貨金額不能大於下注額!'
							});
							isOk = false;
							break;
						}else{
							if(Number(amount.val()) > 0 && amount.val() != ''){
								aPlId.push(nowId);
								aPl.push(pl.val());
								aAmount.push(amount.val());
								isOk = true;
							}
						}
					}
					// 提交
					if(isOk){
						$.myLayer.showLoading();
						var fData = {
							action: 'set_allowsale',
							phaseid: $('#nn').attr('data-pId'),
							pk: $("#upHandicap").val(),
							oddsid: aPlId.join(','),
							currentodds: aPl.join(','),
							amount: aAmount.join(','),
							// number: nowInput.attr('data-num'),
							fast: 2,
							s_user: $("#fastFlyAwayUserName").val()
						};
						Betimes.baseSzszAjax( fData, '');
						// console.log(aPlId, aPl, aAmount);
					}
				},
				openCallBack: function (obj) {
					$("#fastBtn").removeAttr('disabled');
					// 增加外网会员赔率
					var fastFlyAwayUserName = $("#fastFlyAwayUserName");
					// 取当前外网会员下拉框的值
					var nowUserNameVal = fastFlyAwayUserName.val();
					// 获取当前外网会员数据
					var allowSaleData = p.aAllowSaleUserName.saleuser;
					// 取当前外网会员的类型
					var nowAllowType = allowSaleData[nowUserNameVal];
					// 总监处理
					if (p.usertype == '1') {
						_initFlyAwayUserOdds(nowAllowType, nowUserNameVal);
					}

					fastFlyAwayUserName.unbind('change').bind('change', function () {
						_initFlyAwayUserOdds(allowSaleData[fastFlyAwayUserName.val()], fastFlyAwayUserName.val());
					});

					// 初始化总监快速走飞赔率函数
					function _initFlyAwayUserOdds(allowType, s_user) {
						var oddsObj = null;
						if (allowType == '0') {
							for(var i=1; i<=49; i++){
								oddsObj = $("#tmbhPl_" + i);
								oddsObj.val(oddsObj.attr('data-odds')).removeAttr('readonly').removeClass('readonly');
								// oddsObj.val(oddsObj.attr('data-odds')).attr('readonly', isReadonly).addClass(isReadonly);
							}
						}else{
							var b = new getBaseDataAjax({
								url: 'Handler/Handler.ashx',
								_type: 'POST',
								dataType: 'json',
								postData: {
									'action': 'get_oddsinfoex',
									's_user': s_user, 
									'fast': 2
								},
								async: false,
								completeCallBack:function () {},
								successCallBack:function (data) {
									var nOdds = data.data['pl'].split(',');
									for(var i=1; i<=49; i++){
										oddsObj = $("#tmbhPl_" + i);
										oddsObj.val(nOdds[i-1]).attr('readonly', true).addClass('readonly');
									}
								},
								errorCallBack:function () {}
							});
						}
					}

					var calculateInput = $("#calculateInput");
					var calculateBtn = $("#calculate");
					calculateBtn.click(function () {
						var tmbhList = obj.find('.tmbhList');
						var tmbhListLength = tmbhList.length;
						var caVal = calculateInput.val();
						if(!isNaN(caVal) && caVal != ''){
							for (var i = 1; i <= tmbhListLength; i++) {
								var nowId = minId + i;
								var amount = tmbhList.eq(i-1).find('input[name=amount]');
								var amountval = 0;
								if (isAB) {
									amountval = Number(myData[nowId]['AmountValue']) + Number(myData[nowId+49]['AmountValue']);
								}else{
									amountval = Number(myData[nowId]['AmountValue']);
								}
								var nowAmount = parseInt(amountval-Number(caVal));
								if(nowAmount <= 0){
									nowAmount = '';
								}
								amount.val( nowAmount );
							}
						}else if(caVal == ''){
							calculateInput.focus();
							calculateInput.myxTips({
								content: '請輸入每註保畱額度！'
							});
						}else{
							calculateInput.focus();
							calculateInput.myxTips({
								content: '當前項不能為空！'
							});
						}
					});
				},
				closeCallBack: function	() {
					Betimes.isTmFastFlyAway = false;
					$('#upSecond option').eq(1).prop('selected',true);
					$('#upSecond').change();
				}
			})
		},
		// 连码快速走飞
		lmFastFlyAway: function (data) {
			var _tpl = '';
			var _tplList = '';
			var radio = $("input[name=radiobutton]:checked");
			var radioKey = radio.attr('data-radio');
			var radioName = radio.attr('data-name');
			var num = 0;
			var csNum = 0;
			var titleHtml = '';
			var oddsHtml = '';
			var pceHtml = null;

			// 关闭默认更新赔率接口
			$('#upSecond option[value=No]').prop('selected',true);
			$('#upSecond').change();

			var fastFlyAwayData = {};
			var fastFlyAwayUserNameHtml = '';
			// 快速走非对象
			fastFlyAwayData = data.lxl_szsz_amount || data.lm_szsz_amount || data.lmbhlist;

			if(p.pathFolder == 'L_SIX'){
				// 总监
				if (usertype == '1') {
					// 设置出货会员
					var allowSaleData = p.aAllowSaleUserName.saleuser;
					for(var key in allowSaleData){
						fastFlyAwayUserNameHtml += '<option value="'+ key +'">' + key +'</option>';
					}
					fastFlyAwayUserNameHtml ='<select name="select" class="h18" id="fastFlyAwayUserName">'+ fastFlyAwayUserNameHtml +'</select>';
				}
				// 六合彩双赔率
				if( radioKey == '92286' || radioKey == '92288'){
					csNum = 7;
					titleHtml = '<td class="tdbg1">派彩額1</td>'+
								'<td class="tdbg1">派彩額2</td>';
				}else {
					csNum = 6;
					titleHtml = '<td class="tdbg1">派彩額</td>';
				}
			}else{
				csNum = 6;
				titleHtml = '<td class="tdbg1">派彩額</td>';
			}

			function returnTpl(a) {
				var b = '';
				if( radioKey == '92286' || radioKey == '92288'){
					b = '<td class="red">-'+
							parseInt(a)+
						'</td>';
				}
				return b;
			}

			var currentodds = Betimes.myData[radioKey].OddsValue;

			for(var key in fastFlyAwayData){
				var nameId = key.split(',').join('_');
				var name = key;
				var thatOddsHtml = '';
				num++;
				// 返回生肖
				if(radioKey == 92565 || radioKey == 92566 || radioKey == 92567 || radioKey == 92568){
					var arr = key.split(','), newStr = [];
					for(var i=0; i<arr.length; i++){
						newStr.push(returnAnimal(Number(arr[i])));
					}
					name = newStr.join(',');
				}

				if (usertype == '1') {
					if (radioKey == '92286' || radioKey == '92288') {
						thatOddsHtml = '<td><input type="text" value="'+ currentodds.split(',')[0] +'" class="text w80" id="odds_'+ nameId +'_1"/></td>'+
										'<td><input type="text" value="'+ currentodds.split(',')[1] +'" class="text w80" id="odds_'+ nameId +'_2"/></td>';
					}else {
						thatOddsHtml = '<td><input type="text" value="'+ currentodds +'" class="text w80" id="odds_'+ nameId +'"/></td>';
					}
				}

				_tplList += '<tr data-key="'+ nameId +'">'+
								'<td>'+
									num+
								'</td>'+
								'<td>'+
									name+
								'</td>'+
								'<td class="green"><input value="" type="text" data-num="'+ key +'" data-value="'+ fastFlyAwayData[key].split(',')[0] +'" name="textfield" class="text w80 lmbhInput" id="input_'+ nameId +'"></td>'+
								'<td class="blue"><b id="result_'+ nameId +'"></b></td>'+
								'<td>'+
									parseInt(fastFlyAwayData[key].split(',')[0])+
								'</td>'+
								'<td class="red">-'+
									parseInt(fastFlyAwayData[key].split(',')[1])+
								'</td>'+
								returnTpl(fastFlyAwayData[key].split(',')[2])+
							'</tr>';
			}
			_tpl = '<table id="fastFlyAwayBox" class="t_list">'+
						'<thead>'+
							'<tr>'+
								'<th colspan="'+ csNum +'" class="tdbg2" align="left" style="font-weight:normal;">'+
									'&nbsp;&nbsp;每註保畱額度(超過部分補出)：'+
									'<input type="text" name="textfield" id="calculateInput" class="text w80">'+
									'&nbsp;&nbsp;'+
									'<button id="calculate" type="submit" name="Submit" class="btn">'+
										'計算補貨'+
									'</button>'+
									fastFlyAwayUserNameHtml+
								'</th>'+
							'</tr>'+
						'</thead>'+
						'<thead>'+
							'<tr>'+
								'<th colspan="'+ csNum +'">'+
									'『'+
									'<b class="red lmbhTitle">'+
										radioName +
									'</b>'+
									'』 按單組累計 &mdash; 百大排行'+
								'</th>'+
							'</tr>'+
							'<tr>'+
								'<td class="tdbg1">'+
									'排名'+
								'</td>'+
								'<td class="tdbg1">'+
									'號碼組合'+
								'</td>'+
								'<td class="tdbg1">'+
									'補貨金額'+
								'</td>'+
								'<td class="tdbg1">'+
									'補貨結果'+
								'</td>'+
								'<td class="tdbg1">'+
									'下註額'+
								'</td>'+
								titleHtml +
							'</tr>'+
						'</thead>'+
						'<tbody id="fastFlyAwayListWrap" class="lmbh_list">'+
							_tplList +
						'</tbody>'+
					'</table>';


			$(".fastbh").myLayer({
				title: '快速補貨',
				content: _tpl,
				isMiddle: true,
				okText: '快速補出',
				okCallBack: function (obj) {
					var okBtn = obj.find('.myLayerOk');
					okBtn.addClass('grayBtn1');
					var aTr = $("#fastFlyAwayListWrap").find('tr');
					var calculateInput = $("#calculateInput");
					// if(calculateInput.val() == ''){
					// 	calculateInput.focus();
					// 	calculateInput.myxTips({
					// 		content: '請輸入每註保畱額度!'
					// 	});
					// 	return false;
					// }
					var Len = aTr.length;
					var eachIndex = 0;
					DetailsTrAjax(eachIndex);
					function DetailsTrAjax(eachIndex) {
						if(eachIndex == Len){
							okBtn.addClass('grayBtn1');
							tip = tips.msgTips({
								msg: '補貨完成!',
								type : "warning"
							});
							setTimeout(function () {
								$("#myLayer_"+ top.myLayerIndex + " .myLayerClose").click();
								// $.myLayer.close(true);
							}, 10000);
							return;
						}else{
							var that = aTr.eq(eachIndex);
							var thisKey = that.attr('data-key');
							var nowInput = $("#input_"+ thisKey +"");
							if (Betimes.isMyOpen == 'n') {
								tips.msgTips({
									msg: "已封盤！",
									type : "error"
								});
								// $.myLayer.close(true);
								isOk = false;
								return;
							}
							if(Number(nowInput.val()) <= 0 && nowInput.val() != ''){
								nowInput.focus();
								nowInput.myxTips({
									content: '請輸入有效的補貨金額!'
								});
								$("#result_" + thisKey).html('<span class="red">未補貨</span>');
								eachIndex++;
								DetailsTrAjax(eachIndex);
								// return;
							}else if((Number(nowInput.val()) > Number(nowInput.attr('data-value'))) && p.negativesale != '1'){
								nowInput.focus();
								nowInput.myxTips({
									content: '補貨金額不能大於下注額!'
								});
								$("#result_" + thisKey).html('<span class="red">未補貨</span>');
								eachIndex++;
								DetailsTrAjax(eachIndex);
								// return;
							}else{
								if(nowInput.val() != ''){
									nowInput.focus();
									$("#result_" + thisKey).html('補貨中....');
									var fData = {
										action: 'set_allowsale',
										phaseid: $('#nn').attr('data-pId'),
										pk: $("#upHandicap").val(),
										oddsid: radioKey,
										currentodds: Betimes.myData[radioKey].OddsValue,
										amount: nowInput.val(),
										number: nowInput.attr('data-num'),
										fast: 1,
										s_user: $("#fastFlyAwayUserName").val()
									};
									Betimes.baseSzszAjax( fData, thisKey, function () {
										eachIndex++;
										DetailsTrAjax(eachIndex);
									});
								}else{
									eachIndex++;
									DetailsTrAjax(eachIndex);
								}
							}
						}
					}
				},
				openCallBack: function (obj) {
					var calculateInput = $("#calculateInput");
					var calculateBtn = $("#calculate");
					var aTr = $("#fastFlyAwayListWrap").find('tr');
					var okBtn = obj.find('.myLayerOk');
					// okBtn.addClass('grayBtn1');
					// 计算補貨额度
					calculateBtn.unbind('click').click(function () {
						var cValue = calculateInput.val();
						if(calculateInput.val() == ''){
							calculateInput.myxTips({
								content: '請輸入每註保畱額度!'
							});
							return false;
						}
						aTr.each(function () {
							var that = $(this);
							var thisKey = that.attr('data-key');
							var nowInput = $("#input_"+ thisKey +"");
							var bhv = 0;
							var cInputVal = calculateInput.val();
							if(cInputVal != ''){
								var endValue = Number(nowInput.attr('data-value'))-Number(cInputVal);
								if(endValue <= 0){
									bhv = '';
								}else{
									bhv = parseInt(endValue);
								}
							}else{
								bhv = '';
							}

							okBtn.removeClass('grayBtn1');
							nowInput.val( bhv );
						});
					});

				},
				closeCallBack: function () {
					// 恢复赔率加载函数
					$('#upSecond option[value=20]').prop('selected',true);
					$('#upSecond').change();
				}
			});
		}
	}

	module.exports = fastFlyAway;
});
