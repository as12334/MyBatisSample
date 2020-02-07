 define(function (require, exports, module) {
	var $  = require('jquery');
	var getBaseDataAjax = require('getBaseDataAjax');
	require('plus')($);
	var timer = null;
	var oldTime = '';

	var myAmount = 0;

	var amount = $("#amount");

	amount.val(myAmount);

	var oBody = $("body");
	oBody.onlyNumber({
		className: '.text',
		isDecimal: false,
		isMinus: false
	});

	$('form').submit(function () {
		return false;
	});

	// 初始化加载列表
	baseAjax();

	$("#sltSeconds").change(function () {
		clearTimeout(timer);
		$("#updata").html('加載中');
		oldTime = '';
		top.oldId = 0;
		$("#listWrap").html('');
		$("#autoJpWrap").html('');
		baseAjax();
	});

	var newBetTitle = $("#newBetTitle");
	var newBetTitleTop = newBetTitle.offset().top;
	var listWrap = $("#listWrap");
	// newBetTitle.width($(document).width());

	$(window).resize(function(){
		newBetTitle.width(listWrap.width()+1);
	});

	$(window).scroll(function () {
		if($(document).scrollTop() >= newBetTitleTop){
			newBetTitle.addClass('newBetFixed');
		}else{
			newBetTitle.removeClass('newBetFixed');
		}
	});


	// 倒计时
	function countdown(seconds) {
		if (seconds == '') {
			$("#updata").html('加載');
			return;
		}
		clearTimeout(timer);
		if(--seconds){
			timer = setTimeout(function () {
				countdown(seconds);
				$("#updata").html(seconds+'秒');
			}, 1000);
		}else{
			clearTimeout(timer);
			$("#updata").html('加載中');
			baseAjax();
		}
	}

	$(".chk").click(function () {
		clearTimeout(timer);
		$("#updata").html('加載中');
		oldTime = '';
		top.oldId = 0;
		$("#listWrap").html('');
		$("#autoJpWrap").html('');
		baseAjax();
	});
	var updataTimer = null;
	$("#updata").click(function () {
		clearTimeout(timer);
		clearTimeout(updataTimer);
		updataTimer = null;
		$("#updata").html('加載中');
		updataTimer = setTimeout(function  () {
			baseAjax();
		}, 1000);
	});

	$("#amount").blur(function () {
		clearTimeout(timer);
		clearTimeout(updataTimer);
		updataTimer = null;
		$("#updata").html('加載中');
		updataTimer = setTimeout(function  () {
			baseAjax();
		}, 1000);
	});

	// 加载列表
	function baseAjax() {
		var url = '';
		var postData = {};
		var aLids = [];

		// var loadHtml = '<tr id="newBetLoad"><td colspan="9"></td></tr>';

		// $("#listWrap").prepend( loadHtml );
		// 
		if (amount.val() != myAmount) {
			myAmount = amount.val();
			top.oldId = 0;
			$("#listWrap").html('');
		}

		if(masterids == '1'){
			// 六合彩参数
			url = 'Handler/Handler.ashx';
			postData = {
				'action': 'get_newbetlist',
				'top': 50,
				'amount': amount.val(),
				'oldTime': oldTime
			};
		}else{
			// 快彩参数
			url = 'Handler/QueryHandler.ashx';

			aLids = $(".chk:checked").map(function () {
				return this.value;
			}).get();

			postData = {
				'action': 'get_newbetlist',
				'top': 50,
				'amount': amount.val(),
				'lids': aLids.join(','),
				'oldTime': oldTime
			};
		}

		if(aLids.length > 0 || masterids == '1'){
			var b = new getBaseDataAjax({
				url: url,
				postData: postData,
				completeCallBack:function () {},
				successCallBack:function (d) {
					oldTime = d.data.autoJP.timestamp;
					$("#Sound").html('');
					// $("#newBetLoad").remove();
					if(d.data.hasOwnProperty('newbetlist')){
						domHtml(d);
						if($("#ClewSound:checked").length>0){
							var IE = !-[1,];
							if(IE){
								$("#Sound").append("<embed src='../images/ClewSound.swf' loop=false autostart=false mastersound hidden=true width=0 height=0></embed>");
							}else{
								$("#Sound").html('<audio controls="controls" autoplay="autoplay" style="display:none"><source src="../images/ClewSound.ogg" type="audio/ogg"><source src="../images/ClewSound.mp3" type="audio/mpeg">Your browser does not support the audio element.</audio>');
							}	
						}
					}
					if(d.data.autoJP.tipsList.length > 0){
						$("#autoJp").show();
						domJp(d.data.autoJP.tipsList);
						if($("#ClewSound:checked").length>0){
							var IE = !-[1,];
							if(IE){
								$("#Sound").append("<embed src='../images/sound_kj.swf' loop=false autostart=false mastersound hidden=true width=0 height=0></embed>");
							}else{
								$("#Sound").html('<audio controls="controls" autoplay="autoplay" style="display:none"><source src="../images/sound_kj.ogg" type="audio/ogg"><source src="../images/sound_kj.mp3" type="audio/mpeg">Your browser does not support the audio element.</audio>');
							}
						}
					}
					countdown($("#sltSeconds").val());
				},
				errorCallBack:function () {

				}
			});
		}

	}

	// DOM实时滚单列表
	function domHtml (d) {
		var html = '';
		var listData = d.data.newbetlist;
		var p = window.parent;
		var i = 0;
		for(var key in listData){
			var oListData = listData[key];
			var wtHtml = '';
			var zfHtml = '';
			var dHtml = '';
			var tHtml = '';
			// 处理微调
			if(oListData['islm'] == '1'){
				// 是连码
				if(lottoryType == 'six'){
					wtHtml = oListData['betval'] + oListData['betwt'];
				}else{
					wtHtml = oListData['betval'];
				}
			}else{
				// 非连码
				wtHtml = '';
			}
			if(oListData['saletype'] == '1'){
				// 标识走飞单
				zfHtml = '<br><span class="red">走飛</span>';
			}else if(oListData['saletype'] == '2'){
				// 外補
				zfHtml = '<br><span class="red">外補</span>';
			}else{
				// 非走飞单
				zfHtml = '';
			}
			var category = '';
			//if (oListData['category'] != null && oListData['category'] != '') {
			//    category = '<span class="black">' + oListData['category'] + '</span><br>';
			//}

			// 处理双赔率
			if (oListData['playname'].indexOf(',') > 0) {
				var name = oListData['playname'].split(',');
				var pl = oListData['odds'].split(',');
				var name1 = '';
				if(name[1] == '連羊'){
					name1 = '不連羊';
				}else if(name[1] == '連0'){
					name1 = '不連0';
				}else{
					name1 = name[0];
				}
				var plHtml = '<br>'+ name1 +'@<span class="red">'+ pl[0] +'</span><br>'+ name[1] +'@<span class="red">'+ pl[1] +'</span>';
				dHtml = category + '<span class="blue">' + name[0] + '</span>' + plHtml;
			}else{
			    dHtml = category + '<span class="blue">' + oListData['playname'] + '</span>@<span class="red">' + oListData['odds'] + '</span>';
			}

			if(oListData.hasOwnProperty('amounttext')){
				tHtml = oListData['amounttext'];
			}else{
				tHtml = Math.floor(oListData['amount']);
			}

			i++;
			var className = '';
			if(i%2){
				className = 'eachColor';
			}
            var cName = '';
            if(oListData['isdelete'] == '1'){
                cName = 'delline';
            }

            var tabletypeHtml = '';
            if(oListData.hasOwnProperty('tabletype')){
            	tabletypeHtml = oListData.tabletype
            }

			html = '<tr class="'+ className +' '+ cName +'">'+
						'<td width="16%"><span class="green">'+ oListData['lottery_name'] +'</span> #'+ oListData['ordernum'] +'<br>'+ oListData['bettime'] + '<span> ' + oListData['week'] +'</span></td>'+
						'<td width="10%">'+ oListData['hyname'] +'('+ oListData['pk'] +'盤)'+ zfHtml +'</td>'+
						'<td width="10%">'+ oListData['dlname'] +'<br>'+ oListData['dlrate'] +'%</td>'+
						'<td width="10%">'+ oListData['zdname'] +'<br>'+ oListData['zdrate'] +'%</td>'+
						'<td width="10%">'+ oListData['gdname'] +'<br>'+ oListData['gdrate'] +'%</td>'+
						'<td width="10%">'+ oListData['fgsname'] +'<br>'+ oListData['fgsrate'] +'%</td>'+
						'<td width="10%">'+ oListData['zjrate'] +'%<br>'+ oListData['zjdrawback'] +'</td>'+
						'<td width="14%">'+ '<span class="green">第' + oListData['phase'] + '期</span>'+ tabletypeHtml +'<br>' + dHtml + wtHtml + '</td>'+
						'<td width="10%">'+ tHtml +'<br><span class="red">'+ Math.floor(oListData['szamount']) +'</span></td>'+
					'</tr>' + html;
		}
		$("#listWrap").prepend( html );
		// 删掉多出 50 组数据的值
		$("#listWrap tr:gt(49)").remove();
	}

	// DOM自动降赔列表
	function domJp(list) {
		var listHtml = '';
		for(var i=list.length-1; i>=0; i--){
		    var title = '';
		    var category = '';
		    //if (list[i]['category'] != null && list[i]['category'] != '')
		    //{
		    //    category = '【' + list[i]['category'] + '】';
		    //}
			if(list[i]['play_name'] == list[i]['put_val']){
				title = list[i]['play_name'];
			}else{
				title = list[i]['play_name'] + '『' + list[i]['put_val'] + '』';
			}
			listHtml = '<tr>'+
							'<td align="right">'+ list[i]['lottery_name'] + '<span class="green">第' + list[i]['phase'] + '期</span>' +'&nbsp&nbsp</td>'+
							'<td align="left">&nbsp&nbsp' + category + title + ' 賠率由<span class="green">原賠率：</span><span class="red">@'+ list[i]['old_odds'] + '</span> 下降至<span class="green">新賠率：</span><span class="red">@'+ list[i]['new_odds'] + '</span> （賠率下降:<span class="red">'+ list[i]['odds'] + '</span>）&nbsp&nbsp變動時間:' + list[i]['add_time'] +'</td>'+
						'</tr>' + listHtml;
		}

		$("#autoJpWrap").prepend( listHtml );
		// 删掉多出 5 组数据的值
		$("#autoJpWrap tr:gt(4)").remove();
	}

});
