using System;
using System.Collections;
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

namespace 文件搜索
{
    public partial class 文件搜索 : Form
    {
        public 文件搜索()
        {
            InitializeComponent();
        }

      

        //public ArrayList getFileName(string path)
        //{
        //    ArrayList lists = new ArrayList();

          
        //    DirectoryInfo folder = new DirectoryInfo(path);
        //    for (int i = 0; i < folder.GetFiles("*.tar").Count(); i++)
        //    {
        //        lists.Add(folder.GetFiles("*.tar")[i].FullName);
        //    }
        //    return lists;
        //}


        public void panduan(string filename)
        {
           
            if (textBox3.Text == "")
            {
                MessageBox.Show("请选择路径");
            }
            string sPath = textBox3.Text.Trim();

            string[] text =textBox4.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (var item in text)
            {
                try
                {
                    if (item != "")
                    {
                        if (Path.GetFileNameWithoutExtension(filename).Contains(item))
                        {
                            textBox2.Text += DateTime.Now.ToString("HH:MM:ss") + "正在剪切文件：" + Path.GetFileName(filename) + "\r\n";
                            if (!File.Exists(sPath + "\\" + Path.GetFileName(filename)))
                            {
                                File.Copy(filename, sPath + "\\" + Path.GetFileName(filename));
                                return;

                            }
                        }
                    }

                }
                catch (Exception ex)
                {

                    textBox2.Text = ex.ToString() + filename;
                }

            }

        }



        public void Director(string dir, List<string> list)
        {
            DirectoryInfo d = new DirectoryInfo(dir);
            FileInfo[] files = d.GetFiles("*.tar");//文件
            DirectoryInfo[] directs = d.GetDirectories();//文件夹
            foreach (FileInfo f in files)
            {
                list.Add(f.FullName);//添加文件名到列表中  
            }
            //获取子文件夹内的文件列表，递归遍历  
            foreach (DirectoryInfo dd in directs)
            {
                Director(dd.FullName, list);
            }
        }


        public void run()
        {
            try
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("请选择压缩包所在文件夹");
                    return;
                }
                List<string> lists = new List<string>();
                Director(textBox1.Text.Trim(),lists);

                foreach (string list in lists)
                {

                    panduan(list);

                }

                textBox2.Text += "【完成】";

            }
            catch (Exception ex)
            {

                textBox2.Text = ex.ToString();
            }




        }
        private void 文件搜索_Load(object sender, EventArgs e)
        {

        }
        Thread thread;
        private void button3_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
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

        private void button2_Click(object sender, EventArgs e)
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

                textBox3.Text = dialog.SelectedPath;
            }
        }
    }
}
