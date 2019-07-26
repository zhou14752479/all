using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 谷歌浏览器
{
    public partial class Form1 : Form
    {
        public ChromiumWebBrowser browser;

        public Form1()
        {
            InitializeComponent();
            InitBrowser();


        }
        public void InitBrowser()
        {
            Cef.Initialize(new CefSettings());
            browser = new ChromiumWebBrowser("https://s.taobao.com/search?initiative_id=tbindexz_20170306&ie=utf8&spm=a21bo.2017.201856-taobao-item.2&sourceId=tb.index&search_type=item&ssid=s5-e&commend=all&imgfile=&q=%E6%A3%AE%E6%B5%B7%E5%A1%9E%E5%B0%94&suggest=0_1&_input_charset=utf-8&wq=%E6%A3%AE%E6%B5%B7&suggest_query=%E6%A3%AE%E6%B5%B7&source=suggest");
            browser.Parent = this.splitContainer1.Panel2;
            browser.Dock = DockStyle.Fill;
            // browser.ExecuteJavaScriptAsync("alert("你好")");//script是String格式的js代码
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            browser.Parent = this.splitContainer1.Panel2;
            browser.Dock = DockStyle.Fill;
            browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(FrameEndFunc);
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            //browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("document.getElementById('ceshi').click();");
            //for (int i = 1; i < 60; i++)
            //{
            //    browser = new ChromiumWebBrowser("https://bj.meituan.com/meishi/pn"+i+"/");
            //    browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(FrameEndFunc);
                
            //}
          
        }

        private void FrameEndFunc(object sender, FrameLoadEndEventArgs e)
        {
            
            this.BeginInvoke(new Action(() => {
                String html = browser.GetSourceAsync().Result;
                textBox1.Text = html;
               

            }));
        }


      


    }
}
