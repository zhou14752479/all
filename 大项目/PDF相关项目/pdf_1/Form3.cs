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
using myDLL;

namespace pdf_1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] files = Directory.GetFiles(dialog.SelectedPath + @"\", "*.pdf");
                foreach (string file in files)
                {
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(file);
                    lv1.SubItems.Add("未开始");
                }
            }
        }

        public void run()
        {
            try
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    string pdfpath = listView1.Items[i].SubItems[1].Text;
                    string str = method.PDFtoStr(pdfpath);
                   
                    string lie25= Regex.Match(str, @"AD-[A-Z]{3}-\d{6}").Groups[0].Value;
                    string lie3 = Regex.Match(str, @"BP-\d{3}-\d{7}").Groups[0].Value;
                    string lie13 = Regex.Match(str, @"HASBRO \d{1,}").Groups[0].Value.Replace("HASBRO","").Trim();

                    MessageBox.Show(lie13);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;

                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        Thread thread;
        bool zanting = true;
        bool status = true;
        private void button6_Click(object sender, EventArgs e)
        {
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;

        private void Form3_Load(object sender, EventArgs e)
        {

            //DataTable dt1 = myDLL.method.ExcelToDataTable(path + "Sample.xlsx", true);
            //myDLL.method.ShowDataInListView(dt1, listView2);


        }
    }
}
