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
            browser = new ChromiumWebBrowser("https://bj.meituan.com/meishi/pn2/");
            browser.Parent = this.splitContainer1.Panel2;
            browser.Dock = DockStyle.Fill;
            // browser.ExecuteJavaScriptAsync("alert("你好")");//script是String格式的js代码
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            browser.Parent = this.splitContainer1.Panel2;
            browser.Dock = DockStyle.Fill;
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            //browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("document.getElementById('ceshi').click();");
            for (int i = 1; i < 60; i++)
            {
                browser = new ChromiumWebBrowser("https://bj.meituan.com/meishi/pn"+i+"/");
                browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(FrameEndFunc);
                
            }
          
        }

        private void FrameEndFunc(object sender, FrameLoadEndEventArgs e)
        {
            
            this.BeginInvoke(new Action(() => {
                String html = browser.GetSourceAsync().Result;
                textBox1.Text = html;
                MatchCollection titles = Regex.Matches(textBox1.Text, @"address"":""([\s\S]*?)""");

                for (int i = 0; i < titles.Count; i++)
                {
                    textBox2.Text += titles[i].Groups[1].Value + "\r\n";
                }

            }));
        }


      


    }
}
