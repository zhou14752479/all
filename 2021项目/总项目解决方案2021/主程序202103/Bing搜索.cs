using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 主程序202103
{
    public partial class Bing搜索 : Form
    {
        public Bing搜索()
        {
            InitializeComponent();
        }



        public void run()
        {
            for (int i = 0; i < Convert.ToInt32(textBox2.Text); i++)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(bing));
                string o = "shangpucz";
                thread.Start((object)o);
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            

        }
        /// <summary>
        /// bing搜索
        /// </summary>
        /// <returns></returns>
        public void bing(object keyword)
        {
           
                if (keyword.ToString() != "")
                {
                   int newpage = 0;
                    for (int page = 1; page < Convert.ToInt32(textBox5.Text); page++)
                    {
                        if (page == 1)
                        {
                            newpage = 0;
                        }
                        else if (page == 2)
                        {
                            newpage = 5;
                        }
                        else
                        {
                            newpage = ((page - 2) * 10) + 5;
                        }


                        string url = "https://cn.bing.com/search?q=" +keyword.ToString().Replace(" ", "%20") + "&go=Search&ensearch=1&qs=ds&first=" + newpage + "&FORM=PERE1";
                        
                        string html = method.GetUrlWithCookie(url, "MUID=35C5750C71E2697B34B77AE975E26F58; MUIDB=35C5750C71E2697B34B77AE975E26F58; SRCHD=AF=NOFORM; SRCHUID=V=2&GUID=C41733217C754A0FA89197A7C02694C3&dmnchg=1; _SS=SID=1BBF2DF1322E66750DB42206336D67EA&bIm=421765; _FP=hta=on; ENSEARCH=BENVER=1; SerpPWA=reg=1; SRCHUSR=DOB=20210311&T=1615460470000&TPC=1615455042000; ipv6=hit=1615464083705&t=4; ENSEARCHZOSTATUS=STATUS=0; _EDGE_S=SID=1BBF2DF1322E66750DB42206336D67EA&mkt=zh-cn&ui=ja-jp; _EDGE_CD=u=ja-jp; SNRHOP=I=&TS=; ULC=P=7991|10:1&H=7991|10:1&T=7991|10:1:2; SRCHHPGUSR=SRCHLANGV2=ja&CW=1920&CH=435&DPR=1&UTC=480&DM=0&HV=1615461143&WTS=63751057270&BRW=XW&BRH=S&BZA=0", "utf-8");
                        textBox4.Text = html;
                        try
                        {
                            MatchCollection titles = Regex.Matches(html, @"<h2>([\s\S]*?)>([\s\S]*?)</h2>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(5));
                            MatchCollection urls = Regex.Matches(html, @"<h2><a target=""_blank"" href=""([\s\S]*?)""", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(5));
                            MessageBox.Show(titles.Count.ToString());
                            for (int j = 0; j < titles.Count; j++)
                            {
                                try
                                {
                                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                    lv1.SubItems.Add(Regex.Replace(titles[j].Groups[2].Value, "<[^>]+>", ""));
                                    lv1.SubItems.Add(Regex.Replace(urls[j].Groups[1].Value, "<[^>]+>", "") );
                                }
                                catch (Exception ex)
                                {
                                    textBox4.Text = ex.ToString();
                                    continue;
                                }


                            }


                        }
                        catch (RegexMatchTimeoutException ex)
                        {
                            textBox4.Text = ex.ToString();
                        }

                    }
                }
            

        }
        private void Bing搜索_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"qichachafapiao"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(bing);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
        Thread thread;
        bool zanting = true;
        bool status = true;
        private void button6_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void Bing搜索_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否退出？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }
    }
}
