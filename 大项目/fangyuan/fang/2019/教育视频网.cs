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
    public partial class 教育视频网 : Form
    {
        public 教育视频网()
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
                    Match title = Regex.Match(method.GetUrl(url, "utf-8"), @"<title>([\s\S]*?)_");


                    Match ids = Regex.Match(url, @"\d{4,}");
                    string downUrl = "http://e.100xuexi.com/DigitalLibrary/ajax.aspx?action=Download&id=" + ids.Groups[0].Value;

                    method.downloadFile(downUrl, textBox2.Text, title.Groups[1].Value.Trim() + ".exe");

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(url);
                    lv1.SubItems.Add("下载完成");

                    if (listView1.Items.Count - 1 > 1)
                    {
                        listView1.EnsureVisible(listView1.Items.Count - 1);
                    }
                    //如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。

                   
                    Thread.Sleep(1000);



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
            //if (textBox1.Text == "")
            //{
            //    MessageBox.Show("请输入下载地址！");
            //    return;
            //}

            if (textBox2.Text == "")
            {
                MessageBox.Show("请选择保存地址!");
                return;
            }
            //Thread thread = new Thread(new ThreadStart(run));
            //Control.CheckForIllegalCrossThreadCalls = false;
            //thread.Start();

            method.downloadFile("http://1s1k.eduyun.cn/resource/redesign/publishCase/vod_player.jsp?resourceCode=mda-jctxr50gb1i0n2ue&yearMark=5&divId=playercontainers1&wid=770&hei=530&date=1553827920563&sessionKey=zk0AHLHG18ma03Vsex0Y", textBox2.Text,   "1.mp4");
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
    }
}
