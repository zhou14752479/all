using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 主程序202103
{
    public partial class mtc25时时彩 : Form
    {
        public mtc25时时彩()
        {
            InitializeComponent();
        }
        

        #region 主程序
        public void run()
        {

          
            for (DateTime dt = dateTimePicker1.Value; dt < dateTimePicker2.Value; dt=dt.AddDays(1))
            {


                string url = "https://www.mtc25.com/static//data/"+dt.ToString("yyyyMMdd") + "3HistoryLottery.json?_=1615265002031";

                string html = method.GetUrl(url,"utf-8");

                MatchCollection times = Regex.Matches(html, @"""openTime"":""([\s\S]*?)""");
                MatchCollection values = Regex.Matches(html, @"""openNum"":""([\s\S]*?)""");
              
                for (int j = 0; j < times.Count; j++)
                {

                    try
                    {
                        string[] text = values[j].Groups[1].Value.Split(new string[] { "," }, StringSplitOptions.None);
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(times[j].Groups[1].Value);
                        lv1.SubItems.Add(text[0]);
                        lv1.SubItems.Add(text[1]);
                        lv1.SubItems.Add(text[2]);
                        lv1.SubItems.Add(text[3]);
                        lv1.SubItems.Add(text[4]);
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;

                    }
                    catch (Exception)
                    {

                       continue;
                    }
                   

                }
                Thread.Sleep(1000);
            }

        }

        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void mtc25时时彩_Load(object sender, EventArgs e)
        {

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
        Thread thread;
        bool zanting = true;
        bool status = true;
        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"QUpuM7"))
            {
                MessageBox.Show("");
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

        private void mtc25时时彩_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
