<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="ASP姓名身份验证.WebForm1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
		<meta name="robots" content="all"/>
		<meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no"/>
		<meta name="applicable-device" content="mobile"/>
		<meta http-equiv="Cache-Control" content="no-transform"/>
		<meta name="apple-mobile-web-app-capable" content="yes"/>
		<meta name="apple-mobile-web-app-status-bar-style" content="black"/>
		<title>手机号姓名查询,手机号本人验证,本人号码查询,手机号验证本人</title>
		<link rel="stylesheet" href="static/css/common.css"/>
		<link rel="stylesheet" href="static/css/index.css"/>
	</head>
	<body>
		<div class="wrapper">
			<div class="header">
				<div class="mod-head">
					<div class="side">
						<ul>
							<li><a href="/buy.html">批量查询</a></li>
				<li><a href="/contact.html">联系我们</a></li>
							<li><a href="/buy.html">使用说明</a></li>
							<li><a href="/mianze.html">免责声明</a></li>
						</ul>
					</div>
					<div class="mask"></div>
					<div class="hd">
						<a class="menu" href="javascript:;" rel="nofollow">
							<span></span><span></span><span></span>
						</a>
						<a class="logo" href="/"><img src="static/images/logo.png" width="147" height="50" alt="查询网"></a>
					</div>
					<div class="bd">
						<div class="name"><h3>手机号码+姓名匹配查询</h3></div>
						<div class="type">
							<ul>
								<li class="active"><a href="javascript:;">手机号码</a></li>
							</ul>
						</div>
						<div class="panel">
							<div class="search">
								<form  runat="server">
									<input class="input-text"  value="" type="text" id="mobile" name="mobile" size="30" class="form-text" maxlength="11"  placeholder="请输入手机号码"/>
									
									<input class="input-button"   onclick="createresult();" value="查询"/>
								</form>
							</div>
							
						</div>
					</div>
				</div>
			</div>
			<div class="container">
				<div class="module mod-panel">
					<div class="bd">
						<p><a class="btn btn-green" onclick="createresult();" rel="nofollow" target="_blank">点击查询</a></p>
					</div>
				</div>
			</div>
			
			<p style="font-size:25px;text-align:center">查询到的姓名：  <%=Application["result"]%></p>

			<div class="footer">
				<div class="mod-foot">
					<div class="bd">
						<p><a  target="_blank" href="http://wpa.qq.com/msgrd?v=3&uin=852266010&site=qq&menu=yes&from=message&isappinstalled=0"">在线咨询</a></p>
						<p>思忆网络科技 &nbsp; 版权所有 &copy; 2022</p>
						
						<p>
							
							<a href="http://www.beian.miit.gov.cn/" rel="nofollow" target="_blank">苏ICP备18031889号-1</a><br>
						</p>
					</div>
				</div>
				<div class="mod-goback">
					<a href="javascript:;" rel="nofollow">返回顶部</a>
				</div>
			</div>
		</div>
		<script type="text/javascript" src="static/js/common.js"></script>
		<script type="text/javascript" src="static/js/jsencrypt.js"></script>
		<script type="text/javascript" src="static/js/jquery.js"></script>

		<script type="text/javascript">
           
            var encrypt = new JSEncrypt();
            var publickey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC+tbQs5LDpJaqb439FGeTHdAE0NT5E5owvhlv1ZH7EpBGiyp4RUdx9z4o2jvx8OMs1mIp8QcnI/lx8w1NJp5M8AM6EerXbaJdQm420FEQQhrYZ10zqhTU+CPObXgFgIswEX7qFfTq+v3Kp2mY4OW83Lgip/jEkEJ2wjW99sm92mwIDAQAB";
            encrypt.setPublicKey(publickey);

            function RSA(str) {
                if (publickey) {
                    return encrypt.encrypt(str);
                }
                return str;
            }

			function createresult() {
                var mobile = document.getElementById('mobile').value;
                var mobile2 = RSA(mobile);
                window.location.href = "WebForm1.aspx?action=createresult&c1=" + encodeURIComponent(mobile2);
			}

        </script>
		
	</body>
</html>
