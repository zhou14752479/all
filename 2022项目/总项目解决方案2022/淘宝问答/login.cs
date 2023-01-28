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
using System.Web;
using System.Windows.Forms;

namespace 淘宝问答
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void login_Load(object sender, EventArgs e)
        {
            //清除上次的cookie，相当于无痕模式
            try
            {
                
                Cef.GetGlobalCookieManager().DeleteCookiesAsync();
            }
            catch (Exception ex)
            {

            
            }



            timer1.Start();
            browser = new ChromiumWebBrowser("https://login.taobao.com/member/login.jhtml?style=b2b&css_style=b2b&from=b2b&newMini2=true&full_redirect=true&redirect_url=https%3A%2F%2Fwork.1688.com%2Fhome%2Fseller.htm");

            Control.CheckForIllegalCrossThreadCalls = false;
            this.Controls.Add(browser);

            browser.Dock = DockStyle.Fill;
        }

        public delegate void SendData(string ss,string cookie);
        public static SendData sendData;
        public string cookie2 { get; set; }

        public string name { get; set; }

        public string allcookie { get; set; }

        public void getCookie()
        {

            string ss = allcookie;

            if (ss == "" || ss == null)
                return;


            string[] cookstr = ss.Split(';');
            //Console.WriteLine(ss);
            foreach (string c in cookstr)
            {
                if (c.Trim().StartsWith("cookie2="))
                {
                    cookie2 = c.Trim();

                }
                else if (c.Trim().StartsWith("__cn_logon_id__"))
                {
                    string[] namestr = c.Trim().Split('=');
                    Encoding utf8 = Encoding.UTF8;
                    name = HttpUtility.UrlDecode(namestr[1].ToUpper(), utf8);
                }
            }
        }
        #region cefsharp获取cookie
        //browser.FrameLoadEnd += Browser_FrameLoadEnd;调用加载事件
        string cookies = "";


        private void visitor_SendCookie(CefSharp.Cookie obj)
        {
            //obj.Domain.TrimStart('.') + "^" +
            cookies += obj.Name + "=" + obj.Value + ";";

            //这里是完整的cookie
            //toolStripTextBox1.Text = cookies;
            allcookie = cookies;
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            cookies = ""; //把之前加载的重复的cookie清空
            CookieVisitor visitor = new CookieVisitor();
            visitor.SendCookie += visitor_SendCookie;
            ICookieManager cookieManager = browser.GetCookieManager();
            cookieManager.VisitAllCookies(visitor);

            getCookie();

            if (cookie2 != null && name != null &&allcookie.Contains("_m_h5_tk"))
            {
                //MessageBox.Show(allcookie);
                //timer1.Stop();
                Encoding utf8 = Encoding.UTF8;
                sendData(HttpUtility.UrlDecode(name.ToUpper(), utf8),allcookie);
                this.Close();

                
            }


        }
    }
}
