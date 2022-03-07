using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 关键词挖掘360
{
    public partial class 关键词挖掘360 : Form
    {
        public 关键词挖掘360()
        {
            InitializeComponent();
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
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
        #region 修改注册表信息使WebBrowser使用指定版本IE内核 传入11000是IE11
        public static void SetFeatures(UInt32 ieMode)
        {
            //传入11000是IE11, 9000是IE9, 只不过当试着传入6000时, 理应是IE6, 可实际却是Edge, 这时进一步测试, 当传入除IE现有版本以外的一些数值时WebBrowser都使用Edge内核
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
            {
                throw new ApplicationException();
            }
            //获取程序及名称
            string appName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            string featureControlRegKey = "HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\";
            //设置浏览器对应用程序(appName)以什么模式(ieMode)运行
            Registry.SetValue(featureControlRegKey + "FEATURE_BROWSER_EMULATION", appName, ieMode, RegistryValueKind.DWord);
            //不晓得设置有什么用
            Registry.SetValue(featureControlRegKey + "FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", appName, 1, RegistryValueKind.DWord);
        }
        #endregion

        bool shuju = true;  //判断页码是否有数据
        bool status = false;
        public static string html; //网页源码传值
        private void WB_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //if (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
            //    return;
            //if (e.Url.ToString() != webBrowser1.Url.ToString())
            //    return;

            if (webBrowser1.DocumentText.Contains("</html>"))
            {

                html = webBrowser1.DocumentText;

                run();
                status = true;
            }
            else
            {
                Application.DoEvents();
            }
        }

        public void main()
        {
           

            string[] keywords = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string keyword in keywords)
            {

                    if (shuju == false)
                    {
                        return;
                    }
                   
                    status = false;
                    webBrowser1.Navigate("https://m.so.com/s?q=" + System.Web.HttpUtility.UrlEncode(keyword) + "&src=suggest_msearch&sug_pos=0&sug=&nlpv=&ssid=&srcg=home_next");

                    while (this.status == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                  
                    Thread.Sleep(3000);
                
            }

           
            MessageBox.Show("查询结束");

        }

        List<string> lists = new List<string>();
        public void run()
        {

            MatchCollection keys1 = Regex.Matches(html, @"class=""rec-item"" data-index=""([\s\S]*?)"">([\s\S]*?)</a>");
            MatchCollection keys2 = Regex.Matches(html, @"shbt_private"">([\s\S]*?)</a>");
            for (int i = 0; i < keys1.Count; i++)
            {
                if (!lists.Contains(keys1[i].Groups[2].Value))
                {
                    if (!textBox2.Text.Contains(keys1[i].Groups[2].Value))
                    {
                        lists.Add(keys1[i].Groups[2].Value);
                        resultxt.Text += keys1[i].Groups[2].Value + "\r\n";
                    }
                }

            }
            for (int i = 0; i < keys2.Count; i++)
            {
                if (!lists.Contains(keys2[i].Groups[1].Value))
                {
                    if (!textBox2.Text.Contains(keys2[i].Groups[1].Value))
                    {
                        lists.Add(keys2[i].Groups[1].Value);
                        resultxt.Text += keys2[i].Groups[1].Value + "\r\n";
                    }
                }

            }
            if (status == false)
            {
                return;
            }

        }
        private void 关键词挖掘360_Load(object sender, EventArgs e)
        {
            #region 通用检测


            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"18581289009"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion
            SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WB_DocumentCompleted);
            webBrowser1.Navigate("https://m.so.com/");
        }


        Thread thread;
        private void Button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(main);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
