using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASP姓名身份验证
{
    

    public partial class WebForm1 : System.Web.UI.Page
    {

        #region POST请求

        public string PostUrl(string url, string postData)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.Proxy = null;//防止代理抓包
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.Headers.Add("Cookie", "");
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                //MessageBox.Show(ex.ToString());
                return ex.ToString();
            }


        }

        #endregion



       




        protected void Page_Load(object sender, EventArgs e)
        { //Response.Write("1111");
            if (HttpContext.Current.Request.RequestType == "GET")
            {
                if (Request.QueryString["c1"] != "")
                {
                    getresult();
                }
            }
                    
        }



        public void getresult()
        {
            string mobile = Request["c1"];
            string url = "https://puser.zjzwfw.gov.cn/sso/newusp.do";

            string postdata = "action=regByMobile&mobilephone=" + System.Web.HttpUtility.UrlEncode(mobile);
            string html = PostUrl(url, postdata);
            string username = Regex.Match(html, @"""username"":""([\s\S]*?)""").Groups[1].Value;
            if (username == "")
            {
                Application["result"] = "无";
            }
            else
            {
                Application["result"] = username;
            }
          
        }






    }
}