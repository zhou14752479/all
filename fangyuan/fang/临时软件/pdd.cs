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

namespace fang.临时软件
{
    public partial class pdd : Form
    {
        bool zanting = true;

        public pdd()
        {
            InitializeComponent();
        }

        public string[] ReadText()
        {
            string currentDirectory = Environment.CurrentDirectory;
            StreamReader streamReader = new StreamReader(this.textBox1.Text);
            string text = streamReader.ReadToEnd();
            return text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        }


        public void run()
        {
            try
            {
                string[] array = this.ReadText();
                foreach (string text in array)
                {

                    string[] urls = text.Split(new string[] { "&*&" }, StringSplitOptions.None);
                    string url = "https://mms.pinduoduo.com/vodka/v2/mms/search/categories?keyword=" + urls[0];

                    string cookie =textBox3.Text.Trim();
                    string strhtml = method.GetUrlWithCookie(url, "utf-8",cookie);

                    Match match1 = Regex.Match(strhtml, @"cat_name_1"":""([\s\S]*?)""");
                    Match match2 = Regex.Match(strhtml, @"cat_name_2"":""([\s\S]*?)""");
                    Match match3 = Regex.Match(strhtml, @"cat_name_3"":""([\s\S]*?)""");
                    Match match4 = Regex.Match(strhtml, @"cat_name_4"":""([\s\S]*?)""");



                    StringBuilder sb = new StringBuilder();
                    sb.Append(match1.Groups[1].Value.Trim() + "  "+match2.Groups[1].Value.Trim() + "  "+match3.Groups[1].Value.Trim() + "  "+match4.Groups[1].Value.Trim());
                    

                    ListViewItem listViewItem = this.listView1.Items.Add(text);
                    listViewItem.SubItems.Add(sb.ToString());
                    this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
                    Application.DoEvents();
                    Thread.Sleep(Convert.ToInt32(textBox2.Text));

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                MessageBox.Show("请先登陆拼多多后台！");
                return;
            }

            Thread thread = new Thread(new ThreadStart(this.run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.zanting = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.zanting = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            webBrowser web = new webBrowser("https://mms.pinduoduo.com/login/");
            web.Show();
        }

        private void pdd_MouseEnter(object sender, EventArgs e)
        {
          
        }

        private void splitContainer1_Panel1_MouseEnter(object sender, EventArgs e)
        {
            textBox3.Text = webBrowser.cookie;
        }

        private void listView1_MouseEnter(object sender, EventArgs e)
        {
            textBox3.Text = webBrowser.cookie;
        }

        private void pdd_Load(object sender, EventArgs e)
        {

        }
    }
}
