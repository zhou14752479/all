using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Input;

namespace 视频号web端
{
    public partial class xk : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          string  cookie = File.ReadAllText(Server.MapPath("~/") + "cookie.txt").Trim();
            string method = Request["method"];
            string aid = Request["id"];
            if (method == "kd0ph8fynbhj" && aid != "")
            {
                try
                {
                    string url = "https://user.zxxk.com/creator/api/v1/creator-resource/get-resource-detail?resourceId=" + aid;

                    string html = GetUrlWithCookie(url, cookie);
                    Response.Write(html);
                }
                catch (Exception)
                {

                    Response.Write("{\"status\":\"0\",\"msg\":\"网址输入有误\"}");
                }
            }
            else
            {
                Response.Write("{\"status\":\"0\",\"msg\":\"网址输入有误\"}");
            }
        }






        #region GET请求带COOKIE
        public static string GetUrlWithCookie(string Url, string COOKIE)
        {
            string charset = "utf-8";
            string result;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36";
                request.Referer = Url;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("version:TYC-XCX-WX");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                bool flag = response.Headers["Content-Encoding"] == "gzip";
                string html;
                if (flag)
                {
                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader2 = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset));
                    html = reader2.ReadToEnd();
                    reader2.Close();
                }
                response.Close();
                result = html;
            }
            catch (WebException ex)
            {
                result = ex.ToString();

            }
            return result;
        }
        #endregion


    }
}