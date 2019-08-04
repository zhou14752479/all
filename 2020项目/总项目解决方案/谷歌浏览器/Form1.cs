using CefSharp;
using CefSharp.WinForms;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 谷歌浏览器
{
    public partial class Form1 : Form
    {
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



        public ChromiumWebBrowser browser;

        public Form1()
        {
            InitializeComponent();
            InitBrowser();
            browser.LifeSpanHandler = new OpenPageSelf();   //设置在当前窗口打开


        }
        public void InitBrowser()
        {
            Cef.Initialize(new CefSettings());
            browser = new ChromiumWebBrowser("https://www.taobao.com");
            browser.Parent = this.splitContainer1.Panel2;
            browser.Dock = DockStyle.Fill;
            // browser.ExecuteJavaScriptAsync("alert("你好")");//script是String格式的js代码
        }
        private void Form1_Load(object sender, EventArgs e)
        {
       
            browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(FrameEndFunc);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(taobao));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
            //browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("document.getElementById('ceshi').click();");
            //for (int i = 1; i < 60; i++)
            //{
            //    browser = new ChromiumWebBrowser("https://bj.meituan.com/meishi/pn"+i+"/");
            //    browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(FrameEndFunc);

            //}

        }
        #region 下载文件
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="URLAddress">图片地址</param>
        /// <param name="subPath">图片所在文件夹</param>
        /// <param name="name">图片名称</param>
        public static void downloadFile(string URLAddress, string subPath, string name)
        {
            string path = System.IO.Directory.GetCurrentDirectory();

            WebClient client = new WebClient();

            if (false == System.IO.Directory.Exists(subPath))
            {
                //创建pic文件夹
                System.IO.Directory.CreateDirectory(subPath);
            }
            client.DownloadFile(URLAddress, subPath + "\\" + name);
        }
        #region GET请求带COOKIE
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlWithCookie(string Url, string COOKIE, string charset)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.108 Safari/537.36";

                request.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                ex.ToString();



            }
            return "";
        }
        #endregion
        public static string COOKIE;
        #endregion
        public void taobao()
        {
            try
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("请打开网址");
                    return;

                }
                

                string html = GetUrlWithCookie(textBox1.Text, COOKIE, "gb2312");

                Match title = Regex.Match(html, @"<title>([\s\S]*?)</title>");  //主图
                MatchCollection zhupics = Regex.Matches(html, @"<a href=""#""><img([\s\S]*?)src=""([\s\S]*?)jpg");  //主图
                MatchCollection prices = Regex.Matches(html, @"""price"":""([\s\S]*?)""");  //价格


                // Match  aURL = Regex.Match(html, @"descnew([\s\S]*?),");  //详情图来源网址
                string path = AppDomain.CurrentDomain.BaseDirectory + title.Groups[1].Value + "\\";

                string subPath = path + "产品介绍图\\";
                //string ahtml = method.GetUrl("http://descnew"+aURL.Groups[1].Value.Replace("'","").Replace("\"",""), "gb2312");
                //MatchCollection xqpics = Regex.Matches(ahtml, @"<img src=""([\s\S]*?)""");  //详情图


                if (false == System.IO.Directory.Exists(path))
                {
                    //创建pic文件夹
                    System.IO.Directory.CreateDirectory(path);

                    System.IO.Directory.CreateDirectory(subPath);
                }
                if (prices.Count != 0)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int a = 0; a < prices.Count; a++)
                    {
                        sb.Append(prices[a].Groups[1].Value + "   ");
                    }

                    FileStream fs2 = new FileStream(path + "价格_" + prices[0].Groups[1].Value + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw2 = new StreamWriter(fs2);
                    sw2.WriteLine(sb.ToString());
                    sw2.Close();
                    fs2.Close();
                }

                else if (prices.Count == 0)
                {
                    Match price = Regex.Match(html, @"<em class=""tb-rmb-num"">([\s\S]*?)<");  //价格

                    FileStream fs2 = new FileStream(path + "价格_" + price.Groups[1].Value + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw2 = new StreamWriter(fs2);
                    sw2.WriteLine(price.Groups[1].Value);
                    sw2.Close();
                    fs2.Close();
                }

                FileStream fs1 = new FileStream(path + "网址.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1);
                sw.WriteLine(textBox1.Text);
                sw.Close();
                fs1.Close();



                for (int i = 0; i < zhupics.Count; i++)
                {

                    downloadFile("http:" + zhupics[i].Groups[2].Value.Replace("https:", "") + "jpg", subPath, i + ".jpg");
                    textBox1.Text = "正在下载主图" + i + "\r\n";
                }

                //for (int j = 0; j < xqpics.Count; j++)
                //{

                //    method.downloadFile(xqpics[j].Groups[1].Value , subPath, j + ".jpg");
                //    textBox2.Text = "正在下载详情图" + j + "\r\n";
                //}
                MessageBox.Show("下载完成");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void FrameEndFunc(object sender, FrameLoadEndEventArgs e)
        {
            
            this.BeginInvoke(new Action(() => {
                String html = browser.GetSourceAsync().Result;
                //textBox1.Text = html;

                textBox1.Text = browser.Address;
            }));
        }


      


    }
}
