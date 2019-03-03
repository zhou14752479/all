//客服
$(function(){
    var r = window.location.host;
    var hm = document.createElement("script");
      hm.id = 'rwxqqkefu';
      hm.charset = 'utf-8';

    /*if(r=="www.100yingxiao.cn"||r=="100yingxiao.cn"){
         hm.src="http://user.51qke.com/Public/webqqkefu/rwxqqkefu.js?id=93";
      }else{
      hm.src="http://user.51qke.com/Public/webqqkefu/rwxqqkefu.js?id=93";
      }*/
      hm.src="http://user.51qke.com/Public/webqqkefu/rwxqqkefu.js?id=93";
    var s = document.getElementsByTagName("script")[0];
    s.parentNode.insertBefore(hm,s);
});
//公司资质验证加logo图片
$(function(){
  var r = window.location.host;
    if(r=="www.baifenbai.998102.top"||r=="baifenbai.998102.top"){
       $("#webname").html("沈阳弘哲实验室设备有限公司");
       $("#webname3").html("沈阳弘哲实验室设备有限公司");
       $("#webname2").html("隶属于沈阳弘哲实验室设备有限公司");
	}else if(r=="www.baifenbai.fiax.com.cn"||r=="baifenbai.fiax.com.cn"){
       $("#webname").html("威海市日上红贸易有限公司");
       $("#webname3").html("威海市日上红贸易有限公司");
       $("#webname2").html("隶属于威海市日上红贸易有限公司");
	}else if(r=="www.baifenbai.xiaoanlei.cn"||r=="baifenbai.xiaoanlei.cn"){
       $("#webname").html("北京科学树科技有限公司");
       $("#webname3").html("北京科学树科技有限公司");
       $("#webname2").html("隶属于北京科学树科技有限公司");
	}else if(r=="www.360.51kuaituibao.com"||r=="360.51kuaituibao.com"){
       $("#webname").html("北京九林源商贸有限公司 ");
       $("#webname3").html("北京九林源商贸有限公司 ");
       $("#webname2").html("隶属于北京九林源商贸有限公司");
       $("#address").html("北京市西城区西客站南路76号楼7号");
       $("#tel").html("010-86252323");
	}else{
       $("#webname").html("百分百网络营销软件");
       $("#webname3").html("百分百网络营销软件");
       $("#webname2").html("");
       $("#address").html("北京市朝阳区北方明珠大厦1号楼");
	}
});

// 广告位
$(function (){
	$("#left-ad").attr('href', '/qqapp.html');
	$("#left-ad").children('img').attr('src', 'http://res.100public.com/bfb/imgs/bfb-ad-left.jpg');
	$("#right-ad").attr('href', '/qqapp.html');
	$("#right-ad").children('img').attr('src', 'http://res.100public.com/bfb/imgs/bfb-ad-left.jpg');
	$('.left-ad').animate({left:'0px'},1000);
	$('.right-ad').animate({right:'0px'},1000);
	$('.ad-close').click(function(){
	   $(this).parent().hide();
	});
});
























