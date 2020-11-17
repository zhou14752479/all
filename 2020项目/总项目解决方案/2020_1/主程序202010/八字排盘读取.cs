using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 主程序202010
{
    public partial class 八字排盘读取 : Form
    {
        public 八字排盘读取()
        {
            InitializeComponent();
        }
        public void run()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入日期");
                return;
            }
          

            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != "")
                {
                    string t= Regex.Replace(text[i], @"\s+", " ");
                    string[] value = t.Split(new string[] { " " }, StringSplitOptions.None);
                    if (value.Length > 1)
                    {
                        if (value[1] != "")
                        {
                            try
                            {
                                string url = "http://www.wangdailin.com/bzppph.php?ppcont=" + value[1].Trim() + "&yn=undefined&sex=3";

                                string html = method.GetUrl(url, "utf-8");

                                MatchCollection a1s = Regex.Matches(html, @"<tr style=""text-align:center;font-size:23px""><td></td><td>([\s\S]*?)</td><td>([\s\S]*?)</td><td>([\s\S]*?)</td><td>([\s\S]*?)</td>");
                                Match a2 = Regex.Match(html, @"<font color=""#D9111B""><B>([\s\S]*?)</B>");
                                MatchCollection a3s = Regex.Matches(html, @"<font color=""#D9111B"">([\s\S]*?)</font>");

                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(value[0].Trim());
                                lv1.SubItems.Add(Regex.Replace(a1s[0].Groups[1].Value, "<[^>]+>", "") + Regex.Replace(a1s[1].Groups[1].Value, "<[^>]+>", ""));
                                lv1.SubItems.Add(Regex.Replace(a1s[0].Groups[2].Value, "<[^>]+>", "") + Regex.Replace(a1s[1].Groups[2].Value, "<[^>]+>", ""));
                                lv1.SubItems.Add(Regex.Replace(a1s[0].Groups[3].Value, "<[^>]+>", "") + Regex.Replace(a1s[1].Groups[3].Value, "<[^>]+>", ""));

                                lv1.SubItems.Add(Regex.Replace(a2.Groups[1].Value, "<[^>]+>", ""));
                                lv1.SubItems.Add(a3s[a3s.Count - 1].Groups[1].Value);


                                Thread.Sleep(1000);
                            }
                            catch (Exception)
                            {

                                continue;
                            }
                          
                        }
                    }
                  
                }
            }



        }

        Thread thread;
        private void button6_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"8zi"))
            {
                MessageBox.Show("验证失败");
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
