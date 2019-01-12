using System;
using System.Collections;
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

namespace fang.直播平台软件
{
    public partial class kuaishou : Form
    {
        public kuaishou()
        {
            InitializeComponent();
        }

        private void kuaishou_Load(object sender, EventArgs e)
        {

        }
        bool status = true;

        #region  虎牙平台
        public void run()
        {

            try
            {
                int[] cates = { 1001, 22008, 22008, 22007, 22007, 1049, 1049, 1006, 1006, 1002, 1002, 1054, 1054, 1055, 1055, 1012, 1012, 22014, 22014, 1016, 1016, 1039, 1039, 1003, 1003, 1005, 1005, 22011, 22011, 1023, 1023, 1027, 1027, 1036, 1036, 1021, 1021, 1043, 1043, 1030, 1030, 22018, 22018, 22019, 22019, 22023, 22023, 22053, 22053, 22061, 22061, 1010, 1010, 1013, 1013, 1045, 1045, 1024, 1024, 1046, 1046, 22029, 22029, 22062, 22062 };

                foreach (int cate in cates)
                {


                    for (int i = 1; i < 11; i++)
                    {
                        String Url = "https://live.kuaishou.com/cate/DQRM/"+cate+"?page=" + i;

                        string strhtml = method.GetUrl(Url, "utf-8");

                        textBox1.Text = Url;
                        MatchCollection TitleMatchs = Regex.Matches(strhtml, @"""kwaiId"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        ArrayList lists = new ArrayList();
                        foreach (Match NextMatch in TitleMatchs)
                        {
                            lists.Add("https://live.kuaishou.com/profile/" + NextMatch.Groups[1].Value);

                        }

                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;


                        foreach (string url in lists)
                        {

                            string html = method.GetUrl(url, "utf-8");


                            Match infos = Regex.Match(html, @"name=""description"" content=""([\s\S]*?)""");

                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                            lv1.SubItems.Add(url);
                            lv1.SubItems.Add(infos.Groups[1].Value.Trim().Replace(" ", ""));



                            if (listView1.Items.Count > 1)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                            }
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(Convert.ToInt32(1000));

                            while (this.status == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }

                        }


                    }
                }
            }

            catch (System.Exception ex)
            {
                ex.ToString();
            }

        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            this.status = true;

            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.status = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
