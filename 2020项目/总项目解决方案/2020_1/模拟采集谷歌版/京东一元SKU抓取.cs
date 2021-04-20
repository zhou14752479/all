using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using System.Threading;
using System.Text.RegularExpressions;

namespace 模拟采集谷歌版
{
    public partial class 京东一元SKU抓取 : Form
    {
        public 京东一元SKU抓取()
        {
            InitializeComponent();
        }
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
        public ChromiumWebBrowser browser = new ChromiumWebBrowser("https://st.jingxi.com/pingou/yifen.shtml?ptag=138631.26.5&trace=&jxsid=16185720957090575811");
        private void 京东一元SKU抓取_Load(object sender, EventArgs e)
        {

            browser.LifeSpanHandler = new OpenPageSelf();   //设置在当前窗口打开
            browser.Load("https://h5.duoduoyoucai.com/lottery/pages/expert/expert");
            browser.Parent = splitContainer1.Panel2;
            browser.Dock = DockStyle.Fill;
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        Thread thread;
        private void button4_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"VoWQXT"))
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
        public void run()
        {
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
                            string html = response.Result.ToString();
                           
                            textBox1.Text = html;
                            MatchCollection skus = Regex.Matches(html, @"<div data-id=""([\s\S]*?)""");
                            MatchCollection prices = Regex.Matches(html, @"taro-text welfare__goods-price-num"">([\s\S]*?)</span>");
                            for (int i = 0; i < skus.Count; i++)
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(skus[i].Groups[1].Value.Trim());
                                lv1.SubItems.Add(prices[i].Groups[1].Value.Trim());
                            }
                        }
                    }
                }
            });


          


        }
        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Contains("http"))
            {
                browser.Load(textBox2.Text);
            }
            else
            {

                browser.Load("https://"+textBox2.Text);
            }
        }
    }
}
