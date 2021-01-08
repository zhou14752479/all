using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 常用软件非客户
{
    public partial class 图片批量下载 : Form
    {
        public 图片批量下载()
        {
            InitializeComponent();
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
                client.Headers.Add("Referer", "https://m.mm131.net/chemo/89_5.html");
                if (false == System.IO.Directory.Exists(subPath))
                {
                    //创建pic文件夹
                    System.IO.Directory.CreateDirectory(subPath);
                }

                client.DownloadFile(URLAddress, subPath + "\\" + name);
            }
            catch (WebException ex)
            {

                ex.ToString();
            }
        }



        #endregion
        string path = AppDomain.CurrentDomain.BaseDirectory;


        ArrayList finishes = new ArrayList();
        public void download()
        {
            //try
            //{
            //    for (int i = 0; i < richTextBox1.Lines.Length; i++)
            //    {
            //        string filename = Path.GetFileName(richTextBox1.Lines[i]);
            //        downloadFile(richTextBox1.Lines[i], path + "image//", filename, "");
            //        textBox2.Text = "正在下载第：" + (i + 1);


            //    }
            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.ToString());
            //}

            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                string[] text = richTextBox1.Lines[i].Split(new string[] { " " }, StringSplitOptions.None);

                string newTxt = "";
                for (int j = 0; j < text.Length-1; j++)
                {
                    newTxt += text[j] + " ";
                }

                richTextBox2.Text += newTxt + "\r\n";

                button1.Text = richTextBox2.Lines.Length.ToString();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {

            Thread thread = new Thread(download);
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(path+"1.txt", method.EncodingType.GetTxtType(path + "1.txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            MatchCollection names = Regex.Matches(texts, @"\/category\/([\s\S]*?)\.");
            for (int i = 0; i < names.Count; i++)
            {
                textBox2.Text += names[i].Groups[1].Value+"\r\n";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (f.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo folder = new DirectoryInfo(f.SelectedPath);
                for (int i = 0; i < folder.GetFiles().Count(); i++)
                {
                    richTextBox1.Text += folder.GetFiles()[i].Name+"\r\n";
                }
            }

            
           
        }

        private void 图片批量下载_Load(object sender, EventArgs e)
        {

        }
    }
}
