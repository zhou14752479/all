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
using myDLL;

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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
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
        string path = AppDomain.CurrentDomain.BaseDirectory;


        ArrayList finishes = new ArrayList();
        public void download()
        {
            try
            {
                for (int i = 0; i < richTextBox1.Lines.Length; i++)
                {
                    //string filename = Path.GetFileName(richTextBox1.Lines[i]).Replace(" ","");
                    string filename = richTextBox2.Lines[i] + ".jpg";
                      downloadFile(richTextBox1.Lines[i], path + "image//", filename, "_ga=GA1.2.1596665716.1614240361; __ssds=2; __uzmaj2=fc75191e-2afd-4f10-ab0d-b4eb31f80a24; __uzmbj2=1614240381; _fbp=fb.1.1614646492681.1428262791; __ssuzjsr2=a9be0cd8e; __uzmcj2=7542920585999; __uzmdj2=1614758042");
                    textBox2.Text = "正在下载第：" + (i + 1)+"   "+ richTextBox2.Lines[i];


                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            //for (int i = 0; i < richTextBox1.Lines.Length; i++)
            //{
            //    string[] text = richTextBox1.Lines[i].Split(new string[] { " " }, StringSplitOptions.None);

            //    string newTxt = "";
            //    for (int j = 0; j < text.Length-1; j++)
            //    {
            //        newTxt += text[j] + " ";
            //    }

            //    richTextBox2.Text += newTxt + "\r\n";

            //    button1.Text = richTextBox2.Lines.Length.ToString();
            //}
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
