
<?php
header("Content-type:text/html;charset=UTF-8");
date_default_timezone_set('PRC'); //设置中国时区 
require "mysql.php";            //导入mysql.php访问数据库
if(isset($_GET['username']))
{
	$username=$_GET['username'];
}

if(isset($_GET['xm_code']))
{
	$xm_code=$_GET['xm_code'];
}
if(isset($_GET['xm_name']))
{
    $xm_name=$_GET['xm_name'];
}
if(isset($_GET['method']))
{
	$method=$_GET['method'];
}
if(isset($_GET['shanghuname']))
{
    $shanghuname=$_GET['shanghuname'];
}

if(isset($_GET['code']))
{
    $code=$_GET['code'];
}

if(isset($_GET['usertype']))
{
    $usertype=$_GET['usertype'];
}
if(isset($_GET['method']))
{
    $method=$_GET['method'];
}


//$time=time();
/*
 * 首先进行判空操作，通过后进行验证码验证，通过后再进行数据库验证。
 * 手机号码和邮箱验证可根据需要自行添加
 * */
		
			if($method=='getcode')
			{				
				chaxun_code($username,$code,$xm_code);		
			}
			
			if($method=='addcode')
			{				
				addcode($username,$code,$xm_code,$usertype);
			}
			
		



//方法：添加
function addcode($username,$code,$xm_code,$usertype){

    $conn=new Mysql();

     $sql2="select * from users where username= '{$username}';";
    $result=$conn->sql($sql2);
    if($result){
     echo '{"status":"0","msg":"注册失败，账号已存在"}';
     return;
    }
    $sql="insert into codes (username,password,shanghuname,usertype) values('{$username}','{$password}','{$shanghuname}','{$usertype}');";
    $result=$conn->sql($sql);
    if($result){
        echo '{"status":"1","msg":"添加成功"}';
		
    }
    else{
		echo '{"status":"0","msg":"添加失败"}';
    }
    $conn->close();
}
 
 
 //查询
function chaxun_code($username,$code,$xm_code){

$sql="select * from codes where code= '{$code}';";
    $conn=new Mysql();
if($xm_code!="")
{
$sql="select * from codes where xm_code= '{$xm_code}';";
}
if($username!="")
{
$sql="select * from codes where username= '{$username}';";
}
    

    $result=$conn->sql($sql);
    if($result){
       $results = array();
    while ($row = mysqli_fetch_assoc($result)) 
         {
            $results[] = $row;
        }
        echo $str=json_encode($results);
    }
    else{
        echo '{"status":"0","msg":"code不存在"}'.$sql;
    }
    $conn->close();
}
 
 
 function getusers($usertype,$page){

     $conn=new Mysql();

    $sql="select * from users;";
    $result=$conn->sql($sql);
    if($result){
       $results = array();
    while ($row = mysqli_fetch_assoc($result)) 
         {
            $results[] = $row;
        }
echo $str=json_encode($results);
    }
    else{
        echo '{"status":"0","msg":"获取失败"}';
    }
    $conn->close();
}
 
 
 
  function deluser($userid){

     $conn=new Mysql();

    $sql="delete from users where userid='{$userid}';";
    $result=$conn->sql($sql);
  
    if($result){
        echo '{"status":"1","msg":"删除成功"}';
        
    }
    else{
        echo '{"status":"0","msg":"删除失败"}';
    }
    $conn->close();
}
 

?>
