using Microsoft.Win32;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 订单流水
{
    public partial class Form1 : Form
    {




        public Form1()
        {
            InitializeComponent();
        }
       
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = "application/x-www-form-urlencoded";
            //  request.ContentType = "application/json";
            request.ContentLength = postData.Length;
            //request.AllowAutoRedirect = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
            request.Headers.Add("Cookie", COOKIE);
            request.Referer = "https://up.95516.com/unionadmin/system/index";
            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(postData);
            sw.Flush();
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
            response.GetResponseHeader("Set-Cookie");
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
            string html = reader.ReadToEnd();
            reader.Close();
            response.Close();
            return html;
        }

        string cookie = "";
        public void run()

        {
            string start = DateTime.Now.ToString();

          

            string html = PostUrl("https://up.95516.com/unionadmin/tra/order/batchPayOrder/datagrid.json", "loginOrgId=QRA307256511I0H&orgType=12&multiSelect=false&startTime=2020-02-19++00%3A00%3A00&endTime=2020-02-19++23%3A59%3A59&feeType=CNY&page=1&rows=10", cookie, "utf-8");
           MatchCollection a1s = Regex.Matches(html, @"""tradeTime"":""([\s\S]*?)""");
            MatchCollection a2s = Regex.Matches(html, @"""outTradeNo"":""([\s\S]*?)""");
            MatchCollection a3s = Regex.Matches(html, @"""orderNo"":""([\s\S]*?)""");
            MatchCollection a4s = Regex.Matches(html, @"""agentid"":""([\s\S]*?)""");
            MatchCollection a5s = Regex.Matches(html, @"""mchNo"":""([\s\S]*?)""");
            MatchCollection a6s = Regex.Matches(html, @"""mchName"":""([\s\S]*?)""");
            MatchCollection a7s = Regex.Matches(html, @"""payTypeName"":""([\s\S]*?)""");
            MatchCollection a8s = Regex.Matches(html, @"""displayTradeStateName"":""([\s\S]*?)""");
            MatchCollection a9s = Regex.Matches(html, @"""totalFee"":([\s\S]*?),");
            MatchCollection a10s = Regex.Matches(html, @"""startMoney"":([\s\S]*?),");
            MatchCollection a11s = Regex.Matches(html, @"""money"":([\s\S]*?),");
            MatchCollection a12s = Regex.Matches(html, @"""refundMoney"":([\s\S]*?),");
            MatchCollection a13s = Regex.Matches(html, @"""bankType"":""([\s\S]*?)""");
            MatchCollection a14s = Regex.Matches(html, @"""bankTypeName"":""([\s\S]*?)""");
            MatchCollection a15s = Regex.Matches(html, @"""feeTypeName"":""([\s\S]*?)""");


            for (int i = 0; i <a1s.Count; i++)
            {
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                lv1.SubItems.Add(a1s[i].Groups[1].Value);
                lv1.SubItems.Add(a2s[i].Groups[1].Value);
                lv1.SubItems.Add(a3s[i].Groups[1].Value);
                lv1.SubItems.Add(a4s[i].Groups[1].Value);
                lv1.SubItems.Add(a5s[i].Groups[1].Value);
                lv1.SubItems.Add(a6s[i].Groups[1].Value);
                lv1.SubItems.Add(a7s[i].Groups[1].Value);
                lv1.SubItems.Add(a8s[i].Groups[1].Value);
                lv1.SubItems.Add(a9s[i].Groups[1].Value);
                lv1.SubItems.Add(a10s[i].Groups[1].Value);
                lv1.SubItems.Add(a11s[i].Groups[1].Value);
                lv1.SubItems.Add(a12s[i].Groups[1].Value);
                lv1.SubItems.Add(a13s[i].Groups[1].Value);
                lv1.SubItems.Add(a14s[i].Groups[1].Value);
                lv1.SubItems.Add(a15s[i].Groups[1].Value);
            }
           
        }

        /// <summary>  
        /// 修改注册表信息来兼容当前程序
        /// </summary>  
        public static void SetWebBrowserFeatures(IeVersion ieVersion)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime) return;
            //获取程序及名称  
            string AppName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            //得到浏览器的模式的值  
            UInt32 ieMode = GeoEmulationModee((int)ieVersion);

            string featureControlRegKey = @"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\";
            //设置浏览器对应用程序（appName）以什么模式（ieMode）运行  

            Registry.SetValue(featureControlRegKey + "FEATURE_BROWSER_EMULATION", AppName, ieMode, RegistryValueKind.DWord);

            Registry.SetValue(featureControlRegKey + "FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", AppName, 1, RegistryValueKind.DWord);
            Registry.SetValue(featureControlRegKey + "FEATURE_AJAX_CONNECTIONEVENTS", AppName, 1, RegistryValueKind.DWord);
            Registry.SetValue(featureControlRegKey + "FEATURE_GPU_RENDERING", AppName, 1, RegistryValueKind.DWord);
            Registry.SetValue(featureControlRegKey + "FEATURE_WEBOC_DOCUMENT_ZOOM", AppName, 1, RegistryValueKind.DWord);
            Registry.SetValue(featureControlRegKey + "FEATURE_NINPUT_LEGACYMODE", AppName, 0, RegistryValueKind.DWord);
        }

        /// <summary>  
        /// 通过版本得到浏览器模式的值  
        /// </summary>  
        /// <param name="browserVersion"></param>  
        /// <returns></returns>  
        private static UInt32 GeoEmulationModee(int browserVersion)
        {
            UInt32 mode = 11000; // Internet Explorer 11. Webpages containing standards-based !DOCTYPE directives are displayed in IE11 Standards mode.   
            switch (browserVersion)
            {
                case 7:
                    mode = 7000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE7 Standards mode.   
                    break;
                case 8:
                    mode = 8000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE8 mode.   
                    break;
                case 9:
                    mode = 9000; // Internet Explorer 9. Webpages containing standards-based !DOCTYPE directives are displayed in IE9 mode.                      
                    break;
                case 10:
                    mode = 10000; // Internet Explorer 10.  
                    break;
                case 11:
                    mode = 11000; // Internet Explorer 11  
                    break;
            }
            return mode;
        }
        public enum IeVersion
        {
            IE7 = 7,
            IE8 = 8,
            IE9 = 9,
            IE10 = 10,
            IE11 = 11
        };
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
        private void Form1_Load(object sender, EventArgs e)
        {
           



            SetWebBrowserFeatures(IeVersion.IE10);
            webBrowser1.Url = new Uri("https://up.95516.com/unionadmin/");
            
            webBrowser1.ScriptErrorsSuppressed = true;
            timer1.Interval = Convert.ToInt32(textBox1.Text) * 1000;
        }

        private void button1_Click(object sender, EventArgs e)
        {
          cookie=  GetCookies("https://up.95516.com/unionadmin/system/index");
            run();


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            run();

        }
    }
}
