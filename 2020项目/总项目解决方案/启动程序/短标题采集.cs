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

namespace 启动程序
{
    public partial class 短标题采集 : Form
    {
        public 短标题采集()
        {
            InitializeComponent();
        }

        bool zanting = true;
        bool status = true;
        #region  京东品牌帮随机
        public void run()
        {

            try
            {
                Random ran = new Random();
                int RandKey = ran.Next(1, 21000);

                string Url = "http://yp.jd.com/brand_sitemap_"+ RandKey + ".html";

                    string html = method.GetUrl(Url, "utf-8"); ;  //定义的GetRul方法 返回 reader.ReadToEnd()
                    MatchCollection title = Regex.Matches(html, @"<font size=""2"">([\s\S]*?)</font>");

                    foreach (Match item in title)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(item.Groups[1].Value);
                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);
                        }
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                    if (status == false)
                        return;
                    }
                      

              
               

            }


            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        #endregion


        #region  京东搜索
        public void run1()
        {

            try
            {
                for (int i = 1; i < 750; i++)
                {

                    string Url = "https://so.m.jd.com/ware/search.action?keyword=%E6%96%87%E5%85%B7&keywordVal=&page=" + i;

                    string html = method.GetUrl(Url, "utf-8"); ;  //定义的GetRul方法 返回 reader.ReadToEnd()
                    MatchCollection title = Regex.Matches(html, @"""warename"": ""([\s\S]*?) ");

                    foreach (Match item in title)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(item.Groups[1].Value);
                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);
                        }
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                    }



                }

            }


            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        #endregion
        private void 短标题采集_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"duanbiaoti"))
            {   
                status = true;
                button1.Enabled = false;
                zanting = true;
                Thread thread1 = new Thread(new ThreadStart(run));
                thread1.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
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
            status = false;
            button1.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
