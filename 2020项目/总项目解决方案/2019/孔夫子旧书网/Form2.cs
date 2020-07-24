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

namespace 孔夫子旧书网
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        #region  代理iP

        public void getIp()
        {
            string ahtml = method.GetUrl(textBox2.Text, "utf-8");
            this.IP = ahtml.Trim();

        }
        #endregion
        public string IP = "";
        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        bool zanting = true;
        bool status = true;
        string path = AppDomain.CurrentDomain.BaseDirectory + "images\\";
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入店铺地址");
                return;
            }


            try
            {
                for (int i = 0; i < 99999; i++)
                {
                    Match shopid = Regex.Match(textBox1.Text, @"\d{4,}");


                    string url = "http://shop.kongfz.com/"+shopid.Groups[0].Value+"/all/0_50_0_0_"+i+"_sort_desc_0_0/";
                    string html = method.GetUrl(url, "utf-8");

                    MatchCollection bookUrls = Regex.Matches(html, @"<div class=""item-row clearfix([\s\S]*?)<a href=""([\s\S]*?)""");

                    for (int j = 0; j < bookUrls.Count; j++)
                    {

                        string bookHtml = method.GetUrl(bookUrls[j].Groups[2].Value,"utf-8");

                        Match a1 = Regex.Match(bookHtml, @"""itemName"":""([\s\S]*?)""");
                        Match a2 = Regex.Match(bookHtml, @"出版社：([\s\S]*?) ");
                        Match a3 = Regex.Match(bookHtml, @"DETAIL.isbn = ""([\s\S]*?)""");
                        Match a4 = Regex.Match(bookHtml, @"""price"":([\s\S]*?),");
                        Match a5 = Regex.Match(bookHtml, @"""oriPrice"":([\s\S]*?),");
                        Match a6 = Regex.Match(bookHtml, @"quality=""([\s\S]*?)""");

                        Match a7 = Regex.Match(bookHtml, @"class=""store-count"">([\s\S]*?)</i>");
                        Match a8 = Regex.Match(bookHtml, @"state-one"">仅([\s\S]*?)件");

                        string isbn = "";
                        string kucun = a7.Groups[1].Value;
                        if (a7.Groups[1].Value == "" && a8.Groups[1].Value != "")
                        {
                            kucun = "1";
                        }


                        if (Unicode2String(a3.Groups[1].Value) != "")
                        {
                            isbn = "ISBN" + Unicode2String(a3.Groups[1].Value);
                        }
                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            listViewItem.SubItems.Add(Unicode2String(a1.Groups[1].Value));
                            listViewItem.SubItems.Add(Unicode2String(a2.Groups[1].Value));
                            listViewItem.SubItems.Add(isbn);
                            listViewItem.SubItems.Add(Unicode2String(a4.Groups[1].Value));
                            listViewItem.SubItems.Add(Unicode2String(a5.Groups[1].Value));
                            listViewItem.SubItems.Add(Unicode2String(a6.Groups[1].Value));
                        listViewItem.SubItems.Add(kucun);



                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                        Thread.Sleep(500);

                    }


                }
            }
            catch (Exception ex)
            {

                textBox1.Text = ex.ToString();
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"kongfuzidianpu"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion
            status = true;
           // getIp();
            //timer1.Start();
            button1.Enabled = false;
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出吗？", "退出询问"
         , MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;//告诉窗体关闭这个任务取消

            }
            else
            {

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
