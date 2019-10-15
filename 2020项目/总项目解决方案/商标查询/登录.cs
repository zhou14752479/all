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

namespace 商标查询
{
    public partial class 登录 : Form
    {
        public 登录()
        {
            InitializeComponent();
        }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int InternetSetCookieEx(string lpszURL, string lpszCookieName, string lpszCookieData, int dwFlags, IntPtr dwReserved);

        public static string COOKIE = "";
        private void 登录_Load(object sender, EventArgs e)
        {
           

            //默认打开淘宝登录
            webBrowser1.Url = new Uri("https://www.qichacha.com/user_login");
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
        private void Button4_Click(object sender, EventArgs e)
        {
            COOKIE = GetCookies("https://www.qichacha.com/search?key=%E5%AE%BF%E8%BF%81%E7%BD%91%E7%BB%9C%E7%A7%91%E6%8A%80");
           
            this.Hide();
        }
    }
}
