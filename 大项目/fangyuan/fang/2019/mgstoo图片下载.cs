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

namespace fang._2019
{
    public partial class mgstoo图片下载 : Form
    {
        public mgstoo图片下载()
        {
            InitializeComponent();
        }

        private void mgstoo图片下载_Load(object sender, EventArgs e)
        {

        }
        bool zanting = true;

        #region  主函数
        public void run()

        {

            try

            {

                for (int i = 1; i < 500; i++)
                {

                    
                    string URL = "https://www.mgstoo.com/articles/page/" + i + "/";

                    string html = method.GetUrl(URL, "utf-8");

                    MatchCollection urls = Regex.Matches(html, @"<article class=""home-blog-entry col span_1 clr"">([\s\S]*?)<a href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    ArrayList lists = new ArrayList();

                    foreach (Match NextMatch in urls)
                    {
                        lists.Add(NextMatch.Groups[2].Value);

                    }

                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;

                    
                    foreach (string url in lists)
                    {
                        string strhtml = method.GetUrl(url, "utf-8");

                        Match text = Regex.Match(strhtml, @"<div class=""single-text"">([\s\S]*?)</div>");


                        MatchCollection pathes = Regex.Matches(text.Groups[1].Value, @"<img([\s\S]*?)src=""([\s\S]*?)""");
                       
                        foreach (Match path in pathes)
                        {
                            if (path.Groups[2].Value.ToString().Contains("sinaimg"))
                            {
                                textBox1.Text = "正在下载" + url;

                                string filename = Path.GetFileName(path.Groups[2].Value);

                                string downUrl = path.Groups[2].Value;

                                method.downloadFile(downUrl, textBox2.Text , filename);

                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(downUrl);
                                lv1.SubItems.Add("下载完成");

                                if (listView1.Items.Count - 1 > 1)
                                {
                                    listView1.EnsureVisible(listView1.Items.Count - 1);
                                }
                                while (this.zanting == false)
                                {
                                    Application.DoEvents();
                                }
                            }
                            else
                            {
                                textBox1.Text = "正在下载" + url;
                                string downUrl = path.Groups[2].Value;
                                
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(downUrl);
                                lv1.SubItems.Add("不符合要去，跳过");

                                if (listView1.Items.Count - 1 > 1)
                                {
                                    listView1.EnsureVisible(listView1.Items.Count - 1);
                                }
                                while (this.zanting == false)
                                {
                                    Application.DoEvents();
                                }

                            }
                            
                        }

                    }

                }
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



        #endregion

        private void button2_Click(object sender, EventArgs e)
        {

            if (textBox2.Text == "")
            {
                MessageBox.Show("请选择保存地址!");
                return;
            }
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择保存所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }
                else
                {
                    textBox2.Text = dialog.SelectedPath;
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }
    }
}
