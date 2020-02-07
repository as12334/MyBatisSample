define(function(require,exports,moudles){

	var $ = require('jquery');
	var getBaseDataAjax = require('getBaseDataAjax');

	var timer = null;
	var tipsData = {};
	$(".lotcurrentphase").hover(function () {
		var that = $(this);
		var upopenphase = that.find('.currentphase').html();
		clearTimeout(timer);
		timer = null;
		
		var key = upopenphase+'_'+that.attr('lid');
		var tabletype = that.attr('tabletype');
		if(tabletype != ''){
			key = key+'_'+tabletype
		}

		if (!tipsData.hasOwnProperty(key)) {
			timer = setTimeout(function  () {
				new getBaseDataAjax({
					url: '../Handler/QueryHandler.ashx',
					postData: {
						action: 'get_currentphase',
						lid: that.attr('lid'),
						phase: upopenphase,
						tabletype: tabletype
					},
					async: false,
					completeCallBack:function () {},
					successCallBack:function (d) {
						if (d.data.jqkj.length != 0) {
							tipsData[key] = d.data.jqkj[0];
							setTipsFun(that, d.data.jqkj[0]);
						}
					},
					errorCallBack:function () {}
				});
			}, 500);
		}else{
			setTipsFun(that, tipsData[key]);
		}
	}, function () {
		clearTimeout(timer);
		timer = null;
		$("#reportTips").remove();
	});

	function setTipsFun (obj, data) {
		var oData = cloned(data);
		if(pathFolder == 'L_SIX'){
			var zm = [];
			var tm = '';
			var zmAttr = [];
			var tmStr = '';
			var szm = oData.total[0];
			zm = szm.split(',');
			tm = oData.total[1];
			tmStr = returnAnimal(zodiacData[Number(tm)]);
			for(var j=0; j<zm.length; j++){
				zmAttr.push(returnAnimal(zodiacData[Number(zm[j])]));
			}
			oData.total[0] = zmAttr.join('、');
			oData.total[1] = tmStr;
		}
		var o = oData;
		var nH = '';
		var lid = obj.attr('lid');
		for (var i = 0; i < o.draw_num.length; i++) {
			if (lid == '100' && i == 5) {
				nH += '<span class="No_'+ o.draw_num[i] +'"></span><span>+</span>';
			}else if(lid == '7'){
				if(i == 3){
					nH += '<span>=</span><span class="No_'+ o.draw_num[i] +'"></span>';
				}else if(i == 2){
					nH += '<span class="No_'+ o.draw_num[i] +'"></span>';
				}else{
					nH += '<span class="No_'+ o.draw_num[i] +'"></span><span>+</span>';
				}
			}else{
				nH += '<span class="No_'+ o.draw_num[i] +'"></span>';
			}
		}
		var iH = '';
		for (var i = 0; i < o.total.length; i++){
		    //iH += '<td>'+ o.total[i] +'</td>';
		    if (lid == '100') {
		        if (i == 0) {
		            iH += '<td colspan="8">' + o.total[i] + '</td>';
		        }
		        else {
		            iH += '<td>' + o.total[i] + '</td>';
		        }
		    }
		    else {
		        iH += '<td>' + o.total[i] + '</td>';
		    }
		}

		var bjlH = '';
		if(o.hasOwnProperty('pokerList')){
			var pokerList = o.pokerList.split(',');
			var xian_nn = o.xian_nn.split(',');
			var zhuang_nn = o.zhuang_nn.split(',');
			var pokerListHtml = '';
			var xHtml = '';
			var zHtml = '';
			for (var i = 0; i < pokerList.length; i++) {
				pokerListHtml += '<span class="poker'+ pokerList[i] +'"></span>';
			}
			bjlH += '<div>'+ pokerListHtml +'</div>';

			for (var i = 0; i < xian_nn.length; i++) {
				xHtml += '<span class="poker'+ xian_nn[i] +'"></span>';
			}
			for (var i = 0; i < zhuang_nn.length; i++) {
				zHtml += '<span class="poker'+ zhuang_nn[i] +'"></span>';
			}
			bjlH += '<div><b class="blue">閑(<span id="ps" style="width:10px">'+ o.xian_dian +'</span>):</b>'+ xHtml +'<b class="red" style="margin-left:10px">莊(<span id="ps" style="width:10px">'+ o.zhuang_dian +'</span>):</b>'+ zHtml +'</div>'
		}

		var h = '<tr>'+
					'<td>'+
					'<strong class="ball">'+
					nH+
					bjlH+
					'</strong>'+
					'</td>'+
					iH+
				'</tr>';
		var theadHtml = '';
		var pClass = '';
		if (lid == '100') {
                    //theadHtml = '<th>開出號碼</th>'+
                    //    '<th>正碼生肖</th>'+
                    //    '<th>生肖</th>'+
                    //    '<th>單雙</th>'+
                    //    '<th>大小</th>'+
                    //    '<th>尾數</th>'+
                    //    '<th>攤子</th>'+
                    //    '<th>單雙</th>'+
                    //    '<th>總和</th>'+
                    //    '<th>單雙</th>'+
		            //    '<th>大小</th>';
		            theadHtml = '<tr>' +
                    '<th colspan="8" rowspan="2">開獎號碼</th>' +
                    '<th rowspan="2" >正碼生肖</th>' +
                    '<th colspan="6">特碼</th>' +
                    '<th colspan="3">總和</th>' +
                  '</tr>' +
                  '<tr>' +
                    '<th>生肖</th>' +
                    '<th>單雙</th>' +
                    '<th>大小</th>' +
                    '<th>尾數</th>' +
                    '<th>攤子</th>' +
                    '<th>單雙</th>' +
                    '<th>總和</th>' +
                    '<th>單雙</th>' +
                    '<th>大小</th>' +
                  '</tr>';
                    pClass = 'L_SIX';
		}else if(lid == '0'){
                    theadHtml = '<th>開出號碼</th>'+
                        '<th colspan="4">總和</th>'+
                        '<th>龍虎</th>';
                     pClass = 'L_KL10';
		}else if(lid == '1'){
                    theadHtml = '<th>開出號碼</th>'+
                        '<th colspan="3">總和</th>'+
                        '<th>龍虎</th>'+
                        '<th>前三</th>'+
                        '<th>中三</th>'+
                        '<th>后三</th>';
                     pClass = 'L_CQSC';
		}else if(lid == '2'){
                    theadHtml = '<th>開出號碼</th>'+
                        '<th colspan="3">冠亞軍和</th>'+
                        '<th colspan="5">1～5龍虎</th>';
                     pClass = 'L_PK10';
		}else if(lid == '3'){
                    theadHtml = '<th>開出號碼</th>'+
                       ' <th colspan="4">總和</th>'+
                        '<th>家禽野獸</th>';
                     pClass = 'L_XYNC';
		}else if(lid == '4'){
                    theadHtml = '<th>開出號碼</th>'+
                        '<th colspan="2">總和</th>';
                     pClass = 'L_K3';
		}else if(lid == '5'){
                    theadHtml = '<th>開出號碼</th>'+
                        '<th>總和</th>'+
                        '<th>單雙</th>'+
                        '<th>大小</th>'+
                        '<th>單雙和</th>'+
                        '<th>前後和</th>'+
                        '<th>五行</th>';
                     pClass = 'L_KL8';
        } else if (lid == '6') {
                     theadHtml = '<th>開出號碼</th>' +
                        '<th colspan="3">總和</th>' +
                        '<th>龍虎</th>' +
                        '<th>前三</th>' +
                        '<th>中三</th>' +
                        '<th>后三</th>';
                     pClass = 'L_K8SC';
        }else if(lid == '7'){
					theadHtml = '<th>開出號碼</th>'+
						'<th>大小</th>'+
						'<th>單雙</th>'+
						'<th>波色</th>'+
						'<th>極大小</th>'+
						'<th>豹子</th>';
					pClass = 'L_PCDD';
		}else if(lid == '8'){
					theadHtml = '<th>開出號碼</th>'+
						'<th>莊閑和</th>'+
						'<th>大小</th>'+
						'<th>閑對</th>'+
						'<th>莊對</th>';
					pClass = 'L_PKBJL';
		}else if (lid == '9') {
            theadHtml = '<th>開出號碼</th>' +
                '<th colspan="3">冠亞軍和</th>' +
                '<th colspan="5">1～5龍虎</th>';
            pClass = 'L_XYFT5';
		}else if(lid == '10'){
	    theadHtml = '<th>開出號碼</th>'+
            '<th colspan="3">冠亞軍和</th>'+
            '<th colspan="5">1～5龍虎</th>';
	    pClass = 'L_JSCAR';
		} else if (lid == '11') {
		    theadHtml = '<th>開出號碼</th>' +
                '<th colspan="3">總和</th>' +
                '<th>龍虎</th>' +
                '<th>前三</th>' +
                '<th>中三</th>' +
                '<th>后三</th>';
		    pClass = 'L_SPEED5';
	}else if(lid == '12'){
	    theadHtml = '<th>開出號碼</th>'+
            '<th colspan="3">冠亞軍和</th>'+
            '<th colspan="5">1～5龍虎</th>';
	    pClass = 'L_JSPK10';
	}else if(lid == '13'){
	    theadHtml = '<th>開出號碼</th>'+
            '<th colspan="3">總和</th>'+
            '<th>龍虎</th>'+
            '<th>前三</th>'+
            '<th>中三</th>'+
            '<th>后三</th>';
	    pClass = 'L_JSCQSC';
	}else if(lid == '14'){
	    theadHtml = '<th>開出號碼</th>'+
            '<th colspan="4">總和</th>'+
            '<th>龍虎</th>';
	    pClass = 'L_JSSFC';
	} else if (lid == '15') {
	    theadHtml = '<th>開出號碼</th>' +
            '<th colspan="3">冠亞軍和</th>' +
            '<th colspan="5">1～5龍虎</th>';
	    pClass = 'L_JSFT2';
	}
	else if (lid == '16') {
	    theadHtml = '<th>開出號碼</th>' +
            '<th colspan="3">冠亞軍和</th>' +
            '<th colspan="5">1～5龍虎</th>';
	    pClass = 'L_CAR168';
	} else if (lid == '17') {
	    theadHtml = '<th>開出號碼</th>' +
            '<th colspan="3">總和</th>' +
            '<th>龍虎</th>' +
            '<th>前三</th>' +
            '<th>中三</th>' +
            '<th>后三</th>';
	    pClass = 'L_SSC168';
	}
	else if (lid == '18') {
	    theadHtml = '<th>開出號碼</th>' +
            '<th colspan="3">冠亞軍和</th>' +
            '<th colspan="5">1～5龍虎</th>';
	    pClass = 'L_VRCAR';
	} else if (lid == '19') {
	    theadHtml = '<th>開出號碼</th>' +
            '<th colspan="3">總和</th>' +
            '<th>龍虎</th>' +
            '<th>前三</th>' +
            '<th>中三</th>' +
            '<th>后三</th>';
	    pClass = 'L_VRSSC';
	}
	else if (lid == '20') {
	    theadHtml = '<th>開出號碼</th>' +
            '<th colspan="3">冠亞軍和</th>' +
            '<th colspan="5">1～5龍虎</th>';
	    pClass = 'L_XYFTOA';
	}
	else if (lid == '21') {
	    theadHtml = '<th>開出號碼</th>' +
            '<th colspan="3">冠亞軍和</th>' +
            '<th colspan="5">1～5龍虎</th>';
	    pClass = 'L_XYFTSG';
	}

		var tmp = '<table class="t_list">'+
					'<thead>'+
					'<tr>'+theadHtml+'</tr>'+
				'</thead>'+
				'<tbody id="historyResult" class="'+ pClass +'">'+
				h+
				'</tbody>'+
			'</table>';

		setTips(tmp, obj, lid);
	}


	function setTips(contents, obj, lid) {
		$("#reportTips").remove();
		var tmp = '<div id="reportTips">'+ contents +'</div>';
		var td = obj;
		$('body').append(tmp);
		var t = 0;
		if (obj.parent().index() == ($(".lotcurrentphase").length - 1)) {
			t = td.offset().top - $("#reportTips").height();
		}else{
			t = td.offset().top + td.height();
		}
		if(obj.parent().index() == 0){
			t = td.offset().top  + td.height();
		}
		var l = 0;
		if (lid == '5') {
			l = 10;
		}else{
			l = td.offset().left;
		}
		$("#reportTips").css({
			'position': 'absolute',
			'top': t,
			'left': l
		});
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


	function cloned(obj){
		var o,i,j,k;
		if(typeof(obj)!="object" || obj===null)return obj;
		if(obj instanceof(Array))
		{
			o=[];
			i=0;j=obj.length;
			for(;i<j;i++)
			{
				if(typeof(obj[i])=="object" && obj[i]!=null)
				{
					o[i]=arguments.callee(obj[i]);
				}
				else
				{
					o[i]=obj[i];
				}
			}
		}
		else
		{
			o={};
			for(i in obj)
			{
				if(typeof(obj[i])=="object" && obj[i]!=null)
				{
					o[i]=arguments.callee(obj[i]);
				}
				else
				{
					o[i]=obj[i];
				}
			}
		}
		return o;
	}

});