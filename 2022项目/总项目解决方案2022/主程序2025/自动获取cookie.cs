using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序2025
{
    public partial class 自动获取cookie : Form
    {
        public 自动获取cookie()
        {
            InitializeComponent();
        }
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref uint pcchCookieData, int dwFlags, IntPtr lpReserved);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int InternetSetCookieEx(string lpszURL, string lpszCookieName, string lpszCookieData, int dwFlags, IntPtr dwReserved);


       

        private void button1_Click(object sender, EventArgs e)
        {

            writeCookie();
            timer1.Start();
            timer1.Interval = Convert.ToInt32(textBox1.Text.Trim())*60*1000;
        }

        #region  ie浏览器设置
        public static void SetFeatures(uint ieMode)
        {
            bool flag = LicenseManager.UsageMode > LicenseUsageMode.Runtime;
            if (flag)
            {
                throw new ApplicationException();
            }
            string appName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
            string featureControlRegKey = "HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\";
            Registry.SetValue(featureControlRegKey + "FEATURE_BROWSER_EMULATION", appName, ieMode, RegistryValueKind.DWord);
            Registry.SetValue(featureControlRegKey + "FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", appName, 1, RegistryValueKind.DWord);
        }
        #endregion


        #region 获取iE的cookie
        public static string GetCookies(string url)
        {
            uint datasize = 256U;
            StringBuilder cookieData = new StringBuilder((int)datasize);
            bool flag = !InternetGetCookieEx(url, null, cookieData, ref datasize, 8192, IntPtr.Zero);
            if (flag)
            {
                bool flag2 = datasize < 0U;
                if (flag2)
                {
                    return null;
                }
                cookieData = new StringBuilder((int)datasize);
                bool flag3 = !InternetGetCookieEx(url, null, cookieData, ref datasize, 8192, IntPtr.Zero);
                if (flag3)
                {
                    return null;
                }
            }
            return cookieData.ToString();
        }

        #endregion
        private void 自动获取cookie_Load(object sender, EventArgs e)
        {
           SetFeatures(11000);
            //webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("https://sso.zxxk.com/login?service=https%3A%2F%2Fauth.zxxk.com%2Fuser%2Fcallback%3Fcurl%3Dhttps%3A%2F%2Fwww.zxxk.com%2F");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://sso.zxxk.com/login?service=https%3A%2F%2Fauth.zxxk.com%2Fuser%2Fcallback%3Fcurl%3Dhttps%3A%2F%2Fwww.zxxk.com%2F");

        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void timer1_Tick(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
            writeCookie();
            
            
        }


        public void writeCookie()
        {
            string cookies = GetCookies("https://user.zxxk.com/");
            string[] text = cookies.Split(new string[] { ";" }, StringSplitOptions.None);
            string cookie = "";
            for (int i = 0; i < text.Length; i++)
            {

                if (text[i].Contains("xk.passport.ticket.v2"))
                {
                    cookie = text[i];
                    System.IO.File.WriteAllText(path + "cookie.txt", cookie+";", Encoding.UTF8);
                    return;
                }

            }

            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://user.zxxk.com/");
        }

      
       
      

       
    }
}
