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

namespace main._2019_8
{
    public partial class 中国足球网 : Form
    {
        public 中国足球网()
        {
            InitializeComponent();
        }
        #region  运行程序
        bool zanting = true;
        public void run()
        {


            string URL = "http://live.zgzcw.com/qb/";

            string html = method.GetUrl(URL, "utf-8");

            MatchCollection ids = Regex.Matches(html, @"matchid=""([\s\S]*?)""");

            

            for (int i = 0; i < ids.Count; i++)
            {
                string url = "http://fenxi.zgzcw.com/"+ ids[i].Groups[1].Value + "/bjop" ;
                string strhtml = method.GetUrl(url, "utf-8");

                Match ahtml = Regex.Match(strhtml, @"10BET</td>([\s\S]*?)</tr>");
                Match bhtml = Regex.Match(strhtml, @"12bet</td>([\s\S]*?)</tr>");
                Match chtml = Regex.Match(strhtml, @"IBCBET</td>([\s\S]*?)</tr>");


                Match zhu = Regex.Match(strhtml, @"<title>([\s\S]*?)VS");
                Match ke = Regex.Match(strhtml, @"VS([\s\S]*?)</title>");
                Match time = Regex.Match(strhtml, @"比赛时间：([\s\S]*?)</span>");


                MatchCollection  a1s = Regex.Matches(ahtml.Groups[1].Value, @"data=""([\s\S]*?)""");
                MatchCollection b1s = Regex.Matches(bhtml.Groups[1].Value, @"data=""([\s\S]*?)""");
                MatchCollection c1s = Regex.Matches(chtml.Groups[1].Value, @"data=""([\s\S]*?)""");

                if (a1s.Count > 2)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                    lv1.SubItems.Add(a1s[0].Groups[1].Value);   //比分
                    lv1.SubItems.Add(a1s[1].Groups[1].Value);   //比分
                    lv1.SubItems.Add(a1s[2].Groups[1].Value);   //比分
                    lv1.SubItems.Add(a1s[a1s.Count-1].Groups[1].Value);   //比分
                    lv1.SubItems.Add(zhu.Groups[1].Value + "：" + ke.Groups[1].Value);
                    lv1.SubItems.Add("10BET");   //公司
                    lv1.SubItems.Add(time.Groups[1].Value);   //时间

                }
                if (b1s.Count >2)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                    lv1.SubItems.Add(b1s[0].Groups[1].Value);   //比分
                    lv1.SubItems.Add(b1s[1].Groups[1].Value);   //比分
                    lv1.SubItems.Add(b1s[2].Groups[1].Value);   //比分
                    lv1.SubItems.Add(b1s[b1s.Count - 1].Groups[1].Value);   //比分
                    lv1.SubItems.Add(zhu.Groups[1].Value + "：" + ke.Groups[1].Value);
                    lv1.SubItems.Add("12bet");   //公司
                    lv1.SubItems.Add(time.Groups[1].Value);   //时间

                }
                if (c1s.Count > 2)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                    lv1.SubItems.Add(c1s[0].Groups[1].Value);   //比分
                    lv1.SubItems.Add(c1s[1].Groups[1].Value);   //比分
                    lv1.SubItems.Add(c1s[2].Groups[1].Value);   //比分
                    lv1.SubItems.Add(c1s[c1s.Count - 1].Groups[1].Value);   //比分
                    lv1.SubItems.Add(zhu.Groups[1].Value + "：" + ke.Groups[1].Value);
                    lv1.SubItems.Add("IBCBET");   //公司
                    lv1.SubItems.Add(time.Groups[1].Value);   //时间

                }



                if (a1s.Count>2 || b1s.Count > 2 || c1s.Count > 2 )
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据  
                    for (int j = 0; j < 9; j++)
                    {

                        lv1.SubItems.Add("---------------------------");   //比分
                    }
                }
                while (this.zanting == false)
                {
                    label1.Text = "已暂停....";
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }


                if (listView1.Items.Count > 2)
                {
                    listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                }
                Thread.Sleep(1000);
            }



            label1.Text = "验证结束，请点击导出，文本名为【导出结果】";
        }

        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            label1.Text = "软件已经开始运行请勿重复点击....";
        }

        private void 中国足球网_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
