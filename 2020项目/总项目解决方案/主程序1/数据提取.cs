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

namespace 主程序1
{
    public partial class 数据提取 : Form
    {
        public 数据提取()
        {
            InitializeComponent();
        }

        bool zanting = true;
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
            string text = streamReader.ReadToEnd();
            string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < array.Length; i++)
            {


                string html = method.GetUrl(array[i], "utf-8");


                Match key = Regex.Match(html, @"<p class=""ncbi-doc-authors"">([\s\S]*?)</p>");

                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                lv1.SubItems.Add(array[i]);
                lv1.SubItems.Add(key.Groups[1].Value);

                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }

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
            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入网址");
                return;
            }

            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
