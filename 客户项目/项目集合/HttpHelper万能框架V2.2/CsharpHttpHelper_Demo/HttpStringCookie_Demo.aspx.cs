using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using System.Net;
using System.Text;

namespace CsharpHttpHelper_Demo
{
    public partial class HttpStringCookie_Demo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ////创建Httphelper对象
            //HttpHelper http = new HttpHelper();
            ////创建Httphelper参数对象
            //HttpItem item = new HttpItem()
            //{
            //    URL = "http://www.sufeinet.com",//URL     必需项    
            //    Method = "get",//URL     可选项 默认为Get   
            //    ContentType = "text/html",//返回类型    可选项有默认值   
            //    //ResultCookieType = ResultCookieType.String //默认值可以不写
            //};
            ////请求的返回值对象
            //HttpResult result = http.GetHtml(item);
            ////获取请求的Cookie
            //string cookie = result.Cookie;

            //// 第二次使用Cookie

            ////创建Httphelper参数对象
            //item = new HttpItem()
            //{
            //    URL = "http://www.sufeinet.com/thread-9989-1-1.html",//URL     必需项    
            //    Method = "get",//URL     可选项 默认为Get   
            //    ContentType = "text/html",//返回类型    可选项有默认值   
            //    Cookie = cookie,//把Cookie写入请求串中
            //};
            ////请求的返回值对象
            //result = http.GetHtml(item);

            ////获取Html
            //string html = result.Html;




            ////创建Httphelper对象
            //HttpHelper http = new HttpHelper();
            ////创建Httphelper参数对象
            //HttpItem item = new HttpItem()
            //{
            //    URL = "http://www.sufeinet.com",//URL     必需项    
            //    Method = "get",//URL     可选项 默认为Get   
            //    ContentType = "text/html",//返回类型    可选项有默认值   
            //    //ResultCookieType = ResultCookieType.String //默认值可以不写
            //};
            ////请求的返回值对象
            //HttpResult result = http.GetHtml(item);
            ////获取请求的Cookie
            //string cookie = result.Cookie;

            ////字符串Cookie转为CookieCollection类型Cookie
            //CookieCollection cookielist = HttpHelper.StrCookieToCookieCollection(cookie);




            ////创建Httphelper对象
            //HttpHelper http = new HttpHelper();
            ////创建Httphelper参数对象
            //HttpItem item = new HttpItem()
            //{
            //    URL = "http://www.sufeinet.com",//URL     必需项    
            //    Method = "get",//URL     可选项 默认为Get   
            //    ContentType = "text/html",//返回类型    可选项有默认值   
            //    ResultCookieType = ResultCookieType.CookieCollection //默认值可以不写
            //};
            ////请求的返回值对象
            //HttpResult result = http.GetHtml(item);
            ////获取请求的Cookie
            //CookieCollection cookie = result.CookieCollection;

            StringBuilder sb = new StringBuilder();
            sb.Append("l=AXy72RVgfAR8BClVDbA9NXwEPAt8BHwE; isg=6D7B84DB2241F44D6824B6785126E336; ali_apache_track=\"c_ms=2|c_mt=3|c_mid=gzlichengzp|c_lid=gzlichengzp\"; last_mid=gzlichengzp; _cn_slid_=n4cA6vtSNo; __last_loginid__=gzlichengzp; alicnweb=touch_tb_at%3D1431354465081%7Clastlogonid%3Dgzlichengzp%7Cshow_inter_tips%3Dfalse; ali_beacon_id=58.62.16.50.1431244213615.266543.2; cna=rMHEDcU4aCgCATo+EDLUq7jJ; ali_ab=58.62.16.50.1431250916102.4; __rn_alert__=false; _is_show_loginId_change_block_=gzlichengzp_false; _show_force_unbind_div_=gzlichengzp_false; _show_sys_unbind_div_=gzlichengzp_false; _show_user_unbind_div_=gzlichengzp_false; JSESSIONID=8L78TCuu1-DgbTWEbSXLR8T8Lt39-XUZ9RCP-wBof; _tmp_ck_0=\"uS3J%2FTASuVfGq4%2B9bjUo57eCMhGUhO6AMFWKUJF5pqKWn9ORcSRgfOuEpps%2BAASpHLQPppMcRuvYcU3rniJeHlU8wAIvd4H1Btt9PnU7UMx%2BvLmoVxPv7bIpLxEXREqls4tmVmkOWNQEpfdcLy8XPJYoaqrNr0KxHVOIqYe4aluaDBYCz2SBXFOmfoDxZDPePcpCqN0KqzL4t%2F3XzyyOa%2BHyM9kotlQz9ULrGyl0v1nWnS1Wl1O%2FOFXmTpjHCjRjmgf5tz5wzAz2IPMHNYKQ%2FQ6DVPCzMU0nybIerLa7VqPGu%2Fm3cSnCqRYKkjpll2VoEIwFhTOavR0brekeCXkh97VvRphqI8a2fzuEvOx7mMEGLQz%2BVHxptrJ4Bc7anYVp0yCmLcm0EL4qp%2BwsABZPpd9wkXGWcOhY1kB6Zr1I%2FBuKCiENDUGBYD68ukV%2BPNqZorq6L5Iw0xxXIvcQ%2FcuETIRx3c%2FKKIVgG2pJN%2BInZJJI3hZyzKJBRgnClj8TvaGrrPCWYDlnoY3gWr8qMU4ZqA%3D%3D\"; __cn_logon__=true; __cn_logon_id__=gzlichengzp; ali_apache_tracktmp=\"c_w_signed=Y\"; cn_tmp=\"Z28mC+GqtZ1TxeH4N0T4DjZsAVrX6POzEGjAHX1MyN2K2E9nxCEjPTUnyXXWRhsB0GM9sHa7m7dryMfhTcs50R4SqKg1QjxDWEirwbiEygKn4sZfQB7Xgz4vhH9y94eN3RY0rcb6lU5bBMQDKYqXGBqUV/dS7zHpFTIMC/Z2hNhnvyc0GI4xcHsBd59oZv9IuU3Gi9IwaMtGb7nUWQdImCSPRmRnUHI6/dpO9lgDHQA=\"; _nk_=\"9MnzqFikbJBWz36n89epsA%3D%3D\"; LoginUmid=\"GYQM0kd2T6odgmXq7lAkRGPwDxpzF6HXn7klNIfVBA689QAyb6jGqQ%3D%3D\"; userID=\"ZX%2Bz%2FuCL8Hu7fSw3GYYfaWRjh2j2PLsf%2B2MPwLhPQ586sOlEpJKl9g%3D%3D\"; unb=993873027; userIDNum=\"wNHJDfM%2B0K6OMv4GYTxyHg%3D%3D\"; login=\"kFeyVBJLQQI%3D\"; _csrf_token=1431400880881");

            //CookieCollection类型Cookiel转为字符串Cookie
            CookieCollection strcookie = HttpHelper.StrCookieToCookieCollection(sb.ToString());
        }
    }
}