using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CsharpHttpHelper;
using CsharpHttpHelper.Item;
using System.Text;

namespace CsharpHttpHelper_Demo
{
    public partial class HttpHtml_Demo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ////创建Httphelper对象
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


            //List<AItem> alist = HttpHelper.GetAList(html);


            ////创建Httphelper对象
            //HttpHelper http = new HttpHelper();
            ////创建Httphelper参数对象
            //HttpItem item = new HttpItem()
            //{
            //    URL = "http://www.sufeinet.com",//URL     必需项    
            //    Method = "get",//URL     可选项 默认为Get   
            //    ContentType = "text/html",//返回类型    可选项有默认值   
            //    //ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值   
            //};
            ////请求的返回值对象
            //HttpResult result = http.GetHtml(item);
            ////获取请请求的Html
            //string html = result.Html;


            //List<ImgItem> imglist = HttpHelper.GetImgList(html);


            ////创建Httphelper对象
            //HttpHelper http = new HttpHelper();
            ////创建Httphelper参数对象
            //HttpItem item = new HttpItem()
            //{
            //    URL = "http://www.sufeinet.com",//URL     必需项    
            //    Method = "get",//URL     可选项 默认为Get   
            //    ContentType = "text/html",//返回类型    可选项有默认值   
            //    //ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值   
            //};
            ////请求的返回值对象
            //HttpResult result = http.GetHtml(item);
            ////获取请请求的Html
            //string html = result.Html;

            //html = HttpHelper.ReplaceNewLine(html);
            //html = HttpHelper.StripHTML(html);

            //string html = "苏飞论坛";
            //string str = HttpHelper.GetBetweenHtml(html, "苏", "论坛");

            //byte[] b = HttpHelper.StringToByte("苏飞");
            ////可指定编码
            //b = HttpHelper.StringToByte("苏飞", Encoding.UTF8);


            //string s = HttpHelper.ByteToString(b);
            //s = HttpHelper.ByteToString(b, Encoding.UTF8);

            //string title = HttpHelper.GetHtmlTitle(html);
        }
    }
}