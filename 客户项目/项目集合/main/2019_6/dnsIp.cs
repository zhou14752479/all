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

namespace main._2019_6
{
    public partial class dnsIp : Form
    {
        private void DnsIp_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        }
        public dnsIp()
        {
            InitializeComponent();
        }

        public void run()
        {

            for (int i = 1; i <99; i++)
            {

                string URL = "https://dns.aizhan.com/" + textBox1.Text + "/" + i + "/";

                string html = method.GetUrl(URL, "utf-8");

                MatchCollection  urls = Regex.Matches(html, @"rel=""nofollow"" target=""_blank"">([\s\S]*?)</a>");
                MatchCollection names = Regex.Matches(html, @"rel=""nofollow"" target=""_blank"">([\s\S]*?)<td class=""title"">([\s\S]*?)</td>");

                if (urls.Count == 0)
                    break;

                for (int j= 0; j< urls.Count ; j++)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString()); //使用Listview展示数据         
                    lv1.SubItems.Add(urls[j].Groups[1].Value);
                    lv1.SubItems.Add(names[j].Groups[2].Value.Replace("<span>","").Replace("</span>","").Trim());
                    Thread.Sleep(200);
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
