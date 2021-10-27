using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace CEF主程序
{
    public partial class 阿里1688 : Form
    {
        public 阿里1688()
        {
            InitializeComponent();
        }
        ChromiumWebBrowser browser;

        #region   cefsharp在自己窗口打开链接
        //调用 browser.LifeSpanHandler = new OpenPageSelf();
        /// <summary>
        /// 在自己窗口打开链接
        /// </summary>
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
        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        #endregion
        Thread thread;
        bool status = false;

        public void run()

        {
            ArrayList urlList = new ArrayList();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("function tempFunction() {");
            //sb.AppendLine(" return document.body.innerHTML; "); 
            sb.AppendLine(" return document.getElementsByTagName('body')[0].outerHTML; ");
            sb.AppendLine("}");
            sb.AppendLine("tempFunction();");
            var task01 = browser.GetBrowser().GetFrame(browser.GetBrowser().GetFrameNames()[0]).EvaluateScriptAsync(sb.ToString());
            task01.ContinueWith(t =>
            {
                if (!t.IsFaulted)
                {
                    var response = t.Result;
                    if (response.Success == true)
                    {
                        if (response.Result != null)
                        {
                            string resultStr = response.Result.ToString();

                            MatchCollection ids = Regex.Matches(resultStr, @"<div class=""company-name"" title=""([\s\S]*?)""><a href=""([\s\S]*?)""");

                            if (ids.Count == 0)
                            {
                                status = false;
                            }

                            else
                            {
                                for (int j = 0; j < ids.Count; j++)
                                {
                                    string url = ids[j].Groups[2].Value;

                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                    lv1.SubItems.Add(url.Replace("?tracelog=p4p", ""));

                                    //if (!textBox1.Text.Contains(url))
                                    //{
                                    //    textBox1.Text += url + "\r\n";
                                    //}
                                }
                                status = true;
                            }







                        }
                    }
                }
            });

        }



            private void 阿里1688_Load(object sender, EventArgs e)
        {
            browser = new ChromiumWebBrowser("https://www.1688.com");
            Control.CheckForIllegalCrossThreadCalls = false;
            splitContainer1.Panel2.Controls.Add(browser);

            browser.Dock = DockStyle.Fill;
            browser.LifeSpanHandler = new OpenPageSelf();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"Wigbkff"))
            {

                return;
            }



            #endregion
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.ListviewToTxt(listView1, 1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            textBox1.Text = "";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            browser.Refresh();
        }
    }
}
