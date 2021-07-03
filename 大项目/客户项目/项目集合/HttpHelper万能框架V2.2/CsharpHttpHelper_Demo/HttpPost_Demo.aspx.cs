using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CsharpHttpHelper;
using System.Net;
using CsharpHttpHelper.Enum;

namespace CsharpHttpHelper_Demo
{
    public partial class HttpPost_Demo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = "http://www.sufeinet.com",//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值
                PostDataType = PostDataType.String,//默认为字符串，同时支持Byte和文件方法
                PostEncoding = System.Text.Encoding.UTF8,//默认为Default，
                Postdata = "a=123&c=456&d=789",//Post要发送的数据
            };
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;


            ////要Post的数据
            //string postdate = "a=123&c=456&d=789";
            ////将Post数据转为字节数组
            //byte[] bytedate = System.Text.Encoding.UTF8.GetBytes(postdate);
            ////创建Httphelper对象
            //HttpHelper http = new HttpHelper();
            ////创建Httphelper参数对象
            //HttpItem item = new HttpItem()
            //{
            //    URL = "http://www.sufeinet.com",//URL     必需项    
            //    Method = "post",//URL     可选项 默认为Get   
            //    ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值
            //    PostDataType = PostDataType.Byte,
            //    PostdataByte = bytedate
            //};
            ////请求的返回值对象
            //HttpResult result = http.GetHtml(item);
            ////获取请请求的Html
            //string html = result.Html;
            ////获取请求的Cookie
            //string cookie = result.Cookie;


            ////要Post的数据
            //string postfile = @"D:\postdata.txt";
            ////将Post数据转为字节数组
     
            ////创建Httphelper对象
            //HttpHelper http = new HttpHelper();
            ////创建Httphelper参数对象
            //HttpItem item = new HttpItem()
            //{
            //    URL = "http://www.sufeinet.com",//URL     必需项    
            //    Method = "post",//URL     可选项 默认为Get   
            //    ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值
            //    PostDataType = PostDataType.FilePath,
            //    Postdata = postfile
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