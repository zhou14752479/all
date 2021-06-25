using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 模拟采集谷歌版
{
    public partial class 阿里代销 : Form
    {
        public 阿里代销()
        {
            InitializeComponent();
           //browser.BrowserSettings.ImageLoading = CefState.Disabled;  //不加载图片
          //  browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(WB_DocumentCompleted);
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

        int count = 0;

        public void nextUrl()
        {
           
            if (count < listView1.Items.Count)
            {
                
                browser.Load(listView1.Items[count].SubItems[0].Text);
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    listView1.Items[i].BackColor = Color.White;
                }
                listView1.Items[count].BackColor = Color.Red;
                count = count + 1;
            }
        }

        private void WB_DocumentCompleted(object sender, FrameLoadEndEventArgs e)
        {

            if (e.Url.ToString() != browser.Address.ToString())
                return;
            run();


        }

        public void run()
        {
            try
            {
             
                 browser.GetBrowser().MainFrame.EvaluateScriptAsync("window.scrollBy(0,3000)");//运行页面上js的test方法或者自己输入JS代码执行

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString()); ;
            }
        }
        Thread thread;
       
        public ChromiumWebBrowser browser = new ChromiumWebBrowser("https://login.1688.com/member/signin.htm");
        private void 阿里代销_Load(object sender, EventArgs e)
        {
           
        // browser.Load("https://login.1688.com/member/signin.htm");
            browser.Parent = splitContainer1.Panel1;
            browser.Dock = DockStyle.Fill;
            Control.CheckForIllegalCrossThreadCalls = false;
            browser.LifeSpanHandler = new OpenPageSelf();   //设置在当前窗口打开
        }

    
   
        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(nextUrl);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }

            }
        }

        private void 粘贴网址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
            string[] text = Clipboard.GetText().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                ListViewItem lv1 = listView1.Items.Add(text[i]);
            }
           
        }
    }
}
