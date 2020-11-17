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

namespace 常用软件非客户
{
    public partial class 文库举报 : Form
    {
        public 文库举报()
        {
            InitializeComponent();
        }
        public void run()
        {
            for (int i = 1; i < 99999; i++)
            {

                string url = "https://wenku.baidu.com/user/interface/getpgcpublicdoclist?range=0&order=0&pn=" + i + "&uname=" + textBox1.Text.Trim() + "&uid=" + textBox2.Text.Trim() + "&rn=16&sep=0&_=1600822746158";
                string html = method.GetUrl(url, "utf-8");

                MatchCollection docids = Regex.Matches(html, @"""doc_id"":""([\s\S]*?)""");
                MatchCollection names = Regex.Matches(html, @"""title"":""([\s\S]*?)""");

                for (int j = 0; j < docids.Count; j++)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                    lv1.SubItems.Add(names[j].Groups[1].Value);
                    string jubaotxt = jubao(docids[j].Groups[1].Value);
                    lv1.SubItems.Add(jubaotxt);
                    Thread.Sleep(1000);
                }

            }



        }

        public string jubao(string docid)
        {
            string url = "https://wenku.baidu.com/report/submit/reportSubmit";
            string postdata = "doc_id="+docid+"&ct=20116&report_reason=1";
            string html = method.PostUrl(url,postdata,textBox3.Text.Trim(),"utf-8");
            return html;
        }
        private void 文库举报_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }
    }
}
