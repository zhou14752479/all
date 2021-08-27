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

namespace 谷歌地图采集器
{
    public partial class 浏览器界面 : Form
    {
        public 浏览器界面()
        {
            InitializeComponent();
        }
        ChromiumWebBrowser browser;
       

        private void 浏览器界面_Load(object sender, EventArgs e)
        {
            browser = new ChromiumWebBrowser("https://www.google.com/maps/?hl=zh-cn");
            Control.CheckForIllegalCrossThreadCalls = false;
            splitContainer1.Panel2.Controls.Add(browser);

            browser.Dock = DockStyle.Fill;
            //browser.RequestHandler = new WinFormsRequestHandler();//request请求的具体实现
            browser.FrameLoadEnd += Browser_FrameLoadEnd;
        }

        public static string json;
        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("function tempFunction() {");
            //sb.AppendLine(" return document.body.innerHTML; "); 
            sb.AppendLine(" return document.getElementsByTagName('body')[0].outerHTML; ");
            sb.AppendLine("}");
            sb.AppendLine("tempFunction();");
            var task01 = browser.GetBrowser().GetFrame(browser.GetBrowser().GetFrameNames()[0]).EvaluateScriptAsync(sb.ToString());
            task01.ContinueWith(t =>
            {
                if (!t.IsFaulted)
                {
                    var response = t.Result;
                    if (response.Result != null)
                    {
                        string resultStr = response.Result.ToString();
                        dorun(resultStr);
                    }


                }
            });
          

        }



        public delegate void DoRun(string json);
        public event DoRun dorun;
        
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            使用说明 shi = new 使用说明();
            shi.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            browser.Load("https://www.google.com/maps/?hl=zh-cn");
        }

       
    }
}
