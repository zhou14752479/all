using System;
using System.Collections;
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

namespace main._2019_7
{
    public partial class 网站查询 : Form
    {
        public 网站查询()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

        }
        string[] urls = { };
        public void ReadText()
        {

            StreamReader streamReader = new StreamReader(this.textBox1.Text);
            string text = streamReader.ReadToEnd();
            urls= text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        }
        private void 网站查询_Load(object sender, EventArgs e)
        {

        }
        bool zanting = true;
        ArrayList finishes = new ArrayList();

        #region 主程序
        public void run()
        {

            try
            {
               

                foreach (string url in urls)
                {
                    if (!finishes.Contains(url))
                    {
                        finishes.Add(url);
                        if (url.Contains("@"))
                        {
                            string Url = url.Split('@')[1];

                            string html = method.GetUrl("http://www." + Url, "utf-8");

                            Match title = Regex.Match(html, @"<title>([\s\S]*?)</title>", RegexOptions.IgnoreCase);
                            Match keyword = Regex.Match(html, @"<meta name=""keywords"" content=""([\s\S]*?)""", RegexOptions.IgnoreCase);
                            Match description = Regex.Match(html, @"<meta name=""description"" content=""([\s\S]*?)""", RegexOptions.IgnoreCase);

                            if (title.Groups[1].Value != "")
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                                lv1.SubItems.Add(title.Groups[1].Value);
                                lv1.SubItems.Add(keyword.Groups[1].Value);
                                lv1.SubItems.Add(description.Groups[1].Value);
                                lv1.SubItems.Add(url);
                                if (listView1.Items.Count > 2)
                                {
                                    listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                                }
                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                            }


                        }
                    }
                }


            }

            




            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        private void Button5_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ReadText();
            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(ReadText));
            thread.Start();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
