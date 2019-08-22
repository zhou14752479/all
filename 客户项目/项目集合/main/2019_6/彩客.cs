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
    public partial class 彩客 : Form
    {
        public 彩客()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 获取当前的时间戳
        /// </summary>
        /// <returns></returns>
        public static string Timestamp()
        {
            long ts = ConvertDateTimeToInt(DateTime.Now);
            return ts.ToString();
        }

        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            //System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            //long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            long t = (time.Ticks - 621356256000000000) / 10000;
            return t;
        }

        private void 彩客_Load(object sender, EventArgs e)
        {

        }
        bool zanting = true;
        public void run()
        {


            string URL = "http://www.310win.com/info/match/data/goal3.xml?" + Timestamp()+ "000";

            string html = method.GetUrl(URL, "utf-8");

            Match ids = Regex.Match(html, @"<ids>([\s\S]*?)</ids>");

            string[] IDS = ids.Groups[1].Value.Split(new string[] { "," }, StringSplitOptions.None);
          
            for (int i = 0; i < IDS.Length-1; i++)
            {
                string url = "http://1x2d.win007.com/"+IDS[i]+".js";
                string strhtml = method.GetUrl(url, "utf-8");
                Match aaas = Regex.Match(strhtml, @"10BET\|([\s\S]*?)10BET");
                Match bbbs = Regex.Match(strhtml, @"IBCBET\|([\s\S]*?)IBCBET");
                Match cccs = Regex.Match(strhtml, @"12bet\|([\s\S]*?)12BET");
                Match ddds = Regex.Match(strhtml, @"Mansion88\|([\s\S]*?)明陞");

                Match zhu = Regex.Match(strhtml, @"hometeam_cn=""([\s\S]*?)""");
                Match ke = Regex.Match(strhtml, @"guestteam_cn=""([\s\S]*?)""");

                string[] aaa = aaas.Groups[1].Value.Split(new string[] { "|" }, StringSplitOptions.None);
                string[] bbb = bbbs.Groups[1].Value.Split(new string[] { "|" }, StringSplitOptions.None);
                string[] ccc = cccs.Groups[1].Value.Split(new string[] { "|" }, StringSplitOptions.None);
                string[] ddd = ddds.Groups[1].Value.Split(new string[] { "|" }, StringSplitOptions.None);
                if (aaa.Length > 6)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                    for (int j = 0; j < 7; j++)
                    {
                        lv1.SubItems.Add(aaa[j]);   //比分
                    }
                    lv1.SubItems.Add(zhu.Groups[1].Value + "：" + ke.Groups[1].Value);
                    lv1.SubItems.Add("10BET(英国)");   //比分

                }
                if (bbb.Length > 6)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                    for (int j = 0; j < 7; j++)
                    {       
                        lv1.SubItems.Add(bbb[j]);   //比分
                    }
                    lv1.SubItems.Add(zhu.Groups[1].Value+"："+ke.Groups[1].Value);
                    lv1.SubItems.Add("IBCBET");   //比分

                }
                if (ccc.Length > 6)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                    for (int j = 0; j < 7; j++)
                    {
                        lv1.SubItems.Add(ccc[j]);   //比分
                    }
                    lv1.SubItems.Add(zhu.Groups[1].Value + "：" + ke.Groups[1].Value);
                    lv1.SubItems.Add("12bet");   //比分

                }
                if (ddd.Length > 6)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                    for (int j = 0; j < 7; j++)
                    {
                        lv1.SubItems.Add(ddd[j]);   //比分
                    }
                    lv1.SubItems.Add(zhu.Groups[1].Value + "：" + ke.Groups[1].Value);
                    lv1.SubItems.Add("明陞");   //比分

                }


                if ( aaa.Length > 6|| bbb.Length > 6 || ccc.Length > 6 || ddd.Length > 6)
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

       
       
    

    private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            label1.Text = "软件已经开始运行请勿重复点击....";
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}


