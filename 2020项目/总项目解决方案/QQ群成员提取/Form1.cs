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
using myDLL;

namespace QQ群成员提取
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string COOKIE { get; set; }
        public string bkn { get; set; }
        long GetBkn()
        {
            string skey = Regex.Match(COOKIE, @"skey=([\s\S]*?);").Groups[1].Value;
            var hash = 5381;
            for (int i = 0, len = skey.Length; i < len; ++i)
                hash += (hash << 5) + (int)skey[i];
            return hash & 2147483647;
        }

       
        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }


        public void getqunList()
        {
            bkn = GetBkn().ToString();
            string html = method.PostUrl("https://qun.qq.com/cgi-bin/qun_mgr/get_group_list", "bkn="+bkn, COOKIE,"utf-8", "application/x-www-form-urlencoded", "https://qun.qq.com/member.html");
            MatchCollection quns = Regex.Matches(html, @"""gc"":([\s\S]*?),");
            foreach (Match item in quns)
            {
                comboBox1.Items.Add(item.Groups[1].Value);
            }
        }


        private DateTime ConvertStringToDateTime(string timeStamp)
        {

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(Convert.ToDouble(timeStamp));
        }
        /// <summary>
        /// 获取时间戳  秒
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalSeconds);
            return a.ToString();
        }

        public bool panduan(string QQ)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].SubItems[1].Text.Trim()+ listView1.Items[i].SubItems[3].Text.Trim() == QQ.Trim())
                {
                    return true;
                }

            }
            return false;
        }


        public void getqunmember()
        {
            textBox3.Text =DateTime.Now.ToString()+ "：正在监控......";
            bkn = GetBkn().ToString();

            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int a= 0; a < text.Length; a++)
            {


                string postdata = "gc=" + text[a].Trim()+ "&st=0&end=20&sort=10&bkn=" + bkn;
                string html = method.PostUrl("https://qun.qq.com/cgi-bin/qun_mgr/search_group_members", postdata, COOKIE, "utf-8", "application/x-www-form-urlencoded", "https://qun.qq.com/member.html");
                MatchCollection QQs = Regex.Matches(html, @"""uin"":([\s\S]*?),");
                MatchCollection join_times = Regex.Matches(html, @"""join_time"":([\s\S]*?),");
                for (int i = 0; i < QQs.Count; i++)
                {
                    textBox3.Text = DateTime.Now.ToString() + "：正在监控......";
                    try
                    {
                        long now = Convert.ToInt64(GetTimeStamp());
                        long join = Convert.ToInt64(join_times[i].Groups[1].Value);
                        // textBox3.Text += now +"---" + join+ "\r\n";
                        if (now - join < 200)
                        {
                            if (!panduan(QQs[i].Groups[1].Value+ text[a].Trim()))
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(QQs[i].Groups[1].Value);
                                lv1.SubItems.Add(ConvertStringToDateTime(join_times[i].Groups[1].Value).ToString());
                                lv1.SubItems.Add(text[a].Trim());
                            }

                        }
                    }
                    catch (Exception ex)
                    {

                        continue;
                    }

                }
                Thread.Sleep(1000);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"QQqunjiankong"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion


            if (comboBox1.Text == "")
            {
                MessageBox.Show("请选择群");
                return;
            }


            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(getqunmember);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            timer1.Start();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            getqunList();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(getqunmember);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            textBox3.Text = DateTime.Now.ToString() + "：停止监控......";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!textBox1.Text.Contains(comboBox1.Text))
            {
                textBox1.Text += comboBox1.Text.Trim()+"\r\n";
            }
        }
    }
}
