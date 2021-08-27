using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace CEF主程序
{
    public partial class CEF主程序 : Form
    {
        public CEF主程序()
        {
            InitializeComponent();
        }
        ChromiumWebBrowser browser ;

       public static string body = "";
        private void CEF主程序_Load(object sender, EventArgs e)
        {
            browser = new ChromiumWebBrowser("http://q.cy1788.com/app/superscanPH/loginPHValidate.jsp");
            // Cef.Initialize(new CefSettings());
            

            Control.CheckForIllegalCrossThreadCalls = false;
            panel1.Controls.Add(browser);

            browser.Dock = DockStyle.Fill;

           browser.FrameLoadEnd += Browser_FrameLoadEnd;
           browser.RequestHandler = new WinFormsRequestHandler();//request请求的具体实现
          

        }
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




        #endregion

        #region cefsharp获取cookie
        //browser.FrameLoadEnd += Browser_FrameLoadEnd;调用加载事件
        string cookies = "";
        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            cookies = ""; //把之前加载的重复的cookie清空
            CookieVisitor visitor = new CookieVisitor();
            visitor.SendCookie += visitor_SendCookie;
            ICookieManager cookieManager = browser.GetCookieManager();
            cookieManager.VisitAllCookies(visitor);

           
        }
        
        private void visitor_SendCookie(CefSharp.Cookie obj)
        {
            //obj.Domain.TrimStart('.') + "^" +
             cookies += obj.Name + "=" + obj.Value + ";";

            toolStripTextBox1.Text = cookies;
        }


        public class CookieVisitor : CefSharp.ICookieVisitor
        {
            public event Action<CefSharp.Cookie> SendCookie;
            public void Dispose()
            {

            }

            public bool Visit(Cookie cookie, int count, int total, ref bool deleteCookie)
            {
                deleteCookie = false;
                if (SendCookie != null)
                {
                    SendCookie(cookie);
                }

                return true;

            }

        }

        #endregion

        private void 获取cookieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CookieVisitor visitor = new CookieVisitor();
            visitor.SendCookie += visitor_SendCookie;
            ICookieManager cookieManager = browser.GetCookieManager();
            cookieManager.VisitAllCookies(visitor);
        }

        private void 获取request参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripTextBox1.Text = body;
        }

        private void 前进ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
