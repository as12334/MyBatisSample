define(function (require, exports, module) {
	var $  = require('jquery');
	require('plus')($);

	var sub = require('sub');

	var oBody = $("body");
	oBody.onlyNumber({
		className: '.zfNumber',
		isDecimal: false,
		isMinus: false
	});


	function init() {
		function keydownFn(e) {
			if(e.which===13){
				e.preventDefault();
			}
		}
		$("input").on('keydown', function (e) {
			keydownFn(e);
		});
	}
	// init();
	
	// $("#myLayer_"+ top.myLayerIndex + " .myLayerTitle", window.parent.document).find('h3').html('盈利回收');

	$(".toRMB").keyup(function () {
	    // try{
	    $(this).siblings('.toRMBspan').html(changeMoneyToChinese($(this).val()));
	    // window.parent.$.myLayer.setSize(true, true);
	    // 重置对话框
	    sub.dialogReset();
	    // }catch(e){};
	});

	$("#btnSubmit").click(function () {
		$("#form1").submit();
	});


	$("#form1").submit(function () {
		var six_hsed = $("#six_hsed");
		var kc_hsed = $("#kc_hsed");
		if (six_hsed.val() == '') {
			six_hsed.myxTips({
				content: '當前項不能為空！'
			});
			return;
		}
		if (kc_hsed.val() == '') {
			kc_hsed.myxTips({
				content: '當前項不能為空！'
			});
			return;
		}

		sub.setIframeLoading();
		$.ajax({
			type: 'POST',
			url: "?",
			data: $('#form1').serialize(),
			error: function () { alert('处理程序出错,请通知管理员检查！'); },
			success: function (msg) {
				sub.removeAjaxLoading();
				$("#alert_show").html(msg);
			}
		});
		return false;
	});


	$("#closeBtn").click(function () {
		top.dialog.get(window).close().remove();
	});


	function changeMoneyToChinese(money) {
	    var cnNums = new Array("零", "一", "二", "三", "四", "五", "六", "七", "八", "九"); //汉字的数字
	    var cnIntRadice = new Array("", "十", "百", "千"); //基本单位
	    var cnIntUnits = new Array("", "万", "亿", "兆"); //对应整数部分扩展单位
	    var cnDecUnits = new Array("角", "分", "毫", "厘"); //对应小数部分单位
	    var cnInteger = "整"; //整数金额时后面跟的字符
	    var cnIntLast = "元"; //整型完以后的单位
	    var maxNum = 99999999999999.9999; //最大处理的数字

	    var IntegerNum; //金额整数部分
	    var DecimalNum; //金额小数部分
	    var ChineseStr = ""; //输出的中文金额字符串
	    var parts; //分离金额后用的数组，预定义
	    if (money == "") {
	        return "";
	    }
	    money = parseFloat(money);
	    if (money == 0) {
	        ChineseStr = cnNums[0] + cnIntLast + cnInteger;
	        return ChineseStr;
	    }
	    money = money.toString(); //转换为字符串
	    if (money.indexOf(".") == -1) {
	        IntegerNum = money;
	        DecimalNum = '';
	    } else {
	        parts = money.split(".");
	        IntegerNum = parts[0];
	        DecimalNum = parts[1].substr(0, 4);
	    }
	    if (parseInt(IntegerNum, 10) > 0) {//获取整型部分转换
	        zeroCount = 0;
	        IntLen = IntegerNum.length;
	        for (i = 0; i < IntLen; i++) {
	            n = IntegerNum.substr(i, 1);
	            p = IntLen - i - 1;
	            q = p / 4;
	            m = p % 4;
	            if (n == "0") {
	                zeroCount++;
	            } else {
	                if (zeroCount > 0) {
	                    ChineseStr += cnNums[0];
	                }
	                zeroCount = 0; //归零
	                ChineseStr += cnNums[parseInt(n)] + cnIntRadice[m];
	            }
	            if (m == 0 && zeroCount < 4) {
	                ChineseStr += cnIntUnits[q];
	            }
	        }
	        ChineseStr += cnIntLast;
	        //整型部分处理完毕
	    }
	    if (DecimalNum != '') {//小数部分
	        decLen = DecimalNum.length;
	        for (i = 0; i < decLen; i++) {
	            n = DecimalNum.substr(i, 1);
	            if (n != '0') {
	                ChineseStr += cnNums[Number(n)] + cnDecUnits[i];
	            }
	        }
	    }
	    if (ChineseStr == '') {
	        ChineseStr += cnNums[0] + cnIntLast + cnInteger;
	    } else if (DecimalNum == '') {
	        ChineseStr += cnInteger;
	    }
	    if (money >= maxNum) {
	        ChineseStr = '對不起，金額超出可轉換的範圍';
	    }
	    return ChineseStr;
	}


	function To_RMB(whole) {
	    //分离整数与小数
	    var num;
	    var dig;
	    if (whole.indexOf(".") == -1) {
	        num = whole;
	        dig = "";
	    } else {
	        num = whole.substr(0, whole.indexOf("."));
	        dig = whole.substr(whole.indexOf(".") + 1, whole.length);
	    }

	    //转换整数部分
	    var i = 1;
	    var len = num.length;

	    var dw2 = new Array("", "万", "亿"); //大单位
	    var dw1 = new Array("十", "百", "千"); //小单位
	    var dw = new Array("", "一", "二", "三", "四", "五", "六", "七", "八", "九"); //整数部分用
	    var k1 = 0; //计小单位
	    var k2 = 0; //计大单位
	    var str = "";

	    for (i = 1; i <= len; i++) {
	        var n = num.charAt(len - i);
	        if (n == "0") {
	            if (k1 != 0)
	                str = str.substr(1, str.length - 1);
	        }

	        str = dw[Number(n)].concat(str); //加数字

	        if (len - i - 1 >= 0) {
	            if (k1 != 3) {
	                try {
	                    str = dw1[k1].concat(str);
	                } catch (e) { }
	                k1++;
	            } else {
	                k1 = 0;
	                var temp = str.charAt(0);
	                if (temp == "万" || temp == "亿") //若大单位前没有数字则舍去大单位
	                    str = str.substr(1, str.length - 1);
	                try {
	                    str = dw2[k2].concat(str);
	                } catch (e) { }
	            }
	        }

	        if (k1 == 3) {
	            k2++;
	        }

	    }
	    if (str.length >= 2) {
	        if (str.substr(0, 2) == "一十") str = str.substr(1, str.length - 1);
	    }
	    return str;
	}

});