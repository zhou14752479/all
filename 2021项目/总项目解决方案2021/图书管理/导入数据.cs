using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 图书管理
{
    public partial class 导入数据 : Form
    {
        public 导入数据()
        {
            InitializeComponent();
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
                DirectoryInfo d = new DirectoryInfo(dialog.SelectedPath);
                FileInfo[] files = d.GetFiles();//文件
                foreach (FileInfo f in files)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(f.FullName);
                    lv1.SubItems.Add("");
                    lv1.Checked = true;
                }

            }

        }

        

        function fc = new function();

        public void import()
        {
            try
            {
                for (int i = 0; i < listView1.CheckedItems.Count; i++)
                {

                    try
                    {

                        string filename = listView1.CheckedItems[i].SubItems[1].Text;
                        string result = fc.ExcelToDataTable(filename);
                        if (result.Contains("失败"))
                        {
                           

                            string newfilename = fc.xlstoxlsx(filename);
                            result = fc.ExcelToDataTable(newfilename);
                            if (result.Contains("成功"))
                            {
                                File.Delete(newfilename);
                            }

                            listView1.CheckedItems[i].SubItems[2].Text = result;


                        }
                        listView1.CheckedItems[i].SubItems[2].Text = result;



                    }
                    catch (Exception ex)
                    {
                        listView1.CheckedItems[i].SubItems[2].Text = "失败";
                        continue;

                    }


                }
                MessageBox.Show("导入完成");
            }
            catch (System.NullReferenceException ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {


            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(import);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
  


       

























    private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
             openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            //openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
               
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                lv1.SubItems.Add(openFileDialog1.FileName);
                lv1.SubItems.Add("");
                lv1.Checked = true;




            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            string sql = "delete from datas";
          bool status=  fc.SQL(sql) ;
            fc.SQL("VACUUM");
            fc.SQL("DELETE FROM sqlite_sequence WHERE name = 'datas';");
            if(status==true)
            {
                MessageBox.Show("清空成功");
            }
            
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            System.Diagnostics.Process.Start(listView1.SelectedItems[0].SubItems[1].Text);
           
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].SubItems[2].Text.Contains("失败") || listView1.Items[i].SubItems[2].Text=="")
                {
                    listView1.Items[i].Checked = true;
                }
                else
                {
                    listView1.Items[i].Checked =false;
                }
            }
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Checked = true;
            }
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Checked = false;
            }
        }

        private void 导入数据_Load(object sender, EventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException +=

             new UnhandledExceptionEventHandler(UnhandledExceptionEventHandler);
        }

        static void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs e)

        {
            

           //try
           // {
               
           //     using (System.IO.FileStream fs = new System.IO.FileStream(@"D:\testme.log",
           //              System.IO.FileMode.Append, System.IO.FileAccess.Write))

           //     {

           //         using (System.IO.StreamWriter w = new System.IO.StreamWriter(fs,
           //                  System.Text.Encoding.UTF8))
           //         {

           //             w.WriteLine(e.ExceptionObject);

           //         }

           //     }

           // }

           // catch

           // {

           // }

        }


    }
}
