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

namespace 时间转八字
{
    public partial class 时间转八字 : Form
    {
        public 时间转八字()
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
                    string t = Regex.Replace(text[i], @"\s+", " ");
                    string[] value = t.Split(new string[] { " " }, StringSplitOptions.None);

                    if (value.Length > 1)
                    {
                        string date=value[0].Replace("/","-")+" "+value[1];

                       int hour= gethour(value[2]);
                        string date2 = date;
                        if(value[3].Trim()=="足球")
                        {
                          date2=  (Convert.ToDateTime(date).AddHours(hour).AddHours(1).AddMinutes(45)).ToString("yyyy-MM-dd HH:mm");
                        }
                        if (value[3].Trim() == "篮球")
                        {
                            date2 = (Convert.ToDateTime(date).AddHours(hour).AddHours(2)).ToString("yyyy-MM-dd HH:mm");
                        }

                        if (value[1] != "")
                        {
                            try
                            {
                                string url = "http://www.wangdailin.com/bzppph.php?ppcont=" + date2+ "&yn=undefined&sex=3";

                                string html = method.GetUrl(url, "utf-8");

                                MatchCollection a1s = Regex.Matches(html, @"<tr class=""bazi""><td></td><td>([\s\S]*?)</td><td>([\s\S]*?)</td><td>([\s\S]*?)</td><td>([\s\S]*?)</td>");


                            

                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                               
                                 lv1.SubItems.Add(Regex.Replace(a1s[0].Groups[1].Value, "<[^>]+>", "") + Regex.Replace(a1s[1].Groups[1].Value, "<[^>]+>", ""));
                                lv1.SubItems.Add(Regex.Replace(a1s[0].Groups[2].Value, "<[^>]+>", "") + Regex.Replace(a1s[1].Groups[2].Value, "<[^>]+>", ""));
                                lv1.SubItems.Add(Regex.Replace(a1s[0].Groups[3].Value, "<[^>]+>", "") + Regex.Replace(a1s[1].Groups[3].Value, "<[^>]+>", ""));
                                lv1.SubItems.Add(Regex.Replace(a1s[0].Groups[4].Value, "<[^>]+>", "") + Regex.Replace(a1s[1].Groups[4].Value, "<[^>]+>", ""));

                                lv1.SubItems.Add(value[2]+"时间 "+date2+"结束");
                                if(value.Length>4)
                                {
                                    lv1.SubItems.Add(value[4]);
                                }
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
            textBox1.Text="";
            //MessageBox.Show(gethour(textBox1.Text).ToString());
            listView1.Items.Clear();
        }


        

        public int gethour(string key)
        {
            string[] text = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                
                string[] area = text[i].Split(new string[] { "," }, StringSplitOptions.None);
               if(area.Length>1)
                {
                    if (area[1].Contains(key.Trim()))
                    {
                        return Convert.ToInt32(area[0]);
                    }
                }
            }
            return 0;   



        }
        private void 时间转八字_Load(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"8ZiY"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();

            }

            #endregion

          


        }
    }
}
