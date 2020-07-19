using System;
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

namespace 主程序202007
{
    


    public partial class 文件筛选 : Form
    {
        class ListViewNF : System.Windows.Forms.ListView
        {
            public ListViewNF()
            {
                // 开启双缓冲
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

                // Enable the OnNotifyMessage event so we get a chance to filter out 
                // Windows messages before they get to the form's WndProc
                this.SetStyle(ControlStyles.EnableNotifyMessage, true);
            }

            protected override void OnNotifyMessage(Message m)
            {
                //Filter out the WM_ERASEBKGND message
                if (m.Msg != 0x14)
                {
                    base.OnNotifyMessage(m);
                }
            }
        }


        public 文件筛选()
        {
            InitializeComponent();
        }
        string path = "";
        public void getinfos()
        {
            DirectoryInfo folder = new DirectoryInfo(path);

            foreach (FileInfo file in folder.GetFiles())
            {
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                lv1.SubItems.Add(file.FullName);

            }
            MessageBox.Show("加载完成");
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (f.ShowDialog() == DialogResult.OK)
            {
                String DirPath = f.SelectedPath;
                this.textBox1.Text = DirPath;
                path = DirPath;
            }
            Thread thread = new Thread(new ThreadStart(getinfos));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;




        }

        public void deleteFile()
        {
            int count = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string filename = listView1.Items[i].SubItems[1].Text.Trim();
                string name = Path.GetFileNameWithoutExtension(filename);
                if (checkBox1.Checked==true)
                {
                    if (Path.GetExtension(filename) == ".txt")
                    {
                        File.Delete(filename);
                        count = count + 1;
                        textBox3.Text += "txt删除：" + name + "\r\n";
                        continue;
                    }

                }
                if (checkBox2.Checked == true)
                {
                    FileInfo  fileInfo = new System.IO.FileInfo(filename);
                    double kb = System.Math.Ceiling(fileInfo.Length / 1024.0);
                    if (kb < Convert.ToInt32(textBox2.Text))
                    {
                        File.Delete(filename);
                        count = count + 1;
                        textBox3.Text += "文件过小删除：" + name + "\r\n";
                        continue;

                    }
                }

                if (checkBox3.Checked == true)
                {
                   
                  
                    Match panduan = Regex.Match(name, @"^[0-9a-zA-Z]+$");
                   
                    if (panduan.Groups[0].Value != "")
                    {
                        File.Delete(filename);
                        count = count + 1;
                        textBox3.Text += "数字字母文件名删除：" + name + "\r\n";
                        continue;

                    }

                }

                string[] texts= textBox4.Text.Split(new string[] { "," }, StringSplitOptions.None);
                foreach (var text in texts)
                {
                    if (name.Contains(text))
                    {
                        File.Delete(filename);
                        count = count + 1;
                        textBox3.Text += "包含字符"+text+"删除：" + name + "\r\n";
                        break;

                    }
                }
               







            }
            textBox3.Text += "删除完毕，共删除文件数量：" + count + "\r\n";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(deleteFile));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

        }
    }
}
