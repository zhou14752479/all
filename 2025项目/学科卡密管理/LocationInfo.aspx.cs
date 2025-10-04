using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 学科卡密管理
{
    public partial class LocationInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";

            try
            {
                string userIp = Request.UserHostAddress;
                string area = getareaFromIp( userIp);  //获取IP信息

                // 限制宿迁访问
                if (!area.Contains("宿迁市"))
                {
                    Response.Write("{\"status\":1,\"msg\":\"仅限宿迁地区访问\"}");
                    Response.End();
                    return;
                }

                Response.Write("{\"status\":0,\"msg\":\"" + area + "\"}");
            }
            catch (Exception ex)
            {
                Response.Write("{\"status\":2,\"msg\":\"" + ex.Message + "\"}");
            }

            Response.End();
        }




        public static string getareaFromIp(string ip)
        {
            string url = "https://www.ipshudi.com/" + ip + ".htm";
            string html = GetUrlWithCookie(url, "");
            string area = Regex.Match(html, @"归属地</td>([\s\S]*?)</span>").Groups[1].Value;
            area = Regex.Replace(area, "<[^>]+>", "").Replace(" ", "").Trim();
            return area;
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