using SHDocVw;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taobao
{
    public partial class webbrowser : Form
    {

        public static string cookie{get; set;}
        public string URL { get; set; }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);

        #region 非常重要获取当前存在浏览器的cookie，可以登陆wbbbrowser更新cookie。
        /// <summary>
        /// 非常重要获取当前存在浏览器的cookie，可以登陆wbbbrowser更新cookie。
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public  string GetCookies(string url)
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

        public webbrowser(string url)
        {
            InitializeComponent();
            this.URL = url;
        }

        private void WebBrowser_BeforeNavigate2(object pDisp, ref object URL, ref object Flags,ref object TargetFrameName, ref object PostData, ref object Headers, ref bool Cancel)
        {
            string postDataText = System.Text.Encoding.ASCII.GetString(PostData as byte[]);

            textBox1.Text = postDataText;
        }

        private void webbrowser_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true;

            webBrowser1.Url = new Uri(this.URL);

            //获取POST数据包
            //SHDocVw.WebBrowser wb = (SHDocVw.WebBrowser)webBrowser1.ActiveXInstance;
            //wb.BeforeNavigate2 += new DWebBrowserEvents2_BeforeNavigate2EventHandler(WebBrowser_BeforeNavigate2);

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            cookie=GetCookies(this.URL);
            textBox1.Text = cookie;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // cookie = GetCookies("https://zizhanghao.taobao.com/subaccount/monitor/chatRecordQuery.htm?spm=a211ki.11005395.0.0.5c3361c4QipeGm&query=empty");
            cookie = GetCookies(this.URL);
            textBox1.Text = cookie;
            
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
