using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 淘宝搜索
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public static string html;
        bool loding = true;
        
        private void Form2_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://user.uu898.com/buyerOrder.aspx");
        }

        private void Button1_Click(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (loding == false)
            {
                MatchCollection ids = Regex.Matches(html, @"订单编号：([\s\S]*?)</span>");
                MatchCollection times = Regex.Matches(html, @"支付时间：([\s\S]*?)</span>");
                MatchCollection fuwuqis = Regex.Matches(html, @"订单编号：([\s\S]*?)</span>");
                //MatchCollection counts = Regex.Matches(html, @"title='([\s\S]*?)元=([\s\S]*?)金");
                MatchCollection prices = Regex.Matches(html, @"title='([\s\S]*?)元=([\s\S]*?)金");
                MatchCollection status = Regex.Matches(html, @"订单状态：([\s\S]*?)</span>");

                for (int j = 0; j < ids.Count; j++)
                {
                    ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                    listViewItem.SubItems.Add(ids[j].Groups[1].Value);
                    listViewItem.SubItems.Add(times[j].Groups[1].Value);
                    listViewItem.SubItems.Add(fuwuqis[j].Groups[1].Value);
                    listViewItem.SubItems.Add(prices[j].Groups[1].Value);
                    listViewItem.SubItems.Add(prices[j].Groups[2].Value);
                    listViewItem.SubItems.Add(status[j].Groups[1].Value);

                }
            }
            else
            {
                MessageBox.Show("请等待网页加载完成....");
            }
        }

        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            html = webBrowser1.DocumentText;  //获取到的html
            loding = false;
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            loding = true; 
        }
    }
}
