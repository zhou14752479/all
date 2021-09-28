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

namespace 科研鲸
{
    public partial class 科研鲸 : Form
    {
        public 科研鲸()
        {
            InitializeComponent();
        }

        Dictionary<string, string> dics = new Dictionary<string, string>();
        #region 科研鲸
        public void run()
        {
            label1.Text = DateTime.Now.ToLongTimeString() + "：开始运行";
            foreach (var item in dics.Keys)
            {
                string cateid = dics[item];
                try
                {



                    string url = "https://jf.cas-harbour.cn/zhongkehaobo/wx-pull/topic-pageList?categoryId=" + cateid + "&pageNum=1&pageSize=100";

                    string html = method.GetUrl(url, "utf-8");

                    MatchCollection ids = Regex.Matches(html, @"""id"":""([\s\S]*?)""");
                    MatchCollection startTime = Regex.Matches(html, @"""startTime"":""([\s\S]*?)""");
                    MatchCollection courseTime = Regex.Matches(html, @"""courseTime"":""([\s\S]*?)""");
                    MatchCollection topicLingyu = Regex.Matches(html, @"""topicLingyu"":""([\s\S]*?)""");
                    MatchCollection name = Regex.Matches(html, @"""name"":""([\s\S]*?)""");
                    MatchCollection schoolName = Regex.Matches(html, @"""schoolName"":""([\s\S]*?)""");
                    MatchCollection speaker = Regex.Matches(html, @"""speaker"":""([\s\S]*?)""");


                    if (ids.Count == 0)
                    {
                        break;
                    }
                    for (int a = 0; a < ids.Count; a++)
                    {
                        try
                        {
                            label1.Text = DateTime.Now.ToLongTimeString() + "：正在抓取：" + name[a].Groups[1].Value;
                            string aurl = "https://jf.cas-harbour.cn/zhongkehaobo/tbTopic/"+ids[a].Groups[1].Value;
                            string ahtml= method.GetUrl(aurl, "utf-8");

                            string speakerlevel = Regex.Match(ahtml, @"""speakerTitle"":""([\s\S]*?)""").Groups[1].Value;
                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                            lv1.SubItems.Add(item);
                            lv1.SubItems.Add(startTime[a].Groups[1].Value);
                            lv1.SubItems.Add(courseTime[a].Groups[1].Value);
                            lv1.SubItems.Add(topicLingyu[a].Groups[1].Value);
                            lv1.SubItems.Add(name[a].Groups[1].Value);
                            lv1.SubItems.Add(schoolName[a].Groups[1].Value);
                            lv1.SubItems.Add(speaker[a].Groups[1].Value);
                            lv1.SubItems.Add(speakerlevel);
                            Thread.Sleep(100);
                            if (status == false)
                                return;
                        }
                        catch (Exception)
                        {

                            continue;
                        }

                    }

                    label1.Text = DateTime.Now.ToLongTimeString() + "：采集完成";
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());

                }

            }
        }

        #endregion

        Thread thread;
     
        bool status = true;
        private void 科研鲸_Load(object sender, EventArgs e)
        {
            dics.Add("文科", "337e879af366e3439cbec78191808f71");
            dics.Add("理科", "b285103610fe46fa8703401cac395b48");
            dics.Add("工科", "b4d8af343694e3a9c0e96d227738090f");
            dics.Add("商科", "8101a70325b25491304afe73a13beb3f");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = true;
            #region 通用检测


            string html = method.GetUrl("http://www.acaiji.com:8080/api/vip.html", "utf-8");

            if (!html.Contains(@"0Ymp"))
            {

                return;
            }

            #endregion
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
