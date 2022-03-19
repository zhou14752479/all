using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202202
{
    public partial class 图片批量下载 : Form
    {
        public 图片批量下载()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }

                textBox1.Text = dialog.SelectedPath;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }

                textBox2.Text = dialog.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {

                MessageBox.Show("请选择文件夹");
                return;

            }
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
        Thread thread;
        public void run()
        {
            DirectoryInfo di = new DirectoryInfo(textBox1.Text);
            foreach (FileInfo fi in di.GetFiles("*.txt"))
            {
                textBox3.Text += "正在处理：" + Path.GetFileName(fi.FullName) + "\r\n";

                Thread t = new Thread(this.InvokeThread);
                t.Start(fi.FullName);
            }
           
        }
        private void InvokeThread(object filePath)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ReadFile(ReadFileContent), filePath);
            }
            else
            {
                ReadFileContent(filePath);
            }
        }
        #region 下载文件  【好用】
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="URLAddress">图片地址</param>
        /// <param name="subPath">图片所在文件夹</param>
        /// <param name="name">图片名称</param>
        public static void downloadFile(string URLAddress, string subPath, string name, string COOKIE)
        {
            try
            {
                string path = System.IO.Directory.GetCurrentDirectory();

                WebClient client = new WebClient();
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
                client.Headers.Add("Cookie", COOKIE);
                client.Headers.Add("Referer", "");
                if (false == System.IO.Directory.Exists(subPath))
                {
                    //创建pic文件夹
                    System.IO.Directory.CreateDirectory(subPath);
                }

                client.DownloadFile(URLAddress, subPath + "\\" + name);
            }
            catch (WebException ex)
            {

               MessageBox.Show(ex.ToString());
            }
        }



        #endregion
        #region 去掉路径中非法字符
        public string removeValid(string illegal)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                illegal = illegal.Replace(c.ToString(), "");
            }
            return illegal;
        }

        #endregion
        private delegate void ReadFile(object filePath);
        private void ReadFileContent(object filePath)
        {
           

            StreamReader sr = new StreamReader(filePath.ToString(), Encoding.GetEncoding("utf-8"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\n" }, StringSplitOptions.None);

            for (int i = 0; i < text.Length; i++)
            {
               if(text[i].Trim()!="")
                {
                    try
                    {
                        downloadFile(text[i].Trim(), textBox2.Text, Path.GetFileName(text[i].Trim()).Trim(), "");
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                   
                }

            }
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
          
        
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
        }
    }
}
