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
        /// <summary>
        /// 旁站查询
        /// </summary>
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


        /// <summary>
        /// C段查询
        /// </summary>
        public void run1()
        {
            try
            {
                string[] duans = textBox1.Text.Split(new string[] { "." }, StringSplitOptions.None);
                for (int a = 1; a < 256; a++)
                {
                    ListViewItem lv2 = listView2.Items.Add(duans[0] + "." + duans[1] + "." + duans[2] + "." + a); //使用Listview展示数据         

                }

                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    label7.Text = "正在查询" + listView2.Items[i].SubItems[0].Text;
                    string URL = "https://dns.aizhan.com/" + listView2.Items[i].SubItems[0].Text + "/" + i + "/";

                    string html = method.GetUrl(URL, "utf-8");

                    MatchCollection urls = Regex.Matches(html, @"rel=""nofollow"" target=""_blank"">([\s\S]*?)</a>");
                    
                       
                    MatchCollection names = Regex.Matches(html, @"rel=""nofollow"" target=""_blank"">([\s\S]*?)<td class=""title"">([\s\S]*?)</td>");

                    if (urls.Count == 0)
                    {
                        continue;
                    }

                    for (int j = 0; j < urls.Count; j++)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(urls[j].Groups[1].Value);
                        lv1.SubItems.Add(names[j].Groups[2].Value.Replace("<span>", "").Replace("</span>", "").Trim());
                        Thread.Sleep(200);
                    }

                    Thread.Sleep(Convert.ToInt32(textBox2.Text)*1000);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

         
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run1));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
