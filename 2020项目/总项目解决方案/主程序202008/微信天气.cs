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

namespace 主程序202008
{
    public partial class 微信天气 : Form
    {
        public 微信天气()
        {
            InitializeComponent();
        }

        private void 微信天气_Load(object sender, EventArgs e)
        {
            //tabControl1.SelectedIndex = 1;
            method.SetFeatures(10000);
           webBrowser1.ScriptErrorsSuppressed = true;

            webBrowser1.Navigate("https://wx.weather.cn/pmsp-wechat/location.html");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            webBrowser1.Navigate("https://wx.weather.cn/pmsp-wechat/location.html");
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            
        }

        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
           string[] diqus= textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string diqu in diqus)
            {
                if (diqu == "")
                {
                    continue;
                }

                string[] text = diqu.Split(new string[] { "/" }, StringSplitOptions.None);

                string province = System.Web.HttpUtility.UrlEncode(text[0]);
                string city = System.Web.HttpUtility.UrlEncode(text[1]);
                string cnty = System.Web.HttpUtility.UrlEncode(text[2]);

                DateTime start = dateTimePicker1.Value;
                DateTime end = dateTimePicker2.Value;

                for (DateTime now = start; now <= end; now = now.AddDays(1))
                {

                    string url = "https://wx.weather.cn/pmsp-wechat/seeWeather/getHistoryNawsHourSK?province=" + province + "&city=" + city + "&cnty=" + cnty + "&date=" + now.ToString("yyyy-MM-dd") + "+00%3A00%3A00";

                    string html = method.GetUrl(url, "utf-8");

                    MatchCollection a1s = Regex.Matches(html, @"PRE_Time_2020"":([\s\S]*?),");
                    MatchCollection a2s = Regex.Matches(html, @"TEM_Max"":([\s\S]*?),");
                    MatchCollection a3s = Regex.Matches(html, @"TEM_Min"":([\s\S]*?),");
                    MatchCollection a4s = Regex.Matches(html, @"TEM_Avg"":([\s\S]*?),");

                    for (int j = 0; j < a1s.Count; j++)
                    {

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(a1s[j].Groups[1].Value);
                        lv1.SubItems.Add(a2s[j].Groups[1].Value);
                        lv1.SubItems.Add(a3s[j].Groups[1].Value);
                        lv1.SubItems.Add(a4s[j].Groups[1].Value);
                        lv1.SubItems.Add(diqu);
                        lv1.SubItems.Add(now.ToString("yyyy-MM-dd"));

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }


                    }
                    Thread.Sleep(500);
                }
            }
        }

        bool zanting = true;
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"ceshi1111111"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion




            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
