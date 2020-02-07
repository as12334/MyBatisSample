define(function (require, exports, module) {
	var $ = require('jquery');
	require('myLayer')($);
	// 引入jquery插件
	require('plus')($);
	// 引入封装好的Ajax
	var getBaseDataAjax = require('getBaseDataAjax');

	// console.log(window.location.pathname)
	(function () {
		var wl = window.location.pathname;
		var menu = $(".navListBox a", top.document);
		menu.removeClass('onBtn');
		menu.each(function () {
			if (wl.indexOf($(this).attr('data-iframe')) != -1) {
				$(this).addClass('onBtn');
			}
		});
	})();

	$(document).ready(function () {
		$('body').append('<div id="myWarp"></div>');
		// 非子账号
		$(".status").each(function () {
			var that = $(this);
			var status = that.attr('status');
			if(status == '0'){
				that.html("啟用");
			}else if(status == '1'){
				that.html('凍結');
			}else{
				that.html('停用');
			}
		});
		$(".status").unbind('click').bind('click', function () {
			var that = $(this);
			var html = '<div class="userStatus">'+
							'<p>修改“<span>'+ that.attr('data-name') +'</span>”帳戶狀態</p>'+
							'<div>'+
								'<label>'+
									'<input type="radio" value="0" class="setUserStatus" name="setUserStatus">'+
									'<span>啟用</span>'+
								'</label>'+
								'<label>'+
									'<input type="radio" value="1" class="setUserStatus" name="setUserStatus">'+
									'<span>凍結</span>'+
								'</label>'+
								'<label>'+
									'<input type="radio" value="2" class="setUserStatus" name="setUserStatus">'+
									'<span>停用</span>'+
								'</label>'+
							'</div>'+
						'</div>';

			$(this).myLayer({
				title: '帳戶狀態',
				content: html,
				isMiddle: true,
				okText: '確認修改',
				okCallBack: function (obj) {
					obj.find(".myLayerLoading").show();
					$.ajax({
						url: 'ExistNameAjax.aspx',
						type: 'POST',
						cache: false,
						dataType: 'json',
						timeout: 5000,
						async: false,
						data:{
							action: 'updstatus',
							uid: that.attr('uid'),
							status: $(".setUserStatus:checked").val()
						},
						success:function (d) {
							$.myLayer.close(true);
							location.replace(curUrl);
						},
						error:function () {

						}
					});
				}
			});

			$(".setUserStatus[value="+ that.attr('status') +"]").prop('checked', true);
		});

		// 子帐号
		$(".childstatus").each(function () {
			var that = $(this);
			var status = that.attr('status');
			if(status == '0'){
				that.html("啟用");
			}else if(status == '1'){
				that.html('凍結');
			}else{
				that.html('停用');
			}
		});
		$(".childstatus").unbind('click').bind('click', function () {
			var that = $(this);
			var html = '<div class="userStatus">'+
							'<p>修改“<span>'+ that.attr('data-name') +'</span>”帳戶狀態</p>'+
							'<div>'+
								'<label>'+
									'<input type="radio" value="0" class="setUserStatus" name="setUserStatus">'+
									'<span>啟用</span>'+
								'</label>'+
								'<label>'+
									'<input type="radio" value="1" class="setUserStatus" name="setUserStatus">'+
									'<span>凍結</span>'+
								'</label>'+
								'<label>'+
									'<input type="radio" value="2" class="setUserStatus" name="setUserStatus">'+
									'<span>停用</span>'+
								'</label>'+
							'</div>'+
						'</div>';

			$(this).myLayer({
				title: '帳戶狀態',
				content: html,
				isMiddle: true,
				okText: '確認修改',
				okCallBack: function (obj) {
					obj.find(".myLayerLoading").show();
					$.ajax({
						url: 'ExistNameAjax.aspx',
						type: 'POST',
						cache: false,
						dataType: 'json',
						timeout: 5000,
						async: false,
						data:{
							action: 'updstatuschild',
							uid: that.attr('uid'),
							status: $(".setUserStatus:checked").val()
						},
						success:function (d) {
							$.myLayer.close(true);
							location.replace(curUrl);
						},
						error:function () {}
					});
				}
			});

			$(".setUserStatus[value="+ that.attr('status') +"]").prop('checked', true);
		});

		// 出货会员
		$('.delFillUser').click(function () {
			var that = $(this);
			that.myLayer({
				title: '刪除',
				content: '您確定要刪除賬號“'+ that.attr('data-name') +'”',
				isMiddle: true,
				okText: '確定',
				cancelText: '取消',
				okCallBack: function () {
					location.href = "filluser_list.aspx?uid=" + that.attr('uid') + "&isdel=1";
					$.myLayer.close(true);
				}
			});
		});

		// 修改事件绑定
		$('.editBtn').click(function () {
			if ($(this).html() == '記錄') {
				$(this).myLayer({
					title: $(this).html(),
					isMiddle: true,
					width: 0.9,
					isShowBtn: false,
					closeCallBack:function () {
						$.myLayer.close(true);
						// location.replace(curUrl);
					},
					url: $(this).attr('href')
				});
			}else{
				$(this).myLayer({
					title: $(this).html(),
					isMiddle: true,
					isShowBtn: false,
					closeCallBack:function () {
						$.myLayer.close(true);
						// location.replace(curUrl);
					},
					url: $(this).attr('href')
				});
			}
			return false;
		});

		// 新增事件绑定
		$("#newAddBtn, #newAddDuBtn").click(function () {
			$(this).myLayer({
				title: $(this).find('div').html(),
				isMiddle: true,
				isShowBtn: false,
				closeCallBack:function () {
					$.myLayer.close(true);
					if (isReLoad) {
						location.replace(curUrl);
					}
				},
				url: $(this).attr('href')
			});
			return false;
		});
		var timer = null;
		var typeObj = {
			'zj' : "總監",
			'fgs': "分公司",
			'gd' : "股东",
			'zd': "總代理",
			'dl' : "代理"
		};
		var tipsData = {};
		$(".mytip").hover(function () {
			var that = $(this);
			var userName = that.attr('user-name');
			clearTimeout(timer);
			timer = null;
			if (!tipsData.hasOwnProperty(userName)) {
				timer = setTimeout(function  () {
					new getBaseDataAjax({
						url: '/Handler/QueryHandler.ashx',
						postData: {
							action: 'get_user_rate',
							uid: that.attr('uid')
						},
						async: false,
						completeCallBack:function () {},
						successCallBack:function (d) {
							tipsData[userName] = d.data.tips;
							setTipsFun(that, userName, d.data.tips);
						},
						errorCallBack:function () {}
					});
				}, 500);
			}else{
				setTipsFun(that, userName, tipsData[userName]);
			}
		},function () {
			clearTimeout(timer);
			timer = null;
			$('#myxTips').remove();
		});

		function setTipsFun (obj, userName, tipsData) {
			var inputVal = $("input[name=rbnLottery]:checked").val();
			var sixHtml = '', kcHtml = '', tipsHtml = '', zhHtml = '', userListTitleHtml = '', userListTitleSixHtml = '', userListTitleKcHtml = '';

			if (tipsData.hasOwnProperty('six')) {
				var sh = '';
				zhHtml = '';
				for(var key in tipsData.six){
					zhHtml += '<tr><td align="right">' + typeObj[key] + '</td><td class="red">' + tipsData.six[key].split(',')[0] + '</td></tr>';
					sh += '<tr><td>成数</td><td class="green">' + tipsData.six[key].split(',')[1] + '%</td></tr>';
				}
				sixHtml = '<td><table class="userListTipsTable">'+ sh +'</table></td>';
				userListTitleSixHtml = '<th>香港⑥合彩</th>';
			}
			if (tipsData.hasOwnProperty('kc')) {
				var kh = '';
				zhHtml = '';
				for(var key in tipsData.kc){
					zhHtml += '<tr><td align="right">' + typeObj[key] + '</td><td class="red">' + tipsData.kc[key].split(',')[0] + '</td></tr>';
					kh += '<tr><td>成数</td><td class="green">' + tipsData.kc[key].split(',')[1] + '%</td></tr>';
				}
				kcHtml = '<td><table class="userListTipsTable">'+ kh +'</table></td>';
				userListTitleKcHtml = '<th>快彩</th>';
			}

			if (inputVal == 'six') {
				tipsHtml = sixHtml;
				userListTitleHtml = userListTitleSixHtml;
			}else if(inputVal == 'kc'){
				tipsHtml = kcHtml;
				userListTitleHtml = userListTitleKcHtml;
			}else{
				tipsHtml = sixHtml+kcHtml;
				userListTitleHtml = userListTitleSixHtml + userListTitleKcHtml;
			}
			obj.myxTips({
				content: '<h2 class="reportListTitle"><span class="red">' + userName +'</span> 上级占成情况明细</h2><div class="userListTipsWrap base-clear"><table><tr><th>账号</th>'
						+ userListTitleHtml +'<tr><td><table>'+ zhHtml +'</table></td>'+ tipsHtml +'</tr></table></div>',
				ishide: false
			});
		}

		$(".setFlyAway").click(function () {
			var that = $(this);
			if (that.attr('sale') == '1') {
				that.myLayer({
					title: that.attr('title'),
					isMiddle: true,
					isShowBtn: false,
					// width: 0.95,
					closeCallBack:function () {
						$.myLayer.close(true);
						// location.replace(curUrl);
					},
					url: $(this).attr('href')
				});
			}else{
				var msg = '';
				if (that.attr('flag') == 'six') {
					msg = '香港⑥合彩';
				}else{
					msg = '快彩';
				}
				that.myLayer({
					title: that.attr('title'),
					content: "該賬號 "+ msg +" 補貨功能已經被禁用！",
					isMiddle: true,
					isShowBtn: false
				});
			}
			return false;
		});
	});


});
