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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEF主程序
{
    public partial class 药师帮 : Form
    {
        public 药师帮()
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





        #endregion

        public void dianji()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("function tempFunction() {");

                sb.AppendLine(" var id1=document.getElementsByClassName('cap4-condition-button cap4-condition-button__filter')[0];id1.click(); ");
                sb.AppendLine(" var id2=document.getElementsByClassName('v-easy-table-checkbox-inner v-easy-table-radio-inner')[0];id2.click(); ");
                sb.AppendLine("}");
                sb.AppendLine("tempFunction();");
                var task01 = browser.GetBrowser().GetFrame(browser.GetBrowser().GetFrameNames()[1]).EvaluateScriptAsync(sb.ToString());
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

                            }
                        }
                    }
                });

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void 药师帮_Load(object sender, EventArgs e)
        {
            browser = new ChromiumWebBrowser("http://60.214.69.236:8888/seeyon/main.do?method=main");
            //browser = new ChromiumWebBrowser("https://passport.vip.com/login?src=https%3A%2F%2Fdetail.vip.com%2Fdetail-1711548730-6919483919008310362.html");
            //browser = new ChromiumWebBrowser("https://ascendex.com/zh-cn/basic/cashtrade-spottrading/usdt/cns");
            //browser = new ChromiumWebBrowser("https://mygiftcard.jd.com/giftcard/myGiftCardInit.action");
            Control.CheckForIllegalCrossThreadCalls = false;
      splitContainer1.Panel2.Controls.Add(browser);

            browser.Dock = DockStyle.Fill;
           browser.FrameLoadEnd += Browser_FrameLoadEnd; //调用加载事件
            browser.LifeSpanHandler = new OpenPageSelf();


        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            cookies = ""; //把之前加载的重复的cookie清空
            CookieVisitor visitor = new CookieVisitor();
            visitor.SendCookie += visitor_SendCookie;
            ICookieManager cookieManager = browser.GetCookieManager();
            cookieManager.VisitAllCookies(visitor);


            Control.CheckForIllegalCrossThreadCalls = true;
        


        }
        public void getdata(string url)
        {
            // MessageBox.Show(url);
            string ex1 = Regex.Match(url,@"""ex1"":""([\s\S]*?)""").Groups[1].Value;
            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\ex1.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
            sw.WriteLine(ex1);
            sw.Close();
            fs1.Close();
            sw.Dispose();
        }


            #region cefsharp获取cookie
            //browser.FrameLoadEnd += Browser_FrameLoadEnd;调用加载事件
            string cookies = "";
        private void visitor_SendCookie(CefSharp.Cookie obj)
        {
            //obj.Domain.TrimStart('.') + "^" +
            cookies += obj.Name + "=" + obj.Value + ";";


            //FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\cookie.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
            //StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
            //sw.WriteLine(cookies);
            //sw.Close();
            //fs1.Close();
            //sw.Dispose();

           
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

        private void button1_Click(object sender, EventArgs e)
        {
            dianji();
            
            //if(!cookies.Contains("thor"))
            //{

            //    MessageBox.Show("未登录");
            //    return;
            //}
            //else
            //{
            //    string thor = Regex.Match(cookies,@"thor=([\s\S]*?);").Groups[1].Value;
            //    thor = "thor="+thor+";";
            //    京东E卡查询.cookie = thor;
            //}


          

            //this.Hide();

        }



    }
}
