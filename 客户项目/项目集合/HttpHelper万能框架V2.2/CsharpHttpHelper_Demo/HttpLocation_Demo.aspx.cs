using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CsharpHttpHelper;

namespace CsharpHttpHelper_Demo
{
    public partial class HttpLocation_Demo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "http://sufeinet.com",//URL     必需项    
                Method = "get",//URL     可选项 默认为Get   
                ContentType = "text/html",//返回类型    可选项有默认值   
                Allowautoredirect = false//默认为False就是不根据重定向自动跳转
            };
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请求的Cookie
            string cookie = result.Cookie;


            //获取302跳转URl
            string redirectUrl = result.RedirectUrl;


            item = new HttpItem()
            {
                URL = redirectUrl,//URL     必需项    
                Method = "get",//URL     可选项 默认为Get   
                ContentType = "text/html",//返回类型    可选项有默认值   
                Cookie = cookie
            };
            //请求的返回值对象
            result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
            //获取请求的Cookie
            cookie = result.Cookie;
        }
    }
}