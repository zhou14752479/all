using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CsharpHttpHelper;
using System.Collections.Specialized;

namespace CsharpHttpHelper_Demo
{
    public partial class HttpUrl_Demospx : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            string url = HttpHelper.URLEncode("http://member1.taobao.com/member/user_profile.jhtml?user_id=欧影点点");
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL =url,//URL     必需项    
                Method = "get",//URL     可选项 默认为Get   
                ContentType = "text/html",//返回类型    可选项有默认值   
            };
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            string html = result.Html;
            //获取请求的Cookie
            string cookie = result.Cookie;

      



            string parameters = "a=123456&b=456789&c=456456";
            //得到一个参数集合
            NameValueCollection list = HttpHelper.GetNameValueCollection(parameters);


            string a = list["a"];
            string b = list["b"];
            string c = list["c"];

            Response.Write(string.Format("a={0}<br/>b={1}<br/>c={2}", a, b, c));

            list["a"] = list["a"] + "修改过我";

            list["c"] = list["c"] + "修改过我";
             a = list["a"];
             b = list["b"];
             c = list["c"];

             Response.Write(string.Format("<br/><br/>a={0}<br/>b={1}<br/>c={2}", a, b, c));


         
            //使用指定的编码对象对 URL 字符串进行编码。
            string URLEncode = HttpHelper.URLEncode(parameters);


            //使用指定的编码对象将 URL 编码的字符串转换为已解码的字符串。
            string URLDecode = HttpHelper.URLDecode(URLEncode);


            Response.Write(string.Format("<br/><br/>URLEncode={0}<br/>URLDecode={1}<br/>", URLEncode, URLDecode));


            //使用指定的编码对象对 URL 字符串进行编码。
            URLEncode = HttpHelper.URLEncode(parameters, System.Text.Encoding.UTF8);


            //使用指定的编码对象将 URL 编码的字符串转换为已解码的字符串。
            URLDecode = HttpHelper.URLDecode(URLEncode, System.Text.Encoding.UTF8);

            Response.Write(string.Format("<br/><br/>URLEncode={0}<br/>URLDecode={1}<br/>", URLEncode, URLDecode));


            string host = HttpHelper.GetUrlHost("http://www.sufeinet.com");

            string ip = HttpHelper.GetUrlIp("http://www.sufeinet.com");
        }
    }
}