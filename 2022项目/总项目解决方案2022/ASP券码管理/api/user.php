
<?php
header("Content-type:text/html;charset=UTF-8");
date_default_timezone_set('PRC'); //设置中国时区 
require "mysql.php";            //导入mysql.php访问数据库
if(isset($_GET['username']))
{
	$username=$_GET['username'];
}

if(isset($_GET['password']))
{
	$password=$_GET['password'];
}

if(isset($_GET['method']))
{
	$method=$_GET['method'];
}

if(isset($_GET['shanghuname']))
{
    $shanghuname=$_GET['shanghuname'];
}
if(isset($_GET['page']))
{
    $page=$_GET['page'];
}
if(isset($_GET['usertype']))
{
    $usertype=$_GET['usertype'];
}
if(isset($_GET['userid']))
{
    $userid=$_GET['userid'];
}


//$time=time();
/*
 * 首先进行判空操作，通过后进行验证码验证，通过后再进行数据库验证。
 * 手机号码和邮箱验证可根据需要自行添加
 * */
		
			if($method=='register')
			{				
				register($username,$password,$shanghuname,$usertype);		
			}
			
			if($method=='login' )
			{				
				login($username,$password);		
			}
			
		if($method=='getusers' )
            {               
                getusers($usertype,$page);     
            }

	if($method=='deluser' )
            {               
                deluser($userid);     
            }



//方法：添加用户
function register($username,$password,$shanghuname,$usertype){

    $conn=new Mysql();

     $sql2="select * from users where username= '{$username}';";
    $result=$conn->sql($sql2);
    if($result){
     echo '{"status":"0","msg":"注册失败，账号已存在"}';
     return;
    }
    $sql="insert into users (username,password,shanghuname,usertype) values('{$username}','{$password}','{$shanghuname}','{$usertype}');";
    $result=$conn->sql($sql);
    if($result){
        echo '{"status":"1","msg":"添加成功"}';
		
    }
    else{
		echo '{"status":"0","msg":"添加失败"}';
    }
    $conn->close();
}
 
 
 //登录
function login($username,$password){

    $conn=new Mysql();
    $sql="select * from users where username= '{$username}';";
    $result=$conn->sql($sql);
    if($result){
        $row = $result->fetch_assoc();
        //if(strtotime($time) < strtotime($row['viptime'])) //当前时间小于VIP时间 注释掉，登录不需要判断viptime
        //{
            if($password==$row['password'])
            {
                //$row['logintime']=$time;
                echo '{"status":"1","msg":"登录成功","usertype":"'.$row['usertype'].'"}';
            }
            else
            {
                echo '{"status":"0","msg":"登录失败，密码错误"}';
            }
         
        //}
        //else
        //{

            //echo '{"status":"0","msg":"账号过期，请充值"}';
        //}
        
    }
    else{
        echo '{"status":"0","msg":"账号不存在"}';
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
