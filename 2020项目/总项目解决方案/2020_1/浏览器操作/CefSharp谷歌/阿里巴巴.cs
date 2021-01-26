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

namespace CefSharp谷歌
{
    public partial class 阿里巴巴 : Form
    {
        public 阿里巴巴()
        {
            InitializeComponent();
        }
        public ChromiumWebBrowser browser = new ChromiumWebBrowser("https://www.baidu.com/");
        private void 阿里巴巴_Load(object sender, EventArgs e)
        {
            browser.Load("https://www.1688.com/");
            browser.Parent = this.splitContainer1.Panel2;
            browser.Dock = DockStyle.Fill;
            textBox1.Text = browser.Address;
          
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        string source = "";
         private async void Chrome_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
       {
            source = await browser.GetSourceAsync();
      }


        public void run(object sender, FrameLoadEndEventArgs e)

        {
            browser.GetBrowser().MainFrame.EvaluateScriptAsync("function _func(){var test=document.documentElement.outerHTML; return test}");//运行页面上js的test方法
            Task<CefSharp.JavascriptResponse> t = browser.EvaluateScriptAsync("_func()");
            t.Wait();// 等待js 方法执行完后，获取返回值 t.Result 是 CefSharp.JavascriptResponse 对象t.Result.Result 是一个 object 对象，来自js的 callTest2() 方法的返回值
            if (t.Result.Result != null)
            {

                string html = t.Result.Result.ToString();
                MatchCollection uids = Regex.Matches(html, @"<div class=""company-name([\s\S]*?)href=""([\s\S]*?)""");

                if (uids.Count > 0)
                {
                    for (int i = 0; i < uids.Count; i++)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(uids[i].Groups[2].Value.Replace("?tracelog=p4p", ""));
                    }
                }


            }
        }
    private void button2_Click(object sender, EventArgs e)
        {
           
            browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(run);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.ListViewToCSV(listView1, true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            browser.Load(textBox1.Text);
        }
    }
}
