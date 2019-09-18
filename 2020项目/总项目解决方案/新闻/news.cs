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

namespace 新闻
{
    public partial class news : Form
    {
        public news()
        {
            InitializeComponent();
        }
        bool zanting = true;


        #region  新闻获取
        public void run()
        {
            try
            {
                string[] keywords = textBox5.Text.Split(new string[] { "," }, StringSplitOptions.None);
                foreach (string keyword in keywords)
                {


                    for (int i = 1; i < 9999; i = i + 10)
                    {

                        string url = "https://www.baidu.com/s?rtt=1&bsst=1&cl=2&tn=news&rsv_dl=ns_pc&word=" + keyword + "&x_bfe_rqs=03E80&x_bfe_tjscore=0.002777&tngroupname=organic_news&pn=" + i;
                        string html = method.GetUrl(url, "utf-8");

                        MatchCollection ids = Regex.Matches(html, @"baijiahao\.baidu\.com([\s\S]*?)""");


                        if (ids.Count == 0)
                            break;

                        for (int j = 0; j < ids.Count; j++)
                        {

                            string URL = "http://baijiahao.baidu.com" + ids[j].Groups[1].Value;

                            string strhtml = method.GetUrl(URL, "utf-8");



                            Match a1 = Regex.Match(strhtml, @"<h2>([\s\S]*?)</h2>");
                            Match a2 = Regex.Match(strhtml, @"time"":""([\s\S]*?)""");
                            Match a3 = Regex.Match(strhtml, @"uthor-name"">([\s\S]*?)<");
                            Match a4 = Regex.Match(strhtml, @"<div class=""article-content"">([\s\S]*?)</div>");




                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            listViewItem.SubItems.Add(a1.Groups[1].Value);
                            listViewItem.SubItems.Add(a2.Groups[1].Value);
                            listViewItem.SubItems.Add(a3.Groups[1].Value);
                            listViewItem.SubItems.Add(a4.Groups[1].Value);

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }

                            Thread.Sleep(100);

                        }
                    }


                }
            }




            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #endregion
        private void News_Load(object sender, EventArgs e)
        {

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
                if (ip.Groups[1].Value.Trim() == "14.14.14.14")
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {
               
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

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
