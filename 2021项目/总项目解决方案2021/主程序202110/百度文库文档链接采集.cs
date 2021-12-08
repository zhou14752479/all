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
using myDLL;

namespace 主程序202110
{
    public partial class 百度文库文档链接采集 : Form
    {
        public 百度文库文档链接采集()
        {
            InitializeComponent();
        }

        public void run()
        {
            for (int i = 1; i < 9999; i++)
            {
                string url = "https://cuttlefish.baidu.com/search/interface/shopsearch?shop_id="+textBox1.Text.Trim()+"&resource_type=1&sell_type=2&pn="+i+"&rn=50&sort_type=5";
               
                string html = method.GetUrlWithCookie(url,textBox2.Text, "utf-8");
              
                MatchCollection titles = Regex.Matches(html, @"""sourceId"":""([\s\S]*?)""");
                if (titles.Count == 0)
                    return;
                for (int j= 0; j < titles.Count; j++)
                {
                    ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                    listViewItem.SubItems.Add("https://wenku.baidu.com/view/"+ titles[j].Groups[1].Value + "?fr=shopSearch-pc");
                }
            }
        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
