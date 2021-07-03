using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using System.Net;

namespace CsharpHttpHelper_Demo
{
    public partial class HttpCookieCollection_Demo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "http://www.sufeinet.com",//URL     必需项    
                Method = "get",//URL     可选项 默认为Get   
                ContentType = "text/html",//返回类型    可选项有默认值   
                ResultCookieType = ResultCookieType.CookieCollection
            };
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请求的Cookie
            CookieCollection cookie = result.CookieCollection;

            // 第二次使用Cookie

            //创建Httphelper参数对象
            item = new HttpItem()
            {
                URL = "http://www.sufeinet.com/thread-9989-1-1.html",//URL     必需项    
                Method = "get",//URL     可选项 默认为Get   
                ContentType = "text/html",//返回类型    可选项有默认值   
                CookieCollection = cookie,//把Cookie写入请求串中
                ResultCookieType = ResultCookieType.CookieCollection
            };
            //请求的返回值对象
            result = http.GetHtml(item);

            //获取Html
            string html = result.Html;
        }
    }
}