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
using myDLL;
using OpenQA.Selenium;

namespace 主程序selenium
{
    public partial class 亚马逊评论 : Form
    {
        public 亚马逊评论()
        {
            InitializeComponent();
        }
        bool status = true;
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            status = true;
            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入文本");
                return;
            }

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 亚马逊评论_Load(object sender, EventArgs e)
        {
            #region 通用检测


            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"hGRLg"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion

            driver = function.getdriver(false, false);
            driver.Navigate().GoToUrl("https://www.amazon.com/-/zh/product-reviews/B082P77FS5/ref=cm_cr_arp_d_viewopt_sr?ie=UTF8&reviewerType=all_reviews&filterByStar=three_star&pageNumber=1");
        }
      
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                StreamReader sr = new StreamReader(openFileDialog1.FileName, method.EncodingType.GetTxtType(textBox1.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(DateTime.Now.ToString("yyyy-MM-dd"));
                    lv1.SubItems.Add(text[i]);
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }


        IWebDriver driver;
        public void run()
        {


            
            StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text2 = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text2.Length; i++)
            {
                if (text2[i] != "")
                {
                    string[] stars = { "five_star", "four_star", "three_star", "two_star", "one_star" };

                    for (int j = 0; j < stars.Length; j++)
                    {
                        string url = "https://www.amazon.com/-/zh/product-reviews/"+text2[i]+"/ref=cm_cr_arp_d_viewopt_sr?ie=UTF8&reviewerType=all_reviews&filterByStar="+stars[j]+"&pageNumber=1";
                        driver.Navigate().GoToUrl(url);
                        Thread.Sleep(1000);
                    
                        string html = driver.PageSource;
                        //textBox2.Text = html;

                        string values = Regex.Match(html, @"a-spacing-base a-size-base"">([\s\S]*?)</div>").Groups[1].Value.Replace("\\n", "").Trim();
                        values = values.Replace("total", "").Replace("|", "").Replace("总评分", "#").Replace("带评论", "").Replace("<span>", "").Replace("</span>", "").Replace(",","").Trim();
                       
                        string[] text= values.Split(new string[] { "#" }, StringSplitOptions.None);

                        if (text.Length < 1)
                        {

                            label2.Text = "查询失败";
                            continue;
                        }

                        if (stars[j] == "five_star")
                        {
                            listView1.Items[i].SubItems[3].Text = text[0];
                            listView1.Items[i].SubItems[4].Text = text[1];
                        }
                        if (stars[j] == "four_star")
                        {
                            listView1.Items[i].SubItems[5].Text = text[0];
                            listView1.Items[i].SubItems[6].Text = text[1];
                        }
                        if (stars[j] == "three_star")
                        {
                            listView1.Items[i].SubItems[7].Text = text[0];
                            listView1.Items[i].SubItems[8].Text = text[1];
                        }
                        if (stars[j] == "two_star")
                        {
                            listView1.Items[i].SubItems[9].Text = text[0];
                            listView1.Items[i].SubItems[10].Text = text[1];
                        }
                        if (stars[j] == "one_star")
                        {
                            listView1.Items[i].SubItems[11].Text = text[0];
                            listView1.Items[i].SubItems[12].Text = text[1];
                        }

                        if (status == false)
                            return;
                    }

                }
                Thread.Sleep(2000);
            }
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
        }

        private void 亚马逊评论_FormClosing(object sender, FormClosingEventArgs e)
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
