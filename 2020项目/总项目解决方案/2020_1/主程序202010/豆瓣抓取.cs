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
namespace 主程序202010
{
    public partial class 豆瓣抓取 : Form
    {
        public 豆瓣抓取()
        {
            InitializeComponent();
        }
        public void run()
        {
            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string url in text)
            {

                string strhtml = method.GetUrl(url, "utf-8");
                Match title = Regex.Match(strhtml, @"<strong>([\s\S]*?)</strong>");
                Match zhuyan = Regex.Match(strhtml, @"主演</span>:([\s\S]*?)<br");
                Match type = Regex.Match(strhtml, @"类型:</span>([\s\S]*?)<br");
                Match shoubo = Regex.Match(strhtml, @"首播:</span>([\s\S]*?)<br");
                Match score = Regex.Match(strhtml, @"average"">([\s\S]*?)<");
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                lv1.SubItems.Add(title.Groups[1].Value);
                lv1.SubItems.Add(Regex.Replace(zhuyan.Groups[1].Value, "<[^>]+>", ""));
                lv1.SubItems.Add(Regex.Replace(type.Groups[1].Value, "<[^>]+>", ""));
                lv1.SubItems.Add(Regex.Replace(shoubo.Groups[1].Value, "<[^>]+>", ""));
                lv1.SubItems.Add(Regex.Replace(score.Groups[1].Value, "<[^>]+>", ""));




                Thread.Sleep(1000);
                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
            }

           
        }



        public void duanping()
        {
            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string url in text)
            {
                Match aid = Regex.Match(url, @"\d{8}");
                string filename = "";
                for (int i = 0; i < 21; i=i+20)
                {
                    string URL = "https://movie.douban.com/subject/" + aid.Groups[0].Value + "/comments?start="+i+"&limit=20&status=P&sort=new_score";
                    string strhtml = method.GetUrl(URL, "utf-8");
                    Match title = Regex.Match(strhtml, @"<h1>([\s\S]*?)</h1>");
                    filename = title.Groups[1].Value + ".xlsx";
                    MatchCollection rank = Regex.Matches(strhtml, @"allstar([\s\S]*?)0");
                    MatchCollection date = Regex.Matches(strhtml, @"comment-time "" title=""([\s\S]*?)""");
                    MatchCollection body = Regex.Matches(strhtml, @"<p class="" comment-content"">([\s\S]*?)</p>");
                    MatchCollection vote = Regex.Matches(strhtml, @"vote-count"">([\s\S]*?)</span>");

                    for (int j = 0; j < rank.Count; j++)
                    {
                        try
                        {
                            ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv2.SubItems.Add(Regex.Replace(rank[j].Groups[1].Value, "<[^>]+>", "").Trim());
                            lv2.SubItems.Add(Regex.Replace(date[j].Groups[1].Value, "<[^>]+>", "").Trim());
                            lv2.SubItems.Add(Regex.Replace(body[j].Groups[1].Value, "<[^>]+>", "").Trim());
                            lv2.SubItems.Add(Regex.Replace(vote[j].Groups[1].Value, "<[^>]+>", "").Trim());
                        }
                        catch (Exception)
                        {

                            continue;
                        }
                       
                    }
                  
                }

              

                //method.DataTableToExcelName(method.listViewToDataTable(this.listView2), filename.Replace(" ",""), true);
                //listView2.Items.Clear();
                Thread.Sleep(1000);
                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
              
            }


        }
        bool zanting = true;
        private void 豆瓣抓取_Load(object sender, EventArgs e)
        {

        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(duanping);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }
    }
}
