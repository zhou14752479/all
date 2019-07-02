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

namespace main
{
    public partial class webBrowser : Form
    {
        public static string cookie { get; set; }
      
        public string url;

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);



        public webBrowser(string URL)
        {
            InitializeComponent();
            this.url = URL;   //构造函数传参到成员变量
        }

        private void webBrowser_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true;

            webBrowser1.Url = new Uri(this.url);

           // timer1.Start();
            method.SetIE(0);

            this.webBrowser1.ScriptErrorsSuppressed = true;  //屏蔽IE脚本弹出错误
            this.webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;  //屏蔽IE脚本弹出错误
        }

        #region 非常重要获取当前存在浏览器的cookie，可以登陆wbbbrowser更新cookie。
        /// <summary>
        /// 非常重要获取当前存在浏览器的cookie，可以登陆wbbbrowser更新cookie。
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

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.webBrowser1.Document.Window.Error += OnWebBrowserDocumentWindowError;
            textBox1.Text = webBrowser1.Document.Cookie; ;
            cookie = textBox1.Text;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = GetCookies("https://order.jd.com/center/list.action?d=1&s=4096&page=1");
            cookie = textBox1.Text;
            this.Hide();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
        }

      
        private void OnWebBrowserDocumentWindowError(object sender, HtmlElementErrorEventArgs e)
        {
            e.Handled = true;
        }

     

      

        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
          
        }

    }


}
