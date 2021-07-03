using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CsharpHttpHelper;
using System.Net;

namespace CsharpHttpHelper_Demo
{
    public partial class HttpProxy_Demo : System.Web.UI.Page
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
            //    ProxyIp = "192.168.1.18:2011",
            //};

            ////请求的返回值对象
            //HttpResult result = http.GetHtml(item);
            ////获取请请求的Html
            //string html = result.Html;
            ////获取请求的Cookie
            //string cookie = result.Cookie;



            ////创建Httphelper对象
            //HttpHelper http = new HttpHelper();
            ////创建Httphelper参数对象
            //HttpItem item = new HttpItem()
            //{
            //    URL = "http://www.sufeinet.com",//URL     必需项    
            //    Method = "get",//URL     可选项 默认为Get   
            //    ContentType = "text/html",//返回类型    可选项有默认值 
            //    ProxyIp = "192.168.1.18:2011",
            //    ProxyUserName = "admin",
            //    ProxyPwd = "123456"
            //};

            ////请求的返回值对象
            //HttpResult result = http.GetHtml(item);
            ////获取请请求的Html
            //string html = result.Html;
            ////获取请求的Cookie
            //string cookie = result.Cookie;


            //WebProxy myProxy = new WebProxy("192.168.15.11", 8015);
            ////建议连接
            //myProxy.Credentials = new NetworkCredential("admin", "123456");

            ////创建Httphelper对象
            //HttpHelper http = new HttpHelper();
            ////创建Httphelper参数对象
            //HttpItem item = new HttpItem()
            //{
            //    URL = "http://www.sufeinet.com",//URL     必需项    
            //    Method = "get",//URL     可选项 默认为Get   
            //    ContentType = "text/html",//返回类型    可选项有默认值 
            //    WebProxy = myProxy
            //};

            ////请求的返回值对象
            //HttpResult result = http.GetHtml(item);
            ////获取请请求的Html
            //string html = result.Html;
            ////获取请求的Cookie
            //string cookie = result.Cookie;
        }
    }
}