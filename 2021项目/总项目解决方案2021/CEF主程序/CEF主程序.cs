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

        private void CEF主程序_Load(object sender, EventArgs e)
        {
            browser = new ChromiumWebBrowser("https://www.shangxueba.com");
            // Cef.Initialize(new CefSettings());


            Control.CheckForIllegalCrossThreadCalls = false;
            panel1.Controls.Add(browser);

            browser.Dock = DockStyle.Fill;
           
        }




        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            CookieVisitor visitor = new CookieVisitor();
            visitor.SendCookie += visitor_SendCookie;
            ICookieManager cookieManager = browser.GetCookieManager();
            cookieManager.VisitAllCookies(visitor);
        }

        string cookies = "";
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



        private void 获取cookieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CookieVisitor visitor = new CookieVisitor();
            visitor.SendCookie += visitor_SendCookie;
            ICookieManager cookieManager = browser.GetCookieManager();
            cookieManager.VisitAllCookies(visitor);
        }

        private void 获取request参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
    }
}
