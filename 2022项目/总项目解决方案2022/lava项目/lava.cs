using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lava项目
{
    public partial class lava : Form
    {
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int InternetSetCookieEx(string lpszURL, string lpszCookieName, string lpszCookieData, int dwFlags, IntPtr dwReserved);

        public lava()
        {
            InitializeComponent();
        }
        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData, string COOKIE,int type)
        {
            try
            {
                string charset = "utf-8";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";
                //request.ContentLength = postData.Length;
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.Referer = url;
                ServicePointManager.Expect100Continue = false;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
              string setcookie= response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                if(type==0)
                {
                    return setcookie;
                }
                else
                {
                    return html;
                }
               
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion


        #region  获取cookie
        /// <summary>
        /// 获取cookie
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetCookies(string url)
        {
            uint datasize = 256;
            StringBuilder cookieData = new StringBuilder((int)datasize);
            if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x2000, IntPtr.Zero))
            {
                if (datasize < 0)
                    return null;


                cookieData = new StringBuilder((int)datasize);
                if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x00002000, IntPtr.Zero))
                    return null;
            }
            return cookieData.ToString();
        }

        #endregion



        Thread thread;


        public string cookie;


        public void add()
        {
             cookie = GetCookies("https://www.3mtech.cn/lava/instituteMgm/approvalList");
            //@"JSESSIONID=""EaMtx_y7YXjLHobVBkDi-CrclkDfGXmh7vwlOuv-.jb7peccn01-host:8780_lava""; li_id=li_49"
            MessageBox.Show(cookie);
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.3mtech.cn/lava/instituteMgm/addInstitution");

                request.KeepAlive = true;
                request.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0");
                request.Headers.Add("sec-ch-ua", @"""Google Chrome"";v=""105"", ""Not)A;Brand"";v=""8"", ""Chromium"";v=""105""");
                request.Headers.Add("sec-ch-ua-mobile", @"?0");
                request.Headers.Add("sec-ch-ua-platform", @"""Windows""");
                request.Headers.Add("Upgrade-Insecure-Requests", @"1");
                request.Headers.Add("Origin", @"https://www.3mtech.cn");
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                request.Headers.Add("Sec-Fetch-Site", @"same-origin");
                request.Headers.Add("Sec-Fetch-Mode", @"navigate");
                request.Headers.Add("Sec-Fetch-User", @"?1");
                request.Headers.Add("Sec-Fetch-Dest", @"document");
                request.Referer = "https://www.3mtech.cn/lava/instituteMgm/inputInstitution";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.9");
                request.Headers.Set(HttpRequestHeader.Cookie, cookie);

                request.Method = "POST";
                request.ServicePoint.Expect100Continue = false;

                string body = @"name="+ System.Web.HttpUtility.UrlEncode(textBox1.Text) + "&typeID.typeID=1&province=340000&city=340800&comments="+ System.Web.HttpUtility.UrlEncode(textBox2.Text);
                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                request.ContentLength = postBytes.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(postBytes, 0, postBytes.Length);
                stream.Close();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                if (response != null)
                {
                    MessageBox.Show("新增 成功");
                }
            }
            catch (Exception ex)
            {

                textBox2.Text = ex.ToString();
            }
        }
       
        private void button2_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(add);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void lava_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("https://www.3mtech.cn/lava");
        }
    }
}
