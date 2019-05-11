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

namespace main._2019_5
{
    public partial class 文本筛选 : Form
    {
        public 文本筛选()
        {
            InitializeComponent();
        }

        string strpath = "";
        string heipath = "";

        private void button3_Click(object sender, EventArgs e)
        {
            #region 通用登录


            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
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
        
            }
            else
            {
                MessageBox.Show("请登录您的账号！");
                System.Diagnostics.Process.Start("http://www.acaiji.com");
                return;
            }
            #endregion

        }



        public void run()
        {

            if (heipath == "")
            {
                MessageBox.Show("请添加过滤词");
                return;
            }

            if (strpath == "")
            {
                MessageBox.Show("请添加原文件");
                return;
            }

            string path = AppDomain.CurrentDomain.BaseDirectory;
            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            StreamReader sr = new StreamReader(strpath, Encoding.Default);
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);



            StreamReader sr1 = new StreamReader(heipath, Encoding.Default);
            //一次性读取完 
            string texts1 = sr1.ReadToEnd();
            string[] text1 = texts1.Split(new string[] { "\r\n" }, StringSplitOptions.None);


            ArrayList lists = new ArrayList();

            FileStream fs1 = new FileStream(path + "新文档.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1);
            for (int i = 0; i < text.Length; i++)
            {

                if (text[i].Length > Convert.ToInt32(textBox1.Text))
                {

                    lists.Add(text[i].Substring(0, (Convert.ToInt32(textBox1.Text))));
                }
            }

            for (int i = 0; i < lists.Count; i++)
            {

                for (int j = 0; j < text1.Length; j++)
                {
                    if (lists[i].ToString().Contains(text1[j]))
                    {
                        lists[i] = lists[i].ToString().Replace(text1[j], "").Trim();

                    }

                }
                sw.WriteLine(lists[i]);
            }




            sw.Close();
            fs1.Close();
            MessageBox.Show("生成成功");
        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                strpath = openFileDialog1.FileName;
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                heipath = openFileDialog1.FileName;
            }
        }
    }
}



