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

        bool status = true;
        public dnsIp()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 旁站查询
        /// </summary>
        public void run()
        {
            progressBar1.Value = 0;//设置当前值
            progressBar1.Step = 1;//设置没次增长多少
            for (int i = 1; i <99; i++)
            {
                
          
                string URL = "https://dns.aizhan.com/" + textBox1.Text + "/" + i + "/";

                label7.Text = "正在获取第" + i+"页域名信息.....";
                string html = method.GetUrl(URL, "utf-8");

                MatchCollection  urls = Regex.Matches(html, @"rel=""nofollow"" target=""_blank"">([\s\S]*?)</a>");
                MatchCollection names = Regex.Matches(html, @"rel=""nofollow"" target=""_blank"">([\s\S]*?)<td class=""title"">([\s\S]*?)</td>");
                Match Ipaddress = Regex.Match(html, @"<strong class=""red"">([\s\S]*?)<");
                Match area = Regex.Match(html, @"<strong>([\s\S]*?)<");

                Match count = Regex.Match(html, @"共有 <span class=""red"">([\s\S]*?)</span>"); //总数
                label8.Text = Ipaddress.Groups[1].Value;
                label9.Text = area.Groups[1].Value;
                if (urls.Count == 0)
                    break;
                progressBar1.Maximum = Convert.ToInt32(count.Groups[1].Value);//设置最大值
                for (int j= 0; j< urls.Count ; j++)
                {

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString()); //使用Listview展示数据         
                    lv1.SubItems.Add(urls[j].Groups[1].Value);
                    lv1.SubItems.Add(names[j].Groups[2].Value.Replace("<span>","").Replace("</span>","").Trim());
                    Thread.Sleep(200);
                   
                    
                    
                    progressBar1.Value += progressBar1.Step; //让进度条增加一次
                }
            }

            label7.Text = "抓取结束";
            MessageBox.Show("抓取完成");
        }


        /// <summary>
        /// C段查询
        /// </summary>
        public void run1()
        {
            try
            {

                progressBar1.Value = 0;//设置当前值
                progressBar1.Step = 1;//设置没次增长多少
                progressBar1.Maximum =255;//设置最大值
                string[] duans = textBox1.Text.Split(new string[] { "." }, StringSplitOptions.None);
                for (int a = 1; a < 256; a++)
                {
                    ListViewItem lv2 = listView2.Items.Add(duans[0] + "." + duans[1] + "." + duans[2] + "." + a); //使用Listview展示数据         
                    progressBar1.Value += progressBar1.Step; //让进度条增加一次
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

         
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            #region 通用验证

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "2.2.2.2")
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {
                status = false;
                listView1.Items.Clear();
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }
            else
            {
                MessageBox.Show("IP不符");

            }
            #endregion

          
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            #region 通用验证

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "2.2.2.2")
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {
                status = true;
                listView1.Items.Clear();
                listView2.Items.Clear();
                Thread thread = new Thread(new ThreadStart(run1));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }
            else
            {
                MessageBox.Show("IP不符");

            }
            #endregion
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
           
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Process.Start(this.listView1.SelectedItems[0].SubItems[1].Text);
        }

        private void listView2_MouseClick(object sender, MouseEventArgs e)
        {
            status = false;
            listView1.Items.Clear();
            textBox1.Text = this.listView2.SelectedItems[0].SubItems[0].Text;
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

        }
    }
}
