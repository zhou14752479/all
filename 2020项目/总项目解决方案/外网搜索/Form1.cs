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
using helper;
using MySql.Data.MySqlClient;

namespace 外网搜索
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool zanting = true;
        bool status = true;
        ArrayList finishes = new ArrayList();
        int page = 0;
        /// <summary>
        /// 谷歌
        /// </summary>
        public void google()

        {


            try
            {
                string[] array = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] != "")
                    {
                        for (int j = 0; j < page; j++)
                        {
                            string URL = "https://www.google.com/search?q=" + System.Web.HttpUtility.UrlEncode(array[i]) + "&start=" + j + "0";

                            string html = method.GetUrl(URL, "utf-8");
                            MatchCollection urls = Regex.Matches(html, @"<div class=""r""><a href=""([\s\S]*?)""");

                            if (urls.Count == 0)
                            {
                                break;
                            }
                            foreach (Match url in urls)
                            {
                                if (!finishes.Contains(url.Groups[1].Value))
                                {
                                    finishes.Add(url.Groups[1].Value);
                                    Match url2 = Regex.Match(url.Groups[1].Value, @"\/\/([\s\S]*?)\/");
                                    if (url2.Groups[1].Value.Contains("www"))
                                    {
                                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString());
                                        lv1.SubItems.Add(url2.Groups[1].Value);
                                    }
                                    else
                                    {
                                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString());
                                        lv1.SubItems.Add("www." + url2.Groups[1].Value);
                                    }

                                    while (this.zanting == false)
                                    {
                                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                    }

                                    if (status == false)
                                    {
                                        return;
                                    }
                                }
                            }


                        }

                        Thread.Sleep(1000);


                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 雅虎
        /// </summary>
        public void yahoo()

        {


            try
            {
                string[] array = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] != "")
                    {
                        for (int j = 0; j < page; j++)
                        {
                            string URL = "https://hk.search.yahoo.com/search?p="+System.Web.HttpUtility.UrlEncode(array[i])+"&pz=10&fr=sfp&fr2=sb-top-zh.search&b="+j+"1&pz=10&xargs=0";

                            string html = method.GetUrl(URL, "utf-8");
                            
                            MatchCollection urls = Regex.Matches(html, @"va-top"">([\s\S]*?)</span>");

                            if (urls.Count == 0)
                            {
                                break;
                            }
                            foreach (Match url in urls)
                            {
                                if (!finishes.Contains(url.Groups[1].Value))
                                {
                                    finishes.Add(url.Groups[1].Value);
                                    if (url.Groups[1].Value.Contains("bai") || url.Groups[1].Value.Contains("gov"))
                                    {

                                    }

                                    else if (url.Groups[1].Value.Contains("www"))
                                    {
                                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString());
                                        lv1.SubItems.Add(Regex.Replace(url.Groups[1].Value, "/.*", ""));
                                    }
                                    else
                                    {
                                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString());
                                        lv1.SubItems.Add(Regex.Replace(url.Groups[1].Value, "/.*", ""));
                                    }
                                    while (this.zanting == false)
                                    {
                                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                    }

                                    if (status == false)
                                    {
                                        return;
                                    }

                                }
                            }

                        }

                        Thread.Sleep(1000);


                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// 必应
        /// </summary>
        public void bing()

        {


            try
            {
                string[] array = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] != "")
                    {
                        for (int j = 0; j < page; j++)
                        {
                            string URL = "https://search.yahoo.com/search?p=" + System.Web.HttpUtility.UrlEncode(array[i]) + "&pz=10&fr=sfp&fr2=sb-top-zh.search&b=" + j + "1&pz=10&xargs=0";

                            string html = method.GetUrl(URL, "utf-8");

                            MatchCollection urls = Regex.Matches(html, @"lh-17"">([\s\S]*?)</span>");

                            if (urls.Count == 0)
                            {
                                break;
                            }
                            foreach (Match url in urls)
                            {
                                if (!finishes.Contains(url.Groups[1].Value))
                                {
                                    finishes.Add(url.Groups[1].Value);
                                    if (url.Groups[1].Value.Contains("bai") || url.Groups[1].Value.Contains("gov"))
                                    {

                                    }

                                    else if (url.Groups[1].Value.Contains("www"))
                                    {
                                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString());
                                        lv1.SubItems.Add(Regex.Replace(url.Groups[1].Value, "/.*", ""));
                                    }
                                    else
                                    {
                                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString());
                                        lv1.SubItems.Add(Regex.Replace(url.Groups[1].Value, "/.*", ""));
                                    }

                                    while (this.zanting == false)
                                    {
                                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                    }

                                    if (status == false)
                                    {
                                        return;
                                    }
                                }


                            }
                        }

                        Thread.Sleep(1000);


                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            page= Convert.ToInt32(textBox2.Text.Trim());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = true;
            button1.Enabled = false;
            if (checkBox1.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(google));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            if (checkBox2.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(yahoo));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            if (checkBox3.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(bing));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
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

        private void button5_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
            method.ListviewToTxt(listView1,1);
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
