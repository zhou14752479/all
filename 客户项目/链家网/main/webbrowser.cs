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
    public partial class webbrowser : Form
    {
        public static string URL="";

        public static string cookie { get; set; }
       

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);

        public webbrowser(string url)
        {
            InitializeComponent();
            URL = url;
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
        private void Webbrowser_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true;

            webBrowser1.Url = new Uri(URL);
        }

        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            textBox1.Text = GetCookies("http://www.17gp.com/Common/GetOpenCityList");
            cookie = textBox1.Text;
        }
    }
}
