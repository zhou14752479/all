using Microsoft.VisualBasic.Devices;
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

namespace CsharpSelenium
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
            
        }


        string path = "";
        int count = 0;
        int nowcount = 0;

        ArrayList lists = new ArrayList();


        string addname = "教学文档";

        Computer MyComputer = new Computer();



        public void run()
        {
            DirectoryInfo folder = new DirectoryInfo(path);
            FileInfo[] infos = folder.GetFiles();
            int infocount = infos.Length;
            foreach (FileInfo file in infos)
            {
                ListViewItem lv1 = null;
                lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                try
                {
                    nowcount = nowcount + 1;
                    label4.Text = "共：" + infocount + "正在筛选：" + nowcount;

                    string filename = file.FullName;
                   
                    lv1.SubItems.Add(filename);
                    lv1.SubItems.Add("noneed");


                    string name = Path.GetFileNameWithoutExtension(filename);
                    string houzhui = Path.GetExtension(filename);

                    string oldname = Path.GetFileNameWithoutExtension(filename);
                    string newname = "";
                    string errochar = IsFileNameValid(oldname);
                    if (errochar != "")
                    {
                        File.Delete(filename);
                        lv1.SubItems.Add("文件名包含" + errochar);
                        continue;
                    }
                    int len = System.Text.Encoding.UTF8.GetBytes(oldname).Length;


                    if (checkBox1.Checked == true)
                    {
                        if (Path.GetExtension(filename) == ".txt")
                        {
                            File.Delete(filename);
                            count = count + 1;

                            lv1.SubItems.Add("txt删除");
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

                            lv1.SubItems.Add("文件过小" + kb);
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
                            lv1.SubItems.Add("数字或字母文件名");
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
                                lv1.SubItems.Add("PPT过小" + kb);
                                continue;

                            }

                        }

                    }

                    if (checkBox5.Checked == true)
                    {
                        int countA = 0;
                        foreach (char c in name)
                        {
                            countA = countA + 1;
                        }
                        if (countA > 49)
                        {
                            newname = oldname.Substring(0, 20);
                            MyComputer.FileSystem.RenameFile(filename, newname.Trim() + houzhui);
                            count = count + 1;
                            lv1.SubItems.Add("文件名过长" + countA);
                            continue;
                        }

                    }




                    string[] texts = textBox4.Text.Split(new string[] { "," }, StringSplitOptions.None);
                    foreach (var text in texts)
                    {
                        if (name.Contains(text))
                        {

                            File.Delete(filename);
                            count = count + 1;
                            lv1.SubItems.Add("包含字符" + text);
                            break;

                        }
                    }



                    if (len < 13)
                    {
                        newname = oldname + addname;
                        MyComputer.FileSystem.RenameFile(filename, newname.Trim() + houzhui);
                        lv1.SubItems.Add(newname);

                    }
                }
                catch (Exception ex)
                {
                    lv1.SubItems.Add(ex.ToString());
                    continue;
                }
              
               
            }
            label4.Text += "处理完毕，共处理文件数量：" + count + "\r\n";
        }




        private string IsFileNameValid(string name)
        {
           
            string[] errorStr = new string[] { "/", "\\", ":", ",", "*", "?", "\"", "<", ">", "|"};

        
                for (int i = 0; i < errorStr.Length; i++)
                {
                    if (name.Contains(errorStr[i]))
                    {

                        return errorStr[i];
                    }
                }
            return "";

        }


        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }

        private void 文件筛选_Load(object sender, EventArgs e)
        {

        }
    }
}
