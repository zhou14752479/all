using Microsoft.Office.Interop.Word;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 通用项目
{
    public partial class 文档页码读取 : Form
    {
        public 文档页码读取()
        {
            InitializeComponent();
        }
        ArrayList filesList = new ArrayList();
        public void getFiles()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请选择文件夹");
                return;
            }
            DirectoryInfo folder = new DirectoryInfo(textBox1.Text);

            foreach (FileInfo file in folder.GetFiles("*.doc"))
            {
                filesList.Add(file.FullName);
            }

            //foreach (FileInfo file in folder.GetFiles("*.ppt"))
            //{
            //   filesList.Add(file.FullName);
            //}
        }


        public int getpage(object file)
        {
            try
            {
                Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
                object nullobj = System.Reflection.Missing.Value;

                Microsoft.Office.Interop.Word.Document doc = app.Documents.Open(
                  ref file, ref nullobj, ref nullobj,
                  ref nullobj, ref nullobj, ref nullobj,
                  ref nullobj, ref nullobj, ref nullobj,
                  ref nullobj, ref nullobj, ref nullobj,
                  ref nullobj, ref nullobj, ref nullobj);
                doc.ActiveWindow.Selection.WholeStory();
                doc.ActiveWindow.Selection.Copy();
                IDataObject data = Clipboard.GetDataObject();

                // get number of pages
                Microsoft.Office.Interop.Word.WdStatistic stat = Microsoft.Office.Interop.Word.WdStatistic.wdStatisticPages;
                int pages = doc.ComputeStatistics(stat, Type.Missing);

                //string text = data.GetData(DataFormats.Text).ToString();  //获取具体内容，此项目不需要

                doc.Close(ref nullobj, ref nullobj, ref nullobj);
                app.Quit(ref nullobj, ref nullobj, ref nullobj);


                return pages;

            }
            catch (Exception ex)
            {
                ex.ToString();
                return 0;
               
            }
            
        }

        private void 文档页码读取_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text= dialog.SelectedPath;
            }
        }

        ArrayList finishes = new ArrayList();
        public void run()

        {
           
            label2.Text = "共识别到"+filesList.Count+"个文件";
           
            foreach (string filename in filesList)
            {
                try
                {

                    if (!finishes.Contains(filename))
                    {
                        finishes.Add(filename);
                        shengyu = shengyu - 1;
                        if (getpage(filename) <= Convert.ToInt16(textBox2.Text))
                        {
                            if (radioButton1.Checked == true)
                            {
                                File.Delete(filename);
                            }
                            else if (radioButton2.Checked == true)
                            {
                                FileInfo file = new FileInfo(filename);
                                if (textBox3.Text != "")
                                {
                                    file.MoveTo(textBox3.Text + "\\" + file.Name);
                                }
                                else
                                {
                                    MessageBox.Show("请选择移动至的文件夹");
                                    return;
                                }
                            }

                        }
                    }
                        label2.Text = "共识别到" + filesList.Count + "个文件，剩余" + shengyu;
                 }
                    
                catch 
                {

                    continue;
                }
               
            }

        }

        int shengyu = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            getFiles();
            shengyu = filesList.Count;
            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = dialog.SelectedPath;
            }
        }

        private void 文档页码读取_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
