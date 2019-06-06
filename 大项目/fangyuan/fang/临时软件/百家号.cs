using Microsoft.Win32;
using System;
using System.Collections;
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

namespace fang.临时软件
{
    public partial class 百家号 : Form
    {
        public 百家号()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        bool zanting = true;
        private void skinButton1_Click(object sender, EventArgs e)
        {
            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
            lv1.SubItems.Add(textBox1.Text);
            textBox1.Text = "";
        }



        /// <summary>
        /// 获取第二列
        /// </summary>
        /// <returns></returns>
        public ArrayList getListviewValue1(ListView listview)

        {
            ArrayList values = new ArrayList();

            for (int i = 0; i < listview.Items.Count; i++)
            {
                ListViewItem item = listview.Items[i];

                values.Add(item.SubItems[1].Text);


            }

            return values;

        }

        bool condition=true;
        ArrayList finishes = new ArrayList();


        #region  主函数
        public void run()

        {
            ArrayList lists = getListviewValue1(listView1);



            try

            {

                foreach (string id in lists)
                {

                    toolStripLabel2.Text = id.ToString();
                    string html = method.GetUrl("https://author.baidu.com/pipe?tab=2&app_id=" + id + "&num=20&pagelets[]=article&reqID=1&isspeed=0", "utf-8");


                        string rxg = @"data-event-data=\\""news_([\s\S]*?)\\"" data-thread-id=\\""([\s\S]*?)\\""([\s\S]*?)>   <div class=\\""text\\""><h2>([\s\S]*?)</h2> ";
                        MatchCollection titles = Regex.Matches(html, @"<h2>([\s\S]*?)</h2>");
                        MatchCollection times = Regex.Matches(html, @"<div class=\\""time\\"">([\s\S]*?)</div>");
                        MatchCollection urls = Regex.Matches(html, @"<a href=\\""([\s\S]*?)\\""");
                        MatchCollection matches = Regex.Matches(html, rxg);


                    for (int i = 0; i < matches.Count; i++)
                    {
                        string url = "https://mbd.baidu.com/webpage?type=homepage&action=interact&format=jsonp&params=[{\"user_type\":\"3\",\"dynamic_id\":\"" + matches[i].Groups[1].Value + "\",\"dynamic_type\":\"2\",\"dynamic_sub_type\":\"2001\",\"thread_id\":" + matches[i].Groups[2].Value + ",\"feed_id\":\"" + matches[i].Groups[1].Value + "\"}]&uk=bYnkwu6b6HSuNmRoc92UbA&_=1547616333196&callback=jsonp1";

                        string readHtml = method.GetUrl(url, "utf-8");

                        Match reads = Regex.Match(readHtml, @"read_num"":([\s\S]*?),");

                        if (reads.Groups[1].Value != null && reads.Groups[1].Value != "")
                        {

                            if (comboBox1.Text == "1小时内")
                        {
                            this.condition = (times[i].Groups[1].Value.Contains("分") || times[i].Groups[1].Value.Contains("秒") || times[i].Groups[1].Value.Contains("1小时")) && Convert.ToInt32(reads.Groups[1].Value) >= Convert.ToInt32(textBox4.Text);
                        }

                        else if (comboBox1.Text == "3小时内")
                        {
                            this.condition = (times[i].Groups[1].Value.Contains("分") || times[i].Groups[1].Value.Contains("秒") || times[i].Groups[1].Value.Contains("1小时") || times[i].Groups[1].Value.Contains("2小时") || times[i].Groups[1].Value.Contains("3小时")) && Convert.ToInt32(reads.Groups[1].Value) >= Convert.ToInt32(textBox4.Text);
                        }
                        else if (comboBox1.Text == "8小时内")
                        {
                            this.condition = (times[i].Groups[1].Value.Contains("分") || times[i].Groups[1].Value.Contains("秒") || times[i].Groups[1].Value.Contains("1小时") || times[i].Groups[1].Value.Contains("2小时") || times[i].Groups[1].Value.Contains("3小时") || times[i].Groups[1].Value.Contains("4小时") || times[i].Groups[1].Value.Contains("5小时") || times[i].Groups[1].Value.Contains("6小时") || times[i].Groups[1].Value.Contains("7小时") || times[i].Groups[1].Value.Contains("8小时")) && Convert.ToInt32(reads.Groups[1].Value) >= Convert.ToInt32(textBox4.Text);
                        }
                        else if (comboBox1.Text == "24小时内")
                        {
                            this.condition = (times[i].Groups[1].Value.Contains("分") || times[i].Groups[1].Value.Contains("秒") || times[i].Groups[1].Value.Contains("小时") || times[i].Groups[1].Value.Contains("昨天")) && Convert.ToInt32(reads.Groups[1].Value) >= Convert.ToInt32(textBox4.Text);
                        }
                        else if (comboBox1.Text == "48小时内")
                        {
                            this.condition = (times[i].Groups[1].Value.Contains("分") || times[i].Groups[1].Value.Contains("秒") || times[i].Groups[1].Value.Contains("小时") || times[i].Groups[1].Value.Contains("昨天")) && Convert.ToInt32(reads.Groups[1].Value) >= Convert.ToInt32(textBox4.Text);
                        }
                        else if (comboBox1.Text == "3天内")
                        {
                            if (!times[i].Groups[1].Value.Contains("-"))
                            {
                                this.condition = (times[i].Groups[1].Value.Contains("分") || times[i].Groups[1].Value.Contains("秒") || times[i].Groups[1].Value.Contains("小时") || times[i].Groups[1].Value.Contains("昨天")) && Convert.ToInt32(reads.Groups[1].Value) >= Convert.ToInt32(textBox4.Text);

                            }
                            else if (times[i].Groups[1].Value.Contains("-") && !times[i].Groups[1].Value.Contains("2018") && !times[i].Groups[1].Value.Contains("2017"))
                            {
                                string time = "2019," + times[i].Groups[1].Value.Replace("-", ",").Replace(" ", ",").Replace(":", ",");

                                string[] utimes = time.Split(',');

                                DateTime atime = new DateTime(Convert.ToInt32(utimes[0]), Convert.ToInt32(utimes[1]), Convert.ToInt32(utimes[2]));
                                TimeSpan d3 = DateTime.Now.Subtract(atime);
                                this.condition = d3.Days < 3 && Convert.ToInt32(reads.Groups[1].Value) >= Convert.ToInt32(textBox4.Text);

                            }
                        }
                        else if (comboBox1.Text == "1周内")
                        {
                            if (!times[i].Groups[1].Value.Contains("-"))
                            {
                                this.condition = (times[i].Groups[1].Value.Contains("分") || times[i].Groups[1].Value.Contains("秒") || times[i].Groups[1].Value.Contains("小时") || times[i].Groups[1].Value.Contains("昨天")) && Convert.ToInt32(reads.Groups[1].Value) >= Convert.ToInt32(textBox4.Text);

                            }
                            else if (times[i].Groups[1].Value.Contains("-") && !times[i].Groups[1].Value.Contains("2018") && !times[i].Groups[1].Value.Contains("2017"))
                            {
                                string time = "2019," + times[i].Groups[1].Value.Replace("-", ",").Replace(" ", ",").Replace(":", ",");

                                string[] utimes = time.Split(',');


                                DateTime atime = new DateTime(Convert.ToInt32(utimes[0]), Convert.ToInt32(utimes[1]), Convert.ToInt32(utimes[2]));
                                TimeSpan d3 = DateTime.Now.Subtract(atime);
                                this.condition = d3.Days < 8 && Convert.ToInt32(reads.Groups[1].Value) >= Convert.ToInt32(textBox4.Text);

                            }

                        }
                    }


                        else
                        {
                            this.condition = true;
                        }
                            

                            if (this.condition)
                            {


                                ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv2.SubItems.Add(titles[i].Groups[1].Value);

                                lv2.SubItems.Add(reads.Groups[1].Value);
                                lv2.SubItems.Add(times[i].Groups[1].Value);
                                lv2.SubItems.Add(urls[i].Groups[1].Value);



                                if (listView2.Items.Count - 1 > 1)
                                {
                                    listView2.EnsureVisible(listView2.Items.Count - 1);
                                }
                                //如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。

                                while (this.zanting == false)
                                {
                                    Application.DoEvents();
                                }
                                Thread.Sleep(1000);


                            }

                        }



                    }

                }
            

            catch (Exception ex)
            {
              MessageBox.Show(  ex.ToString());
            }
        }

