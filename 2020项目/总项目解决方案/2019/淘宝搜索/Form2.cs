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
        public static string COOKIE="";
        private void Form2_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://user.uu898.com/buyerOrder.aspx");
        }

        private void Button1_Click(object sender, EventArgs e)
        {

        }

        public void run()

        {
            if (loding == false)
            {
                COOKIE = method.GetCookies("https://user.uu898.com/buyerOrder.aspx");

                MatchCollection ids = Regex.Matches(html, @"data-no='([\s\S]*?)'");
                MatchCollection times = Regex.Matches(html, @"支付时间：([\s\S]*?)</span>");
                MatchCollection fuwuqis = Regex.Matches(html, @"魔兽世界([\s\S]*?)</p>");
                MatchCollection jinbis = Regex.Matches(html, @"title='([\s\S]*?)元=([\s\S]*?)金");
                MatchCollection prices = Regex.Matches(html, @"<p style=""line-height: 40px;"">([\s\S]*?)</p>");
                MatchCollection status = Regex.Matches(html, @"订单状态：([\s\S]*?)</span>");

                MatchCollection jines = Regex.Matches(html, @"padding-top:10px"">([\s\S]*?)</p>");

                for (int j = 0; j < ids.Count; j++)
                {
                    string ahtml = method.GetUrlWithCookie("https://www.uu898.com/ordertracking.aspx?ID=" + ids[j].Groups[1].Value, COOKIE, "utf-8");
                    Match juese = Regex.Match(ahtml, @"收货角色名：([\s\S]*?)</li>");


                    string jinbi = (Convert.ToDecimal(jinbis[j].Groups[2].Value) * Convert.ToInt32(prices[(2 * j + 1)].Groups[1].Value)).ToString();

                    ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                    listViewItem.SubItems.Add(ids[j].Groups[1].Value);
                    listViewItem.SubItems.Add(times[j].Groups[1].Value);
                    listViewItem.SubItems.Add("魔兽世界" + fuwuqis[j].Groups[1].Value);
                    listViewItem.SubItems.Add(jinbi);
                    listViewItem.SubItems.Add(jines[j].Groups[1].Value.Trim());
                    listViewItem.SubItems.Add(status[j].Groups[1].Value);
                    listViewItem.SubItems.Add(juese.Groups[1].Value.Trim());
                    if (listView1.Items.Count > 2)
                    {
                        listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                    }
                    Thread.Sleep(1000);
                }

                MessageBox.Show("完成");
            }
            else
            {
                MessageBox.Show("请等待网页加载完成....");
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;



        }

        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            html = webBrowser1.DocumentText;  //获取到的html
            loding = false;
        }

      

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            HtmlDocument dc = webBrowser1.Document;
            HtmlElementCollection es = dc.GetElementsByTagName("input");   //GetElementsByTagName返回集合
            foreach (HtmlElement e1 in es)
            {
                if (e1.GetAttribute("name") == "UserName")
                {
                    e1.SetAttribute("value", textBox2.Text.Trim());
                }
                if (e1.GetAttribute("name") == "PassWord")
                {
                    e1.SetAttribute("value", textBox3.Text.Trim());
                }
            }
        }
    }
}
