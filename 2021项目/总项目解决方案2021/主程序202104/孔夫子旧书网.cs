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
    public partial class 孔夫子旧书网 : Form
    {
        public 孔夫子旧书网()
        {
            InitializeComponent();
        }


        Thread thread;
        bool zanting = true;
        bool status = true;

        /// <summary>
        /// 全部价格
        /// </summary>
        public void run()
        {

            for (int page = 1; page < 5000; page++)
            {
                try
                {
                    label2.Text = "正在抓取第："+page+"页";

                    string id = Regex.Match(textBox1.Text, @"\d{4,}").Groups[0].Value;

                    string url = "http://shop.kongfz.com/" + id + "/all/0_50_0_0_" + page + "_sort_desc_0_0/";

                    string html = method.GetUrl(url, "utf-8");

                    MatchCollection uids = Regex.Matches(html, @"<div class=""item-row clearfix""([\s\S]*?)isbn=""([\s\S]*?)""");
                    MatchCollection names = Regex.Matches(html, @"class=""row-name"">([\s\S]*?)</a>");
                    MatchCollection prices = Regex.Matches(html, @"<div class=""row-price"">([\s\S]*?)</div>");


                    if (names.Count == 0)
                    {
                        label2.Text = "采集结束";
                        break;
                    }
                        

                    for (int j = 0; j < names.Count; j++)
                    {

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(Regex.Replace(uids[j].Groups[2].Value.Trim(), "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(names[j].Groups[1].Value.Trim(), "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(prices[j].Groups[1].Value.Trim(), "<[^>]+>", ""));



                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;


                    }
                    Thread.Sleep(1000);

                }

                
                catch (Exception ex)
                {

                    ex.ToString();
                }

            }
            label2.Text = "采集结束";



        }
        private void button6_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com:8080/api/vip.html", "utf-8");

            if (!html.Contains(@"OaO6Wx"))
            {

                return;
            }



            #endregion
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入店铺网址");
                return;
            }

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 孔夫子旧书网_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void 孔夫子旧书网_Load(object sender, EventArgs e)
        {

        }
    }
}
