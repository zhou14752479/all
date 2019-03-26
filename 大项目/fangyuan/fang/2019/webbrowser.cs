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

namespace fang._2019
{
    public partial class webbrowser : Form
    {
        public webbrowser()
        {
            InitializeComponent();
        }
        public static string cookie { get; set; }
       

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);

        #region 非常重要获取当前存在浏览器的cookie，可以登陆wbbbrowser更新cookie。
        /// <summary>
        /// 非常重要获取当前存在浏览器的cookie，可以登陆wbbbrowser更新cookie。
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetCookies(string url)
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
        private void button1_Click(object sender, EventArgs e)
        {
            cookie = GetCookies("https://myseller.taobao.com/home.htm");
            textBox1.Text = cookie;

            //淘宝商品上下架 tb = new 淘宝商品上下架();
            //tb.Show();
            //this.Hide();
        }

        private void webbrowser_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://login.taobao.com/member/login.jhtml");

        }
    }
}
