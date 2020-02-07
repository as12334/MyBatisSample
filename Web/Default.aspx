<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Agent.Web.LoginUserControl.Login9.LoginControl" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>

<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>用戶登錄</title>
<meta name="renderer" content="webkit">
<meta http-equiv="X-UA-Compatible" content="IE=EDGE">
<link href="favicon.ico" rel="shortcut icon">
<script>
var isDisplayCode =  false;
var jsver=20170128;
var myLayerIndex = '19841011';
var myLayerIndexArr = [];
</script>
<script src="Scripts/sea.js" type="text/javascript"></script>
<script src="Scripts/otherConfig.js" type="text/javascript"></script>
<style>
	.btnD{
		background: #ccc!important;
		color: #999!important;
	}
</style>
</head>

<body>

<link href="/LoginUserControl/Login9/Styles/userlogin.css" rel="stylesheet" type="text/css" />
<!--[if IE 6]>
<script src="Styles/DD_belatedPNG_0.0.8a.js"></script>
<script>
  DD_belatedPNG.fix('.AL_t01,.AL_t02,.AL_t03,.AL_t02 .right .btn2,.btn2,.AL_t02 .right .btn2:hover,.btn2:hover');
</script>
<![endif]-->



<div class="AL_box">
     <form id="login">
	<div class="AL_t01"></div>
	<div class="AL_t02">
     <div class="right">
			<ul>
			<li class="nameBg">
             <i><img src="/LoginUserControl/Login9/Images/user.png"></i>
              <input  data-type="text" type="text" value="" id="loginName" name="loginName" tabindex="1" class="text" placeholder="請輸入您的用戶名" /></li>
			  <li class="passBg">
               <i><img src="/LoginUserControl/Login9/Images/lock.png"></i>
              <input  data-type="text" type="password" value="" tabindex="2" id="loginPwd" name="loginPwd" class="text" placeholder="請輸入您的密碼" /></li>
			  <li class="Bg"  style="display:none;position: relative;">
               <i style="top: 7px; left: 7px;"><img src="/LoginUserControl/Login9/Images/YZM.png"></i>
              <input  data-type="text" type="text" id="pic_input" name="ValidateCode"  tabindex="3" maxlength="4" class="w100 text" placeholder="請輸入驗證碼" /><span style="top: 5px;right: 10px;"><img id="pic_code" style="height:30px; width:100px;  margin-top:-3px; cursor:pointer;display:none;" title="图片看不清？点击重新得到验证码" /></span></li>
			</ul>
			<label>
		  <button id="login_btn" type="button" value="Login" class="loginBtn" >登 錄</button>
		  </label>
		  
	  </div>
	  <div class="copy">版權所有   2019  All  Rights  Reserved</div>
	<div class="clear"></div>    
   </div>
	<div class="AL_t03">
    </div>
    </form>
<div class="clear"></div>
</div>


<script>
    window.onload = function () {
        seajs.use('login');
    };
</script>
</body>
</html>
