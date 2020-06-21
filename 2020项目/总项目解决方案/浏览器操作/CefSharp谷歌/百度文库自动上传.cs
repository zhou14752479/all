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

namespace CefSharp谷歌
{
    public partial class 百度文库自动上传 : Form
    {
        public 百度文库自动上传()
        {
            InitializeComponent();
        }
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






        public ChromiumWebBrowser browser = new ChromiumWebBrowser("https://wenku.baidu.com/");
        private void 百度文库自动上传_Load(object sender, EventArgs e)
        {
            browser.Load("https://wenku.baidu.com/");
            browser.Parent = splitContainer1.Panel1;
            browser.Dock = DockStyle.Fill;

            browser.LifeSpanHandler = new OpenPageSelf();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            browser.GetBrowser().MainFrame.EvaluateScriptAsync("document.getElementById(\"user-bar-uname\").click()");
        }
    }
}
