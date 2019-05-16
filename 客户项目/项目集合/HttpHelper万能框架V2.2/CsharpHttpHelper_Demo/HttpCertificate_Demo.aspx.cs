using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CsharpHttpHelper;
using System.Security.Cryptography.X509Certificates;

namespace CsharpHttpHelper_Demo
{
    public partial class HttpCertificate_Demo : System.Web.UI.Page
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
            //    CerPath = "D:\\123.cer"
            //};

            ////请求的返回值对象
            //HttpResult result = http.GetHtml(item);
            ////获取请请求的Html
            //string html = result.Html;
            ////获取请求的Cookie
            //string cookie = result.Cookie;


            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "http://www.sufeinet.com",//URL     必需项    
                Method = "get",//URL     可选项 默认为Get   
                ContentType = "text/html",//返回类型    可选项有默认值   
            };
            item.ClentCertificates = new X509CertificateCollection();

            //配置多个证书在这里设置
            item.ClentCertificates.Add(new X509Certificate("d:\\123.cer","123456"));

            //配置多个证书在这里设置
            item.ClentCertificates.Add(new X509Certificate("d:\\456.cer"));

            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;
        }
    }
}