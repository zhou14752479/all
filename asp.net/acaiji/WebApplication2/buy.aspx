<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="buy.aspx.cs" Inherits="WebApplication2.buy" %>

<!DOCTYPE html>

<html>
<head>
	<meta charset="UTF-8">
	<title>爱采集软件积分充值</title>

	<link rel="stylesheet" type="text/css" href="css/amazeui.min.css" />
	<link rel="stylesheet" type="text/css" href="css/main.css" />

</head>
<body>
	<div class="pay">
		<!--主内容开始编辑-->
		<div class="tr_recharge">
			<div class="tr_rechtext">
				<p class="te_retit"><img src="images/coin.png" alt="" />充值中心</p>
				<p>1.积分是爱采集所有软件导出数据所需的物品</p>
				<p>2.100元=10000积分，200元=30000积分，500元升级永久会员所有软件导出数据免积分</p>
			</div>
			<form   runat="server"   class="am-form" >
				<div class="tr_rechbox">
					<div class="tr_rechhead">
						<img src="images/ys_head2.jpg" />
						<p >输入充值帐号：
							<input type="text" name="username" /><br/>
                            <asp:LinkButton ID="LinkButton1" CssClass="tr_pay" runat="server" value="确定"  OnClick="btn_Click">确定</asp:LinkButton>
						</p>
						<div class="tr_rechheadcion">
							<img src="images/coin.png" alt="" />
							<span>当前用户：<span><%= username%></span></span>
						</div>
                       
					</div>
					<div class="tr_rechli am-form-group">
						<input type="hidden" name="money" class="money" value="">
						<input type="hidden" name="type" class="type">
						<ul class="ui-choose am-form-group" id="uc_01">
							<li>
								<label class="am-radio-inline">
									<input type="radio"  value="" name="docVlGender" required data-validation-message="请选择一项充值额度"> 100￥
								</label>
							</li>
							<li>
								<label class="am-radio-inline">
									<input type="radio" name="docVlGender" data-validation-message="请选择一项充值额度"> 200￥
								</label>
							</li>

							<li>
								<label class="am-radio-inline">
									<input type="radio" name="docVlGender" data-validation-message="请选择一项充值额度"> 500￥
								</label>
							</li>
							<li>
								<label class="am-radio-inline">
									<input type="radio" name="docVlGender" data-validation-message="请选择一项充值额度"> 其他金额
								</label>
							</li>
						</ul>
						
					</div>
					<div class="tr_rechcho am-form-group">
						<span>充值方式：</span>
						<label class="am-radio">
							<input type="radio" name="radio1" value="1" data-am-ucheck =required data-validation-message="请选择一种充值方式"/> <img src="images/wechatpay.png">
						</label>
						<label class="am-radio" style="margin-right:30px;">
							<input type="radio" name="radio1" value="2" data-am-ucheck =required data-validation-message="请选择一种充值方式"/><img src="images/zfbpay.png">
						</label>
					</div>
					<div class="tr_rechnum">
						<span>应付金额：</span>
						<p class="rechnum">0.00元</p>
					</div>
				</div>
				<div class="tr_paybox">
					<input  onclick="" type="button" value="确认支付" class="tr_pay am-btn" />
					<span>温馨提示：如遇到扫码支付不成功请通过底部账号转账，截图联系QQ852266010或微信17606117606。</span>
				</div>
			</form>
		</div>

	</div>
	<div class="shadow">
		
	</div>
	<div class="box">
		<img src="images/w.jpg" class="money_img">
	</div>
	<script type="text/javascript" src="js/jquery.min.js"></script>
	<script type="text/javascript" src="js/amazeui.min.js"></script>
	<script type="text/javascript" src="js/ui-choose.js"></script>
	<script type="text/javascript">
	// 将所有.ui-choose实例化
	$('.ui-choose').ui_choose();
	// uc_01 ul 单选
	var uc_01 = $('#uc_01').data('ui-choose'); // 取回已实例化的对象
	uc_01.click = function(index, item) {
		console.log('click', index, item.text())
	}
	uc_01.change = function(index, item) {
		console.log('change', index, item.text())
	}
	$(function() {
		$('.tr_rechcho input').change(function(){
			$('input[name=type]').val($(this).val())
		})
		$('#uc_01 li:eq(3)').click(function() {
			$('.tr_rechoth').show();
			$('.tr_rechoth').find("input").attr('required', 'true')
			$('.rechnum').text('0元');
			$('input[name=money]').val('0')
		})
		$('#uc_01 li:eq(0)').click(function() {
			$('.tr_rechoth').hide();
			$('.rechnum').text('100.00元');
			$('.othbox').val('');
			$('input[name=money]').val('1')
		})
		$('#uc_01 li:eq(1)').click(function() {
			$('.tr_rechoth').hide();
			$('.rechnum').text('200.00元');
			$('.othbox').val('');
			$('input[name=money]').val('2')
		})
		$('#uc_01 li:eq(2)').click(function() {
			$('.tr_rechoth').hide();
			$('.rechnum').text('500.00元');
			$('.othbox').val('');
			$('input[name=money]').val('5')
		})

		$('.tr_pay').click(function(){
			// alert($('input[name=type]').val())
			var money=parseInt($('input[name=money]').val())
			var type=parseInt($('input[name=type]').val())
			switch(type)
			{
				case 1:
				$('.shadow,.box').show()
				$('.money_img').attr('src','images/w'+money+'.jpg');
				break;
				case 2:
				$('.shadow,.box').show()
				$('.money_img').attr('src','images/z'+money+'.jpg');
				break;
			}
		})
		$(document).ready(function() {
			$('.othbox').on('input propertychange', function() {
				var num = $(this).val();
				$('.rechnum').html(num + ".00元");
			});
		});
	})

	$(function() {
		$('#doc-vld-msg').validator({
			onValid: function(validity) {
				$(validity.field).closest('.am-form-group').find('.am-alert').hide();
			},
			onInValid: function(validity) {
				var $field = $(validity.field);
				var $group = $field.closest('.am-form-group');
				var $alert = $group.find('.am-alert');
				// 使用自定义的提示信息 或 插件内置的提示信息
				var msg = $field.data('validationMessage') || this.getValidationMessage(validity);

				if(!$alert.length) {
					$alert = $('<div class="am-alert am-alert-danger"></div>').hide().
					appendTo($group);
				}
				$alert.html(msg).show();
			}
		});
	});
</script>

<div style="text-align:center;">	
	<p class="bot">100元=10000积分，200元=30000积分，500元升级永久会员所有软件导出数据免积分</p>
	<p>其他支付方式：</p>
    <p>支付宝账号：17606117606  支付宝用户名：周凯歌</p>
    <p>微信账号：17606117606   微信用户名：周凯歌</p>
	<p>更多软件：<a href="http://www.acaiji.com/" target="_blank">爱采集</a></p>
</div>

</body>
</html>