using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 常用软件非客户
{
    public partial class 文件名大小判断 : Form
    {
        public 文件名大小判断()
        {
            InitializeComponent();
        }
        string path = "";
        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (f.ShowDialog() == DialogResult.OK)
            {
                String DirPath = f.SelectedPath;
                this.textBox3.Text = DirPath;
                path = DirPath;
            }
            ;
        }

        Computer MyComputer = new Computer();

        int chuli = 0;
        public void checkinfos()
        {
            DirectoryInfo folder = new DirectoryInfo(path);
            label3.Text = "共：" + folder.GetFiles().Length;
            foreach (FileInfo file in folder.GetFiles())
            {
                chuli = chuli + 1;
                label3.Text = "共：" + folder.GetFiles().Length+"已处理："+chuli;
                string oldname = Path.GetFileNameWithoutExtension(file.FullName);
                string newname = oldname + textBox2.Text.Trim();
                int len = System.Text.Encoding.UTF8.GetBytes(oldname).Length;
                string houzhui = Path.GetExtension(file.FullName);
               if (len < 13)
                {
                    try
                    {
                        MyComputer.FileSystem.RenameFile(file.FullName, newname + houzhui);
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(oldname);
                        lv1.SubItems.Add(newname);
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                   
                }

            }
            MessageBox.Show("完成");
        }

        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(checkinfos);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
