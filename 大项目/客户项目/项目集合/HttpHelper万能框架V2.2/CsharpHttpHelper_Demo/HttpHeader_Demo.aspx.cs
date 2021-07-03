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
    public partial class HttpHeader_Demo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            //===================================header======================================

            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "http://www.sufeinet.com",//URL     必需项    
                Method = "get",//URL     可选项 默认为Get   
                ContentType = "text/html",//返回类型    可选项有默认值   
                //ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值   
            };
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;
            //Header
            WebHeaderCollection header = result.Header;

            if (header != null)
            {
                string Vary = header["Vary"];
                string XCache = header["X-Cache"];
                string XServer = header["X-Server"];
            }

        }
    }
}