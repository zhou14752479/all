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
        private void 药师帮_Load(object sender, EventArgs e)
        {
            browser = new ChromiumWebBrowser("https://dian.ysbang.cn/index.html#/indexContent?searchKey=&_t=1633924287163");
            Control.CheckForIllegalCrossThreadCalls = false;
      splitContainer1.Panel2.Controls.Add(browser);

            browser.Dock = DockStyle.Fill;
           browser.FrameLoadEnd += Browser_FrameLoadEnd; //调用加载事件


         
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


            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\cookie.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
            sw.WriteLine(cookies);
            sw.Close();
            fs1.Close();
            sw.Dispose();
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
            yaoshibang_WinFormsRequestHandler winr = new yaoshibang_WinFormsRequestHandler();
            browser.RequestHandler = winr;//request请求的具体实现
            winr.getdata = new yaoshibang_WinFormsRequestHandler.GetData(getdata);

           
        }
    }
}
