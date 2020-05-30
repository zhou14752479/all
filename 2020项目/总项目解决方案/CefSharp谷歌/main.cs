using CefSharp;
using CefSharp.WinForms;
using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CefSharp谷歌
{
    public partial class main : Form
    {
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

       

        public ChromiumWebBrowser browser = new ChromiumWebBrowser("https://www.baidu.com/");




        public main()
        {
            InitializeComponent();
           
        }

        private void Main_Load(object sender, EventArgs e)
        {
            browser.Load("https://www.baidu.com/");
            browser.Parent = this.splitContainer1.Panel2;
            browser.Dock = DockStyle.Fill;
            textBox1.Text = browser.Address;
            //browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(FrameEndFunc);
        }

        private void Button1_Click(object sender, EventArgs e)
        {

           browser.Load(textBox1.Text);


            //textBox1.Text = browser.Address;

        }


        private void FrameEndFunc(object sender, FrameLoadEndEventArgs e)
        {
            


            
            
        }


        

        private void button2_Click(object sender, EventArgs e)
        {
            //无返回值直接调用三者任选其一
            //browser.GetBrowser().MainFrame.EvaluateScriptAsync("test_val=" + new Random().Next().ToString("F")); //设置页面上js的test_val变量为随机数
            //browser.GetBrowser().MainFrame.EvaluateScriptAsync("testArg('123','我是NET' )");//运行页面上js的testArg带参数的方法
            // browser.GetBrowser().MainFrame.EvaluateScriptAsync("alert(document.cookie)");//运行页面上js的test方法或者自己输入JS代码执行


            //有返回值先和上面一样调用一次，然后在读取他的返回值，或者不执行，可以读取页面上自身的js函数返回值
            browser.GetBrowser().MainFrame.EvaluateScriptAsync("function _func(){return document.cookie}");//运行页面上js的test方法
            Task<CefSharp.JavascriptResponse> t = browser.EvaluateScriptAsync("_func()");   
            t.Wait();// 等待js 方法执行完后，获取返回值 t.Result 是 CefSharp.JavascriptResponse 对象t.Result.Result 是一个 object 对象，来自js的 callTest2() 方法的返回值
            if (t.Result.Result != null)
            {
                MessageBox.Show(t.Result.Result.ToString());
                textBox2.Text = t.Result.Result.ToString();
            }




        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
