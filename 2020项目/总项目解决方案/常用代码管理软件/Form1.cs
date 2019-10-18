using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 常用代码管理软件
{
    public partial class Form1 : Form
    {
        string path = AppDomain.CurrentDomain.BaseDirectory + "data\\";
        public Form1()
        {
            InitializeComponent();
        }
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

                    StreamReader sr = new StreamReader(path + folder.GetFiles("*.txt")[i].Name, Encoding.Default);
                    //一次性读取完 
                    string texts = sr.ReadToEnd();
                    listBox1.Items.Add(texts);
                    sr.Close();
                }
                

            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            getLists();

        }

        private void 存储ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            add ad = new add();
            ad.Show();
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            getLists();
        }

        private void ListBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int index = listBox1.IndexFromPoint(e.X, e.Y);
            listBox1.SelectedIndex = index;
            if (listBox1.SelectedIndex != -1)
            {
                MessageBox.Show(listBox1.SelectedItem.ToString());
           
            }
        }
    }
}
