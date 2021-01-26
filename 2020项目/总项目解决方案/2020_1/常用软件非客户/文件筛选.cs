using Microsoft.VisualBasic.Devices;
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

namespace 常用软件非客户
{
    public partial class 文件筛选 : Form
    {
        public 文件筛选()
        {
            InitializeComponent();
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
            getinfos();
        }

        string path = "";
        public void getinfos()
        {
            DirectoryInfo folder = new DirectoryInfo(path);

            foreach (FileInfo file in folder.GetFiles())
            {
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                lv1.SubItems.Add(file.FullName);
                lv1.SubItems.Add(" ");
            }
            MessageBox.Show("加载完成");
        }

        public void deleteFile()
        {
            int count = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string filename = listView1.Items[i].SubItems[1].Text.Trim();
                string name = Path.GetFileNameWithoutExtension(filename);
                if (checkBox1.Checked == true)
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
                    FileInfo fileInfo = new System.IO.FileInfo(filename);
                    double kb = System.Math.Ceiling(fileInfo.Length / 1024.0);
                    if (kb < Convert.ToInt32(textBox2.Text))
                    {
                        File.Delete(filename);
                        count = count + 1;
                        // textBox3.Text += "文件过小删除：" + name + "\r\n";
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


                if (checkBox4.Checked == true)
                {
                    string extension = Path.GetExtension(filename);
                    if (extension == ".ppt" || extension == ".pptx" || extension == ".PPT" || extension == ".PPTX")
                    {
                        FileInfo fileInfo = new System.IO.FileInfo(filename);
                        double kb = System.Math.Ceiling(fileInfo.Length / 1024.0);
                        if (kb < Convert.ToInt32(textBox5.Text))
                        {
                            File.Delete(filename);
                            count = count + 1;
                            textBox3.Text += "PPT大小"+ kb + "kb删除：" + name + "\r\n";
                            continue;

                        }
                        
                    }

                }

                string[] texts = textBox4.Text.Split(new string[] { "," }, StringSplitOptions.None);
                foreach (var text in texts)
                {
                    if (name.Contains(text))
                    {

                        File.Delete(filename);
                        count = count + 1;
                        textBox3.Text += "包含字符" + text + "删除：" + name + "\r\n";
                        break;

                    }
                }








            }
            textBox3.Text += "删除完毕，共删除文件数量：" + count + "\r\n";

        }

     
        string addname = "教学文档";

        Computer MyComputer = new Computer();

        public void checkinfos()
        {
            DirectoryInfo folder = new DirectoryInfo(path);
            label3.Text = "共：" + folder.GetFiles().Length;
            for (int i = 0; i < listView1.Items.Count; i++)
            {

                try
                {
                    string filpath = listView1.Items[i].SubItems[1].Text;
                    string houzhui = Path.GetExtension(filpath);

                    string oldname = Path.GetFileNameWithoutExtension(filpath);
                    string newname = "";

                    int len = System.Text.Encoding.UTF8.GetBytes(oldname).Length;

                    if (len < 13)
                    {
                        newname = oldname + addname;
                        MyComputer.FileSystem.RenameFile(filpath, newname.Trim() + houzhui);
                        listView1.Items[i].SubItems[2].Text = newname.Trim();

                    }
                    if (len>80)
                    {
                        newname = oldname.Substring(0,20);
                        MyComputer.FileSystem.RenameFile(filpath, newname.Trim() + houzhui);
                        listView1.Items[i].SubItems[2].Text = newname.Trim();
                    }

                    if (oldname.Contains(" "))
                    {
                        newname = oldname.Replace(" ","");
                        MyComputer.FileSystem.RenameFile(filpath, newname.Trim() + houzhui);
                        listView1.Items[i].SubItems[2].Text = newname.Trim();
                    }
                    else
                    {
                        listView1.Items[i].SubItems[2].Text = "noneed";
                    }

                }
                catch (Exception ex)
                {
                    File.Delete(listView1.Items[i].SubItems[1].Text);
                    listView1.Items[i].SubItems[2].Text = "已删除";
                    listView1.Items[i].BackColor = Color.Red;
                    continue;
                }


            }
            MessageBox.Show("完成");
        }
        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(deleteFile);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
        Thread t;
        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            getinfos();
            if (t == null || !t.IsAlive)
            {
                t = new Thread(checkinfos);
                t.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 文件筛选_Load(object sender, EventArgs e)
        {

        }
    }
}
