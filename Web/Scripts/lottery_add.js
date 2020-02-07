define(function (require, exports, module) {
    var $ = require('jquery');
    require('myLayer')($);
    require('wdatePicker');
 
 
    function addForm() {
        if ($("#txtPhase").val() == '') {
			$("body").myLayer({
				isMiddle: true,
				isShowBtn: true,
				content: "“本期編號”請務必輸入!!",
				okText:'确定',
				okCallBack: function () {
					$.myLayer.close(true);
					$("#txtPhase").focus();
				},
				closeCallBack: function () {
					$("#txtPhase").focus();
				}
			});
            return false;
        }
        if ($("#txtOpenDate").val() == '') {
			$("body").myLayer({
				isMiddle: true,
				isShowBtn: true,
				content: "“開獎日期”請務必輸入!!",
				okText:'确定',
				okCallBack: function () {
					$.myLayer.close(true);
					$("#txtOpenDate").focus();
				},
				closeCallBack: function () {
					$("#txtOpenDate").focus();
				}
			});
            return false;
        }
        if (isTime($("#txtOpenTime")) == false) {
            return false;
        }
        if (isTime($("#txtEndTime")) == false) {
            return false;
        }
        if (isTime($("#txtTMEndTime")) == false) {
            return false;
        }
        var opendate = $("#txtOpenDate").val();
        $("#txtEndDate").val(opendate);
        $("#txtTMEndDate").val(opendate);
 

 
		$("body").myLayer({
			isMiddle: true,
			isShowBtn: true,
			content: "是否確定寫入本獎期嗎?",
			okCallBack: function () {
		        $.ajax({
		            type: 'POST',
		            url: "?",
		            data: $('#subform').serialize(), // 你的formid 
		            error: function () {
		                alert('处理程序出错,请通知管理员检查！');
		            },
		            success: function (msg) {
		                //$('#Top_all').hideLoading();
		                $("#alert_show").html(msg);
		            }
		        });
				$.myLayer.close(true);
			}
		});
    }
 
    $("#btnSubmit").click(function () {
        addForm();
    });
 
    $("#txtOpenDate").bind('blur', function () {
        $("#txtEndDate").val($("#txtOpenDate").val());
        $("#txtTMEndDate").val($("#txtOpenDate").val());
    });
 
 
    $(".hhmmss").click(function () {
        var that = $(this);
        WdatePicker({
            skin: top.skinPath,
            dateFmt: 'HH:mm:ss',
            onpicked: function () {}
        });
    });
 
    //驗證時間格式
 
    function isTime(obj) {
		var str = obj.val();
        var a = str.match(/^(\d{1,2})(:)?(\d{1,2})\2(\d{1,2})$/);
        if (a == null) {		
			obj.myLayer({
				isMiddle: true,
				isShowBtn: true,
				content: "時間格式不正確",
				okText:'确定',
				okCallBack: function () {
					$.myLayer.close(true);
					obj.focus();
				},
				closeCallBack: function () {
					obj.focus();
				}
			});
            return false;
        }
        if (a[1] > 24 || a[3] > 60 || a[4] > 60) {		
			obj.myLayer({
				isMiddle: true,
				isShowBtn: true,
				content: "時間格式不正確",
				okText:'确定',
				okCallBack: function () {
					$.myLayer.close(true);
					obj.focus();
				},
				closeCallBack: function () {
					obj.focus();
				}
			});
            return false;
        }
        return true;
    }
 
 
    function setUrl(src, str) {
        var a = src.split("/");
        a[a.length - 2] = str;
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