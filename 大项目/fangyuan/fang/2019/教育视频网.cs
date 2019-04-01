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
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(url);
                    lv1.SubItems.Add("下载中");


                    for (int i = 0; i < 9999; i++)
                    {

                        Match CaseId = Regex.Match(url, @"CaseId=([\s\S]*?)&");
                        Match sessionKey = Regex.Match(url, @"sessionKey=([\s\S]*?) ");
                        string Url = "http://1s1k.eduyun.cn/resource/resource/RedesignCaseView/viewCaseBbs1s1k.jspx?code=-1&sdResIdCaseId=" + CaseId.Groups[1].Value + "&flags=&guideId=&sk=&sessionKey=" + sessionKey.Groups[1].Value;

                        string html = method.GetUrl(Url, "utf-8");
                        Match docValue = Regex.Match(html, @"doc-([\s\S]*?)'");
                        Match videoValue = Regex.Match(html, @"onclick=""getvodStatus\('([\s\S]*?)'");
                        Match title = Regex.Match(html, @"<h1>([\s\S]*?)</h1>");
                        string vedioUrl = "http://hknm5s6gzvm5a6wju24.exp.bcevod.com/" + videoValue.Groups[1].Value + "/" + videoValue.Groups[1].Value + ".m3u8."+i+".ts";

                        textBox1.Text = vedioUrl;
                        method.downloadFile(vedioUrl, textBox2.Text, title.Groups[1].Value.Trim()+i + ".ts");

                       
            
                    }

                    ListViewItem lv = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv.SubItems.Add(url);
                    lv.SubItems.Add("下载完成");
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

            //if (textBox2.Text == "")
            //{
            //    MessageBox.Show("请选择保存地址!");
            //    return;
            //}
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

        private void 教育视频网_Load(object sender, EventArgs e)
        {

        }


    }
}
