<?php

function getcookie($url)
{
	$curl = curl_init(); // 启动一个CURL会话
	curl_setopt($curl, CURLOPT_URL, $url); // 要访问的地址
	curl_setopt($curl, CURLOPT_SSL_VERIFYPEER, FALSE); // 对认证证书来源的检查
	curl_setopt($curl, CURLOPT_SSL_VERIFYHOST, FALSE); // 从证书中检查SSL加密算法是否存在
	curl_setopt($curl, CURLOPT_USERAGENT, 'Mozilla/5.0 (compatible; MSIE 5.01; Windows NT 5.0)'); // 模拟用户使用的浏览器
	curl_setopt($curl, CURLOPT_FOLLOWLOCATION, 1); // 使用自动跳转
	curl_setopt($curl, CURLOPT_AUTOREFERER, 1);    // 自动设置Referer
	curl_setopt($curl, CURLOPT_POST, 1);             // 发送一个常规的Post请求
	//curl_setopt($curl, CURLOPT_POSTFIELDS, $data);   // Post提交的数据包x
	curl_setopt($curl, CURLOPT_TIMEOUT, 30);         // 设置超时限制 防止死循环
	curl_setopt($curl, CURLOPT_HEADER, 1);           // 显示返回的Header区域内容
	curl_setopt($curl, CURLOPT_RETURNTRANSFER, 1);   // 获取的信息以文件流的形式返回
	$tmpInfo = curl_exec($curl); // 执行操作
	if(curl_errno($curl))
{
    echo 'Errno'.curl_error($curl);//捕抓异常
}
	curl_close($curl); // 关闭CURL会话4
//获取Set-Cookie
	preg_match_all('|Set-Cookie: (.*);|U', $tmpInfo, $arr);
	$cookies = implode(';', $arr[1]);
	//echo($cookies);
	//return $tmpInfo ; // 返回数据
      return $cookies;    

}


function getdata($wangwang,$cookies)
{
	header('Access-Control-Allow-Origin:*');
	header('Content-Type:application/json; charset=utf-8');
	$url="http://106.12.189.59/app/superscanPH/opQuery.jsp?m=queryAliim&aliim=".$wangwang;
	$curl = curl_init();              
	curl_setopt($curl, CURLOPT_URL, $url);
    curl_setopt($curl, CURLOPT_HEADER, 1);
    curl_setopt($curl, CURLOPT_TIMEOUT, 2);
    curl_setopt($curl, CURLOPT_RETURNTRANSFER, 1);
	curl_setopt($curl, CURLOPT_SSL_VERIFYHOST, 0);
    curl_setopt($curl, CURLOPT_SSL_VERIFYPEER, 0);
	curl_setopt($curl , CURLOPT_COOKIE , $cookies);
    $result = curl_exec($curl);
    curl_close($curl);
	//ob_clean();
    //echo json_encode($result,JSON_UNESCAPED_UNICODE);
    //die; 
	/* if(strpos($result,'正常') ==true)
	{
		return $result;
	} */
	echo $result;
	
}


$wangwang=$_GET["wangwang"];
$cookies=getcookie("http://106.12.189.59/app/superscanPH/loginPH.jsp?m=login&username=18588777745&password=MUSHANG123&parcame=ajax");
$result= getdata(urlencode($wangwang),$cookies);


?>