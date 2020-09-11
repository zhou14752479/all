using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using helper;

namespace 模拟采集谷歌版
{
    public partial class 拼多多后台 : Form
    {
        public 拼多多后台()
        {
            InitializeComponent();
        }
        string cookie = "";

        internal class OpenPageSelf : ILifeSpanHandler
        {
            public bool DoClose(IWebBrowser browserControl, IBrowser browser)
            {
                return false;
            }

            public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
            {

            }

            public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
            {

            }

            public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl,
    string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures,
    IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
            {
                newBrowser = null;
                var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;
                chromiumWebBrowser.Load(targetUrl);
                return true; //Return true to cancel the popup creation copyright by codebye.com.
            }
        }


        #region POST请求

        public string PostUrl(string url, string postData)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.Headers.Add("Cookie", cookie);
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Referer = "http://czt.sc.gov.cn/kj/toPage?toPage=cms/isreal";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                MessageBox.Show(ex.ToString());
                return "";
            }


        }

        #endregion



        public ChromiumWebBrowser browser = new ChromiumWebBrowser("https://mms.pinduoduo.com/login");

      


        private void 拼多多后台_Load(object sender, EventArgs e)
        {
            browser.LifeSpanHandler = new OpenPageSelf();   //设置在当前窗口打开
            browser.Load("https://mms.pinduoduo.com/login");
            browser.Parent = this.splitContainer1.Panel2;
            browser.Dock = DockStyle.Fill;
            textBox1.Text = browser.Address;
           // browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(run);
        }
        string source = "";

        //private async void Chrome_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        //{

        //    source = await browser.GetSourceAsync();
        //}
        public void run()

        {
            browser.GetBrowser().MainFrame.EvaluateScriptAsync("function _func(){var test=document.documentElement.outerHTML; return test}");//运行页面上js的test方法
            Task<CefSharp.JavascriptResponse> t = browser.EvaluateScriptAsync("_func()");
            t.Wait();// 等待js 方法执行完后，获取返回值 t.Result 是 CefSharp.JavascriptResponse 对象t.Result.Result 是一个 object 对象，来自js的 callTest2() 方法的返回值
            if (t.Result.Result != null)
            {

                string html = t.Result.Result.ToString();
                textBox2.Text = html;

               


            }
        }
        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            run();
        }
    }
}
