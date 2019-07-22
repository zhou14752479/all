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
            browser = new ChromiumWebBrowser("https://item.taobao.com/item.htm?spm=a230r.1.14.22.24cf119a70FFdo&id=592957367498&ns=1&abbucket=5#detail");
            browser.Parent = this.splitContainer1.Panel2;
            browser.Dock = DockStyle.Fill;
            // browser.ExecuteJavaScriptAsync("alert("你好")");//script是String格式的js代码
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {

            //browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("document.getElementById('ceshi').click();");
            browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(FrameEndFunc);
        }

        private void FrameEndFunc(object sender, FrameLoadEndEventArgs e)
        {
            MessageBox.Show("加载完毕");
            this.BeginInvoke(new Action(() => {
                String html = browser.GetSourceAsync().Result;
                textBox1.Text = html;
            }));
        }


      


    }
}
