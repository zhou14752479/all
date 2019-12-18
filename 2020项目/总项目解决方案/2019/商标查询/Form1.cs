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

namespace 商标查询
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool zanting = true;
        private void Button6_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(this.run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        public void run()
        {
            try
            {
                StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[]
                {
                    "\r\n"
                }, StringSplitOptions.None);
                for (int i = 0; i < array.Length - 1; i++)
                {
                    this.label1.Text = "正在抓取" + array[i] + "........";
                    string url = "https://api.ipr.kuaifawu.com/xcx/tmsearch/index";
                    string text2 = this.textBox2.Text;
                    text2 = Regex.Replace(text2, "\\d{6,}", array[i].Trim());
                    string input = method.PostUrl(url, text2, "", "utf-8");
                    Match match = Regex.Match(input, "\"MarkName\":\"([\\s\\S]*?)\"");
                    Match match2 = Regex.Match(input, "UnionTypeCode\":([\\s\\S]*?),");
                    Match match3 = Regex.Match(input, "\"StateDate2017\":\"([\\s\\S]*?)\"");
                    Match match4 = Regex.Match(input, "\"AppPerson\":\"([\\s\\S]*?)\"");
                    Match match5 = Regex.Match(input, "\"Addr\":\"([\\s\\S]*?)\"");
                    Match match6 = Regex.Match(input, "\"Addr\":\"([\\s\\S]*?)省");
                    Match match7 = Regex.Match(input, "省([\\s\\S]*?)市");
                    Match match8 = Regex.Match(input, "\"AppDate\":\"([\\s\\S]*?)\"");
                    Match match9 = Regex.Match(input, "\"AgentName\":\"([\\s\\S]*?)\"");
                    string url2 = "https://zhiqingzhe.zqz510.com/api/tq/gti?uid=18f0514dacf94e9899136734417b7c83&an=" + array[i].Trim() + "&ic=" + match2.Groups[1].Value.Trim();
                    string url3 = method.GetUrl(url2, "utf-8");
                    Match match10 = Regex.Match(url3, "\"gglx\":\"([\\s\\S]*?)\",\"pd\":\"([\\s\\S]*?)\"");
                    ListViewItem listViewItem = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
                    listViewItem.SubItems.Add(array[i]);
                    listViewItem.SubItems.Add(match.Groups[1].Value);
                    listViewItem.SubItems.Add(match2.Groups[1].Value);
                    listViewItem.SubItems.Add(match4.Groups[1].Value);
                    listViewItem.SubItems.Add("中国");
                    listViewItem.SubItems.Add(match6.Groups[1].Value);
                    listViewItem.SubItems.Add(match7.Groups[1].Value);
                    listViewItem.SubItems.Add(match5.Groups[1].Value);
                    listViewItem.SubItems.Add(match8.Groups[1].Value);
                    listViewItem.SubItems.Add(match9.Groups[1].Value);
                    listViewItem.SubItems.Add(match10.Groups[2].Value + match10.Groups[1].Value);
                    while (!this.zanting)
                    {
                        Application.DoEvents();
                    }
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            bool flag = dialogResult == DialogResult.OK;
            if (flag)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
