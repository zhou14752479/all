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

namespace main._2019_10
{
    public partial class 登陆 : Form
    {
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int InternetSetCookieEx(string lpszURL, string lpszCookieName, string lpszCookieData, int dwFlags, IntPtr dwReserved);

        public static string COOKIE = "";
        public 登陆()
        {
            InitializeComponent();
        }

        private void 登陆_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true;

            //默认打开淘宝登录
            webBrowser1.Url = new Uri("https://login.taobao.com/member/login.jhtml?style=mini&newMini2=true&from=alimama&redirectURL=http%3A%2F%2Flogin.taobao.com%2Fmember%2Ftaobaoke%2Flogin.htm%3Fis_login%3d1&full_redirect=true&disableQuickLogin=true");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HtmlDocument dc = webBrowser1.Document;
            HtmlElementCollection es = dc.GetElementsByTagName("input");   //GetElementsByTagName返回集合
            foreach (HtmlElement e1 in es)
            {
                if (e1.GetAttribute("name") == "TPL_username")
                {
                    e1.SetAttribute("value", textBox1.Text.Trim());
                }
                if (e1.GetAttribute("name") == "TPL_password")
                {
                    e1.SetAttribute("value", textBox2.Text.Trim());
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri("https://www.alimama.com/member/minilogin.htm?redirect=https%3A%2F%2Fwww.alimama.com%2Findex.htm");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HtmlDocument dc = webBrowser1.Document;
            HtmlElementCollection es = dc.GetElementsByTagName("input");   //GetElementsByTagName返回集合
            foreach (HtmlElement e1 in es)
            {
                if (e1.GetAttribute("name") == "logname")
                {
                    e1.SetAttribute("value", textBox3.Text.Trim());
                }
                if (e1.GetAttribute("id") == "J_logpassword")
                {
                    e1.SetAttribute("value", textBox4.Text.Trim());
                }
            }
        }
        #region  获取cookie
        /// <summary>
        /// 获取cookie
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string GetCookies(string url)
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

      
        private void button4_Click(object sender, EventArgs e)
        {
            COOKIE = GetCookies("https://pub.alimama.com/manage/overview/index.htm");
            this.Hide();
        }
    }
}
