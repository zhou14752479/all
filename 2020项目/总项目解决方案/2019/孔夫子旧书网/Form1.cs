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

namespace 孔夫子旧书网
{
    public partial class Form1 : Form
    {
        public Form1()
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
        #region 去掉路径中非法字符
        public string removeValid(string illegal)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                illegal = illegal.Replace(c.ToString(), "");
            }
            return illegal;
        }

        #endregion
        bool zanting = true;
        bool status = true;
        string path = AppDomain.CurrentDomain.BaseDirectory + "images\\";
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("请输入代理IP地址");
                return;
            }

            if (textBox3.Text == "")
            {
                MessageBox.Show("请输入ISBN号");
                return;
            }


            try
            {
                string[] text = textBox3.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {

                    string url = "http://search.kongfz.com/item_result/?select=0&key="+ text[i].Trim()+ "&status=1";
                    string html = method.GetUrlwithIP(url, this.IP);

                    Match bookUrl = Regex.Match(html, @"<a class=""img-box"" target=""_blank""([\s\S]*?)href=""([\s\S]*?)""");
                    if (bookUrl.Groups[2].Value == "")
                    {
                        textBox1.Text += text[i]+"\r\n";
                      
                        
                    }
                    else
                    {
                        string bookHtml = method.GetUrlwithIP(bookUrl.Groups[2].Value, this.IP);

                        Match a1 = Regex.Match(bookHtml, @"""bookName"":""([\s\S]*?)""");
                        Match a2 = Regex.Match(bookHtml, @"""author"":""([\s\S]*?)""");
                        Match a3 = Regex.Match(bookHtml, @"""press"":""([\s\S]*?)""");
                        Match a4 = Regex.Match(bookHtml, @"""pubDate"":""([\s\S]*?)""");
                        Match a5 = Regex.Match(bookHtml, @"""edition"":""([\s\S]*?)""");
                        Match a6 = Regex.Match(bookHtml, @"""isbn"":""([\s\S]*?)""");
                        Match a7 = Regex.Match(bookHtml, @"""price"":""([\s\S]*?)""");
                        Match a8 = Regex.Match(bookHtml, @"""binding"":""([\s\S]*?)""");
                        Match a9 = Regex.Match(bookHtml, @"""pageSize"":""([\s\S]*?)""");
                        Match a10 = Regex.Match(bookHtml, @"""usedPaper"":""([\s\S]*?)""");
                        Match a11 = Regex.Match(bookHtml, @"""pageNum"":""([\s\S]*?)""");
                        Match a12 = Regex.Match(bookHtml, @"""wordNum"":""([\s\S]*?)""");
                        Match a13 = Regex.Match(bookHtml, @"""language"":""([\s\S]*?)""");
                        Match a14 = Regex.Match(bookHtml, @"""series"":""([\s\S]*?)""");
                        Match a15 = Regex.Match(bookHtml, @"""catName"":""([\s\S]*?)""");
                        Match a16 = Regex.Match(bookHtml, @"""contentIntroduction"":""([\s\S]*?)""");
                        Match a17 = Regex.Match(bookHtml, @"""authorIntroduction"":""([\s\S]*?)""");
                        Match a18 = Regex.Match(bookHtml, @"""directory"":""([\s\S]*?)""");

                        Match picurl = Regex.Match(bookHtml, @"<meta property=""og:image"" content=""([\s\S]*?)""");
                        string picName = Regex.Replace(picurl.Groups[1].Value, ".*/", "");  //去标签

                        if (picurl.Groups[1].Value.Contains("jpg"))
                        {
                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            listViewItem.SubItems.Add(Unicode2String(a1.Groups[1].Value));
                            listViewItem.SubItems.Add(Unicode2String(a2.Groups[1].Value));
                            listViewItem.SubItems.Add(Unicode2String(a3.Groups[1].Value));
                            listViewItem.SubItems.Add(Unicode2String(a4.Groups[1].Value));
                            listViewItem.SubItems.Add(Unicode2String(a5.Groups[1].Value));
                            listViewItem.SubItems.Add(Unicode2String(a6.Groups[1].Value));
                            listViewItem.SubItems.Add(Unicode2String(a7.Groups[1].Value));
                            listViewItem.SubItems.Add(Unicode2String(a8.Groups[1].Value));
                            listViewItem.SubItems.Add(Unicode2String(a9.Groups[1].Value));
                            listViewItem.SubItems.Add(Unicode2String(a10.Groups[1].Value));
                            listViewItem.SubItems.Add(Unicode2String(a11.Groups[1].Value));
                            listViewItem.SubItems.Add(Unicode2String(a12.Groups[1].Value));
                            listViewItem.SubItems.Add(Unicode2String(a13.Groups[1].Value));
                            listViewItem.SubItems.Add(Unicode2String(a14.Groups[1].Value));
                            listViewItem.SubItems.Add(Unicode2String(a15.Groups[1].Value));
                            listViewItem.SubItems.Add(Unicode2String(a16.Groups[1].Value).Trim());
                            listViewItem.SubItems.Add(Unicode2String(a17.Groups[1].Value).Trim());
                            listViewItem.SubItems.Add(Unicode2String(a18.Groups[1].Value).Trim());
                            listViewItem.SubItems.Add(picName);


                            method.downloadFile(picurl.Groups[1].Value, path, picName + ".jpg","");

                        }

                        else
                        {
                            textBox1.Text += text[i] + "\r\n";

                        }
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                        Thread.Sleep(100);
                        
                    }
                   
                   
                }
            }
            catch (Exception ex)
            {

                textBox1.Text = ex.ToString();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = true;
            getIp();
            timer1.Start();
            button1.Enabled = false;
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

        }

     

        private void Button2_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
           
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", path);
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            button1.Enabled = true;
        }

        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            getIp();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
            
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            method.ListViewToCSV(listView1, true);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
