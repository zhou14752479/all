using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    public delegate void rundelegate();//开始方法的委托

    public delegate void Userdelegate(string username);  //登陆传输用户名方法的委托



    class pubVariables
    {
        public int index { get; set; }  //datagridview自增id

        public string mtcookie { get; set; }  //美团美食cookie

        public string aliCookie { get; set; }  //阿里巴巴cookie

        public static string Cookie { get; set; }
        public static bool status { get; set; } //控制程序开始停止

        public static string exs { get; set; }  //异常值

        public static  string item { get; set; }  //模板列表右键选中的菜单项。


        public static string citys { get; set; }  //参数界面城市
        public static string keywords { get; set; }//参数界面关键词行业

        public static int fangFrom { get; set; } //房产来源0为个人 1为中介
    }
}
