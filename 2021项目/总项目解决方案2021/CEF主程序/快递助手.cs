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

namespace CEF主程序
{
    public partial class 快递助手 : Form
    {
        public 快递助手()
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


        private void 快递助手_Load(object sender, EventArgs e)
        {
            browser = new ChromiumWebBrowser("https://www.baidu.com");
            Control.CheckForIllegalCrossThreadCalls = false;
            splitContainer1.Panel1.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
            browser.LifeSpanHandler = new OpenPageSelf();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }
    }
}
