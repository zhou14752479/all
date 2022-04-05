using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202203
{
    public partial class cookie获取 : Form
    {
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int InternetSetCookieEx(string lpszURL, string lpszCookieName, string lpszCookieData, int dwFlags, IntPtr dwReserved);

        public cookie获取()
        {
            InitializeComponent();
        }
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

      
        public void  PostUrl()
        {
            try
            {
                string cookie = GetCookies("https://sinfo.ccb.com/tran/WCCMainPlatV5?CCB_IBSVersion=V5&SERVLET_NAME=WCCMainPlatV5&TXCODE=NYS10Z&BILL_TYPE=100&BILL_ITEM=01018");
                textBox1.Text = cookie;
                string postData = "cookie=" + cookie;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://h.qakmak.com/api/index/jianshecookie");
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
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
               
            }
            catch (WebException ex)
            {

                textBox1.Text= ex.ToString();
            }


        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("appid:orders");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 100000;

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {
                return ex.ToString();

            }



        }
        #endregion
        private void cookie获取_Load(object sender, EventArgs e)
        {
            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"360问答"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("https://cloud.life.ccb.com/live/live/appService_portal.jhtml?billItem=01018");
        }


        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
            timer1.Start();
           
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(PostUrl);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(PostUrl);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
