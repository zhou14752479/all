using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CsharpHttpHelper;
using System.Net;
using System.Text;
using CsharpHttpHelper.Enum;

namespace CsharpHttpHelper_Demo
{
    public partial class HttpGet_Demo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ////创建Httphelper对象
            HttpHelper http = new HttpHelper();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "https://www.ct10649.com:/ecportal/login/logout.action",//URL     必需项    
                SecurityProtocol = (SecurityProtocolType)3072
            };
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
             
            string html = result.Html;
        }
        /// <summary>
        /// 在异步执行完成后要回调的方法
        /// </summary>
        /// <param name="result"></param>
        public void SetHtml(HttpResult result)
        {
            //获取请请求的Html
            string html = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;

            //状态码
            HttpStatusCode code = result.StatusCode;
            //状态描述
            string Des = result.StatusDescription;
            if (code == HttpStatusCode.OK)
            {
                //状态为200
            }
        }
    }
}