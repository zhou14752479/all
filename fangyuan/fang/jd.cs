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

namespace fang
{
    public partial class jd : Form
    {
        bool zanting = true;
        public jd()
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

                    string[] urls= text.Split(new string[] { "&*&" }, StringSplitOptions.None);
                      string url = "https://search.jd.com/Search?keyword="+ urls[0] + "&enc=utf-8";
                       string html = method.GetUrl(url,"utf-8");
                        Match match = Regex.Match(html, @"data-sku=""([\s\S]*?)""");

                        string URL = "https://item.jd.com/"+match.Groups[1].Value.Trim()+".html";
                        string strhtml = method.GetUrl(URL,"gbk");

                        MatchCollection matches = Regex.Matches(strhtml, @"mbNav-([\s\S]*?)"">([\s\S]*?)</a>");
                        StringBuilder sb = new StringBuilder();
                        foreach (Match item in matches)
                        {
                            sb.Append(item.Groups[2].Value.Trim()+"  ");
                        }
                       
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

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
