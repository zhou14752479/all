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

namespace 主程序202104
{
    public partial class 极速赛车 : Form
    {
        public 极速赛车()
        {
            InitializeComponent();
        }
        #region 主程序
        public void run()
        {
            string url = "https://backs.wlyylw.cn/lottery-client-api/races/min/10002/history?date=";

            string html = method.GetUrl(url, "utf-8");
            MatchCollection pdts = Regex.Matches(html, @"""pdt"":""([\s\S]*?)""");
            MatchCollection pdis = Regex.Matches(html, @"""pdi"":([\s\S]*?),");
            MatchCollection pdcs = Regex.Matches(html, @"""pdc"":\[([\s\S]*?)\]");

            for (int j = 0; j < pdts.Count; j++)
            {

                try
                {


                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(pdts[j].Groups[1].Value);
                    lv1.SubItems.Add(pdis[j].Groups[1].Value);
                    lv1.SubItems.Add(pdcs[j].Groups[1].Value);
                }
                catch (Exception ex)
                {


                    continue;
                }
            }
        }

        #endregion

        Thread thread;
        bool zanting = true;
        bool status = true;


        private void 极速赛车_Load(object sender, EventArgs e)
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
    }
}
