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

namespace fang._2019
{
    public partial class 圣才电子书 : Form
    {
        public 圣才电子书()
        {
            InitializeComponent();
        }
        
        #region  主函数
        public void run()

        {
            string[] urls = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            try

            {

                foreach (string url in urls)
                {
                    string html = method.GetUrl(url, "utf-8");
                    Match title = Regex.Match(html, @"<title>([\s\S]*?)</title>");


                    Match path = Regex.Match(html, @"DirPath="" \+ ""([\s\S]*?)""");

                    if (path.Groups[1].Value !="")
                    {
                        string downUrl = "http://e.100xuexi.com/uploads/ebook/" + path.Groups[1].Value + "/mobile/" + path.Groups[1].Value + ".epub";

                        textBox1.Text = downUrl;
                        method.downloadFile(downUrl, textBox2.Text, title.Groups[1].Value.Replace("_", "").Trim() + ".epub");

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(url);
                        lv1.SubItems.Add("下载完成");

                        if (listView1.Items.Count - 1 > 1)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);
                        }

                        Thread.Sleep(1000);

                    }

                    else
                    {

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(url);
                        lv1.SubItems.Add("无epub地址");
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

            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入下载地址！");
                return;
            }

            if (textBox2.Text=="")
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

        private void 圣才电子书_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }


    }
}
