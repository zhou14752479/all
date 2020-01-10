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

namespace 网络游戏定单
{
    public partial class 登录 : Form
    {
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int InternetSetCookieEx(string lpszURL, string lpszCookieName, string lpszCookieData, int dwFlags, IntPtr dwReserved);

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
        public static string COOKIE;
        public 登录()
        {
            InitializeComponent();
        }

        private void 登录_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://passport.dd373.com/");
            webBrowser1.ScriptErrorsSuppressed = true;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            COOKIE = GetCookies("https://newuser.dd373.com/usercenter/index.html");
            //COOKIE = "acw_tc=781bad2415784720157685722e22398240b3e401f65c0641759954504134d9; Hm_lvt_b1609ca2c0a77d0130ec3cf8396eb4d5=1578471993,1578533798; ASP.NET_SessionId=n05u4rffiquc1uyvpy2bc5u5; Hm_lpvt_b1609ca2c0a77d0130ec3cf8396eb4d5=1578533817; member_session=QJE6nVJhu6SBytZ1QcWZT718jN%2bxWYRNrCdH2xniueHdQ12mx0bs1NHhdsdEKUh6nSbOMe%2foBhDCLRunLMWg0Oyi%2bPlc%2bwKbzD4RlNKI9b%2bsyyLXTkcy0R1t2knjgZjuYfPpzmj5y8EoOsgo6o8BmDE%2bz8tDhAZGZEMXO%2bTBuN3EviJJVcoKlga2u2yB9IC3eCvzNllTt2eHwhqjN8%2bAii%2bTvLV0lpxRgPd6gN7J36ZikHepwEHjVQInl6Hr38jS8m%2ftFLHkrpOclnRAQVibJ55EB7pjxD8vXcVkefvnJ2YS2Tgeo7hxYb2vSn%2fPFjXZcpY%2bwjivvHlK3lBfD0rm1w%3d%3d; SERVERID=cc4b31544f244b28513350b1f4866ec2|1578533949|1578533848";
            this.Hide();
        }
    }
}
