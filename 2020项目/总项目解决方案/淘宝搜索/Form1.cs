using System;
using System.Collections;
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

namespace 淘宝搜索
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://www.dianping.com/suqian/ch0");
          this.webBrowser1.ScriptErrorsSuppressed = true;  //屏蔽IE脚本弹出错误
        }

        bool loading = true;   //该变量表示网页是否正在加载.

        string html = string.Empty;

  

        public void GetHtml(ArrayList urls)

        {

            webBrowser1.Navigated += new WebBrowserNavigatedEventHandler(browser_Navigated);

            foreach (string url in urls)
            {
                loading = true;  //表示正在加载

                webBrowser1.Navigate(url);
                textBox1.Text = url;

                while (loading)
                {
                    Application.DoEvents();//等待本次加载完毕才执行下次循环.

                }

            }

        }



        void browser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
           html = webBrowser1.DocumentText;  //获取到的html.
            MatchCollection titles = Regex.Matches(html, @"<h4>([\s\S]*?)</h4>");
            textBox2.Text = html;
           
            for (int j = 0; j < titles.Count; j++)
            {
                ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                listViewItem.SubItems.Add(titles[j].Groups[1].Value);

            }
            loading = false;//在加载完成后,将该变量置为false,下一次循环随即开始执行.
        }
        



        private void Button1_Click(object sender, EventArgs e)
        {
            ArrayList urls = new ArrayList();
            for (int i = 1; i < 10; i++)
            {
                urls.Add("http://www.dianping.com/suqian/ch0/p" + i);
            }
            GetHtml(urls);

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("");
        }

        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            
        }
    }
}
