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
using MySql.Data.MySqlClient;

namespace 搜索引擎
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        bool zanting = true;
       

        bool status1 = false;
        bool status2 = false;
        bool status3 = false;
        bool status4 = false;

        #region  读取数据插入数据库

        public void insertData(string a, string b, string c, string d)
        {

            try
            {

                string constr = "Host =" + textBox1.Text.Trim() + ";Database=" + textBox2.Text.Trim() + ";Username=" + textBox3.Text.Trim() + ";Password=" + textBox4.Text.Trim();
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO '" + textBox6.Text.Trim() + " ' ('" + textBox7.Text.Trim() + " ','" + textBox8.Text.Trim() + " ','" + textBox9.Text.Trim() + " ','" + textBox10.Text.Trim() + " ')VALUES('" + a + " ', '" + b + " ','" + c + " ', '" + d + "')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {


                }

                mycon.Close();



            }

            catch (System.Exception ex)
            {
                ex.ToString();
            }


        }

        #endregion
       
        #region  百度获取
        public void baidu()
        {
            try
            {
                status1 = false;
                string[] keywords = textBox5.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string key in keywords)

                {
                    string[] keyword1 = key.Split(new string[] { "," }, StringSplitOptions.None);

                    foreach (string keyword in keyword1)
                    {


                        textBox11.Text += "正在抓取百度搜索" + keyword;
                        for (int i = 0; i < 9999; i = i + 10)
                        {

                            string url = "https://www.baidu.com/s?ie=utf-8&cl=2&medium=2&rtt=1&bsst=1&rsv_dl=news_b_pn&tn=news&wd=" + keyword + "&tfflag=0&x_bfe_rqs=03E80&x_bfe_tjscore=0.002154&tngroupname=organic_news&pn=" + i;
                            string html = method.GetUrl(url, "utf-8");

                            MatchCollection ids = Regex.Matches(html, @"baijiahao\.baidu\.com([\s\S]*?)""");


                            if (ids.Count == 0)
                                break;

                            for (int j = 0; j < ids.Count; j++)
                            {

                                string URL = "http://baijiahao.baidu.com" + ids[j].Groups[1].Value;

                                string strhtml = method.GetUrl(URL, "utf-8");



                                Match a1 = Regex.Match(strhtml, @"<h2>([\s\S]*?)</h2>");
                                Match a2 = Regex.Match(strhtml, @"dateUpdate"" content=""([\s\S]*?)""");
                                Match a3 = Regex.Match(strhtml, @"uthor-name"">([\s\S]*?)<");
                                Match a4 = Regex.Match(strhtml, @"<div class=""article-content"">([\s\S]*?)</div>");

                                //DateTime dt = Convert.ToDateTime(a2.Groups[1].Value);
                                //if (dateTimePicker1.Value < dt && dt < dateTimePicker2.Value)
                                //{
                                if (checkBox1.Checked == true)
                                {
                                    insertData(a1.Groups[1].Value, a2.Groups[1].Value, a3.Groups[1].Value, a4.Groups[1].Value);
                                }


                                ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                listViewItem.SubItems.Add(a1.Groups[1].Value);
                                listViewItem.SubItems.Add(a2.Groups[1].Value);
                                listViewItem.SubItems.Add(a3.Groups[1].Value);
                                listViewItem.SubItems.Add(Regex.Replace(a4.Groups[1].Value, "<(?!img|p|/p)[^>]*>", "").Trim());
                                listViewItem.SubItems.Add(keyword);


                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }

                                Thread.Sleep(100);
                                //    }


                            }
                        }

                    }
                }
                status1 = true;
            }




            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #endregion

        #region  搜狗获取
        public void sougou()
        {
            try
            {

                status2 = false;
                string[] keywords = textBox5.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string key in keywords)

                {
                    string[] keyword1 = key.Split(new string[] { "," }, StringSplitOptions.None);

                    foreach (string keyword in keyword1)
                    {
                        textBox11.Text += "正在抓取搜狗搜索" + keyword;
                        for (int i = 0; i < 9999; i = i + 1)
                        {

                            string url = "https://news.sogou.com/news?mode=1&media=&query=site%3Asohu.com " + keyword + "&time=0&clusterId=&sort=1&page=" + i + "&p=42230305&dp=1";
                            string html = method.GetUrl(url, "gbk");

                            MatchCollection urls = Regex.Matches(html, @"<h3 class=""vrTitle"">([\s\S]*?)<a href=""([\s\S]*?)""");


                            if (urls.Count == 0)
                                break;

                            for (int j = 0; j < urls.Count; j++)
                            {


                                string strhtml = method.GetUrl(urls[j].Groups[2].Value, "utf-8");


                                Match a1 = Regex.Match(strhtml, @"<title>([\s\S]*?)</title>");
                                Match a2 = Regex.Match(strhtml, @"dateUpdate"" content=""([\s\S]*?)""");
                                Match a3 = Regex.Match(strhtml, @"mediaid"" content=""([\s\S]*?)""");
                                Match a4 = Regex.Match(strhtml, @"<p data-role=""original-title"" style=""display:none"">([\s\S]*?)</article>");

                                //DateTime dt = Convert.ToDateTime(a2.Groups[1].Value);
                                //if (dateTimePicker1.Value < dt && dt < dateTimePicker2.Value)
                                //{
                                if (checkBox1.Checked == true)
                                {
                                    insertData(a1.Groups[1].Value, a2.Groups[1].Value, a3.Groups[1].Value, a4.Groups[1].Value);
                                }


                                ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                listViewItem.SubItems.Add(a1.Groups[1].Value);
                                listViewItem.SubItems.Add(a2.Groups[1].Value);
                                listViewItem.SubItems.Add(a3.Groups[1].Value);
                                listViewItem.SubItems.Add(Regex.Replace(a4.Groups[1].Value, "<(?!img|p|/p)[^>]*>", "").Replace("<!-- 政务处理 -->", "").Trim());
                                listViewItem.SubItems.Add(keyword);



                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }

                                Thread.Sleep(1000);

                                //    }


                            }
                        }
                    }
                }
                status2 = true;
            }




            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #endregion

        
        #region  360获取
        public void so360()
        {
            try
            {

                status3 = false;
                string[] keywords = textBox5.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string key in keywords)

                {
                    string[] keyword1 = key.Split(new string[] { "," }, StringSplitOptions.None);

                    foreach (string keyword in keyword1)
                    {
                        textBox11.Text += "正在抓取360搜索" + keyword;
                        for (int i = 0; i < 9999; i = i + 1)
                        {

                            string url = "https://news.sogou.com/news?mode=1&media=&query=site:qq.com " + keyword + "&time=0&clusterId=&sort=1&page=" + i + "&p=42230305&dp=1";
                            string html = method.GetUrl(url, "gbk");

                            MatchCollection urls = Regex.Matches(html, @"<h3 class=""vrTitle"">([\s\S]*?)<a href=""([\s\S]*?)""");


                            if (urls.Count == 0)
                                break;

                            for (int j = 0; j < urls.Count; j++)
                            {


                                string strhtml = method.GetUrl(urls[j].Groups[2].Value, "gb2312");



                                Match a1 = Regex.Match(strhtml, @"<title>([\s\S]*?)_");
                                Match a2 = Regex.Match(strhtml, @"pubtime:'([\s\S]*?)'");
                                Match a3 = Regex.Match(strhtml, @"jgname"">([\s\S]*?)</span>");
                                Match a4 = Regex.Match(strhtml, @"<div class=""content-article"">([\s\S]*?)<div id=""Status""></div>");

                                //DateTime dt = Convert.ToDateTime(a2.Groups[1].Value);
                                //if (dateTimePicker1.Value < dt && dt < dateTimePicker2.Value)
                                //{
                                if (checkBox1.Checked == true)
                                {
                                    insertData(a1.Groups[1].Value, a2.Groups[1].Value, a3.Groups[1].Value, a4.Groups[1].Value);
                                }


                                ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                listViewItem.SubItems.Add(a1.Groups[1].Value);
                                listViewItem.SubItems.Add(a2.Groups[1].Value);
                                listViewItem.SubItems.Add(a3.Groups[1].Value);
                                listViewItem.SubItems.Add(Regex.Replace(a4.Groups[1].Value, "<(?!img|p|/p)[^>]*>", "").Trim());
                                listViewItem.SubItems.Add(keyword);


                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }

                                Thread.Sleep(100);
                                //    }

                            }
                        }
                    }
                }
                status3 = true;
            }




            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #endregion

        #region  必应获取
        public void biying()
        {
            try
            {


                status4 = false;
                string[] keywords = textBox5.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string key in keywords)

                {
                    string[] keyword1 = key.Split(new string[] { "," }, StringSplitOptions.None);

                    foreach (string keyword in keyword1)
                    {
                        textBox11.Text += "正在抓取必应搜索" + keyword;
                        for (int i = 0; i < 9999; i = i + 1)
                        {

                            string url = "https://news.sogou.com/news?mode=1&media=&query=site:sina.com.cn " + keyword + "&time=0&clusterId=&sort=1&page=" + i + "&p=42230305&dp=1";
                            string html = method.GetUrl(url, "gbk");

                            MatchCollection urls = Regex.Matches(html, @"<h3 class=""vrTitle"">([\s\S]*?)<a href=""([\s\S]*?)""");


                            if (urls.Count == 0)
                                break;

                            for (int j = 0; j < urls.Count; j++)
                            {


                                string strhtml = method.GetUrl(urls[j].Groups[2].Value, "utf-8");



                                Match a1 = Regex.Match(strhtml, @"<title>([\s\S]*?)_");
                                Match a2 = Regex.Match(strhtml, @"<span class=""date"">([\s\S]*?)</span>");
                                Match a3 = Regex.Match(strhtml, @"rel=""nofollow"">([\s\S]*?)</a>");
                                Match a4 = Regex.Match(strhtml, @"<!-- 正文 start -->([\s\S]*?)<!-- 正文 end -->");


                                if (checkBox1.Checked == true)
                                {
                                    insertData(a1.Groups[1].Value, a2.Groups[1].Value, a3.Groups[1].Value, a4.Groups[1].Value);
                                }


                                ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                listViewItem.SubItems.Add(a1.Groups[1].Value);
                                listViewItem.SubItems.Add(a2.Groups[1].Value);
                                listViewItem.SubItems.Add(a3.Groups[1].Value);
                                listViewItem.SubItems.Add(Regex.Replace(a4.Groups[1].Value, "<(?!img|p|/p)[^>]*>", "").Trim());
                                listViewItem.SubItems.Add(keyword);



                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }

                                Thread.Sleep(100);
                                //    }


                            }
                        }


                    }
                }
                status4 = true;
            }




            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #endregion

        /// <summary>
        /// 判断相似度
        /// </summary>
        /// <returns></returns>
        public bool panduan()
        {
            return false;
        }
        private void Main_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            timer1.Start();

          
               
                if (checkedListBox1.GetItemChecked(0) == true)
                {
                    Thread thread = new Thread(new ThreadStart(baidu));
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
                if (checkedListBox1.GetItemChecked(1) == true)
                {
                    Thread thread = new Thread(new ThreadStart(sougou));
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
                if (checkedListBox1.GetItemChecked(2) == true)
                {
                    Thread thread = new Thread(new ThreadStart(so360));
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
                if (checkedListBox1.GetItemChecked(3) == true)
                {
                    Thread thread = new Thread(new ThreadStart(biying));
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
              
            

            
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
             
        }


        public void export()
        {
            if (status1 == true && status2 == true && status3 == true && status4 == true)
            {

                timer1.Stop();
                string[] keywords = textBox5.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string key in keywords)

                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + key + "\\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path); //创建文件夹
                    }
                    string filename = path + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Trim() + ".xlsx";

                    method.DataTableToExcelTime(method.listViewToDataTableSx(this.listView1, key), "Sheet1", true, filename);
                }

                textBox11.Text += "数据导出完成";
            }

       }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            export();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            zanting = true;
            button1.Enabled = true;
        }
    }
}
