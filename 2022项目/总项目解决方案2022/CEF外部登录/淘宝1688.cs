using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEF外部登录
{
    public partial class 淘宝1688 : Form
    {
        public 淘宝1688()
        {
            InitializeComponent();
        }

        #region cefsharp获取cookie
        //browser.FrameLoadEnd += Browser_FrameLoadEnd;调用加载事件
        string cookies = "";
     

        private void visitor_SendCookie(CefSharp.Cookie obj)
        {
            //obj.Domain.TrimStart('.') + "^" +
            cookies += obj.Name + "=" + obj.Value + ";";



            //这里是完整的cookie
            toolStripTextBox1.Text = cookies;

            System.IO.File.WriteAllText(path + "cookie.txt", cookies, Encoding.UTF8);
            //this.Hide();
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


        ChromiumWebBrowser browser;

        private void 淘宝1688_Load(object sender, EventArgs e)
        {
            //browser = new ChromiumWebBrowser("https://uuser.zjzwfw.gov.cn/uuuser/doLoginSuccess.do");
            browser = new ChromiumWebBrowser("https://scm.kkday.com/v1/zh-hk/auth/login");
            Control.CheckForIllegalCrossThreadCalls = false;
            panel1.Controls.Add(browser);

            browser.Dock = DockStyle.Fill;
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void 获取cookieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cookies = ""; //把之前加载的重复的cookie清空
            CookieVisitor visitor = new CookieVisitor();
            visitor.SendCookie += visitor_SendCookie;
            ICookieManager cookieManager = browser.GetCookieManager();
            cookieManager.VisitAllCookies(visitor);
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            browser = new ChromiumWebBrowser("https://scm.kkday.com/v1/zh-hk/auth/login");
            //browser.Refresh();
        }
    }
}
