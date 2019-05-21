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

namespace fang.临时软件
{
    public partial class 文件下载 : Form
    {
        public 文件下载()
        {
            InitializeComponent();
        }

        bool zanting = true;
        private void 文件下载_Load(object sender, EventArgs e)
        {

        }

        public string[] ReadText()
        {
            
            StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
            string text = streamReader.ReadToEnd();
            return text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        }
        ArrayList finishes = new ArrayList();
        public void run()

        {
            int a = 1;
            string[] urls = this.ReadText();
            string path = textBox2.Text;
            foreach (string url in urls)
            {

                    
               
                    //if (!Directory.Exists(path + id))
                    //{
                    //    Directory.CreateDirectory(path + id); //创建文件夹
                    //}
           
                        if (method.GetUrl(url, "utf-8") != "")   //判断请求图片的网址响应是否为空，如果为空表示没有图片，下载会报错！
                        {
                            method.downloadFile(url, path , "//" + a + ".xlsx");

                        }

                        label2.Text = url;
                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
                a++;
                }
            }



        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("请选择下载地址或选择下载保存地址");
                return;
            }


            #region 通用登录

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (System.Text.RegularExpressions.Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == localip.Trim())
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {

                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }
            else
            {
                MessageBox.Show("请登录您的账号！");
                System.Diagnostics.Process.Start("http://www.acaiji.com");
                return;
            }
            #endregion


        }

        private void Button2_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                textBox2.Text = foldPath;
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            zanting = true;
        }
    }

}
