<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="药价中台.login" %>

<!DOCTYPE html>
<html class="fly-html-layui fly-html-store">
<head>
    <title>登录账号</title>
    <meta charset="utf-8">
    <meta name="renderer" content="webkit">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="referrer" content="never">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">

    <link rel="shortcut icon" href="/favicon.ico" type="image/x-icon" />
    <link rel="stylesheet" href="./css/font.css">
    <link rel="stylesheet" href="./css/xadmin.css">
    <script src="./js/md5.min_1.js"></script>
    <script type="text/javascript" src="https://cdn.bootcss.com/jquery/3.2.1/jquery.min.js"></script>
    <script src="./lib/layui/layui.js" charset="utf-8"></script>
    <script type="text/javascript" src="./js/xadmin.js"></script>
    <script type="text/javascript" src="./js/function.js"></script>
</head>
<body class="login-bg">
    <form id="form1" runat="server">
    <div class="login">
        <div class="message">中台登录</div>
        <div id="darkbannerwrap"></div>
        <input name="username" placeholder="手机号" type="text" value="" lay-verify="required" class="layui-input">
        <hr class="hr15">
        <input name="password"  lay-verify="required" placeholder="密码" type="password" class="layui-input">
        <hr class="hr15">
        <input value="登 录"  lay-filter="login" style="width:100%;" type="submit" id="btn-login">
        <hr class="hr20">
       
    </div>
   </form>



    <!-- 底部结束 -->

</body>
</html>