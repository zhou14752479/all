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

namespace 地点微博抓取
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Thread thread;
        bool zanting = true;
        public void run1()
        {
            StreamReader sr = new StreamReader(textBox2.Text, method.EncodingType.GetTxtType(textBox2.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();

            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 1; i < text.Length; i++)
            {
                string[] values = text[i].Split(new string[] { "," }, StringSplitOptions.None);

                string lontitude = values[5];
                string laititude = values[6];
                string url = "https://m.weibo.cn/api/container/getIndex?containerid=2306570043_"+ lontitude + "_"+ laititude + "&extparam=map__";
            
                string html = method.GetUrl(url, "utf-8");



                MatchCollection titles = Regex.Matches(html, @"show_additional_indication"":([\s\S]*?),""text"":""([\s\S]*?)"",""textLength");
                MatchCollection times = Regex.Matches(html, @"""created_at"":""([\s\S]*?)""");
                MatchCollection lxrs = Regex.Matches(html, @"""screen_name"":""([\s\S]*?)""");
                MatchCollection comment = Regex.Matches(html, @"""comments_count"":([\s\S]*?),");


                if (titles.Count == 0)
                    break;
                for (int j = 0; j < titles.Count; j++)
                {
                    try
                    {
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(Regex.Replace(method.Unicode2String(titles[j].Groups[2].Value), "<[^>]+>", ""));
                        lv1.SubItems.Add(method.Unicode2String(times[j].Groups[1].Value));
                        lv1.SubItems.Add(method.Unicode2String(lxrs[j].Groups[1].Value));
                        lv1.SubItems.Add(lontitude + "," + laititude);
                        lv1.SubItems.Add(method.Unicode2String(comment[j].Groups[1].Value));
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                    }
                    catch (Exception)
                    {

                        continue;
                    }
                   
                }
                Thread.Sleep(1000);

            }
        }
        string cookie = "SINAGLOBAL=2631464369339.447.1613870261937; SUBP=0033WrSXqPxfM725Ws9jqgMF55529P9D9WFAbTDTMuOyH2NWYqT8Dr.s5JpX5KMhUgL.Fo-ESo.XS0-0eh52dJLoI7LLqcyadsvc9P.t; UOR=,,www.baidu.com; ALF=1646039692; SSOLoginState=1614503693; SCF=Alslx_RaEKKXj8jzZBGXOQSLv6Lc3mi-TFwTiE0NdNW11_iKCsZfECXVs1TXPpYwhNCrvVXhedu1XrCn3DQS_tE.; SUB=_2A25NPy9dDeRhGeNM7VsV9yvPyzyIHXVuTQeVrDV8PUNbmtAKLU3ukW9NThXiEml9hxoUPW1KVrOkfJf8mWDmpVxk; _s_tentry=login.sina.com.cn; Apache=4896365850556.852.1614503693467; ULV=1614503693472:4:4:2:4896365850556.852.1614503693467:1614473754283; WBStorage=8daec78e6a891122|undefined";
        /// <summary>
        /// 高级搜索
        /// </summary>

        public void run()
        {

            for (DateTime dt = Convert.ToDateTime("2020-11-01"); dt < Convert.ToDateTime("2020-12-01"); dt=dt.AddDays(1))
            {

                for (int page = 1; page < 51; page++)
                {

                    string url = "https://s.weibo.com/weibo?q=%E5%A4%A9%E6%B4%A5&typeall=1&suball=1&timescope=custom:" + dt.ToString("yyyy-MM-dd") + ":" + dt.ToString("yyyy-MM-dd") + "&Refer=g&page=" + page;

                    string html = method.GetUrlWithCookie(url,cookie, "utf-8");

                    MatchCollection ahtmls = Regex.Matches(html, @"<!--微博内容-->([\s\S]*?)<!--/微博内容-->");

                    MatchCollection lxrs = Regex.Matches(html, @"feed_list_content"" nick-name=""([\s\S]*?)"">");
                    MatchCollection times = Regex.Matches(html, @"click:wb_time"">([\s\S]*?)</a>");
                  
                    MatchCollection comment = Regex.Matches(html, @"feed_list_comment"">评论([\s\S]*?)</a>");


                    if (times.Count == 0)
                        break;

                    for (int j = 0; j < times.Count; j++)
                    {
                        try
                        {

                            string ahtml = ahtmls[j].Groups[1].Value;
                            Match cFull = Regex.Match(ahtml, @"feed_list_content_full"" nick-name=""([\s\S]*?)"" style=""display: none"">([\s\S]*?)</p>");
                            Match c = Regex.Match(ahtml, @"feed_list_content"" nick-name=""([\s\S]*?)"">([\s\S]*?)</p>");
                            string content = cFull.Groups[2].Value;

                            if (cFull.Groups[1].Value == "")
                            {
                                content = c.Groups[2].Value;
                            }

                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(Regex.Replace(content, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(times[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(lxrs[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add("");
                            lv1.SubItems.Add(comment[j].Groups[1].Value.Trim());
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                        }

                        catch (Exception)
                        {

                            continue;
                        }
                    }



                    Thread.Sleep(1000);

                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

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
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox2.Text = this.openFileDialog1.FileName;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
