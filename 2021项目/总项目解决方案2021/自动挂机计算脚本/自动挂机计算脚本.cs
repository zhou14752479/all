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

namespace 自动挂机计算脚本
{
    public partial class 自动挂机计算脚本 : Form
    {
        public 自动挂机计算脚本()
        {
            InitializeComponent();
        }
        function fc = new function();
        public string importFolder = "";
        public string outputFolder = "";

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
                importFolder = dialog.SelectedPath;
                fc.getfiles(importFolder,listView3);
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
                outputFolder = dialog.SelectedPath;

            }
        }

       
     
        private void ListView3_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item.Checked == true)
            {
                e.Item.Selected = e.Item.Checked;
                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                lv1.SubItems.Add(e.Item.SubItems[1].Text);
                lv1.SubItems.Add("等待");
            }
        }

        private void 清空数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }
        Thread thread;
        bool zanting = true;
        bool status = true;
        string path = "";
        public string importtoSite(string filename)
        {
            StreamReader sr = new StreamReader(importFolder + filename+".txt", function.EncodingType.GetTxtType(importFolder + filename + ".txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {

            }


            return "导入成功";
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                if (listView1.CheckedItems[i].SubItems[2].Text == "等待" || listView1.CheckedItems[i].SubItems[2].Text == "失败")
                {
                    Thread t = new Thread(() =>
                    {
                        try
                        {
                            listView1.CheckedItems[i].SubItems[2].Text = "进行中";



                        }
                        catch (Exception ex)
                        {
                            listView1.CheckedItems[i].SubItems[2].Text = "失败";
                        }

                    });
                    t.Start();
                }
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            //window.open('http://www.sufeinet.com', 'newwindow', 'width=10,heigh=10,left=10,top=10')
        }
    }
}
