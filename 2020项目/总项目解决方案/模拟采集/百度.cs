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
using helper;

namespace 模拟采集
{
    public partial class 百度 : Form
    {
        public 百度()
        {
            InitializeComponent();
        }

        private void 百度_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WB_DocumentCompleted);
        }

        public static string html; //网页源码传值
        bool status = false;
        private void WB_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
           

            
            if (webBrowser1.DocumentText.Contains("</html>") || webBrowser1.DocumentText.Contains("</HTML>"))
            {
                
                html = webBrowser1.DocumentText;
                run();
                status = true;
            }
            else
            {
                Application.DoEvents();
            }
        }


        public void run()
        {

            MatchCollection titles = Regex.Matches(html, @"""title"":""([\s\S]*?)""");
            if (titles.Count == 0)
            {
                return;
            }

            for (int i = 0; i < titles.Count; i++)
            {
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                lv1.SubItems.Add(titles[i].Groups[1].Value);

            }


        }
        //site:www.pzboy.com inurl:app
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= 100; i++)
            {
              
                status = false; //这个false很重要，因为浏览器初始化加载会触发run（）,status赋值true。
               
                webBrowser1.Navigate("https://www.baidu.com/s?wd=%E5%AE%BF%E8%BF%81&pn="+i+"0&oq=%E5%AE%BF%E8%BF%81&ie=utf-8");
              
                while (this.status == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }

                Thread.Sleep(1000);

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //System.IO.StreamReader getReader = new System.IO.StreamReader(this.webBrowser1.DocumentStream, System.Text.Encoding.GetEncoding("gb2312"));

            //string html = getReader.ReadToEnd();
           

           
        }
    }
}
