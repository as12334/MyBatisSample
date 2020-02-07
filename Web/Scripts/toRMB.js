define(function(require, exports, module) {


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

        if (len - i - 1 >= 0){
            if (k1 != 3){
                str = dw1[k1].concat(str);
                k1++;
            } else {
                k1 = 0;
                var temp = str.charAt(0);
                if (temp == "万" || temp == "亿") //若大单位前没有数字则舍去大单位
                    str = str.substr(1, str.length - 1);
                str = dw2[k2].concat(str);
            }
        }

        if (k1 == 3){
            k2++;
        }

    }
    if (str.length >= 2) {
        if (str.substr(0, 2) == "一十") str = str.substr(1, str.length - 1);
    }
	return str;
}