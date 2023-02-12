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

namespace 药房网
{
    public partial class 药师帮 : Form
    {
        public 药师帮()
        {
            InitializeComponent();
        }

        bool zanting = true;
        bool status = true;
        Thread thread;

        string[] cates = { "10", "15", "12", "29", "206", "30", "11", "17", "14", "19", "16", "20" };
        public void run_xcx()
        {
            try
            {
                foreach (string cate in cates)
                {

                    for (int page = 1; page < 999; page++)
                    {

                        string aurl = "https://pub.yaofangwang.com/4000/4000/0/guest.medicine.getMedicines?conditions={\"categoryid\":" + cate + ",\"sort\":\"\",\"sorttype\":\"\"}&pageIndex=" + page + "&pageSize=100&version=8.0.28&__client=app_wx&app_version=4.9.22&osVersion=miniapp&deviceName=iPhone 13<iPhone14,5>&os=ios&market=iPhone&networkType=true&lat=33.94001092793934&lng=118.25325794385365&user_city_name=宿迁市&user_region_id=1739&idfa=wx_091GxYZv3F5uiZ2R3W2w3vMJsz3GxYZK&deviceNo=wx_091GxYZv3F5uiZ2R3W2w3vMJsz3GxYZK";
                        string ahtml = method.GetUrl(aurl, "utf-8");

                        MatchCollection names = Regex.Matches(ahtml, @"""namecn"":""([\s\S]*?)""");
                        MatchCollection standards = Regex.Matches(ahtml, @"""standard"":""([\s\S]*?)""");
                        MatchCollection mill_title = Regex.Matches(ahtml, @"""mill_title"":""([\s\S]*?)""");
                        MatchCollection authorized_code = Regex.Matches(ahtml, @"""authorized_code"":""([\s\S]*?)""");
                        MatchCollection min_price = Regex.Matches(ahtml, @"""min_price"":([\s\S]*?),");
                        if (names.Count == 0)
                            break;
                        for (int a = 0; a < names.Count; a++)
                        {
                            try
                            {
                                textBox2.Text = "正在读取：" + names[a].Groups[1].Value;
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(names[a].Groups[1].Value);
                                lv1.SubItems.Add(standards[a].Groups[1].Value);
                                lv1.SubItems.Add(mill_title[a].Groups[1].Value);
                                lv1.SubItems.Add(authorized_code[a].Groups[1].Value);
                                lv1.SubItems.Add(min_price[a].Groups[1].Value);

                                if (listView1.Items.Count > 2)
                                {
                                    this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                                }
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

            }
            catch (Exception ex)
            {
                textBox2.Text = ex.ToString();
            }


        }
        private void button6_Click(object sender, EventArgs e)
        {
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run_xcx);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            textBox2.Text = "开始抓取......";
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

        private void 药师帮_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
