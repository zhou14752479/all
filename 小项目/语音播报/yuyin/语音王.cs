using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace yuyin
{
    public partial class 语音王 : Form
    {
        public 语音王()
        {
            InitializeComponent();
        }
        string path = AppDomain.CurrentDomain.BaseDirectory + "data\\";
        private void 语音王_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
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
        /// <summary>
        /// 加载数据
        /// </summary>
        public void getLists()

        {
            DirectoryInfo folder = new DirectoryInfo(path);
            for (int i = 0; i < folder.GetFiles("*.txt").Count(); i++)
            {

                if (!folder.GetFiles("*.txt")[i].Name.Contains("body"))
                {

                    StreamReader sr = new StreamReader(path + folder.GetFiles("*.txt")[i].Name, Encoding.GetEncoding("utf-8"));
                    //一次性读取完 
                    string texts = sr.ReadToEnd();
                    listBox1.Items.Add(texts);
                    sr.Close();
                }


            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            getLists();
            label1.Text = DateTime.Now.ToString("T");
            label2.Text = DateTime.Now.ToString("D")+" "+ DateTime.Now.ToString("dddd");
        }

        private void 新增文稿ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            新增文稿 add = new 新增文稿();
            add.Show();
        }
    }
}
