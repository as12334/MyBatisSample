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

    // $("#smt_submit").parents('#form1').submit(function () {
    $("#smt_submit").click(function () {
        var oInput = $("#txt_recharge");
        var v = Number(oInput.val());
        if(isNaN(v) || oInput.val() == ''){
            oInput.myxTips({
                content: '不能為空！'
            });
        }else{
            if(v<1){
                oInput.myxTips({
                    content: '充值金額必須大於"0"！'
                });
            }else{
                sub.setIframeLoading();
                $.ajax({
                    type: 'POST',
                    url: "?",
                    data: $('#form1').serialize(),
                    error: function () { alert('处理程序出错,请通知管理员检查！'); },
                    success: function (msg) {
                        sub.removeAjaxLoading();
                        // try{
                        $("#alert_show").html(msg);
                        // }catch(e){}
                        // window.parent.$.myLayer.close(true);
                        // $("#iframeTopMask", top.document).remove();
                        // window.parent.location.reload();
                    }
                });
            }
        }
    });

    // $("#withdraw_submit").parents('#form1').submit(function () {
    $("#withdraw_submit").click(function () {
        var oInput = $("#txt_withdraw");
        var v = Number(oInput.val());
        if(isNaN(v) || oInput.val() == ''){
            oInput.myxTips({
                content: '不能為空！'
            });
        }else{
            if(v<1){
                oInput.myxTips({
                    content: '取款金額必須大於"0"！'
                });
            }else if(v>Number($("#balance").html())){
                oInput.myxTips({
                    content: '余額不足！'
                });
            }else{
                sub.setIframeLoading();
                $.ajax({
                    type: 'POST',
                    url: "?",
                    data: $('#form1').serialize(),
                    error: function () { alert('处理程序出错,请通知管理员检查！'); },
                    success: function (msg) {
                        sub.removeAjaxLoading();
                        // try{
                        $("#alert_show").html(msg);
                        // }catch(e){}
                        // window.parent.$.myLayer.close(true);
                        // $("#iframeTopMask", top.document).remove();
                        // window.parent.location.reload();
                    }
                });
            }
        }
    });

    // $("#smt_submit").click(function() {
    //     $(this).parents('#form1').submit();
    // });

    // $("#withdraw_submit").click(function() {
    //     $(this).parents('#form1').submit();
    // });


    $("#txt_withdraw, #txt_recharge").keyup(function (e) {
        var key =  e.which;
        if(key == 13){
            $("#withdraw_submit").click();
            $("#smt_submit").click();
        }
    });


});