        #endregion

        private void skinButton8_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {


                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(text[i]);


                }
            }
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            //读取注册码信息才能运行软件！
            RegistryKey rsg = Registry.CurrentUser.OpenSubKey("zhucema"); //true表可修改                
            if (rsg != null && rsg.GetValue("mac") != null)  //如果值不为空
            {
               
                    Thread thread = new Thread(new ThreadStart(run));
                    Control.CheckForIllegalCrossThreadCalls = false;
                    thread.Start();
               
                

            }

            else
            {
                MessageBox.Show("请注册软件！");
                login lg = new login();
                lg.Show();
            }
        }

        private void skinButton7_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Format("{0:g}", DateTime.Now));
        }


        private void skinButton6_Click(object sender, EventArgs e)
        {
            this.listView2.Items.Clear();
        }

        private void 跳转到文章链接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(this.listView2.SelectedItems[0].SubItems[4].Text); 
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            this.zanting = false;
        }

        private void skinButton5_Click(object sender, EventArgs e)
        {
            this.zanting = true;
        }

        private void skinButton7_Click_1(object sender, EventArgs e)
        {
            //this.zanting = false;

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(DateTime.Now - startTime).TotalSeconds; // 相差秒数

            DateTime atime = new DateTime(2004, 1, 1, 15, 36, 05);
            DateTime btime = new DateTime(2004, 1, 2, 15, 36, 05);
            TimeSpan d3 = btime.Subtract(atime);
            MessageBox.Show(d3.Days.ToString());
        }
        //对Listview进行排序
        private void listView2_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (listView2.Columns[e.Column].Tag == null)
            {
                listView2.Columns[e.Column].Tag = true;
            }
            bool tabK = (bool)listView2.Columns[e.Column].Tag;
            if (tabK)
            {
                listView2.Columns[e.Column].Tag = false;
            }
            else
            {
                listView2.Columns[e.Column].Tag = true;
            }
            listView2.ListViewItemSorter = new ListViewSort(e.Column, listView2.Columns[e.Column].Tag);
            //指定排序器并传送列索引与升序降序关键字
            listView2.Sort();//对列表进行自定义排序
        }

        //排序类的定义:
        ///
        ///自定义ListView控件排序函数
        ///
        public class ListViewSort : IComparer
        {
            private int col;
            private bool descK;

            public ListViewSort()
            {
                col = 0;
            }
            public ListViewSort(int column, object Desc)
            {
                descK = (bool)Desc;
                col = column; //当前列,0,1,2...,参数由ListView控件的ColumnClick事件传递
            }
            public int Compare(object x, object y)
            {
                int tempInt = String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
                if (descK)
                {
                    return -tempInt;
                }
                else
                {
                    return tempInt;
                }
            }

        }

        private void 百家号_Load(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.listView1.Items.Remove(this.listView1.SelectedItems[0]);
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
        }
    }
}
