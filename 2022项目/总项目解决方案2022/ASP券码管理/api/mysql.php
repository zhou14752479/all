<?php
class Mysql{
         private static $host="localhost";
         private static $user="root";
         private static $password="root";
         private static $dbName="quan";                     //数据库名
         private static $charset="utf8";                    //字符编码
         private static $port="3306";                       //端口号
         private  $conn=null;
         function __construct(){
             $this->conn=new mysqli(self::$host,self::$user,self::$password,self::$dbName,self::$port);
             if(!$this->conn)
             {
                   die("数据库连接失败！".$this->conn->connect_error);
             }else{
                 //echo "连接成功！";
             }
             $this->conn->query("set names ".self::$charset);
         }
         
         //执行sql语句
         function sql($sql){
          try
          {
            $res=$this->conn->query($sql);

         if(!$res)
              {
               
                   echo "数据操作失败";

              }
              else
              {
                   if($this->conn->affected_rows>0)
                   {
                         return $res;
                   }
                   else
                   {
                        //echo "0行数据受影响！";
                   }
              }
          }
             

             catch (Exception $e) {

echo $e->getMessage();

}
              
         }
         
         //返回受影响数据行数
         function getResultNum($sql){
	           $res=$this->conn->query($sql);
	           return mysqli_num_rows($res);
           }
         
         //关闭数据库
         public function close()
         {
             @mysqli_close($this->conn);
         }
}
?>

