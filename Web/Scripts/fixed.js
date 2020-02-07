define(function(require,exports,moudles){

	var $ = require('jquery');
	var p = window.parent;

	var fixed = function () {
		setTimeout(function () {
			var myLayerContent = $("#myLayer_"+ top.myLayerIndex + " .myLayerContent", parent.document);
			var myLayerIframe = $("#myLayer_"+ top.myLayerIndex + " .myLayerIframe", parent.document);
			var pagerWrap = $("#pagerWrap");
			var pageLayerHeader = $("#pageLayerHeader");
			var h = 0;
			if(myLayerContent.height() < myLayerIframe.height()){
				h = 20;
			}else{
				h = 30;
			}

			pagerWrap.css({
				top: myLayerContent.height() - 30
			});

			myLayerContent.unbind('scroll').bind('scroll', function () {
				var mt = myLayerContent.scrollTop();
				pagerWrap.css({
					top: myLayerContent.height() + mt - 30
				});

				pageLayerHeader.css({
					top: mt ? mt - 10 : mt
				});
			});
		}, 10);
	};

	moudles.exports = fixed;
});
